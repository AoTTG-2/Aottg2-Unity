using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SimpleJSONFixed;
using MapEditor;

public class TexturePickerWindow : EditorWindow
{
    private struct TextureEntry
    {
        public string Name;
        public string Category;
        public string Path;
        public Texture2D Preview;
    }

    private static Dictionary<string, List<TextureEntry>> _categories;
    private static bool _loaded;

    private MapObjectPrefabMarker _marker;
    private System.Action _onPicked;
    private Vector2 _scrollPos;
    private string _searchFilter = "";
    private string _selectedCategory = "All";
    private List<string> _categoryNames = new List<string>();
    private int _gridSize = 72;

    public static void Show(MapObjectPrefabMarker marker, System.Action onPicked)
    {
        var window = GetWindow<TexturePickerWindow>(true, "Select Texture", true);
        window._marker = marker;
        window._onPicked = onPicked;
        window.minSize = new Vector2(450, 400);
        window.LoadTextures();
        window.Show();
    }

    void LoadTextures()
    {
        if (_loaded && _categories != null)
        {
            RebuildCategoryNames();
            return;
        }

        _categories = new Dictionary<string, List<TextureEntry>>();

        var textureListAsset = Resources.Load<TextAsset>("Data/Info/MapTextureList");
        if (textureListAsset == null)
        {
            Debug.LogWarning("Could not load Data/Info/MapTextureList");
            return;
        }

        var textureList = JSON.Parse(textureListAsset.text);
        foreach (string category in textureList.Keys)
        {
            var entries = new List<TextureEntry>();
            foreach (JSONNode textureNode in textureList[category])
            {
                var entry = new TextureEntry();
                entry.Name = textureNode["Name"].Value;
                entry.Category = category;
                entry.Path = category + "/" + entry.Name;

                if (category != "Legacy")
                {
                    entry.Preview = Resources.Load<Texture2D>("Map/Textures/" + category + "/" + entry.Name + "Texture");
                }

                entries.Add(entry);
            }
            _categories[category] = entries;
        }

        _loaded = true;
        RebuildCategoryNames();
    }

    void RebuildCategoryNames()
    {
        _categoryNames.Clear();
        _categoryNames.Add("All");
        foreach (var key in _categories.Keys)
            _categoryNames.Add(key);
    }

    void OnGUI()
    {
        if (_marker == null)
        {
            EditorGUILayout.HelpBox("No marker selected.", MessageType.Warning);
            return;
        }

        // Toolbar
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        // Category filter
        int catIndex = _categoryNames.IndexOf(_selectedCategory);
        if (catIndex < 0) catIndex = 0;
        EditorGUI.BeginChangeCheck();
        catIndex = EditorGUILayout.Popup(catIndex, _categoryNames.ToArray(), EditorStyles.toolbarPopup, GUILayout.Width(100));
        if (EditorGUI.EndChangeCheck())
        {
            _selectedCategory = _categoryNames[catIndex];
            _scrollPos = Vector2.zero;
        }

        // Search
        _searchFilter = EditorGUILayout.TextField(_searchFilter, EditorStyles.toolbarSearchField);
        if (GUILayout.Button("", GUI.skin.FindStyle("SearchCancelButton") ?? EditorStyles.toolbarButton, GUILayout.Width(18)))
        {
            _searchFilter = "";
            GUI.FocusControl(null);
        }

        // Grid size slider
        GUILayout.Label("Size:", GUILayout.Width(32));
        _gridSize = (int)GUILayout.HorizontalSlider(_gridSize, 48, 128, GUILayout.Width(80));

        EditorGUILayout.EndHorizontal();

        // Current selection
        EditorGUILayout.LabelField("Current: " + _marker.MaterialTexture, EditorStyles.miniLabel);

        // None button
        if (GUILayout.Button("None (Clear Texture)", GUILayout.Height(22)))
        {
            SelectTexture("Misc/None");
        }

        EditorGUILayout.Space(2);

        // Grid
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        float windowWidth = position.width - 20;
        int columns = Mathf.Max(1, (int)(windowWidth / (_gridSize + 8)));

        string filterLower = _searchFilter.ToLower();

        foreach (var kvp in _categories)
        {
            if (_selectedCategory != "All" && _selectedCategory != kvp.Key)
                continue;

            // Collect filtered entries
            var filtered = new List<TextureEntry>();
            foreach (var entry in kvp.Value)
            {
                if (!string.IsNullOrEmpty(_searchFilter) && !entry.Name.ToLower().Contains(filterLower))
                    continue;
                filtered.Add(entry);
            }

            if (filtered.Count == 0)
                continue;

            // Category header
            GUILayout.Label(kvp.Key, EditorStyles.boldLabel);

            int col = 0;
            EditorGUILayout.BeginHorizontal();

            foreach (var entry in filtered)
            {
                if (col >= columns)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    col = 0;
                }

                bool isSelected = _marker.MaterialTexture == entry.Path;

                EditorGUILayout.BeginVertical(GUILayout.Width(_gridSize + 4));

                // Draw button with texture preview
                var style = isSelected ? "selectionRect" : "button";
                var btnRect = GUILayoutUtility.GetRect(_gridSize, _gridSize, GUILayout.Width(_gridSize));

                if (entry.Preview != null)
                {
                    if (GUI.Button(btnRect, "", style))
                    {
                        SelectTexture(entry.Path);
                    }
                    GUI.DrawTexture(new Rect(btnRect.x + 2, btnRect.y + 2, btnRect.width - 4, btnRect.height - 4), entry.Preview, ScaleMode.ScaleToFit);

                    // Selection highlight
                    if (isSelected)
                    {
                        EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.y, btnRect.width, 2), new Color(0.2f, 0.6f, 1f));
                        EditorGUI.DrawRect(new Rect(btnRect.x, btnRect.yMax - 2, btnRect.width, 2), new Color(0.2f, 0.6f, 1f));
                    }
                }
                else
                {
                    if (GUI.Button(btnRect, entry.Name, style))
                    {
                        SelectTexture(entry.Path);
                    }
                }

                // Label
                GUILayout.Label(entry.Name, EditorStyles.centeredGreyMiniLabel, GUILayout.Width(_gridSize));

                EditorGUILayout.EndVertical();
                col++;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(6);
        }

        EditorGUILayout.EndScrollView();
    }

    void SelectTexture(string texturePath)
    {
        Undo.RecordObject(_marker, "Change Texture");
        _marker.MaterialTexture = texturePath;
        EditorUtility.SetDirty(_marker);
        _onPicked?.Invoke();
        Close();
    }
}
