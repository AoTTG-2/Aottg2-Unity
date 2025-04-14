using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using GameManagers;
using Map;
using MapEditor;
using Unity.VisualScripting;


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
        protected int ButtomMaskPadding => 50;
        protected override bool ScrollBar => true;

        private Dictionary<int, GameObject> _idToItem = new Dictionary<int, GameObject>();
        private HashSet<int> _selected = new HashSet<int>();


        private int _lastClickedItem = -1;
        private float _lastclickedTime = 0f;
        private bool _draggingItem = false;
        private MapEditorMenu _menu;
        private ElementStyle _style;
        private MapEditorGameManager _gameManager;
        private InputSettingElement _searchInput;
        private StringSetting _searchSetting = new StringSetting(string.Empty);
        private Text _pageLabel;
        private Transform _topGroup;

        // Pooling
        private const int MaxVisibleObjects = 40;
        private ScrollRect _scrollRect;
        private GameObject _scroll;
        private Scrollbar _scrollBar;
        private GameObject _scrollView;

        private List<MapEditorHierarchyButton> _elementsPool = new();
        private Dictionary<int, MapEditorHierarchyButton> _idToElement = new();
        private List<MapObject> _visibleObjects = new();

        public int TotalElementCount => MapLoader.IdToMapObject.Count;

        // Selectors
        int _targetID = -1;
        int _targetParent = -1;
        int? _targetSibling = null;
        MapEditorHierarchyButton _lastHighlighted = null;
        bool _blockUI = false;

        // State
        public bool IsTreeView = true;


        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            _menu = (MapEditorMenu)UIManager.CurrentMenu;
            _style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);
            var style = new ElementStyle(fontSize: 18, titleWidth: 0f, themePanel: ThemePanel);

            TopBar.Find("Label").gameObject.SetActive(false);

            _topGroup = ElementFactory.CreateHorizontalGroup(TopBar, 10f, TextAnchor.MiddleLeft).transform;

            var selector = ElementFactory.CreateIconButton(_topGroup, _style, "Icons/Navigation/CheckIcon", onClick: () => SelectAllInHierarchy(), elementHeight: 32f, elementWidth: 32f);
            _searchInput = ElementFactory.CreateInputSetting(_topGroup, style, _searchSetting, "", elementWidth: 100f, elementHeight: 32f,
                onEndEdit: () => Sync()).GetComponent<InputSettingElement>();
            _pageLabel = ElementFactory.CreateDefaultLabel(_topGroup, style, "0").GetComponent<Text>();
            _topGroup.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 0, 0, 0);

            var panel = transform.Find("SinglePanelContent(Clone)");

            #region CustomScrollRectSetup
            _scrollRect = panel.GetComponent<ScrollRect>();
            _scrollView = panel.Find("ScrollView").gameObject;
            _scroll = panel.Find("Scrollbar").gameObject;
            _scrollBar = _scroll.GetComponent<Scrollbar>();
            DestroyImmediate(_scrollRect);  // potentially bad - honestly if more stuff pops up, please just remake the prefab.
            _scrollRect = panel.AddComponent<PooledScrollRect>();
            _scrollRect.content = _scrollView.GetComponent<RectTransform>();
            _scrollRect.horizontal = false;
            _scrollRect.vertical = true;
            _scrollRect.movementType = ScrollRect.MovementType.Clamped;
            _scrollRect.inertia = false;
            _scrollRect.scrollSensitivity = 11;
            _scrollRect.verticalScrollbar = _scrollBar;
            _scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
            _scrollRect.onValueChanged.AddListener(OnScrollChanged);
            _scrollBar.onValueChanged.AddListener(OnScroll);
            #endregion

            InitializePooledElements();

            Sync();
        }

        public override void Show()
        {
            base.Show();
        }

        #region TreeViewAlgorithms
        public List<MapObject> GetVisibleItems()
        {
            List<int> ids = GetVisibleIds();
            return ids.Select(id => MapLoader.IdToMapObject[id]).ToList();
        }

        public List<int> GetVisibleIds()
        {
            List<int> items = new List<int>();
            GetOrderedChildren(-1, items);

            // For now so that we can check other maps,
            if (items.Count == 0) GetOrderedChildren(0, items); // TODO: Remove this when map migration is added.

            return items;
        }

        private void GetOrderedChildren(int parent, List<int> items, int level = 0)
        {
            if (MapLoader.IdToChildren.ContainsKey(parent) == false)
                return;
            // Note: When expanding the parent, we're seeing that this code runs twice and the second time around,
            // the parent we're expanding (which should still have a parent of -1, is no longer in this mapping）
            IEnumerable<int> orderedChildren = MapLoader.IdToChildren[parent].OrderBy(id => MapLoader.IdToMapObject[id].SiblingIndex);
            foreach (int child in orderedChildren)
            {
                MapLoader.IdToMapObject[child].Level = level;
                items.Add(child);
                if (MapLoader.IdToMapObject[child].Expanded)
                    GetOrderedChildren(child, items, level + 1);
            }
        }

        public void RemoveFromParent(int id)
        {
            MapObject obj = MapLoader.IdToMapObject[id];
            if (obj == null)
                return;

            int parent = obj.Parent;
            int siblingIndex = obj.SiblingIndex;

            MapLoader.IdToMapObject[id].Parent = -1;
            IEnumerable<int> orderedChildren = MapLoader.IdToChildren[parent].OrderBy(id => MapLoader.IdToMapObject[id].SiblingIndex);

        }

        #endregion

        private void Update()
        {
            HandleElementDrag();
            if (_requestRedraw)
            {
                _requestRedraw = false;
                Canvas.ForceUpdateCanvases();
            }
            _blockUI = false;
        }

        public void OnScroll(float x) { }

        public void OnScrollChanged(Vector2 vec)
        {
            UpdateVisibleElements();
        }

        private bool _requestRedraw = true;
        public void UpdateVisibleElements()
        {
            float scrollPos = _scrollRect.verticalNormalizedPosition;
            int totalElements = _visibleObjects.Count;
            float size = Mathf.Min(1f, (float)MaxVisibleObjects / totalElements);
            _scrollRect.verticalScrollbar.size = size;

            int startIndex = 0;
            if (totalElements > MaxVisibleObjects)
            {
                int maxStartIndex = totalElements - MaxVisibleObjects;
                startIndex = scrollPos > 0f ? Mathf.Clamp(Mathf.FloorToInt((1f - scrollPos) * maxStartIndex), 0, maxStartIndex) : maxStartIndex;
            }

            bool _requestRedraw = false;
            for (int i = 0; i < MaxVisibleObjects; i++)
            {
                var element = _elementsPool[i];
                int elementIndex = startIndex + i;
                if (elementIndex < totalElements)
                {
                    bool wasActive = element.gameObject.activeSelf;
                    var mapObject = _visibleObjects[elementIndex];
                    int siblingIndex = element.transform.GetSiblingIndex();
                    bool isCorrectlyBound = element.IsCorrectlyBound(mapObject, _selected.Contains(mapObject.ScriptObject.Id));
                    _requestRedraw = !wasActive || isCorrectlyBound || siblingIndex != i;
                    if (!wasActive)
                    {
                        element.gameObject.SetActive(true);
                    }

                    if (!isCorrectlyBound)
                    {
                        element.Bind(mapObject, _selected.Contains(mapObject.ScriptObject.Id));
                    }

                    if (siblingIndex != i)
                    {
                        element.transform.SetSiblingIndex(i);
                    }
                }
                else if (element.gameObject.activeSelf)
                {
                    element.gameObject.SetActive(false);
                    _requestRedraw = true;
                }
            }
            _pageLabel.text = $"{startIndex} - {startIndex + MaxVisibleObjects} ({MapLoader.IdToMapObject.Values.Count})";
        }

        private void HandleElementDrag()
        {
            if (_blockUI)
                return;
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

                    if (_targetID != _targetParent && MapLoader.IdToMapObject[_targetParent].Parent != _targetID)
                    {
                        _gameManager.NewCommand(new SetParentCommand(new List<MapObject>() { MapLoader.IdToMapObject[_targetID] }, _targetParent, _targetSibling));
                    }
                }
            }
        }

        private MapEditorHierarchyButton FindButtonMouseOver()
        {
            Vector2 mouse = Input.mousePosition;
            foreach (var item in _elementsPool)
            {
                if (item.gameObject.activeSelf)
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
                var go = CreatePooledElement();
                go.SetActive(false);
                _elementsPool.Add(go.GetComponent<MapEditorHierarchyButton>());
            }
        }

        /// <summary>
        /// We will be removing the concept of pagination and instead rendering a fixed number of objects from the virtual tree view.
        /// We need to figure out the item offset based on the scroll position and then draw the next X items.
        /// </summary>
        public void UpdateDataSource()
        {
            // Clear and repopulate the view
            // foreach (var btn in _elementsPool) btn.gameObject.SetActive(false);
            string searchTerm = _searchSetting.Value.ToLower();

            _idToItem.Clear();
            _selected.Clear();
            _visibleObjects.Clear();

            IsTreeView = searchTerm == string.Empty;
            if (!IsTreeView) _visibleObjects = MapLoader.Query(searchTerm).Select(id => MapLoader.IdToMapObject[id]).ToList();
            else _visibleObjects = GetVisibleItems();
        }

        public void Sync()
        {
            UpdateDataSource();
            UpdateVisibleElements();
            SyncSelectedItems();
        }

        private GameObject CreatePooledElement()
        {
            var go = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/MapEditorHierarchyButton");
            var button = go.AddComponent<MapEditorHierarchyButton>();
            button.Setup(
                Width - ButtomMaskPadding,
                () => OnButtonClick(button.BoundID),
                () => OnElementCallback(button.BoundID, true),
                () => OnElementCallback(button.BoundID, false)
            );
            button.Bind(string.Empty, -1, -1, false);
            return go;
        }

        private void OnElementCallback(int id, bool expanded)
        {
            _blockUI = true;
            if (expanded) OnElementExpand(id);
            else OnElementClose(id);
            Sync();
        }

        private void OnElementExpand(int id)
        {
            MapLoader.IdToMapObject[id].Expanded = true;
        }

        private void OnElementClose(int id)
        {
            MapLoader.IdToMapObject[id].Expanded = false;
        }

        private void OnButtonClick(int id)
        {
            if (_menu.IsPopupActive()) return;
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
                    if (_idToItem.ContainsKey(selected)) _idToItem[selected].transform.Find("Highlight").gameObject.SetActive(false);
                }
            }
            foreach (MapObject obj in _gameManager.SelectedObjects)
            {
                if (!_selected.Contains(obj.ScriptObject.Id))
                {
                    _selected.Add(obj.ScriptObject.Id);
                    if (_idToItem.ContainsKey(obj.ScriptObject.Id)) _idToItem[obj.ScriptObject.Id].transform.Find("Highlight").gameObject.SetActive(true);
                }
            }
        }

        public void SelectAllInHierarchy()
        {
            _gameManager.DeselectAll();
            if (_gameManager.SelectedObjects.Count == _visibleObjects.Count) return;
            foreach (var element in _visibleObjects) _gameManager.SelectObject(MapLoader.IdToMapObject[element.ScriptObject.Id]);
            _gameManager.OnSelectionChange();
        }
    }
}
