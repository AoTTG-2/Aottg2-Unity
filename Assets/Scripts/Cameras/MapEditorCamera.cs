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
using System.Linq;
using System;
using static UnityEngine.GraphicsBuffer;

namespace Cameras
{
    class MapEditorCamera : BaseCamera
    {
        private MapEditorInputSettings _input;
        private MapEditorSettings _settings;
        private MapEditorMenu _menu;
        private Camera _uiCamera;
        private bool _wasRotating;
        private bool _startDrag;
        private Vector3 _lastDragPosition;

        protected override void Awake()
        {
            base.Awake();
            _input = SettingsManager.InputSettings.MapEditor;
            _settings = SettingsManager.MapEditorSettings;
            ApplyGraphicsSettings();
        }

        public void ApplyGraphicsSettings()
        {
            Camera.farClipPlane = SettingsManager.MapEditorSettings.RenderDistance.Value;
        }

        protected void CreateUICamera()
        {
            /*
            _uiCamera = Util.CreateObj<Camera>();
            _uiCamera.clearFlags = CameraClearFlags.Depth;
            _uiCamera.cullingMask = PhysicsLayer.GetMask(PhysicsLayer.UI);
            _uiCamera.depth = 1;
            _uiCamera.fieldOfView = 60f;
            var canvas = _menu.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _uiCamera;
            canvas.planeDistance = 335.1518f;
            */
        }

        protected void Update()
        {
            if (_menu == null)
                return;
            if (!_menu.IsInputFocused)
                UpdateMovement();
            UpdateRotation();
        }

        private void UpdateMovement()
        {
            Vector3 direction = Vector3.zero;
            if (_input.SaveMap.GetKey())
                return;
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
            float speed = _settings.CameraMoveSpeed.Value;
            if (_input.Slow.GetKey())
                speed = _settings.CameraSlowMoveSpeed.Value;
            else if (_input.Fast.GetKey())
                speed = _settings.CameraFastMoveSpeed.Value;
             Cache.Transform.position += direction * Time.deltaTime * speed;
            UpdateDragMovement(speed);
            UpdateZoomMovement(speed);

            if (_input.ToggleOrthographic.GetKeyDown())
            {
                Camera.orthographic = !Camera.orthographic;
                if (Camera.orthographic)
                {
                    Camera.orthographicSize = 0.25f * Screen.height;
                }
            }
        }

        private void UpdateDragMovement(float speed)
        {
            if (_input.Pan.GetKeyDown())
            {
                _startDrag = true;
                _lastDragPosition = Input.mousePosition;
            }
            else if (_input.Pan.GetKey())
            {
                if (_startDrag)
                {
                    Vector2 delta = Input.mousePosition - _lastDragPosition;
                    _lastDragPosition = Input.mousePosition;
                    Cache.Transform.position -= Cache.Transform.right * delta.x * Time.deltaTime * speed * 0.5f;
                    Cache.Transform.position -= Cache.Transform.up * delta.y * Time.deltaTime * speed * 0.5f;
                }
            }
            else if (_input.Pan.GetKeyUp())
            {
                _startDrag = false;
            }
        }

        /// TODO: Use scale aware zooming speed when object is selected.
        private void UpdateZoomMovement(float speed)
        {
            if (_menu.IsMouseUI)
                return;
            if (!Camera.orthographic)
            {
                if (_input.ZoomIn.GetKey())
                    Cache.Transform.position += Cache.Transform.forward * Time.deltaTime * speed * 5;
                else if (_input.ZoomOut.GetKey())
                    Cache.Transform.position -= Cache.Transform.forward * Time.deltaTime * speed * 5;
            }
            else
            {
                if (_input.ZoomIn.GetKey())
                    Camera.orthographicSize -= Time.deltaTime * speed * 5;
                else if (_input.ZoomOut.GetKey())
                    Camera.orthographicSize += Time.deltaTime * speed * 5;
            }
            
        }

        private void UpdateRotation()
        {
            if (!_wasRotating && _menu.IsMouseUI)
                return;
            _wasRotating = false;
            if (_input.RotateCamera.GetKey())
            {
                float inputX = Input.GetAxis("Mouse X");
                float inputY = Input.GetAxis("Mouse Y");
                float speed = _settings.CameraRotateSpeed.Value;
                Cache.Transform.RotateAround(Cache.Transform.position, Vector3.up, inputX * Time.deltaTime * speed);
                Cache.Transform.RotateAround(Cache.Transform.position, Cache.Transform.right, -inputY * Time.deltaTime * speed);
                _wasRotating = true;
            }
            

            if (_input.SnapCameraLeft.GetKeyDown())
                SnapCameraToAxis(Vector3.down);
            else if (_input.SnapCameraRight.GetKeyDown())
                SnapCameraToAxis(Vector3.up);
            else if (_input.SnapCameraUp.GetKeyDown())
                SnapCameraToAxis(Vector3.right);
            else if (_input.SnapCameraDown.GetKeyDown())
                SnapCameraToAxis(Vector3.left);
        }

        private bool AlignedWithWorldAxis()
        {
            return Cache.Transform.forward == Vector3.forward || Cache.Transform.forward == Vector3.back
                || Cache.Transform.forward == Vector3.left || Cache.Transform.forward == Vector3.right
                || Cache.Transform.forward == Vector3.up || Cache.Transform.forward == Vector3.down;
        }

        private void AlignToWorldAxis()
        {
            // The simplest way to do this is to take the cube’s forward and up vectors, align them to the nearest world axes, and then recalculate the cube orientation using Quaternion.LookRotation. To snap a vector along the nearest world axis, find out which of its components (x, y, or z) has the greatest magn…
            Vector3 forward = Cache.Transform.forward;
            Vector3 up = Cache.Transform.up;

            Vector3 newForward = Vector3.zero;
            Vector3 newUp = Vector3.zero;

            float forwardAbsX = Mathf.Abs(forward.x);
            float forwardAbsY = Mathf.Abs(forward.y);
            float forwardAbsZ = Mathf.Abs(forward.z);

            float upAbsX = Mathf.Abs(up.x);
            float upAbsY = Mathf.Abs(up.y);
            float upAbsZ = Mathf.Abs(up.z);

            if (forwardAbsX > forwardAbsY && forwardAbsX > forwardAbsZ)
            {
                newForward = forward.x > 0 ? Vector3.right : Vector3.left;
            }
            else if (forwardAbsY > forwardAbsX && forwardAbsY > forwardAbsZ)
            {
                newForward = forward.y > 0 ? Vector3.up : Vector3.down;
            }
            else if (forwardAbsZ > forwardAbsX && forwardAbsZ > forwardAbsY)
            {
                newForward = forward.z > 0 ? Vector3.forward : Vector3.back;
            }

            if (upAbsX > upAbsY && upAbsX > upAbsZ)
            {
                newUp = up.x > 0 ? Vector3.right : Vector3.left;
            }
            else if (upAbsY > upAbsX && upAbsY > upAbsZ)
            {
                newUp = up.y > 0 ? Vector3.up : Vector3.down;
            }
            else if (upAbsZ > upAbsX && upAbsZ > upAbsY)
            {
                newUp = up.z > 0 ? Vector3.forward : Vector3.back;
            }

            Cache.Transform.forward = newForward;
            Cache.Transform.up = newUp;
        }

        private void SnapCameraToAxis(Vector3 direction)
        {

            MapEditorGameManager mapEditorGameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            Vector3 position = Vector3.zero;
            if (mapEditorGameManager.SelectedObjects.Count == 0)
            {
                position = Cache.Transform.position + Cache.Transform.forward * 50f;
            }
            else
            {
                foreach (MapObject obj in mapEditorGameManager.SelectedObjects)
                {
                    position += obj.GameObject.transform.position;
                }
                position /= mapEditorGameManager.SelectedObjects.Count;
            }
            float distance = (Cache.Transform.position - position).magnitude;

            // rotate 90 degrees in the given direction and clamp to the the 4 90 degree angles of the world axis

            if (!AlignedWithWorldAxis())
            {
                Debug.Log("Not aligned with world axis");
                AlignToWorldAxis();
                return;
            }

            // Rotate the camera 90 degrees in the given direction relative to the current forward vector
            Quaternion rotation = Quaternion.AngleAxis(90f, Cache.Transform.TransformDirection(direction));
            Cache.Transform.rotation = rotation * Cache.Transform.rotation;

            // Set position to the same distance from the target
            Cache.Transform.position = position - Cache.Transform.forward * distance;

            // Ensure the camera is facing the right direction
            Cache.Transform.LookAt(position);
        }

        protected override void SetDefaultCameraPosition()
        {
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            CreateUICamera();
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
    }
}