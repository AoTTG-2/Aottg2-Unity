#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    /// <summary>
    /// EDITOR-ONLY component to mark GameObjects with MapScript prefab information.
    /// This component is stripped from builds and only exists for Unity-to-MapScript conversion.
    /// </summary>
    [ExecuteInEditMode]
    public class MapObjectPrefabMarker : MonoBehaviour
    {
        [Header("Prefab Identity")]
        [Tooltip("The prefab name from MapPrefabList.json")]
        public string PrefabName;
        
        [Tooltip("Custom asset path override (leave empty to use prefab default)")]
        public string CustomAsset;
        
        [Tooltip("Object ID for MapScript (leave 0 for auto-assign)")]
        public int ObjectId;

        [Header("Object Properties")]
        [Tooltip("Override the active state")]
        public bool OverrideActive;
        public bool Active = true;
        
        [Tooltip("Override the static flag")]
        public bool OverrideStatic;
        public bool Static = true;
        
        [Tooltip("Override the visible flag")]
        public bool OverrideVisible;
        public bool Visible = true;
        
        [Tooltip("Override the networked flag")]
        public bool OverrideNetworked;
        public bool Networked;

        [Header("Physics Settings")]
        [Tooltip("Override physics settings")]
        public bool OverridePhysics;
        public string CollideMode = "Physical";
        public string CollideWith = "Entities";
        public string PhysicsMaterial = "Default";

        [Header("Material Settings")]
        [Tooltip("Override material settings")]
        public bool OverrideMaterial;
        public string MaterialShader = "Default";
        public Color MaterialColor = Color.white;
        public string MaterialTexture = "Misc/None";
        public Vector2 MaterialTiling = Vector2.one;
        public Vector2 MaterialOffset = Vector2.zero;
        public Color ReflectColor = Color.white;
        
        [Header("Custom Components")]
        [Tooltip("Custom components for MapScript")]
        public List<ComponentData> CustomComponents = new List<ComponentData>();

        [System.Serializable]
        public class ComponentData
        {
            public string ComponentName;
            public List<string> Parameters = new List<string>();
        }
    }
}
#endif
