using CustomLogic;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Map
{
    class MapObject
    {
        public int Parent;
        public GameObject GameObject;
        public MapScriptBaseObject ScriptObject;
        public Vector3 BaseScale;
        public List<CustomLogicComponentInstance> ComponentInstances = new List<CustomLogicComponentInstance>();
        public Renderer[] renderCache;
        public Collider[] colliderCache;

        public MapObject(int parent, GameObject gameObject, MapScriptBaseObject scriptObject)
        {
            Parent = parent;
            GameObject = gameObject;
            ScriptObject = scriptObject;
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
    }
}
