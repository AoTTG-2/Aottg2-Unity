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

namespace UI
{
    class MapEditorHierarchyPanel: HeadedPanel
    {
        protected override float Width => 260f;
        protected override float Height => 1005f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 10;
        private List<GameObject> _items = new List<GameObject>();
        private Dictionary<int, GameObject> _idToItem = new Dictionary<int, GameObject>();
        private Dictionary<int, int> _idToIndex = new Dictionary<int, int>();
        private Dictionary<int, int> _indexToId = new Dictionary<int, int>();
        private HashSet<int> _selected = new HashSet<int>();
        private int _lastClickedItem = -1;
        private float _lastclickedTime = 0f;
        private bool _draggingItem = false;
        private const float DoubleClickTime = 0.5f;
        private MapEditorMenu _menu;
        private ElementStyle _style;
        private MapEditorGameManager _gameManager;
        private InputSettingElement _searchInput;
        private StringSetting _searchSetting = new StringSetting(string.Empty);
        private Text _pageLabel;
        private int _currentPage;
        private const int ObjectsPerPage = 30;
        private Transform _topGroup;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);
            var style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);
            _topGroup = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleLeft).transform;
            _searchInput = ElementFactory.CreateInputSetting(_topGroup, style, _searchSetting, "", elementWidth: 100f, elementHeight: 32f,
                onEndEdit: () => Sync()).GetComponent<InputSettingElement>();
            ElementFactory.CreateIconButton(_topGroup, _style, "Icons/Navigation/ArrowLeftIcon", onClick: () => OnPageClick(true), elementHeight: 18f, elementWidth: 18f);
            ElementFactory.CreateIconButton(_topGroup, _style, "Icons/Navigation/ArrowRightIcon", onClick: () => OnPageClick(false), elementHeight: 18f, elementWidth: 18f);
            CreateHorizontalDivider(SinglePanel);
            _pageLabel = ElementFactory.CreateDefaultLabel(_topGroup, style, "0/0").GetComponent<Text>();
            Sync();
        }

        public override void Show()
        {
            base.Show();
        }

        public void Sync()
        {
            foreach (GameObject go in _items)
                Destroy(go);
            _items.Clear();
            _idToItem.Clear();
            _idToIndex.Clear();
            _indexToId.Clear();
            _selected.Clear();
            var objs = new List<MapObject>();
            string search = _searchSetting.Value.ToLower();
            foreach (MapObject obj in MapLoader.IdToMapObject.Values)
            {
                if (search == "" || obj.ScriptObject.Name.ToLower().Contains(search) || obj.ScriptObject.Id.ToString() == search)
                    objs.Add(obj);
            }
            int objectCount = objs.Count;
            int pages = (objectCount - 1) / ObjectsPerPage + 1;
            _currentPage = Mathf.Clamp(_currentPage, 1, pages);
            _pageLabel.text = _currentPage.ToString() + "/" + pages.ToString();
            for (int i = (_currentPage - 1) * ObjectsPerPage; i < _currentPage * ObjectsPerPage; i++)
            {
                if (i < objectCount)
                    CreateMapItem(objs[i], 0);
            }
            SyncSelectedItems();
            if (objectCount > 0)
                _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 0, 0, 0);
            else
                _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
        }

        private GameObject CreateMapItem(MapObject obj, int level)
        {
            string name = obj.ScriptObject.Name;
            var go = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/MapEditorHierarchyButton");
            _items.Add(go);
            _idToItem.Add(obj.ScriptObject.Id, go);
            _idToIndex.Add(obj.ScriptObject.Id, _items.Count - 1);
            _indexToId.Add(_items.Count - 1, obj.ScriptObject.Id);
            go.transform.Find("Highlight").gameObject.SetActive(false);
            go.transform.Find("Text").GetComponent<Text>().text = name;
            go.transform.Find("Text").GetComponent<LayoutElement>().preferredWidth = Width;
            go.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            go.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10 * (level + 1), 0, 0, 0);
            go.GetComponent<LayoutElement>().minWidth = Width;
            go.GetComponent<LayoutElement>().preferredWidth = Width;
            var button = go.AddComponent<MapEditorHirarchyButton>();
            button.Setup(() => OnButtonClick(obj.ScriptObject.Id), () => OnButtonRelease(obj.ScriptObject.Id));
            go.transform.Find("ArrowRightButton").gameObject.SetActive(false);
            go.transform.Find("ArrowDownButton").gameObject.SetActive(false);
            return go;
        }

        private void OnPageClick(bool left)
        {
            if (left)
                _currentPage--;
            else
                _currentPage++;
            Sync();
        }

        private void OnButtonRelease(int id)
        {
        }

        private void OnButtonClick(int id)
        {
            bool multi = SettingsManager.InputSettings.MapEditor.Multiselect.GetKey();
            if (_selected.Contains(id))
            {
                if (!multi && _gameManager.SelectedObjects.Count > 1)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange();
                }
                else if (multi)
                {
                    _gameManager.DeselectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange();
                }
                else
                {
                    var transform = SceneLoader.CurrentCamera.Cache.Transform;
                    transform.position = MapLoader.IdToMapObject[id].GameObject.transform.position - transform.forward * 50f;
                }
            }
            else
            {
                if (_selected.Count == 0 || multi)
                {
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange();
                }
                else if (_selected.Count > 0 && !multi)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange();
                }
            }
            _lastClickedItem = id;
            _lastclickedTime = Time.time;
        }
        public void SyncSelectedItems()
        {
            foreach (int selected in _selected.ToList())
            {
                MapObject value = null;
                MapLoader.IdToMapObject.TryGetValue(selected, out value);
                if (!_gameManager.SelectedObjects.Contains(value))
                {
                    _selected.Remove(selected);
                    if (_idToItem.ContainsKey(selected))
                        _idToItem[selected].transform.Find("Highlight").gameObject.SetActive(false);
                }
            }
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                if (!_selected.Contains(obj.ScriptObject.Id))
                {
                    _selected.Add(obj.ScriptObject.Id);
                    if (_idToItem.ContainsKey(obj.ScriptObject.Id))
                        _idToItem[obj.ScriptObject.Id].transform.Find("Highlight").gameObject.SetActive(true);
                }
            }
        }

        private void Update()
        {

        }
    }
}
