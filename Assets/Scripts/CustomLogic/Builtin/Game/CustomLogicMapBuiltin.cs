using Map;
using Photon.Pun;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Finding, creating, and destroying map objects.
    /// </summary>
    [CLType(Name = "Map", Static = true, Abstract = true)]
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

        /// <summary>
        /// Find all map objects.
        /// </summary>
        /// <returns>A list of all map objects.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindAllMapObjects()
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                listBuiltin.List.Add(CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        /// <summary>
        /// Find a map object by name.
        /// </summary>
        /// <param name="objectName">The name of the map object to find.</param>
        /// <returns>The map object if found, null otherwise.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin FindMapObjectByName(string objectName)
        {
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject);
            }
            return null;
        }

        /// <summary>
        /// Find all map objects by name.
        /// </summary>
        /// <param name="objectName">The name of the map objects to find.</param>
        /// <returns>A list of map objects with the specified name.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindMapObjectsByName(string objectName)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (mapObject.ScriptObject.Name == objectName)
                    listBuiltin.List.Add(CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject));
            }
            return listBuiltin;
        }

        /// <summary>
        /// Find all map objects by regex pattern.
        /// </summary>
        /// <param name="pattern">The regex pattern to match against map object names.</param>
        /// <param name="sorted">If true, sorts the results by name (default: false).</param>
        /// <returns>A list of map objects matching the regex pattern.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindMapObjectsByRegex(string pattern, bool sorted = false)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                if (regex.IsMatch(mapObject.ScriptObject.Name))
                {
                    listBuiltin.List.Add(CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject));
                }
            }

            if (sorted)
            {
                listBuiltin.List.Sort((a, b) => string.Compare(((CustomLogicMapObjectBuiltin)a).Value.ScriptObject.Name, ((CustomLogicMapObjectBuiltin)b).Value.ScriptObject.Name));
            }

            return listBuiltin;
        }

        /// <summary>
        /// Find all map objects by component.
        /// </summary>
        /// <param name="className">The class name of the component to search for.</param>
        /// <returns>The first map object with the specified component, or null if not found.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin FindMapObjectByComponent(string className)
        {
            CustomLogicListBuiltin listBuiltin = FindMapObjectsByComponent(className);
            return (CustomLogicMapObjectBuiltin)(listBuiltin.Count > 0 ? listBuiltin.Get(0) : null);
        }

        /// <summary>
        /// Find all map objects by component.
        /// </summary>
        /// <param name="className">The class name of the component to search for.</param>
        /// <returns>A list of map objects with the specified component.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindMapObjectsByComponent(string className)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            foreach (MapObject mapObject in MapLoader.GoToMapObject.Values)
            {
                foreach (CustomLogicComponentInstance component in mapObject.ComponentInstances)
                {
                    if (component.ClassName == className)
                    {
                        listBuiltin.List.Add(CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject));
                        break;
                    }
                }
            }
            return listBuiltin;
        }

        /// <summary>
        /// Find a map object by ID.
        /// </summary>
        /// <param name="id">The ID of the map object to find.</param>
        /// <returns>The map object if found, null otherwise.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin FindMapObjectByID(int id)
        {
            if (MapLoader.IdToMapObject.ContainsKey(id))
                return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(MapLoader.IdToMapObject[id]);
            return null;
        }

        /// <summary>
        /// Find a map object by tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>The first map object with the specified tag, or null if not found.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin FindMapObjectByTag(string tag)
        {
            if (MapLoader.Tags.ContainsKey(tag))
            {
                if (MapLoader.Tags[tag].Count > 0)
                    return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(MapLoader.Tags[tag][0]);
            }
            return null;
        }

        /// <summary>
        /// Find all map objects by tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A list of map objects with the specified tag.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindMapObjectsByTag(string tag)
        {
            CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
            if (MapLoader.Tags.ContainsKey(tag))
            {
                {
                    foreach (MapObject mapObject in MapLoader.Tags[tag])
                        listBuiltin.List.Add(CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject));
                }
            }
            return listBuiltin;
        }

        /// <summary>
        /// Find a map objects of Player.
        /// </summary>
        /// <param name="player">The player to find map objects for.</param>
        /// <returns>A list of map objects owned by the player.</returns>
        [CLMethod(Static = true, ReturnTypeArguments = new[] { "MapObject" })]
        public static CustomLogicListBuiltin FindMapObjectsByPlayer(CustomLogicPlayerBuiltin player)
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

        /// <summary>
        /// Create a new map object.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="position">The position to spawn at (default: null, uses prefab position).</param>
        /// <param name="rotation">The rotation to spawn with (default: null, uses prefab rotation).</param>
        /// <param name="scale">The scale to spawn with (default: null, uses prefab scale).</param>
        /// <returns>The created map object.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin CreateMapObject(
            CustomLogicPrefabBuiltin prefab,
            CustomLogicVector3Builtin position = null,
            CustomLogicVector3Builtin rotation = null,
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

        /// <summary>
        /// Create a new map object.
        /// </summary>
        /// <param name="prefab">The serialized prefab string to instantiate.</param>
        /// <returns>The created map object.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin CreateMapObjectRaw(string prefab)
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

        /// <summary>
        /// Create a new prefab object from the current object.
        /// </summary>
        /// <param name="mapObject">The map object to create a prefab from.</param>
        /// <param name="clearComponents">If true, clears all components from the prefab (default: false).</param>
        /// <returns>The created prefab.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicPrefabBuiltin PrefabFromMapObject(CustomLogicMapObjectBuiltin mapObject, bool clearComponents = false)
        {
            string serialized = mapObject.Value.ScriptObject.Serialize();
            return new CustomLogicPrefabBuiltin(serialized, clearComponents);
        }

        /// <summary>
        /// Destroy a map object.
        /// </summary>
        /// <param name="mapObject">The map object to destroy.</param>
        /// <param name="includeChildren">If true, also destroys all child objects.</param>
        [CLMethod(Static = true)]
        public static void DestroyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren)
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

        /// <summary>
        /// Copy a map object.
        /// </summary>
        /// <param name="mapObject">The map object to copy.</param>
        /// <param name="includeChildren">If true, also copies all child objects (default: true).</param>
        /// <returns>The copied map object.</returns>
        [CLMethod(Static = true)]
        public static CustomLogicMapObjectBuiltin CopyMapObject(CustomLogicMapObjectBuiltin mapObject, bool includeChildren = true)
        {
            var copy = CopyMapObject(mapObject.Value, mapObject.Value.Parent, includeChildren);
            copy.RuntimeCreated = true;
            return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(copy);
        }

        /// <summary>
        /// Destroy a map targetable.
        /// </summary>
        /// <param name="targetable">The map targetable to destroy.</param>
        [CLMethod(Static = true)]
        public static void DestroyMapTargetable(CustomLogicMapTargetableBuiltin targetable)
        {
            Object.Destroy(targetable.GameObject);
            MapLoader.MapTargetables.Remove(targetable.Value);
        }

        /// <summary>
        /// Update the nav mesh.
        /// </summary>
        [CLMethod(Static = true)]
        public static void UpdateNavMesh()
        {
            MapLoader.UpdateNavMesh().Wait();
        }

        /// <summary>
        /// Update the nav mesh asynchronously.
        /// </summary>
        [CLMethod(Static = true)]
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
            return CustomLogicManager.Evaluator.GetOrCreateMapObjectBuiltin(mapObject);
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
