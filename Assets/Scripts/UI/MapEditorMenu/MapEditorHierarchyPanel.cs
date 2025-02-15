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
using log4net.Core;

namespace UI
{
    public class VirtualTreeView
    {
        public List<VirtualTreeViewItem> Items = new List<VirtualTreeViewItem>();
        private Dictionary<int, VirtualTreeViewItem> itemDictionary = new Dictionary<int, VirtualTreeViewItem>();

        public void Clear()
        {
            Items.Clear();
            itemDictionary.Clear();
        }

        // Method to add an item to the tree
        public void AddItem(VirtualTreeViewItem item)
        {
            // Set the level based on the parent's level
            if (item.ParentID == -1)
            {
                item.Level = 0; // Root level
            }
            else if (itemDictionary.TryGetValue(item.ParentID, out var parent))
            {
                item.Level = parent.Level + 2;
            }
            else
            {
                item.Level = 0; // Default to root level if parent not found
            }

            Items.Add(item);
            itemDictionary[item.ID] = item;
        }

        // Method to get the flattened tree
        public List<VirtualTreeViewItem> GetFlattenedTree()
        {
            List<VirtualTreeViewItem> flattenedTree = new List<VirtualTreeViewItem>();

            // Get all root elements
            List<VirtualTreeViewItem> rootElements = Items.Where(item => item.Level == 0).ToList();

            foreach (var item in rootElements)
            {
                if (item.Level == 0)
                {
                    flattenedTree.Add(item);
                    if (item.Expanded)
                    {
                        AddChildren(item, flattenedTree);
                    }
                }
            }
            return flattenedTree;
        }

        // Optimized method to check if the parent of an item is expanded
        private bool IsParentExpanded(VirtualTreeViewItem item)
        {
            if (item.ParentID == -1) return true; // Root items are always considered expanded
            return itemDictionary.TryGetValue(item.ParentID, out var parent) && parent.Expanded;
        }

        // Helper method to add children of an item to the flattened tree
        private void AddChildren(VirtualTreeViewItem parent, List<VirtualTreeViewItem> flattenedTree)
        {
            foreach (var item in Items)
            {
                if (item.ParentID == parent.ID)
                {
                    flattenedTree.Add(item);
                    if (item.Expanded)
                    {
                        AddChildren(item, flattenedTree);
                    }
                }
            }
        }

        public List<VirtualTreeViewItem> GetChildrenRecursive(VirtualTreeViewItem parent)
        {
            List<VirtualTreeViewItem> children = new List<VirtualTreeViewItem>();
            foreach (var item in Items)
            {
                if (item.ParentID == parent.ID)
                {
                    children.Add(item);
                    children.AddRange(GetChildrenRecursive(item));
                }
            }
            return children;
        }

        // Method to check if an item has children
        public bool HasChildren(VirtualTreeViewItem item)
        {
            foreach (var child in Items)
            {
                if (child.ParentID == item.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class VirtualTreeViewItem
    {
        public int ID;
        public int ParentID;
        public int SiblingID;
        public int Level;
        public bool Expanded;
        public GameObject GameObject;
    }


    class MapEditorHierarchyPanel: HeadedPanel
    {
        protected override float Width => 300f;
        protected override float Height => 1005f;
        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
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
        private int _currentPage;
        private const int ObjectsPerPage = 30;
        private Transform _topGroup;
        private Transform _bottomGroup;

        // Resize
        private bool _isResizing = false;
        private Vector2 _resizeStartMousePosition;
        private Vector2 _resizeStartPanelSize;
        private RectTransform _rectTransform;

        // Pooling
        private const int MaxVisibleObjects = 35;
        private int _visibleIndex = 0;
        private VirtualTreeView _treeView = new VirtualTreeView();
        private GameObject _scroll;
        private Scrollbar _scrollBar;

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

            // Create a scrollable bottom group
            _bottomGroup = ElementFactory.CreateVerticalGroup(SinglePanel, 0f, TextAnchor.UpperLeft).transform;


            _scroll = transform.Find("SinglePanelContent(Clone)/Scrollbar").gameObject;
            _scrollBar = _scroll.GetComponent<Scrollbar>();

            InitializePooledElements();

            Sync();
        }

        public override void Show()
        {
            base.Show();
        }

        private void Update()
        {
            //HandleResize();  // We can do this later but this breaks right now as the panels are obnoxiously nested to the point any resize breaks them.

            // For pooling, calculate the visible index based on scroll position using the scrollbar
            _scrollBar.numberOfSteps = MapLoader.IdToMapObject.Values.Count;
            _scrollBar.size = (float)MaxVisibleObjects / MapLoader.IdToMapObject.Values.Count;
            // Calculate the visible index based on scroll position
            _visibleIndex = (int)((1f - _scrollBar.value) * (MapLoader.IdToMapObject.Values.Count - MaxVisibleObjects));
            Sync();
        }

        /*private void HandleResize()
        {
            if (_isResizing)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 sizeDelta = currentMousePosition - _resizeStartMousePosition;
                _rectTransform.sizeDelta = _resizeStartPanelSize + new Vector2(sizeDelta.x, -sizeDelta.y);

                if (Input.GetMouseButtonUp(0))
                {
                    _isResizing = false;
                }
            }
            else
            {
                if (IsMouseOnResizableEdge())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _isResizing = true;
                        _resizeStartMousePosition = Input.mousePosition;
                        _resizeStartPanelSize = _rectTransform.sizeDelta;
                    }
                }
            }
        }

        private bool IsMouseOnResizableEdge()
        {
            Vector2 localMousePosition = _rectTransform.InverseTransformPoint(Input.mousePosition);
            Rect rect = _rectTransform.rect;
            float edgeThickness = 10f; // Thickness of the resizable edge

            return localMousePosition.x >= rect.width - edgeThickness || localMousePosition.y <= -rect.height + edgeThickness;
        }*/


        // Virtual Tree view.


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

            // Get the flattened tree
            var flattenedTree = _treeView.GetFlattenedTree();
            // Calculate the start index based on scroll position
            int startIndex = _visibleIndex;
            // Calculate the end index based on the number of objects to display
            int endIndex = Mathf.Min(startIndex + MaxVisibleObjects, flattenedTree.Count);
            // Activate only the items that are within the current view
            for (int i = startIndex; i < endIndex; i++)
            {
                var item = flattenedTree[i];
                bool hasChildren = _treeView.HasChildren(item);
                var go = _items[i - startIndex];
                go.SetActive(true);
                RedrawPooled(go.GetComponent<MapEditorHirarchyButton>(), MapLoader.IdToMapObject[item.ID], item.Level, item.SiblingID, item.Expanded, hasChildren);

                _idToItem.Add(item.ID, go);
            }

            _pageLabel.text = $"{endIndex - startIndex}/{MapLoader.IdToMapObject.Values.Count} ({startIndex}-{endIndex})";

            if (flattenedTree.Count > 0)
                _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 0, 0, 0);
            else
                _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
        }


        public void Sync()
        {
            SyncPooled();
            SyncSelectedItems();
        }

        private void RedrawPooled(MapEditorHirarchyButton element, MapObject obj, int level, int siblingID, bool expanded, bool hasChildren)
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
            var button = go.AddComponent<MapEditorHirarchyButton>();
            button.Setup(
                Width,
                () => OnButtonClick(button.BoundID),
                () => OnButtonRelease(button.BoundID),
                () => { OnElementCallback(button.BoundID, true); },
                () => { OnElementCallback(button.BoundID, false); }
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
            if (_menu.IsPopupActive())
                return;
            bool multi = SettingsManager.InputSettings.MapEditor.Multiselect.GetKey();
            if (_selected.Contains(id))
            {
                if (!multi && _gameManager.SelectedObjects.Count > 1)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
                    _gameManager.OnSelectionChange();
                }
                else if (multi)
                {
                    _gameManager.DeselectObject(MapLoader.IdToMapObject[id]);
                    _treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.DeselectObject(MapLoader.IdToMapObject[item.ID]));
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
                    _treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
                    _gameManager.OnSelectionChange();
                }
                else if (_selected.Count > 0 && !multi)
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _treeView.GetChildrenRecursive(_treeView.Items.Where(item => item.ID == id).FirstOrDefault()).ForEach(item => _gameManager.SelectObject(MapLoader.IdToMapObject[item.ID]));
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
