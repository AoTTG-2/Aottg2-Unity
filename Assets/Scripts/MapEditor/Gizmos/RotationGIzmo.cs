using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using Settings;
using GameManagers;

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

        bool firstFrame = true;
        bool selectionUniform = false;
        Dictionary<MapObject, List<MapObject>> collectionsToMove = new Dictionary<MapObject, List<MapObject>>();
        protected override void Update()
        {
            base.Update();
            var camera = SceneLoader.CurrentCamera;
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
                    if (firstFrame)
                    {
                        firstFrame = false;

                        // Gather all objects we're moving, if multiselect and all roots, move roots and all children (setup hierarchy)
                        // if multiselect and not all roots, move all selected objects independently
                        selectionUniform = _gameManager.IsSelectionUniform();
                        if (selectionUniform)
                        {
                            foreach (var obj in _gameManager.SelectedObjects)
                            {
                                collectionsToMove.Add(obj, MapLoader.SetupGameObjectHierarchy(obj));
                            }
                        }
                    }
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
                    if (selectionUniform)
                    {
                        // Collect collections into one list
                        List<MapObject> objects = new List<MapObject>();
                        foreach (var obj in collectionsToMove)
                        {
                            MapLoader.ClearGameObjectHierarchy(obj.Key);
                            foreach (var child in obj.Value)
                            {
                                objects.Add(child);
                            }
                        }
                        _gameManager.NewCommand(new TransformCommand(objects));
                    }
                    else
                    {
                        _gameManager.NewCommand(new TransformCommand(new List<MapObject>(_gameManager.SelectedObjects)));
                    }

                    ResetColors();
                    _activeCircle = null;
                    firstFrame = true;
                    selectionUniform = false;
                    collectionsToMove.Clear();
                }
                _gameManager.IgnoreNextSelect = true;
            }
        }

        private Vector3 GetAxis()
        {
            if (_activeCircle == _circleX)
                return _transform.right;
            else if (_activeCircle == _circleY)
                return _transform.up;
            else
                return _transform.forward;
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
            _transform.rotation = Quaternion.identity;
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                if (_gameManager.CurrentGizmoMode == GizmoMode.Local && _gameManager.SelectedObjects.Count == 1)
                {
                    _transform.rotation = obj.GameObject.transform.rotation;
                }
                totalPosition += obj.GameObject.transform.position;
            }
                
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
