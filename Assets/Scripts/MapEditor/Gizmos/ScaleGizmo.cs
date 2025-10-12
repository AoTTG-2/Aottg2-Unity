using System.Collections.Generic;
using UnityEngine;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using Settings;
using UnityEditor;
using System.Linq;
using UnityEngine.Rendering;

namespace MapEditor
{
    class ScaleGizmo : BaseGizmo
    {
        private Transform _lineX;
        private Transform _lineY;
        private Transform _lineZ;
        private Transform _center;
        private Color SelectedColor = Color.white;
        private Color LineXColor = Color.red;
        private Color LineYColor = Color.yellow;
        private Color LineZColor = Color.blue;
        private Color CenterColor = new Color(120, 120, 120);
        private Transform _activeLine;
        private Vector3 _previousMousePoint;
        private Vector3 _currentScaleAmount;

        public static ScaleGizmo Create()
        {
            var go = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Gizmos/ScaleGizmo");
            var gizmo = go.AddComponent<ScaleGizmo>();
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
            _center = _transform.Find("Center");
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

        protected bool ContainsCenter(RaycastHit[] hits)
        {

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform == _center)
                {
                    return true;
                }
            }
            return false;
        }

        protected void Update()
        {
            base.Update();
            var camera = SceneLoader.CurrentCamera;
            var mouseKey = SettingsManager.InputSettings.MapEditor.Select;
            if (_activeLine == null)
            {
                if (!_menu.IsMouseUI && mouseKey.GetKeyDown())
                {
                    RaycastHit[] hits = Physics.RaycastAll(camera.Camera.ScreenPointToRay(Input.mousePosition), 100000f, PhysicsLayer.GetMask(PhysicsLayer.MapEditorGizmo));
                    if (ContainsCenter(hits))
                    {
                        _activeLine = _center;
                        ResetColors();
                        SetLineColor(_activeLine, SelectedColor);
                        SetLineColor(_lineX, SelectedColor);
                        SetLineColor(_lineY, SelectedColor);
                        SetLineColor(_lineZ, SelectedColor);
                        _previousMousePoint = hits.First(hit => hit.collider.transform == _center).point;
                    }
                    else
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            if (hit.collider.transform == _lineX || hit.collider.transform == _lineY || hit.collider.transform == _lineZ)
                            {
                                _activeLine = hit.collider.transform;
                                ResetColors();
                                SetLineColor(_activeLine, SelectedColor);
                                _previousMousePoint = hit.point;
                                break;
                            }
                        }
                    }
                }

                _currentScaleAmount = Vector3.zero;
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
                    Vector3 drag = mousePoint - _previousMousePoint;
                    Vector3 direction;
                    if (_activeLine == _lineX)
                        direction = Vector3.right;
                    else if (_activeLine == _lineY)
                        direction = Vector3.up;
                    else if (_activeLine == _lineZ)
                        direction = Vector3.forward;
                    else
                        direction = Vector3.one;
                    if (_activeLine == _center)
                        drag = direction * Vector3.Dot(drag, camera.transform.right);
                    else
                        drag = direction * Vector3.Dot(drag, _activeLine.right);
                    Vector3 frameDelta = drag * 0.1f;

                    if (_gameManager.Snap)
                    {
                        var snap = SettingsManager.MapEditorSettings.SnapScale.Value;
                        _currentScaleAmount += frameDelta;
                        frameDelta = Vector3.zero;

                        if (direction == Vector3.right && Mathf.Abs(_currentScaleAmount.x) > snap)
                        {
                            frameDelta.x = Mathf.Round(_currentScaleAmount.x / snap) * snap;
                            _currentScaleAmount.x %= snap;
                        }
                        
                        if (direction == Vector3.up && Mathf.Abs(_currentScaleAmount.y) > snap)
                        {
                            frameDelta.y = Mathf.Round(_currentScaleAmount.y / snap) * snap;
                            _currentScaleAmount.y %= snap;
                        }
                        
                        if (direction == Vector3.forward && Mathf.Abs(_currentScaleAmount.z) > snap)
                        {
                            frameDelta.z = Mathf.Round(_currentScaleAmount.z / snap) * snap;
                            _currentScaleAmount.z %= snap;
                        }

                        if (direction == Vector3.one && Mathf.Abs(_currentScaleAmount.x) > snap)
                        {
                            frameDelta.x= Mathf.Round(_currentScaleAmount.x / snap) * snap;
                            frameDelta.y = Mathf.Round(_currentScaleAmount.x / snap) * snap;
                            frameDelta.z = Mathf.Round(_currentScaleAmount.x / snap) * snap;
                            _currentScaleAmount.x %= snap;
                        }

                    }
                    
                    ScaleSelectedObjects(frameDelta);
                    ResetCenter();
                    _previousMousePoint = mousePoint;
                }
                else
                {
                    _gameManager.NewCommand(new TransformScaleCommand(new List<MapObject>(_gameManager.SelectedObjects)));
                    ResetColors();
                    _activeLine = null;
                }
                _gameManager.IgnoreNextSelect = true;
            }
        }

        private void ScaleSelectedObjects(Vector3 frameDelta)
        {
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                Vector3 baseScale = obj.BaseScale;
                Vector3 change = Util.MultiplyVectors(baseScale, frameDelta);
                obj.GameObject.transform.localScale += change;
            }
        }

        private void ResetCenter()
        {
            Vector3 totalPosition = new Vector3();
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                totalPosition += obj.GameObject.transform.position;
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
            SetLineColor(_center, CenterColor, renderQueue: 4000);
        }

        private void SetLineColor(Transform line, Color color, int renderQueue=3111)
        {
            foreach (Renderer renderer in line.GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
                renderer.material.renderQueue = renderQueue;
            }
        }
    }
}
