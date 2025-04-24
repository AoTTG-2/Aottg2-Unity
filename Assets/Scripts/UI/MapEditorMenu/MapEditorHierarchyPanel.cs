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
using UnityEngine.EventSystems;
using Photon.Realtime;


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
        public int TotalElementCount => MapLoader.IdToMapObject.Count;
        public int TotalVisibleCount => _visibleObjects.Count;
        public readonly int MAX_DEPTH = 100;

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
        private bool _requestRedraw = true;

        // Selectors
        int _targetID = -1;
        int _targetParent = -1;
        int? _targetSibling = null;
        MapEditorHierarchyButton _lastHighlighted = null;
        bool _blockUI = false;
        Vector2 _lastMousePosition = Vector2.zero;
        bool _pollingDrag = false;

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
        public List<MapObject> GetVisibleMapObjects()
        {
            List<int> ids = GetVisibleIds();
            return ids.Select(id => MapLoader.IdToMapObject[id]).ToList();
        }

        public List<int> GetVisibleIds()
        {
            List<int> results = new List<int>();
            if (MapLoader.IdToChildren.Count == 0) return results;
            if (MapLoader.IdToChildren.ContainsKey(MapLoader.ROOT))
            {
                GetOrderedChildren(-1, results);
                return results;
            }

            GetOrderedChildren(0, results);   // Handle outdated maps for now
            return results;
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

        //public List<int> GetVisibleIds()
        //{
        //    if (MapLoader.IdToChildren.Count == 0) return new List<int>();
        //    if (MapLoader.IdToChildren.ContainsKey(MapLoader.ROOT)) return GetOrderedChildren(-1);

        //    return GetOrderedChildren(0);   // Handle outdated maps for now
        //}

        // TODO: Debug why the iterative version is not working when expanding the parent. - should also validate the iterative one in gamemanager/maploader (transform setup).
        // This is needed for release as its more efficient.
        /*private List<int> GetOrderedChildren(int root)
        {
            List<int> results = new List<int>();
            Stack<(int parent, int level)> stack = new Stack<(int, int)>();
            stack.Push((root, 0));

            while (stack.Count > 0)
            {
                var (currentParent, level) = stack.Pop();

                if (!MapLoader.IdToChildren.ContainsKey(currentParent))
                    continue;

                IEnumerable<int> orderedChildren = MapLoader.IdToChildren[currentParent]
                    .OrderBy(id => MapLoader.IdToMapObject[id].SiblingIndex);

                foreach (int child in orderedChildren)
                {
                    MapLoader.IdToMapObject[child].Level = level;
                    results.Add(child);

                    if (MapLoader.IdToMapObject[child].Expanded && level + 1 < MAX_DEPTH)
                        stack.Push((child, level + 1));
                }
            }

            return results;
        }*/

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
                        if (_idToItem.ContainsKey(mapObject.ScriptObject.Id))
                            _idToItem.Remove(mapObject.ScriptObject.Id);
                        _idToItem.Add(mapObject.ScriptObject.Id, element.gameObject);
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
            // TODO: Simplify logic into state machine.
            // Mouse Down -> Poll Drag Threshold -> Poll | StartDrag  | DiscardDrag -> Drag -> Drag | SetParent | DiscardDrag
            if (_blockUI)
                return;
            if (_lastHighlighted != null)
                _lastHighlighted.ClearContextHighlight();

            // On mouse down begin checking for drag threshold
            if (Input.GetMouseButtonDown(0))
            {
                _lastMousePosition = Input.mousePosition;
                _pollingDrag = true;
            }
            if (_pollingDrag && Input.GetMouseButton(0))
            {
                if (Vector2.Distance(_lastMousePosition, Input.mousePosition) > EventSystem.current.pixelDragThreshold)
                {
                    _pollingDrag = false;
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
            }
            if (_pollingDrag && Input.GetMouseButtonUp(0))
            {
                _pollingDrag = false;
                _draggingItem = false;
                _targetID = -1;
                _targetParent = -1;
                _targetSibling = null;
            }
            if (Input.GetMouseButton(0) && _draggingItem)  // Highlight Drag Target
            {
                _lastHighlighted = FindButtonMouseOver();
                if (_lastHighlighted != null)
                {
                    _lastHighlighted.ContextHighlight();
                }
            }
            else if (Input.GetMouseButtonUp(0) && _draggingItem)    // Reposition Elements
            {
                _lastHighlighted = FindButtonMouseOver();
                if (_lastHighlighted != null)
                {
                    _lastHighlighted.SetHighlight(false);
                    _draggingItem = false;

                    Vector2 percentCovered = _lastHighlighted.GetPercentCovered();

                    if (percentCovered.y >= _lastHighlighted.TopBorder)   // Top
                    {
                        // Target parent is the same as the highlighted parent, the sibling id is the same as it will push right
                        _targetParent = MapLoader.IdToMapObject[_lastHighlighted.BoundID].Parent;
                        _targetSibling = MapLoader.IdToMapObject[_lastHighlighted.BoundID].SiblingIndex;
                    }
                    else if (percentCovered.y <= _lastHighlighted.BottomBorder)  // Bottom
                    {
                        // Target parent is the same as the highlighted parent, the sibling id is past the highlighted element
                        _targetParent = MapLoader.IdToMapObject[_lastHighlighted.BoundID].Parent;
                        _targetSibling = MapLoader.IdToMapObject[_lastHighlighted.BoundID].SiblingIndex + 1;
                    }
                    else
                    {
                        _targetParent = _lastHighlighted.BoundID;
                        _targetSibling = 0;
                    }

                    // Merge selected and target in union
                    List<MapObject> targets = new List<MapObject>();
                    if (!_selected.Contains(_targetID))
                    {
                        _selected.Add(_targetID);
                    }
                    foreach (int id in _selected)
                    {
                        MapObject obj = MapLoader.IdToMapObject[id];
                        if (obj == null)
                            continue;
                        // Check if the target parent is the same as the current parent
                        if (obj.Parent != _targetParent)
                        {
                            if (_targetParent == -1 || MapLoader.IdToMapObject[_targetParent].Parent != obj.ScriptObject.Id)
                            {
                                targets.Add(obj);
                            }
                        }
                    }
                    _selected.Remove(_targetID);
                    if (targets.Count > 0) _gameManager.NewCommand(new SetParentCommand(targets, _targetParent, _targetSibling));
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
            else _visibleObjects = GetVisibleMapObjects();
        }

        public void Sync()
        {
            SyncSelectedItems();
            UpdateDataSource();
            UpdateVisibleElements();
            SyncSelectedItems();
            // UpdateHighlight(); -> Might just handle this in UpdateVisibleElements.
            //SyncSelectedItems();
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
            bool multiSelect = SettingsManager.InputSettings.MapEditor.Multiselect.GetKey();
            bool shiftSelect = SettingsManager.InputSettings.MapEditor.ShiftSelect.GetKey();
            bool singleSelect = !multiSelect && !shiftSelect;

            if (singleSelect)
            {
                if (_selected.Contains(id) && _selected.Count == 1)
                {
                    var transform = SceneLoader.CurrentCamera.Cache.Transform;
                    transform.position = MapLoader.IdToMapObject[id].GameObject.transform.position - transform.forward * 50f;
                }
                else
                {
                    _gameManager.DeselectAll();
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange(false);
                }
            }
            else if (multiSelect)
            {
                if (_selected.Contains(id))
                {
                    _gameManager.DeselectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange(false);
                }
                else
                {
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange(false);
                }
            }
            else if (shiftSelect)
            {
                if (_selected.Count == 0)
                {
                    // If nothing is selected, just select the current item
                    _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
                    _gameManager.OnSelectionChange(false);
                    return;
                }

                // Get the index of the last selected item (assume last selected was most recent)
                int lastSelectedIndex = -1;
                List<int> visibleItems = _visibleObjects.Select(_visibleObjects => _visibleObjects.ScriptObject.Id).ToList();
                foreach (var selectedId in _selected)
                {
                    int index = visibleItems.IndexOf(selectedId);
                    if (index != -1 && (lastSelectedIndex == -1 || index > lastSelectedIndex))
                    {
                        lastSelectedIndex = index;
                    }
                }

                // Get the index of the currently clicked item
                int currentIndex = visibleItems.IndexOf(id);

                if (lastSelectedIndex == -1 || currentIndex == -1) return;

                // Determine range to select
                int start = Mathf.Min(lastSelectedIndex, currentIndex);
                int end = Mathf.Max(lastSelectedIndex, currentIndex);

                // Select all items in the range
                for (int i = start; i <= end; i++)
                {
                    if (!_selected.Contains(visibleItems[i]))
                    {
                        _gameManager.SelectObject(MapLoader.IdToMapObject[visibleItems[i]]);
                    }
                }

                _gameManager.OnSelectionChange(false);
            }


            //    if (_selected.Contains(id))
            //{
            //    if (!multi && _gameManager.SelectedObjects.Count > 1)
            //    {
            //        _gameManager.DeselectAll();
            //        _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
            //        _gameManager.OnSelectionChange(false);
            //    }
            //    else if (multi)
            //    {
            //        _gameManager.DeselectObject(MapLoader.IdToMapObject[id]);
            //        _gameManager.OnSelectionChange(false);
            //    }
            //    else
            //    {
            //        var transform = SceneLoader.CurrentCamera.Cache.Transform;
            //        transform.position = MapLoader.IdToMapObject[id].GameObject.transform.position - transform.forward * 50f;
            //    }
            //}
            //else
            //{
            //    if (_selected.Count == 0 || multi)
            //    {
            //        _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
            //        _gameManager.OnSelectionChange(false);
            //    }
            //    else if (_selected.Count > 0 && !multi)
            //    {
            //        _gameManager.DeselectAll();
            //        _gameManager.SelectObject(MapLoader.IdToMapObject[id]);
            //        _gameManager.OnSelectionChange(false);
            //    }
            //}
            _lastClickedItem = id;
            _lastclickedTime = Time.time;
        }
        
        public void SyncSelectedItems()
        {
            // TODO: Rework this to auto-scroll to the intended selected element instead of just the one in the pooled frame.
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
 
        /// <summary>Jump to the element selected, may need to be deferred to a double click due to performance.</summary>
        public void SyncSelectedItemsAndJumpToFirst()
        {
            SyncSelectedItems();
            if (_gameManager.SelectedObjects.Count == 0) return;

            MapObject first = _gameManager.SelectedObjects.First();

            // Calculate what it would take to access this element, need to know what elements to expand, and what scroll level to set.
            int iters = 0;
            int id = first.ScriptObject.Id;
            decimal elapsedElements = 0;

            // TODO: Cache Subtree elapsed element count for performance if possible, not that performance is a big deal here.

            while (iters < MAX_DEPTH && id != MapLoader.ROOT && MapLoader.IdToMapObject.ContainsKey(id))
            {
                MapObject current = MapLoader.IdToMapObject[id];
                int parent = current.Parent;
                var children = MapLoader.IdToChildren[parent];
                elapsedElements += children.Sum(x => MapLoader.IdToMapObject[x].SiblingIndex <= current.SiblingIndex? 1 : 0);

                // Expand parent if possible
                if (MapLoader.IdToMapObject.ContainsKey(parent) && !MapLoader.IdToMapObject[parent].Expanded)
                {
                    MapLoader.IdToMapObject[parent].Expanded = true;
                }

                id = current.Parent;
            }

            int maxStartIndex = TotalVisibleCount - MaxVisibleObjects;
            int centerIndex = Mathf.Clamp((int)elapsedElements - (MaxVisibleObjects / 2), 0, maxStartIndex);
            float targetScrollPos = 1f - ((float)centerIndex / maxStartIndex);

            // Snap to end if the element is at the end of the list
            if (elapsedElements <= TotalVisibleCount && elapsedElements >= TotalVisibleCount - MaxVisibleObjects / 2)
            {
                targetScrollPos = 0f;
            }

            targetScrollPos = Mathf.Clamp(targetScrollPos, 0f, 1f);

            // TODO: only scroll and sync if element is not already in view.
            _scrollRect.verticalNormalizedPosition = targetScrollPos;
            Sync();
        }

        public void SelectAllInHierarchy()
        {
            _gameManager.DeselectAll();
            if (_gameManager.SelectedObjects.Count == _visibleObjects.Count) return;
            foreach (var element in _visibleObjects) _gameManager.SelectObject(MapLoader.IdToMapObject[element.ScriptObject.Id]);
            _gameManager.OnSelectionChange(false);
        }
    }
}
