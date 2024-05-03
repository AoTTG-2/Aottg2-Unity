using Settings;
using UnityEngine;
using Photon;
using Characters;
using System.Collections.Generic;
using System.Collections;
using Effects;
using ApplicationManagers;
using GameManagers;
using UI;
using Utility;
using CustomLogic;
using Cameras;
using Photon.Pun;

namespace Projectiles
{
    class ThunderspearProjectile : BaseProjectile
    {
        Color _color;
        float _radius;
        public Vector3 InitialPlayerVelocity;
        Vector3 _lastPosition;
        static LayerMask _collideMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles,
            PhysicsLayer.TitanPushbox);
        static LayerMask _blockMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles,
            PhysicsLayer.TitanPushbox, PhysicsLayer.Human);
        //Expedition Extension
        float falloff = 1f; //Added by Sysyfus for damage calculation
        bool gravity = true; //Added by Sysyfus for attaching to titan/object
        bool attached = false; //Added by Sysyfus for attaching to titan/object
        GameObject attachParent = null;
        Collider attachCollider = null;
        Vector3 relativeAttachPoint = new Vector3(0f, 0f, 0f);
        AudioSource tsCharge;

        protected override void SetupSettings(object[] settings)
        {
            _radius = (float)settings[0];
            _color = (Color)settings[1];
            _lastPosition = transform.position;
        }

        protected override void RegisterObjects()
        {
            var trail = transform.Find("Trail").GetComponent<ParticleSystem>();
            var flame = transform.Find("Flame").GetComponent<ParticleSystem>();
            var model = transform.Find("ThunderspearModel").gameObject;
            _hideObjects.Add(flame.gameObject);
            _hideObjects.Add(model);
            if (SettingsManager.AbilitySettings.ShowBombColors.Value)
            {
                var main = trail.main;
                main.startColor = _color;
                main = flame.main;
                main.startColor = _color;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (photonView.IsMine && !Disabled)
            {
                if (CharacterData.HumanWeaponInfo["Thunderspear"]["Ricochet"].AsBool)
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * _velocity.magnitude * CharacterData.HumanWeaponInfo["Thunderspear"]["RicochetSpeed"].AsFloat;
                else
                {
                    //Explode(); //removed by Sysyfus Dec 20 2023 for sticky TS
                    if (!attached)
                        Attach(collision);
                    _rigidbody.velocity = Vector3.zero;
                }
            }
        }

        protected override void OnExceedLiveTime()
        {
            Explode();
        }

        public void Explode()
        {
            if (!Disabled)
            {
                float effectRadius = _radius * 5f;
                if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                    effectRadius = _radius * 2f;
                bool killedPlayer = KillPlayersInRadius(_radius);
                bool killedTitan = KillTitansInRadius(_radius);
                EffectSpawner.Spawn(EffectPrefabs.ThunderspearExplode, transform.position, transform.rotation, effectRadius, true, new object[] { _color, killedPlayer || killedTitan });
                StunMyHuman();
                DestroySelf();
                KillMyHuman(); //Added by Momo Dec 6 2023 to kill people too close to the explosion.
                photonView.RPC("StopChargeEffectRPC", RpcTarget.AllViaServer, new object[0]); //Added by Sysyfus Jan 4 2024
                gravity = false;
            }
        }

        void StunMyHuman()
        {
            if (_owner == null || !(_owner is Human) || !_owner.IsMainCharacter())
                return;
            if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                return;
            float radius = CharacterData.HumanWeaponInfo["Thunderspear"]["StunBlockRadius"].AsFloat;
            float range = CharacterData.HumanWeaponInfo["Thunderspear"]["StunRange"].AsFloat;
            Vector3 direction = _owner.Cache.Transform.position - transform.position;
            RaycastHit hit;
            if (Vector3.Distance(_owner.Cache.Transform.position, transform.position) < range)
            {
                if (Physics.SphereCast(transform.position, radius, direction.normalized, out hit, range, _blockMask))
                {
                    if (hit.collider.transform.root.gameObject == _owner.gameObject)
                        ((Human)_owner).GetStunnedByTS(transform.position);
                }
            }
        }

        bool KillTitansInRadius(float radius)
        {
            var position = transform.position;
            var colliders = Physics.OverlapSphere(position, radius, PhysicsLayer.GetMask(PhysicsLayer.Hurtbox));
            bool killedTitan = false;
            foreach (var collider in colliders)
            {
                var titan = collider.transform.root.gameObject.GetComponent<BaseTitan>();
                var handler = collider.gameObject.GetComponent<CustomLogicCollisionHandler>();
                if (handler != null)
                {
                    handler.GetHit(_owner, "Thunderspear", 100, "Thunderspear");
                    continue;
                }
                if (titan != null && titan != _owner && !TeamInfo.SameTeam(titan, _team) && !titan.Dead)
                {
                    if (collider == titan.BaseTitanCache.NapeHurtbox && CheckTitanNapeAngle(position, titan.BaseTitanCache.Head))
                    {
                        if (_owner == null || !(_owner is Human))
                            titan.GetHit("Thunderspear", 100, "Thunderspear", collider.name);
                        else
                        {
                            var damage = CalculateDamage4(titan, radius, collider); //changed by Sysyfus Dec 21 2023 to CalculateDamage4 //changed by Sysyfus Dec 20 2023 to CalculateDamage3 //changed by Sysyfus Dec 6 2023 from CalculateDamage() to CalculateDamage2()
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                            ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(titan.BaseTitanCache.Neck.position, damage);
                            titan.GetHit(_owner, damage, "Thunderspear", collider.name); //removed by Sysyfus Dec 20 2023 to accommodate accuracy tier damage
                        }
                        killedTitan = true;
                    }
                }
            }
            return killedTitan;
        }

        bool KillPlayersInRadius(float radius)
        {
            var gameManager = (InGameManager)SceneLoader.CurrentGameManager;
            var position = transform.position;
            bool killedHuman = false;
            foreach (Human human in gameManager.Humans)
            {
                if (human == null || human.Dead)
                    continue;
                if (Vector3.Distance(human.Cache.Transform.position, position) < radius && human != _owner && !TeamInfo.SameTeam(human, _team))
                {
                    if (_owner == null || !(_owner is Human))
                        human.GetHit("", 100, "Thunderspear", "");
                    else
                    {
                        var damage = CalculateDamage();
                        human.GetHit(_owner, damage, "Thunderspear", "");
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        ((InGameCamera)SceneLoader.CurrentCamera).TakeSnapshot(human.Cache.Transform.position, damage);
                    }
                    killedHuman = true;
                }
            }
            return killedHuman;
        }

        int CalculateDamage()
        {
            int damage = Mathf.Max((int)(InitialPlayerVelocity.magnitude * 10f * 
                CharacterData.HumanWeaponInfo["Thunderspear"]["DamageMultiplier"].AsFloat), 10);
            if (_owner != null && _owner is Human)
            {
                var human = (Human)_owner;
                if (human.CustomDamageEnabled)
                    return human.CustomDamage;
            }
            return damage;
        }

        bool CheckTitanNapeAngle(Vector3 position, Transform nape)
        {
            Vector3 direction = (position - nape.position).normalized;
            return Vector3.Angle(-nape.forward, direction) < CharacterData.HumanWeaponInfo["Thunderspear"]["RestrictAngle"].AsFloat;
        }

        protected override void Update()
        {
            base.Update();
            if (_photonView.IsMine)
            {
                if (GetComponent<Rigidbody>().velocity.magnitude > 0f)
                    transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
            }
        }

        protected void FixedUpdate()
        {

            if (_photonView.IsMine)
            {
                if (!attached)
                {
                    GetComponent<Rigidbody>().velocity *= 0.94f; //added by Sysyfus Dec 6 2023 to simulate wind resistance
                    //FixedUpdateInWater(); //added by Sysyfus Jan 9 2024
                    if (gravity)
                        GetComponent<Rigidbody>().velocity -= new Vector3(0f, 3.2f, 0f); //added by Sysyfus Dec 6 2023 to simulate gravity

                    RaycastHit hit;
                    Vector3 direction = (transform.position - _lastPosition);
                    if (Physics.SphereCast(_lastPosition, 0.5f, direction.normalized, out hit, direction.magnitude, _collideMask))
                    {
                        transform.position = hit.point;
                        Attach(hit);
                    }
                    _lastPosition = transform.position;
                }
                else
                    transform.position = GetAttachedPosition(); //Changed by Sysyfus Dec 20 2023 for sticky TS
            }
        }

        #region EXPEDITION EXTENSION TS
        void KillMyHuman()
        {
            if (_owner == null || !(_owner is Human) || !_owner.IsMainCharacter())
                return;
            if (SettingsManager.InGameCurrent.Misc.ThunderspearPVP.Value)
                return;
            float radius = CharacterData.HumanWeaponInfo["Thunderspear"]["StunBlockRadius"].AsFloat / 1.6f;
            float range = CharacterData.HumanWeaponInfo["Thunderspear"]["StunRange"].AsFloat / 1.6f;
            Vector3 direction = _owner.Cache.Transform.position - transform.position;
            RaycastHit hit;
            if (Vector3.Distance(_owner.Cache.Transform.position, transform.position) < range)
            {
                if (Physics.SphereCast(transform.position, radius, direction.normalized, out hit, range, _blockMask))
                {
                    if (hit.collider.transform.root.gameObject == _owner.gameObject)
                    {
                        ((Human)_owner).DieToTS();
                    }
                }
            }
        }
        int CalculateDamage4(BaseTitan titan, float radius, Collider collider)
        {
            int damage = Mathf.Max((int)(InitialPlayerVelocity.magnitude * 10f *
                CharacterData.HumanWeaponInfo["Thunderspear"]["DamageMultiplier"].AsFloat), 100);
            if (_owner != null && _owner is Human)
            {
                var human = (Human)_owner;
                if (human.CustomDamageEnabled)
                    return human.CustomDamage;
            }

            float distanceRatio = Vector3.Distance(this.transform.position, collider.transform.position) / radius; //how far hit point is from nape relative to explosion radius
            falloff = Mathf.Clamp(-1f * Mathf.Pow(1.1f * distanceRatio, 2) + 2.0625f, 0.5f, 1.5f); //falloff should not exceed +-50%, +50% at 0.6 distance ratio and -50% at 1.0 distance ratio

            damage = (int)((float)damage * falloff);

            return damage;
        }

        //added by Sysyfus Dec 20 2023
        //when raw damage insufficient to kill titan, hidden bonus damage may apply based on quality of aim
        void AdjustTitanHealth(BaseTitan titan, int damage, Collider collider)
        {
            int newHealth = titan.CurrentHealth - damage;

            int newDamage = 0;
            if (newHealth > 0) //if raw damage insufficient to kill titan, calculate % max HP damage
            {
                if (falloff >= 0.99f)    //A tier, good aim, instant kill regardless of damage 
                {
                    newDamage = titan.MaxHealth;
                }
                else if (falloff > 0.25f) //B tier, decent aim, bonus damage up to 50% of titan's maximum health
                {
                    newDamage = titan.MaxHealth / 2;
                }
                else //C tier, you barely hit the nape, bonus damage up to 25% of titan's maximum health
                {
                    newDamage = titan.MaxHealth / 4;
                }
            }

            if (damage < newDamage) //apply bonus damage if raw damage lower
            {
                newHealth = titan.CurrentHealth - newDamage;
            }

            if (newHealth < 0) // no negative HP >:(
            {
                newHealth = 0;
            }

            titan.GetHit(_owner, damage, "Thunderspear", collider.name); //still show the raw damage number in feed
            titan.SetCurrentHealth(newHealth); //make sure health adjusted to proper level
        }

        //added by Sysyfus Dec 20 2023 to make TS stick to surface before exploding
        void Attach(Collision collision)
        {
            this._timeLeft = 1f; //TS explodes after 1 second

            attachParent = collision.collider.gameObject;
            attachCollider = collision.collider;
            Vector3 point = attachCollider.ClosestPoint(collision.GetContact(0).point);
            relativeAttachPoint = point - attachCollider.transform.position;
            relativeAttachPoint = attachCollider.transform.InverseTransformPoint(point);
            //transform.position = attachCollider.transform.position + relativeAttachPoint;
            transform.position = attachCollider.transform.TransformPoint(relativeAttachPoint);

            //transform.SetParent(attachCollider.transform);;

            photonView.RPC("PlayChargeEffectRPC", RpcTarget.AllViaServer, new object[0]);

            attached = true;

        }
        void Attach(RaycastHit hit)
        {
            this._timeLeft = 1f; //TS explodes after 1 second


            attachParent = hit.collider.gameObject;
            relativeAttachPoint = attachParent.transform.position - hit.point;
            transform.position = attachParent.transform.position + relativeAttachPoint;
            photonView.RPC("PlayChargeEffectRPC", RpcTarget.AllViaServer, new object[0]);
            attached = true;

        }


        private Vector3 GetAttachedPosition()
        {
            return attachCollider.transform.TransformPoint(relativeAttachPoint);
            //return attachCollider.transform.position + relativeAttachPoint;
        }

        [PunRPC]
        public void PlayChargeEffectRPC(PhotonMessageInfo info)
        {
            tsCharge.Play();
        }
        [PunRPC]
        public void StopChargeEffectRPC(PhotonMessageInfo info)
        {
            tsCharge.Stop();
        }
        #endregion
    }
}
