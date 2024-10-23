using ApplicationManagers;
using GameManagers;
using Map;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicMapBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicMapBuiltin(): base("Map")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "FindAllMapObjects")
            {
                CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
                foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
                {
                    listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                }
                return listBuiltin;
            }
            if (name == "FindMapObjectByName")
            {
                string objectName = (string)parameters[0];
                foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
                {
                    if (mapObject.ScriptObject.Name == objectName)
                        return new CustomLogicMapObjectBuiltin(mapObject);
                }
                return null;
            }
            if (name == "FindMapObjectsByName")
            {
                CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
                string objectName = (string)parameters[0];
                foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
                {
                    if (mapObject.ScriptObject.Name == objectName)
                        listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                }
                return listBuiltin;
            }
            if (name == "FindMapObjectsByComponent")
            {
                CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
                string className = (string)parameters[0];
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
            if (name == "FindMapObjectByID")
            {
                int id = (int)parameters[0];
                if (MapLoader.IdToMapObject.ContainsKey(id))
                    return new CustomLogicMapObjectBuiltin(MapLoader.IdToMapObject[id]);
                return null;
            }
            if (name == "FindMapObjectByTag")

            {
                string tag = (string)parameters[0];
                if (MapLoader.Tags.ContainsKey(tag))
                {
                    if (MapLoader.Tags[tag].Count > 0)
                        return new CustomLogicMapObjectBuiltin(MapLoader.Tags[tag][0]);
                }
                return null;
            }
            if (name == "FindMapObjectsByTag")
            {
                CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
                string tag = (string)parameters[0];
                if (MapLoader.Tags.ContainsKey(tag))
                {
                    {
                        foreach (MapObject mapObject in MapLoader.Tags[tag])
                            listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
                    }
                }
                return listBuiltin;
            }
            if (name == "CreateMapObjectRaw")
            {
                string prefab = (string)parameters[0];
                prefab = string.Join("", prefab.Split('\n'));
                var script = new MapScriptSceneObject();
                script.Deserialize(prefab);
                script.Id = MapLoader.GetNextObjectId();
                script.Parent = 0;
                var mapObject = MapLoader.LoadObject(script, false);
                MapLoader.SetParent(mapObject);
                CustomLogicManager.Evaluator.LoadMapObjectComponents(mapObject, true);
                return new CustomLogicMapObjectBuiltin(mapObject);
            }
            if (name == "DestroyMapObject")
            {
                var mapObject = (CustomLogicMapObjectBuiltin)parameters[0];
                bool incldueChildren = (bool)parameters[1];
                DestroyMapObject(mapObject, incldueChildren);
                return null;
            }
            if (name == "CopyMapObject")
            {
                var mapObject = (CustomLogicMapObjectBuiltin)parameters[0];
                bool includeChildren = (bool)parameters[1];
                var copy = CopyMapObject(mapObject.Value, mapObject.Value.Parent, includeChildren);
                return new CustomLogicMapObjectBuiltin(copy);
            }
            if (name == "DestroyMapTargetable")
            {
                var targetable = (CustomLogicMapTargetableBuiltin)parameters[0];
                Object.Destroy(targetable.GameObject);
                MapLoader.MapTargetables.Remove(targetable.Value);
                return null;
            }
            if (name == "UpdateNavMesh")
            {
                MapLoader.UpdateNavMesh().Wait();
                return null;
            }
            if (name == "UpdateNavMeshAsync")
            {
                _ = MapLoader.UpdateNavMesh();
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        protected MapObject CopyMapObject(MapObject obj, int parent, bool recursive)
        {
            var script = new MapScriptSceneObject();
            script.Deserialize(obj.ScriptObject.Serialize());
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = parent;
            var copy = MapLoader.LoadObject(script, false);
            MapLoader.SetParent(copy);
            CustomLogicManager.Evaluator.LoadMapObjectComponents(copy, true);
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

        // obj is CustomLogicMapObjectBuiltin or MapObject
        protected void DestroyMapObject(object obj, bool recursive)
        {
            if (obj is not CustomLogicMapObjectBuiltin or MapObject)
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
                children = MapLoader.IdToChildren[mapObject.ScriptObject.Id];
            foreach (var component in mapObject.ComponentInstances)
            {
                CustomLogicManager.Evaluator.RemoveComponent(component);
            }
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

        public override object GetField(string name)
        {
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
