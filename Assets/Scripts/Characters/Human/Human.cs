using ApplicationManagers;
using Cameras;
using Controllers;
using CustomLogic;
using CustomSkins;
using Effects;
using GameManagers;
using GameProgress;
using Map;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UI;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.XR;
using Utility;
using Weather;

namespace Characters
{
    class Human : BaseCharacter
    {//Zippy: add drop item on death (100% chance for gas, 50 for blade)
        // setup
        public HumanComponentCache HumanCache;
        public BaseUseable Special;
        public BaseUseable Special_2;
        public BaseUseable Special_3;
        public BaseUseable[] SpecialsArray; // added by Ata 12 May 2024 for Ability Wheel //
        public BaseUseable Weapon;
        public HookUseable HookLeft;
        public HookUseable HookRight;
        public HumanMountState MountState = HumanMountState.None;
        public HumanCarryState CarryState = HumanCarryState.None;
        public Horse Horse;
        public HumanSetup Setup;
        public bool FinishSetup;
        private HumanCustomSkinLoader _customSkinLoader;
        public override List<string> EmoteActions => new List<string>() { "Salute", "Wave", "Nod", "Shake", "Dance", "Eat", "Flip" };
        public static LayerMask AimMask = PhysicsLayer.GetMask(PhysicsLayer.TitanPushbox, PhysicsLayer.MapObjectProjectiles,
           PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectAll);
        public static LayerMask ClipMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectCharacters,
            PhysicsLayer.MapObjectEntities);

        // state
        private HumanState _state = HumanState.Idle;
        public float CurrentGas = -1f;
        public float MaxGas = -1f;
        public float GasUsage = 0.2f;
        public float HorseSpeed = 50f;
        public string CurrentSpecial;
        public string SideSpecial_1;
        public string SideSpecial_2;
        public BaseTitan Grabber;
        public Transform GrabHand;
        public Human Carrier;
        public Transform CarryBack;
        public Human BackHuman;
        public Vector3 CarryVelocity;
        public MapObject MountedMapObject;
        public Transform MountedTransform;
        public Vector3 MountedPositionOffset;
        public Vector3 MountedRotationOffset;
        public int AccelerationStat;
        public int RunSpeedStat;
        public bool CancelHookLeftKey;
        public bool CancelHookRightKey;
        public bool CancelHookBothKey;
        private object[] _lastMountMessage = null;

        // physics
        public float ReelInAxis = 0f;
        public float ReelOutAxis = 0f;
        public float ReelOutScrollTimeLeft = 0f;
        public float TargetMagnitude = 0f;
        public bool IsWalk;
        public const float RealismMaxReel = 120f;
        public const float RealismDeathVelocity = 100f;
        private const float MaxVelocityChange = 10f;
        public float RunSpeed;
        private float _originalDashSpeed;
        public Quaternion _targetRotation;
        private float _wallRunTime = 0f;
        private bool _wallJump = false;
        private bool _launchLeft;
        private bool _launchRight;
        private float _launchLeftTime;
        private float _launchRightTime;
        private bool _needLean;
        private bool _almostSingleHook;
        private bool _leanLeft;
        private bool _isTrigger;
        private Vector3 _lastPosition;
        private Vector3 _lastVelocity;
        private Vector3 _currentVelocity;

        public override LayerMask GroundMask => PhysicsLayer.GetMask(PhysicsLayer.TitanPushbox, PhysicsLayer.MapObjectEntities,
            PhysicsLayer.MapObjectAll);

        // actions
        public string StandAnimation;
        public string AttackAnimation;
        public bool _gunArmAim;
        public string RunAnimation;
        public bool _attackRelease;
        public bool _attackButtonRelease;
        public bool _reelInWaitForRelease;
        private float _stateTimeLeft = 0f;
        private float _dashTimeLeft = 0f;
        private bool _cancelGasDisable;
        private bool _animationStopped;
        private bool _needFinishReload;
        private float _reloadTimeLeft;
        private float _reloadCooldownLeft;
        private string _reloadAnimation;
        private float _dashCooldownLeft = 0f;
        private Human _hookHuman;
        private bool _hookHumanLeft;
        private float _hookHumanConstantTimeLeft;
        private bool _isReelingOut;
        private Dictionary<BaseTitan, float> _lastNapeHitTimes = new Dictionary<BaseTitan, float>();

        [SerializeField]
        private GameObject LogisticianBackPack;
        private ZippsUIManager _zippsUIManager; // added by Ata for setting up special icons in the ability wheel only once, should give optimal performance and memory usage //

        [PunRPC]
        public override void MarkDeadRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Dead = true;
            Setup.DeleteDie();
            GetComponent<CapsuleCollider>().enabled = false;
            if (IsMine())
            {
                FalseAttack();
                SetCarrierTriggerCollider(false);
            }
        }

        [PunRPC]
        public virtual void UngrabRPC(PhotonMessageInfo info)
        {
            if (Grabber == null || info.Sender != Grabber.Cache.PhotonView.Owner)
                return;
            Ungrab(false, true);
        }

        public override Vector3 GetAimPoint()
        {
            RaycastHit hit;
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin + ray.direction * 1000f;
            if (Physics.Raycast(ray, out hit, 1000f, AimMask.value))
                target = hit.point;
            return target;
        }

        public Vector3 GetAimPoint(Vector3 origin, Vector3 direction)
        {
            RaycastHit hit;
            Vector3 target = origin + direction * 1000f;
            if (Physics.Raycast(origin, direction, out hit, 1000f, AimMask.value))
                target = hit.point;
            return target;
        }

        public bool CanJump()
        {
            return (Grounded && CarryState != HumanCarryState.Carry && (State == HumanState.Idle || State == HumanState.Slide) &&
                !Cache.Animation.IsPlaying(HumanAnimations.Jump) && !Cache.Animation.IsPlaying(HumanAnimations.HorseMount));
        }

        public void Jump()
        {
            Idle();
            CrossFade(HumanAnimations.Jump, 0.1f);
            PlaySound(HumanSounds.Jump);
            ToggleSparks(false);
        }

        public void Mount(Transform transform, Vector3 positionOffset, Vector3 rotationOffset)
        {
            Transform parent = transform;
            MapObject mapObject = null;
            string transformName = "";
            while (parent != null)
            {
                if (MapLoader.GoToMapObject.ContainsKey(parent.gameObject))
                {
                    mapObject = MapLoader.GoToMapObject[parent.gameObject];
                    break;
                }
                if (transformName == "")
                    transformName = parent.name;
                else
                    transformName = parent.name + "/" + transformName;
                parent = parent.parent;
            }
            Mount(mapObject, transformName, positionOffset, rotationOffset);
        }

        public void Mount(MapObject mapObject, Vector3 positionOffset, Vector3 rotationOffset)
        {
            Mount(mapObject, "", positionOffset, rotationOffset);
        }

        public void Mount(MapObject mapObject, string transformName, Vector3 positionOffset, Vector3 rotationOffset)
        {
            if (MountedTransform != transform)
            {
                Unmount(true);
                SetInterpolation(false);
                SetTriggerCollider(true);
            }
            int scriptId = -100;
            if (mapObject != null)
                scriptId = mapObject.ScriptObject.Id;
            _lastMountMessage = new object[] { scriptId, transformName, positionOffset, rotationOffset };
            Cache.PhotonView.RPC("MountRPC", RpcTarget.All, _lastMountMessage);
        }

        [PunRPC]
        public void MountRPC(int mapObjectID, string transformName, Vector3 positionOffset, Vector3 rotationOffset, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            MountState = HumanMountState.MapObject;
            MountedMapObject = null;
            MountedTransform = null;
            if (_inGameManager.IsFinishedLoading())
                FinishMount(mapObjectID, transformName, positionOffset, rotationOffset);
            else
                StartCoroutine(WaitAndMount(mapObjectID, transformName, positionOffset, rotationOffset));
        }

        private IEnumerator WaitAndMount(int mapObjectID, string transformName, Vector3 positionOffset, Vector3 rotationOffset)
        {
            while (!_inGameManager.IsFinishedLoading())
                yield return null;
            if (MountState == HumanMountState.MapObject)
                FinishMount(mapObjectID, transformName, positionOffset, rotationOffset);
        }

        private void FinishMount(int mapObjectID, string transformName, Vector3 positionOffset, Vector3 rotationOffset)
        {
            if (mapObjectID == -100)
                return;
            if (MapLoader.IdToMapObject.ContainsKey(mapObjectID))
            {
                var mapObject = MapLoader.IdToMapObject[mapObjectID];
                Transform transform;
                if (transformName != "")
                    transform = mapObject.GameObject.transform.Find(transformName);
                else
                    transform = mapObject.GameObject.transform;
                if (transform != null)
                {
                    MountedMapObject = mapObject;
                    MountedTransform = transform;
                    MountedPositionOffset = positionOffset;
                    MountedRotationOffset = rotationOffset;
                }
            }
        }

        public void Unmount(bool immediate)
        {
            SetInterpolation(true); 
            if (MountState == HumanMountState.Horse && !immediate)
            {
                PlayAnimation(HumanAnimations.HorseDismount);
                Cache.Rigidbody.AddForce((((Vector3.up * 10f) - (Cache.Transform.forward * 2f)) - (Cache.Transform.right * 1f)), ForceMode.VelocityChange);
                MountState = HumanMountState.None;
            }
            else
            {
                MountState = HumanMountState.None;
                Idle();
                SetTriggerCollider(false);
            }
            _lastMountMessage = null;
            Cache.PhotonView.RPC("UnmountRPC", RpcTarget.All, new object[0]);
        }

        [PunRPC]
        public void UnmountRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            MountState = HumanMountState.None;
            MountedTransform = null;
            MountedMapObject = null;
        }

        public void MountHorse()
        {
            if (Horse != null && MountState == HumanMountState.None && Vector3.Distance(Horse.Cache.Transform.position, Cache.Transform.position) < 15f)
            {
                PlayAnimation(HumanAnimations.HorseMount);
                TargetAngle = Horse.transform.rotation.eulerAngles.y;
                PlaySound(HumanSounds.Dodge);
            }
        }

        public void Dodge(float targetAngle)
        {
            State = HumanState.GroundDodge;
            TargetAngle = targetAngle;
            _targetRotation = GetTargetRotation();
            CrossFade(HumanAnimations.Dodge, 0.1f);
            PlaySound(HumanSounds.Dodge);
            ToggleSparks(false);
        }

        public void DodgeWall()
        {
            State = HumanState.GroundDodge;
            PlayAnimation(HumanAnimations.Dodge, 0.2f);
            ToggleSparks(false);
        }

        public void Dash(float targetAngle)
        {
            if (_dashTimeLeft <= 0f && CurrentGas > 0 && MountState == HumanMountState.None &&
                State != HumanState.Grab && CarryState != HumanCarryState.Carry && _dashCooldownLeft <= 0f)
            {
                UseGas(Mathf.Min(MaxGas * 0.04f, 10));
                TargetAngle = targetAngle;
                Vector3 direction = GetTargetDirection();
                _originalDashSpeed = Cache.Rigidbody.velocity.magnitude;
                _targetRotation = GetTargetRotation();
                Cache.Rigidbody.rotation = _targetRotation;
                EffectSpawner.Spawn(EffectPrefabs.GasBurst, Cache.Transform.position, Cache.Transform.rotation);
                PlaySound(HumanSounds.GasBurst);
                _dashTimeLeft = 0.5f;
                CrossFade(HumanAnimations.Dash, 0.1f, 0.1f);
                State = HumanState.AirDodge;
                FalseAttack();
                Cache.Rigidbody.AddForce(direction * 40f, ForceMode.VelocityChange);
                _dashCooldownLeft = 0.2f;
                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShakeGas();
            }
        }

        #region Upward Dash by Ata - 2 May 24

        public void DashUpwards()
        {
            if (_dashTimeLeft <= 0f && CurrentGas > 0 && MountState == HumanMountState.None && 
                State != HumanState.Grab && CarryState != HumanCarryState.Carry && _dashCooldownLeft <= 0f)
            {
                UseGas(Mathf.Min(MaxGas * 0.06f, 10));
                EffectSpawner.Spawn(EffectPrefabs.GasBurst, Cache.Transform.position, Cache.Transform.rotation);
                PlaySound(HumanSounds.GasBurst);
                _originalDashSpeed = Cache.Rigidbody.velocity.magnitude;
                _dashTimeLeft = 0.5f;
                CrossFade(HumanAnimations.Dash, 0.1f, 0.1f);
                State = HumanState.AirDodge;
                FalseAttack();
                Cache.Rigidbody.AddForce(Vector3.up * 70f, ForceMode.VelocityChange);
                _dashCooldownLeft = 0.5f;
                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShakeGas();
            }
        }

        #endregion

        #region Downward Dash by Ata - 21 May 24

        public void DashDownwards()
        {
            if (_dashTimeLeft <= 0f && CurrentGas > 0 && MountState == HumanMountState.None &&
                State != HumanState.Grab && CarryState != HumanCarryState.Carry && _dashCooldownLeft <= 0f)
            {
                UseGas(Mathf.Min(MaxGas * 0.04f, 10));
                EffectSpawner.Spawn(EffectPrefabs.GasBurst, Cache.Transform.position, Cache.Transform.rotation);
                PlaySound(HumanSounds.GasBurst);
                _originalDashSpeed = Cache.Rigidbody.velocity.magnitude;
                _dashTimeLeft = 0.5f;
                CrossFade(HumanAnimations.Dash, 0.1f, 0.1f);
                State = HumanState.AirDodge;
                FalseAttack();
                Cache.Rigidbody.AddForce(Vector3.down * 40f, ForceMode.VelocityChange);
                _dashCooldownLeft = 0.5f;
                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShakeGas();
            }
        }

        #endregion

        public void Idle()
        {
            if (State == HumanState.Attack || State == HumanState.SpecialAttack)
                FalseAttack();
            State = HumanState.Idle;
            if (MountState == HumanMountState.Horse) // added by ata 2 May 2024 for a potential sword slash while mounted in the future //
            {
                // CrossFade an animation here after actually fixing the state issues //
                return;
            }
            else if (MountState == HumanMountState.None)
            {
                CrossFade(StandAnimation, 0.1f);
            }
        }

        public void Grab(BaseTitan grabber, Transform hand)
        {
            if (MountState != HumanMountState.None)
                Unmount(true);
            HookLeft.DisableAnyHook();
            HookRight.DisableAnyHook();
            UnhookHuman(true);
            UnhookHuman(false);
            State = HumanState.Grab;
            grabber.Cache.PhotonView.RPC("GrabRPC", grabber.Cache.PhotonView.Owner, new object[] { Cache.PhotonView.ViewID });
            SetTriggerCollider(true);
            FalseAttack();
            Grabber = grabber;
            GrabHand = hand;
            Cache.PhotonView.RPC("SetSmokeRPC", RpcTarget.All, new object[] { false });
            PlayAnimation(HumanAnimations.Grabbed);
            ToggleSparks(false);
            if (IsMainCharacter())
                MusicManager.PlayDeathSong();
        }

        public void Ungrab(bool notifyTitan, bool idle)
        {
            if (notifyTitan && Grabber != null)
                Grabber.Cache.PhotonView.RPC("UngrabRPC", Grabber.Cache.PhotonView.Owner, new object[0]);
            Grabber = null;
            SetTriggerCollider(false);
            if (idle)
                Idle();
        }

        public void Carry(Human carrier, Transform back)
        {
            if (MountState != HumanMountState.None)
                Unmount(true);
            HookLeft.DisableAnyHook();
            HookRight.DisableAnyHook();
            UnhookHuman(true);
            UnhookHuman(false);
            CarryState = HumanCarryState.Carry;
            SetTriggerCollider(true);
            FalseAttack();
            Carrier = carrier;
            CarryBack = back;
            SetCarrierTriggerCollider(true);
            Cache.PhotonView.RPC("SetSmokeRPC", RpcTarget.All, new object[] { false });
            ToggleSparks(false);
            State = HumanState.Idle;
            CrossFade(StandAnimation, 0.01f);
        }

        [PunRPC]
        public void CarryRPC(int viewId, PhotonMessageInfo info)
        {
            var view = PhotonView.Find(viewId);
            if (view.Owner != info.Sender)
                return;
            Carrier = view.GetComponent<Human>();
            Carrier.BackHuman = this;
            CarryState = HumanCarryState.Carry;
            if (Cache.PhotonView.IsMine)
                Carry(Carrier, Carrier.Cache.Transform);
        }

        public void Uncarry()
        {
            SetCarrierTriggerCollider(false);
            SetTriggerCollider(false);
            SetVelocityFromCarrier();
            Cache.Rigidbody.AddForce((((Vector3.up * 10f) - (Cache.Transform.forward * 2f)) - (Cache.Transform.right * 1f)), ForceMode.VelocityChange);
        }

        [PunRPC]
        public virtual void UncarryRPC(PhotonMessageInfo info)
        {
            CarryState = HumanCarryState.None;
            if (Cache.PhotonView.IsMine)
                Uncarry();
            if (Carrier != null)
                Carrier.BackHuman = null;
            Carrier = null;
        }

        public void SetCarrierTriggerCollider(bool trigger)
        {
            if (Carrier != null)
                Carrier.Cache.Colliders[0].isTrigger = trigger;
        }

        public void SetVelocityFromCarrier()
        {
            if (Carrier != null)
                Cache.Rigidbody.velocity = Carrier.CarryVelocity;
        }

        public void StartSpecialCarry()
        {
            Human human = FindNearestHuman();
            if (BackHuman != null)
            {
                BackHuman.Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
            }
            else if (human != null && human.CarryState == HumanCarryState.None && human.Carrier == null && human.BackHuman == null
                && Vector3.Distance(human.Cache.Transform.position, Cache.Transform.position) < 7f)
            {
                human.Cache.PhotonView.RPC("CarryRPC", RpcTarget.All, new object[] { Cache.PhotonView.ViewID });
            }
        }

        public void SpecialActionState(float time)
        {
            State = HumanState.SpecialAction;
            _stateTimeLeft = time;
        }

        public void TransformShifter(string shifter, float liveTime)
        {
            _inGameManager.SpawnPlayerShifterAt(shifter, liveTime, Cache.Transform.position, Cache.Transform.rotation.eulerAngles.y);
            ((BaseShifter)_inGameManager.CurrentCharacter).PreviousHumanGas = CurrentGas;
            ((BaseShifter)_inGameManager.CurrentCharacter).PreviousHumanWeapon = Weapon;
            PhotonNetwork.Destroy(gameObject);
        }

        public IEnumerator WaitAndTransformFromShifter(float previousHumanGas, BaseUseable previousHumanWeapon)
        {
            while (!FinishSetup)
            {
                yield return null;
            }
            CurrentGas = previousHumanGas;
            if (previousHumanWeapon is BladeWeapon)
            {
                BladeWeapon previousBlade = (BladeWeapon)previousHumanWeapon;
                BladeWeapon weapon = (BladeWeapon)Weapon;
                weapon.BladesLeft = previousBlade.BladesLeft;
                weapon.CurrentDurability = previousBlade.CurrentDurability;
                if (weapon.CurrentDurability == 0)
                {
                    ToggleBlades(false);
                }
            }
            else if (previousHumanWeapon is AmmoWeapon)
            {
                AmmoWeapon previousAmmoWeapon = (AmmoWeapon)previousHumanWeapon;
                AmmoWeapon weapon = (AmmoWeapon)Weapon;
                weapon.RoundLeft = previousAmmoWeapon.RoundLeft;
                weapon.AmmoLeft = previousAmmoWeapon.AmmoLeft;
                if (weapon.RoundLeft == 0 && Weapon is ThunderspearWeapon)
                    SetThunderspears(false, false);
            }
        }

        public void Reload()
        {
            if ((Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG) && !SettingsManager.InGameCurrent.Misc.GunsAirReload.Value && !Grounded)
                return;
            if (_needFinishReload || _reloadCooldownLeft > 0f)
                return;
            if (Weapon is AmmoWeapon)
            {
                if (((AmmoWeapon)Weapon).AmmoLeft <= 0)
                    return;
                if (Weapon is AHSSWeapon || Weapon is APGWeapon)
                {
                    ToggleBlades(false);
                    if (Weapon is AHSSWeapon)
                    {
                        CancelHookLeftKey = true;
                        CancelHookRightKey = true;
                        CancelHookBothKey = true;
                    }
                }
                else if (Weapon is ThunderspearWeapon)
                {
                    SetThunderspears(false, false);
                    CancelHookLeftKey = true;
                    CancelHookRightKey = true;
                    CancelHookBothKey = true;
                }
                PlaySound(HumanSounds.GunReload);
            }
            else if (Weapon is BladeWeapon)
            {
                if (((BladeWeapon)Weapon).BladesLeft <= 0)
                    return;
                ToggleBlades(false);
                if (Grounded)
                    PlaySound(HumanSounds.BladeReloadGround);
                else
                    PlaySound(HumanSounds.BladeReloadAir);
            }
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.Thunderspear || Setup.Weapon == HumanWeapon.APG)
            {
                if (Grounded)
                    _reloadAnimation = HumanAnimations.AHSSGunReloadBoth;
                else
                    _reloadAnimation = HumanAnimations.AHSSGunReloadBothAir;
            }
            else
            {
                if (Grounded)
                    _reloadAnimation = HumanAnimations.ChangeBlade;
                else
                    _reloadAnimation = HumanAnimations.ChangeBladeAir;
            }
            CrossFade(_reloadAnimation, 0.1f, 0f);
            State = HumanState.Reload;
            _stateTimeLeft = Cache.Animation[_reloadAnimation].length / Cache.Animation[_reloadAnimation].speed;
            _needFinishReload = true;
            _reloadTimeLeft = _stateTimeLeft;
            _reloadCooldownLeft = _reloadTimeLeft + 0.5f;
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.Reload();
        }

        protected void FinishReload()
        {
            if (!_needFinishReload)
                return;
            _needFinishReload = false;
            Weapon.Reload();
            if (Weapon is BladeWeapon || Weapon is AHSSWeapon || Weapon is APGWeapon)
            {
                ToggleBlades(true);
            }
            else if (Weapon is ThunderspearWeapon)
                SetThunderspears(true, true);
        }

        public bool Refill()
        {
            if (!Grounded || State != HumanState.Idle)
                return false;
            State = HumanState.Refill;
            if(Special is SupplySpecial && Special.UsesLeft == 0 )
            { 
                Special.Reset();
            }
            ToggleSparks(false);
            CrossFade(HumanAnimations.Refill, 0.1f);
            PlaySound(HumanSounds.Refill);
            _stateTimeLeft = Cache.Animation[HumanAnimations.Refill].length;
            return true;
        }

        public bool NeedRefill()
        {
            if (CurrentGas < MaxGas)
                return true;
            if (Special is SupplySpecial && Special.UsesLeft == 0)
            {
                return true;
            }
            if (Weapon is BladeWeapon)
            {
                var weapon = (BladeWeapon)Weapon;
                return weapon.BladesLeft < weapon.MaxBlades || weapon.CurrentDurability < weapon.MaxDurability;
            }
            else if (Weapon is AmmoWeapon)
            {
                var weapon = (AmmoWeapon)Weapon;
                return weapon.NeedRefill();
            }
            return false;
        }

        public void FinishRefill()
        {
            if (Weapon == null || Dead)
                return;
            if (Weapon is BladeWeapon)
            {
                ToggleBlades(true);
            }
            Weapon.Reset();
            CurrentGas = MaxGas;
            EmVariables.LogisticianBladeSupply = 4;
            EmVariables.LogisticianGasSupply = 4;
        }

        public override void Emote(string emote)
        {
            if (CanEmote())
            {
                if (State == HumanState.Attack)
                    FalseAttack();
                string animation = HumanAnimations.EmoteSalute;
                if (emote == "Salute")
                    animation = HumanAnimations.EmoteSalute;
                else if (emote == "Dance")
                    animation = HumanAnimations.SpecialArmin;
                else if (emote == "Flip")
                    animation = HumanAnimations.Dodge;
                else if (emote == "Wave")
                    animation = HumanAnimations.EmoteWave;
                else if (emote == "Nod")
                    animation = HumanAnimations.EmoteYes;
                else if (emote == "Shake")
                    animation = HumanAnimations.EmoteNo;
                else if (emote == "Eat")
                    animation = HumanAnimations.SpecialSasha;
                EmoteAnimation(animation);
                ToggleSparks(false);
            }
        }

        public void EmoteAnimation(string animation)
        {
            State = HumanState.EmoteAction;
            CrossFade(animation, 0.1f);
            _stateTimeLeft = Cache.Animation[animation].length / Cache.Animation[animation].speed;
            ToggleSparks(false);
        }

        public bool CanEmote()
        {
            return !Dead && State != HumanState.Grab && CarryState != HumanCarryState.Carry && State != HumanState.AirDodge && State != HumanState.EmoteAction && State != HumanState.SpecialAttack && MountState == HumanMountState.None
                && State != HumanState.Stun;
        }

        public override Transform GetCameraAnchor()
        {
            return HumanCache.Head;
        }

        protected override void CreateCache(BaseComponentCache cache)
        {
            HumanCache = new HumanComponentCache(gameObject);
            base.CreateCache(HumanCache);
        }

        protected override IEnumerator WaitAndDie()
        {
            if (State == HumanState.Grab)
                PlaySound(HumanSounds.Death5);
            else if (Grounded)
                PlaySound(HumanSounds.Death2);
            else
                PlaySound(HumanSounds.Death2);
            EffectSpawner.Spawn(EffectPrefabs.Blood2, Cache.Transform.position, Cache.Transform.rotation);
            yield return new WaitForSeconds(2f);
            PhotonNetwork.Destroy(gameObject);
        }

        public void Init(bool ai, string team, InGameCharacterSettings settings)
        {
            base.Init(ai, team);
            Setup.Copy(settings);
            if (!ai)
                gameObject.AddComponent<HumanPlayerController>();
        }

        protected override void Awake()
        {
            if (SceneLoader.SceneName == SceneName.CharacterEditor)
            {
                this.enabled = false;
                return;
            }
            base.Awake();
            HumanCache = (HumanComponentCache)Cache;
            Cache.Rigidbody.freezeRotation = true;
            Cache.Rigidbody.useGravity = false;
            if (gameObject.GetComponent<HumanSetup>() == null)
                Setup = gameObject.AddComponent<HumanSetup>();
            Setup = gameObject.GetComponent<HumanSetup>();
            _customSkinLoader = gameObject.AddComponent<HumanCustomSkinLoader>();
        }

        protected override void Start()
        {
            _inGameManager.Humans.Add(this);
            base.Start();
            SetInterpolation(true);
            if (IsMine())
            {
                TargetAngle = Cache.Transform.eulerAngles.y;
                Cache.PhotonView.RPC("SetupRPC", RpcTarget.AllBuffered, Setup.CustomSet.SerializeToJsonString(), (int)Setup.Weapon);
                LoadSkin();
                if (SettingsManager.InGameCurrent.Misc.Horses.Value)
                {
                    Horse = (Horse)CharacterSpawner.Spawn(CharacterPrefabs.Horse, Cache.Transform.position + Vector3.right * 2f, Quaternion.Euler(0f, TargetAngle, 0f));
                    Horse.Init(this);
                }
                if (DebugTesting.DebugPhase)
                    GetComponent<CapsuleCollider>().isTrigger = true;
            }
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            base.OnPlayerEnteredRoom(player);
            if (IsMine())
            {
                Cache.PhotonView.RPC("SetTriggerColliderRPC", player, new object[] { _isTrigger });
                if (MountState == HumanMountState.MapObject && _lastMountMessage != null)
                    Cache.PhotonView.RPC("MountRPC", player, _lastMountMessage);
                if (BackHuman != null && BackHuman.CarryState == HumanCarryState.Carry)
                    BackHuman.Cache.PhotonView.RPC("CarryRPC", player, new object[] { Cache.PhotonView.ViewID });
            }
        }

        [PunRPC]
        public override void GetHitRPC(int viewId, string name, int damage, string type, string collider)
        {
            if (Dead)
                return;
            if (type == "TitanEat")
            {
                base.GetHitRPC(viewId, name, damage, type, collider);
                if (!Dead)
                    Ungrab(false, true);
            }
            else if (type.StartsWith("Grab"))
            {
                if (State == HumanState.Grab)
                    return;
                var titan = (BaseTitan)Util.FindCharacterByViewId(viewId);
                if (type == "GrabLeft")
                    Grab(titan, titan.BaseTitanCache.GrabLSocket);
                else
                    Grab(titan, titan.BaseTitanCache.GrabRSocket);
            }
            else
                base.GetHitRPC(viewId, name, damage, type, collider);
        }

        public override void OnHit(BaseHitbox hitbox, object victim, Collider collider, string type, bool firstHit)
        {
            if (hitbox != null)
            {
                if (hitbox == HumanCache.BladeHitLeft || hitbox == HumanCache.BladeHitRight)
                    type = "Blade";
                else if (hitbox == HumanCache.AHSSHit)
                    type = "AHSS";
                else if (hitbox == HumanCache.APGHit)
                    type = "APG";
            }
            int damage = (CarryState == HumanCarryState.Carry && Carrier != null)
                ? Mathf.Max((int)(Carrier.CarryVelocity.magnitude * 10f), 10)
                : Mathf.Max((int)(Cache.Rigidbody.velocity.magnitude * 10f), 10);
            if (type == "Blade")
            {
                EffectSpawner.Spawn(EffectPrefabs.Blood1, hitbox.transform.position, Quaternion.Euler(270f, 0f, 0f));
                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                    PlaySound(HumanSounds.OldBladeHit);
                else
                    PlaySound(HumanSounds.BladeHit);
                var weapon = (BladeWeapon)Weapon;
                weapon.UseDurability(2f);
                if (weapon.CurrentDurability == 0f)
                {
                    ToggleBlades(false);
                    PlaySound(HumanSounds.BladeBreak);
                }
                damage = (int)(damage * CharacterData.HumanWeaponInfo["Blade"]["DamageMultiplier"].AsFloat);
            }
            else if (type == "AHSS")
                damage = (int)(damage * CharacterData.HumanWeaponInfo["AHSS"]["DamageMultiplier"].AsFloat);
            else if (type == "APG")
                damage = (int)(damage * CharacterData.HumanWeaponInfo["APG"]["DamageMultiplier"].AsFloat);
            if (CustomDamageEnabled)
                damage = CustomDamage;
            if (victim is CustomLogicCollisionHandler)
            {
                (victim as CustomLogicCollisionHandler).GetHit(this, Name, damage, type);
                return;
            }
            var victimChar = (BaseCharacter)victim;
            if (!victimChar.Dead)
            {
                if (victimChar is BaseTitan)
                {
                    var titan = (BaseTitan)victimChar;
                    if (titan.BaseTitanCache.NapeHurtbox == collider)
                    {
                        if (type == "Blade" && !CheckTitanNapeAngle(hitbox.transform.position, titan.BaseTitanCache.Head.transform,
                            CharacterData.HumanWeaponInfo["Blade"]["RestrictAngle"].AsFloat))
                            return;
                        if (type == "AHSS" && !CheckTitanNapeAngle(hitbox.transform.position, titan.BaseTitanCache.Head.transform,
                            CharacterData.HumanWeaponInfo["AHSS"]["RestrictAngle"].AsFloat))
                            return;
                        if (type == "APG" && !CheckTitanNapeAngle(hitbox.transform.position, titan.BaseTitanCache.Head.transform,
                            CharacterData.HumanWeaponInfo["APG"]["RestrictAngle"].AsFloat))
                            return;
                        if (type != "APG" && _lastNapeHitTimes.ContainsKey(titan) && (_lastNapeHitTimes[titan] + 0.2f) > Time.time)
                            return;
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(titan.BaseTitanCache.Neck.position, damage);
                        if (type == "Blade" && SettingsManager.GraphicsSettings.BloodSplatterEnabled.Value)
                            ((InGameMenu)UIManager.CurrentMenu).ShowBlood();
                        if (type == "Blade" || type == "AHSS" || type == "APG")
                        {
                            if (SettingsManager.SoundSettings.OldNapeEffect.Value)
                                PlaySound(HumanSounds.OldNapeHit);
                            else
                                PlaySound(HumanSounds.NapeHit);
                        }
                        _lastNapeHitTimes[titan] = Time.time;
                    }
                    if (titan.BaseTitanCache.Hurtboxes.Contains(collider))
                    {
                        EffectSpawner.Spawn(EffectPrefabs.CriticalHit, hitbox.transform.position, Quaternion.Euler(270f, 0f, 0f));
                        victimChar.GetHit(this, damage, type, collider.name);
                        if (titan.BaseTitanCache.NapeHurtbox != collider)
                            PlaySound(HumanSounds.LimbHit);
                    }
                }
                else
                {
                    ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                    ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(victimChar.Cache.Transform.position, damage);
                    victimChar.GetHit(this, damage, type, collider.name);
                }
            }
        }

        bool CheckTitanNapeAngle(Vector3 position, Transform nape, float angle)
        {
            Vector3 direction = (position - nape.position).normalized;
            return Vector3.Angle(-nape.forward, direction) < angle;
        }

        protected void Update()
        {
            if (IsMine() && !Dead)
            {
                _stateTimeLeft -= Time.deltaTime;
                _dashCooldownLeft -= Time.deltaTime;
                _reloadCooldownLeft -= Time.deltaTime;
                if (_needFinishReload)
                {
                    _reloadTimeLeft -= Time.deltaTime;
                    if (Weapon is BladeWeapon)
                    {
                        if (Grounded && (Cache.Animation[_reloadAnimation].normalizedTime > 0.5f || _reloadTimeLeft <= 0f))
                            FinishReload();
                        else if (!Grounded && (Cache.Animation[_reloadAnimation].normalizedTime > 0.56f || _reloadTimeLeft <= 0f))
                            FinishReload();
                    }
                    else
                    {
                        if (Cache.Animation[_reloadAnimation].normalizedTime > 0.62f || _reloadTimeLeft <= 0f)
                            FinishReload();
                    }
                }
                if (State == HumanState.Grab)
                {
                    if (Grabber == null || Grabber.Dead)
                        Ungrab(false, true);
                    else
                    {
                        Cache.Transform.position = GrabHand.transform.position;
                        Cache.Transform.rotation = GrabHand.transform.rotation;
                    }
                }
                else if (MountState == HumanMountState.MapObject)
                {
                    if (MountedTransform == null)
                        Unmount(true);
                    else
                    {
                        Cache.Transform.position = MountedTransform.TransformPoint(MountedPositionOffset);
                        Cache.Transform.rotation = Quaternion.Euler(MountedTransform.rotation.eulerAngles + MountedRotationOffset);
                    }
                }
                else if (MountState == HumanMountState.Horse)
                {
                    if (Horse == null)
                        Unmount(true);
                    else
                    {
                        Cache.Transform.position = Horse.Cache.Transform.position + Vector3.up * 1.95f;
                        Cache.Transform.rotation = Horse.Cache.Transform.rotation;
                    }
                }
                else if (State == HumanState.Attack)
                {
                    if (Setup.Weapon == HumanWeapon.Blade)
                    {
                        var bladeWeapon = (BladeWeapon)Weapon;
                        if (!bladeWeapon.IsActive)
                            _attackButtonRelease = true;
                        if (!_attackRelease)
                        {
                            if (_attackButtonRelease)
                            {
                                ContinueAnimation();
                                _attackRelease = true;
                            }
                            else if (Cache.Animation[AttackAnimation].normalizedTime >= 0.32f)
                                PauseAnimation();
                        }
                        float startTime;
                        float endTime;
                        if (bladeWeapon.CurrentDurability <= 0f)
                            startTime = endTime = -1f;
                        else if (AttackAnimation == HumanAnimations.Attack4)
                        {
                            startTime = 0.6f;
                            endTime = 0.9f;
                        }
                        else
                        {
                            startTime = 0.5f;
                            endTime = 0.85f;
                        }
                        if (Cache.Animation[AttackAnimation].normalizedTime > startTime && Cache.Animation[AttackAnimation].normalizedTime < endTime)
                        {
                            if (!HumanCache.BladeHitLeft.IsActive())
                            {
                                HumanCache.BladeHitLeft.Activate();
                                if (SettingsManager.SoundSettings.OldBladeEffect.Value)
                                    PlaySound(HumanSounds.OldBladeSwing);
                                else
                                {
                                    int random = UnityEngine.Random.Range(1, 5);
                                    PlaySound("BladeSwing" + random.ToString());
                                }
                                ToggleBladeTrails(true);
                            }
                            if (!HumanCache.BladeHitRight.IsActive())
                                HumanCache.BladeHitRight.Activate();
                        }
                        else if (HumanCache.BladeHitLeft.IsActive())
                        {
                            HumanCache.BladeHitLeft.Deactivate();
                            HumanCache.BladeHitRight.Deactivate();
                            ToggleBladeTrails(false);
                        }
                        if (Cache.Animation[AttackAnimation].normalizedTime >= 1f)
                            Idle();
                    }
                    else if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.Thunderspear || Setup.Weapon == HumanWeapon.APG)
                    {
                        if (Cache.Animation[AttackAnimation].normalizedTime >= 1f)
                            Idle();
                    }
                }
                else if (State == HumanState.EmoteAction || State == HumanState.SpecialAction || State == HumanState.Stun)
                {
                    if (_stateTimeLeft <= 0f)
                        Idle();
                }
                else if (State == HumanState.GroundDodge)
                {
                    if (Cache.Animation.IsPlaying(HumanAnimations.Dodge))
                    {
                        if (!(Grounded || (Cache.Animation[HumanAnimations.Dodge].normalizedTime <= 0.6f)))
                            Idle();
                        if (Cache.Animation[HumanAnimations.Dodge].normalizedTime >= 1f)
                            Idle();
                    }
                }
                else if (State == HumanState.Land)
                {
                    if (Cache.Animation.IsPlaying(HumanAnimations.Land) && (Cache.Animation[HumanAnimations.Land].normalizedTime >= 1f))
                        Idle();
                }
                else if (State == HumanState.Refill)
                {
                    if (_stateTimeLeft <= 0f)
                    {
                        Idle();
                        FinishRefill();
                    }
                }
                else if (State == HumanState.Reload)
                {
                    if (_stateTimeLeft <= 0f)
                        Idle();
                }
                else if (State == HumanState.Slide)
                {
                    if (!Grounded)
                        Idle();
                }
                else if (State == HumanState.AirDodge)
                {
                    if (_dashTimeLeft > 0f)
                    {
                        _dashTimeLeft -= Time.deltaTime;
                        if (Cache.Rigidbody.velocity.magnitude > _originalDashSpeed)
                            Cache.Rigidbody.AddForce(-Cache.Rigidbody.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
                    }
                    else
                        Idle();
                }

                if (CarryState == HumanCarryState.Carry)
                {
                    if (Carrier == null || Carrier.Dead)
                        Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
                    else if (MountState != HumanMountState.None || State == HumanState.Grab)
                        Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
                    else
                    {
                        Vector3 offset = CarryBack.transform.forward * -0.4f + CarryBack.transform.up * 0.5f;
                        Cache.Transform.position = CarryBack.transform.position + offset;
                        Cache.Transform.rotation = CarryBack.transform.rotation;
                    }

                    if (Carrier != null && Vector3.Distance(Carrier.Cache.Transform.position, Cache.Transform.position) > 7f)
                        Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
                }
            }
        }

        protected void FixedUpdate()
        {
            if (gameObject.GetPhotonView().Owner.CustomProperties.ContainsKey("Logistician"))
                LogisticianBackPack.SetActive(true);
            else
                LogisticianBackPack.SetActive(false);

            if (IsMine())
            {
                FixedUpdateLookTitan();
                FixedUpdateUseables();
                _isReelingOut = false;
                if (State == HumanState.Grab || Dead)
                {
                    Cache.Rigidbody.velocity = Vector3.zero;
                    return;
                }
                if (CarryState == HumanCarryState.Carry)
                {
                    Cache.Rigidbody.velocity = Vector3.zero;
                    Grounded = false;
                    return;
                }
                if (MountState == HumanMountState.Horse)
                {
                    Cache.Rigidbody.velocity = Horse.Cache.Rigidbody.velocity;
                    return;
                }
                if (MountState == HumanMountState.MapObject)
                {
                    Cache.Rigidbody.velocity = Vector3.zero;
                    ToggleSparks(false);
                    if (State != HumanState.Idle)
                        Idle();
                    return;
                }
                if (_hookHuman != null && !_hookHuman.Dead)
                {
                    Vector3 vector2 = _hookHuman.Cache.Transform.position - Cache.Transform.position;
                    float magnitude = vector2.magnitude;
                    if (magnitude > 2f)
                        Cache.Rigidbody.AddForce((vector2.normalized * Mathf.Pow(magnitude, 0.15f) * 30f) - (Cache.Rigidbody.velocity * 0.95f), ForceMode.VelocityChange);
                    _hookHumanConstantTimeLeft -= Time.fixedDeltaTime;
                    if (_hookHumanConstantTimeLeft <= 0f)
                    {
                        _hookHumanConstantTimeLeft = 1f;
                        _hookHuman.Cache.PhotonView.RPC("OnStillHookedByHuman", _hookHuman.Cache.PhotonView.Owner, new object[] { Cache.PhotonView.ViewID });
                    }
                }
                _currentVelocity = Cache.Rigidbody.velocity;
                GameProgressManager.RegisterSpeed(gameObject, _currentVelocity.magnitude);
                Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, _targetRotation, Time.deltaTime * 6f);
                CheckGround();
                bool pivotLeft = FixedUpdateLaunch(true);
                bool pivotRight = FixedUpdateLaunch(false);
                bool pivot = pivotLeft || pivotRight;
                if (Grounded)
                {
                    Vector3 newVelocity = Vector3.zero;
                    if (JustGrounded)
                    {
                        if (State != HumanState.Attack && State != HumanState.SpecialAttack && State != HumanState.SpecialAction
                            && State != HumanState.Stun && !HasDirection && !HasHook())
                        {
                            State = HumanState.Land;
                            CrossFade(HumanAnimations.Land, 0.01f);
                            if (!IsPlayingSound(HumanSounds.Land))
                                PlaySound(HumanSounds.Land);
                        }
                        else
                        {
                            _attackButtonRelease = true;
                            Vector3 v = _currentVelocity;
                            if (State != HumanState.Attack && State != HumanState.SpecialAttack && State != HumanState.SpecialAction && State != HumanState.Stun &&
                                State != HumanState.EmoteAction && (v.x * v.x + v.z * v.z > RunSpeed * RunSpeed * 1.5f) && State != HumanState.Refill)
                            {
                                State = HumanState.Slide;
                                CrossFade(HumanAnimations.Slide, 0.05f);
                                TargetAngle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
                                _targetRotation = GetTargetRotation();
                                HasDirection = true;
                                ToggleSparks(true);
                                if (!IsPlayingSound(HumanSounds.CrashLand) && SettingsManager.SoundSettings.CrashLandEffect.Value)
                                    PlaySound(HumanSounds.CrashLand);
                            }
                        }
                        newVelocity = _currentVelocity;
                    }
                    if (State == HumanState.GroundDodge)
                    {
                        if (Cache.Animation[HumanAnimations.Dodge].normalizedTime >= 0.2f && Cache.Animation[HumanAnimations.Dodge].normalizedTime < 0.8f)
                            newVelocity = -Cache.Transform.forward * 2.4f * RunSpeed;
                        else if (Cache.Animation[HumanAnimations.Dodge].normalizedTime > 0.8f)
                            newVelocity = Cache.Rigidbody.velocity * 0.9f;
                    }
                    else if (State == HumanState.Idle)
                    {
                        newVelocity = Vector3.zero;
                        if (HasDirection)
                        {
                            newVelocity = GetTargetDirection() * TargetMagnitude * RunSpeed;
                            if (!Cache.Animation.IsPlaying(HumanAnimations.Run) && !Cache.Animation.IsPlaying(HumanAnimations.Jump) &&
                                !Cache.Animation.IsPlaying(HumanAnimations.RunBuffed) && (!Cache.Animation.IsPlaying(HumanAnimations.HorseMount) ||
                                Cache.Animation[HumanAnimations.HorseMount].normalizedTime >= 0.5f))
                            {
                                CrossFade(RunAnimation, 0.1f);
                                _stepPhase = 0;
                            }
                            if (!Cache.Animation.IsPlaying(HumanAnimations.WallRun))
                                _targetRotation = GetTargetRotation();
                        }
                        else if (!(Cache.Animation.IsPlaying(StandAnimation) || State == HumanState.Land || Cache.Animation.IsPlaying(HumanAnimations.Jump) || Cache.Animation.IsPlaying(HumanAnimations.HorseMount) || Cache.Animation.IsPlaying(HumanAnimations.Grabbed)))
                        {
                            CrossFade(StandAnimation, 0.1f);
                        }
                    }
                    else if (State == HumanState.Land)
                    {
                        newVelocity = Cache.Rigidbody.velocity * 0.96f;
                    }
                    else if (State == HumanState.Slide)
                    {
                        newVelocity = Cache.Rigidbody.velocity * 0.985f;
                        if (_currentVelocity.magnitude < RunSpeed * 1.2f)
                        {
                            Idle();
                            ToggleSparks(false);
                        }
                    }
                    Vector3 force = newVelocity - _currentVelocity;
                    force.x = Mathf.Clamp(force.x, -MaxVelocityChange, MaxVelocityChange);
                    force.z = Mathf.Clamp(force.z, -MaxVelocityChange, MaxVelocityChange);
                    force.y = 0f;
                    if (Cache.Animation.IsPlaying(HumanAnimations.Jump) && Cache.Animation[HumanAnimations.Jump].normalizedTime > 0.18f)
                        force.y += 8f;
                    if (Cache.Animation.IsPlaying(HumanAnimations.HorseMount) && Cache.Animation[HumanAnimations.HorseMount].normalizedTime > 0.18f && Cache.Animation[HumanAnimations.HorseMount].normalizedTime < 1f)
                    {
                        force = -_currentVelocity;
                        force.y = 6f;
                        float distance = Vector3.Distance(Horse.Cache.Transform.position, Cache.Transform.position);
                        force += (Horse.Cache.Transform.position - Cache.Transform.position).normalized * 0.6f * Gravity.magnitude * distance / 12f;
                    }
                    if (!IsStock(pivot) && !pivot)
                    {
                        _currentVelocity += force;
                        Cache.Rigidbody.velocity = _currentVelocity;
                    }
                    Cache.Rigidbody.rotation = Quaternion.Lerp(Cache.Transform.rotation, Quaternion.Euler(0f, TargetAngle, 0f), Time.deltaTime * 10f);
                }
                else
                {
                    ToggleSparks(false);
                    if (Horse != null && (Cache.Animation.IsPlaying(HumanAnimations.HorseMount) || Cache.Animation.IsPlaying(HumanAnimations.AirFall)) && Cache.Rigidbody.velocity.y < 0f && Vector3.Distance(Horse.Cache.Transform.position + Vector3.up * 1.65f, Cache.Transform.position) < 0.5f)
                    {
                        Cache.Transform.position = Horse.Cache.Transform.position + Vector3.up * 1.95f;
                        Cache.Transform.rotation = Horse.Cache.Transform.rotation;
                        MountState = HumanMountState.Horse;
                        SetInterpolation(false);
                        if (!Cache.Animation.IsPlaying(HumanAnimations.HorseIdle))
                            CrossFade(HumanAnimations.HorseIdle, 0.1f);
                    }
                    if (Cache.Animation[HumanAnimations.Dash].normalizedTime >= 0.99f || (State == HumanState.Idle && !Cache.Animation.IsPlaying(HumanAnimations.Dash) && !Cache.Animation.IsPlaying(HumanAnimations.WallRun) && !Cache.Animation.IsPlaying(HumanAnimations.ToRoof)
                        && !Cache.Animation.IsPlaying(HumanAnimations.HorseMount) && !Cache.Animation.IsPlaying(HumanAnimations.HorseDismount) && !Cache.Animation.IsPlaying(HumanAnimations.AirRelease)
                        && MountState == HumanMountState.None && (!Cache.Animation.IsPlaying(HumanAnimations.AirHookLJust) || Cache.Animation[HumanAnimations.AirHookLJust].normalizedTime >= 1f) && (!Cache.Animation.IsPlaying(HumanAnimations.AirHookRJust) || Cache.Animation[HumanAnimations.AirHookRJust].normalizedTime >= 1f)))
                    {
                        if (!IsHookedAny() && (Cache.Animation.IsPlaying(HumanAnimations.AirHookL) || Cache.Animation.IsPlaying(HumanAnimations.AirHookR) || Cache.Animation.IsPlaying(HumanAnimations.AirHook)) && Cache.Rigidbody.velocity.y > 20f)
                        {
                            CrossFade(HumanAnimations.AirRelease);
                        }
                        else
                        {
                            if ((Mathf.Abs(_currentVelocity.x) + Mathf.Abs(_currentVelocity.z)) <= 25f)
                            {
                                if (_currentVelocity.y < 0f)
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.AirFall))
                                        CrossFade(HumanAnimations.AirFall, 0.2f);
                                }
                                else if (!Cache.Animation.IsPlaying(HumanAnimations.AirRise))
                                    CrossFade(HumanAnimations.AirRise, 0.2f);
                            }
                            else if (!IsHookedAny())
                            {
                                float angle = -Mathf.DeltaAngle(-Mathf.Atan2(_currentVelocity.z, _currentVelocity.x) * Mathf.Rad2Deg, Cache.Transform.rotation.eulerAngles.y - 90f);
                                if (Mathf.Abs(angle) < 45f)
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.Air2))
                                        CrossFade(HumanAnimations.Air2, 0.2f);
                                }
                                else if ((angle < 135f) && (angle > 0f))
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.Air2Right))
                                        CrossFade(HumanAnimations.Air2Right, 0.2f);
                                }
                                else if ((angle > -135f) && (angle < 0f))
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.Air2Left))
                                        CrossFade(HumanAnimations.Air2Left, 0.2f);
                                }
                                else if (!Cache.Animation.IsPlaying(HumanAnimations.Air2Backward))
                                    CrossFade(HumanAnimations.Air2Backward, 0.2f);
                            }
                            else if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                            {
                                if (IsHookedLeft())
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.AHSSHookForwardL))
                                        CrossFade(HumanAnimations.AHSSHookForwardL, 0.1f);
                                }
                                else if (IsHookedRight())
                                {
                                    if (!Cache.Animation.IsPlaying(HumanAnimations.AHSSHookForwardR))
                                        CrossFade(HumanAnimations.AHSSHookForwardR, 0.1f);
                                }
                                else if (!Cache.Animation.IsPlaying(HumanAnimations.AHSSHookForwardBoth))
                                    CrossFade(HumanAnimations.AHSSHookForwardBoth, 0.1f);
                            }
                            else if (!IsHookedRight())
                            {
                                if (!Cache.Animation.IsPlaying(HumanAnimations.AirHookL))
                                    CrossFade(HumanAnimations.AirHookL, 0.1f);
                            }
                            else if (!IsHookedLeft())
                            {
                                if (!Cache.Animation.IsPlaying(HumanAnimations.AirHookR))
                                    CrossFade(HumanAnimations.AirHookR, 0.1f);
                            }
                            else if (!Cache.Animation.IsPlaying(HumanAnimations.AirHook))
                                CrossFade(HumanAnimations.AirHook, 0.1f);
                        }
                    }
                    if (!Cache.Animation.IsPlaying(HumanAnimations.AirRise))
                    {
                        if (State == HumanState.Idle && Cache.Animation.IsPlaying(HumanAnimations.AirRelease) && Cache.Animation[HumanAnimations.AirRelease].normalizedTime >= 1f)
                            CrossFade(HumanAnimations.AirRise, 0.2f);
                        else if (Cache.Animation.IsPlaying(HumanAnimations.HorseDismount) && Cache.Animation[HumanAnimations.HorseDismount].normalizedTime >= 1f)
                            CrossFade(HumanAnimations.AirRise, 0.2f);
                    }
                    if (Cache.Animation.IsPlaying(HumanAnimations.ToRoof))
                    {
                        if (Cache.Animation[HumanAnimations.ToRoof].normalizedTime < 0.22f)
                        {
                            Cache.Rigidbody.velocity = Vector3.zero;
                            Cache.Rigidbody.AddForce(new Vector3(0f, Gravity.magnitude * Cache.Rigidbody.mass, 0f));
                        }
                        else
                        {
                            if (!_wallJump)
                            {
                                _wallJump = true;
                                Cache.Rigidbody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                            }
                            Cache.Rigidbody.AddForce(Cache.Transform.forward * 0.05f, ForceMode.Impulse);
                        }
                        if (Cache.Animation[HumanAnimations.ToRoof].normalizedTime >= 1f)
                        {
                            PlayAnimation(HumanAnimations.AirRise);
                        }
                    }
                    else if (!(State != HumanState.Idle || !IsPressDirectionTowardsHero() || SettingsManager.InputSettings.Human.Jump.GetKey() || SettingsManager.InputSettings.Human.HookLeft.GetKey() || SettingsManager.InputSettings.Human.HookRight.GetKey() || SettingsManager.InputSettings.Human.HookBoth.GetKey() || !IsFrontGrounded() || Cache.Animation.IsPlaying(HumanAnimations.WallRun) || Cache.Animation.IsPlaying(HumanAnimations.Dodge)))
                    {
                        CrossFade(HumanAnimations.WallRun, 0.1f);
                        _wallRunTime = 0f;
                    }
                    else if (Cache.Animation.IsPlaying(HumanAnimations.WallRun))
                    {
                        Cache.Rigidbody.AddForce(Vector3.up * RunSpeed - Cache.Rigidbody.velocity, ForceMode.VelocityChange);
                        _wallRunTime += Time.deltaTime;
                        if (_wallRunTime > 1f || !HasDirection)
                        {
                            Cache.Rigidbody.AddForce(-Cache.Transform.forward * RunSpeed * 0.75f, ForceMode.Impulse);
                            DodgeWall();
                        }
                        else if (!IsUpFrontGrounded())
                        {
                            _wallJump = false;
                            CrossFade(HumanAnimations.ToRoof, 0.1f);
                        }
                        else if (!IsFrontGrounded())
                            CrossFade(HumanAnimations.AirFall, 0.1f);
                    }
                    else if (!Cache.Animation.IsPlaying(HumanAnimations.Dash) && !Cache.Animation.IsPlaying(HumanAnimations.Jump) && !IsFiringThunderspear())
                    {
                        Vector3 targetDirection = GetTargetDirection() * TargetMagnitude * Setup.CustomSet.Acceleration.Value / 5f;
                        if (!HasDirection)
                        {
                            if (State == HumanState.Attack)
                                targetDirection = Vector3.zero;
                        }
                        else
                            _targetRotation = GetTargetRotation();
                        bool isUsingGas = SettingsManager.InputSettings.Human.Jump.GetKey() ^ SettingsManager.InputSettings.Human.AutoUseGas.Value;
                        if (((!pivotLeft && !pivotRight) && (MountState == HumanMountState.None && isUsingGas)) && (CurrentGas > 0f))
                        {
                            if (HasDirection)
                            {
                                Cache.Rigidbody.AddForce(targetDirection, ForceMode.Acceleration);
                            }
                            else
                            {
                                Cache.Rigidbody.AddForce((Cache.Transform.forward * targetDirection.magnitude), ForceMode.Acceleration);
                            }
                            pivot = true;
                        }
                    }
                    if ((Cache.Animation.IsPlaying(HumanAnimations.AirFall) && (_currentVelocity.magnitude < 0.2f)) && this.IsFrontGrounded())
                    {
                        CrossFade(HumanAnimations.OnWall, 0.3f);
                    }
                }
                if (pivotLeft && pivotRight)
                    FixedUpdatePivot((HookRight.GetHookPosition() + HookLeft.GetHookPosition()) * 0.5f);
                else if (pivotLeft)
                    FixedUpdatePivot(HookLeft.GetHookPosition());
                else if (pivotRight)
                    FixedUpdatePivot(HookRight.GetHookPosition());
                bool lowerGravity = false;
                if (IsHookedLeft() && HookLeft.GetHookPosition().y > Cache.Transform.position.y && _launchLeft)
                    lowerGravity = true;
                else if (IsHookedRight() && HookRight.GetHookPosition().y > Cache.Transform.position.y && _launchRight)
                    lowerGravity = true;
                Vector3 gravity;
                if (lowerGravity)
                    gravity = Gravity * 0.5f * Cache.Rigidbody.mass;
                else
                    gravity = Gravity * Cache.Rigidbody.mass;
                if (Grounded && State == HumanState.Attack)
                {
                    if (Cache.Animation.IsPlaying(HumanAnimations.Attack1) || Cache.Animation.IsPlaying(HumanAnimations.Attack2))
                    {
                        bool stockPivot = pivotLeft || pivotRight;
                        bool isStock = IsStock(stockPivot);
                        if (isStock || !stockPivot)
                        {
                            _currentVelocity += Cache.Transform.forward * 4f / Mathf.Max(Cache.Rigidbody.mass, 0.001f);
                            Cache.Rigidbody.velocity = _currentVelocity;
                        }
                        if (!SettingsManager.InGameCurrent.Misc.AllowStock.Value || SettingsManager.InGameCurrent.Misc.RealismMode.Value)
                        {
                            _currentVelocity = _currentVelocity.normalized * Mathf.Min(_currentVelocity.magnitude, 20f);
                            Cache.Rigidbody.velocity = _currentVelocity;
                        }
                    }
                    ToggleSparks(false);
                }
                gravity += WeatherManager.GetWeatherForce();
                Cache.Rigidbody.AddForce(gravity);
                if (!_cancelGasDisable)
                {
                    if (pivot)
                    {
                        UseGas(GasUsage * Time.deltaTime);
                        if (!HumanCache.Smoke.emission.enabled)
                            Cache.PhotonView.RPC("SetSmokeRPC", RpcTarget.All, new object[] { true });
                        if (!IsPlayingSound(HumanSounds.GasLoop) && SettingsManager.SoundSettings.GasEffect.Value)
                            PlaySound(HumanSounds.GasLoop);
                    }
                    else
                    {
                        if (HumanCache.Smoke.emission.enabled)
                            Cache.PhotonView.RPC("SetSmokeRPC", RpcTarget.All, new object[] { false });
                        if (IsPlayingSound(HumanSounds.GasLoop))
                        {
                            StopSound(HumanSounds.GasLoop);
                            ToggleSound(HumanSounds.GasEnd, true);
                        }
                    }
                }
                else
                    _cancelGasDisable = false;
                var windEmission = HumanCache.Wind.emission;
                var windMain = HumanCache.Wind.main;
                if (WindWeatherEffect.WindEnabled)
                {
                    if (!windEmission.enabled)
                        windEmission.enabled = true;
                    windMain.startSpeedMultiplier = 100f;
                    HumanCache.WindTransform.LookAt(Cache.Transform.position + WindWeatherEffect.WindDirection);
                }
                else if (_currentVelocity.magnitude > 80f && SettingsManager.GraphicsSettings.WindEffectEnabled.Value)
                {
                    if (!windEmission.enabled)
                        windEmission.enabled = true;
                    windMain.startSpeedMultiplier = _currentVelocity.magnitude;
                    HumanCache.WindTransform.LookAt(Cache.Transform.position - _currentVelocity);
                }
                else if (windEmission.enabled)
                    windEmission.enabled = false;
                FixedUpdateSetHookedDirection();
                FixedUpdateBodyLean();
                FixedUpdateClippingCheck();
                ReelInAxis = 0f;
            }
        }

        protected override void LateUpdate()
        {
            if (IsMine() && MountState == HumanMountState.None && State != HumanState.Grab)
            {
                LateUpdateTilt();
                LateUpdateGun();
                LateUpdateReelOut();
            }
            base.LateUpdate();
        }

        protected void OnCollisionEnter(Collision collision)
        {
            var velocity = Cache.Rigidbody.velocity;
            float angle = Mathf.Abs(Vector3.Angle(velocity, _lastVelocity));
            float speedMultiplier = Mathf.Max(1f - (angle * 1.5f * 0.01f), 0f);
            float speed = _lastVelocity.magnitude * speedMultiplier;
            Cache.Rigidbody.velocity = velocity.normalized * speed;
            float speedDiff = _lastVelocity.magnitude - Cache.Rigidbody.velocity.magnitude;
            if (SettingsManager.InGameCurrent.Misc.RealismMode.Value && speedDiff > RealismDeathVelocity)
            {
                GetKilled("Impact");
                return;
            }
        }

        private void LateUpdateReelOut()
        {
            ToggleSound(HumanSounds.ReelOut, _isReelingOut && SettingsManager.SoundSettings.ReelOutEffect.Value);
        }

        private void UpdateBladeTrails()
        {

        }

        private bool FixedUpdateLaunch(bool left)
        {
            bool launch;
            HookUseable hook;
            bool pivot = false;
            float launchTime;
            if (left)
            {
                launch = _launchLeft;
                hook = HookLeft;
                _launchLeftTime += Time.deltaTime;
                launchTime = _launchLeftTime;
            }
            else
            {
                launch = _launchRight;
                hook = HookRight;
                _launchRightTime += Time.deltaTime;
                launchTime = _launchRightTime;
            }
            if (launch)
            {
                if (hook.IsHooked())
                {
                    Vector3 v = (hook.GetHookPosition() - Cache.Transform.position).normalized * 10f;
                    if (!(_launchLeft && _launchRight))
                        v *= 2f;
                    if ((Vector3.Angle(Cache.Rigidbody.velocity, v) > 90f) && (SettingsManager.InputSettings.Human.Jump.GetKey() ^ SettingsManager.InputSettings.Human.AutoUseGas.Value))
                    {
                        pivot = true;
                    }
                    if (!pivot)
                    {
                        Cache.Rigidbody.AddForce(v);
                        if (Vector3.Angle(Cache.Rigidbody.velocity, v) > 90f)
                            Cache.Rigidbody.AddForce(-Cache.Rigidbody.velocity * 2f, ForceMode.Acceleration);
                    }
                }
                if (hook.IsActive && CurrentGas > 0f)
                    UseGas(GasUsage * Time.deltaTime);
                else if (launchTime > 0.3f)
                {
                    if (left)
                        _launchLeft = false;
                    else
                        _launchRight = false;
                    hook.DisableActiveHook();
                    UnhookHuman(left);
                    pivot = false;
                }
            }
            return pivot;
        }

        private void FixedUpdatePivot(Vector3 position)
        {
            float addSpeed = 0.1f;
            if (Grounded)
                addSpeed = -0.01f;
            float newSpeed = _currentVelocity.magnitude + addSpeed;
            Vector3 v = position - Cache.Rigidbody.position;
            float reelAxis = GetReelAxis();
            if (reelAxis > 0f)
            {
                if (SettingsManager.InGameCurrent.Misc.RealismMode.Value && Vector3.Distance(Cache.Transform.position, position) > RealismMaxReel)
                    reelAxis = 0f;
            }
            float reel = Mathf.Clamp(reelAxis, -0.8f, 0.8f) + 1f;
            v = Vector3.RotateTowards(v, _currentVelocity, 1.53938f * reel, 1.53938f * reel).normalized;
            if (reelAxis > 0f)
                _isReelingOut = true;
            else if (reelAxis < 0f && !_reelInWaitForRelease)
            {
                if (SettingsManager.SoundSettings.ReelInEffect.Value)
                    PlaySoundRPC(HumanSounds.ReelIn, Util.CreateLocalPhotonInfo());
                if (!SettingsManager.InputSettings.Human.ReelInHolding.Value)
                    _reelInWaitForRelease = true;
            }
            _currentVelocity = v * newSpeed;
            Cache.Rigidbody.velocity = _currentVelocity;
        }

        private bool IsStock(bool pivot)
        {
            return Grounded && State == HumanState.Attack && GetReelAxis() > 0f && pivot &&
                (Cache.Animation.IsPlaying(HumanAnimations.Attack1) || Cache.Animation.IsPlaying(HumanAnimations.Attack2));
        }

        private void FixedUpdateSetHookedDirection()
        {
            _almostSingleHook = false;
            float oldTargetAngle = TargetAngle;
            if (IsHookedLeft() && IsHookedRight())
            {
                Vector3 hookDiff = HookLeft.GetHookPosition() - HookRight.GetHookPosition();
                Vector3 direction = (HookLeft.GetHookPosition() + HookRight.GetHookPosition()) * 0.5f - Cache.Transform.position;
                if (hookDiff.sqrMagnitude < 4f)
                {
                    TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    if ((Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG) && State != HumanState.Attack)
                    {
                        float current = -Mathf.Atan2(Cache.Rigidbody.velocity.z, Cache.Rigidbody.velocity.x) * Mathf.Rad2Deg;
                        float target = -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                        TargetAngle -= Mathf.DeltaAngle(current, target);
                    }
                    _almostSingleHook = true;
                }
                else
                {
                    Vector3 left = Cache.Transform.position - HookLeft.GetHookPosition();
                    Vector3 right = Cache.Transform.position - HookRight.GetHookPosition();
                    if (Vector3.Angle(-direction, left) < 30f && Vector3.Angle(-direction, right) < 30f)
                    {
                        _almostSingleHook = true;
                        TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        _almostSingleHook = false;
                        Vector3 forward = Cache.Transform.forward;
                        Vector3.OrthoNormalize(ref hookDiff, ref forward);
                        TargetAngle = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
                        float angle = Mathf.Atan2(left.x, left.z) * Mathf.Rad2Deg;
                        if (Mathf.DeltaAngle(angle, TargetAngle) > 0f)
                            TargetAngle += 180f;
                    }
                }
            }
            else
            {
                _almostSingleHook = true;
                Vector3 v;
                if (IsHookedLeft())
                    v = HookLeft.GetHookPosition() - Cache.Transform.position;
                else if (IsHookedRight())
                    v = HookRight.GetHookPosition() - Cache.Transform.position;
                else
                    return;
                TargetAngle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
                if (State != HumanState.Attack)
                {
                    float angle1 = -Mathf.Atan2(Cache.Rigidbody.velocity.z, Cache.Rigidbody.velocity.x) * Mathf.Rad2Deg;
                    float angle2 = -Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
                    float delta = -Mathf.DeltaAngle(angle1, angle2);
                    if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                        TargetAngle += delta;
                    else
                    {
                        float multiplier = 0.1f;
                        if ((IsHookedLeft() && delta < 0f) || (IsHookedRight() && delta > 0f))
                            multiplier = -0.1f;
                        TargetAngle += delta * multiplier;
                    }
                }
            }
            if (IsFiringThunderspear())
                TargetAngle = oldTargetAngle;
        }

        private void FixedUpdateBodyLean()
        {
            float z = 0f;
            _needLean = false;
            if (Setup.Weapon != HumanWeapon.AHSS && Setup.Weapon != HumanWeapon.APG && State == HumanState.Attack && !IsFiringThunderspear())
            {
                Vector3 v = Cache.Rigidbody.velocity;
                float diag = Mathf.Sqrt((v.x * v.x) + (v.z * v.z));
                float angle = Mathf.Atan2(v.y, diag) * Mathf.Rad2Deg;
                _targetRotation = Quaternion.Euler(-angle * (1f - (Vector3.Angle(v, Cache.Transform.forward) / 90f)), TargetAngle, 0f);
                if (IsHookedAny())
                    Cache.Transform.rotation = _targetRotation;
            }
            else
            {
                if (!Grounded)
                {
                    if (IsHookedLeft() && IsHookedRight())
                    {
                        if (_almostSingleHook)
                        {
                            _needLean = true;
                            z = GetLeanAngle(HookRight.GetHookPosition(), true);
                        }
                    }
                    else if (IsHookedLeft())
                    {
                        _needLean = true;
                        z = GetLeanAngle(HookLeft.GetHookPosition(), true);
                    }
                    else if (IsHookedRight())
                    {
                        _needLean = true;
                        z = GetLeanAngle(HookRight.GetHookPosition(), false);

                    }
                }
                if (_needLean)
                {
                    float a = 0f;
                    if (Setup.Weapon != HumanWeapon.AHSS && Setup.Weapon != HumanWeapon.APG && State != HumanState.Attack)
                    {
                        a = Cache.Rigidbody.velocity.magnitude * 0.1f;
                        a = Mathf.Min(a, 20f);
                    }
                    _targetRotation = Quaternion.Euler(-a, TargetAngle, z);
                }
                else if (State != HumanState.Attack && !Cache.Animation.IsPlaying(HumanAnimations.WallRun))
                    _targetRotation = Quaternion.Euler(0f, TargetAngle, 0f);
            }
        }

        private void FixedUpdateUseables()
        {
            if (FinishSetup)
            {
                Weapon.OnFixedUpdate();
                HookLeft.OnFixedUpdate();
                HookRight.OnFixedUpdate();

                for (int i = 0; i < SpecialsArray.Length; i++) // changed by ata to update all specials in the ability wheel //
                {
                    SpecialsArray[i]?.OnFixedUpdate();
                }

                /*if (Special != null)
                    Special.OnFixedUpdate();*/
            }
        }

        public void FixedUpdateLookTitan()
        {
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = PhysicsLayer.GetMask(PhysicsLayer.EntityDetection);
            RaycastHit[] hitArr = Physics.RaycastAll(ray, 200f, mask.value);
            if (hitArr.Length == 0)
                return;
            List<RaycastHit> hitList = new List<RaycastHit>(hitArr);
            hitList.Sort((x, y) => x.distance.CompareTo(y.distance));
            int maxCount = Math.Min(hitList.Count, 3);
            for (int i = 0; i < maxCount; i++)
            {
                var entity = hitList[i].collider.GetComponent<TitanEntityDetection>();
                entity.Owner.TitanColliderToggler.RegisterLook();
            }
        }

        private void FixedUpdateClippingCheck()
        {
            Vector3 finalPosition = Cache.Rigidbody.position;
            if (_lastVelocity.magnitude > 100f)
            {
                float maxDistance = _lastVelocity.magnitude * 1.1f;
                Vector3 start = _lastPosition + Vector3.up * 0.7f;
                Vector3 end = Cache.Rigidbody.position + Vector3.up * 0.7f;
                Vector3 v = start - end;
                if (v.magnitude > maxDistance)
                    start = end + v.normalized * maxDistance;
                v = end - start;
                var hitArr = Physics.RaycastAll(start, v.normalized, v.magnitude, ClipMask.value);
                System.Array.Sort(hitArr, (x, y) => x.distance.CompareTo(y.distance));
                if (hitArr.Length > 0)
                {
                    bool foundHit = false;
                    RaycastHit firstHit = hitArr[0];
                    foreach (RaycastHit hit in hitArr)
                    {
                        if (hit.collider.isTrigger)
                        {
                            var collisionHandler = hit.collider.GetComponent<CustomLogicCollisionHandler>();
                            if (collisionHandler != null)
                                collisionHandler.OnTriggerEnter(GetComponent<CapsuleCollider>());
                            continue;
                        }
                        if (!foundHit)
                        {
                            firstHit = hit;
                            foundHit = true;
                        }
                    }
                    if (foundHit)
                    {
                        Vector3 position = firstHit.point - Vector3.up * 0.7f;
                        Cache.Rigidbody.position = position;
                        finalPosition = position;
                    }
                }
            }
            _lastPosition = finalPosition;
            _lastVelocity = _currentVelocity;
        }

        private void LateUpdateTilt()
        {
            if (IsMainCharacter() && SettingsManager.GeneralSettings.CameraTilt.Value)
            {
                Quaternion rotation;
                Vector3 left = Vector3.zero;
                Vector3 right = Vector3.zero;
                if (_launchLeft && IsHookedLeft())
                    left = HookLeft.GetHookPosition();
                if (_launchRight && IsHookedRight())
                    right = HookRight.GetHookPosition();
                Vector3 target = Vector3.zero;
                if (left.magnitude != 0f && right.magnitude == 0f)
                    target = left;
                else if (right.magnitude != 0f && left.magnitude == 0f)
                    target = right;
                else if (left.magnitude != 0f && right.magnitude != 0f)
                    target = 0.5f * (left + right);
                Transform camera = SceneLoader.CurrentCamera.Cache.Transform;
                Vector3 projectUp = Vector3.Project(target - Cache.Transform.position, camera.up);
                Vector3 projectRight = Vector3.Project(target - Cache.Transform.position, camera.right);
                if (target.magnitude > 0f)
                {
                    Vector3 projectDirection = projectUp + projectRight;
                    float angle = Vector3.Angle(target - Cache.Transform.position, Cache.Rigidbody.velocity) * 0.005f;
                    Vector3 finalRight = camera.right + projectRight.normalized;
                    float finalAngle = Vector3.Angle(projectUp, projectDirection) * angle;
                    rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, (finalRight.magnitude >= 1f) ? -finalAngle : finalAngle);
                }
                else
                    rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, 0f);
                camera.rotation = Quaternion.Lerp(camera.rotation, rotation, Time.deltaTime * 2f);
            }
        }

        private void LateUpdateGun()
        {
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
            {
                if (!Grounded)
                {
                    HumanCache.HandL.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    HumanCache.HandR.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (_gunArmAim && Setup.Weapon == HumanWeapon.AHSS)
                {
                    Vector3 target = GetAimPoint();
                    Vector3 direction = (target - Cache.Transform.position).normalized;
                    float angle = -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                    float delta = -Mathf.DeltaAngle(angle, Cache.Transform.rotation.eulerAngles.y - 90f);
                    GunHeadMovement();
                    if (!IsHookedAny())
                    {
                        if (delta <= 0f && delta > -90f)
                            LeftArmAim(target);
                        else if (delta > 0f && delta < 90f)
                            RightArmAim(target);
                    }
                    else if (!IsHookedLeft() && delta < 40f && delta > -90f)
                        LeftArmAim(target);
                    else if (!IsHookedRight() && delta > -40f && delta < 90f)
                        RightArmAim(target);
                }
                if (IsHookedLeft())
                    LeftArmAim(HookLeft.GetHookPosition());
                if (IsHookedRight())
                    RightArmAim(HookRight.GetHookPosition());
            }
        }

        private void GunHeadMovement()
        {
            return;
            Vector3 _gunTarget = GetAimPoint();
            Vector3 position = Cache.Transform.position;
            float x = Mathf.Sqrt(Mathf.Pow(_gunTarget.x - position.x, 2f) + Mathf.Pow(_gunTarget.z - position.z, 2f));
            var originalRotation = Cache.Transform.rotation;
            Vector3 euler = originalRotation.eulerAngles;
            Vector3 direction = _gunTarget - position;
            float angle = -Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float deltaY = -Mathf.DeltaAngle(angle, euler.y - 90f);
            deltaY = Mathf.Clamp(deltaY, -40f, 40f);
            float y = HumanCache.Neck.position.y - _gunTarget.y;
            float deltaX = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            deltaX = Mathf.Clamp(deltaX, -40f, 30f);
            var targetRotation = Quaternion.Euler(euler.x + deltaX, euler.y + deltaY, euler.z);
            _targetRotation = Quaternion.Lerp(_targetRotation, targetRotation, Time.deltaTime * 100f);
        }

        private void LeftArmAim(Vector3 target)
        {
            float x = target.x - HumanCache.UpperarmL.position.x;
            float y = target.y - HumanCache.UpperarmL.position.y;
            float z = target.z - HumanCache.UpperarmL.position.z;
            float sq = Mathf.Sqrt((x * x) + (z * z));
            HumanCache.HandL.localRotation = Quaternion.Euler(0f, 0f, 0f);
            HumanCache.ForearmL.localRotation = Quaternion.Euler(0f, 0f, 0f);
            HumanCache.UpperarmL.rotation = Quaternion.Euler(0f, -90f + (Mathf.Atan2(x, z) * Mathf.Rad2Deg), -90f + Mathf.Atan2(y, sq) * Mathf.Rad2Deg);
        }

        private void RightArmAim(Vector3 target)
        {
            float x = target.x - HumanCache.UpperarmR.position.x;
            float y = target.y - HumanCache.UpperarmR.position.y;
            float z = target.z - HumanCache.UpperarmR.position.z;
            float sq = Mathf.Sqrt((x * x) + (z * z));
            HumanCache.HandR.localRotation = Quaternion.Euler(0f, 0f, 0f);
            HumanCache.ForearmR.localRotation = Quaternion.Euler(0f, 0f, 0f);
            HumanCache.UpperarmR.rotation = Quaternion.Euler(180f, -90f + (Mathf.Atan2(x, z) * Mathf.Rad2Deg), -90f - Mathf.Atan2(y, sq) * Mathf.Rad2Deg);
        }

        protected override void SetColliders()
        {
            foreach (Collider c in GetComponentsInChildren<Collider>())
            {
                if (c.name == "checkBox")
                    c.gameObject.layer = PhysicsLayer.Hitbox;
                else
                    c.gameObject.layer = PhysicsLayer.NoCollision;
            }
            gameObject.layer = PhysicsLayer.Human;
        }

        [PunRPC]
        public void SetupRPC(string customSetJson, int humanWeapon, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            HumanCustomSet set = new HumanCustomSet();
            set.DeserializeFromJsonString(customSetJson);
            Setup.Load(set, (HumanWeapon)humanWeapon, false);
            bool isGun = humanWeapon == (int)HumanWeapon.AHSS || humanWeapon == (int)HumanWeapon.APG;
            HookLeft = new HookUseable(this, true, isGun);
            HookRight = new HookUseable(this, false, isGun);
            if (MaxGas == -1f)
                MaxGas = set.Gas.Value;
            if (CurrentGas == -1f)
                CurrentGas = MaxGas;
            SetAcceleration(set.Acceleration.Value);
            SetRunSpeed(set.Speed.Value);
            bool male = Setup.CustomSet.Sex.Value == (int)HumanSex.Male;
            RunAnimation = HumanAnimations.Run;
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                StandAnimation = male ? HumanAnimations.IdleAHSSM : HumanAnimations.IdleAHSSF;
            else if (Setup.Weapon == HumanWeapon.Thunderspear)
            {
                StandAnimation = male ? HumanAnimations.IdleTSM : HumanAnimations.IdleTSF;
                RunAnimation = HumanAnimations.RunTS;
            }
            else
                StandAnimation = male ? HumanAnimations.IdleM : HumanAnimations.IdleF;
            if (IsMine())
            {
                SetupWeapon(set, humanWeapon);
                SetupItems();
                //SetSpecial(SettingsManager.InGameCharacterSettings.Special.Value);
                SetAllSpecials(SettingsManager.InGameCharacterSettings.Special.Value,
                               SettingsManager.InGameCharacterSettings.Special_2.Value,
                               SettingsManager.InGameCharacterSettings.Special_3.Value); // added by Ata 12 May 2024 for Ability Wheel //
                SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special.Value, 1);
                _zippsUIManager = FindFirstObjectByType<ZippsUIManager>(); // setting up the ability wheel UI //
                _zippsUIManager.SetWheelImages(); // setting up the ability wheel UI //
                _zippsUIManager.Ability1Selected = true; // setting up the ability wheel UI //
                _zippsUIManager.Ability2Selected = false; // setting up the ability wheel UI //
                _zippsUIManager.Ability3Selected = false; // setting up the ability wheel UI //
            }
            FinishSetup = true;
            CustomAnimationSpeed();
        }

        public void SetAcceleration(int acceleration)
        {
            AccelerationStat = acceleration;
            Cache.Rigidbody.mass = 0.5f - (acceleration - 100) * 0.001f;
        }

        public void SetRunSpeed(int speed)
        {
            RunSpeedStat = speed;
            RunSpeed = speed / 10f;
        }

        protected void SetupWeapon(HumanCustomSet set, int humanWeapon)
        {
            if (humanWeapon == (int)HumanWeapon.Blade)
            {
                var bladeInfo = CharacterData.HumanWeaponInfo["Blade"];
                Weapon = new BladeWeapon(this, set.Blade.Value * bladeInfo["DurabilityMultiplier"].AsFloat, bladeInfo["Blades"].AsInt);
            }
            else if (humanWeapon == (int)HumanWeapon.AHSS)
            {
                var gunInfo = CharacterData.HumanWeaponInfo["AHSS"];
                Weapon = new AHSSWeapon(this, gunInfo["AmmoTotal"].AsInt, gunInfo["AmmoRound"].AsInt, gunInfo["CD"].AsFloat);
            }
            else if (humanWeapon == (int)HumanWeapon.APG)
            {
                JSONNode gunInfo;
                if (SettingsManager.InGameCurrent.Misc.APGPVP.Value)
                    gunInfo = CharacterData.HumanWeaponInfo["APGPVP"];
                else
                    gunInfo = CharacterData.HumanWeaponInfo["APG"];
                Weapon = new APGWeapon(this, gunInfo["AmmoTotal"].AsInt, gunInfo["AmmoRound"].AsInt, gunInfo["CD"].AsFloat);
            }
            else if (humanWeapon == (int)HumanWeapon.Thunderspear)
            {
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                {
                    int radiusStat = SettingsManager.AbilitySettings.BombRadius.Value;
                    int cdStat = SettingsManager.AbilitySettings.BombCooldown.Value;
                    int speedStat = SettingsManager.AbilitySettings.BombSpeed.Value;
                    int rangeStat = SettingsManager.AbilitySettings.BombRange.Value;
                    if (radiusStat + cdStat + speedStat + rangeStat > 16)
                    {
                        radiusStat = speedStat = 6;
                        rangeStat = 3;
                        cdStat = 1;
                    }
                    float travelTime = ((rangeStat * 60f) + 200f) / ((speedStat * 60f) + 200f);
                    float radius = (radiusStat * 4f) + 20f;
                    float cd = ((cdStat + 4) * -0.4f) + 5f;
                    float speed = (speedStat * 60f) + 200f;
                    Weapon = new ThunderspearWeapon(this, -1, -1, cd, radius, speed, travelTime, 0f);
                    if (CustomLogicManager.Evaluator.CurrentTime > 10f)
                        Weapon.SetCooldownLeft(5f);
                    else
                        Weapon.SetCooldownLeft(10f);
                }
                else
                {
                    var tsInfo = CharacterData.HumanWeaponInfo["Thunderspear"];
                    float travelTime = tsInfo["Range"].AsFloat / tsInfo["Speed"].AsFloat;
                    Weapon = new ThunderspearWeapon(this, tsInfo["AmmoTotal"].AsInt, tsInfo["AmmoRound"].AsInt, tsInfo["CD"].AsFloat, tsInfo["Radius"].AsFloat,
                        tsInfo["Speed"].AsFloat, travelTime, tsInfo["Delay"].AsFloat);
                }
            }
        }

        protected void SetupItems()
        {
            Items.Add(new FlareItem(this, "Green", new Color(0f, 1f, 0f, 0.7f), 10f));
            Items.Add(new FlareItem(this, "Red", new Color(1f, 0f, 0f, 0.7f), 10f));
            Items.Add(new FlareItem(this, "Black", new Color(0f, 0f, 0f, 0.7f), 10f));
            Items.Add(new FlareItem(this, "Purple", new Color(153f / 255, 0f, 204f / 255, 0.7f), 10f));
            Items.Add(new FlareItem(this, "Blue", new Color(0f, 102f / 255, 204f / 255, 0.7f), 10f));
            Items.Add(new FlareItem(this, "Yellow", new Color(1f, 1f, 0f, 0.7f), 10f));
        }

        public void SetSpecial(string special)
        {
            CurrentSpecial = special;
            Special = HumanSpecials.GetSpecialUseable(this, special);
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.SetSpecialIcon(HumanSpecials.GetSpecialIcon(special));
        }

        #region Ability Wheel
        public void SetAllSpecials(string special1, string special2, string special3)
        {
            SpecialsArray = new BaseUseable[]
            {
                HumanSpecials.GetSpecialUseable(this, special1),
                (special2.Length > 0) ? HumanSpecials.GetSpecialUseable(this, special2) : null,
                (special3.Length > 0) ? HumanSpecials.GetSpecialUseable(this, special3) : null
            };

            // add the icons for all specials at some point //
        }

        public void SwitchCurrentSpecial(string special, int newSpecial)
        {
            if (CurrentSpecial != special)
            {
                bool canAnimationReset = (State != HumanState.Die && State != HumanState.Grab && State != HumanState.MountingHorse && State != HumanState.Stun && State != HumanState.GroundDodge);
                if (canAnimationReset)
                    State = HumanState.Idle;

                CurrentSpecial = special;
                Special = SpecialsArray[newSpecial - 1];
                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.SetSpecialIcon(HumanSpecials.GetSpecialIcon(special));

                if (newSpecial == 1)
                {
                    Special_2 = SpecialsArray[1];
                    Special_3 = SpecialsArray[2];

                    SideSpecial_1 = SettingsManager.InGameCharacterSettings.Special_2.Value;
                    SideSpecial_2 = SettingsManager.InGameCharacterSettings.Special_3.Value;
                }
                if (newSpecial == 2)
                {
                    Special_2 = SpecialsArray[0];
                    Special_3 = SpecialsArray[2];

                    SideSpecial_1 = SettingsManager.InGameCharacterSettings.Special.Value;
                    SideSpecial_2 = SettingsManager.InGameCharacterSettings.Special_3.Value;
                }
                if (newSpecial == 3)
                {
                    Special_2 = SpecialsArray[0];
                    Special_3 = SpecialsArray[1];

                    SideSpecial_1 = SettingsManager.InGameCharacterSettings.Special.Value;
                    SideSpecial_2 = SettingsManager.InGameCharacterSettings.Special_2.Value;
                }

                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.SetSpecialIcon_2(HumanSpecials.GetSpecialIcon(SideSpecial_1));
                ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.SetSpecialIcon_3(HumanSpecials.GetSpecialIcon(SideSpecial_2));
            }
        }

        #endregion


        protected void LoadSkin()
        {
            if (IsMine())
            {
                if (SettingsManager.CustomSkinSettings.Human.SkinsEnabled.Value)
                {
                    HumanCustomSkinSet set = (HumanCustomSkinSet)SettingsManager.CustomSkinSettings.Human.GetSelectedSet();
                    string url = string.Join(",", new string[] { set.Horse.Value, set.Hair.Value, set.Eye.Value, set.Glass.Value, set.Face.Value,
                set.Skin.Value, set.Costume.Value, set.Logo.Value, set.GearL.Value, set.GearR.Value, set.Gas.Value, set.Hoodie.Value,
                    set.WeaponTrail.Value, set.ThunderspearL.Value, set.ThunderspearR.Value, set.HookLTiling.Value.ToString(), set.HookL.Value,
                    set.HookRTiling.Value.ToString(), set.HookR.Value });
                    int viewID = -1;
                    if (Horse != null)
                    {
                        viewID = Horse.gameObject.GetPhotonView().ViewID;
                    }
                    Cache.PhotonView.RPC("LoadSkinRPC", RpcTarget.AllBuffered, new object[] { viewID, url });
                }
            }
        }

        [PunRPC]
        public void LoadSkinRPC(int horse, string url, PhotonMessageInfo info)
        {
            if (info.Sender != photonView.Owner)
                return;
            HumanCustomSkinSettings settings = SettingsManager.CustomSkinSettings.Human;
            if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || photonView.IsMine))
            {
                StartCoroutine(_customSkinLoader.LoadSkinsFromRPC(new object[] { horse, url }));
            }
        }

        [PunRPC]
        public void SetHookStateRPC(bool left, int hookId, int state, PhotonMessageInfo info)
        {
            if (left)
                HookLeft.Hooks[hookId].OnSetHookState(state, info);
            else
                HookRight.Hooks[hookId].OnSetHookState(state, info);
        }

        [PunRPC]
        public void SetHookingRPC(bool left, int hookId, Vector3 baseVelocity, Vector3 relativeVelocity, PhotonMessageInfo info)
        {
            if (left)
                HookLeft.Hooks[hookId].OnSetHooking(baseVelocity, relativeVelocity, info);
            else
                HookRight.Hooks[hookId].OnSetHooking(baseVelocity, relativeVelocity, info);
        }

        [PunRPC]
        public void SetHookedRPC(bool left, int hookId, Vector3 position, int viewId, int objectId, PhotonMessageInfo info)
        {
            if (left)
                HookLeft.Hooks[hookId].OnSetHooked(position, viewId, objectId, info);
            else
                HookRight.Hooks[hookId].OnSetHooked(position, viewId, objectId, info);
        }

        [PunRPC]
        public void SetSmokeRPC(bool active, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            var emission = HumanCache.Smoke.emission;
            emission.enabled = active;
        }

        protected void ToggleSparks(bool toggle)
        {
            ToggleSound(HumanSounds.Slide, toggle);
            if (toggle != HumanCache.Sparks.emission.enabled)
                Cache.PhotonView.RPC("ToggleSparksRPC", RpcTarget.All, new object[] { toggle });
        }

        [PunRPC]
        protected void ToggleSparksRPC(bool toggle, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            var emission = HumanCache.Sparks.emission;
            emission.enabled = toggle;
        }

        public void SetThunderspears(bool hasLeft, bool hasRight)
        {
            photonView.RPC("SetThunderspearsRPC", RpcTarget.All, new object[] { hasLeft, hasRight });
        }

        [PunRPC]
        public void SetThunderspearsRPC(bool hasLeft, bool hasRight, PhotonMessageInfo info)
        {
            if (info.Sender != photonView.Owner)
                return;
            if (Setup._part_blade_l != null)
                Setup._part_blade_l.SetActive(hasLeft);
            if (Setup._part_blade_r != null)
                Setup._part_blade_r.SetActive(hasRight);
        }

        public void OnHooked(bool left, Vector3 position)
        {
            if (left)
            {
                _launchLeft = true;
                _launchLeftTime = 0f;
            }
            else
            {
                _launchRight = true;
                _launchRightTime = 0f;
            }
            if (State == HumanState.Grab || State == HumanState.Reload || MountState == HumanMountState.MapObject
                || State == HumanState.Stun)
                return;
            if (MountState == HumanMountState.Horse)
                Unmount(true);
            if (CarryState == HumanCarryState.Carry)
                Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
            if (State != HumanState.Attack && State != HumanState.SpecialAttack)
                Idle();
            Vector3 v = (position - Cache.Transform.position).normalized * 20f;
            if (IsHookedLeft() && IsHookedRight())
                v *= 0.8f;
            if (State != HumanState.SpecialAttack)
            {
                FalseAttack();
                Idle();
                if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                    CrossFade(HumanAnimations.AHSSHookForwardBoth, 0.1f);
                else if (left && !IsHookedRight())
                    CrossFade(HumanAnimations.AirHookLJust, 0.1f);
                else if (!left && !IsHookedLeft())
                    CrossFade(HumanAnimations.AirHookRJust, 0.1f);
                else
                {
                    CrossFade(HumanAnimations.Dash, 0.1f);
                }
            }
            Vector3 force = v;
            if (v.y < 30f)
                force += Vector3.up * (30f - v.y);
            if (position.y >= Cache.Transform.position.y)
                force += Vector3.up * (position.y - Cache.Transform.position.y) * 10f;
            Cache.Rigidbody.AddForce(force);
            TargetAngle = Mathf.Atan2(force.x, force.z) * Mathf.Rad2Deg;
            _targetRotation = GetTargetRotation();
            Cache.Transform.rotation = _targetRotation;
            Cache.Rigidbody.rotation = _targetRotation;
            ToggleSparks(false);
            _cancelGasDisable = true;
        }

        public void OnHookedHuman(bool left, Vector3 position, Human human)
        {
            if (State == HumanState.Grab || MountState == HumanMountState.MapObject || State == HumanState.Stun)
                return;
            if (!human.Dead && human != this)
            {
                _hookHuman = human;
                _hookHumanLeft = left;
                human.Cache.PhotonView.RPC("OnHookedByHuman", human.Cache.PhotonView.Owner, new object[] { Cache.PhotonView.ViewID });
                Vector3 launchForce = position - Cache.Transform.position;
                float num = Mathf.Pow(launchForce.magnitude, 0.1f);
                if (Grounded)
                    Cache.Rigidbody.AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, (10f)), ForceMode.Impulse);
                Cache.Rigidbody.AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
                _hookHumanConstantTimeLeft = 1f;
            }
        }

        public void UnhookHuman(bool left)
        {
            if (left == _hookHumanLeft)
                _hookHuman = null;
        }

        [PunRPC]
        public void OnHookedByHuman(int viewId, PhotonMessageInfo info)
        {
            var human = Util.FindCharacterByViewId(viewId);
            if (IsMine() && human != null && !Dead && human.Cache.PhotonView.Owner == info.Sender &&
                State != HumanState.Grab && CarryState != HumanCarryState.Carry && MountState == HumanMountState.None && human != this)
            {
                Vector3 direction = human.Cache.Transform.position - Cache.Transform.position;
                float loss = CharacterData.HumanWeaponInfo["Hook"]["InitialVelocityLoss"].AsFloat;
                Cache.Rigidbody.AddForce(-Cache.Rigidbody.velocity * loss, ForceMode.VelocityChange);
                float num = Mathf.Pow(direction.magnitude, 0.1f);
                if (Grounded)
                    Cache.Rigidbody.AddForce(Vector3.up * Mathf.Min(direction.magnitude * 0.2f, 10f), ForceMode.Impulse);
                Cache.Rigidbody.AddForce(direction * num * CharacterData.HumanWeaponInfo["Hook"]["InitialPullForce"].AsFloat, ForceMode.Impulse);
                CrossFade(HumanAnimations.Dash, 0.05f, 0.1f / Cache.Animation[HumanAnimations.Dash].length);
                State = HumanState.Stun;
                _stateTimeLeft = CharacterData.HumanWeaponInfo["Hook"]["StunTime"].AsFloat;
                FalseAttack();
                float facingDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                Cache.Rigidbody.rotation = quaternion;
                Cache.Transform.rotation = quaternion;
                _targetRotation = quaternion;
                TargetAngle = facingDirection;
            }
        }

        [PunRPC]
        public void OnStillHookedByHuman(int viewId, PhotonMessageInfo info)
        {
            var human = Util.FindCharacterByViewId(viewId);
            if (IsMine() && human != null && !Dead && human.Cache.PhotonView.Owner == info.Sender &&
                State != HumanState.Grab && CarryState != HumanCarryState.Carry && MountState == HumanMountState.None && human != this)
            {
                float loss = CharacterData.HumanWeaponInfo["Hook"]["ConstantVelocityLoss"].AsFloat;
                Cache.Rigidbody.AddForce(-Cache.Rigidbody.velocity * loss, ForceMode.VelocityChange);
                float constantPullForce = CharacterData.HumanWeaponInfo["Hook"]["ConstantPullForce"].AsFloat;
                if (constantPullForce > 0f)
                {
                    Vector3 direction = human.Cache.Transform.position - Cache.Transform.position;
                    float num = Mathf.Pow(direction.magnitude, 0.1f);
                    Cache.Rigidbody.AddForce(direction * num * constantPullForce, ForceMode.Impulse);
                }
            }
        }

        public void GetStunnedByTS(Vector3 origin)
        {
            Vector3 direction = Cache.Transform.position - origin;
            Cache.Rigidbody.AddForce(direction.normalized * CharacterData.HumanWeaponInfo["Thunderspear"]["StunForce"].AsFloat, ForceMode.VelocityChange);
            CrossFade(HumanAnimations.Dash, 0.05f, 0.1f / Cache.Animation[HumanAnimations.Dash].length);
            State = HumanState.Stun;
            _stateTimeLeft = CharacterData.HumanWeaponInfo["Thunderspear"]["StunDuration"].AsFloat;
            FalseAttack();
            float facingDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
            Cache.Rigidbody.rotation = quaternion;
            Cache.Transform.rotation = quaternion;
            _targetRotation = quaternion;
            TargetAngle = facingDirection;
            UseGas(Mathf.Min(MaxGas * CharacterData.HumanWeaponInfo["Thunderspear"]["StunGasPenalty"].AsFloat, 100));
            ((InGameMenu)UIManager.CurrentMenu).HUDBottomHandler.ShakeGas();
            EffectSpawner.Spawn(EffectPrefabs.GasBurst, Cache.Transform.position, Cache.Transform.rotation);
            PlaySound(HumanSounds.GasBurst);
            ((InGameCamera)SceneLoader.CurrentCamera).StartShake();
        }

        public void SetInterpolation(bool interpolate)
        {
            if (IsMine() && interpolate && SettingsManager.GraphicsSettings.InterpolationEnabled.Value)
                Cache.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            else
                Cache.Rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        private void SetTriggerCollider(bool trigger)
        {
            if (IsMine())
            {
                _isTrigger = trigger;
                Cache.PhotonView.RPC("SetTriggerColliderRPC", RpcTarget.All, new object[] { trigger });
            }
        }

        [PunRPC]
        public void SetTriggerColliderRPC(bool trigger, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            GetComponent<CapsuleCollider>().isTrigger = trigger;
        }

        private float GetReelAxis()
        {
            if (ReelInAxis != 0f)
                return ReelInAxis;
            return ReelOutAxis;
        }

        private float GetLeanAngle(Vector3 hookPosition, bool left)
        {
            if (Setup.Weapon != HumanWeapon.AHSS && Setup.Weapon != HumanWeapon.APG && State == HumanState.Attack)
                return 0f;
            float height = hookPosition.y - Cache.Transform.position.y;
            float dist = Vector3.Distance(hookPosition, Cache.Transform.position);
            float angle = Mathf.Acos(height / dist) * Mathf.Rad2Deg * 0.1f * (1f + Mathf.Pow(Cache.Rigidbody.velocity.magnitude, 0.2f));
            Vector3 v = hookPosition - Cache.Transform.position;
            float current = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
            float target = Mathf.Atan2(Cache.Rigidbody.velocity.x, Cache.Rigidbody.velocity.z) * Mathf.Rad2Deg;
            float delta = Mathf.DeltaAngle(current, target);
            angle += Mathf.Abs(delta * 0.5f);
            if (State != HumanState.Attack)
                angle = Mathf.Min(angle, 80f);
            _leanLeft = delta > 0f;
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                return angle * (delta >= 0f ? 1f : -1f);
            float multiplier = 0.5f;
            if ((left && delta < 0f) || (!left && delta > 0f))
                multiplier = 0.1f;
            return angle * (delta >= 0f ? multiplier : -multiplier);
        }

        public bool CanBladeAttack()
        {
            return Weapon is BladeWeapon && Weapon.CanUse();
        }

        public void StartSpecialAttack(string animation)
        {
            if (State == HumanState.Attack || State == HumanState.SpecialAttack)
                FalseAttack();
            PlayAnimation(animation);
            State = HumanState.SpecialAttack;
            ToggleSparks(false);
        }

        public void ActivateBlades()
        {
            if (!HumanCache.BladeHitLeft.IsActive())
            {
                HumanCache.BladeHitLeft.Activate();
                ToggleBladeTrails(true);
            }
            if (!HumanCache.BladeHitRight.IsActive())
            {
                HumanCache.BladeHitRight.Activate();
                ToggleBladeTrails(true);
            }
        }

        public void StartBladeSwing()
        {
            if (!Grounded && (HookLeft.IsHooked() || HookRight.IsHooked()))
            {
                if (SettingsManager.InputSettings.General.Left.GetKey())
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookL1 : HumanAnimations.Attack1HookL2;
                else if (SettingsManager.InputSettings.General.Right.GetKey())
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookR1 : HumanAnimations.Attack1HookR2;
                else if (_leanLeft)
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookL1 : HumanAnimations.Attack1HookL2;
                else
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookR1 : HumanAnimations.Attack1HookR2;
            }
            /*else if (_state == HumanState.MountingHorse) // Added and commented out by Ata 2 May 24 //
            {
                if (SettingsManager.InputSettings.General.Left.GetKey())
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookL1 : HumanAnimations.Attack1HookL2;
                else if (SettingsManager.InputSettings.General.Right.GetKey())
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookR1 : HumanAnimations.Attack1HookR2;
                else if (_leanLeft)
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookL1 : HumanAnimations.Attack1HookL2;
                else
                    AttackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HumanAnimations.Attack1HookR1 : HumanAnimations.Attack1HookR2;
            }*/
            else if (SettingsManager.InputSettings.General.Left.GetKey())
                AttackAnimation = HumanAnimations.Attack2;
            else if (SettingsManager.InputSettings.General.Right.GetKey())
                AttackAnimation = HumanAnimations.Attack1;
            else
            {
                BaseTitan titan = FindNearestTitan();
                if (titan != null)
                    AttackAnimation = GetBladeAnimationTarget(titan.BaseTitanCache.Neck);
                else
                    AttackAnimation = GetBladeAnimationMouse();
            }
            if (Grounded)
            {
                Cache.Rigidbody.AddForce(Cache.Transform.forward * 200f);
            }
            PlayAnimationReset(AttackAnimation);
            _attackButtonRelease = false;
            State = HumanState.Attack;
            if (Grounded)
            {
                _attackRelease = true;
                _attackButtonRelease = true;
            }
            else
                _attackRelease = false;
            ToggleSparks(false);
        }

        private string GetBladeAnimationMouse()
        {
            if (Input.mousePosition.x < (Screen.width * 0.5))
                return HumanAnimations.Attack2;
            else
                return HumanAnimations.Attack1;
        }

        private string GetBladeAnimationTarget(Transform target)
        {
            Vector3 v = target.position - Cache.Transform.position;
            float current = -Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
            float delta = -Mathf.DeltaAngle(current, Cache.Transform.rotation.eulerAngles.y - 90f);
            if (((Mathf.Abs(delta) < 90f) && (v.magnitude < 6f)) && ((target.position.y <= (Cache.Transform.position.y + 2f)) && (target.position.y >= (Cache.Transform.position.y - 5f))))
                return HumanAnimations.Attack4;
            else if (delta > 0f)
                return HumanAnimations.Attack1;
            else
                return HumanAnimations.Attack2;
        }

        private BaseTitan FindNearestTitan()
        {
            float nearestDistance = float.PositiveInfinity;
            BaseTitan nearestTitan = null;
            foreach (BaseTitan titan in _inGameManager.Titans)
            {
                float distance = Vector3.Distance(Cache.Transform.position, titan.Cache.Transform.position);
                if (distance < nearestDistance)
                {
                    nearestTitan = titan;
                    nearestDistance = distance;
                }
            }
            foreach (BaseTitan titan in _inGameManager.Shifters)
            {
                float distance = Vector3.Distance(Cache.Transform.position, titan.Cache.Transform.position);
                if (distance < nearestDistance)
                {
                    nearestTitan = titan;
                    nearestDistance = distance;
                }
            }
            return nearestTitan;
        }

        private Human FindNearestHuman()
        {
            float nearestDistance = float.PositiveInfinity;
            Human nearestHuman = null;
            foreach (Human human in _inGameManager.Humans)
            {
                if(human != this && TeamInfo.SameTeam(human, Team))
                {
                    float distance = Vector3.Distance(Cache.Transform.position, human.Cache.Transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestHuman = human;
                        nearestDistance = distance;
                    }
                }
            }
            return nearestHuman;
        }

        private void FalseAttack()
        {
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.Thunderspear || Setup.Weapon == HumanWeapon.APG)
            {
                if (!_attackRelease)
                    _attackRelease = true;
            }
            else
            {
                ToggleBladeTrails(false);
                HumanCache.BladeHitLeft.Deactivate();
                HumanCache.BladeHitRight.Deactivate();
                if (!_attackRelease)
                {
                    ContinueAnimation();
                    _attackRelease = true;
                }
            }
        }

        private void ContinueAnimation()
        {
            if (!_animationStopped)
                return;
            _animationStopped = false;
            Cache.PhotonView.RPC("ContinueAnimationRPC", RpcTarget.All, new object[0]);
        }

        [PunRPC]
        public void ContinueAnimationRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            foreach (AnimationState animation in Cache.Animation)
            {
                animation.speed = 1f;
            }
            CustomAnimationSpeed();
            string animationName = GetCurrentAnimation();
            if (animationName != "")
                PlayAnimation(animationName);
        }

        private void PauseAnimation()
        {
            if (_animationStopped)
                return;
            _animationStopped = true;
            Cache.PhotonView.RPC("PauseAnimationRPC", RpcTarget.All, new object[0]);
        }

        [PunRPC]
        public void PauseAnimationRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            foreach (AnimationState animation in Cache.Animation)
                animation.speed = 0f;
        }

        private void CustomAnimationSpeed()
        {
            Cache.Animation[HumanAnimations.SpecialLevi].speed = 1.85f;
            Cache.Animation[HumanAnimations.ChangeBlade].speed = 1.2f;
            Cache.Animation[HumanAnimations.AirRelease].speed = 0.6f;
            Cache.Animation[HumanAnimations.ChangeBladeAir].speed = 0.8f;
            Cache.Animation[HumanAnimations.AHSSGunReloadBoth].speed = 0.38f;
            Cache.Animation[HumanAnimations.AHSSGunReloadBothAir].speed = 0.5f;
            Cache.Animation[HumanAnimations.SpecialShifter].speed = 0.3f;
            if (Setup.Weapon == HumanWeapon.Thunderspear)
            {
                Cache.Animation[HumanAnimations.AHSSGunReloadBoth].speed = 0.76f;
                Cache.Animation[HumanAnimations.AHSSGunReloadBothAir].speed = 1f;
            }
        }

        private bool HasHook()
        {
            return HookLeft.HasHook() || HookRight.HasHook();
        }

        private bool IsHookedAny()
        {
            return IsHookedLeft() || IsHookedRight();
        }

        private bool IsHookedLeft()
        {
            return HookLeft.IsHooked();
        }

        private bool IsHookedRight()
        {
            return HookRight.IsHooked();
        }

        private bool IsFrontGrounded()
        {
            return CheckRaycastIgnoreTriggers(Cache.Transform.position + Cache.Transform.up * 1f, Cache.Transform.forward, 1f, GroundMask.value);
        }

        private bool IsPressDirectionTowardsHero()
        {
            if (!HasDirection)
                return false;
            return (Mathf.Abs(Mathf.DeltaAngle(TargetAngle, Cache.Transform.rotation.eulerAngles.y)) < 45f);
        }

        private bool IsUpFrontGrounded()
        {
            return CheckRaycastIgnoreTriggers(Cache.Transform.position + Cache.Transform.up * 3f, Cache.Transform.forward, 1.2f, GroundMask.value);
        }

        public bool IsFiringThunderspear()
        {
            return Setup.Weapon == HumanWeapon.Thunderspear && (Cache.Animation.IsPlaying(HumanAnimations.TSShootL) || Cache.Animation.IsPlaying(HumanAnimations.TSShootR) || Cache.Animation.IsPlaying(HumanAnimations.TSShootLAir) || Cache.Animation.IsPlaying(HumanAnimations.TSShootRAir));
        }

        public void UseGas(float amount)
        {
            CurrentGas -= amount;
            CurrentGas = Mathf.Max(CurrentGas, 0f);
        }

        private void ToggleBladeTrails(bool toggle)
        {
            if (IsMine())
                Cache.PhotonView.RPC("ToggleBladeTrailsRPC", RpcTarget.All, new object[] { toggle });
        }

        private void ToggleBlades(bool toggle)
        {
            if (IsMine())
                Cache.PhotonView.RPC("ToggleBladesRPC", RpcTarget.All, new object[] { toggle });
        }

        [PunRPC]
        protected void ToggleBladesRPC(bool toggle, PhotonMessageInfo info)
        {
            if (info.Sender != null && info.Sender != Cache.PhotonView.Owner)
                return;
            if (toggle)
            {
                Setup._part_blade_l.SetActive(true);
                Setup._part_blade_r.SetActive(true);
            }
            else
            {
                if (Setup.LeftTrail != null && Setup.RightTrail != null)
                {
                    Setup.LeftTrail.StopImmediate();
                    Setup.RightTrail.StopImmediate();
                }
                Setup._part_blade_l.SetActive(false);
                Setup._part_blade_r.SetActive(false);
            }
        }

        [PunRPC]
        protected void ToggleBladeTrailsRPC(bool toggle, PhotonMessageInfo info)
        {
            if (info.Sender != null && info.Sender != Cache.PhotonView.Owner)
                return;
            if (toggle && SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
            {
                Setup.LeftTrail.Emit = true;
                Setup.RightTrail.Emit = true;
                Setup.LeftTrail._emitTime = 0f;
                Setup.RightTrail._emitTime = 0f;
            }
            else
            {
                Setup.LeftTrail._emitTime = 0.1f;
                Setup.RightTrail._emitTime = 0.1f;
            }
        }

        protected override string GetFootstepAudio(int phase)
        {
            return phase == 0 ? HumanSounds.Footstep1 : HumanSounds.Footstep2;
        }

        protected override int GetFootstepPhase()
        {
            if (Cache.Animation.IsPlaying(HumanAnimations.Run) || Cache.Animation.IsPlaying(HumanAnimations.RunTS))
            {
                float time = Cache.Animation[HumanAnimations.Run].normalizedTime % 1f;
                return (time >= 0.1f && time < 0.6f) ? 1 : 0;
            }
            else if (Cache.Animation.IsPlaying(HumanAnimations.RunBuffed))
            {
                float time = Cache.Animation[HumanAnimations.RunBuffed].normalizedTime % 1f;
                return (time >= 0.1f && time < 0.6f) ? 1 : 0;
            }
            return _stepPhase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (HumanCache != null)
            {
                if (HumanCache.AHSSHit != null && HumanCache.AHSSHit.gameObject != null)
                    Destroy(HumanCache.AHSSHit.gameObject);
                if (HumanCache.APGHit != null && HumanCache.APGHit.gameObject != null)
                    Destroy(HumanCache.APGHit.gameObject);
            }
            if (Setup != null)
                Setup.DeleteDie();
        }

        protected override void CheckGround()
        {

            JustGrounded = false;
            if (CheckRaycastIgnoreTriggers(Cache.Transform.position + Vector3.up * 0.1f, -Vector3.up, GroundDistance, GroundMask.value))
            {
                if (!Grounded)
                    Grounded = JustGrounded = true;
            }
            else if (_needLean && (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG))
            {
                if (CheckRaycastIgnoreTriggers(HumanCache.GroundLeft.position + Vector3.up * 0.1f, -Vector3.up, GroundDistance, GroundMask.value))
                {
                    if (!Grounded)
                        Grounded = JustGrounded = true;
                }
                else if (CheckRaycastIgnoreTriggers(HumanCache.GroundRight.position + Vector3.up * 0.1f, -Vector3.up, GroundDistance, GroundMask.value))
                {
                    if (!Grounded)
                        Grounded = JustGrounded = true;
                }
                else
                    Grounded = false;
            }
            else
                Grounded = false;
        }

        protected override List<Renderer> GetFPSDisabledRenderers()
        {
            List<Renderer> renderers = new List<Renderer>();
            if (FinishSetup)
            {
                AddRendererIfExists(renderers, Setup._part_head);
                AddRendererIfExists(renderers, Setup._part_hair);
                AddRendererIfExists(renderers, Setup._part_eye);
                AddRendererIfExists(renderers, Setup._part_glass);
                AddRendererIfExists(renderers, Setup._part_face);
                AddRendererIfExists(renderers, Setup._part_3dmg);
                AddRendererIfExists(renderers, Setup._part_belt);
                AddRendererIfExists(renderers, Setup._part_gas_l);
                AddRendererIfExists(renderers, Setup._part_gas_r);
                AddRendererIfExists(renderers, Setup._part_chest_1);
                AddRendererIfExists(renderers, Setup._part_chest_2);
            }
            return renderers;
        }

        protected override List<SkinnedMeshRenderer> GetFPSDisabledSkinnedRenderers()
        {
            List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();
            AddSkinnedRendererIfExists(renderers, Setup._part_upper_body);
            AddSkinnedRendererIfExists(renderers, Setup._part_chest);
            AddSkinnedRendererIfExists(renderers, Setup._part_brand_1);
            AddSkinnedRendererIfExists(renderers, Setup._part_brand_2);
            AddSkinnedRendererIfExists(renderers, Setup._part_brand_3);
            AddSkinnedRendererIfExists(renderers, Setup._part_brand_4);
            AddSkinnedRendererIfExists(renderers, Setup._part_arm_l);
            AddSkinnedRendererIfExists(renderers, Setup._part_arm_r);
            AddSkinnedRendererIfExists(renderers, Setup._part_leg);
            return renderers;
        }

        protected override List<SkinnedMeshRenderer> GetFPSDisabledClothRenderers()
        {
            List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();
            AddSkinnedRendererIfExists(renderers, Setup._part_hair_1);
            AddSkinnedRendererIfExists(renderers, Setup._part_cape);
            AddSkinnedRendererIfExists(renderers, Setup._part_chest_3);
            return renderers;
        }


        public HumanState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state == HumanState.AirDodge || _state == HumanState.GroundDodge)
                    _dashTimeLeft = 0f;
                _state = value;
            }
        }


        [PunRPC]
        public void moveToRPC(float x, float y, float z, PhotonMessageInfo info)
        {
            if (info.Sender.IsMasterClient)
            {
                if (MountState == HumanMountState.Horse) Horse.transform.position = new Vector3(x, y, z);
                else transform.position = new Vector3(x, y, z);
            }
        }
        #region EXPEDITION EXTENSION HUMAN

        //THUNDERSPEAR EDIT
        public void DieToTS() //Added by Momo Dec 6 2023 to kill people too close to the explosion. and print 100 damage.
        {
            GetHit("Thunderspear", 100, "Thunderspear", "");
            Die();
        }
        #endregion
    }

    public enum HumanState
    {
        Idle,
        Attack,
        GroundDodge,
        AirDodge,
        Reload,
        Refill,
        Die,
        Grab,
        EmoteAction,
        SpecialAttack,
        SpecialAction,
        Slide,
        Run,
        Land,
        MountingHorse,
        Stun
    }

    public enum HumanMountState
    {
        None,
        Horse,
        MapObject
    }

    public enum HumanCarryState
    {
        None,
        Carry
    }

    public enum HumanWeapon
    {
        Blade,
        AHSS,
        Thunderspear,
        APG
    }

    public enum HumanDashDirection
    {
        None,
        Forward,
        Back,
        Left,
        Right
    }
}
