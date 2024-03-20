using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using CustomSkins;
using ApplicationManagers;
using Map;
using Settings;
using MapEditor;
using CustomLogic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun.Demo.PunBasics;

namespace GameManagers
{
    class MapEditorGameManager : BaseGameManager
    {
        public MapScript MapScript;
        public CustomLogicEvaluator LogicEvaluator;
        public HashSet<MapObject> SelectedObjects = new HashSet<MapObject>();
        public BaseGizmo CurrentGizmo;
        public bool Snap;
        private List<BaseCommand> _undoCommands = new List<BaseCommand>();
        private List<BaseCommand> _redoCommands = new List<BaseCommand>();
        private string _clipboard = string.Empty;
        private MapEditorMenu _menu;
        private MapEditorInputSettings _input;
        public PositionGizmo _positionGizmo;
        public RotationGizmo _rotationGizmo;
        public ScaleGizmo _scaleGizmo;
        private OutlineGizmo _outlineGizmo;
        private int _currentObjectId;
        public bool IgnoreNextSelect;
        private bool _isDrag;
        private Vector3 _dragStart;
        
        public void ShowAddObject()
        {
            if (_menu.AddObjectPopup.IsActive)
                _menu.AddObjectPopup.Hide();
            else if (!_menu.IsMouseUI)
                _menu.AddObjectPopup.Show();
        }

        public void AddObject(string name)
        {
            var mapScriptObjects = new MapScriptObjects();
            MapScriptSceneObject prefab;
            if (name.StartsWith("Custom/"))
            {
                prefab = new MapScriptSceneObject();
                prefab.Asset = name;
                prefab.Name = name.Split('/')[2];
                prefab.Material.Shader = MapObjectShader.DefaultNoTint;
            }
            else
                prefab = (MapScriptSceneObject)BuiltinMapPrefabs.AllPrefabs[name];
            var position = SceneLoader.CurrentCamera.Cache.Transform.position + SceneLoader.CurrentCamera.Cache.Transform.forward * 50f;
            // if snap is enabled, round the position to the nearest snap distance
            if (((MapEditorGameManager)SceneLoader.CurrentGameManager).Snap)
            {
                float snap = SettingsManager.MapEditorSettings.SnapMove.Value;
                float x = Mathf.Round(position.x / snap) * snap;
                float y = Mathf.Round(position.y / snap) * snap;
                float z = Mathf.Round(position.z / snap) * snap;
                position = new Vector3(x, y, z);
            }
            prefab.SetPosition(position);
            mapScriptObjects.Objects.Add(prefab);
            NewCommand(new AddObjectCommand(mapScriptObjects.Objects));
            DeselectAll();
            SelectObject(MapLoader.IdToMapObject[_currentObjectId]);
            _menu.SyncHierarchyPanel();
            OnSelectionChange();
        }

        public void Undo()
        {
            if (_undoCommands.Count == 0)
                return;
            var command = _undoCommands[_undoCommands.Count - 1];
            command.Unexecute();
            _redoCommands.Add(command);
            _undoCommands.RemoveAt(_undoCommands.Count - 1);
            if (command is AddObjectCommand || command is DeleteObjectCommand)
                _menu.SyncHierarchyPanel();
            OnSelectionChange();
        }

        public void Redo()
        {
            if (_redoCommands.Count == 0)
                return;
            var command = _redoCommands[_redoCommands.Count - 1];
            command.Execute();
            _undoCommands.Add(command);
            _redoCommands.RemoveAt(_redoCommands.Count - 1);
            if (command is AddObjectCommand || command is DeleteObjectCommand)
                _menu.SyncHierarchyPanel();
            OnSelectionChange();
        }

        public void Copy()
        {
            if (SelectedObjects.Count == 0)
                return;
            var mapScriptObjects = new MapScriptObjects();
            foreach (var obj in SelectedObjects)
                mapScriptObjects.Objects.Add(obj.ScriptObject);
            _clipboard = mapScriptObjects.Serialize();
        }

        public void Paste()
        {
            if (_clipboard == string.Empty)
                return;
            var mapScriptObjects = new MapScriptObjects();
            mapScriptObjects.Deserialize(_clipboard);
            NewCommand(new AddObjectCommand(mapScriptObjects.Objects));
            DeselectAll();
            foreach (var obj in mapScriptObjects.Objects)
                SelectObject(MapLoader.IdToMapObject[obj.Id]);
            _menu.SyncHierarchyPanel();
            OnSelectionChange();
        }

        public void Cut()
        {
            Copy();
            Delete();
        }

        public void Delete()
        {
            if (SelectedObjects.Count == 0)
                return;
            NewCommand(new DeleteObjectCommand(new List<MapObject>(SelectedObjects)));
            _menu.SyncHierarchyPanel();
            OnSelectionChange();
        }

        public void Select(bool multi)
        {
            var camera = SceneLoader.CurrentCamera;
            Vector3 diff = Input.mousePosition - _dragStart;
            RaycastHit hit;
            if (diff.magnitude < 1f)
            {
                if (!_menu.IsMouseUI)
                {
                    if (Physics.Raycast(camera.Camera.ScreenPointToRay(Input.mousePosition), out hit, 100000f, PhysicsLayer.GetMask(PhysicsLayer.MapEditorObject)))
                    {

                        var mapObject = MapLoader.FindObjectFromCollider(hit.collider);
                        if (multi)
                        {
                            if (SelectedObjects.Contains(mapObject))
                                DeselectObject(mapObject);
                            else
                                SelectObject(mapObject);
                        }
                        else
                        {
                            if (SelectedObjects.Count == 1 && SelectedObjects.Contains(mapObject))
                                DeselectAll();
                            else if (SelectedObjects.Count > 0)
                            {
                                DeselectAll();
                                SelectObject(mapObject);
                            }
                            else
                                SelectObject(mapObject);
                        }
                    }
                    else if (!multi)
                        DeselectAll();
                }
            }
            else
            {
                if (!multi)
                    DeselectAll();
                foreach (var gameObject in MapLoader.GoToMapObject.Keys)
                {
                    var mapObject = MapLoader.GoToMapObject[gameObject];
                    var renderer = gameObject.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        Vector3 center = renderer.bounds.center;
                        Vector2 screenPosition = camera.Camera.WorldToScreenPoint(center);
                        if (Vector3.Distance(center, camera.Cache.Transform.position) < camera.Camera.farClipPlane && Util.IsVectorBetween(screenPosition, (Vector2)_dragStart, (Vector2)Input.mousePosition))
                        {
                            if (!SelectedObjects.Contains(mapObject))
                                SelectObject(mapObject);
                        }
                    }
                }
            }
            OnSelectionChange();
        }

        public void DeselectAll()
        {
            foreach (MapObject obj in new List<MapObject>(SelectedObjects))
                DeselectObject(obj);
        }

        public void DeselectObject(MapObject obj)
        {
            SelectedObjects.Remove(obj);
        }

        public void SelectObject(MapObject obj)
        {
            SelectedObjects.Add(obj);
        }

        public void NewCommand(BaseCommand command)
        {
            command.Execute();
            _undoCommands.Add(command);
            _redoCommands.Clear();
            if (command is TransformPositionCommand || command is TransformPositionRotationCommand || command is TransformScaleCommand )
                _menu.SyncInspector();
        }

        protected override void OnFinishLoading()
        {
            MapScript = MapManager.MapScript;
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _positionGizmo = PositionGizmo.Create();
            _outlineGizmo = OutlineGizmo.Create();
            _rotationGizmo = RotationGizmo.Create();
            _scaleGizmo = ScaleGizmo.Create();
            _currentObjectId = GetHighestObjectId();
            _menu.ShowHierarchyPanel();
            LogicEvaluator = CustomLogicManager.GetEditorEvaluator(MapScript.Logic);
            CurrentGizmo = _positionGizmo;
            if (MapLoader.Errors.Count > 0)
                _menu.ErrorPopup.Show(string.Join("\n", MapLoader.Errors));
        }

        protected override void Awake()
        {
            base.Awake();
            _input = SettingsManager.InputSettings.MapEditor;
        }

        protected void Update()
        {
            UpdateInput();
            UpdateDrag();
        }

        protected void UpdateDrag()
        {
            if (_menu == null)
                return;
            if (CurrentGizmo != null && CurrentGizmo.IsActive())
                _isDrag = false;
            if (_menu.IsPopupActive())
                _isDrag = false;
            var system = EventSystem.current;
            var selected = system.currentSelectedGameObject;
            if (selected != null && selected.GetComponent<InputField>() != null)
                _isDrag = false;
            if (_isDrag)
            {
                _menu.SetDrag(true, _dragStart, Input.mousePosition);
                if (_input.Select.GetKeyUp())
                {
                    Select(_input.Multiselect.GetKey() && SelectedObjects.Count > 0);
                    _isDrag = false;
                    _menu.SetDrag(false, Vector2.zero, Vector2.zero);
                }
            }
            else
            {
                _menu.SetDrag(false, Vector2.zero, Vector2.zero);
            }
        }

        protected void UpdateInput()
        {
            if (_menu == null)
                return;
            if (_input.SaveMap.GetKeyDown())
            {
                Save();
                return;
            }
            if (_menu.IsPopupActive())
                return;
            var system = EventSystem.current;
            var selected = system.currentSelectedGameObject;
            if (selected != null && selected.GetComponent<InputField>() != null)
                return;
            if (_input.Undo.GetKeyDown())
                Undo();
            else if (_input.Redo.GetKeyDown())
                Redo();
            else if (_input.CopyObjects.GetKeyDown())
                Copy();
            else if (_input.Paste.GetKeyDown())
                Paste();
            else if (_input.Cut.GetKeyDown())
                Cut();
            else if (_input.AddObject.GetKeyDown())
                ShowAddObject();
            else if (_input.Delete.GetKeyDown())
                Delete();
            else if (_input.Deselect.GetKeyDown())
            {
                DeselectAll();
                OnSelectionChange();
            }
            else if (_input.ChangeGizmo.GetKeyDown())
                _menu._topPanel.NextGizmo();
            else if (_input.ToggleSnap.GetKeyDown())
                _menu._topPanel.ToggleSnap();
            else if (_input.Select.GetKeyDown() && !_menu.IsMouseUI)
            {
                _isDrag = true;
                _dragStart = Input.mousePosition;
            }
            IgnoreNextSelect = false;
        }

        public void Save()
        {
            _menu._topPanel.Save();
        }

        public void OnSelectionChange()
        {
            foreach (var obj in new HashSet<MapObject>(SelectedObjects))
            {
                if (!MapLoader.IdToMapObject.ContainsKey(obj.ScriptObject.Id))
                    DeselectObject(obj);
            }
            if (SelectedObjects.Count == 1)
                _menu.ShowInspector(new List<MapObject>(SelectedObjects)[0]);
            else
                _menu.HideInspector();
            _menu.HierarchyPanel.SyncSelectedItems();
            SyncGizmos();
        }

        public void SyncGizmos()
        {
            _outlineGizmo.OnSelectionChange();
            _positionGizmo.OnSelectionChange();
            _rotationGizmo.OnSelectionChange();
            _scaleGizmo.OnSelectionChange();
        }

        public void SetGizmo(string gizmo)
        {
            if (gizmo == "Position")
                CurrentGizmo = _positionGizmo;
            else if (gizmo == "Rotation")
                CurrentGizmo = _rotationGizmo;
            else
                CurrentGizmo = _scaleGizmo;
            SyncGizmos();
        }

        public int GetNextObjectId()
        {
            _currentObjectId++;
            return _currentObjectId;
        }

        protected int GetHighestObjectId()
        {
            int max = 0;
            foreach (int id in MapLoader.IdToMapObject.Keys)
            {
                max = Mathf.Max(max, id);
            }
            return max;
        }
    }
}
