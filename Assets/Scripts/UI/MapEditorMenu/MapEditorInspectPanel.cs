using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;
using Map;
using MapEditor;
using Utility;
using UnityEngine.EventSystems;
using System.Globalization;
using CustomLogic;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

namespace UI
{
    class MapEditorInspectPanel : HeadedPanel
    {
        protected override float Width => 400f;
        protected override float Height => 990f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 15;
        protected override bool ScrollBar => true;
        private MapEditorGameManager _gameManager;
        private MapEditorMenu _menu;
        private MapObject _mapObject;
        private IntSetting _parent = new IntSetting();
        private StringSetting _name = new StringSetting();
        private BoolSetting _active = new BoolSetting();
        private BoolSetting _static = new BoolSetting();
        private BoolSetting _networked = new BoolSetting();
        private BoolSetting _visible = new BoolSetting();
        private FloatSetting _positionX = new FloatSetting();
        private FloatSetting _positionY = new FloatSetting();
        private FloatSetting _positionZ = new FloatSetting();
        private FloatSetting _rotationX = new FloatSetting();
        private FloatSetting _rotationY = new FloatSetting();
        private FloatSetting _rotationZ = new FloatSetting();
        private FloatSetting _scaleX = new FloatSetting();
        private FloatSetting _scaleY = new FloatSetting();
        private FloatSetting _scaleZ = new FloatSetting();
        private StringSetting _collideMode = new StringSetting();
        private StringSetting _collideWith = new StringSetting();
        private StringSetting _physicsMaterial = new StringSetting();
        private StringSetting _shader = new StringSetting();
        private ColorSetting _color = new ColorSetting();
        private ColorSetting _reflectColor = new ColorSetting();
        private StringSetting _texture = new StringSetting("Misc/None");
        private FloatSetting _tilingX = new FloatSetting(1f);
        private FloatSetting _tilingY = new FloatSetting(1f);
        private FloatSetting _offsetX = new FloatSetting();
        private FloatSetting _offsetY = new FloatSetting();
        private List<Dictionary<string, BaseSetting>> _components = new List<Dictionary<string, BaseSetting>>();
        private List<string> _componentNames = new List<string>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
        }

        private bool HasNonConvexMeshCollider(MapObject mapObject)
        {
            foreach (Collider c in mapObject.GameObject.GetComponentsInChildren<Collider>())
            {
                if (c is MeshCollider && !((MeshCollider)c).convex && c.name != "NonConvexMeshCollider")
                    return true;
            }
            return false;
        }

        public void Show(MapObject mapObject)
        {
            base.Show();
            _mapObject = mapObject;
            SyncSettings();
            ElementStyle style = new ElementStyle(fontSize: 18, titleWidth: 80f, spacing: 10f, themePanel: ThemePanel);
            var label = ElementFactory.CreateDefaultLabel(SinglePanel, style, "Object Id: " + _mapObject.ScriptObject.Id.ToString() + " | " + "Asset: " + _mapObject.ScriptObject.Asset, alignment: TextAnchor.MiddleLeft);
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            style.TitleWidth = 45f;
            ElementFactory.CreateToggleSetting(group, style, _active, "Active", elementWidth: 25f, elementHeight: 25f, onValueChanged: () => OnChange());
            ElementFactory.CreateToggleSetting(group, style, _static, "Static", elementWidth: 25f, elementHeight: 25f, onValueChanged: () => OnChange());
            style.TitleWidth = 80f;
            ElementFactory.CreateToggleSetting(group, style, _networked, "Networked", elementWidth: 25f, elementHeight: 25f, onValueChanged: () => OnChange());
            ElementFactory.CreateInputSetting(SinglePanel, style, _name, "Name", elementWidth: 140f, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(SinglePanel, style, _parent, "Parent Id", elementWidth: 140f, elementHeight: 35f, onEndEdit: () => OnChange());
            CreateHorizontalDivider(SinglePanel);
            float inputWidth = 80f;
            style = new ElementStyle(fontSize: 18, titleWidth: 15f, spacing: 5f, themePanel: ThemePanel);
            ElementFactory.CreateDefaultLabel(SinglePanel, style, "Position");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 15f, TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateInputSetting(group, style, _positionX, "X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _positionY, "Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _positionZ, "Z", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateDefaultLabel(SinglePanel, style, "Rotation");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 15f, TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateInputSetting(group, style, _rotationX, "X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _rotationY, "Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _rotationZ, "Z", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateDefaultLabel(SinglePanel, style, "Scale");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 15f, TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateInputSetting(group, style, _scaleX, "X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _scaleY, "Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            ElementFactory.CreateInputSetting(group, style, _scaleZ, "Z", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            CreateHorizontalDivider(SinglePanel);
            style = new ElementStyle(fontSize: 18, titleWidth: 160f, spacing: 20f, themePanel: ThemePanel);
            if (!HasNonConvexMeshCollider(_mapObject))
            {
                ElementFactory.CreateDropdownSetting(SinglePanel, style, _collideMode, "Collide Mode",
                new string[] { MapObjectCollideMode.Physical, MapObjectCollideMode.Region, MapObjectCollideMode.None },
                elementHeight: 30f, onDropdownOptionSelect: () => OnChange());
            }
            else
            {
                ElementFactory.CreateDropdownSetting(SinglePanel, style, _collideMode, "Collide Mode",
                new string[] { MapObjectCollideMode.Physical, MapObjectCollideMode.None },
                elementHeight: 30f, onDropdownOptionSelect: () => OnChange());
            }
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _collideWith, "Collide With",
                new string[] { MapObjectCollideWith.Entities, MapObjectCollideWith.Characters, MapObjectCollideWith.Projectiles, MapObjectCollideWith.Hitboxes,
                    MapObjectCollideWith.MapObjects, MapObjectCollideWith.All}, elementHeight: 30f, onDropdownOptionSelect: () => OnChange());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _physicsMaterial, "Physics Material",
                            new string[] { MapObjectPhysicsMaterial.Default, MapObjectPhysicsMaterial.Ice }, elementHeight: 30f, onDropdownOptionSelect: () => OnChange());
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateToggleSetting(SinglePanel, style, _visible, "Visible", elementWidth: 25f, elementHeight: 25f, onValueChanged: () => OnChange());
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _shader, "Shader",
               new string[] { MapObjectShader.Default, MapObjectShader.Basic, MapObjectShader.Transparent, MapObjectShader.Reflective, MapObjectShader.DefaultNoTint, MapObjectShader.DefaultTiled },
               elementHeight: 30f, onDropdownOptionSelect: () => OnChange());
            if (_shader.Value != MapObjectShader.DefaultNoTint)
            {
                ElementFactory.CreateColorSetting(SinglePanel, style, _color, "Color", _menu.ColorPickPopup, onChangeColor: () => OnChange(),
               elementHeight: 25f);
            }
            if (MapObjectShader.IsLegacyShader(_shader.Value))
            {
                group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingX, "Tiling X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingY, "Tiling Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            }
            else if (_shader.Value == MapObjectShader.DefaultTiled)
            {
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingX, "Tiling X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingY, "Tiling Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            }
            else if (_shader.Value != MapObjectShader.Default && _shader.Value != MapObjectShader.DefaultNoTint && _shader.Value != MapObjectShader.DefaultTiled)
            {
                if (_shader.Value == MapObjectShader.Reflective)
                {
                    ElementFactory.CreateColorSetting(SinglePanel, style, _reflectColor, "Reflect color", _menu.ColorPickPopup,
                        onChangeColor: () => OnChange(), elementHeight: 25f);
                }
                group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
                label = ElementFactory.CreateDefaultLabel(group, style, "Texture", alignment: TextAnchor.MiddleLeft);
                label.GetComponent<LayoutElement>().preferredWidth = 160f;
                ElementFactory.CreateDefaultButton(group, style, _texture.Value, onClick: () => OnButtonClick("Texture"));
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingX, "Tiling X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
                ElementFactory.CreateInputSetting(SinglePanel, style, _tilingY, "Tiling Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
                ElementFactory.CreateInputSetting(SinglePanel, style, _offsetX, "Offset X", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
                ElementFactory.CreateInputSetting(SinglePanel, style, _offsetY, "Offset Y", elementWidth: inputWidth, elementHeight: 35f, onEndEdit: () => OnChange());
            }
            CreateHorizontalDivider(SinglePanel);

            for (int i = 0; i < _components.Count; i++)
            {
                ElementFactory.CreateDefaultLabel(SinglePanel, style, _componentNames[i]);
                var settings = _components[i];
                string description = CustomLogicManager.GetModeDescription(settings);
                if (description != "")
                    ElementFactory.CreateDefaultLabel(SinglePanel, style, description, alignment: TextAnchor.MiddleLeft);
                var tooltips = new Dictionary<string, string>();
                var dropboxes = new Dictionary<string, string[]>();
                foreach (string key in settings.Keys)
                {
                    BaseSetting setting = settings[key];
                    if (key.EndsWith("Tooltip") && setting is StringSetting)
                        tooltips[key.Substring(0, key.Length - 7)] = ((StringSetting)setting).Value;
                    else if (key.EndsWith("Dropbox") && setting is StringSetting)
                    {
                        List<string> options = new List<string>();
                        foreach (string option in ((StringSetting)setting).Value.Split(','))
                            options.Add(option.Trim());
                        if (options.Count == 0)
                            options.Add("None");
                        dropboxes[key.Substring(0, key.Length - 7)] = options.ToArray();
                    }
                }
                foreach (string key in settings.Keys)
                {
                    var setting = settings[key];
                    if (key == "Description")
                        continue;
                    if (key.EndsWith("Tooltip") && setting is StringSetting)
                        continue;
                    if (key.EndsWith("Dropbox") && setting is StringSetting)
                        continue;
                    string tooltip = "";
                    if (tooltips.ContainsKey(key))
                        tooltip = tooltips[key];
                    if (dropboxes.ContainsKey(key) && setting is StringSetting)
                        ElementFactory.CreateDropdownSetting(SinglePanel, style, setting, key, dropboxes[key], tooltip, elementWidth: 140f, elementHeight: 35f, onDropdownOptionSelect: () => OnChange());
                    else if (setting is BoolSetting)
                        ElementFactory.CreateToggleSetting(SinglePanel, style, setting, key, tooltip, onValueChanged: () => OnChange());
                    else if (setting is StringSetting || setting is FloatSetting || setting is IntSetting)
                        ElementFactory.CreateInputSetting(SinglePanel, style, setting, key, tooltip, elementWidth: 140f, elementHeight: 35f, onEndEdit: () => OnChange());
                    else if (setting is ColorSetting)
                        ElementFactory.CreateColorSetting(SinglePanel, style, setting, key, _menu.ColorPickPopup, tooltip, onChangeColor: () => OnChange());
                    else if (setting is Vector3Setting)
                        ElementFactory.CreateVector3Setting(SinglePanel, style, setting, key, _menu.Vector3Popup, tooltip, onChangeVector: () => OnChange());
                }
                string name = "DeleteComponent" + i.ToString();
                ElementFactory.CreateDefaultButton(SinglePanel, style, "Delete", onClick: () => OnButtonClick(name));
                CreateHorizontalDivider(SinglePanel);
            }
            ElementFactory.CreateDefaultButton(SinglePanel, style, "Add Component", onClick: () => OnButtonClick("AddComponent"));
            SinglePanel.gameObject.SetActive(false);
            StartCoroutine(WaitAndEnablePanel());
        }

        private IEnumerator WaitAndEnablePanel()
        {
            yield return new WaitForEndOfFrame();
            SinglePanel.gameObject.SetActive(true);
        }

        private void OnButtonClick(string name)
        {
            if (name == "Texture")
                _menu.TexturePopup.Show();
            else if (name == "AddComponent")
            {
                var components = _gameManager.LogicEvaluator.GetComponentNames();
                components.Sort();
                _menu.SelectComponentPopup.ShowLoad(components, "Component", onLoad: () => OnAddComponent());
            }
            else if (name.StartsWith("DeleteComponent"))
            {
                int index = int.Parse(name.Substring("DeleteComponent".Length));
                _menu.ConfirmPopup.Show("Delete this component?", onConfirm: () => OnDeleteComponent(index));
            }
        }

        private void OnAddComponent()
        {
            string component = _menu.SelectComponentPopup.FinishSetting.Value;
            if (!_componentNames.Contains(component))
            {
                _components.Add(_gameManager.LogicEvaluator.GetComponentSettings(component, new List<string>()));
                _componentNames.Add(component);
            }
            OnChange();
        }

        private void OnDeleteComponent(int index)
        {
            _components.RemoveAt(index);
            _componentNames.RemoveAt(index);
            OnChange();
        }

        public void SyncSettings()
        {
            var script = (MapScriptSceneObject)_mapObject.ScriptObject;
            _name.Value = script.Name;
            _active.Value = script.Active;
            _static.Value = script.Static;
            _networked.Value = script.Networked;
            _parent.Value = script.Parent;
            _visible.Value = script.Visible;
            var position = script.GetPosition();
            _positionX.Value = position.x;
            _positionY.Value = position.y;
            _positionZ.Value = position.z;
            var rotation = script.GetRotation();
            _rotationX.Value = rotation.x;
            _rotationY.Value = rotation.y;
            _rotationZ.Value = rotation.z;
            var scale = script.GetScale();
            _scaleX.Value = scale.x;
            _scaleY.Value = scale.y;
            _scaleZ.Value = scale.z;
            _collideMode.Value = script.CollideMode;
            _collideWith.Value = script.CollideWith;
            _physicsMaterial.Value = script.PhysicsMaterial;
            _shader.Value = script.Material.Shader;
            _color.Value = script.Material.Color;
            if (script.Material is MapScriptLegacyMaterial)
            {
                var material = (MapScriptLegacyMaterial)script.Material;
                _tilingX.Value = material.Tiling.x;
                _tilingY.Value = material.Tiling.y;
            }
            else if (script.Material is MapScriptDefaultTiledMaterial)
            {
                var material = (MapScriptDefaultTiledMaterial)script.Material;
                _tilingX.Value = material.Tiling.x;
                _tilingY.Value = material.Tiling.y;
            }    
            else if (typeof(MapScriptBasicMaterial).IsAssignableFrom(script.Material.GetType()))
            {
                var material = (MapScriptBasicMaterial)script.Material;
                _texture.Value = material.Texture;
                _tilingX.Value = material.Tiling.x;
                _tilingY.Value = material.Tiling.y;
                _offsetX.Value = material.Offset.x;
                _offsetY.Value = material.Offset.y;
                if (script.Material is MapScriptReflectiveMaterial)
                    _reflectColor.Value = ((MapScriptReflectiveMaterial)script.Material).ReflectColor;
            }
            _components = new List<Dictionary<string, BaseSetting>>();
            _componentNames = new List<string>();
            foreach (var component in ((MapScriptSceneObject)_mapObject.ScriptObject).Components)
            {
                _componentNames.Add(component.ComponentName);
                _components.Add(_gameManager.LogicEvaluator.GetComponentSettings(component.ComponentName, component.Parameters));
            }
            SyncSettingElements();
        }

        public void OnSelectTexture(string texture)
        {
            _texture.Value = BuiltinMapTextures.AllTextures[texture].Texture;
            OnChange();
        }

        public void OnChange()
        {
            bool needRefresh = false;
            var script = (MapScriptSceneObject)_mapObject.ScriptObject;
            bool nameChange = script.Name != _name.Value;
            script.Name = _name.Value;
            script.Active = _active.Value;
            script.Static = _static.Value;
            script.Networked = _networked.Value;
            script.Visible = _visible.Value;
            script.Parent = _parent.Value;
            if (script.Parent > 0)
            {
                if (!MapLoader.IdToMapObject.ContainsKey(script.Parent) || script.Parent == script.Id || MapLoader.IdToMapObject[script.Parent].Parent == script.Id)
                {
                    script.Parent = 0;
                    _parent.Value = 0;
                }
            }
            var newPosition = new Vector3(_positionX.Value, _positionY.Value, _positionZ.Value);
            if (script.GetPosition() != newPosition)
            {
                _mapObject.GameObject.transform.position = newPosition;
                _gameManager.NewCommand(new TransformPositionCommand(new List<MapObject>() { _mapObject }));
            }
            var newRotation = new Vector3(_rotationX.Value, _rotationY.Value, _rotationZ.Value);
            if (script.GetRotation() != newRotation)
            {
                _mapObject.GameObject.transform.rotation = Quaternion.Euler(newRotation);
                _gameManager.NewCommand(new TransformRotationCommand(new List<MapObject>() { _mapObject }));
            }
            var newScale = new Vector3(_scaleX.Value, _scaleY.Value, _scaleZ.Value);
            if (script.GetScale() != newScale)
            {
                _mapObject.GameObject.transform.localScale = Util.MultiplyVectors(_mapObject.BaseScale, newScale);
                _gameManager.NewCommand(new TransformScaleCommand(new List<MapObject>() { _mapObject }));
            }
            script.CollideMode = _collideMode.Value;
            script.CollideWith = _collideWith.Value;
            script.PhysicsMaterial = _physicsMaterial.Value;
            if (_shader.Value != script.Material.Shader)
            {
                if (_shader.Value == MapObjectShader.Default || _shader.Value == MapObjectShader.DefaultNoTint)
                {
                    script.Material = new MapScriptBaseMaterial();
                    _color.Value = new Color255();
                }
                else if (_shader.Value == MapObjectShader.DefaultTiled)
                {
                    script.Material = new MapScriptDefaultTiledMaterial();
                    _color.Value = new Color255();
                }
                else if (_shader.Value == MapObjectShader.Basic || _shader.Value == MapObjectShader.Transparent)
                    script.Material = new MapScriptBasicMaterial();
                else if (_shader.Value == MapObjectShader.Reflective)
                    script.Material = new MapScriptReflectiveMaterial();
                else
                    script.Material = new MapScriptLegacyMaterial();
                needRefresh = true;
            }
            script.Material.Shader = _shader.Value;
            script.Material.Color = _color.Value;
            if (script.Material is MapScriptLegacyMaterial)
            {
                var material = (MapScriptLegacyMaterial)script.Material;
                material.Tiling = new Vector2(_tilingX.Value, _tilingY.Value);
            }
            else if (script.Material is MapScriptDefaultTiledMaterial)
            {
                var material = (MapScriptDefaultTiledMaterial)script.Material;
                material.Tiling = new Vector2(_tilingX.Value, _tilingY.Value);
            }
            else if (typeof(MapScriptBasicMaterial).IsAssignableFrom(script.Material.GetType()))
            {
                var material = (MapScriptBasicMaterial)script.Material;
                if (material.Texture != _texture.Value)
                    needRefresh = true;
                material.Texture = _texture.Value;
                material.Tiling = new Vector2(_tilingX.Value, _tilingY.Value);
                material.Offset = new Vector2(_offsetX.Value, _offsetY.Value);
                if (material is MapScriptReflectiveMaterial)
                    ((MapScriptReflectiveMaterial)material).ReflectColor = _reflectColor.Value;
            }
            if (script.Components.Count != _componentNames.Count)
                needRefresh = true;
            var oldComponentDict = new Dictionary<string, List<string>>();
            foreach (var component in script.Components)
                oldComponentDict.Add(component.ComponentName, component.Parameters);
            List<MapScriptComponent> newComponents = new List<MapScriptComponent>();
            for (int i = 0; i < _componentNames.Count; i++)
            {
                var newParameters = new List<string>();
                var settings = _components[i];
                HashSet<string> parsedParameters = new HashSet<string>();
                foreach (string key in settings.Keys)
                {
                    parsedParameters.Add(key);
                    if (key == "Description")
                        continue;
                    if (key.EndsWith("Tooltip") && settings[key] is StringSetting)
                        continue;
                    if (key.EndsWith("Dropbox") && settings[key] is StringSetting)
                        continue;
                    newParameters.Add(key + ":" + SerializeSetting(settings[key]));
                }
                if (oldComponentDict.ContainsKey(_componentNames[i]))
                {
                    var oldParameters = oldComponentDict[_componentNames[i]];
                    foreach (string oldParameter in oldParameters)
                    {
                        string key = oldParameter.Split(':')[0];
                        if (!parsedParameters.Contains(key))
                            newParameters.Add(oldParameter);
                    }
                }
                var component = new MapScriptComponent();
                component.ComponentName = _componentNames[i];
                component.Parameters = newParameters;
                newComponents.Add(component);
            }
            script.Components = newComponents;
            MapLoader.SetMaterial(_mapObject.GameObject, script.Asset, script.Material, script.Visible, true);
            _gameManager.SyncGizmos();
            if (needRefresh)
                _menu.ShowInspector(_mapObject);
            if (nameChange)
                _menu.SyncHierarchyPanel();
        }

        public string SerializeSetting(BaseSetting setting)
        {
            if (setting is FloatSetting)
                return ((FloatSetting)setting).Value.ToString();
            else if (setting is IntSetting)
                return ((IntSetting)setting).Value.ToString();
            else if (setting is BoolSetting)
                return ((BoolSetting)setting).Value ? "true" : "false";
            else if (setting is StringSetting)
                return ((StringSetting)setting).Value.Replace(',', ' ').Replace(':', ' ').Replace('|', ' ');
            else if (setting is ColorSetting)
            {
                var color = ((ColorSetting)setting).Value;
                return string.Join("/", new string[] { color.R.ToString(), color.G.ToString(), color.B.ToString(), color.A.ToString() });
            }
            else if (setting is Vector3Setting)
            {
                var vector = ((Vector3Setting)setting).Value;
                return string.Join("/", new string[] { vector.x.ToString(), vector.y.ToString(), vector.z.ToString() });
            }
            return string.Empty;
        }

        private void Update()
        {
            return;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var system = EventSystem.current;
                var selected = system.currentSelectedGameObject;
                if (selected == null || (selected.transform.parent != SinglePanel && selected.transform.parent.parent != SinglePanel)
                    || selected.GetComponent<InputField>() == null)
                    return;
                var selectable = selected.GetComponent<Selectable>();
                if (selectable == null)
                    return;
                var nextRight = selectable.FindSelectableOnRight();
                var nextDown = selectable.FindSelectableOnDown();
                while (nextRight != null || nextDown != null)
                {
                    Debug.Log(nextRight);
                    if (nextRight != null)
                    {
                        var inputField = nextRight.GetComponent<InputField>();
                        if (inputField != null)
                        {
                            inputField.OnPointerClick(new PointerEventData(system));
                            system.SetSelectedGameObject(nextRight.gameObject, new BaseEventData(system));
                            return;
                        }
                    }
                    else if (nextDown != null)
                    {
                        var inputField = nextDown.GetComponent<InputField>();
                        if (inputField != null)
                        {
                            inputField.OnPointerClick(new PointerEventData(system));
                            system.SetSelectedGameObject(nextDown.gameObject, new BaseEventData(system));
                            return;
                        }
                    }
                    if (nextRight != null)
                    {
                        nextRight = nextRight.FindSelectableOnRight();
                        nextDown = nextRight.FindSelectableOnDown();
                    }
                    else
                    {
                        nextRight = nextDown.FindSelectableOnRight();
                        nextDown = nextDown.FindSelectableOnDown();
                    }
                }
            }
        }
    }
}