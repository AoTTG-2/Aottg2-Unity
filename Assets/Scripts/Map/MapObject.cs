using CustomLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Map
{
    class MapObject : IEquatable<MapObject>
    {
        public int Parent
        {
            get => ScriptObject.Parent;
            set
            {
                ScriptObject.Parent = value;
            }
        }
        public GameObject GameObject;
        public MapScriptBaseObject ScriptObject;
        public Vector3 BaseScale;
        public List<CustomLogicComponentInstance> ComponentInstances = new List<CustomLogicComponentInstance>();
        public Renderer[] renderCache;
        public Collider[] colliderCache;
        public bool RuntimeCreated = false;
        public string Name => ScriptObject.Name; // debug $"{ScriptObject.Name}: ({ScriptObject.Id}, {SiblingIndex})<-({Parent})";
        private int _parent;

        /// New fields for serialization
        // Editor
        public int SiblingIndex = 0;
        public int Level = 0;
        public bool Expanded = false;
        public bool HasChildren => MapLoader.IdToChildren.ContainsKey(ScriptObject.Id) && MapLoader.IdToChildren[ScriptObject.Id].Count > 0;

        // Networking
        public bool NetworkedMovement = false;

        // Rendering
        public ShadowCastingMode ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        public bool ReceiveShadows = true;

        public MapObject(int parent, GameObject gameObject, MapScriptBaseObject scriptObject)
        {
            ScriptObject = scriptObject;
            Parent = parent;
            GameObject = gameObject;
            colliderCache = GameObject.GetComponentsInChildren<Collider>();
            if (scriptObject.Static)
            {
                renderCache = new Renderer[0];
            }
            else
            {
                renderCache = GameObject.GetComponentsInChildren<Renderer>();
            }
        }

        public void RegisterComponentInstance(CustomLogicComponentInstance instance)
        {
            ComponentInstances.Add(instance);
        }

        public CustomLogicComponentInstance FindComponentInstance(string name)
        {
            foreach (var instance in ComponentInstances)
            {
                if (instance.ClassName == name)
                    return instance;
            }
            return null;
        }

        public bool Equals(MapObject other)
        {
            return other != null &&
                   other.ScriptObject.Id == ScriptObject.Id &&
                   other.GameObject == GameObject;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ScriptObject.Id, GameObject);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as MapObject);
        }

    }
}
