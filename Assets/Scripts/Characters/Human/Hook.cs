using ApplicationManagers;
using CustomLogic;
using Map;
using Photon.Pun;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Characters
{
    class Hook : MonoBehaviour
    {
        public HookState State = HookState.Disabled;
        public Transform Anchor;
        public BaseCharacter HookCharacter;
        public Transform HookParent;
        protected bool _hasHookParent;
        public LineRenderer _renderer;
        protected bool _left;
        protected Human _owner;
        protected int _id;
        protected List<Vector3> _nodes = new List<Vector3>();
        protected Vector3 _baseVelocity = Vector3.zero;
        protected Vector3 _relativeVelocity = Vector3.zero;
        protected Vector3 _hookPosition = Vector3.zero;
        protected Vector3 _lastWorldHookPosition = Vector3.zero;
        protected float _currentLiveTime = 0f;
        protected ParticleSystem _particles;
        protected GameObject _endSprite;
        private static LayerMask HookMask = PhysicsLayer.GetMask(PhysicsLayer.Human, PhysicsLayer.TitanPushbox, PhysicsLayer.EntityDetection,
            PhysicsLayer.MapObjectProjectiles, PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectAll);
        protected float _tiling;
        protected float _lastLength;
        protected float _maxLiveTime;

        public static Hook CreateHook(Human owner, bool left, int id, float maxLiveTime, bool gun = false)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(owner.transform);
            Hook hook = obj.AddComponent<Hook>();
            hook._left = left;
            hook._owner = owner;
            hook._maxLiveTime = maxLiveTime;
            hook._id = id;
            if (left)
            {
                if (gun)
                    hook.Anchor = owner.HumanCache.HookLeftAnchorGun;
                else
                    hook.Anchor = owner.HumanCache.HookLeftAnchorDefault;
            }
            else
            {
                if (gun)
                    hook.Anchor = owner.HumanCache.HookRightAnchorGun;
                else
                    hook.Anchor = owner.HumanCache.HookRightAnchorDefault;
            }
            return hook;
        }

        protected void Awake()
        {
            _renderer = gameObject.AddComponent<LineRenderer>();
            _renderer.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/BasicMaterial", true);
            _renderer.material.color = Color.black;
            _renderer.positionCount = 0;
            _particles = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Characters, "Human/Particles/Prefabs/HookParticle", true)
                .GetComponent<ParticleSystem>();
            _endSprite = _particles.transform.Find("HookEnd").gameObject;
        }

        public void SetSkin(float tiling)
        {
            _tiling = tiling;
        }

        private void UpdateSkin()
        {
            if (_tiling > 0f)
            {
                float ropeLength = (GetHookPosition() - Anchor.position).magnitude;
                if (ropeLength != _lastLength)
                {
                    _lastLength = ropeLength;
                    _renderer.material.mainTextureScale = new Vector2(_tiling * ropeLength, 1f);
                }
            }
        }

        public void OnSetHookState(int state, PhotonMessageInfo info)
        {
            if (info.Sender != _owner.Cache.PhotonView.Owner)
                return;
            State = (HookState)state;
            _currentLiveTime = 0f;
            if ((State == HookState.DisablingHooked || State == HookState.DisablingHooking) && SettingsManager.SoundSettings.HookRetractEffect.Value)
            {
                if (_left)
                    _owner.PlaySoundRPC(HumanSounds.HookRetractLeft, Util.CreateLocalPhotonInfo());
                else
                    _owner.PlaySoundRPC(HumanSounds.HookRetractRight, Util.CreateLocalPhotonInfo());
            }
            _endSprite.SetActive(false);
        }

        public void OnSetHooking(Vector3 baseVelocity, Vector3 relativeVelocity, PhotonMessageInfo info)
        {
            if (info.Sender != _owner.Cache.PhotonView.Owner)
                return;
            State = HookState.Hooking;
            _baseVelocity = baseVelocity;
            _relativeVelocity = relativeVelocity;
            _hookPosition = Anchor.position;
            _hasHookParent = false;
            _nodes.Clear();
            _currentLiveTime = 0f;
            _nodes.Add(_hookPosition);
            _renderer.startWidth = _renderer.endWidth = 0.1f;
            if (SettingsManager.SoundSettings.OldHookEffect.Value)
                _owner.PlaySoundRPC(HumanSounds.OldHookLaunch, Util.CreateLocalPhotonInfo());
            else
                _owner.PlaySoundRPC(HumanSounds.HookLaunch, Util.CreateLocalPhotonInfo());
            _particles.transform.position = GetHookPosition();
            _particles.transform.forward = -(_baseVelocity * 50f + _relativeVelocity).normalized;
            _particles.Stop();
            _particles.Play();
            _endSprite.SetActive(false);
        }

        public void OnSetHooked(Vector3 position, int photonViewId, int objectId, PhotonMessageInfo info)
        {
            if (info.Sender != _owner.Cache.PhotonView.Owner)
                return;
            Transform t = null;
            if (photonViewId != -1)
                t = PhotonView.Find(photonViewId).gameObject.transform;
            if (objectId != -1 && MapManager.MapLoaded)
                t = MapLoader.IdToMapObject[objectId].GameObject.transform;
            OnSetHooked(position, t);
        }

        private void OnSetHooked(Vector3 position, Transform transform)
        {
            State = HookState.Hooked;
            _hookPosition = position;
            HookParent = null;
            HookCharacter = null;
            _hasHookParent = false;
            if (transform != null)
            {
                HookParent = transform;
                _hookPosition = transform.InverseTransformPoint(position);
                _hasHookParent = true;
                _lastWorldHookPosition = position;
                HookCharacter = transform.root.GetComponent<BaseCharacter>();
            }
            _currentLiveTime = 0f;
            _renderer.startWidth = _renderer.endWidth = 0.1f;
            if (SettingsManager.SoundSettings.HookImpactEffect.Value)
            {
                if (HookCharacter != null && HookCharacter is Human)
                    _owner.PlaySoundRPC(HumanSounds.HookImpactLoud, Util.CreateLocalPhotonInfo());
                else
                    _owner.PlaySoundRPC(HumanSounds.HookImpact, Util.CreateLocalPhotonInfo());
            }
            _endSprite.SetActive(true);
        }

        public void SetHookState(HookState state)
        {
            _owner.Cache.PhotonView.RPC("SetHookStateRPC", RpcTarget.All, new object[] { _left, _id, (int)state });
        }

        public void SetHooking(Vector3 baseVelocity, Vector3 relativeVelocity)
        {
            _owner.Cache.PhotonView.RPC("SetHookingRPC", RpcTarget.All, new object[] { _left, _id, baseVelocity, relativeVelocity });
        }

        public void SetHooked(Vector3 position, Transform t = null, int viewId = -1, int objectId = -1)
        {
            _owner.Cache.PhotonView.RPC("SetHookedRPC", RpcTarget.Others, new object[] { _left, _id, position, viewId, objectId });
            OnSetHooked(position, t);
            _owner.OnHooked(_left, position);
            if (t != null && t.GetComponent<Human>() != null)
                _owner.OnHookedHuman(_left, position, t.GetComponent<Human>());
        }

        protected void FinishDisable()
        {
            _renderer.positionCount = 0;
            State = HookState.Disabled;
            _endSprite.SetActive(false);
        }

        protected void UpdateHooking()
        {
            if (_nodes.Count > 0)
            {
                Vector3 v = Anchor.position - _nodes[0];
                _renderer.positionCount = _nodes.Count;
                for (int i = 0; i < _nodes.Count; i++)
                    _renderer.SetPosition(i, _nodes[i] + v * Mathf.Pow(0.75f, i));
                if (_nodes.Count > 1)
                    _renderer.SetPosition(1, Anchor.position);
            }
        }

        protected void UpdateHooked()
        {
            Vector3 position = GetHookPosition();
            Vector3 v1 = position - Anchor.position;
            Vector3 velocity = _owner.Cache.Rigidbody.velocity;
            int vertex = (int)((v1.magnitude + velocity.magnitude) / 5f);
            vertex = Mathf.Clamp(vertex, 2, 6);
            _renderer.positionCount = vertex;
            _renderer.SetPosition(0, Anchor.position);
            float rndFactor = Mathf.Pow(v1.magnitude, 0.3f);
            for (int i = 1; i < vertex - 1; i++)
            {
                int midpoint = vertex / 2;
                float midDiff = Mathf.Abs((i - midpoint));
                float noise = (midpoint - midDiff) / (float)midpoint;
                noise = Mathf.Pow(noise, 0.5f);
                float max = ((rndFactor + velocity.magnitude) * 0.0015f) * noise;
                Vector3 noisePosition = Anchor.position + new Vector3(Random.Range(-max, max), Random.Range(-max, max), Random.Range(-max, max));
                noisePosition += (v1 * ((float)i / (float)vertex)) - (Vector3.up * rndFactor * 0.05f * noise) - (velocity * 0.001f * noise * rndFactor);
                _renderer.SetPosition(i, noisePosition);
            }
            _renderer.SetPosition(vertex - 1, position);
            _endSprite.transform.position = position + v1.normalized * 0.1f;
            _endSprite.transform.rotation = Quaternion.LookRotation(v1.normalized, (SceneLoader.CurrentCamera.Cache.Transform.position - _endSprite.transform.position).normalized);
        }

        protected void UpdateDisablingHooking()
        {
            Vector3 position = GetHookPosition();
            position += _baseVelocity + (_relativeVelocity * Time.deltaTime);
            _nodes.Add(position);
            Vector3 v = Anchor.position - _nodes[0];
            _renderer.positionCount = _nodes.Count;
            for (int i = 0; i < _nodes.Count; i++)
            {
                _renderer.SetPosition(i, _nodes[i] + (v * Mathf.Pow(0.75f, i)));
            }
            if (_nodes.Count > 1)
                _renderer.SetPosition(1, Anchor.position);
            _currentLiveTime += Time.deltaTime;
            float width = 0.1f - _currentLiveTime * 0.2f;
            _renderer.startWidth = _renderer.endWidth = width;
            if (_currentLiveTime > 0.5f)
            {
                FinishDisable();
            }
        }

        protected void UpdateDisablingHooked()
        {
            _renderer.positionCount = 2;
            _renderer.SetPosition(0, GetHookPosition());
            _renderer.SetPosition(1, Anchor.position);
            _currentLiveTime += Time.deltaTime;
            float width = 0.1f - _currentLiveTime * 0.2f;
            _renderer.startWidth = _renderer.endWidth = width;
            if (_currentLiveTime > 0.5f)
                FinishDisable();
        }

        protected void FixedUpdateHooking()
        {
            if (_owner.IsMine())
            {
                _hookPosition += _baseVelocity * Time.deltaTime * 50f + _relativeVelocity * Time.deltaTime;
                Vector3 start = _nodes[_nodes.Count - 1];
                if (_nodes.Count > 1)
                    start = _nodes[_nodes.Count - 2];
                Vector3 v = _hookPosition - start;
                RaycastHit[] hitArr = Physics.RaycastAll(start, v.normalized, v.magnitude, HookMask.value);
                System.Array.Sort(hitArr, (x, y) => x.distance.CompareTo(y.distance));
                if (hitArr.Length > 0)
                {
                    bool foundHit = false;
                    RaycastHit finalHit = hitArr[0];
                    foreach (RaycastHit hit in hitArr)
                    {
                        if (hit.collider.gameObject.layer == PhysicsLayer.EntityDetection)
                        {
                            hit.collider.gameObject.GetComponent<TitanEntityDetection>().RegisterHook(this);
                            continue;
                        }
                        if (hit.collider.gameObject == _owner.gameObject)
                            continue;
                        if (_owner.BackHuman != null && hit.collider.gameObject == _owner.BackHuman.gameObject)
                            continue;
                        if (_owner.CarryState == HumanCarryState.Carry && _owner.Carrier != null && hit.collider.gameObject == _owner.Carrier.gameObject)
                            continue;
                        finalHit = hit;
                        foundHit = true;
                        break;
                    }
                    if (foundHit)
                    {
                        GameObject obj = finalHit.collider.gameObject;
                        if (obj.layer == PhysicsLayer.Human || obj.layer == PhysicsLayer.TitanPushbox)
                        {
                            Vector3 point = finalHit.point;
                            if (obj.layer == PhysicsLayer.Human)
                                point = obj.transform.position + Vector3.up * 0.8f;
                            SetHooked(point, obj.transform, obj.transform.root.gameObject.GetPhotonView().ViewID);
                            return;
                        }
                        else
                        {
                            var root = obj.transform.root.gameObject;
                            if (MapLoader.GoToMapObject.ContainsKey(root))
                            {
                                MapObject mapObject = MapLoader.GoToMapObject[obj.transform.root.gameObject];
                                if (mapObject.ScriptObject.Static)
                                    SetHooked(finalHit.point + new Vector3(0f, 0.1f, 0f));
                                else
                                    SetHooked(finalHit.point, obj.transform, -1, mapObject.ScriptObject.Id);
                                var collisionHandler = mapObject.GameObject.GetComponent<CustomLogicCollisionHandler>();
                                if (collisionHandler != null)
                                    collisionHandler.GetHooked(_owner, finalHit.point, _left);
                            }
                            else
                                SetHooked(finalHit.point);
                            return;
                        }
                    }
                }
                _nodes.Add(_hookPosition);
                _currentLiveTime += Time.deltaTime;
                if (_currentLiveTime > _maxLiveTime)
                    SetHookState(HookState.DisablingHooking);
            }
            else
            {
                _hookPosition += _baseVelocity * Time.deltaTime * 50f + _relativeVelocity * Time.deltaTime;
                _nodes.Add(_hookPosition);
            }
        }

        protected void Update()
        {
            if (State == HookState.Hooking)
                UpdateHooking();
            else if (State == HookState.Hooked)
                UpdateHooked();
            else if (State == HookState.DisablingHooking)
                UpdateDisablingHooking();
            else if (State == HookState.DisablingHooked)
                UpdateDisablingHooked();
            if (State != HookState.Disabled)
                UpdateSkin();
        }


        protected void FixedUpdate()
        {
            if (State == HookState.Hooking)
                FixedUpdateHooking();
            if (State == HookState.Hooking || State == HookState.Hooked)
                _particles.transform.position = GetHookPosition();
            if (State == HookState.Hooked)
            {
                if (_hasHookParent)
                {
                    if (HookParent == null || (HookCharacter != null && HookCharacter.Dead && HookCharacter is Human))
                        SetHookState(HookState.DisablingHooked);
                }
            }
        }

        public Vector3 GetHookPosition()
        {
            if (_hasHookParent)
            {
                if (HookParent != null)
                    _lastWorldHookPosition = HookParent.TransformPoint(_hookPosition);
                return _lastWorldHookPosition;
            }
            return _hookPosition;
        }

        protected void OnDestroy()
        {
            if (_particles != null && _particles.gameObject != null)
                Destroy(_particles.gameObject);
        }
    }

    public enum HookState
    {
        Disabled,
        Hooking,
        Hooked,
        DisablingHooking,
        DisablingHooked
    }
}
