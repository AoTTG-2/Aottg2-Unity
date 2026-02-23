using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Map;
using Utility;
using SimpleJSONFixed;
using MapEditor;

/// <summary>
/// Tool to synchronize MapObjectPrefabMarker components on Map prefabs with the MapPrefabList.json definitions.
/// This ensures prefabs have the correct default properties from the JSON database.
/// </summary>
public class MapPrefabMarkerSync : EditorWindow
{
    private Vector2 _scrollPos;
    private List<string> _syncLog = new List<string>();
    private bool _includeHiddenPrefabs = false;
    private bool _overwriteExisting = true;
    private string _filterCategory = "(All)";
    private List<string> _categories = new List<string>();

    [MenuItem("Tools/MapScript/Sync Markers on Map Prefabs")]
    static void ShowWindow()
    {
        var window = GetWindow<MapPrefabMarkerSync>("Sync Map Prefab Markers");
        window.Show();
    }

    void OnEnable()
    {
        LoadCategories();
    }

    void OnGUI()
    {
        GUILayout.Label("Sync Map Prefab Markers", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox(
            "This tool adds/updates MapObjectPrefabMarker components on all Map prefabs " +
            "to match their definitions in MapPrefabList.json. This ensures prefabs have " +
            "correct default properties for export.", 
            MessageType.Info);
        
        EditorGUILayout.Space();

        _overwriteExisting = EditorGUILayout.Toggle("Overwrite Existing Markers", _overwriteExisting);
        _includeHiddenPrefabs = EditorGUILayout.Toggle("Include Hidden Prefabs", _includeHiddenPrefabs);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filter Category:", GUILayout.Width(120));
        int selectedIndex = Mathf.Max(0, _categories.IndexOf(_filterCategory));
        selectedIndex = EditorGUILayout.Popup(selectedIndex, _categories.ToArray());
        _filterCategory = _categories[selectedIndex];
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Apply Markers to All Prefabs", GUILayout.Height(35)))
        {
            ApplyMarkersToAllPrefabs();
        }
        
        if (GUILayout.Button("Remove All Markers", GUILayout.Height(35)))
        {
            if (EditorUtility.DisplayDialog("Confirm Remove", 
                "This will remove MapObjectPrefabMarker from all Map prefabs. Continue?", 
                "Yes", "Cancel"))
            {
                RemoveAllMarkers();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Log"))
        {
            _syncLog.Clear();
            Repaint();
        }

        EditorGUILayout.Space();

        if (_syncLog.Count > 0)
        {
            GUILayout.Label($"Sync Log ({_syncLog.Count} entries):", EditorStyles.boldLabel);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(400));
            foreach (var log in _syncLog)
            {
                EditorGUILayout.LabelField(log, EditorStyles.wordWrappedLabel);
            }
            EditorGUILayout.EndScrollView();
        }
    }

    void LoadCategories()
    {
        _categories.Clear();
        _categories.Add("(All)");

        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
            return;

        var prefabList = JSON.Parse(prefabListAsset.text);
        foreach (string categoryKey in prefabList.Keys)
        {
            _categories.Add(categoryKey);
        }
    }

    void ApplyMarkersToAllPrefabs()
    {
        _syncLog.Clear();
        
        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not load Data/Info/MapPrefabList", "OK");
            return;
        }

        var prefabList = JSON.Parse(prefabListAsset.text);
        int processed = 0;
        int updated = 0;
        int skipped = 0;
        int failed = 0;

        // Collect all prefab work items first for progress tracking
        var workItems = new List<(string categoryKey, JSONNode prefabNode, JSONNode info)>();
        foreach (string categoryKey in prefabList.Keys)
        {
            if (_filterCategory != "(All)" && _filterCategory != categoryKey)
                continue;

            JSONNode categoryNode = prefabList[categoryKey];
            JSONNode info = categoryNode["Info"];

            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                workItems.Add((categoryKey, prefabNode, info));
            }
        }

        int totalItems = workItems.Count;
        var prefabsToSave = new List<GameObject>();

        // Batch asset editing for better performance
        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < workItems.Count; i++)
            {
                var (categoryKey, prefabNode, info) = workItems[i];
                processed++;

                EditorUtility.DisplayProgressBar("Applying Markers",
                    $"Processing {processed}/{totalItems}",
                    (float)processed / totalItems);

                // Skip hidden prefabs unless requested
                if (!_includeHiddenPrefabs && prefabNode.HasKey("Hidden") && prefabNode["Hidden"].AsBool)
                {
                    skipped++;
                    continue;
                }

                string prefabName = prefabNode["Name"].Value;

                // Build asset path
                string asset = BuildAssetPath(prefabNode, info);

                if (asset == "None" || string.IsNullOrEmpty(asset))
                {
                    _syncLog.Add($"? Skipped: {prefabName} (no asset)");
                    skipped++;
                    continue;
                }

                // Find the prefab in Resources
                GameObject prefab = LoadPrefabFromAsset(asset);

                if (prefab == null)
                {
                    _syncLog.Add($"? Failed: {prefabName} - Could not load asset '{asset}'");
                    failed++;
                    continue;
                }

                // Check if already has marker
                var marker = prefab.GetComponent<MapObjectPrefabMarker>();
                if (marker != null && !_overwriteExisting)
                {
                    _syncLog.Add($"? Skipped: {prefabName} (already has marker)");
                    skipped++;
                    continue;
                }

                // Add or update marker
                if (marker == null)
                {
                    marker = prefab.AddComponent<MapObjectPrefabMarker>();
                }

                // Populate marker from JSON
                PopulateMarkerFromJson(marker, prefabNode, info, prefabName, asset);

                // Queue for batch save
                prefabsToSave.Add(prefab);
                updated++;
                _syncLog.Add($"? Updated: {prefabName} ? {asset}");
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
        }

        // Batch save all modified prefabs
        EditorUtility.DisplayProgressBar("Saving Prefabs", "Saving modified prefabs...", 0.5f);
        foreach (var prefab in prefabsToSave)
        {
            PrefabUtility.SavePrefabAsset(prefab);
        }
        EditorUtility.ClearProgressBar();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        _syncLog.Add($"\n=== Sync Complete ===");
        _syncLog.Add($"Processed: {processed}");
        _syncLog.Add($"Updated: {updated}");
        _syncLog.Add($"Skipped: {skipped}");
        _syncLog.Add($"Failed: {failed}");

        EditorUtility.DisplayDialog("Sync Complete", 
            $"Processed {processed} prefabs\n" +
            $"Updated: {updated}\n" +
            $"Skipped: {skipped}\n" +
            $"Failed: {failed}", 
            "OK");

        Repaint();
    }

    void RemoveAllMarkers()
    {
        _syncLog.Clear();
        
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/Map" });
        int removed = 0;
        int total = prefabGuids.Length;
        var prefabsToSave = new List<GameObject>();

        // Batch asset editing for better performance
        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < prefabGuids.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Removing Markers",
                    $"Processing {i + 1}/{total}",
                    (float)(i + 1) / total);

                string path = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                if (prefab != null)
                {
                    var marker = prefab.GetComponent<MapObjectPrefabMarker>();
                    if (marker != null)
                    {
                        DestroyImmediate(marker, true);
                        prefabsToSave.Add(prefab);
                        removed++;
                        _syncLog.Add($"Removed marker from: {prefab.name}");
                    }
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
        }

        // Batch save all modified prefabs
        EditorUtility.DisplayProgressBar("Saving Prefabs", "Saving modified prefabs...", 0.5f);
        foreach (var prefab in prefabsToSave)
        {
            PrefabUtility.SavePrefabAsset(prefab);
        }
        EditorUtility.ClearProgressBar();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        _syncLog.Add($"\n=== Remove Complete ===");
        _syncLog.Add($"Total Prefabs: {total}");
        _syncLog.Add($"Markers Removed: {removed}");

        EditorUtility.DisplayDialog("Complete", $"Removed markers from {removed} prefabs", "OK");
        Repaint();
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

    GameObject LoadPrefabFromAsset(string asset)
    {
        string[] parts = asset.Split('/');
        if (parts.Length < 2)
            return null;

        string category = parts[0];
        string assetName = parts[parts.Length - 1];

        // Try standard path first
        string resourcePath = "Map/" + category + "/Prefabs/" + assetName;
        GameObject prefab = Resources.Load<GameObject>(resourcePath);

        // Try MoMoSync special path
        if (prefab == null && (category == "MoMoSync" || category.StartsWith("Momo")))
        {
            resourcePath = "Map/" + category + "/" + assetName;
            prefab = Resources.Load<GameObject>(resourcePath);
        }

        return prefab;
    }

    void PopulateMarkerFromJson(MapObjectPrefabMarker marker, JSONNode prefabNode, JSONNode info, string prefabName, string asset)
    {
        // Set identity
        marker.PrefabName = prefabName;
        marker.CustomAsset = ""; // Clear custom asset - use default

        // Set object properties
        marker.OverrideActive = false;
        marker.Active = true;

        if (prefabNode.HasKey("Static"))
        {
            marker.OverrideStatic = true;
            marker.Static = prefabNode["Static"].AsBool;
        }

        if (prefabNode.HasKey("Visible"))
        {
            marker.OverrideVisible = true;
            marker.Visible = prefabNode["Visible"].AsBool;
        }

        if (prefabNode.HasKey("Networked"))
        {
            marker.OverrideNetworked = true;
            marker.Networked = prefabNode["Networked"].AsBool;
        }

        // Set physics properties
        if (prefabNode.HasKey("CollideMode") || prefabNode.HasKey("CollideWith") || prefabNode.HasKey("PhysicsMaterial"))
        {
            marker.OverridePhysics = true;
            
            if (prefabNode.HasKey("CollideMode"))
                marker.CollideMode = prefabNode["CollideMode"].Value;
            else
                marker.CollideMode = "Physical";

            if (prefabNode.HasKey("CollideWith"))
                marker.CollideWith = prefabNode["CollideWith"].Value;
            else
                marker.CollideWith = "Entities";

            if (prefabNode.HasKey("PhysicsMaterial"))
                marker.PhysicsMaterial = prefabNode["PhysicsMaterial"].Value;
            else
                marker.PhysicsMaterial = "Default";
        }

        // Set material properties
        if (prefabNode.HasKey("Material"))
        {
            marker.OverrideMaterial = true;
            string materialString = prefabNode["Material"].Value;
            ParseMaterialString(marker, materialString);
        }

        // Set components
        marker.CustomComponents.Clear();
        if (prefabNode.HasKey("Components"))
        {
            foreach (JSONNode componentNode in prefabNode["Components"])
            {
                var compData = new MapObjectPrefabMarker.ComponentData();
                
                string componentString = componentNode.Value;
                string[] parts = componentString.Split('|');
                
                if (parts.Length > 0)
                {
                    compData.ComponentName = parts[0];
                    compData.Parameters = new List<string>();
                    
                    for (int i = 1; i < parts.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(parts[i]))
                            compData.Parameters.Add(parts[i]);
                    }
                    
                    marker.CustomComponents.Add(compData);
                }
            }
        }
    }

    void ParseMaterialString(MapObjectPrefabMarker marker, string materialString)
    {
        string[] parts = materialString.Split('|');
        
        if (parts.Length == 0)
            return;

        // Shader
        marker.MaterialShader = parts[0];

        // Color (R/G/B/A)
        if (parts.Length > 1)
        {
            string[] colorParts = parts[1].Split('/');
            if (colorParts.Length >= 4)
            {
                int r = int.Parse(colorParts[0]);
                int g = int.Parse(colorParts[1]);
                int b = int.Parse(colorParts[2]);
                int a = int.Parse(colorParts[3]);
                marker.MaterialColor = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
        }

        // Texture
        if (parts.Length > 2)
        {
            marker.MaterialTexture = parts[2];
        }

        // Tiling (X/Y)
        if (parts.Length > 3)
        {
            string[] tilingParts = parts[3].Split('/');
            if (tilingParts.Length >= 2)
            {
                marker.MaterialTiling = new Vector2(
                    float.Parse(tilingParts[0]),
                    float.Parse(tilingParts[1])
                );
            }
        }

        // Offset (X/Y)
        if (parts.Length > 4)
        {
            string[] offsetParts = parts[4].Split('/');
            if (offsetParts.Length >= 2)
            {
                marker.MaterialOffset = new Vector2(
                    float.Parse(offsetParts[0]),
                    float.Parse(offsetParts[1])
                );
            }
        }

        // ReflectColor (for reflective materials)
        if (parts.Length > 5 && marker.MaterialShader == "Reflective")
        {
            string[] reflectParts = parts[5].Split('/');
            if (reflectParts.Length >= 4)
            {
                int r = int.Parse(reflectParts[0]);
                int g = int.Parse(reflectParts[1]);
                int b = int.Parse(reflectParts[2]);
                int a = int.Parse(reflectParts[3]);
                marker.ReflectColor = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
        }
    }
}
