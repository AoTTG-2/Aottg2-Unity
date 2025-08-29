using Map;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Finding, creating, and destroying map objects.
    /// </summary>
    [CLType(Name = "Map", Static = true, Abstract = true)]
    partial class CustomLogicMapBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicMapBuiltin()
        {
        }

        [CLMethod(description: "Find all map objects")]
        public CustomLogicListBuiltin FindAllMapObjects()
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        [CLMethod(description: "Find a map object by name")]
        public CustomLogicMapObjectBuiltin FindMapObjectByName(string objectName)
        {
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    return new CustomLogicMapObjectBuiltin(mapObject);
            }
            return null;
        }

        [CLMethod(description: "Find all map objects by name")]
        public CustomLogicListBuiltin FindMapObjectsByName(string objectName)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        [CLMethod(description: "Find all map objects by regex pattern")]
        public CustomLogicListBuiltin FindMapObjectsByRegex(string pattern, bool sorted = false)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (regex.IsMatch(mapObject.ScriptObject.Name))
                {
                    listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                }
            }

            if (sorted)
            {
                listBuiltin.List.Sort((a, b) => string.Compare(((CustomLogicMapObjectBuiltin)a).Value.ScriptObject.Name, ((CustomLogicMapObjectBuiltin)b).Value.ScriptObject.Name));
            }

            return listBuiltin;
        }

        [CLMethod(description: "Find all map objects by component")]
        public CustomLogicMapObjectBuiltin FindMapObjectByComponent(string className)
        {
            CustomLogicListBuiltin listBuiltin = FindMapObjectsByComponent(className);
            return (CustomLogicMapObjectBuiltin)(listBuiltin.Count > 0 ? listBuiltin.Get(0) : null);
        }

        [CLMethod(description: "Find all map objects by component")]
        public CustomLogicListBuiltin FindMapObjectsByComponent(string className)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                foreach (CustomLogicComponentInstance component in mapObject.ComponentInstances)
                {
                    if (component.ClassName == className)
                    {
                        listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                        break;
                    }
                }
            }
            return listBuiltin;
        }

        [CLMethod(description: "Find a map object by ID")]
        public CustomLogicMapObjectBuiltin FindMapObjectByID(int id)
        {
            if (MapLoader.IdToMapObject.ContainsKey(id))
                return new CustomLogicMapObjectBuiltin(MapLoader.IdToMapObject[id]);
            return null;
        }

        [CLMethod(description: "Find a map object by tag")]
        public CustomLogicMapObjectBuiltin FindMapObjectByTag(string tag)
        {
            if (MapLoader.Tags.ContainsKey(tag))
            {
                if (MapLoader.Tags[tag].Count > 0)
                    return new CustomLogicMapObjectBuiltin(MapLoader.Tags[tag][0]);
            }
            return null;
        }

        [CLMethod(description: "Find all map objects by tag")]
        public CustomLogicListBuiltin FindMapObjectsByTag(string tag)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            if (MapLoader.Tags.ContainsKey(tag))
            {
                {
                    foreach (MapObject mapObject in MapLoader.Tags[tag])
                        listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                }
            }
            return listBuiltin;
        }

        // Scene,Category/Prefab,ID,Parent ID, 
        // Scene,Geometry/Sphere,117,0,1,0,1,1,Ball,-1.524334,50,17.24871,0,0,0,1,1,1,Physical,All,Default,Basic|150/150/150/255|Misc/Diamond1|1/1|0/0,Ball|Mass:1|Gravity:-40|HitForceMax:5000|HitMultiplier:100|HitForceMin:1|DribbleForce:300|DribbleForceMax:1000|VelocityThreshold:10|ForceMultiplier:100,Rigidbody|Mass:1|Gravity:0/-20/0|FreezeRotation:false|Interpolate:false
        // Scene,Buildings/Arch3,118,0,1,1,1,0,Arch3,-59.26698,17.15096,158.3891,0,0,0,1,1,1,Physical,Entities,Default,Default|255/255/255/255,
        [CLMethod(description: "Create a new map object")]
        public CustomLogicMapObjectBuiltin CreateMapObject(CustomLogicPrefabBuiltin prefab, CustomLogicVector3Builtin position = null, CustomLogicVector3Builtin rotation = null, CustomLogicVector3Builtin scale = null)
        {
            var scriptSerial = prefab.Value.Serialize();
            var script = new MapScriptSceneObject();
            script.Deserialize(scriptSerial);
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = 0;
            script.Networked = false;

            if (position != null)
                script.SetPosition(position);
            if (rotation != null)
                script.SetRotation(rotation);
            if (scale != null)
                script.SetScale(scale);

            var mapObject = MapLoader.LoadObject(script, false);
            MapLoader.SetParent(mapObject);
            CustomLogicManager.Evaluator.LoadMapObjectComponents(mapObject, true);
            mapObject.RuntimeCreated = true;
            return new CustomLogicMapObjectBuiltin(mapObject);
        }

        [CLMethod(description: "Create a new map object")]
        public CustomLogicMapObjectBuiltin CreateMapObjectRaw(string prefab)
        {
            prefab = string.Join("", prefab.Split('\n'));
            var script = new MapScriptSceneObject();
            script.Deserialize(prefab);
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = 0;
            script.Networked = false;
            var mapObject = MapLoader.LoadObject(script, false);
            MapLoader.SetParent(mapObject);
            CustomLogicManager.Evaluator.LoadMapObjectComponents(mapObject, true);
            mapObject.RuntimeCreated = true;
            return new CustomLogicMapObjectBuiltin(mapObject);
        }

        [CLMethod(description: "Create a new prefab object from the current object")]
        public CustomLogicPrefabBuiltin PrefabFromMapObject(CustomLogicMapObjectBuiltin mapObject, bool clearComponents = false)
        {
            string serialized = mapObject.Value.ScriptObject.Serialize();
            return new CustomLogicPrefabBuiltin(serialized, clearComponents);
        }

        [CLMethod(description: "Destroy a map object")]
        public void DestroyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren)
        {
            DestroyMapObjectBuiltin(mapObject, includeChildren);
        }

        [CLMethod(description: "Copy a map object")]
        public CustomLogicMapObjectBuiltin CopyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren = true)
        {
            var copy = CopyMapObject(mapObject.Value, mapObject.Value.Parent, includeChildren);
            copy.RuntimeCreated = true;
            return new CustomLogicMapObjectBuiltin(copy);
        }

        [CLMethod(description: "Destroy a map targetable")]
        public void DestroyMapTargetable(CustomLogicMapTargetableBuiltin targetable)
        {
            Object.Destroy(targetable.GameObject);
            MapLoader.MapTargetables.Remove(targetable.Value);
        }

        [CLMethod(description: "Update the nav mesh")]
        public void UpdateNavMesh()
        {
            MapLoader.UpdateNavMesh().Wait();
        }

        [CLMethod(description: "Update the nav mesh asynchronously")]
        public void UpdateNavMeshAsync()
        {
            _ = MapLoader.UpdateNavMesh();
        }

        protected MapObject CopyMapObject(MapObject obj, int parent, bool recursive)
        {
            var script = new MapScriptSceneObject();
            script.Deserialize(obj.ScriptObject.Serialize());
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = parent;
            script.Networked = false; // we dont support runtime network instantiation for now
            var copy = MapLoader.LoadObject(script, false);
            MapLoader.SetParent(copy);
            CustomLogicManager.Evaluator.LoadMapObjectComponents(copy, true);
            copy.RuntimeCreated = true;
            if (recursive && MapLoader.IdToChildren.ContainsKey(obj.ScriptObject.Id))
            {
                foreach (int child in MapLoader.IdToChildren[obj.ScriptObject.Id])
                {
                    if (MapLoader.IdToMapObject.ContainsKey(child))
                    {
                        CopyMapObject(MapLoader.IdToMapObject[child], script.Id, true);
                    }
                }
            }
            return copy;
        }

        protected void DestroyMapObjectBuiltin(object obj, bool recursive)
        {
            if ((obj is not CustomLogicMapObjectBuiltin) && (obj is not MapObject))
            {
                return;
            }

            MapObject mapObject;
            if (obj is CustomLogicMapObjectBuiltin mapObjectBuiltin)
            {
                mapObject = mapObjectBuiltin.Value;
                mapObjectBuiltin.Value = null;
            }
            else
                mapObject = obj as MapObject;

            var id = mapObject.ScriptObject.Id;
            HashSet<int> children = new HashSet<int>();
            if (MapLoader.IdToChildren.ContainsKey(id))
            {
                foreach (var child in MapLoader.IdToChildren[mapObject.ScriptObject.Id])
                    children.Add(child);
            }
            foreach (var component in mapObject.ComponentInstances)
                CustomLogicManager.Evaluator.RemoveComponent(component);
            MapLoader.DeleteObject(mapObject);
            if (recursive)
            {
                foreach (int child in children)
                {
                    if (MapLoader.IdToMapObject.ContainsKey(child))
                        DestroyMapObjectBuiltin(MapLoader.IdToMapObject[child], true);
                }
            }
        }
    }
}
