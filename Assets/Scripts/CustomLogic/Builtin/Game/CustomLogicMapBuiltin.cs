using Map;
using Photon.Pun;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Map", Static = true, Abstract = true, Description = "Finding, creating, and destroying map objects.")]
    partial class CustomLogicMapBuiltin : BuiltinClassInstance
    {
        private static RateLimit _instantiateLimit = null;
        private static int _instantiateCount = 0;
        private static int _allowedInstantiateCount = 100;

        [CLConstructor]
        public CustomLogicMapBuiltin(){
            if (PhotonNetwork.IsMasterClient)
            {
                _instantiateLimit = new RateLimit(40, 1.0f);
                _allowedInstantiateCount = 1000;
            }
            else
            {
                _instantiateLimit = new RateLimit(20, 1.0f);
                _allowedInstantiateCount = 300;
            }
        }

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find all map objects.")]
        public static CustomLogicListBuiltin FindAllMapObjects()
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        [CLMethod(Static = true, Description = "Find a map object by name.")]
        public static CustomLogicMapObjectBuiltin FindMapObjectByName(
            [CLParam("The name of the map object to find.")]
            string objectName)
        {
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    return new CustomLogicMapObjectBuiltin(mapObject);
            }
            return null;
        }

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find all map objects by name.")]
        public static CustomLogicListBuiltin FindMapObjectsByName(
            [CLParam("The name of the map objects to find.")]
            string objectName)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find all map objects by regex pattern.")]
        public static CustomLogicListBuiltin FindMapObjectsByRegex(
            [CLParam("The regex pattern to match against map object names.")]
            string pattern,
            [CLParam("If true, sorts the results by name (default: false).")]
            bool sorted = false)
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

        [CLMethod(Static = true, Description = "Find all map objects by component.")]
        public static CustomLogicMapObjectBuiltin FindMapObjectByComponent(
            [CLParam("The class name of the component to search for.")]
            string className)
        {
            CustomLogicListBuiltin listBuiltin = FindMapObjectsByComponent(className);
            return (CustomLogicMapObjectBuiltin)(listBuiltin.Count > 0 ? listBuiltin.Get(0) : null);
        }

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find all map objects by component.")]
        public static CustomLogicListBuiltin FindMapObjectsByComponent(
            [CLParam("The class name of the component to search for.")]
            string className)
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

        [CLMethod(Static = true, Description = "Find a map object by ID.")]
        public static CustomLogicMapObjectBuiltin FindMapObjectByID(
            [CLParam("The ID of the map object to find.")]
            int id)
        {
            if (MapLoader.IdToMapObject.ContainsKey(id))
                return new CustomLogicMapObjectBuiltin(MapLoader.IdToMapObject[id]);
            return null;
        }

        [CLMethod(Static = true, Description = "Find a map object by tag.")]
        public static CustomLogicMapObjectBuiltin FindMapObjectByTag(
            [CLParam("The tag to search for.")]
            string tag)
        {
            if (MapLoader.Tags.ContainsKey(tag))
            {
                if (MapLoader.Tags[tag].Count > 0)
                    return new CustomLogicMapObjectBuiltin(MapLoader.Tags[tag][0]);
            }
            return null;
        }

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find all map objects by tag.")]
        public static CustomLogicListBuiltin FindMapObjectsByTag(
            [CLParam("The tag to search for.")]
            string tag)
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

        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" }, Description = "Find a map objects of Player.")]
        public static CustomLogicListBuiltin FindMapObjectsByPlayer(
            [CLParam("The player to find map objects for.")]
            CustomLogicPlayerBuiltin player)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (CustomLogicNetworkViewBuiltin nv in CustomLogicManager.Evaluator.IdToNetworkView.Values)
            {
                if (nv.Owner == player)
                {
                    listBuiltin.Add(nv.Sync.CustomLogicMapObjectBuiltin);
                }
            }
            return listBuiltin;
        }

        [CLMethod(Static = true, Description = "Create a new map object.")]
        public static CustomLogicMapObjectBuiltin CreateMapObject(
            [CLParam("The prefab to instantiate.")]
            CustomLogicPrefabBuiltin prefab,
            [CLParam("The position to spawn at (default: null, uses prefab position).")]
            CustomLogicVector3Builtin position = null,
            [CLParam("The rotation to spawn with (default: null, uses prefab rotation).")]
            CustomLogicVector3Builtin rotation = null,
            [CLParam("The scale to spawn with (default: null, uses prefab scale).")]
            CustomLogicVector3Builtin scale = null)
        {
            var scriptSerial = prefab.Value.Serialize();
            var script = new MapScriptSceneObject();
            script.Deserialize(scriptSerial);

            if (position != null)
                script.SetPosition(position);
            if (rotation != null)
                script.SetRotation(rotation);
            if (scale != null)
                script.SetScale(scale);

            if (script.Networked)
            {
                return CreateRuntimeNetworkedMapObject(script, prefab.PersistsOwnership);
            }
            return CreateRuntimeMapObject(script);
        }

        [CLMethod(Static = true, Description = "Create a new map object.")]
        public static CustomLogicMapObjectBuiltin CreateMapObjectRaw(
            [CLParam("The serialized prefab string to instantiate.")]
            string prefab)
        {
            prefab = string.Join("", prefab.Split('\n'));
            var script = new MapScriptSceneObject();
            script.Deserialize(prefab);

            if (script.Networked)
            {
                return CreateRuntimeNetworkedMapObject(script);
            }
            return CreateRuntimeMapObject(script);
        }

        [CLMethod(Static = true, Description = "Create a new prefab object from the current object.")]
        public static CustomLogicPrefabBuiltin PrefabFromMapObject(
            [CLParam("The map object to create a prefab from.")]
            CustomLogicMapObjectBuiltin mapObject,
            [CLParam("If true, clears all components from the prefab (default: false).")]
            bool clearComponents = false)
        {
            string serialized = mapObject.Value.ScriptObject.Serialize();
            return new CustomLogicPrefabBuiltin(serialized, clearComponents);
        }

        [CLMethod(Static = true, Description = "Destroy a map object.")]
        public static void DestroyMapObject(
            [CLParam("The map object to destroy.")]
            CustomLogicMapObjectBuiltin mapObject,
            [CLParam("If true, also destroys all child objects.")]
            bool includeChildren)
        {
            if (mapObject.Value.ScriptObject.Networked)
            {
                if (CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(mapObject.Value.ScriptObject.Id))
                {
                    var networkView = CustomLogicManager.Evaluator.IdToNetworkView[mapObject.Value.ScriptObject.Id];
                    CustomLogicManager.Evaluator.IdToNetworkView.Remove(mapObject.Value.ScriptObject.Id);
                    networkView.Sync.DestroyMe();
                    return;
                }
            }
            DestroyMapObjectBuiltin(mapObject, includeChildren);
        }

        [CLMethod(Static = true, Description = "Copy a map object.")]
        public static CustomLogicMapObjectBuiltin CopyMapObject(
            [CLParam("The map object to copy.")]
            CustomLogicMapObjectBuiltin mapObject,
            [CLParam("If true, also copies all child objects (default: true).")]
            bool includeChildren = true)
        {
            var copy = CopyMapObject(mapObject.Value, mapObject.Value.Parent, includeChildren);
            copy.RuntimeCreated = true;
            return new CustomLogicMapObjectBuiltin(copy);
        }

        [CLMethod(Static = true, Description = "Destroy a map targetable.")]
        public static void DestroyMapTargetable(
            [CLParam("The map targetable to destroy.")]
            CustomLogicMapTargetableBuiltin targetable)
        {
            Object.Destroy(targetable.GameObject);
            MapLoader.MapTargetables.Remove(targetable.Value);
        }

        [CLMethod(Static = true, Description = "Update the nav mesh.")]
        public static void UpdateNavMesh()
        {
            MapLoader.UpdateNavMesh().Wait();
        }

        [CLMethod(Static = true, Description = "Update the nav mesh asynchronously.")]
        public static void UpdateNavMeshAsync()
        {
            _ = MapLoader.UpdateNavMesh();
        }

        protected static CustomLogicMapObjectBuiltin CreateRuntimeNetworkedMapObject(MapScriptSceneObject script, bool persistsOwnership = false)
        {
            TrySpawningRuntimeNetworkedObject();
            script.Id = -1; // -> will be set by the created photonview.
            script.Parent = 0;
            script.Networked = true;
            object[] data = new object[] { (int)SpawnIntent.NetworkedRuntime, };
            // We need to have all this handled by the photonsync instantiation.
            var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncDynamicPrefab", Vector3.zero, Quaternion.identity, 0, data);
            var photonSync = go.GetComponent<CustomLogicPhotonSync>();
            photonSync.InitDynamic(persistsOwnership, script.Serialize());
            return photonSync.CustomLogicMapObjectBuiltin;
        }

        protected static CustomLogicMapObjectBuiltin CreateRuntimeMapObject(MapScriptSceneObject script)
        {
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = 0;
            script.Networked = false;
            var mapObject = MapLoader.LoadObject(script, false);
            MapLoader.SetParent(mapObject);
            CustomLogicManager.Evaluator.LoadMapObjectComponents(mapObject, true);
            mapObject.RuntimeCreated = true;
            return new CustomLogicMapObjectBuiltin(mapObject);
        }

        protected static MapObject CopyMapObject(MapObject obj, int parent, bool recursive)
        {
            var script = new MapScriptSceneObject();
            script.Deserialize(obj.ScriptObject.Serialize());
            script.Id = MapLoader.GetNextObjectId();
            script.Parent = parent;
            script.Networked = false; // we dont support runtime network instantiation for now -> probably ever for copy since its just a different paradigm, just create prefab and spawn.
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

        public static void DestroyMapObjectBuiltin(object obj, bool recursive)
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

            if (mapObject.RuntimeCreated && mapObject.ScriptObject.Networked)
            {
                _instantiateCount--;
            }

            if (CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(id))
            {
                var networkView = CustomLogicManager.Evaluator.IdToNetworkView[id];
                CustomLogicManager.Evaluator.IdToNetworkView.Remove(id);
            }

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


        public static bool HasInstantiateAvailable => _instantiateCount < _allowedInstantiateCount;

        public static bool CanSpawnRuntimeNetworkedMapObject()
        {
            return HasInstantiateAvailable && _instantiateLimit.Peek(1);
        }

        public static void TrySpawningRuntimeNetworkedObject()
        {
            if (HasInstantiateAvailable == false)
            {
                throw new System.Exception("Out of instantiations, please clean up networked objects to spawn more.");
            }
            if (_instantiateLimit.Use(1) == false)
            {
                throw new System.Exception("Spawning networked runtime map objects too fast, please slow down.");
            }
            _instantiateCount++;
        }
    }
}
