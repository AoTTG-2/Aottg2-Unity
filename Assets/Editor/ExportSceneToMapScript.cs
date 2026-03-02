using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Map;
using Utility;
using SimpleJSONFixed;
using ApplicationManagers;
using UnityEngine.SceneManagement;
using MapEditor;

public class ExportSceneToMapScript : EditorWindow
{
    private string _exportFileName = "ExportedMap";
    private bool _includeInactive = false;
    private bool _autoAssignIds = true;
    private bool _preserveIds = false;
    private Vector2 _scrollPos;
    private List<string> _warnings = new List<string>();
    private string _warningsText = "";
    private Dictionary<string, MapScriptBaseObject> _prefabDatabase = new Dictionary<string, MapScriptBaseObject>();
    private Dictionary<GameObject, string> _prefabNameCache = new Dictionary<GameObject, string>();
    private Dictionary<string, string> _assetToPrefabName = new Dictionary<string, string>();
    private Dictionary<string, Vector3> _baseScaleCache = new Dictionary<string, Vector3>();

    [MenuItem("Tools/Export Scene to MapScript")]
    static void ShowWindow()
    {
        var window = GetWindow<ExportSceneToMapScript>("Export Scene to MapScript");
        window.Show();
    }

    [MenuItem("Tools/MapScript/Add Prefab Marker to Selected")]
    static void AddMarkerToSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            if (obj.GetComponent<MapObjectPrefabMarker>() == null)
            {
                obj.AddComponent<MapObjectPrefabMarker>();
                EditorUtility.SetDirty(obj);
            }
        }
    }

    [MenuItem("Tools/MapScript/Remove Prefab Marker from Selected")]
    static void RemoveMarkerFromSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            var marker = obj.GetComponent<MapObjectPrefabMarker>();
            if (marker != null)
            {
                DestroyImmediate(marker);
                EditorUtility.SetDirty(obj);
            }
        }
    }

    [MenuItem("Tools/MapScript/Add Markers to All Scene Objects")]
    static void AddMarkersToAllSceneObjects()
    {
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        int added = 0;
        
        foreach (var rootObj in rootObjects)
        {
            // Skip system objects
            if (rootObj.name.StartsWith("NetworkManager") || 
                rootObj.name.StartsWith("EventSystem") ||
                rootObj.name.StartsWith("Main Camera") ||
                rootObj.name == "Batched Meshes")
                continue;

            AddMarkersRecursive(rootObj, ref added);
        }

        EditorUtility.DisplayDialog("Complete", $"Added markers to {added} objects", "OK");
    }

    static void AddMarkersRecursive(GameObject obj, ref int count)
    {
        if (obj.GetComponent<MapObjectPrefabMarker>() == null)
        {
            obj.AddComponent<MapObjectPrefabMarker>();
            EditorUtility.SetDirty(obj);
            count++;
        }

        foreach (Transform child in obj.transform)
        {
            // Skip children that are internal parts of a prefab
            if (IsInternalPrefabChild(child.gameObject))
                continue;
            AddMarkersRecursive(child.gameObject, ref count);
        }
    }

    /// <summary>
    /// Returns true if the given object is an internal part of a prefab instance
    /// (i.e. not the root of its prefab). These should never be exported as separate
    /// MapScript objects since the prefab itself already contains them.
    /// </summary>
    static bool IsInternalPrefabChild(GameObject obj)
    {
        // If the object is not part of a prefab at all, it's a scene object (not internal)
        if (!PrefabUtility.IsPartOfPrefabInstance(obj))
            return false;

        // Get the outermost prefab instance root for this object
        GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);

        // If this object IS the prefab root, it's not an internal child
        if (prefabRoot == obj)
            return false;

        // This object is inside a prefab instance but is not its root — skip it
        return true;
    }

    void OnEnable()
    {
        LoadPrefabDatabase();
    }

    void OnGUI()
    {
        GUILayout.Label("Export Scene to MapScript", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        _exportFileName = EditorGUILayout.TextField("Export File Name", _exportFileName);
        _includeInactive = EditorGUILayout.Toggle("Include Inactive Objects", _includeInactive);
        _autoAssignIds = EditorGUILayout.Toggle("Auto-Assign IDs", _autoAssignIds);
        _preserveIds = EditorGUILayout.Toggle("Preserve Existing IDs", _preserveIds);

        EditorGUILayout.Space();

        if (GUILayout.Button("Export Scene", GUILayout.Height(30)))
        {
            ExportScene();
        }

        if (GUILayout.Button("Reload Prefab Database", GUILayout.Height(25)))
        {
            LoadPrefabDatabase();
        }

        EditorGUILayout.Space();

        if (_warnings.Count > 0)
        {
            GUILayout.Label($"Warnings ({_warnings.Count}):", EditorStyles.boldLabel);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(300));
            GUI.enabled = false;
            EditorGUILayout.TextArea(_warningsText, EditorStyles.wordWrappedLabel);
            GUI.enabled = true;
            EditorGUILayout.EndScrollView();
        }
    }

    void LoadPrefabDatabase()
    {
        _prefabDatabase.Clear();
        _assetToPrefabName.Clear();


        // Load the MapPrefabList JSON
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

            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                MapScriptSceneObject sceneObject = new MapScriptSceneObject();
                sceneObject.Type = info["Type"].Value;
                sceneObject.Name = prefabNode["Name"].Value;

                string asset = string.Empty;
                if (info.HasKey("AssetSameAsName") && info["AssetSameAsName"].AsBool)
                    asset = prefabNode["Name"].Value;
                if (prefabNode.HasKey("Asset"))
                    asset = prefabNode["Asset"].Value;
                if (asset == string.Empty || asset == "None")
                    asset = "None";
                else
                {
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
                    asset = basePath + asset;
                }
                sceneObject.Asset = asset;

                if (prefabNode.HasKey("Static"))
                    sceneObject.Static = prefabNode["Static"].AsBool;
                if (prefabNode.HasKey("Networked"))
                    sceneObject.Networked = prefabNode["Networked"].AsBool;
                if (prefabNode.HasKey("Visible"))
                    sceneObject.Visible = prefabNode["Visible"].AsBool;
                if (prefabNode.HasKey("CollideMode"))
                    sceneObject.CollideMode = prefabNode["CollideMode"].Value;
                if (prefabNode.HasKey("CollideWith"))
                    sceneObject.CollideWith = prefabNode["CollideWith"].Value;
                if (prefabNode.HasKey("Material"))
                    sceneObject.Material = MapScriptSceneObject.DeserializeMaterial(prefabNode["Material"].Value);
                if (prefabNode.HasKey("Components"))
                {
                    foreach (JSONNode componentNode in prefabNode["Components"])
                    {
                        MapScriptComponent component = new MapScriptComponent();
                        component.Deserialize(componentNode.Value);
                        sceneObject.Components.Add(component);
                    }
                }

                _prefabDatabase[sceneObject.Name] = sceneObject;
                
                // Create asset path to prefab name mapping for easier lookup
                if (sceneObject.Asset != "None")
                {
                    string assetName = Path.GetFileNameWithoutExtension(sceneObject.Asset);
                    if (!_assetToPrefabName.ContainsKey(assetName))
                        _assetToPrefabName[assetName] = sceneObject.Name;
                }
            }
        }

        Debug.Log($"Loaded {_prefabDatabase.Count} prefabs from database");
    }

    void ExportScene()
    {
        _warnings.Clear();
        _prefabNameCache.Clear();
        _baseScaleCache.Clear();

        // Create MapScript
        MapScript mapScript = new MapScript();
        
        // Get all root GameObjects in the scene
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        
        // Dictionary to track GameObject to ID mapping
        Dictionary<GameObject, int> goToId = new Dictionary<GameObject, int>();
        int currentId = 0;

        // First pass: assign IDs and collect all objects
        List<GameObject> allObjects = new List<GameObject>();
        foreach (var rootObj in rootObjects)
        {
            // Skip certain system objects
            if (rootObj.name.StartsWith("NetworkManager") || 
                rootObj.name.StartsWith("EventSystem") ||
                rootObj.name.StartsWith("Main Camera") ||
                rootObj.name == "Batched Meshes")
                continue;
                
            CollectObjects(rootObj, allObjects);
        }

        // Build ID mappings
        if (_preserveIds)
        {
            // First pass: collect objects with existing IDs
            foreach (var obj in allObjects)
            {
                var marker = obj.GetComponent<MapObjectPrefabMarker>();
                if (marker != null && marker.ObjectId > 0)
                {
                    goToId[obj] = marker.ObjectId;
                    currentId = Mathf.Max(currentId, marker.ObjectId + 1);
                }
            }
            
            // Second pass: assign new IDs to objects without them
            foreach (var obj in allObjects)
            {
                if (!goToId.ContainsKey(obj))
                {
                    goToId[obj] = currentId++;
                }
            }
        }
        else
        {
            foreach (var obj in allObjects)
            {
                goToId[obj] = currentId++;
            }
        }

        // Second pass: create MapScriptSceneObjects
        int exportedCount = 0;
        foreach (var obj in allObjects)
        {
            // Skip objects that shouldn't be exported
            if (!_includeInactive && !obj.activeInHierarchy)
                continue;

            // Try to match to a prefab
            string prefabName = GetPrefabName(obj);
            
            if (prefabName == null)
            {
                // Create a generic empty object
                prefabName = "Empty Object";
            }

            MapScriptSceneObject scriptObject = CreateScriptObject(obj, prefabName, goToId);
            
            if (scriptObject != null)
            {
                mapScript.Objects.Objects.Add(scriptObject);
                exportedCount++;
            }
        }

        // Serialize and save
        string mapScriptContent = mapScript.Serialize();
        string defaultPath = Path.Combine(Application.dataPath, "..", "ExportedMaps");
        if (!Directory.Exists(defaultPath))
            Directory.CreateDirectory(defaultPath);
            
        string path = EditorUtility.SaveFilePanel("Save MapScript", defaultPath, _exportFileName + ".txt", "txt");
        
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, mapScriptContent);
            Debug.Log($"Exported {exportedCount} objects to {path}");
            EditorUtility.DisplayDialog("Export Complete", 
                $"Successfully exported {exportedCount} objects.\n\n" +
                (_warnings.Count > 0 ? $"{_warnings.Count} warnings generated. Check console for details." : "No warnings."),
                "OK");
            
            if (_warnings.Count > 0)
            {
                Debug.LogWarning($"Export completed with {_warnings.Count} warnings:");
                foreach (var warning in _warnings)
                {
                    Debug.LogWarning(warning);
                }
            }
        }

        _warningsText = string.Join("\n", _warnings);
        Repaint();
    }

    void CollectObjects(GameObject obj, List<GameObject> collection)
    {
        collection.Add(obj);
        
        foreach (Transform child in obj.transform)
        {
            // Skip children that are internal parts of a prefab instance.
            // The prefab's internal hierarchy is baked into the asset;
            // only the root transform matters in MapScript.
            if (IsInternalPrefabChild(child.gameObject))
                continue;

            CollectObjects(child.gameObject, collection);
        }
    }

    string GetPrefabName(GameObject obj)
    {
        if (_prefabNameCache.ContainsKey(obj))
            return _prefabNameCache[obj];

#if UNITY_EDITOR
        // Check for MapObjectPrefabMarker component (custom component for marking prefabs)
        var marker = obj.GetComponent<MapObjectPrefabMarker>();
        if (marker != null && !string.IsNullOrEmpty(marker.PrefabName))
        {
            if (_prefabDatabase.ContainsKey(marker.PrefabName))
            {
                _prefabNameCache[obj] = marker.PrefabName;
                return marker.PrefabName;
            }
        }
#endif

        // Try to get prefab name from PrefabUtility
        var prefabRoot = PrefabUtility.GetNearestPrefabInstanceRoot(obj);
        if (prefabRoot != null)
        {
            var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
            if (prefabAsset != null)
            {
                string assetPath = AssetDatabase.GetAssetPath(prefabAsset);
                string prefabAssetName = Path.GetFileNameWithoutExtension(assetPath);
                
                // Try matching by asset name in our database
                if (_assetToPrefabName.ContainsKey(prefabAssetName))
                {
                    string matchedName = _assetToPrefabName[prefabAssetName];
                    _prefabNameCache[obj] = matchedName;
                    return matchedName;
                }

                // Try direct name match
                if (_prefabDatabase.ContainsKey(prefabAssetName))
                {
                    _prefabNameCache[obj] = prefabAssetName;
                    return prefabAssetName;
                }

                // Try matching by path
                foreach (var kvp in _prefabDatabase)
                {
                    if (kvp.Value.Asset != "None" && kvp.Value.Asset.EndsWith(prefabAssetName))
                    {
                        _prefabNameCache[obj] = kvp.Key;
                        return kvp.Key;
                    }
                }
            }
        }

        // Try matching by GameObject name
        string cleanName = obj.name.Replace("(Clone)", "").Trim();
        if (_prefabDatabase.ContainsKey(cleanName))
        {
            _prefabNameCache[obj] = cleanName;
            return cleanName;
        }

        // Check for special cases based on components
        if (obj.GetComponent<Light>() != null)
        {
            Light light = obj.GetComponent<Light>();
            if (light.type == LightType.Directional)
            {
                _prefabNameCache[obj] = "Daylight";
                return "Daylight";
            }
            else if (light.type == LightType.Point)
            {
                _prefabNameCache[obj] = "PointLight";
                return "PointLight";
            }
        }

        // Check if this looks like an Empty Object
        bool hasRenderer = obj.GetComponent<Renderer>() != null || obj.GetComponentInChildren<Renderer>() != null;
        bool hasSpecialComponent = obj.GetComponent<Light>() != null || obj.GetComponent<ParticleSystem>() != null;
        
        if (!hasRenderer && !hasSpecialComponent)
        {
            _prefabNameCache[obj] = "Empty Object";
            return "Empty Object";
        }

        _warnings.Add($"Could not match GameObject '{obj.name}' to any prefab in database. Using 'Empty Object'.");
        _prefabNameCache[obj] = null;
        return null;
    }

    MapScriptSceneObject CreateScriptObject(GameObject obj, string prefabName, Dictionary<GameObject, int> goToId)
    {
        MapScriptSceneObject scriptObject = new MapScriptSceneObject();

        // Copy base prefab properties if it exists in the database.
        // These are the canonical defaults from MapPrefabList.json and should
        // only be overwritten by explicit marker overrides, never by extracting
        // from the Unity renderer (which doesn't map cleanly to MapScript materials).
        bool hasDbEntry = _prefabDatabase.ContainsKey(prefabName);
        if (hasDbEntry)
        {
            scriptObject.Copy(_prefabDatabase[prefabName]);
        }
        else
        {
            scriptObject.Asset = "None";
            scriptObject.Name = "Empty Object";
        }

#if UNITY_EDITOR
        // Check for existing MapObjectPrefabMarker to use as source of truth
        var marker = obj.GetComponent<MapObjectPrefabMarker>();
#else
        MapObjectPrefabMarker marker = null;
#endif
        
        // Set ID
        if (_preserveIds && marker != null && marker.ObjectId > 0)
        {
            scriptObject.Id = marker.ObjectId;
        }
        else
        {
            scriptObject.Id = goToId[obj];
        }

        // Set parent — walk up the hierarchy to find the nearest exported ancestor.
        // This correctly handles the case where a prefab instance is parented under
        // another prefab instance (the immediate Unity parent may be an internal child
        // of that prefab, which isn't in goToId).
        scriptObject.Parent = 0;
        Transform parentTransform = obj.transform.parent;
        while (parentTransform != null)
        {
            if (goToId.ContainsKey(parentTransform.gameObject))
            {
                scriptObject.Parent = goToId[parentTransform.gameObject];
                break;
            }
            parentTransform = parentTransform.parent;
        }

        // Override name with actual GameObject name (sanitized)
        string sanitizedName = SanitizeName(obj.name);
        if (sanitizedName != prefabName)
        {
            scriptObject.Name = sanitizedName;
        }

        // Apply marker overrides if present
        if (marker != null)
        {
            if (!string.IsNullOrEmpty(marker.CustomAsset))
                scriptObject.Asset = marker.CustomAsset;

            if (marker.OverrideActive)
                scriptObject.Active = marker.Active;
            else
                scriptObject.Active = obj.activeSelf;

            if (marker.OverrideStatic)
                scriptObject.Static = marker.Static;
            else
                scriptObject.Static = obj.isStatic;

            if (marker.OverrideVisible)
                scriptObject.Visible = marker.Visible;

            if (marker.OverrideNetworked)
                scriptObject.Networked = marker.Networked;

            if (marker.OverridePhysics)
            {
                scriptObject.CollideMode = marker.CollideMode;
                scriptObject.CollideWith = marker.CollideWith;
                scriptObject.PhysicsMaterial = marker.PhysicsMaterial;
            }
            else if (!hasDbEntry)
            {
                // No database entry and no marker override — try to infer from scene
                ExtractPhysicsInfo(obj, scriptObject);
            }
            // else: keep the database defaults already set by Copy()

            if (marker.OverrideMaterial)
            {
                ApplyMarkerMaterial(marker, scriptObject);
            }
            // else: keep the database defaults already set by Copy()

            // Always use marker components if present
            ExtractComponents(obj, scriptObject, marker);
        }
        else
        {
            // No marker — use database defaults if available, otherwise extract from scene
            scriptObject.Active = obj.activeSelf;
            scriptObject.Static = obj.isStatic;
            if (!hasDbEntry)
            {
                ExtractPhysicsInfo(obj, scriptObject);
            }
            ExtractComponents(obj, scriptObject, null);
        }

        // Set transform.
        // MapScript always stores world-space position and rotation, regardless of
        // parenting. The runtime sets transform.position (world) first, then parents
        // afterward — so the stored values must always be world-space.
        // Scale is stored as a multiplier relative to the prefab's native localScale
        // (BaseScale): finalScale = BaseScale * MapScriptScale.
        Vector3 baseScale = GetBaseScale(scriptObject.Asset);

        scriptObject.SetPosition(obj.transform.position);
        scriptObject.SetRotation(obj.transform.rotation);
        scriptObject.SetScale(DivideScale(obj.transform.localScale, baseScale));

        return scriptObject;
    }

    void ApplyMarkerMaterial(MapObjectPrefabMarker marker, MapScriptSceneObject scriptObject)
    {
        string shader = marker.MaterialShader;

        if (shader == "Basic" || shader == "Transparent")
        {
            var material = new MapScriptBasicMaterial();
            material.Shader = shader;
            material.Color = new Color255(marker.MaterialColor);
            material.Texture = marker.MaterialTexture;
            material.Tiling = marker.MaterialTiling;
            material.Offset = marker.MaterialOffset;
            scriptObject.Material = material;
        }
        else if (shader == "DefaultTiled")
        {
            var material = new MapScriptDefaultTiledMaterial();
            material.Shader = shader;
            material.Color = new Color255(marker.MaterialColor);
            material.Tiling = marker.MaterialTiling;
            scriptObject.Material = material;
        }
        else if (shader == "Reflective")
        {
            var material = new MapScriptReflectiveMaterial();
            material.Shader = shader;
            material.Color = new Color255(marker.MaterialColor);
            material.Texture = marker.MaterialTexture;
            material.Tiling = marker.MaterialTiling;
            material.Offset = marker.MaterialOffset;
            material.ReflectColor = new Color255(marker.ReflectColor);
            scriptObject.Material = material;
        }
        else // Default or other
        {
            var material = new MapScriptBaseMaterial();
            material.Shader = shader;
            material.Color = new Color255(marker.MaterialColor);
            scriptObject.Material = material;
        }
    }


    void ExtractPhysicsInfo(GameObject obj, MapScriptSceneObject scriptObject)
    {
        var collider = obj.GetComponent<Collider>();
        if (collider == null)
            collider = obj.GetComponentInChildren<Collider>();

        if (collider != null)
        {
            if (!collider.enabled)
            {
                scriptObject.CollideMode = MapObjectCollideMode.None;
            }
            else if (collider.isTrigger)
            {
                scriptObject.CollideMode = MapObjectCollideMode.Region;
            }
            else
            {
                scriptObject.CollideMode = MapObjectCollideMode.Physical;
            }

            // Try to determine CollideWith from layer (use layer index directly)
            int layer = obj.layer;
            if (layer == 24) // MapObjectAll
                scriptObject.CollideWith = MapObjectCollideWith.All;
            else if (layer == 22) // MapObjectCharacters
                scriptObject.CollideWith = MapObjectCollideWith.Characters;
            else if (layer == 29) // MapObjectTitans
                scriptObject.CollideWith = MapObjectCollideWith.Titans;
            else if (layer == 30) // MapObjectHumans
                scriptObject.CollideWith = MapObjectCollideWith.Humans;
            else if (layer == 21) // MapObjectProjectiles
                scriptObject.CollideWith = MapObjectCollideWith.Projectiles;
            else if (layer == 23) // MapObjectEntities
                scriptObject.CollideWith = MapObjectCollideWith.Entities;
            else if (layer == 20) // MapObjectMapObjects
                scriptObject.CollideWith = MapObjectCollideWith.MapObjects;
            else if (layer == 13) // Hurtbox
                scriptObject.CollideWith = MapObjectCollideWith.Hitboxes;
            else if (layer == 25) // MapEditorObject
                scriptObject.CollideWith = MapObjectCollideWith.MapEditor;

            // Physics material
            if (collider.material != null && collider.sharedMaterial != null)
            {
                string matName = collider.sharedMaterial.name;
                if (matName.Contains("Ice"))
                    scriptObject.PhysicsMaterial = MapObjectPhysicsMaterial.Ice;
            }
        }
    }

    void ExtractComponents(GameObject obj, MapScriptSceneObject scriptObject, MapObjectPrefabMarker marker)
    {
#if UNITY_EDITOR
        // Use marker components if available
        if (marker != null && marker.CustomComponents != null && marker.CustomComponents.Count > 0)
        {
            foreach (var compData in marker.CustomComponents)
            {
                MapScriptComponent component = new MapScriptComponent();
                component.ComponentName = compData.ComponentName;
                component.Parameters = new List<string>(compData.Parameters);
                
                // Only add if not already in the list
                bool exists = scriptObject.Components.Any(c => c.ComponentName == component.ComponentName);
                if (!exists)
                {
                    scriptObject.Components.Add(component);
                }
            }
            return;
        }
#endif

        // Otherwise try to detect from Unity components
        
        // Check for Light component
        var light = obj.GetComponent<Light>();
        if (light != null)
        {
            if (light.type == LightType.Directional)
            {
                if (!scriptObject.Components.Any(c => c.ComponentName == "Daylight"))
                {
                    MapScriptComponent component = new MapScriptComponent();
                    component.ComponentName = "Daylight";
                    component.Parameters = new List<string>();
                    scriptObject.Components.Add(component);
                }
            }
            else if (light.type == LightType.Point)
            {
                if (!scriptObject.Components.Any(c => c.ComponentName == "PointLight"))
                {
                    MapScriptComponent component = new MapScriptComponent();
                    component.ComponentName = "PointLight";
                    component.Parameters = new List<string>();
                    scriptObject.Components.Add(component);
                }
            }
        }

        // Check for common Unity tags and convert to Tag components
        if (!string.IsNullOrEmpty(obj.tag) && obj.tag != "Untagged")
        {
            string tagName = obj.tag;
            if (!scriptObject.Components.Any(c => c.ComponentName == "Tag"))
            {
                AddTagComponent(scriptObject, tagName);
            }
        }

        // Note: More sophisticated component extraction would require:
        // 1. Custom marker components on GameObjects
        // 2. Parsing BaseLogic.txt to understand component structure
        // 3. Custom editor inspectors to set component data
    }

    void AddTagComponent(MapScriptSceneObject scriptObject, string tagName)
    {
        MapScriptComponent component = new MapScriptComponent();
        component.ComponentName = "Tag";
        component.Parameters = new List<string> { "Name:" + tagName };
        scriptObject.Components.Add(component);
    }

    /// <summary>
    /// Gets the prefab asset's native localScale (BaseScale). The runtime uses this as a
    /// multiplier base: finalScale = BaseScale * MapScriptScale. We need to divide by it
    /// when exporting so that the MapScript stores the correct multiplier.
    /// </summary>
    Vector3 GetBaseScale(string asset)
    {
        if (asset == "None" || string.IsNullOrEmpty(asset))
            return Vector3.one;

        if (_baseScaleCache.ContainsKey(asset))
            return _baseScaleCache[asset];

        Vector3 baseScale = Vector3.one;

        string[] parts = asset.Split('/');
        if (parts.Length >= 2)
        {
            string category = parts[0];
            string assetName = parts[parts.Length - 1];

            string resourcePath = "Map/" + category + "/Prefabs/" + assetName;
            GameObject prefab = Resources.Load<GameObject>(resourcePath);

            if (prefab == null)
            {
                resourcePath = "Map/" + category + "/" + assetName;
                prefab = Resources.Load<GameObject>(resourcePath);
            }

            if (prefab != null)
            {
                baseScale = prefab.transform.localScale;
            }
        }

        _baseScaleCache[asset] = baseScale;
        return baseScale;
    }

    static Vector3 DivideScale(Vector3 sceneScale, Vector3 baseScale)
    {
        return new Vector3(
            baseScale.x != 0f ? sceneScale.x / baseScale.x : sceneScale.x,
            baseScale.y != 0f ? sceneScale.y / baseScale.y : sceneScale.y,
            baseScale.z != 0f ? sceneScale.z / baseScale.z : sceneScale.z
        );
    }

    string SanitizeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "Unnamed";

        // Remove characters used in MapScript CSV parsing
        char[] invalidChars = new char[] { ',', ';', '/', '\\', '\n', '\r', '|', ':' };
        foreach (char c in invalidChars)
        {
            name = name.Replace(c.ToString(), string.Empty);
        }

        // Limit to 255 characters
        if (name.Length > 255)
            name = name.Substring(0, 255);

        return name.Trim();
    }
}
