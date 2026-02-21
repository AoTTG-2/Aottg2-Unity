using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Map;
using Utility;
using MapEditor;
using UnityEngine.SceneManagement;

public class ImportMapScriptToScene : EditorWindow
{
    private string _importFilePath = "";
    private Vector2 _scrollPos;
    private List<string> _importLog = new List<string>();
    private bool _addMarkers = true;
    private bool _preserveExisting = false;
    private bool _createParentHierarchy = true;

    [MenuItem("Tools/Import MapScript to Scene")]
    static void ShowWindow()
    {
        var window = GetWindow<ImportMapScriptToScene>("Import MapScript to Scene");
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Import MapScript to Scene", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("This will load a MapScript file and instantiate objects in the current scene.", MessageType.Info);
        
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MapScript File:", GUILayout.Width(100));
        _importFilePath = EditorGUILayout.TextField(_importFilePath);
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string defaultPath = Path.Combine(Application.dataPath, "..", "ExportedMaps");
            string path = EditorUtility.OpenFilePanel("Select MapScript File", defaultPath, "txt");
            if (!string.IsNullOrEmpty(path))
            {
                _importFilePath = path;
            }
        }
        EditorGUILayout.EndHorizontal();

        _addMarkers = EditorGUILayout.Toggle("Add Prefab Markers", _addMarkers);
        _preserveExisting = EditorGUILayout.Toggle("Preserve Existing Objects", _preserveExisting);
        _createParentHierarchy = EditorGUILayout.Toggle("Create Parent Hierarchy", _createParentHierarchy);

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(_importFilePath) || !File.Exists(_importFilePath));
        if (GUILayout.Button("Import to Scene", GUILayout.Height(30)))
        {
            ImportMap();
        }
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Clear Import Log"))
        {
            _importLog.Clear();
            Repaint();
        }

        EditorGUILayout.Space();

        if (_importLog.Count > 0)
        {
            GUILayout.Label($"Import Log ({_importLog.Count} entries):", EditorStyles.boldLabel);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(300));
            foreach (var log in _importLog)
            {
                EditorGUILayout.LabelField(log, EditorStyles.wordWrappedLabel);
            }
            EditorGUILayout.EndScrollView();
        }
    }

    void ImportMap()
    {
        _importLog.Clear();

        if (!File.Exists(_importFilePath))
        {
            EditorUtility.DisplayDialog("Error", "File does not exist: " + _importFilePath, "OK");
            return;
        }

        string mapScriptContent = File.ReadAllText(_importFilePath);
        MapScript mapScript = new MapScript();
        
        try
        {
            mapScript.Deserialize(mapScriptContent);
            _importLog.Add($"Successfully parsed MapScript with {mapScript.Objects.Objects.Count} objects");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", "Failed to parse MapScript:\n" + e.Message, "OK");
            Debug.LogError("MapScript parse error: " + e);
            _importLog.Add($"ERROR: Failed to parse MapScript - {e.Message}");
            return;
        }

        // Clear existing if not preserving
        if (!_preserveExisting)
        {
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            int cleared = 0;
            foreach (var obj in rootObjects)
            {
                if (!obj.name.StartsWith("NetworkManager") && 
                    !obj.name.StartsWith("EventSystem") &&
                    !obj.name.StartsWith("Main Camera") &&
                    !obj.name.StartsWith("Directional Light"))
                {
                    DestroyImmediate(obj);
                    cleared++;
                }
            }
            _importLog.Add($"Cleared {cleared} existing objects from scene");
        }

        // Track created objects by ID
        Dictionary<int, GameObject> idToGameObject = new Dictionary<int, GameObject>();

        // First pass: create all objects
        int created = 0;
        int failed = 0;
        foreach (MapScriptBaseObject baseObj in mapScript.Objects.Objects)
        {
            if (!(baseObj is MapScriptSceneObject))
                continue;

            MapScriptSceneObject sceneObj = (MapScriptSceneObject)baseObj;
            GameObject go = CreateGameObjectFromScript(sceneObj);
            
            if (go != null)
            {
                idToGameObject[sceneObj.Id] = go;
                
#if UNITY_EDITOR
                // Add marker component if requested
                if (_addMarkers)
                {
                    var marker = go.AddComponent<MapObjectPrefabMarker>();
                    marker.PrefabName = sceneObj.Name;
                    marker.ObjectId = sceneObj.Id;
                    
                    marker.OverrideNetworked = true;
                    marker.Networked = sceneObj.Networked;
                    
                    marker.OverrideVisible = true;
                    marker.Visible = sceneObj.Visible;

                    marker.OverrideStatic = true;
                    marker.Static = sceneObj.Static;

                    marker.OverridePhysics = true;
                    marker.CollideMode = sceneObj.CollideMode;
                    marker.CollideWith = sceneObj.CollideWith;
                    marker.PhysicsMaterial = sceneObj.PhysicsMaterial;

                    // Add material info
                    marker.OverrideMaterial = true;
                    ApplyMaterialToMarker(sceneObj.Material, marker);
                    
                    // Add components
                    foreach (var comp in sceneObj.Components)
                    {
                        var compData = new MapObjectPrefabMarker.ComponentData();
                        compData.ComponentName = comp.ComponentName;
                        compData.Parameters = new List<string>(comp.Parameters);
                        marker.CustomComponents.Add(compData);
                    }
                }
#endif
                
                created++;
                _importLog.Add($"? Created: {sceneObj.Name} (ID: {sceneObj.Id})");
            }
            else
            {
                failed++;
                _importLog.Add($"? Failed: {sceneObj.Name} (ID: {sceneObj.Id}, Asset: {sceneObj.Asset})");
            }
        }

        // Second pass: set parents
        if (_createParentHierarchy)
        {
            int parented = 0;
            foreach (MapScriptBaseObject baseObj in mapScript.Objects.Objects)
            {
                if (!(baseObj is MapScriptSceneObject))
                    continue;

                MapScriptSceneObject sceneObj = (MapScriptSceneObject)baseObj;
                if (sceneObj.Parent > 0 && idToGameObject.ContainsKey(sceneObj.Id) && idToGameObject.ContainsKey(sceneObj.Parent))
                {
                    idToGameObject[sceneObj.Id].transform.SetParent(idToGameObject[sceneObj.Parent].transform, true);
                    parented++;
                }
                else if (sceneObj.Parent > 0)
                {
                    _importLog.Add($"Warning: Object {sceneObj.Name} (ID: {sceneObj.Id}) has invalid parent ID {sceneObj.Parent}");
                }
            }
            _importLog.Add($"Set {parented} parent relationships");
        }

        EditorUtility.DisplayDialog("Import Complete", 
            $"Successfully imported {created} objects from MapScript.\n" +
            (failed > 0 ? $"{failed} objects failed to import." : ""), 
            "OK");
        
        _importLog.Add($"\n=== Import Summary ===");
        _importLog.Add($"Created: {created}");
        _importLog.Add($"Failed: {failed}");
        
        Repaint();
    }

    GameObject CreateGameObjectFromScript(MapScriptSceneObject sceneObj)
    {
        GameObject go = null;

        // Try to load the asset
        if (sceneObj.Asset != "None" && !string.IsNullOrEmpty(sceneObj.Asset))
        {
            string[] parts = sceneObj.Asset.Split('/');
            if (parts.Length >= 2)
            {
                string category = parts[0];
                string assetName = parts[parts.Length - 1];
                
                // Try to load from Resources/Map
                string resourcePath = "Map/" + category + "/Prefabs/" + assetName;
                GameObject prefab = Resources.Load<GameObject>(resourcePath);
                
                if (prefab != null)
                {
                    go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    // Try loading without the category structure (for custom paths like MoMoSync)
                    if (category == "MoMoSync" || category == "MomoModels")
                    {
                        resourcePath = "Map/" + category + "/" + assetName;
                        prefab = Resources.Load<GameObject>(resourcePath);
                        if (prefab != null)
                        {
                            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                        }
                    }
                    
                    if (go == null)
                    {
                        _importLog.Add($"Warning: Could not load asset '{sceneObj.Asset}' for {sceneObj.Name}");
                    }
                }
            }
        }

        // Create empty if no asset loaded
        if (go == null)
        {
            go = new GameObject();
        }

        // Set properties
        go.name = sceneObj.Name;
        go.SetActive(sceneObj.Active);
        go.isStatic = sceneObj.Static;

        // Set transform (use world space for now, will be adjusted when parenting)
        go.transform.position = sceneObj.GetPosition();
        go.transform.rotation = Quaternion.Euler(sceneObj.GetRotation());
        go.transform.localScale = sceneObj.GetScale();

        // Set layer based on CollideWith
        SetLayerFromCollideWith(go, sceneObj.CollideWith);

        // Apply collider settings
        ApplyColliderSettings(go, sceneObj);

        return go;
    }

    void SetLayerFromCollideWith(GameObject go, string collideWith)
    {
        int layer = 0;
        
        if (collideWith == MapObjectCollideWith.All)
            layer = 24;
        else if (collideWith == MapObjectCollideWith.Characters)
            layer = 22;
        else if (collideWith == MapObjectCollideWith.Titans)
            layer = 29;
        else if (collideWith == MapObjectCollideWith.Humans)
            layer = 30;
        else if (collideWith == MapObjectCollideWith.Projectiles)
            layer = 21;
        else if (collideWith == MapObjectCollideWith.Entities)
            layer = 23;
        else if (collideWith == MapObjectCollideWith.MapObjects)
            layer = 20;
        else if (collideWith == MapObjectCollideWith.Hitboxes)
            layer = 13;
        else if (collideWith == MapObjectCollideWith.MapEditor)
            layer = 25;

        go.layer = layer;
        
        // Apply to children as well
        foreach (Transform child in go.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;
        }
    }

    void ApplyColliderSettings(GameObject go, MapScriptSceneObject sceneObj)
    {
        var colliders = go.GetComponentsInChildren<Collider>();
        
        foreach (var collider in colliders)
        {
            if (sceneObj.CollideMode == MapObjectCollideMode.None)
            {
                collider.enabled = false;
            }
            else if (sceneObj.CollideMode == MapObjectCollideMode.Region)
            {
                collider.isTrigger = true;
                collider.enabled = true;
            }
            else // Physical
            {
                collider.isTrigger = false;
                collider.enabled = true;
            }
        }
    }

    void ApplyMaterialToMarker(MapScriptBaseMaterial material, MapObjectPrefabMarker marker)
    {
        marker.MaterialShader = material.Shader;
        marker.MaterialColor = material.Color.ToColor();

        if (material is MapScriptBasicMaterial basicMat)
        {
            marker.MaterialTexture = basicMat.Texture;
            marker.MaterialTiling = basicMat.Tiling;
            marker.MaterialOffset = basicMat.Offset;
        }
        else if (material is MapScriptDefaultTiledMaterial tiledMat)
        {
            marker.MaterialTiling = tiledMat.Tiling;
        }
        else if (material is MapScriptReflectiveMaterial reflectMat)
        {
            marker.MaterialTexture = reflectMat.Texture;
            marker.MaterialTiling = reflectMat.Tiling;
            marker.MaterialOffset = reflectMat.Offset;
            marker.ReflectColor = reflectMat.ReflectColor.ToColor();
        }
    }
}

