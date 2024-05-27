using UnityEngine;
using Utility;
using Settings;
using UI;
using Weather;
using System.Collections;
using GameProgress;
using Map;
using GameManagers;
using Events;
using ApplicationManagers;
using Characters;
using System.Linq;
using System.Collections.Generic;
using CustomLogic;
using NUnit.Framework.Internal.Commands;

namespace Cameras
{
    class InGameCamera : BaseCamera
    {
        public BaseCharacter _follow;
        private InGameManager _inGameManager;
        private InGameMenu _menu;
        private GeneralInputSettings _input;
        public CameraInputMode CurrentCameraMode;
        public float _cameraDistance;
        private float _heightDistance;
        private float _anchorDistance;
        private const float DistanceMultiplier = 10f;
        private bool _napeLock;
        private BaseTitan _napeLockTitan;
        private SnapshotHandler _snapshotHandler;
        private const float ShakeDistance = 10f;
        private const float ShakeDuration = 1f;
        private const float ShakeDecay = 0.15f;
        private bool _shakeFlip;
        private float _shakeTimeLeft;
        private float _currentShakeDistance;
        private static LayerMask _clipMask = PhysicsLayer.GetMask(PhysicsLayer.MapObjectAll, PhysicsLayer.MapObjectEntities);
        private bool _freeCam = false;

        public void ApplyGraphicsSettings()
        {
            SetTerrainDetails(SettingsManager.GraphicsSettings.DetailDistance.Value, SettingsManager.GraphicsSettings.DetailDensity.Value, SettingsManager.GraphicsSettings.TreeDistance.Value);
            Camera.farClipPlane = SettingsManager.GraphicsSettings.RenderDistance.Value;
        }

        public void ApplyGeneralSettings()
        {
            _cameraDistance = SettingsManager.GeneralSettings.CameraDistance.Value + 0.3f;
            if (SettingsManager.GeneralSettings.CameraDistance.Value == 0f)
                _cameraDistance = 0f;
            CurrentCameraMode = (CameraInputMode)SettingsManager.GeneralSettings.CameraMode.Value;
        }

        public void StartShake()
        {
            _shakeTimeLeft = ShakeDuration;
            _currentShakeDistance = ShakeDistance;
            _shakeFlip = false;
        }

        protected override void SetDefaultCameraPosition()
        {
            GameObject go = MapManager.GetRandomTag(MapTags.CameraSpawnPoint);
            if (go != null)
            {
                Cache.Transform.position = go.transform.position;
                Cache.Transform.rotation = go.transform.rotation;
            }
            else
            {
                Cache.Transform.position = Vector3.up * 100f;
                Cache.Transform.rotation = Quaternion.identity;
            }
        }

        public void SetFollow(BaseCharacter character, bool resetRotation = true)
        {
            _follow = character;
            if (_follow == null)
            {
                _menu.HUDBottomHandler.SetBottomHUD();
                return;
            }
            if (character is Human)
            {
                _anchorDistance = _heightDistance = 0.64f;
            }
            else if (character is BaseTitan || character is BaseShifter)
            {
                _anchorDistance = Vector3.Distance(character.GetCameraAnchor().position, character.Cache.Transform.position) * 0.25f;
                _heightDistance = Vector3.Distance(character.GetCameraAnchor().position, character.Cache.Transform.position) * 0.35f;
            }
            else
                _anchorDistance = _heightDistance = 1f;
            if (resetRotation)
                Cache.Transform.rotation = character.IsMine()
                    ? Util.ConstrainedToY(_follow.Cache.Transform.rotation)
                    : Quaternion.Euler(0f, 0f, 0f);
            if (character is Human && character.IsMine())
                _menu.HUDBottomHandler.SetBottomHUD((Human)character);
            else
                _menu.HUDBottomHandler.SetBottomHUD();
        }

        protected override void Awake()
        {
            base.Awake();
            ApplyGraphicsSettings();
            ApplyGeneralSettings();
            if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
                _snapshotHandler = gameObject.AddComponent<SnapshotHandler>();
        }

        public void TakeSnapshot(Vector3 position, int damage)
        {
            if (_snapshotHandler == null)
                return;
            _snapshotHandler.TakeSnapshot(position, damage);
        }

        protected void Start()
        {
            _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            _input = SettingsManager.InputSettings.General;
            _menu = (InGameMenu)UIManager.CurrentMenu;
        }

        public void SyncCustomPosition()
        {
            Camera.fieldOfView = CustomLogicManager.CameraFOV > 0f ? CustomLogicManager.CameraFOV : 50f;
            Cache.Transform.position = CustomLogicManager.CameraPosition;
            Cache.Transform.rotation = Quaternion.Euler(CustomLogicManager.CameraRotation);
        }

        private void UpdateMapLights()
        {
            var transform = Cache.Transform;
            foreach (var mapLight in MapLoader.MapLights)
            {
                mapLight.UpdateCull(transform);
            }
        }
        
        protected override void LateUpdate()
        {
            UpdateMapLights();
            if (CustomLogicManager.Cutscene || CustomLogicManager.ManualCamera)
            {
                SyncCustomPosition();
                base.LateUpdate();
                return;
            }
            else
            {
                if (_follow != _inGameManager.CurrentCharacter && _inGameManager.CurrentCharacter != null)
                    SetFollow(_inGameManager.CurrentCharacter);
                if (_inGameManager.CurrentCharacter == null)
                {
                    if (_input.ChangeCamera.GetKeyDown())
                        _freeCam = !_freeCam;
                }
                else
                    _freeCam = false;
                if (_freeCam)
                    _follow = null;
                else if (_follow == null)
                    FindNextSpectate();
                if (_follow != null)
                {
                    if (_follow == _inGameManager.CurrentCharacter)
                    {
                        UpdateMain();
                        if (_follow.Dead)
                            _menu.HUDBottomHandler.SetBottomHUD();
                    }
                    else
                        UpdateSpectate();
                    if (!SettingsManager.GeneralSettings.CameraClipping.Value && _follow is Human && _cameraDistance > 0f)
                        UpdateObstacles();
                    if (_follow.Dead)
                        _menu.HUDBottomHandler.SetBottomHUD();
                }
                else if (_freeCam)
                    UpdateFreeCam();
            }
            UpdateFOV();
            UpdateNapeLockImage();
            base.LateUpdate();
        }

        private void UpdateNapeLockImage()
        {
            if (_follow != null && _follow == _inGameManager.CurrentCharacter && _napeLock && _napeLockTitan != null)
            {
                _menu.NapeLock.SetActive(true);
                Vector3 position = _napeLockTitan.BaseTitanCache.Neck.position - _napeLockTitan.BaseTitanCache.Neck.forward * _napeLockTitan.Size;
                Vector3 screenPosition = SceneLoader.CurrentCamera.Camera.WorldToScreenPoint(position);
                _menu.NapeLock.transform.position = screenPosition;
            }
            else
            {
                _menu.NapeLock.SetActive(false);
            }
        }

        private void UpdateMain()
        {

            if (!ChatManager.IsChatActive() && !InGameMenu.InMenu())
            {
                if (_input.ChangeCamera.GetKeyDown())
                {
                    if (CurrentCameraMode == CameraInputMode.TPS)
                        CurrentCameraMode = CameraInputMode.Original;
                    else if (CurrentCameraMode == CameraInputMode.Original)
                        CurrentCameraMode = CameraInputMode.TPS;
                }
                if (SettingsManager.InputSettings.Human.NapeLock.GetKeyDown())
                {
                    _napeLock = !_napeLock;
                    if (_napeLock)
                    {
                        var titan = GetNearestTitan();
                        float distance = Vector3.Distance(_follow.Cache.Transform.position, titan.Cache.Transform.position);
                        if (distance >= 150f)
                            _napeLock = false;
                        else
                            _napeLockTitan = titan;
                    }
                }
            }
            float offset = _cameraDistance * (200f - Camera.fieldOfView) / 150f;
            if (_cameraDistance == 0f)
                offset = 0.1f;
            Cache.Transform.position = _follow.GetCameraAnchor().position;
            Cache.Transform.position += Vector3.up * GetHeightDistance() * SettingsManager.GeneralSettings.CameraHeight.Value;
            float height = _cameraDistance == 0f ? 0.6f : _cameraDistance;
            Cache.Transform.position -= Vector3.up * (0.6f - height) * 2f;
            float sensitivity = SettingsManager.GeneralSettings.MouseSpeed.Value;
            int invertY = SettingsManager.GeneralSettings.InvertMouse.Value ? -1 : 1;
            if (InGameMenu.InMenu())
                sensitivity = 0f;
            if (_napeLock && (_napeLockTitan != null))
            {
                float z = Cache.Transform.eulerAngles.z;
                Transform neck = _napeLockTitan.BaseTitanCache.Neck;
                Cache.Transform.LookAt(_follow.GetCameraAnchor().position * 0.8f + neck.position * 0.2f);
                Cache.Transform.localEulerAngles = new Vector3(Cache.Transform.eulerAngles.x, Cache.Transform.eulerAngles.y, z);
                Cache.Transform.position -= Cache.Transform.forward * DistanceMultiplier * _anchorDistance * offset;
                if (_napeLockTitan.Dead)
                {
                    _napeLockTitan = null;
                }
            }
            else if (CurrentCameraMode == CameraInputMode.Original)
            {
                if (Input.mousePosition.x < (Screen.width * 0.4f))
                {
                    float angle = -(((Screen.width * 0.4f) - Input.mousePosition.x) / Screen.width) * 0.4f * 150f * GetSensitivityDeltaTime(sensitivity);
                    Cache.Transform.RotateAround(Cache.Transform.position, Vector3.up, angle);
                }
                else if (Input.mousePosition.x > (Screen.width * 0.6f))
                {
                    float angle = ((Input.mousePosition.x - (Screen.width * 0.6f)) / Screen.width) * 0.4f * 150f * GetSensitivityDeltaTime(sensitivity);
                    Cache.Transform.RotateAround(Cache.Transform.position, Vector3.up, angle);
                }
                float rotationX = 0.5f * (280f * (Screen.height * 0.6f - Input.mousePosition.y)) / Screen.height;
                Cache.Transform.rotation = Quaternion.Euler(rotationX, Cache.Transform.rotation.eulerAngles.y, Cache.Transform.rotation.eulerAngles.z);
                Cache.Transform.position -= Cache.Transform.forward * DistanceMultiplier * _anchorDistance * offset;
            }
            else if (CurrentCameraMode == CameraInputMode.TPS)
            {
                float inputX = Input.GetAxis("Mouse X") * 10f * sensitivity;
                float inputY = -Input.GetAxis("Mouse Y") * 10f * sensitivity * invertY;
                Cache.Transform.RotateAround(Cache.Transform.position, Vector3.up, inputX);
                float angleY = Cache.Transform.rotation.eulerAngles.x % 360f;
                float sumY = inputY + angleY;
                bool rotateUp = inputY <= 0f || ((angleY >= 260f || sumY <= 260f) && (angleY >= 80f || sumY <= 80f));
                bool rotateDown = inputY >= 0f || ((angleY <= 280f || sumY >= 280f) && (angleY <= 100f || sumY >= 100f));
                if (rotateUp && rotateDown)
                    Cache.Transform.RotateAround(Cache.Transform.position, Cache.Transform.right, inputY);
                Cache.Transform.position -= Cache.Transform.forward * DistanceMultiplier * _anchorDistance * offset;
            }
            Cache.Transform.position += Cache.Transform.right * (SettingsManager.GeneralSettings.CameraSide.Value - 1f);
            UpdateShake();
        }

        private void UpdateSpectate()
        {
            float offset = Mathf.Max(_cameraDistance, 0.3f) * (200f - Camera.fieldOfView) / 150f;
            Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, _follow.GetComponent<BaseMovementSync>()._correctCamera,
                Time.deltaTime * 10f);
            Cache.Transform.position = _follow.GetCameraAnchor().position;
            Cache.Transform.position += Vector3.up * GetHeightDistance() * SettingsManager.GeneralSettings.CameraHeight.Value;
            float height = _cameraDistance;
            Cache.Transform.position -= Vector3.up * (0.6f - height) * 2f;
            Cache.Transform.position -= Cache.Transform.forward * DistanceMultiplier * _anchorDistance * offset;
            if (_inGameManager.Humans.Count > 0 && !InGameMenu.InMenu() && !ChatManager.IsChatActive())
            {
                if (_input.SpectateNextPlayer.GetKeyDown())
                {
                    int nextSpectateIndex = GetSpectateIndex() + 1;
                    if (nextSpectateIndex >= _inGameManager.Humans.Count)
                        nextSpectateIndex = 0;
                    SetFollow(GetSortedCharacters()[nextSpectateIndex]);
                }
                if (_input.SpectatePreviousPlayer.GetKeyDown())
                {
                    int nextSpectateIndex = GetSpectateIndex() - 1;
                    if (nextSpectateIndex < 0)
                        nextSpectateIndex = _inGameManager.Humans.Count - 1;
                    SetFollow(GetSortedCharacters()[nextSpectateIndex]);
                }
            }
        }

        private void UpdateFreeCam()
        {
            if (!InGameMenu.InMenu() && !ChatManager.IsChatActive())
            {
                if (_input.SpectateNextPlayer.GetKeyDown() || _input.SpectatePreviousPlayer.GetKey())
                {
                    _freeCam = false;
                    return;
                }
                Vector3 direction = Vector3.zero;
                if (_input.Forward.GetKey())
                    direction += Cache.Transform.forward;
                else if (_input.Back.GetKey())
                    direction -= Cache.Transform.forward;
                if (_input.Right.GetKey())
                    direction += Cache.Transform.right;
                else if (_input.Left.GetKey())
                    direction -= Cache.Transform.right;
                if (_input.Up.GetKey())
                    direction += Cache.Transform.up;
                else if (_input.Down.GetKey())
                    direction -= Cache.Transform.up;
                Cache.Transform.position += direction * Time.deltaTime * 200f;
                float inputX = Input.GetAxis("Mouse X");
                float inputY = Input.GetAxis("Mouse Y");
                Cache.Transform.RotateAround(Cache.Transform.position, Vector3.up, inputX * Time.deltaTime * 200f);
                Cache.Transform.RotateAround(Cache.Transform.position, Cache.Transform.right, -inputY * Time.deltaTime * 200f);
            }
        }

        private float GetHeightDistance()
        {
            if (_cameraDistance == 0f && _follow != null && _follow is Human)
                return 0.3f;
            return _heightDistance;
        }

        private void UpdateObstacles()
        {
            Vector3 start = _follow.GetCameraAnchor().position;
            Vector3 direction = (start - Cache.Transform.position).normalized;
            Vector3 end = start - direction * DistanceMultiplier * _anchorDistance;
            LayerMask mask = _clipMask;
            RaycastHit hit;
            if (Physics.Linecast(start, end, out hit, mask))
                Cache.Transform.position = hit.point;
        }

        private void UpdateFOV()
        {
            if (_follow != null && _follow is Human)
            {
                float fovMin = SettingsManager.GeneralSettings.FOVMin.Value;
                float fovMax = SettingsManager.GeneralSettings.FOVMax.Value;
                if (_cameraDistance <= 0f)
                {
                    fovMin = SettingsManager.GeneralSettings.FPSFOVMin.Value;
                    fovMax = SettingsManager.GeneralSettings.FPSFOVMax.Value;
                }
                fovMax = Mathf.Max(fovMin, fovMax);
                float speed = _follow.Cache.Rigidbody.velocity.magnitude;
                float fovTarget = fovMin;

                if (speed > 10f)
                    fovTarget = Mathf.Min(fovMax, speed + fovMin - 10f);
                Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, fovTarget, 5f * Time.deltaTime);
            }
            else
                Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, SettingsManager.GeneralSettings.FOVMin.Value, 5f * Time.deltaTime);
            if (CustomLogicManager.CameraFOV > 0f)
                Camera.fieldOfView = CustomLogicManager.CameraFOV;
        }

        private void FindNextSpectate()
        {
            var characters = GetSortedCharacters();
            if (characters.Count > 0)
                SetFollow(characters[0]);
            else
                SetFollow(null);
        }

        private int GetSpectateIndex()
        {
            if (_follow == null)
                return -1;
            var humans = GetSortedCharacters();
            for (int i = 0; i < humans.Count; i++)
            {
                if (humans[i] == _follow)
                    return i;
            }
            return -1;
        }

        private float GetSensitivityDeltaTime(float sensitivity)
        {
            return (sensitivity * Time.deltaTime) * 62f;
        }

        private BaseTitan GetNearestTitan()
        {
            BaseTitan nearestTitan = null;
            float nearestDistance = Mathf.Infinity;
            foreach (var character in _inGameManager.GetAllCharacters())
            {
                if (character is BaseTitan && !TeamInfo.SameTeam(character, _follow))
                {
                    float distance = Vector3.Distance(_follow.Cache.Transform.position, character.Cache.Transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestTitan = (BaseTitan)character;
                    }
                }
            }
            return nearestTitan;
        }

        private List<BaseCharacter> GetSortedCharacters()
        {
            List<BaseCharacter> characters = new List<BaseCharacter>();
            foreach (var human in _inGameManager.Humans)
            {
                if (!human.AI)
                    characters.Add(human);
            } 
            foreach (var shifter in _inGameManager.Shifters)
            {
                if (!shifter.AI)
                    characters.Add(shifter);
            }
            return characters.OrderBy(x => x.Cache.PhotonView.Owner.ActorNumber).ToList();
        }

        private void UpdateShake()
        {
            if (_shakeTimeLeft > 0f)
            {
                _shakeTimeLeft -= Time.deltaTime;
                if (_shakeFlip)
                    Cache.Transform.position += Vector3.up * _currentShakeDistance;
                else
                    Cache.Transform.position -= Vector3.up * _currentShakeDistance;
                _shakeFlip = !_shakeFlip;
                float decay = ShakeDecay * Time.deltaTime * 60f;
                _currentShakeDistance *= (1 - decay);
            }
        }

        // Added by Snake for Terrain Detail Slider 28 may 24 
        public void SetTerrainDetails(int DetailDistance, int DetailDensity, int TreeDistance)
        {
            Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>();
            foreach (Terrain terrain in terrains)
            {
                terrain.detailObjectDistance = DetailDistance;
                terrain.detailObjectDensity = DetailDensity /1000f; 
                terrain.treeDistance = TreeDistance;
            }
        }
    }
}
