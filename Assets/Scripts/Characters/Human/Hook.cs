using ApplicationManagers;
using CustomLogic;
using GameManagers;
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
        protected float _hookWidth = 0.08f;

        protected readonly struct HookRetractSettings
        {
            public readonly float TotalRetractTime;
            public readonly float WaveStartDelay;
            public readonly float WaveTransitionTime;
            public readonly float BaseAmplitude;
            public readonly float WaveFrequency;
            public readonly float MinWaveInfluence;
            public readonly float PointSpacing;

            public HookRetractSettings(float retractTime = 0.6f, float startDelay = 0.15f, float transitionTime = 0.15f,
                float amplitude = 0.15f, float frequency = 2.5f, float minInfluence = 0.1f, float spacing = 1.2f)
            {
                TotalRetractTime = retractTime;
                WaveStartDelay = startDelay;
                WaveTransitionTime = transitionTime;
                BaseAmplitude = amplitude;
                WaveFrequency = frequency;
                MinWaveInfluence = minInfluence;
                PointSpacing = spacing;
            }
        }

        protected void TransitionFromCurvedToStraight(List<Vector3> sourceNodes, Vector3 startPos, Vector3 endPos, float transitionProgress)
        {
            int vertex = (int)((endPos - startPos).magnitude / 5f);
            vertex = Mathf.Clamp(vertex, 2, 6);

            int pointCount = Mathf.Max(sourceNodes.Count, vertex);
            _renderer.positionCount = pointCount;

            for (int i = 0; i < pointCount; i++)
            {
                float normalizedI = (float)i / (pointCount - 1);

                Vector3 curvedPos;
                if (i < sourceNodes.Count)
                {
                    Vector3 v = startPos - sourceNodes[0];
                    curvedPos = sourceNodes[i] + v * Mathf.Pow(0.85f, i);
                }
                else
                {
                    curvedPos = endPos;
                }

                Vector3 straightPos = Vector3.Lerp(startPos, endPos, normalizedI);

                Vector3 finalPos = Vector3.Lerp(curvedPos, straightPos, transitionProgress);
                _renderer.SetPosition(i, finalPos);
            }

            _renderer.SetPosition(_renderer.positionCount - 1, endPos);
        }

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
            _renderer.material = ResourceManager.InstantiateAsset<Material>(ResourcePaths.Map, "Materials/HookMaterial", true);
            _renderer.material.color = Color.white;
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

            _endSprite.SetActive(State == HookState.DisablingHooked ? true : false);
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
            _renderer.startWidth = _renderer.endWidth = _hookWidth;
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

                if (SettingsManager.InGameCurrent.Misc.RealismMode.Value)
                {
                    if (HookCharacter != null && HookCharacter is Human && !TeamInfo.SameTeam(HookCharacter, _owner))
                    {
                        HookCharacter.GetKilled(_owner.Name + "'s hook");
                    }
                }
            }
            _currentLiveTime = 0f;
            _renderer.startWidth = _renderer.endWidth = _hookWidth;
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

        private float _hookTransitionTime = 0f;
        private const float HOOK_TRANSITION_DURATION = 0.1f;
        private List<Vector3> _lastHookingNodes = new List<Vector3>();

        public void SetHooked(Vector3 position, Transform t = null, int viewId = -1, int objectId = -1)
        {
            _lastHookingNodes = new List<Vector3>(_nodes);
            _hookTransitionTime = 0f;

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
                    _renderer.SetPosition(i, _nodes[i] + v * Mathf.Pow(0.85f, i));
                if (_nodes.Count > 1)
                    _renderer.SetPosition(1, Anchor.position);
            }
        }

        protected void UpdateHooked()
        {
            Vector3 position = GetHookPosition();
            Vector3 v1 = position - Anchor.position;
            Vector3 velocity = _owner.Cache.Rigidbody.velocity;

            // Calculate transition progress
            _hookTransitionTime += Time.deltaTime;
            float transitionProgress = Mathf.Clamp01(_hookTransitionTime / HOOK_TRANSITION_DURATION);
            transitionProgress = Mathf.SmoothStep(0, 1, transitionProgress);

            if (transitionProgress < 1f && _lastHookingNodes.Count > 0)
            {
                // Use shared transition function for the curved-to-straight transition
                TransitionFromCurvedToStraight(_lastHookingNodes, Anchor.position, position, transitionProgress);
            }
            else
            {
                // After transition, handle normal hooked state with velocity-based noise
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
                    Vector3 noisePosition = Anchor.position + new Vector3(
                        Random.Range(-max, max),
                        Random.Range(-max, max),
                        Random.Range(-max, max));
                    noisePosition += (v1 * ((float)i / (float)vertex))
                        - (Vector3.up * rndFactor * 0.05f * noise)
                        - (velocity * 0.001f * noise * rndFactor);
                    _renderer.SetPosition(i, noisePosition);
                }
                _renderer.SetPosition(vertex - 1, position);
            }

            // Update end sprite
            _endSprite.transform.position = position + v1.normalized * 0.1f;
            _endSprite.transform.rotation = Quaternion.LookRotation(v1.normalized,
                (SceneLoader.CurrentCamera.Cache.Transform.position - _endSprite.transform.position).normalized);
        }

        private float _randomRotationAngle;
        private float _secondaryRotationAngle;
        private float _waveVariation;
        private float _amplitudeVariation;

        protected void UpdateRetractingHook(Vector3 startPos, Vector3 endPos, HookRetractSettings settings)
        {
            if (_currentLiveTime == 0)
            {
                _randomRotationAngle = Random.Range(0f, 360f);
                _secondaryRotationAngle = Random.Range(-30f, 30f);
                _waveVariation = Random.Range(0.8f, 1.2f);
                _amplitudeVariation = Random.Range(0.9f, 1.1f);
            }

            float totalDistance = Vector3.Distance(startPos, endPos);
            _currentLiveTime += Time.deltaTime;

            float baseProgress = _currentLiveTime / settings.TotalRetractTime;
            float exponentialProgress = 1f - Mathf.Pow(1f - baseProgress, 3f);

            if (_currentLiveTime > 0.325f)
            {
                float lateGameAcceleration = (_currentLiveTime - 0.325f) / 0.2f;
                exponentialProgress = Mathf.Lerp(exponentialProgress, 1f, lateGameAcceleration * lateGameAcceleration);
            }
            exponentialProgress = Mathf.Min(exponentialProgress, 1f);

            float waveTimeInfluence = _currentLiveTime < settings.WaveStartDelay ? 0f :
                Mathf.SmoothStep(0f, 1f, (_currentLiveTime - settings.WaveStartDelay) / settings.WaveTransitionTime);

            int pointCount = Mathf.Max(24, Mathf.Min(32, Mathf.CeilToInt(totalDistance / settings.PointSpacing)));
            _renderer.positionCount = pointCount;

            Vector3 directVector = endPos - startPos;
            Vector3 forward = directVector.normalized;

            // Create a randomized rotation basis
            Quaternion randomRotation = Quaternion.Euler(0, _randomRotationAngle, _secondaryRotationAngle);
            Vector3 right = randomRotation * Vector3.Cross(forward, Vector3.up).normalized;
            Vector3 up = Vector3.Cross(right, forward);

            // Generate points along the line
            for (int i = 0; i < pointCount; i++)
            {
                float pointProgress = (float)i / (pointCount - 1);
                Vector3 basePoint = Vector3.Lerp(startPos, endPos, Mathf.Lerp(exponentialProgress, 1f, pointProgress));

                if (exponentialProgress < 0.95f && waveTimeInfluence > 0f)
                {
                    float waveInfluence = Mathf.Lerp(1f, settings.MinWaveInfluence, Mathf.Pow(pointProgress, 0.9f));

                    float time = _currentLiveTime * settings.WaveFrequency * _waveVariation;
                    float wavePhase = pointProgress * Mathf.PI * settings.WaveFrequency + time;

                    float amplitude = totalDistance * settings.BaseAmplitude * _amplitudeVariation *
                        waveInfluence *
                        (1f - exponentialProgress) *
                        waveTimeInfluence;

                    // Add some variation to the spiral shape
                    float horizontalScale = Mathf.Sin(pointProgress * Mathf.PI * 0.5f);
                    float verticalScale = Mathf.Cos(pointProgress * Mathf.PI * 0.5f);

                    Vector3 offset = right * Mathf.Sin(wavePhase) * amplitude * horizontalScale +
                                   up * Mathf.Cos(wavePhase) * amplitude * verticalScale;

                    if (exponentialProgress > 0.8f)
                    {
                        float endSmoothing = 1f - ((exponentialProgress - 0.8f) / 0.15f);
                        offset *= Mathf.SmoothStep(0f, 1f, endSmoothing);
                    }

                    basePoint += offset;
                }

                _renderer.SetPosition(i, basePoint);
            }

            // Handle end points
            if (exponentialProgress < 0.95f)
            {
                Vector3 slightOffset = Vector3.Lerp(
                    _renderer.GetPosition(pointCount - 2) - endPos,
                    Vector3.zero,
                    exponentialProgress);
                _renderer.SetPosition(pointCount - 1, endPos + slightOffset * 0.1f);
            }
            else
            {
                _renderer.SetPosition(pointCount - 1, endPos);
            }

            _renderer.startWidth = _renderer.endWidth = _hookWidth;

            // Update end sprite
            if (_endSprite != null)
            {
                Debug.Log("END SPRITE EXISTS!!!");
                Vector3 direction = (_renderer.GetPosition(1) - _renderer.GetPosition(0)).normalized;
                _endSprite.transform.position = _renderer.GetPosition(0) + direction * 0.1f;
                _endSprite.transform.rotation = Quaternion.LookRotation(direction,
                    (SceneLoader.CurrentCamera.Cache.Transform.position - _endSprite.transform.position).normalized);
            }

            if (_currentLiveTime >= settings.TotalRetractTime || Vector3.Distance(startPos, endPos) < 0.1f)
            {
                FinishDisable();
            }
        }

        protected void UpdateDisablingHooking()
        {
            const float TOTAL_DURATION = 0.5f;
            _currentLiveTime += Time.deltaTime;

            if (_nodes.Count > 0)
            {
                float retractProgress = _currentLiveTime / TOTAL_DURATION;
                retractProgress = 1f - Mathf.Pow(1f - retractProgress, 3f); // Exponential easing

                int nodeCount = Mathf.Max(2, Mathf.CeilToInt(_nodes.Count * (1f - retractProgress)));
                _renderer.positionCount = nodeCount;

                Vector3 v = Anchor.position - _nodes[0];
                for (int i = 0; i < nodeCount; i++)
                {
                    _renderer.SetPosition(i, _nodes[i] + v * Mathf.Pow(0.85f, i));
                }

                if (nodeCount > 1)
                {
                    _renderer.SetPosition(1, Anchor.position);
                }
            }

            _renderer.startWidth = _renderer.endWidth = _hookWidth;

            if (_currentLiveTime >= TOTAL_DURATION)
            {
                FinishDisable();
            }
        }

        protected void UpdateDisablingHooked()
        {
            var settings = new HookRetractSettings(
                retractTime: 0.45f,
                startDelay: 0.12f,
                transitionTime: 0.15f,
                amplitude: 0.3f,
                frequency: 2.5f,
                minInfluence: 0.2f,
                spacing: 1.2f
            );
            UpdateRetractingHook(GetHookPosition(), Anchor.position, settings);
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
