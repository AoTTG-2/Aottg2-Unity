using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using Settings;

namespace MapEditor
{
    class RotationGizmo : BaseGizmo
    {
        private Transform _circleX;
        private Transform _circleY;
        private Transform _circleZ;
        private Color SelectedColor = Color.white;
        private Color CircleXColor = Color.red;
        private Color CircleYColor = Color.yellow;
        private Color CircleZColor = Color.blue;
        private Transform _activeCircle;
        private Vector3 _previousMousePoint;
        private float _currentAngle;

        public static RotationGizmo Create()
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Gizmos/RotationGizmo");
            var gizmo = go.AddComponent<RotationGizmo>();
            go.SetActive(false);
            return gizmo;
        }

        public override bool IsActive()
        {
            return _activeCircle != null;
        }

        protected override void Awake()
        {
            base.Awake();
            _circleX = _transform.Find("CircleX");
            _circleY = _transform.Find("CircleY");
            _circleZ = _transform.Find("CircleZ");
            ResetColors();
        }

        public override void OnSelectionChange()
        {
            if (_gameManager.SelectedObjects.Count > 0  && _gameManager.CurrentGizmo == this)
            {
                gameObject.SetActive(true);
                ResetCenter();
                ResetColors();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected void Update()
        {
            var camera = SceneLoader.CurrentCamera;
            float distance = Vector3.Distance(camera.Cache.Transform.position, _transform.position);
            _transform.localScale = Vector3.one * distance / 200f;
            var mouseKey = SettingsManager.InputSettings.MapEditor.Select;
            if (_activeCircle == null)
            {
                RaycastHit hit;
                if (!_menu.IsMouseUI && mouseKey.GetKeyDown() && Physics.Raycast(camera.Camera.ScreenPointToRay(Input.mousePosition), out hit, 100000f, PhysicsLayer.GetMask(PhysicsLayer.MapEditorGizmo)))
                {
                    _activeCircle = hit.collider.transform.parent;
                    ResetColors();
                    SetCircleColor(_activeCircle, SelectedColor);
                    _previousMousePoint = hit.point;
                }

                _currentAngle = 0f;
            }
            else
            {
                if (mouseKey.GetKey())
                {
                    Ray ray = camera.Camera.ScreenPointToRay(Input.mousePosition);
                    Vector3 previousRay = (_previousMousePoint - ray.origin);
                    float angle = Vector3.Angle(ray.direction, previousRay) * Mathf.Deg2Rad;
                    float rayDistance = previousRay.magnitude * 1f / Mathf.Cos(angle);
                    Vector3 mousePoint = ray.origin + ray.direction * rayDistance;
                    Vector3 center = _activeCircle.position;
                    Vector3 origDirection = (_previousMousePoint - center).normalized;
                    Vector3 newDirection = (mousePoint - center).normalized;
                    angle = Util.SignedAngle(origDirection, newDirection, GetAxis());

                    if (_gameManager.Snap)
                    {
                        var snap = SettingsManager.MapEditorSettings.SnapRotate.Value;
                        _currentAngle += angle;
                        angle = 0;

                        if (Mathf.Abs(_currentAngle) > snap)
                        {
                            angle = Mathf.Round(_currentAngle / snap) * snap;
                            _currentAngle %= snap;
                        }
                    }
                    
                    RotateSelectedObjects(center, GetAxis(), angle);
                    ResetCenter();
                    _previousMousePoint = mousePoint;
                }
                else
                {
                    _gameManager.NewCommand(new TransformPositionRotationCommand(new List<MapObject>(_gameManager.SelectedObjects)));
                    ResetColors();
                    _activeCircle = null;
                }
                _gameManager.IgnoreNextSelect = true;
            }
        }

        private Vector3 GetAxis()
        {
            if (_activeCircle == _circleX)
                return Vector3.right;
            else if (_activeCircle == _circleY)
                return Vector3.up;
            else
                return Vector3.forward;
        }

        private void RotateSelectedObjects(Vector3 center, Vector3 axis, float angle)
        {
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                obj.GameObject.transform.RotateAround(center, axis, angle);
            }
        }

        private void ResetCenter()
        {
            Vector3 totalPosition = new Vector3();
            foreach (MapObject obj in _gameManager.SelectedObjects)
                totalPosition += obj.GameObject.transform.position;
            Vector3 center = totalPosition / _gameManager.SelectedObjects.Count;
            _transform.position = center;
        }

        private void ResetColors()
        {
            SetCircleColor(_circleX, CircleXColor);
            SetCircleColor(_circleY, CircleYColor);
            SetCircleColor(_circleZ, CircleZColor);
        }

        private void SetCircleColor(Transform line, Color color)
        {
            foreach (Renderer renderer in line.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
                renderer.material.renderQueue = 3111;
            }
        }
    }
}
