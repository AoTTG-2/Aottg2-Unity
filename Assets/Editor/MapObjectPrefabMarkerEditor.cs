using UnityEngine;
using UnityEditor;
using MapEditor;
using System.Collections.Generic;
using System.Linq;
using Map;
using SimpleJSONFixed;
using Utility;

[CustomEditor(typeof(MapObjectPrefabMarker))]
public class MapObjectPrefabMarkerEditor : Editor
{
    private MapObjectPrefabMarker _marker;
    private List<string> _availablePrefabs = new List<string>();
    private List<string> _availableComponents = new List<string>();
    private List<string> _availableShaders = new List<string>();
    private List<string> _availableCollideModes = new List<string>();
    private List<string> _availableCollideWith = new List<string>();
    private bool _showComponents = true;
    private bool _showPhysics = true;
    private bool _showMaterial = true;

    // Texture list loaded from MapTextureList.json
    private static Dictionary<string, List<TextureEntry>> _textureCategories;
    private static Dictionary<string, TextureEntry> _allTextures;

    private struct TextureEntry
    {
        public string Name;
        public string Category;
        public string Path; // "Category/Name"
        public float TilingX;
        public float TilingY;
    }

    void OnEnable()
    {
        _marker = (MapObjectPrefabMarker)target;
        LoadAvailablePrefabs();
        LoadAvailableOptions();
        LoadTextureList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("EDITOR-ONLY: This component is stripped from builds. " +
            "It stores prefab identity and properties for MapScript export.", MessageType.Info);
        
        EditorGUILayout.Space();

        // === PREFAB IDENTITY ===
        EditorGUILayout.LabelField("Prefab Identity", EditorStyles.boldLabel);
        
        // Show PrefabName as an editable text field so the value is always visible,
        // with an optional dropdown to pick from the prefab database.
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        string newName = EditorGUILayout.TextField("Prefab Name", _marker.PrefabName);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_marker, "Change Prefab Name");
            _marker.PrefabName = newName;
            EditorUtility.SetDirty(_marker);
        }

        if (GUILayout.Button("Pick", GUILayout.Width(40)))
        {
            GenericMenu menu = new GenericMenu();
            foreach (string prefab in _availablePrefabs)
            {
                string p = prefab;
                menu.AddItem(new GUIContent(p), p == _marker.PrefabName, () =>
                {
                    Undo.RecordObject(_marker, "Pick Prefab Name");
                    _marker.PrefabName = p;
                    EditorUtility.SetDirty(_marker);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomAsset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ObjectId"));
        
        EditorGUILayout.Space();

        // === OBJECT PROPERTIES ===
        EditorGUILayout.LabelField("Object Properties", EditorStyles.boldLabel);
        
        var overrideActive = serializedObject.FindProperty("OverrideActive");
        EditorGUILayout.PropertyField(overrideActive);
        if (overrideActive.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Active"));
            EditorGUI.indentLevel--;
        }

        var overrideStatic = serializedObject.FindProperty("OverrideStatic");
        EditorGUILayout.PropertyField(overrideStatic);
        if (overrideStatic.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Static"));
            EditorGUI.indentLevel--;
        }
        
        var overrideVisible = serializedObject.FindProperty("OverrideVisible");
        EditorGUILayout.PropertyField(overrideVisible);
        if (overrideVisible.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Visible"));
            EditorGUI.indentLevel--;
        }
        
        var overrideNetworked = serializedObject.FindProperty("OverrideNetworked");
        EditorGUILayout.PropertyField(overrideNetworked);
        if (overrideNetworked.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Networked"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // === PHYSICS ===
        _showPhysics = EditorGUILayout.Foldout(_showPhysics, "Physics Settings", true, EditorStyles.foldoutHeader);
        if (_showPhysics)
        {
            EditorGUI.indentLevel++;
            var overridePhysics = serializedObject.FindProperty("OverridePhysics");
            EditorGUILayout.PropertyField(overridePhysics);
            
            if (overridePhysics.boolValue)
            {
                var collideModeIndex = _availableCollideModes.IndexOf(_marker.CollideMode);
                if (collideModeIndex < 0) collideModeIndex = 0;
                
                EditorGUI.BeginChangeCheck();
                collideModeIndex = EditorGUILayout.Popup("Collide Mode", collideModeIndex, _availableCollideModes.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    _marker.CollideMode = _availableCollideModes[collideModeIndex];
                    EditorUtility.SetDirty(_marker);
                }

                var collideWithIndex = _availableCollideWith.IndexOf(_marker.CollideWith);
                if (collideWithIndex < 0) collideWithIndex = 0;
                
                EditorGUI.BeginChangeCheck();
                collideWithIndex = EditorGUILayout.Popup("Collide With", collideWithIndex, _availableCollideWith.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    _marker.CollideWith = _availableCollideWith[collideWithIndex];
                    EditorUtility.SetDirty(_marker);
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("PhysicsMaterial"));
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // === MATERIAL ===
        _showMaterial = EditorGUILayout.Foldout(_showMaterial, "Material Settings", true, EditorStyles.foldoutHeader);
        if (_showMaterial)
        {
            EditorGUI.indentLevel++;
            var overrideMaterial = serializedObject.FindProperty("OverrideMaterial");
            EditorGUILayout.PropertyField(overrideMaterial);
            
            if (overrideMaterial.boolValue)
            {
                // Shader dropdown — mirrors in-game editor
                var shaderIndex = _availableShaders.IndexOf(_marker.MaterialShader);
                if (shaderIndex < 0) shaderIndex = 0;
                
                EditorGUI.BeginChangeCheck();
                shaderIndex = EditorGUILayout.Popup("Shader", shaderIndex, _availableShaders.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_marker, "Change Shader");
                    _marker.MaterialShader = _availableShaders[shaderIndex];
                    EditorUtility.SetDirty(_marker);
                    ApplyMaterialPreview();
                }

                // Color — hidden for DefaultNoTint (same as in-game)
                if (_marker.MaterialShader != MapObjectShader.DefaultNoTint)
                {
                    EditorGUI.BeginChangeCheck();
                    Color newColor = EditorGUILayout.ColorField("Color", _marker.MaterialColor);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_marker, "Change Material Color");
                        _marker.MaterialColor = newColor;
                        EditorUtility.SetDirty(_marker);
                        ApplyMaterialPreview();
                    }
                }

                // Texture picker — only for shaders that support textures
                bool showTexture = _marker.MaterialShader == MapObjectShader.Basic ||
                                   _marker.MaterialShader == MapObjectShader.Transparent ||
                                   _marker.MaterialShader == MapObjectShader.Reflective;
                if (showTexture)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Texture");
                    if (GUILayout.Button(_marker.MaterialTexture, EditorStyles.popup))
                    {
                        ShowTexturePicker();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // Tiling — for Basic, Transparent, Reflective, DefaultTiled
                bool showTiling = showTexture || _marker.MaterialShader == MapObjectShader.DefaultTiled;
                if (showTiling)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector2 newTiling = EditorGUILayout.Vector2Field("Tiling", _marker.MaterialTiling);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_marker, "Change Tiling");
                        _marker.MaterialTiling = newTiling;
                        EditorUtility.SetDirty(_marker);
                        ApplyMaterialPreview();
                    }
                }

                // Offset — for textured shaders but not DefaultTiled
                if (showTexture && _marker.MaterialShader != MapObjectShader.DefaultTiled)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector2 newOffset = EditorGUILayout.Vector2Field("Offset", _marker.MaterialOffset);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_marker, "Change Offset");
                        _marker.MaterialOffset = newOffset;
                        EditorUtility.SetDirty(_marker);
                        ApplyMaterialPreview();
                    }
                }

                // Reflect color — only for Reflective shader
                if (_marker.MaterialShader == MapObjectShader.Reflective)
                {
                    EditorGUI.BeginChangeCheck();
                    Color newReflect = EditorGUILayout.ColorField("Reflect Color", _marker.ReflectColor);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_marker, "Change Reflect Color");
                        _marker.ReflectColor = newReflect;
                        EditorUtility.SetDirty(_marker);
                        ApplyMaterialPreview();
                    }
                }

                EditorGUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Apply Preview"))
                {
                    ApplyMaterialPreview();
                }
                if (GUILayout.Button("Reset Preview"))
                {
                    ResetMaterialPreview();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // === COMPONENTS ===
        _showComponents = EditorGUILayout.Foldout(_showComponents, "Custom Components", true, EditorStyles.foldoutHeader);
        
        if (_showComponents)
        {
            EditorGUI.indentLevel++;
            
            var componentsProperty = serializedObject.FindProperty("CustomComponents");
            
            for (int i = 0; i < componentsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                var componentProperty = componentsProperty.GetArrayElementAtIndex(i);
                
                EditorGUILayout.PropertyField(componentProperty.FindPropertyRelative("ComponentName"));
                EditorGUILayout.PropertyField(componentProperty.FindPropertyRelative("Parameters"), true);
                
                if (GUILayout.Button("Remove Component"))
                {
                    componentsProperty.DeleteArrayElementAtIndex(i);
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            if (GUILayout.Button("Add Component"))
            {
                componentsProperty.arraySize++;
                var newComponent = componentsProperty.GetArrayElementAtIndex(componentsProperty.arraySize - 1);
                newComponent.FindPropertyRelative("ComponentName").stringValue = "";
                newComponent.FindPropertyRelative("Parameters").ClearArray();
            }
            
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        
        if (GUILayout.Button("Copy Values from Prefab Definition"))
        {
            CopyFromPrefabDefinition();
        }
    }

    void CopyFromPrefabDefinition()
    {
        if (string.IsNullOrEmpty(_marker.PrefabName) || _marker.PrefabName == "(None)")
        {
            EditorUtility.DisplayDialog("Error", "No prefab name set", "OK");
            return;
        }

        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not load Data/Info/MapPrefabList", "OK");
            return;
        }

        var prefabList = JSON.Parse(prefabListAsset.text);
        
        // Find the prefab in JSON
        JSONNode foundPrefab = null;
        JSONNode foundInfo = null;
        
        foreach (string categoryKey in prefabList.Keys)
        {
            JSONNode categoryNode = prefabList[categoryKey];
            JSONNode info = categoryNode["Info"];

            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                if (prefabNode["Name"].Value == _marker.PrefabName)
                {
                    foundPrefab = prefabNode;
                    foundInfo = info;
                    break;
                }
            }
            
            if (foundPrefab != null)
                break;
        }

        if (foundPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", $"Could not find prefab '{_marker.PrefabName}' in JSON", "OK");
            return;
        }

        // Apply values from JSON (same logic as PopulateMarkerFromJson)
        Undo.RecordObject(_marker, "Copy from Prefab Definition");

        if (foundPrefab.HasKey("Static"))
        {
            _marker.OverrideStatic = true;
            _marker.Static = foundPrefab["Static"].AsBool;
        }

        if (foundPrefab.HasKey("Visible"))
        {
            _marker.OverrideVisible = true;
            _marker.Visible = foundPrefab["Visible"].AsBool;
        }

        if (foundPrefab.HasKey("Networked"))
        {
            _marker.OverrideNetworked = true;
            _marker.Networked = foundPrefab["Networked"].AsBool;
        }

        if (foundPrefab.HasKey("CollideMode") || foundPrefab.HasKey("CollideWith"))
        {
            _marker.OverridePhysics = true;
            _marker.CollideMode = foundPrefab.HasKey("CollideMode") ? foundPrefab["CollideMode"].Value : "Physical";
            _marker.CollideWith = foundPrefab.HasKey("CollideWith") ? foundPrefab["CollideWith"].Value : "Entities";
        }

        if (foundPrefab.HasKey("Material"))
        {
            _marker.OverrideMaterial = true;
            ParseMaterialString(_marker, foundPrefab["Material"].Value);
        }

        EditorUtility.SetDirty(_marker);
        EditorUtility.DisplayDialog("Success", "Copied properties from prefab definition", "OK");
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
        {
            marker.MaterialTexture = parts[2];
        }

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

    void ShowTexturePicker()
    {
        if (_textureCategories == null)
            LoadTextureList();

        GenericMenu menu = new GenericMenu();

        foreach (var kvp in _textureCategories)
        {
            string category = kvp.Key;
            foreach (var tex in kvp.Value)
            {
                string texPath = tex.Path;
                menu.AddItem(new GUIContent(category + "/" + tex.Name), texPath == _marker.MaterialTexture, () =>
                {
                    Undo.RecordObject(_marker, "Change Texture");
                    _marker.MaterialTexture = texPath;
                    EditorUtility.SetDirty(_marker);
                    ApplyMaterialPreview();
                });
            }
        }

        menu.ShowAsContext();
    }

    void ApplyMaterialPreview()
    {
        if (_marker == null)
            return;

        GameObject go = _marker.gameObject;
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return;

        string shader = _marker.MaterialShader;

        foreach (Renderer renderer in renderers)
        {
            if (renderer.name == "OutlineGizmo")
                continue;

            // Create a new instance so we never modify the shared asset material
            Material mat;

            if (shader == MapObjectShader.Default || shader == MapObjectShader.DefaultNoTint || shader == MapObjectShader.DefaultTiled)
            {
                // Start from the prefab's original material
                mat = new Material(renderer.sharedMaterial);
                if (shader != MapObjectShader.DefaultNoTint)
                    mat.color = _marker.MaterialColor;
                if (shader == MapObjectShader.DefaultTiled)
                {
                    mat.mainTextureScale = _marker.MaterialTiling;
                }
            }
            else
            {
                // Load the MapScript material template
                Material template = Resources.Load<Material>("Map/Materials/" + shader + "Material");
                if (template != null)
                    mat = new Material(template);
                else
                    mat = new Material(renderer.sharedMaterial);

                mat.color = _marker.MaterialColor;

                // Apply texture
                string texturePath = _marker.MaterialTexture;
                if (!string.IsNullOrEmpty(texturePath) && texturePath != "Misc/None" && texturePath != "None")
                {
                    string[] texParts = texturePath.Split('/');
                    if (texParts.Length == 2)
                    {
                        string texCategory = texParts[0];
                        string texName = texParts[1];

                        Texture2D texture = null;
                        if (texCategory == "Legacy" && _allTextures.ContainsKey(texName))
                        {
                            // Legacy textures have their own path format; skip for editor preview
                        }
                        else
                        {
                            texture = Resources.Load<Texture2D>("Map/Textures/" + texCategory + "/" + texName + "Texture");
                        }

                        if (texture != null)
                        {
                            mat.mainTexture = texture;

                            // Apply tiling with texture reference tiling multiplied in
                            float refTilingX = 1f, refTilingY = 1f;
                            if (_allTextures != null && _allTextures.ContainsKey(texName))
                            {
                                refTilingX = _allTextures[texName].TilingX;
                                refTilingY = _allTextures[texName].TilingY;
                            }
                            mat.mainTextureScale = new Vector2(
                                _marker.MaterialTiling.x * refTilingX,
                                _marker.MaterialTiling.y * refTilingY);
                            mat.mainTextureOffset = _marker.MaterialOffset;
                        }
                    }
                }
                else
                {
                    mat.mainTextureScale = _marker.MaterialTiling;
                    mat.mainTextureOffset = _marker.MaterialOffset;
                }

                if (shader == MapObjectShader.Reflective && mat.HasProperty("_SpecularMap"))
                {
                    mat.SetColor("_SpecularMap", _marker.ReflectColor);
                }
            }

            renderer.material = mat;
        }
    }

    void ResetMaterialPreview()
    {
        if (_marker == null)
            return;

        // Revert to the prefab's original shared materials
        GameObject go = _marker.gameObject;
        if (PrefabUtility.IsPartOfPrefabInstance(go))
        {
            // Revert renderer material overrides
            var renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var prefabRenderer = PrefabUtility.GetCorrespondingObjectFromSource(renderer);
                if (prefabRenderer != null)
                {
                    renderer.sharedMaterials = prefabRenderer.sharedMaterials;
                }
            }
        }
    }

    static void LoadTextureList()
    {
        if (_textureCategories != null)
            return;

        _textureCategories = new Dictionary<string, List<TextureEntry>>();
        _allTextures = new Dictionary<string, TextureEntry>();

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
                entry.TilingX = textureNode.HasKey("TilingX") ? textureNode["TilingX"].AsFloat : 1f;
                entry.TilingY = textureNode.HasKey("TilingY") ? textureNode["TilingY"].AsFloat : 1f;

                entries.Add(entry);

                if (!_allTextures.ContainsKey(entry.Name))
                    _allTextures[entry.Name] = entry;
            }

            _textureCategories[category] = entries;
        }
    }

    void LoadAvailablePrefabs()
    {
        _availablePrefabs.Clear();
        _availablePrefabs.Add("(None)");

        var prefabListAsset = Resources.Load<TextAsset>("Data/Info/MapPrefabList");
        if (prefabListAsset == null)
            return;

        var prefabList = JSON.Parse(prefabListAsset.text);
        foreach (string categoryKey in prefabList.Keys)
        {
            JSONNode categoryNode = prefabList[categoryKey];
            foreach (JSONNode prefabNode in categoryNode["Prefabs"])
            {
                string prefabName = prefabNode["Name"].Value;
                _availablePrefabs.Add(prefabName);
            }
        }

        _availablePrefabs.Sort();
    }

    void LoadAvailableOptions()
    {
        // Shaders
        _availableShaders = new List<string>
        {
            "Default",
            "DefaultNoTint",
            "DefaultTiled",
            "Basic",
            "Transparent",
            "Reflective"
        };

        // Collide Modes
        _availableCollideModes = new List<string>
        {
            "Physical",
            "Region",
            "None"
        };

        // Collide With
        _availableCollideWith = new List<string>
        {
            "All",
            "MapObjects",
            "Characters",
            "Titans",
            "Humans",
            "Projectiles",
            "Entities",
            "Hitboxes",
            "MapEditor"
        };

        LoadAvailableComponents();
    }

    void LoadAvailableComponents()
    {
        _availableComponents.Clear();
        
        // Common component types from the game
        _availableComponents.AddRange(new string[]
        {
            "Tag",
            "Daylight",
            "PointLight",
            "SupplyStation",
            "Cannon",
            "CaptureDevice",
            "Rigidbody",
            "Floater",
            "Wagon",
            "Dummy",
            "Executioner",
            "RacingFinishRegion",
            "RacingCheckpointRegion",
            "KillRegion",
            "DamageRegion",
            "CaptureZone",
            "Animal",
            "DestructibleTrigger",
            "TitanTarget",
            "Bell",
            "WallGateA1"
        });
    }
}
