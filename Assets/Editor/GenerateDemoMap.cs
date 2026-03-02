using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using SimpleJSONFixed;
using Map;
using MapEditor;
using UnityEngine.SceneManagement;

public class GenerateDemoMap : EditorWindow
{
    private float _spacing = 6f;
    private int _columnsPerCategory = 10;
    private float _floorPadding = 10f;
    private float _categoryGap = 12f;
    private bool _skipHidden = true;
    private bool _skipVariants = false;
    private bool _skipTerrain = true;
    private bool _addLabels = true;
    private bool _addDaylight = true;
    private float _maxPrefabSize = 10f;
    private Vector2 _scrollPos;
    private string _statusMessage = "";

    [MenuItem("Tools/Generate Demo Map")]
    static void ShowWindow()
    {
        var window = GetWindow<GenerateDemoMap>("Generate Demo Map");
        window.minSize = new Vector2(350, 340);
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Generate Demo Map", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox(
            "Spawns every prefab from the MapPrefabList database onto a large " +
            "floor, organized by category. All objects receive a MapObjectPrefabMarker " +
            "so the scene can be exported to MapScript.",
            MessageType.Info);

        EditorGUILayout.Space();

        _spacing = EditorGUILayout.FloatField("Spacing Between Prefabs", _spacing);
        _columnsPerCategory = EditorGUILayout.IntField("Columns Per Category", _columnsPerCategory);
        _floorPadding = EditorGUILayout.FloatField("Floor Padding", _floorPadding);
        _categoryGap = EditorGUILayout.FloatField("Gap Between Categories", _categoryGap);
        _skipHidden = EditorGUILayout.Toggle("Skip Hidden Prefabs", _skipHidden);
        _skipVariants = EditorGUILayout.Toggle("Skip Variants", _skipVariants);
        _skipTerrain = EditorGUILayout.Toggle("Skip Terrain Prefabs", _skipTerrain);
        _addLabels = EditorGUILayout.Toggle("Add Category Labels", _addLabels);
        _addDaylight = EditorGUILayout.Toggle("Add Daylight", _addDaylight);
        _maxPrefabSize = EditorGUILayout.FloatField("Max Prefab Size", _maxPrefabSize);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Demo Map", GUILayout.Height(30)))
        {
            Generate();
        }

        if (!string.IsNullOrEmpty(_statusMessage))
        {
            EditorGUILayout.Space();
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(120));
            EditorGUILayout.HelpBox(_statusMessage, MessageType.None);
            EditorGUILayout.EndScrollView();
        }
    }

    void Generate()
    {
        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not load Data/Info/MapPrefabList", "OK");
            return;
        }

        var prefabList = JSON.Parse(prefabListAsset.text);

        // Collect all prefabs grouped by display category
        var categorizedPrefabs = new Dictionary<string, List<PrefabEntry>>();
        var categoryOrder = new List<string>();

        foreach (string categoryKey in prefabList.Keys)
        {
            JSONNode categoryNode = prefabList[categoryKey];
            JSONNode info = categoryNode["Info"];
            string displayCategory = info.HasKey("Category") ? info["Category"].Value : categoryKey;

            if (!categorizedPrefabs.ContainsKey(displayCategory))
            {
                categorizedPrefabs[displayCategory] = new List<PrefabEntry>();
                categoryOrder.Add(displayCategory);
            }

            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                bool hidden = prefabNode.HasKey("Hidden") && prefabNode["Hidden"].AsBool;
                if (_skipHidden && hidden)
                    continue;

                string prefabName = prefabNode["Name"].Value;

                bool isVariant = false;
                if (prefabName.Length > 1 && char.IsDigit(prefabName[prefabName.Length - 1]))
                    isVariant = true;

                if (_skipVariants && isVariant)
                    continue;

                string asset = BuildAssetPath(prefabNode, info);

                var entry = new PrefabEntry
                {
                    Name = prefabName,
                    Asset = asset,
                    Category = displayCategory,
                    PrefabNode = prefabNode,
                    CategoryInfo = info,
                    Hidden = hidden
                };

                categorizedPrefabs[displayCategory].Add(entry);
            }
        }

        categoryOrder.Sort();

        // Calculate total footprint so we can size the floor
        int totalPrefabs = 0;
        float totalDepth = 0f;
        float maxRowWidth = 0f;

        foreach (string cat in categoryOrder)
        {
            var entries = categorizedPrefabs[cat];
            if (entries.Count == 0)
                continue;

            int rows = Mathf.CeilToInt((float)entries.Count / _columnsPerCategory);
            float catDepth = rows * _spacing + _categoryGap;
            float catWidth = Mathf.Min(entries.Count, _columnsPerCategory) * _spacing;

            totalDepth += catDepth;
            if (catWidth > maxRowWidth)
                maxRowWidth = catWidth;
            totalPrefabs += entries.Count;
        }

        if (totalPrefabs == 0)
        {
            EditorUtility.DisplayDialog("Error", "No prefabs found in database.", "OK");
            return;
        }

        // Create root container
        GameObject root = new GameObject("DemoMap");
        Undo.RegisterCreatedObjectUndo(root, "Generate Demo Map");

        // Create ground floor using Geometry/Cuboid
        float floorWidth = maxRowWidth + _floorPadding * 2f;
        float floorDepth = totalDepth + _floorPadding * 2f;

        GameObject floorPrefab = LoadPrefab("Geometry/Cuboid");
        GameObject floor;
        if (floorPrefab != null)
        {
            floor = (GameObject)PrefabUtility.InstantiatePrefab(floorPrefab);
        }
        else
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        floor.name = "Floor";
        floor.isStatic = true;
        Vector3 baseScale = floor.transform.localScale;
        floor.transform.localScale = new Vector3(
            floorWidth * baseScale.x,
            1f * baseScale.y,
            floorDepth * baseScale.z);
        floor.transform.position = new Vector3(
            maxRowWidth * 0.5f - _spacing * 0.5f,
            -10f,
            -totalDepth * 0.5f + _floorPadding);
        floor.transform.SetParent(root.transform, true);

        var floorMarker = floor.AddComponent<MapObjectPrefabMarker>();
        floorMarker.PrefabName = "Cuboid";
        floorMarker.OverrideStatic = true;
        floorMarker.Static = true;
        floorMarker.OverridePhysics = true;
        floorMarker.CollideMode = "Physical";
        floorMarker.CollideWith = "Entities";
        floorMarker.PhysicsMaterial = "Default";
        floorMarker.OverrideMaterial = true;
        floorMarker.MaterialShader = "Basic";
        floorMarker.MaterialColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        floorMarker.MaterialTexture = "Misc/None";
        floorMarker.MaterialTiling = new Vector2(
            Mathf.Round(floorWidth / 4f),
            Mathf.Round(floorDepth / 4f));

        // Add daylight
        if (_addDaylight)
        {
            GameObject daylightPrefab = LoadPrefab("General/EditorDaylight");
            GameObject daylight;
            if (daylightPrefab != null)
            {
                daylight = (GameObject)PrefabUtility.InstantiatePrefab(daylightPrefab);
            }
            else
            {
                daylight = new GameObject();
                var light = daylight.AddComponent<Light>();
                light.type = LightType.Directional;
                light.shadows = LightShadows.Soft;
            }

            daylight.name = "Daylight";
            daylight.transform.position = new Vector3(0f, 20f, 0f);
            daylight.transform.rotation = Quaternion.Euler(50f, 130f, 0f);
            daylight.transform.SetParent(root.transform, true);

            var dlMarker = daylight.GetComponent<MapObjectPrefabMarker>();
            if (dlMarker == null)
                dlMarker = daylight.AddComponent<MapObjectPrefabMarker>();
            dlMarker.PrefabName = "Daylight";
            dlMarker.OverrideVisible = true;
            dlMarker.Visible = false;
            dlMarker.CustomComponents = new List<MapObjectPrefabMarker.ComponentData>
            {
                new MapObjectPrefabMarker.ComponentData
                {
                    ComponentName = "Daylight",
                    Parameters = new List<string>
                    {
                        "Color:255/255/255/255",
                        "Intensity:1",
                        "WeatherControlled:true"
                    }
                }
            };
        }

        // Place prefabs category by category
        float currentZ = 0f;
        int placed = 0;
        int failed = 0;
        var failedNames = new List<string>();

        foreach (string cat in categoryOrder)
        {
            var entries = categorizedPrefabs[cat];
            if (entries.Count == 0)
                continue;

            if (_addLabels)
            {
                GameObject label = new GameObject($"--- {cat} ({entries.Count}) ---");
                label.transform.position = new Vector3(-_spacing, 2f, -currentZ);
                label.transform.SetParent(root.transform, true);

                var labelMarker = label.AddComponent<MapObjectPrefabMarker>();
                labelMarker.PrefabName = "Empty Object";
                labelMarker.OverrideVisible = true;
                labelMarker.Visible = false;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                PrefabEntry entry = entries[i];
                int col = i % _columnsPerCategory;
                int row = i / _columnsPerCategory;

                float x = col * _spacing;
                float z = -(currentZ + row * _spacing);

                GameObject prefab = LoadPrefab(entry.Asset);

                if (_skipTerrain && prefab != null && prefab.GetComponentInChildren<Terrain>(true) != null)
                    continue;

                GameObject instance;

                if (prefab != null)
                {
                    instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    instance = new GameObject();
                    failed++;
                    failedNames.Add(entry.Name);
                }

                instance.name = entry.Name;
                Vector3 centerOffset = NormalizePrefabSize(instance);
                instance.transform.position = new Vector3(x, 0f, z) - centerOffset;
                instance.transform.SetParent(root.transform, true);

                AddMarkerToInstance(instance, entry);
                placed++;
            }

            int rows = Mathf.CeilToInt((float)entries.Count / _columnsPerCategory);
            currentZ += rows * _spacing + _categoryGap;
        }

        Selection.activeGameObject = root;
        if (SceneView.lastActiveSceneView != null)
            SceneView.lastActiveSceneView.FrameSelected();

        _statusMessage = $"Placed {placed} prefabs across {categoryOrder.Count(c => categorizedPrefabs[c].Count > 0)} categories.";
        if (failed > 0)
        {
            _statusMessage += $"\n{failed} prefab(s) could not be loaded (empty objects created):\n";
            _statusMessage += string.Join(", ", failedNames.Take(30));
            if (failedNames.Count > 30)
                _statusMessage += $"... and {failedNames.Count - 30} more";
        }

        EditorUtility.DisplayDialog("Demo Map Generated",
            $"Placed {placed} prefabs.\n" +
            (failed > 0 ? $"{failed} could not load (see status)." : "All prefabs loaded successfully.") +
            "\n\nUse Tools > Export Scene to MapScript to export.",
            "OK");
    }

    Vector3 NormalizePrefabSize(GameObject instance)
    {
        Bounds? combinedBounds = null;

        var renderers = instance.GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            if (combinedBounds == null)
                combinedBounds = renderer.bounds;
            else
            {
                var b = combinedBounds.Value;
                b.Encapsulate(renderer.bounds);
                combinedBounds = b;
            }
        }

        var terrains = instance.GetComponentsInChildren<Terrain>(true);
        foreach (var terrain in terrains)
        {
            if (terrain.terrainData == null)
                continue;

            Vector3 terrainPos = terrain.transform.position;
            Vector3 terrainSize = terrain.terrainData.size;
            Bounds terrainBounds = new Bounds(
                terrainPos + terrainSize * 0.5f,
                terrainSize);

            if (combinedBounds == null)
                combinedBounds = terrainBounds;
            else
            {
                var b = combinedBounds.Value;
                b.Encapsulate(terrainBounds);
                combinedBounds = b;
            }
        }

        if (combinedBounds == null)
            return Vector3.zero;

        Bounds bounds = combinedBounds.Value;
        Vector3 centerOffset = bounds.center - instance.transform.position;

        float largestAxis = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        if (largestAxis <= 0f || largestAxis <= _maxPrefabSize)
            return centerOffset;

        float scaleFactor = _maxPrefabSize / largestAxis;
        instance.transform.localScale *= scaleFactor;
        centerOffset *= scaleFactor;

        return centerOffset;
    }

    void AddMarkerToInstance(GameObject instance, PrefabEntry entry)
    {
        var marker = instance.GetComponent<MapObjectPrefabMarker>();
        if (marker == null)
            marker = instance.AddComponent<MapObjectPrefabMarker>();

        marker.PrefabName = entry.Name;
        marker.CustomAsset = "";
        marker.OverrideActive = false;
        marker.Active = true;

        JSONNode prefabNode = entry.PrefabNode;

        if (prefabNode == null)
            return;

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

        if (prefabNode.HasKey("CollideMode") || prefabNode.HasKey("CollideWith") || prefabNode.HasKey("PhysicsMaterial"))
        {
            marker.OverridePhysics = true;
            marker.CollideMode = prefabNode.HasKey("CollideMode") ? prefabNode["CollideMode"].Value : "Physical";
            marker.CollideWith = prefabNode.HasKey("CollideWith") ? prefabNode["CollideWith"].Value : "Entities";
            marker.PhysicsMaterial = prefabNode.HasKey("PhysicsMaterial") ? prefabNode["PhysicsMaterial"].Value : "Default";
        }

        if (prefabNode.HasKey("Material"))
        {
            marker.OverrideMaterial = true;
            ParseMaterialString(marker, prefabNode["Material"].Value);
        }

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

        marker.MaterialShader = parts[0];

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

        if (parts.Length > 2)
            marker.MaterialTexture = parts[2];

        if (parts.Length > 3)
        {
            string[] tilingParts = parts[3].Split('/');
            if (tilingParts.Length >= 2)
                marker.MaterialTiling = new Vector2(float.Parse(tilingParts[0]), float.Parse(tilingParts[1]));
        }

        if (parts.Length > 4)
        {
            string[] offsetParts = parts[4].Split('/');
            if (offsetParts.Length >= 2)
                marker.MaterialOffset = new Vector2(float.Parse(offsetParts[0]), float.Parse(offsetParts[1]));
        }

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

    private class PrefabEntry
    {
        public string Name;
        public string Asset;
        public string Category;
        public bool Hidden;
        public JSONNode PrefabNode;
        public JSONNode CategoryInfo;
    }
}
