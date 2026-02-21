using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using SimpleJSONFixed;
using Map;

public class MapPrefabBrowser : EditorWindow
{
    private Vector2 _categoryScrollPos;
    private Vector2 _prefabScrollPos;
    private string _selectedCategory = "General";
    private string _searchQuery = "";
    private List<string> _categories = new List<string>();
    private Dictionary<string, List<PrefabInfo>> _prefabsByCategory = new Dictionary<string, List<PrefabInfo>>();
    private Dictionary<string, List<string>> _variantGroups = new Dictionary<string, List<string>>();
    private List<PrefabInfo> _filteredPrefabs = new List<PrefabInfo>();
    private bool _showHidden = false;
    private bool _showVariants = true;
    private bool _showPreviews = true;
    private bool _gridView = true;
    private GUIStyle _categoryButtonStyle;
    private GUIStyle _selectedCategoryStyle;
    private GUIStyle _prefabButtonStyle;
    private GUIStyle _variantButtonStyle;
    private bool _stylesInitialized = false;
    private float _categoryWidth = 150f;
    private bool _isResizing = false;
    private Rect _resizerRect;
    private Dictionary<string, Texture2D> _previewCache = new Dictionary<string, Texture2D>();
    private float _previewSize = 50f;
    private float _tileSize = 90f;
    private int _tilesPerRow = 5;
    private HashSet<string> _expandedVariants = new HashSet<string>();

    [MenuItem("Window/Map Prefab Browser")]
    public static void ShowWindow()
    {
        var window = GetWindow<MapPrefabBrowser>("Map Prefabs");
        window.Show();
    }

    void OnEnable()
    {
        LoadPrefabDatabase();
        FilterPrefabs();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void Update()
    {
        if (AssetPreview.IsLoadingAssetPreviews())
        {
            Repaint();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (DragAndDrop.objectReferences.Length == 0 && DragAndDrop.GetGenericData("MapPrefabInfo") != null)
        {
            Event e = Event.current;
            
            if (e.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                e.Use();
            }
            else if (e.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                
                PrefabInfo prefabInfo = (PrefabInfo)DragAndDrop.GetGenericData("MapPrefabInfo");
                if (prefabInfo != null)
                {
                    AddPrefabToSceneAtMousePosition(prefabInfo, sceneView);
                }
                e.Use();
            }
        }
    }

    void AddPrefabToSceneAtMousePosition(PrefabInfo prefabInfo, SceneView sceneView)
    {
        GameObject prefab = LoadPrefab(prefabInfo.Asset);
        
        if (prefab == null)
        {
            Debug.LogError($"Could not load prefab: {prefabInfo.Asset}");
            return;
        }

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        
        Vector3 spawnPos = ray.origin + ray.direction * 10f;
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            spawnPos = hit.point;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = spawnPos;
        
        Selection.activeGameObject = instance;
        Undo.RegisterCreatedObjectUndo(instance, "Add Map Prefab");
    }

    void OnGUI()
    {
        InitializeStyles();
        DrawToolbar();
        
        EditorGUILayout.BeginHorizontal();
        DrawCategoryPanel();
        DrawResizer();
        DrawPrefabPanel();
        EditorGUILayout.EndHorizontal();
        
        HandleResizing();
    }

    void InitializeStyles()
    {
        if (_stylesInitialized)
            return;

        _categoryButtonStyle = new GUIStyle(GUI.skin.button);
        _categoryButtonStyle.alignment = TextAnchor.MiddleLeft;
        _categoryButtonStyle.padding = new RectOffset(10, 10, 5, 5);

        _selectedCategoryStyle = new GUIStyle(_categoryButtonStyle);
        _selectedCategoryStyle.normal.background = _categoryButtonStyle.active.background;
        _selectedCategoryStyle.fontStyle = FontStyle.Bold;

        _prefabButtonStyle = new GUIStyle(GUI.skin.button);
        _prefabButtonStyle.alignment = TextAnchor.MiddleLeft;
        _prefabButtonStyle.padding = new RectOffset(5, 5, 5, 5);
        _prefabButtonStyle.wordWrap = false;

        _variantButtonStyle = new GUIStyle(_prefabButtonStyle);
        _variantButtonStyle.fontSize = 10;
        _variantButtonStyle.padding = new RectOffset(15, 5, 3, 3);

        _stylesInitialized = true;
    }

    void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(60)))
        {
            LoadPrefabDatabase();
            FilterPrefabs();
            _previewCache.Clear();
            _expandedVariants.Clear();
        }

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();
        _searchQuery = EditorGUILayout.TextField(_searchQuery, EditorStyles.toolbarSearchField);
        if (EditorGUI.EndChangeCheck())
        {
            FilterPrefabs();
        }

        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton") ?? EditorStyles.toolbarButton, GUILayout.Width(20)))
        {
            _searchQuery = "";
            FilterPrefabs();
            GUI.FocusControl(null);
        }

        GUILayout.FlexibleSpace();

        EditorGUI.BeginChangeCheck();
        _gridView = GUILayout.Toggle(_gridView, "Grid", EditorStyles.toolbarButton, GUILayout.Width(50));
        if (EditorGUI.EndChangeCheck())
        {
            Repaint();
        }

        if (_gridView)
        {
            GUILayout.Label("Size:", EditorStyles.toolbarButton, GUILayout.Width(35));
            EditorGUI.BeginChangeCheck();
            _tileSize = GUILayout.HorizontalSlider(_tileSize, 60f, 150f, GUILayout.Width(80));
            if (EditorGUI.EndChangeCheck())
            {
                Repaint();
            }
        }

        EditorGUI.BeginChangeCheck();
        _showPreviews = GUILayout.Toggle(_showPreviews, "Preview", EditorStyles.toolbarButton, GUILayout.Width(70));
        if (EditorGUI.EndChangeCheck())
        {
            Repaint();
        }

        EditorGUI.BeginChangeCheck();
        _showVariants = GUILayout.Toggle(_showVariants, "Variants", EditorStyles.toolbarButton, GUILayout.Width(70));
        if (EditorGUI.EndChangeCheck())
        {
            FilterPrefabs();
        }

        EditorGUI.BeginChangeCheck();
        _showHidden = GUILayout.Toggle(_showHidden, "Hidden", EditorStyles.toolbarButton, GUILayout.Width(60));
        if (EditorGUI.EndChangeCheck())
        {
            FilterPrefabs();
        }

        EditorGUILayout.EndHorizontal();
    }

    void DrawCategoryPanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(_categoryWidth));
        
        GUILayout.Label("Categories", EditorStyles.boldLabel);
        
        _categoryScrollPos = EditorGUILayout.BeginScrollView(_categoryScrollPos);

        if (GUILayout.Button("All", _selectedCategory == "All" ? _selectedCategoryStyle : _categoryButtonStyle))
        {
            _selectedCategory = "All";
            FilterPrefabs();
        }

        EditorGUILayout.Space(5);

        foreach (string category in _categories)
        {
            if (category == "All")
                continue;

            int count = _prefabsByCategory.ContainsKey(category) ? _prefabsByCategory[category].Count : 0;
            string label = $"{category} ({count})";

            if (GUILayout.Button(label, _selectedCategory == category ? _selectedCategoryStyle : _categoryButtonStyle))
            {
                _selectedCategory = category;
                FilterPrefabs();
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawResizer()
    {
        _resizerRect = new Rect(_categoryWidth - 2, 20, 4, position.height - 20);
        EditorGUI.DrawRect(_resizerRect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUIUtility.AddCursorRect(_resizerRect, MouseCursor.ResizeHorizontal);
    }

    void HandleResizing()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && _resizerRect.Contains(e.mousePosition))
        {
            _isResizing = true;
            e.Use();
        }
        else if (e.type == EventType.MouseUp)
        {
            _isResizing = false;
        }
        else if (e.type == EventType.MouseDrag && _isResizing)
        {
            _categoryWidth = Mathf.Clamp(e.mousePosition.x, 100f, position.width - 200f);
            Repaint();
            e.Use();
        }
    }

    void DrawPrefabPanel()
    {
        EditorGUILayout.BeginVertical();

        string title = string.IsNullOrEmpty(_searchQuery) 
            ? $"{_selectedCategory} ({_filteredPrefabs.Count})" 
            : $"Search Results ({_filteredPrefabs.Count})";
        GUILayout.Label(title, EditorStyles.boldLabel);

        _prefabScrollPos = EditorGUILayout.BeginScrollView(_prefabScrollPos);

        if (_filteredPrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No prefabs found", MessageType.Info);
        }
        else
        {
            if (_gridView && _showPreviews)
            {
                DrawPrefabGrid();
            }
            else
            {
                DrawPrefabList();
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawPrefabGrid()
    {
        float availableWidth = position.width - _categoryWidth - 40;
        _tilesPerRow = Mathf.Max(1, Mathf.FloorToInt(availableWidth / _tileSize));

        int col = 0;

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < _filteredPrefabs.Count; i++)
        {
            PrefabInfo prefabInfo = _filteredPrefabs[i];

            if (prefabInfo.IsVariant)
                continue;

            if (col == 0)
                EditorGUILayout.BeginHorizontal();

            DrawPrefabTile(prefabInfo);

            col++;
            if (col >= _tilesPerRow)
            {
                EditorGUILayout.EndHorizontal();
                col = 0;
            }

            if (_expandedVariants.Contains(prefabInfo.Name) && _showVariants && _variantGroups.ContainsKey(prefabInfo.Name))
            {
                if (col > 0)
                {
                    EditorGUILayout.EndHorizontal();
                    col = 0;
                }

                DrawVariantGrid(prefabInfo.Name);
            }
        }

        if (col > 0)
            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    void DrawVariantGrid(string basePrefabName)
    {
        if (!_variantGroups.ContainsKey(basePrefabName))
            return;

        var variantPrefabs = _filteredPrefabs.Where(p => p.IsVariant && p.BaseName == basePrefabName).ToList();

        if (variantPrefabs.Count == 0)
            return;

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label($"Variants of {basePrefabName}:", EditorStyles.miniLabel);

        int col = 0;
        EditorGUILayout.BeginHorizontal();

        foreach (PrefabInfo variant in variantPrefabs)
        {
            if (col > 0 && col % _tilesPerRow == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

            DrawPrefabTile(variant);
            col++;
        }

        if (col > 0)
            EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }

    void DrawPrefabList()
    {
        foreach (PrefabInfo prefabInfo in _filteredPrefabs)
        {
            if (prefabInfo.IsVariant)
                continue;

            DrawPrefabButton(prefabInfo);

            if (_expandedVariants.Contains(prefabInfo.Name) && _showVariants && _variantGroups.ContainsKey(prefabInfo.Name))
            {
                EditorGUI.indentLevel++;
                var variantPrefabs = _filteredPrefabs.Where(p => p.IsVariant && p.BaseName == prefabInfo.Name).ToList();
                foreach (var variant in variantPrefabs)
                {
                    DrawVariantButton(variant);
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    void DrawPrefabTile(PrefabInfo prefabInfo)
    {
        Color originalBg = GUI.backgroundColor;
        if (prefabInfo.Networked)
            GUI.backgroundColor = new Color(0.7f, 0.9f, 1f);
        else if (!prefabInfo.Visible)
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.7f);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(_tileSize), GUILayout.Height(_tileSize + 40));
        GUI.backgroundColor = originalBg;

        bool hasVariants = _variantGroups.ContainsKey(prefabInfo.Name) && _showVariants && !prefabInfo.IsVariant;
        if (hasVariants)
        {
            Rect headerRect = GUILayoutUtility.GetRect(_tileSize - 10, 18);
            GUILayout.Space(-18);

            bool isExpanded = _expandedVariants.Contains(prefabInfo.Name);
            string expandIcon = isExpanded ? "v" : ">";
            string variantLabel = $"{expandIcon} {_variantGroups[prefabInfo.Name].Count}";
            
            Rect buttonRect = new Rect(headerRect.x + headerRect.width - 40, headerRect.y + 2, 40, 16);
            GUI.backgroundColor = new Color(0.3f, 0.6f, 1f, 0.9f);
            if (GUI.Button(buttonRect, variantLabel, EditorStyles.miniButton))
            {
                if (isExpanded)
                    _expandedVariants.Remove(prefabInfo.Name);
                else
                    _expandedVariants.Add(prefabInfo.Name);
                Event.current.Use();
            }
            GUI.backgroundColor = originalBg;
        }

        Texture2D preview = GetPreviewIcon(prefabInfo);
        Rect previewRect = GUILayoutUtility.GetRect(_tileSize - 10, _tileSize - 30);
        
        if (preview != null)
        {
            GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
        }
        else
        {
            EditorGUI.DrawRect(previewRect, new Color(0.2f, 0.2f, 0.2f, 1f));
            GUIStyle noPreviewStyle = new GUIStyle(GUI.skin.label);
            noPreviewStyle.alignment = TextAnchor.MiddleCenter;
            noPreviewStyle.fontSize = 24;
            noPreviewStyle.normal.textColor = Color.gray;
            GUI.Label(previewRect, "?", noPreviewStyle);
        }

        if (Event.current.type == EventType.MouseDown && previewRect.Contains(Event.current.mousePosition))
        {
            if (Event.current.button == 0)
            {
                AddPrefabToScene(prefabInfo);
                Event.current.Use();
            }
            else if (Event.current.button == 1)
            {
                ShowPrefabContextMenu(prefabInfo);
                Event.current.Use();
            }
        }

        if (Event.current.type == EventType.MouseDrag && previewRect.Contains(Event.current.mousePosition))
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.SetGenericData("MapPrefabInfo", prefabInfo);
            DragAndDrop.StartDrag(prefabInfo.Name);
            Event.current.Use();
        }

        string displayName = prefabInfo.Name;
        int maxLen = Mathf.FloorToInt(_tileSize / 7);
        if (displayName.Length > maxLen)
            displayName = displayName.Substring(0, maxLen - 2) + "..";
        
        if (prefabInfo.IsVariant)
            displayName = "+ " + displayName;
        if (prefabInfo.Hidden)
            displayName += " [H]";

        GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 9;
        labelStyle.wordWrap = true;
        
        GUILayout.Label(displayName, labelStyle);

        EditorGUILayout.EndVertical();

        Rect tileRect = GUILayoutUtility.GetLastRect();
        if (tileRect.Contains(Event.current.mousePosition))
        {
            string tooltip = $"{prefabInfo.Name}\n{prefabInfo.Category}";
            if (prefabInfo.Components.Count > 0)
                tooltip += $"\n{string.Join(", ", prefabInfo.Components)}";
            if (!string.IsNullOrEmpty(prefabInfo.CollideMode))
                tooltip += $"\n{prefabInfo.CollideMode}";
            if (hasVariants)
                tooltip += $"\n{_variantGroups[prefabInfo.Name].Count} variants (click arrow)";
            GUI.Label(tileRect, new GUIContent("", tooltip));
        }
    }

    void DrawPrefabButton(PrefabInfo prefabInfo)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        bool hasVariants = _variantGroups.ContainsKey(prefabInfo.Name) && _showVariants && !prefabInfo.IsVariant;
        if (hasVariants)
        {
            bool expanded = _expandedVariants.Contains(prefabInfo.Name);
            string arrow = expanded ? "v" : ">";
            if (GUILayout.Button(arrow, EditorStyles.label, GUILayout.Width(15)))
            {
                if (expanded)
                    _expandedVariants.Remove(prefabInfo.Name);
                else
                    _expandedVariants.Add(prefabInfo.Name);
                Event.current.Use();
            }
        }
        else
        {
            GUILayout.Space(15);
        }

        if (_showPreviews)
        {
            Texture2D preview = GetPreviewIcon(prefabInfo);
            if (preview != null)
            {
                GUILayout.Label(preview, GUILayout.Width(_previewSize), GUILayout.Height(_previewSize));
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(_previewSize), GUILayout.Height(_previewSize));
            }
        }

        EditorGUILayout.BeginVertical();

        string displayName = prefabInfo.Name;
        if (prefabInfo.Hidden)
            displayName += " [Hidden]";
        if (hasVariants)
            displayName += $" ({_variantGroups[prefabInfo.Name].Count} variants)";

        Color originalColor = GUI.backgroundColor;
        if (prefabInfo.Networked)
            GUI.backgroundColor = new Color(0.7f, 0.9f, 1f);
        else if (!prefabInfo.Visible)
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.7f);

        if (GUILayout.Button(displayName, _prefabButtonStyle, GUILayout.Height(_showPreviews ? 32 : 24)))
        {
            if (Event.current.button == 0)
                AddPrefabToScene(prefabInfo);
            else if (Event.current.button == 1)
                ShowPrefabContextMenu(prefabInfo);
        }

        GUI.backgroundColor = originalColor;

        Rect buttonRect = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseDrag && buttonRect.Contains(Event.current.mousePosition))
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.SetGenericData("MapPrefabInfo", prefabInfo);
            DragAndDrop.StartDrag(prefabInfo.Name);
            Event.current.Use();
        }

        if (_showPreviews)
        {
            string props = $"{prefabInfo.Category}";
            if (prefabInfo.Components.Count > 0)
                props += $" | {prefabInfo.Components.Count} component(s)";
            if (!string.IsNullOrEmpty(prefabInfo.CollideMode))
                props += $" | {prefabInfo.CollideMode}";
            
            EditorGUILayout.LabelField(props, EditorStyles.miniLabel);
        }

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("?", GUILayout.Width(24), GUILayout.Height(_showPreviews ? _previewSize : 20)))
        {
            ShowPrefabInfo(prefabInfo);
        }

        EditorGUILayout.EndHorizontal();
    }

    void DrawVariantButton(PrefabInfo prefabInfo)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        if (_showPreviews)
        {
            GUILayout.Space(20);
            Texture2D preview = GetPreviewIcon(prefabInfo);
            if (preview != null)
            {
                GUILayout.Label(preview, GUILayout.Width(_previewSize * 0.8f), GUILayout.Height(_previewSize * 0.8f));
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(_previewSize * 0.8f), GUILayout.Height(_previewSize * 0.8f));
            }
        }
        else
        {
            GUILayout.Space(20);
        }

        EditorGUILayout.BeginVertical();

        string displayName = "+ " + prefabInfo.Name;
        if (GUILayout.Button(displayName, _variantButtonStyle, GUILayout.Height(_showPreviews ? 24 : 20)))
        {
            AddPrefabToScene(prefabInfo);
        }

        Rect buttonRect = GUILayoutUtility.GetLastRect();
        if (Event.current.type == EventType.MouseDrag && buttonRect.Contains(Event.current.mousePosition))
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new UnityEngine.Object[] { };
            DragAndDrop.SetGenericData("MapPrefabInfo", prefabInfo);
            DragAndDrop.StartDrag(prefabInfo.Name);
            Event.current.Use();
        }

        if (_showPreviews && !string.IsNullOrEmpty(prefabInfo.Type))
        {
            EditorGUILayout.LabelField($"Type: {prefabInfo.Type}", EditorStyles.miniLabel);
        }

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("?", GUILayout.Width(24), GUILayout.Height(_showPreviews ? 40 : 18)))
        {
            ShowPrefabInfo(prefabInfo);
        }

        EditorGUILayout.EndHorizontal();
    }

    Texture2D GetPreviewIcon(PrefabInfo prefabInfo)
    {
        if (_previewCache.ContainsKey(prefabInfo.Asset))
            return _previewCache[prefabInfo.Asset];

        Texture2D preview = null;

        string[] assetParts = prefabInfo.Asset.Split('/');
        if (assetParts.Length >= 2)
        {
            string assetName = assetParts[assetParts.Length - 1];
            preview = Resources.Load<Texture2D>("Map/Previews/" + assetName + "Preview");
            
            if (preview != null)
            {
                _previewCache[prefabInfo.Asset] = preview;
                return preview;
            }
        }

        GameObject prefab = LoadPrefab(prefabInfo.Asset);
        if (prefab != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(prefab);
            if (!string.IsNullOrEmpty(assetPath))
            {
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefabAsset != null)
                {
                    int instanceID = prefabAsset.GetInstanceID();
                    
                    preview = AssetPreview.GetAssetPreview(prefabAsset);
                    
                    if (preview == null || AssetPreview.IsLoadingAssetPreview(instanceID))
                    {
                        if (prefabAsset.GetComponentInChildren<Renderer>() != null)
                        {
                            preview = AssetPreview.GetMiniThumbnail(prefabAsset);
                        }
                        else
                        {
                            preview = AssetPreview.GetMiniTypeThumbnail(typeof(GameObject));
                        }
                    }
                    
                    _previewCache[prefabInfo.Asset] = preview;
                    return preview;
                }
            }
        }

        return null;
    }

    void ShowPrefabContextMenu(PrefabInfo prefabInfo)
    {
        GenericMenu menu = new GenericMenu();
        
        menu.AddItem(new GUIContent("Add to Scene"), false, () => AddPrefabToScene(prefabInfo));
        menu.AddItem(new GUIContent("Add Multiple..."), false, () => AddMultiplePrefabs(prefabInfo));
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Show Info"), false, () => ShowPrefabInfo(prefabInfo));
        menu.AddItem(new GUIContent("Ping Prefab Asset"), false, () => PingPrefabAsset(prefabInfo));
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Copy Prefab Name"), false, () => EditorGUIUtility.systemCopyBuffer = prefabInfo.Name);
        menu.AddItem(new GUIContent("Copy Asset Path"), false, () => EditorGUIUtility.systemCopyBuffer = prefabInfo.Asset);
        
        menu.ShowAsContext();
    }

    void AddMultiplePrefabs(PrefabInfo prefabInfo)
    {
        string input = EditorInputDialog.Show("Add Multiple", "Number of instances:", "5");
        if (string.IsNullOrEmpty(input))
            return;

        if (!int.TryParse(input, out int count))
        {
            EditorUtility.DisplayDialog("Error", "Invalid number", "OK");
            return;
        }

        count = Mathf.Clamp(count, 1, 100);

        GameObject prefab = LoadPrefab(prefabInfo.Asset);
        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", $"Could not load prefab: {prefabInfo.Asset}", "OK");
            return;
        }

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(count));
        float spacing = 5f;

        Vector3 startPos = Vector3.zero;
        if (SceneView.lastActiveSceneView != null)
        {
            Camera cam = SceneView.lastActiveSceneView.camera;
            startPos = cam.transform.position + cam.transform.forward * 10f;
        }

        List<GameObject> instances = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            int x = i % gridSize;
            int z = i / gridSize;
            Vector3 offset = new Vector3(x * spacing, 0, z * spacing);
            
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.transform.position = startPos + offset;
            instances.Add(instance);
            
            Undo.RegisterCreatedObjectUndo(instance, "Add Multiple Prefabs");
        }

        Selection.objects = instances.ToArray();
        
        if (SceneView.lastActiveSceneView != null)
            SceneView.lastActiveSceneView.FrameSelected();
    }

    void PingPrefabAsset(PrefabInfo prefabInfo)
    {
        GameObject prefab = LoadPrefab(prefabInfo.Asset);
        if (prefab != null)
        {
            EditorGUIUtility.PingObject(prefab);
            Selection.activeObject = prefab;
        }
    }

    void AddPrefabToScene(PrefabInfo prefabInfo)
    {
        GameObject prefab = LoadPrefab(prefabInfo.Asset);
        
        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", $"Could not load prefab: {prefabInfo.Asset}", "OK");
            return;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        
        if (SceneView.lastActiveSceneView != null)
        {
            Camera cam = SceneView.lastActiveSceneView.camera;
            Vector3 spawnPos = cam.transform.position + cam.transform.forward * 10f;
            instance.transform.position = spawnPos;
        }
        else
        {
            instance.transform.position = Vector3.zero;
        }

        Selection.activeGameObject = instance;
        
        if (SceneView.lastActiveSceneView != null)
            SceneView.lastActiveSceneView.FrameSelected();

        Undo.RegisterCreatedObjectUndo(instance, "Add Map Prefab");
    }

    void ShowPrefabInfo(PrefabInfo prefabInfo)
    {
        string info = $"Name: {prefabInfo.Name}\n" +
                      $"Category: {prefabInfo.Category}\n" +
                      $"Asset: {prefabInfo.Asset}\n" +
                      $"Type: {prefabInfo.Type}\n";

        if (prefabInfo.IsVariant)
            info += $"Base Prefab: {prefabInfo.BaseName}\n";
        
        if (_variantGroups.ContainsKey(prefabInfo.Name))
            info += $"Variants: {_variantGroups[prefabInfo.Name].Count}\n";

        if (prefabInfo.Static)
            info += "Static: Yes\n";
        if (!prefabInfo.Visible)
            info += "Visible: No\n";
        if (prefabInfo.Networked)
            info += "Networked: Yes\n";
        if (prefabInfo.Hidden)
            info += "Hidden: Yes\n";

        if (!string.IsNullOrEmpty(prefabInfo.CollideMode))
            info += $"Collide Mode: {prefabInfo.CollideMode}\n";
        if (!string.IsNullOrEmpty(prefabInfo.CollideWith))
            info += $"Collide With: {prefabInfo.CollideWith}\n";

        if (prefabInfo.Components.Count > 0)
        {
            info += $"Components: {string.Join(", ", prefabInfo.Components)}\n";
        }

        EditorUtility.DisplayDialog(prefabInfo.Name, info, "OK");
    }

    GameObject LoadPrefab(string asset)
    {
        if (asset == "None" || string.IsNullOrEmpty(asset))
            return null;

        string[] parts = asset.Split('/');
        if (parts.Length < 2)
            return null;

        string category = parts[0];
        string assetName = parts[parts.Length - 1];

        string resourcePath = "Map/" + category + "/Prefabs/" + assetName;
        GameObject prefab = Resources.Load<GameObject>(resourcePath);

        if (prefab == null && (category == "MoMoSync" || category.StartsWith("Momo")))
        {
            resourcePath = "Map/" + category + "/" + assetName;
            prefab = Resources.Load<GameObject>(resourcePath);
        }

        return prefab;
    }

    void LoadPrefabDatabase()
    {
        _categories.Clear();
        _prefabsByCategory.Clear();
        _variantGroups.Clear();

        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
        {
            Debug.LogError("Could not load Data/Info/MapPrefabList");
            return;
        }

        var prefabList = JSON.Parse(prefabListAsset.text);

        foreach (string categoryKey in prefabList.Keys)
        {
            JSONNode categoryNode = prefabList[categoryKey];
            JSONNode info = categoryNode["Info"];

            string displayCategory = info.HasKey("Category") ? info["Category"].Value : categoryKey;
            
            if (!_categories.Contains(displayCategory))
                _categories.Add(displayCategory);

            if (!_prefabsByCategory.ContainsKey(displayCategory))
                _prefabsByCategory[displayCategory] = new List<PrefabInfo>();

            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                PrefabInfo prefabInfo = new PrefabInfo();
                prefabInfo.Name = prefabNode["Name"].Value;
                prefabInfo.Category = displayCategory;
                prefabInfo.Type = info["Type"].Value;
                prefabInfo.Asset = BuildAssetPath(prefabNode, info);

                if (prefabNode.HasKey("Static"))
                    prefabInfo.Static = prefabNode["Static"].AsBool;
                if (prefabNode.HasKey("Visible"))
                    prefabInfo.Visible = prefabNode["Visible"].AsBool;
                else
                    prefabInfo.Visible = true;
                if (prefabNode.HasKey("Networked"))
                    prefabInfo.Networked = prefabNode["Networked"].AsBool;
                if (prefabNode.HasKey("Hidden"))
                    prefabInfo.Hidden = prefabNode["Hidden"].AsBool;

                if (prefabNode.HasKey("CollideMode"))
                    prefabInfo.CollideMode = prefabNode["CollideMode"].Value;
                if (prefabNode.HasKey("CollideWith"))
                    prefabInfo.CollideWith = prefabNode["CollideWith"].Value;

                if (prefabNode.HasKey("Components"))
                {
                    foreach (JSONNode compNode in prefabNode["Components"])
                    {
                        string compString = compNode.Value;
                        string compName = compString.Split('|')[0];
                        prefabInfo.Components.Add(compName);
                    }
                }

                if (prefabNode.HasKey("Variants"))
                {
                    List<string> variants = new List<string>();
                    foreach (JSONNode variantNode in prefabNode["Variants"])
                    {
                        variants.Add(variantNode.Value);
                    }
                    _variantGroups[prefabInfo.Name] = variants;
                }

                string baseName = GetBasePrefabName(prefabInfo.Name);
                if (baseName != prefabInfo.Name)
                {
                    prefabInfo.IsVariant = true;
                    prefabInfo.BaseName = baseName;
                }

                _prefabsByCategory[displayCategory].Add(prefabInfo);
            }
        }

        _categories.Sort();
    }

    string BuildAssetPath(JSONNode prefabNode, JSONNode info)
    {
        string asset = string.Empty;

        if (info.HasKey("AssetSameAsName") && info["AssetSameAsName"].AsBool)
            asset = prefabNode["Name"].Value;

        if (prefabNode.HasKey("Asset"))
            asset = prefabNode["Asset"].Value;

        if (asset == string.Empty || asset == "None")
            return "None";

        string basePath = "";
        if (info.HasKey("AssetBasePath"))
        {
            basePath = info["AssetBasePath"].Value;
            if (!basePath.EndsWith("/"))
                basePath += "/";
        }
        else if (info.HasKey("AssetPrefix"))
        {
            basePath = info["AssetPrefix"].Value;
        }

        return basePath + asset;
    }

    string GetBasePrefabName(string prefabName)
    {
        if (prefabName.Length > 1)
        {
            char lastChar = prefabName[prefabName.Length - 1];
            if (char.IsDigit(lastChar))
            {
                int i = prefabName.Length - 1;
                while (i >= 0 && char.IsDigit(prefabName[i]))
                    i--;
                return prefabName.Substring(0, i + 1);
            }
        }
        return prefabName;
    }

    void FilterPrefabs()
    {
        _filteredPrefabs.Clear();

        List<PrefabInfo> sourcePrefabs;
        if (_selectedCategory == "All")
        {
            sourcePrefabs = new List<PrefabInfo>();
            foreach (var list in _prefabsByCategory.Values)
                sourcePrefabs.AddRange(list);
        }
        else if (_prefabsByCategory.ContainsKey(_selectedCategory))
        {
            sourcePrefabs = _prefabsByCategory[_selectedCategory];
        }
        else
        {
            return;
        }

        foreach (PrefabInfo prefabInfo in sourcePrefabs)
        {
            if (prefabInfo.Hidden && !_showHidden)
                continue;

            if (!string.IsNullOrEmpty(_searchQuery))
            {
                string query = _searchQuery.ToLower();
                if (!prefabInfo.Name.ToLower().Contains(query) &&
                    !prefabInfo.Category.ToLower().Contains(query) &&
                    !prefabInfo.Asset.ToLower().Contains(query))
                    continue;
            }

            _filteredPrefabs.Add(prefabInfo);
        }

        _filteredPrefabs = _filteredPrefabs
            .OrderBy(p => p.BaseName ?? p.Name)
            .ThenBy(p => p.IsVariant ? 1 : 0)
            .ThenBy(p => p.Name)
            .ToList();
    }

    private class PrefabInfo
    {
        public string Name;
        public string Category;
        public string Type;
        public string Asset;
        public bool Static;
        public bool Visible = true;
        public bool Networked;
        public bool Hidden;
        public string CollideMode;
        public string CollideWith;
        public List<string> Components = new List<string>();
        public bool IsVariant;
        public string BaseName;
    }
}
