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
using Assets.Scripts.ApplicationManagers;
using UnityEngine.EventSystems;
using ExitGames.Client.Photon.StructWrapping;


namespace UI
{
    

    class MapEditorHierarchyPanel: HeadedPanel
    {
        protected override float Width => 300f;
        protected override float Height => 1005f;
        protected override float TopBarHeight => 50f;
        protected override float BottomBarHeight => 50f;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 10;
        protected override bool ScrollBar => true;

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
        private Transform _topGroup;

        // Pooling
        private const int MaxVisibleObjects = 35;
        private int _visibleIndex = 0;
        private VirtualTreeView _treeView = new VirtualTreeView();
        //private GameObject _scroll;
        //private Scrollbar _scrollBar;
        private List<VirtualTreeViewItem> _visibleTreeViewItems = new List<VirtualTreeViewItem>();
        private ScrollRect _scrollRect;
        private GameObject _scroll;
        private Scrollbar _scrollBar;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);
            var style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);

            TopBar.Find("Label").gameObject.SetActive(false);

            _topGroup = ElementFactory.CreateHorizontalGroup(TopBar, 10f, TextAnchor.MiddleLeft).transform;
            _searchInput = ElementFactory.CreateInputSetting(_topGroup, style, _searchSetting, "", elementWidth: 100f, elementHeight: 32f,
                onEndEdit: () => Sync()).GetComponent<InputSettingElement>();
            //ElementFactory.CreateIconButton(_topGroup, _style, "Icons/Navigation/ArrowLeftIcon", onClick: () => OnPageClick(true), elementHeight: 18f, elementWidth: 18f);
            //ElementFactory.CreateIconButton(_topGroup, _style, "Icons/Navigation/ArrowRightIcon", onClick: () => OnPageClick(false), elementHeight: 18f, elementWidth: 18f);
            // CreateHorizontalDivider(SinglePanel);
            _pageLabel = ElementFactory.CreateDefaultLabel(_topGroup, style, "0/0").GetComponent<Text>();
            _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 0, 0, 0);

            var panel = transform.Find("SinglePanelContent(Clone)");

            _scrollRect = panel.GetComponent<ScrollRect>();
            _scroll = panel.Find("Scrollbar").gameObject;
            _scrollBar = _scroll.GetComponent<Scrollbar>();

            _scrollRect.onValueChanged.AddListener(OnScrollChanged);
            _scrollRect.scrollSensitivity = 10f;

            InitializePooledElements();

            Sync();
        }

        #region TreeViewAlgorithms
        /// <summary>
        /// This code returns a flattened list of mapobjects to draw to the hierarchy as well as their order.
        /// This code will be rerun every resync and needs to be efficient.
        /// </summary>
        /// <returns>A list of ids that should be shown in the hierarchy panel.</returns>
        public List<int> GetVisibleItems()
        {
            // foreach (MapObject obj in MapLoader.IdToMapObject.Values)
            List<int> items = new List<int>();
            GetOrderedChildren(-1, items);

            // For now so that we can check other maps,
            if (items.Count == 0) GetOrderedChildren(0, items); // TODO: Remove this when map migration is added.

            return items;
        }

        private void GetOrderedChildren(int parent, List<int> items)
        {
            if (MapLoader.IdToChildren.ContainsKey(parent) == false)
                return;

            IEnumerable<int> orderedChildren = MapLoader.IdToChildren[parent].OrderBy(id => MapLoader.IdToMapObject[id].SiblingIndex);
            foreach (int child in orderedChildren)
            {
                items.Add(child);
                if (MapLoader.IdToMapObject[child].Expanded)
                    GetOrderedChildren(child, items);
            }
        }

        /// <summary>
        /// Remove the element from its parent and reorder the sibling indices.
        /// </summary>
        /// <param name="id"></param>
        public void RemoveFromParent(int id)
        {
            MapObject obj = MapLoader.IdToMapObject[id];
            if (obj == null)
                return;

            int parent = obj.Parent;
            int siblingIndex = obj.SiblingIndex;

            MapLoader.IdToMapObject[id].ScriptObject.Parent = -1;
            IEnumerable<int> orderedChildren = MapLoader.IdToChildren[parent].OrderBy(id => MapLoader.IdToMapObject[id].SiblingIndex);

        }

        #endregion


        /// <summary>
        /// Called when the scroll bar is moved.
        /// Use to determine the position in the virtual list to being drawing.
        /// </summary>
        /// <param name="vec"></param>
        public void OnScrollChanged(Vector2 vec)
        {
            // Find the _visibleIndex based on the scroll position
            _visibleIndex = (int)((1f - vec.y) * (MapLoader.IdToMapObject.Values.Count - MaxVisibleObjects));
            _scrollRect.content.sizeDelta = new Vector2(_scrollRect.content.sizeDelta.x, _visibleTreeViewItems.Count * 25f);
            _scrollRect.verticalScrollbar.size = Mathf.Min(1f, (MaxVisibleObjects / (float)_visibleTreeViewItems.Count));
        }

        public override void Show()
        {
            base.Show();
        }

        private void OnScroll()
        {
            int startIndex = Mathf.Max(0, _visibleIndex);
            int endIndex = Mathf.Min(startIndex + MaxVisibleObjects, _visibleTreeViewItems.Count);
            for (int i = startIndex; i < endIndex; i++)
            {
                var item = _visibleTreeViewItems[i];
                bool hasChildren = _treeView.HasChildren(item);
                var go = _items[i - startIndex];
                go.SetActive(true);
                RedrawPooled(go.GetComponent<MapEditorHierarchyButton>(), MapLoader.IdToMapObject[item.ID], item.Level, item.SiblingID, item.Expanded, hasChildren);
                if (!_idToItem.ContainsKey(item.ID))
                    _idToItem.Add(item.ID, go);
            }

            _pageLabel.text = $"{endIndex - startIndex}/{MapLoader.IdToMapObject.Values.Count} ({startIndex}-{endIndex})";
        }

        int _targetID = -1;
        int _targetParent = -1;
        int? _targetSibling = null;
        MapEditorHierarchyButton _lastHighlighted = null;

        private void Update()
        {
            OnScroll();

            if (_lastHighlighted != null)
                _lastHighlighted.SetHighlight(false);

            // mouse down, mouse hold, mouse up
            if (Input.GetMouseButtonDown(0))
            {
                _draggingItem = false;
                _targetID = -1;
                _targetParent = -1;
                _targetSibling = null;
                _lastHighlighted = FindButtonMouseOver();
                if (_lastHighlighted != null)
                {
                    _lastHighlighted.SetHighlight(true);
                    _draggingItem = true;
                    _targetID = _lastHighlighted.BoundID;
                }
            }
            else if (Input.GetMouseButton(0) && _draggingItem)
            {
                _lastHighlighted = FindButtonMouseOver();
                if (_lastHighlighted != null)
                {
                    _lastHighlighted.SetHighlight(true);
                }
            }
            else if (Input.GetMouseButtonUp(0) && _draggingItem)
            {
                _lastHighlighted = FindButtonMouseOver();
                if (_lastHighlighted != null)
                {
                    _lastHighlighted.SetHighlight(false);
                    _draggingItem = false;
                    _targetParent = _lastHighlighted.BoundID;
                    _targetSibling = 0;

                    if (_targetID != _targetParent)
                    {
                        _gameManager.NewCommand(new SetParentCommand(new List<MapObject>() { MapLoader.IdToMapObject[_targetID] }, _targetParent, _targetSibling));
                    }
                }
            }

        }


        /// <summary>
        /// Using the button sizes, find the button the mouse is currently over.
        /// </summary>
        /// <returns></returns>
        private MapEditorHierarchyButton FindButtonMouseOver()
        {
            Vector2 mouse = Input.mousePosition;
            foreach (var item in _items)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(item.GetComponent<RectTransform>(), mouse))
                    return item.GetComponent<MapEditorHierarchyButton>();
            }
            return null;
        }


        /// <summary>
        /// Create a fixed number of MapHierarchyButtons for reuse, disabled by default.
        /// </summary>
        public void InitializePooledElements()
        {
            for (int i = 0; i < MaxVisibleObjects; i++)
            {
                var go = CreatePooledItem();
                go.SetActive(false);
                _items.Add(go);
            }
        }

        /// <summary>
        /// We will be removing the concept of pagination and instead rendering a fixed number of objects from the virtual tree view.
        /// We need to figure out the item offset based on the scroll position and then draw the next X items.
        /// </summary>
        public void SyncPooled()
        {
            // Clear and repopulate the treeview
            foreach (GameObject go in _items)
                go.SetActive(false);
            _idToItem.Clear();
            _idToIndex.Clear();
            _indexToId.Clear();
            _selected.Clear();

            // Copy what ids were expanded
            var expandedIds = _treeView.Items.Where(item => item.Expanded).Select(item => item.ID).ToList();

            _treeView.Clear();
            foreach (MapObject obj in MapLoader.IdToMapObject.Values)
            {
                // if the obj is not in the treeview, add it
                _treeView.AddItem(new VirtualTreeViewItem()
                {
                    ID = obj.ScriptObject.Id,
                    ParentID = obj.ScriptObject.Parent,
                    Level = 0,
                    GameObject = null,
                    Expanded = expandedIds.Contains(obj.ScriptObject.Id)
                });

            }

            _visibleTreeViewItems = _treeView.GetFlattenedTree();
        }


        public void Sync()
        {
            SyncPooled();
            SyncSelectedItems();
        }

        private void RedrawPooled(MapEditorHierarchyButton element, MapObject obj, int level, int siblingID, bool expanded, bool hasChildren)
        {
            element.Bind(obj.ScriptObject.Name, obj.ScriptObject.Id, level, _selected.Contains(obj.ScriptObject.Id));
            element.SetExpanded(expanded, hasChildren);

            if (_selected.Contains(obj.ScriptObject.Id))
                element.Highlight.SetActive(true);
            else
                element.Highlight.SetActive(false);
        }

        private GameObject CreatePooledItem()
        {
            var go = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/MapEditorHierarchyButton");
            var button = go.AddComponent<MapEditorHierarchyButton>();
            button.Setup(
                Width,
                () => OnButtonClick(button.BoundID),
                () => OnElementCallback(button.BoundID, true),
                () => OnElementCallback(button.BoundID, false)
            );
            button.Bind(string.Empty, -1, -1, false);
            return go;
        }

        private void OnElementCallback(int id, bool expanded)
        {
            if (expanded)
                OnElementExpand(id);
            else
                OnElementClose(id);
            Sync();
        }

        private void OnElementExpand(int id)
        {
            Debug.Log("Expand");
            MapObject obj = MapLoader.IdToMapObject[id];
            if (obj == null)
                return;

            // find the virtual element and set expanded to true
            var item = _treeView.Items.FirstOrDefault(i => i.ID == id);
            if (item != null)
            {
                item.Expanded = true;
            }
        }

        private void OnElementClose(int id)
        {
            Debug.Log("Close");
            // find the virtual element and set expanded to false
            var item = _treeView.Items.FirstOrDefault(i => i.ID == id);
            if (item != null)
            {
                item.Expanded = false;
            }
        }

        private void OnPageClick(bool left)
        {
            Sync();
        }

        private void OnButtonClick(int id)
        {
            if (_menu.IsPopupActive())
                return;
            bool multi = SettingsManager.InputSettings.MapEditor.Multiselect.GetKey();
            if (_selected.Contains(id))
            {
                if (!multi && _gameManager.SelectedObjects.Count > 1)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    // TODO: Add a subselection so that we can move child elements with the parent.
                    //_treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
                    _gameManager.OnSelectionChange();
                }
                else if (multi)
                {
                    _gameManager.DeselectObject(MapLoader.IdToMapObject[id]);
                    //_treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.DeselectObject(MapLoader.IdToMapObject[item.ID]));
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
                    //_treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
                    _gameManager.OnSelectionChange();
                }
                else if (_selected.Count > 0 && !multi)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    //_treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
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

    }
}
