using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using Settings;
using UnityEngine.Rendering;

namespace MapEditor
{
    class PositionGizmo : BaseGizmo
    {
        private Transform _lineX;
        private Transform _lineY;
        private Transform _lineZ;
        private Color SelectedColor = Color.white;
        private Color LineXColor = Color.red;
        private Color LineYColor = Color.yellow;
        private Color LineZColor = Color.blue;
        private Transform _activeLine;
        private Vector3 _previousMousePoint;


        public static PositionGizmo Create()
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Gizmos/PositionGizmo");
            var gizmo = go.AddComponent<PositionGizmo>();
            go.SetActive(false);
            return gizmo;
        }

        public override bool IsActive()
        {
            return _activeLine != null;
        }

        protected override void Awake()
        {
            base.Awake();
            _lineX = _transform.Find("LineX");
            _lineY = _transform.Find("LineY");
            _lineZ = _transform.Find("LineZ");
            ResetColors();
        }

        public override void OnSelectionChange()
        {
            if (_gameManager.SelectedObjects.Count > 0 && _gameManager.CurrentGizmo == this)
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
            if (_activeLine == null)
            {
                RaycastHit hit;
                if (!_menu.IsMouseUI && mouseKey.GetKeyDown() && Physics.Raycast(camera.Camera.ScreenPointToRay(Input.mousePosition), out hit, 100000f, PhysicsLayer.GetMask(PhysicsLayer.MapEditorGizmo)))
                {
                    _activeLine = hit.collider.transform;
                    ResetColors();
                    SetLineColor(_activeLine, SelectedColor);
                    _previousMousePoint = hit.point;
                }
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
                    Vector3 drag = mousePoint - _previousMousePoint;
                    if (_activeLine != _lineY)
                        drag += previousRay.normalized * Vector3.Dot(drag, camera.Cache.Transform.up) * 2f;
                    drag = _activeLine.right * Vector3.Dot(drag, _activeLine.right);
                    Vector3 frameDelta = Vector3.zero;

                    if (_gameManager.CurrentGizmoMode == GameManagers.GizmoMode.Center)
                    {
                        if (_activeLine == _lineX)
                            frameDelta.x = drag.x;
                        else if (_activeLine == _lineY)
                            frameDelta.y = drag.y;
                        else if (_activeLine == _lineZ)
                            frameDelta.z = drag.z;
                    }
                    else if (_gameManager.CurrentGizmoMode == GameManagers.GizmoMode.Local)
                    {
                        frameDelta = drag;
                    }

                    
                    if (_gameManager.Snap && _gameManager.CurrentGizmoMode == GameManagers.GizmoMode.Center)
                    {
                        float snap = SettingsManager.MapEditorSettings.SnapMove.Value;
                        if (_activeLine == _lineX)
                        {
                            float x = Mathf.Round((_transform.position.x + frameDelta.x) / snap) * snap;
                            frameDelta.x = x - _transform.position.x;
                        }
                        else if (_activeLine == _lineY)
                        {
                            float y = Mathf.Round((_transform.position.y + frameDelta.y) / snap) * snap;
                            frameDelta.y = y - _transform.position.y;
                        }
                        else if (_activeLine == _lineZ)
                        {
                            float z = Mathf.Round((_transform.position.z + frameDelta.z) / snap) * snap;
                            frameDelta.z = z - _transform.position.z;
                        }
                        if (frameDelta.magnitude >= snap)
                            _previousMousePoint = mousePoint;
                    }
                    else if (_gameManager.Snap && _gameManager.CurrentGizmoMode == GameManagers.GizmoMode.Local)
                    {
                        // Snap along drag direction instead of grid.
                        float snap = SettingsManager.MapEditorSettings.SnapMove.Value;
                        float magnitude = frameDelta.magnitude;
                        float snapMagnitude = Mathf.Round(magnitude / snap) * snap;
                        frameDelta = frameDelta.normalized * snapMagnitude;
                        if (frameDelta.magnitude >= snap)
                            _previousMousePoint = mousePoint;
                    }
                    else
                        _previousMousePoint = mousePoint;
                    MoveSelectedObjects(frameDelta);
                    ResetCenter();
                }
                else
                {
                    if (selectionUniform)
                    {
                        foreach (var obj in collectionsToMove)
                        {
                            MapLoader.ClearGameObjectHierarchy(obj.Key);
                            _gameManager.NewCommand(new TransformCommand(obj.Value));
                        }
                    }
                    else
                    {
                        _gameManager.NewCommand(new TransformCommand(new List<MapObject>(_gameManager.SelectedObjects)));
                    }


                    ResetColors();
                    _activeLine = null;
                    firstFrame = true;
                    selectionUniform = false;
                    collectionsToMove.Clear();

                }
                _gameManager.IgnoreNextSelect = true;
            }
        }

        private void MoveSelectedObjects(Vector3 frameDelta)
        {
            foreach (MapObject obj in _gameManager.SelectedObjects)
                obj.GameObject.transform.position += frameDelta;
        }

        private void ResetCenter()
        {
            Vector3 totalPosition = new Vector3();
            _transform.rotation = Quaternion.identity;
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                totalPosition += obj.GameObject.transform.position;
                if (_gameManager.CurrentGizmoMode == GameManagers.GizmoMode.Local)
                    _transform.rotation = obj.GameObject.transform.rotation;
            }
            Vector3 center = totalPosition / _gameManager.SelectedObjects.Count;
            _transform.position = center;
        }

        private void ResetColors()
        {
            SetLineColor(_lineX, LineXColor);
            SetLineColor(_lineY, LineYColor);
            SetLineColor(_lineZ, LineZColor);
        }

        private void SetLineColor(Transform line, Color color)
        {
            foreach (Renderer renderer in line.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
                renderer.material.renderQueue = 3111;
            }
        }
    }
}
