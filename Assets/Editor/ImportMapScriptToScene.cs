using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Map;
using Utility;
using MapEditor;
using UnityEngine.SceneManagement;
using SimpleJSONFixed;

public class ImportMapScriptToScene : EditorWindow
{
    private string _importFilePath = "";
    private bool _addMarkers = true;
    private bool _preserveExisting = false;
    private bool _createParentHierarchy = true;

    private static Dictionary<string, Vector2> _textureTilingCache;

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

    }

    void ImportMap()
    {
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
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", "Failed to parse MapScript:\n" + e.Message, "OK");
            Debug.LogError("MapScript parse error: " + e);
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
                    var marker = go.GetComponent<MapObjectPrefabMarker>();
                    if (marker == null)
                        marker = go.AddComponent<MapObjectPrefabMarker>();
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

                    // Apply material to scene renderers
                    ApplyMaterialPreview(go, marker);
                    
                    // Add components
                    foreach (var comp in sceneObj.Components)
                    {
                        var compData = new MapObjectPrefabMarker.ComponentData();
                        compData.ComponentName = comp.ComponentName;
                        compData.Parameters = new List<string>(comp.Parameters);
                        marker.CustomComponents.Add(compData);
                    }

                    // Configure Unity components in the editor from marker data
                    ApplyEditorComponents(go, marker);
                }
#endif

        // Apply visual overrides for non-active / non-visible objects
        ApplyVisualOverrides(go, sceneObj);
                
        created++;
    }
    else
    {
        failed++;
        Debug.LogWarning($"Failed to create: {sceneObj.Name} (ID: {sceneObj.Id}, Asset: {sceneObj.Asset})");
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
                    Debug.LogWarning($"Object {sceneObj.Name} (ID: {sceneObj.Id}) has invalid parent ID {sceneObj.Parent}");
                }
            }
        }

        EditorUtility.DisplayDialog("Import Complete", 
            $"Successfully imported {created} objects from MapScript.\n" +
            (failed > 0 ? $"{failed} objects failed to import. Check console for details." : ""), 
            "OK");
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
                        Debug.LogWarning($"Could not load asset '{sceneObj.Asset}' for {sceneObj.Name}");
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
        go.isStatic = sceneObj.Static;

        // Set transform (use world space for now, will be adjusted when parenting)
        // MapScript stores scale as a multiplier relative to the prefab's native
        // localScale (BaseScale). Capture it before overwriting so we can recover
        // the correct scene scale: finalScale = BaseScale * MapScriptScale.
        Vector3 baseScale = go.transform.localScale;
        Vector3 mapScale = sceneObj.GetScale();
        go.transform.position = sceneObj.GetPosition();
        go.transform.rotation = Quaternion.Euler(sceneObj.GetRotation());
        go.transform.localScale = new Vector3(
            mapScale.x * baseScale.x,
            mapScale.y * baseScale.y,
            mapScale.z * baseScale.z);

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

    void ApplyMaterialPreview(GameObject go, MapObjectPrefabMarker marker)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return;

        if (_textureTilingCache == null)
            LoadTextureTilingCache();

        string shader = marker.MaterialShader;

        foreach (Renderer renderer in renderers)
        {
            if (renderer.name == "OutlineGizmo")
                continue;

            if (renderer.sharedMaterial == null)
                continue;

            Material mat;

            if (shader == MapObjectShader.Default || shader == MapObjectShader.DefaultNoTint || shader == MapObjectShader.DefaultTiled)
            {
                mat = new Material(renderer.sharedMaterial);
                if (shader != MapObjectShader.DefaultNoTint)
                    mat.color = marker.MaterialColor;
                if (shader == MapObjectShader.DefaultTiled)
                    mat.mainTextureScale = marker.MaterialTiling;
            }
            else
            {
                Material template = Resources.Load<Material>("Map/Materials/" + shader + "Material");
                if (template != null)
                    mat = new Material(template);
                else
                    mat = new Material(renderer.sharedMaterial);

                mat.color = marker.MaterialColor;

                string texturePath = marker.MaterialTexture;
                if (!string.IsNullOrEmpty(texturePath) && texturePath != "Misc/None" && texturePath != "None")
                {
                    string[] texParts = texturePath.Split('/');
                    if (texParts.Length == 2)
                    {
                        string texCategory = texParts[0];
                        string texName = texParts[1];

                        if (texCategory != "Legacy")
                        {
                            Texture2D texture = Resources.Load<Texture2D>("Map/Textures/" + texCategory + "/" + texName + "Texture");
                            if (texture != null)
                            {
                                mat.mainTexture = texture;

                                float refTilingX = 1f, refTilingY = 1f;
                                if (_textureTilingCache.TryGetValue(texName, out Vector2 refTiling))
                                {
                                    refTilingX = refTiling.x;
                                    refTilingY = refTiling.y;
                                }
                                mat.mainTextureScale = new Vector2(
                                    marker.MaterialTiling.x * refTilingX,
                                    marker.MaterialTiling.y * refTilingY);
                                mat.mainTextureOffset = marker.MaterialOffset;
                            }
                        }
                    }
                }
                else
                {
                    mat.mainTextureScale = marker.MaterialTiling;
                    mat.mainTextureOffset = marker.MaterialOffset;
                }

                if (shader == MapObjectShader.Reflective && mat.HasProperty("_SpecularMap"))
                {
                    mat.SetColor("_SpecularMap", marker.ReflectColor);
                }
            }

            renderer.material = mat;
        }
    }

    static void LoadTextureTilingCache()
    {
        _textureTilingCache = new Dictionary<string, Vector2>();

        var textureListAsset = Resources.Load<TextAsset>("Data/Info/MapTextureList");
        if (textureListAsset == null)
            return;

        var textureList = JSON.Parse(textureListAsset.text);
        foreach (string category in textureList.Keys)
        {
            foreach (JSONNode textureNode in textureList[category])
            {
                string name = textureNode["Name"].Value;
                float tilingX = textureNode.HasKey("TilingX") ? textureNode["TilingX"].AsFloat : 1f;
                float tilingY = textureNode.HasKey("TilingY") ? textureNode["TilingY"].AsFloat : 1f;
                if (!_textureTilingCache.ContainsKey(name))
                    _textureTilingCache[name] = new Vector2(tilingX, tilingY);
            }
        }
    }

    void ApplyEditorComponents(GameObject go, MapObjectPrefabMarker marker)
    {
        foreach (var compData in marker.CustomComponents)
        {
            if (compData.ComponentName == "Daylight")
            {
                ApplyLightComponent(go, compData, LightType.Directional);
            }
            else if (compData.ComponentName == "PointLight")
            {
                ApplyLightComponent(go, compData, LightType.Point);
            }
            else if (compData.ComponentName == "SpotLight")
            {
                ApplyLightComponent(go, compData, LightType.Spot);
            }
        }
    }

    void ApplyLightComponent(GameObject go, MapObjectPrefabMarker.ComponentData compData, LightType type)
    {
        var light = go.GetComponent<Light>();
        if (light == null)
            light = go.AddComponent<Light>();

        light.type = type;

        var paramDict = ParseComponentParameters(compData.Parameters);

        if (paramDict.TryGetValue("Color", out string colorStr))
            light.color = ParseColor255(colorStr);

        if (paramDict.TryGetValue("Intensity", out string intensityStr) && float.TryParse(intensityStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float intensity))
            light.intensity = intensity;

        if (paramDict.TryGetValue("Range", out string rangeStr) && float.TryParse(rangeStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float range))
            light.range = range;

        if (paramDict.TryGetValue("Angle", out string angleStr) && float.TryParse(angleStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float angle))
            light.spotAngle = angle;

        if (type == LightType.Directional)
        {
            light.shadows = LightShadows.Soft;
            light.shadowStrength = 0.8f;
            light.shadowBias = 0.2f;

            if (paramDict.TryGetValue("ShadowType", out string shadowTypeStr))
            {
                if (shadowTypeStr == "None")
                    light.shadows = LightShadows.None;
                else if (shadowTypeStr == "Hard")
                    light.shadows = LightShadows.Hard;
                else
                    light.shadows = LightShadows.Soft;
            }

            if (paramDict.TryGetValue("ShadowStrength", out string shadowStrStr) && float.TryParse(shadowStrStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float shadowStr))
                light.shadowStrength = shadowStr;
        }
        else
        {
            light.shadows = LightShadows.None;
            light.renderMode = LightRenderMode.ForcePixel;
            light.bounceIntensity = 0f;
        }
    }

    Dictionary<string, string> ParseComponentParameters(List<string> parameters)
    {
        var dict = new Dictionary<string, string>();
        foreach (var param in parameters)
        {
            int colonIndex = param.IndexOf(':');
            if (colonIndex > 0 && colonIndex < param.Length - 1)
            {
                string key = param.Substring(0, colonIndex);
                string value = param.Substring(colonIndex + 1);
                dict[key] = value;
            }
        }
        return dict;
    }

    Color ParseColor255(string colorStr)
    {
        string[] parts = colorStr.Split('/');
        if (parts.Length >= 3)
        {
            int.TryParse(parts[0], out int r);
            int.TryParse(parts[1], out int g);
            int.TryParse(parts[2], out int b);
            int a = 255;
            if (parts.Length >= 4)
                int.TryParse(parts[3], out a);
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        return Color.white;
    }

    void ApplyVisualOverrides(GameObject go, MapScriptSceneObject sceneObj)
    {
        Material overrideMat = null;

        if (!sceneObj.Active)
        {
            overrideMat = new Material(Shader.Find("Standard"));
            overrideMat.name = "InactiveOverride";
            SetMaterialTransparent(overrideMat, new Color(1f, 0.5f, 0f, 0.1f));
        }
        else if (!sceneObj.Visible)
        {
            overrideMat = new Material(Shader.Find("Standard"));
            overrideMat.name = "NotVisibleOverride";
            SetMaterialTransparent(overrideMat, new Color(0.5f, 0.8f, 1f, 0.25f));
        }

        if (overrideMat != null)
        {
            var renderers = go.GetComponentsInChildren<Renderer>(true);
            foreach (var renderer in renderers)
            {
                var mats = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < mats.Length; i++)
                    mats[i] = overrideMat;
                renderer.sharedMaterials = mats;
            }
        }
    }

    void SetMaterialTransparent(Material mat, Color color)
    {
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        mat.color = color;
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

