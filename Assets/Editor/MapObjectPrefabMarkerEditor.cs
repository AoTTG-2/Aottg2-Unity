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
    private int _selectedPrefabIndex = 0;
    private bool _showComponents = true;
    private bool _showPhysics = true;
    private bool _showMaterial = true;

    void OnEnable()
    {
        _marker = (MapObjectPrefabMarker)target;
        LoadAvailablePrefabs();
        LoadAvailableOptions();
        
        // Find current prefab index
        if (!string.IsNullOrEmpty(_marker.PrefabName))
        {
            _selectedPrefabIndex = _availablePrefabs.IndexOf(_marker.PrefabName);
            if (_selectedPrefabIndex < 0)
                _selectedPrefabIndex = 0;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.HelpBox("EDITOR-ONLY: This component is stripped from builds. " +
            "It stores prefab identity and properties for MapScript export.", MessageType.Info);
        
        EditorGUILayout.Space();

        // === PREFAB IDENTITY ===
        EditorGUILayout.LabelField("Prefab Identity", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        _selectedPrefabIndex = EditorGUILayout.Popup("Prefab Name", _selectedPrefabIndex, _availablePrefabs.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            _marker.PrefabName = _availablePrefabs[_selectedPrefabIndex];
            EditorUtility.SetDirty(_marker);
        }

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
                var shaderIndex = _availableShaders.IndexOf(_marker.MaterialShader);
                if (shaderIndex < 0) shaderIndex = 0;
                
                EditorGUI.BeginChangeCheck();
                shaderIndex = EditorGUILayout.Popup("Shader", shaderIndex, _availableShaders.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    _marker.MaterialShader = _availableShaders[shaderIndex];
                    EditorUtility.SetDirty(_marker);
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaterialColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MaterialTexture"));
                
                // Show tiling/offset for textured materials
                if (_marker.MaterialShader == "Basic" || _marker.MaterialShader == "Transparent" || 
                    _marker.MaterialShader == "Reflective" || _marker.MaterialShader == "DefaultTiled")
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("MaterialTiling"));
                    
                    if (_marker.MaterialShader != "DefaultTiled")
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaterialOffset"));
                    }
                }

                // Show reflect color for reflective
                if (_marker.MaterialShader == "Reflective")
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ReflectColor"));
                }
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

        var prefabListAsset = Resources.Load<TextAsset>("Info/MapPrefabList");
        if (prefabListAsset == null)
            return;

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

    void LoadAvailablePrefabs()
    {
        _availablePrefabs.Clear();
        _availablePrefabs.Add("(None)");

        var prefabListAsset = Resources.Load<TextAsset>("Info/MapPrefabList");
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
