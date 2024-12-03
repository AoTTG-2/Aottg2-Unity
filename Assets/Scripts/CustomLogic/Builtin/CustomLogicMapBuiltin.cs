using ApplicationManagers;
using GameManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(InheritBaseMembers = true, Static = true)]
    class CustomLogicMapBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicMapBuiltin(): base("Map")
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

        [CLMethod(description: "Destroy a map object")]
        public void DestroyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren)
        {
            DestroyMapObject(mapObject, includeChildren);
        }

        [CLMethod(description: "Copy a map object")]
        public CustomLogicMapObjectBuiltin CopyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren)
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

        protected void DestroyMapObject(object obj, bool recursive)
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
                        DestroyMapObject(MapLoader.IdToMapObject[child], true);
                }
            }
        }
    }
}
