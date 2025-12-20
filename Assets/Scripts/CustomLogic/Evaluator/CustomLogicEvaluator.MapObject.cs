using Map;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    internal partial class CustomLogicEvaluator
    {
        #region Map Object Setup
        public void LoadMapObjectComponents(MapObject obj, bool init = false)
        {
            if (obj.ScriptObject is MapScriptSceneObject)
            {
                var photonView = SetupNetworking(obj);
                var mapObjectBuiltin = SetupMapObject(obj);
                bool rigidbody = LoadRuntimeMapObjectComponents(obj, init);
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, rigidbody);
            }
        }

        public bool LoadRuntimeMapObjectComponents(MapObject obj, bool init = false)
        {
            List<MapScriptComponent> components = ((MapScriptSceneObject)obj.ScriptObject).Components;
            bool rigidbody = false;
            foreach (var component in components)
            {
                if (_start.Classes.ContainsKey(component.ComponentName))
                {
                    CustomLogicComponentInstance instance = CreateComponentInstance(component.ComponentName, obj, component);
                    obj.RegisterComponentInstance(instance);
                    if (init)
                    {
                        EvaluateMethod(instance, "Init");
                        instance.Inited = true;
                    }
                    if (component.ComponentName == "Rigidbody")
                        rigidbody = true;
                }
            }
            return rigidbody;
        }

        public CustomLogicComponentInstance AddMapObjectComponent(MapObject obj, string componentName)
        {
            if (_start.Classes.ContainsKey(componentName))
            {
                var parameters = new List<string>();
                foreach (var assignment in _start.Classes[componentName].Assignments)
                {
                    if (assignment.Left is CustomLogicVariableExpressionAst left)
                    {
                        if (assignment.Right is CustomLogicPrimitiveExpressionAst right)
                        {
                            if (left.Name == "Description")
                                continue;
                            if (left.Name.EndsWith("Tooltip") && right.Value is string)
                                continue;
                            if (left.Name.EndsWith("Dropbox") && right.Value is string)
                                continue;
                        }

                        var str = left.Name + ":" + CustomLogicUtils.SerializeAst(assignment.Right);
                        parameters.Add(str);
                    }
                }

                MapScriptComponent component = new()
                {
                    ComponentName = componentName,
                    Parameters = parameters
                };

                var photonView = SetupNetworking(obj);
                var mapObjectBuiltin = SetupMapObject(obj);
                CustomLogicComponentInstance instance = CreateComponentInstance(componentName, obj, component);
                obj.RegisterComponentInstance(instance);
                EvaluateMethod(instance, "Init");
                instance.Inited = true;
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, componentName == "Rigidbody");
                else if (componentName == "Rigidbody" && IdToNetworkView.TryGetValue(instance.MapObject.Value.ScriptObject.Id, out var networkView))
                {
                    // re-init if a rigidbody is added
                    networkView.Sync.Init(obj.ScriptObject.Id, true);
                }
                return instance;
            }

            throw new Exception("No component named " + componentName + " found");
        }

        public void RemoveComponent(CustomLogicComponentInstance instance)
        {
            RemoveCallbacks(instance);
        }

        public CustomLogicComponentInstance CreateComponentInstance(string className, MapObject obj, MapScriptComponent script)
        {
            CustomLogicNetworkViewBuiltin networkView = null;
            if (obj.ScriptObject.Networked)
                networkView = IdToNetworkView[obj.ScriptObject.Id];
            var classInstance = new CustomLogicComponentInstance(className, IdToMapObjectBuiltin[obj.ScriptObject.Id], script, networkView);
            
            // Set namespace if available
            if (_start.ClassNamespaces.TryGetValue(className, out var classNamespace))
            {
                classInstance.Namespace = classNamespace;
            }
            
            if (networkView != null)
                networkView.RegisterComponentInstance(classInstance);
            RunAssignmentsClassInstance(classInstance);
            classInstance.LoadVariables();
            AddCallbacks(classInstance);
            if (classInstance.UsesCollider())
            {
                HashSet<GameObject> colliders = new HashSet<GameObject>();
                FindSubcolliders(obj.GameObject.transform, colliders);
                foreach (var go in colliders)
                {
                    var collisionHandler = go.GetComponent<CustomLogicCollisionHandler>();
                    if (collisionHandler == null)
                        collisionHandler = go.AddComponent<CustomLogicCollisionHandler>();
                    collisionHandler.RegisterInstance(classInstance);
                }
            }
            return classInstance;
        }

        private void FindSubcolliders(Transform t, HashSet<GameObject> set)
        {
            if (t.GetComponent<Collider>() != null)
                set.Add(t.gameObject);
            foreach (Transform child in t)
            {
                if (MapLoader.GoToMapObject.ContainsKey(child.gameObject))
                    continue;
                FindSubcolliders(child, set);
            }
        }

        public CustomLogicMapObjectBuiltin SetupMapObject(MapObject obj)
        {
            if (!IdToMapObjectBuiltin.ContainsKey(obj.ScriptObject.Id))
            {
                IdToMapObjectBuiltin.Add(obj.ScriptObject.Id, new CustomLogicMapObjectBuiltin(obj));
            }
            return IdToMapObjectBuiltin[obj.ScriptObject.Id];
        }

        public CustomLogicPhotonSync SetupNetworking(MapObject obj)
        {
            if (obj.ScriptObject.Networked)
            {
                if (!IdToNetworkView.ContainsKey(obj.ScriptObject.Id))
                {
                    IdToNetworkView.Add(obj.ScriptObject.Id, new CustomLogicNetworkViewBuiltin(obj));
                    if (PhotonNetwork.IsMasterClient)
                    {
                        object[] data = new object[] { (int)SpawnIntent.PreplacedBind, obj.ScriptObject.Id, false };
                        var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncDynamicPrefab", Vector3.zero, Quaternion.identity, 0, data);
                        var photonView = go.GetComponent<CustomLogicPhotonSync>();
                        return photonView;
                    }
                }
            }
            return null;
        }

        #endregion

    }
}
