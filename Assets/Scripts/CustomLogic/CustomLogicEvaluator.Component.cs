using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Map;
using Photon.Pun;
using UnityEngine;

namespace CustomLogic
{
    partial class CustomLogicEvaluator
    {
        public async UniTaskVoid LoadMapObjectComponents(MapObject obj, bool init = false)
        {
            if (obj.ScriptObject is MapScriptSceneObject sceneObject)
            {
                var photonView = SetupNetworking(obj);
                var components = sceneObject.Components;
                var rigidbody = false;
                foreach (var component in components)
                {
                    if (_start.Classes.ContainsKey(component.ComponentName))
                    {
                        var instance = CreateComponentInstance(component.ComponentName, obj, component);
                        obj.RegisterComponentInstance(instance);

                        if (init) await EvaluateMethod(instance, "Init");

                        if (component.ComponentName == "Rigidbody")
                            rigidbody = true;
                    }
                }

                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, rigidbody);
            }
        }

        public CustomLogicComponentInstance AddMapObjectComponent(MapObject obj, string componentName)
        {
            if (_start.Classes.TryGetValue(componentName, out var @class))
            {
                var parameters = new List<string>();
                foreach (var assignment in @class.Assignments)
                {
                    if (assignment.Left is CustomLogicVariableExpressionAst left)
                    {
                        if (assignment.Right is CustomLogicPrimitiveExpressionAst right)
                        {
                            var skip = left.Name == "Description"
                                || (left.Name.EndsWith("Tooltip") && right.Value is string)
                                || (left.Name.EndsWith("Dropbox") && right.Value is string);
                            
                            if (skip) continue;
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
                var instance = CreateComponentInstance(componentName, obj, component);
                obj.RegisterComponentInstance(instance);
                EvaluateMethod(instance, "Init").Forget();
                if (photonView != null)
                    photonView.Init(obj.ScriptObject.Id, componentName == "Rigidbody");
                else if (componentName == "Rigidbody" &&
                         IdToNetworkView.TryGetValue(instance.MapObject.Value.ScriptObject.Id, out var networkView))
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
        
        private CustomLogicComponentInstance CreateComponentInstance(string className, MapObject obj,
            MapScriptComponent script)
        {
            CustomLogicNetworkViewBuiltin networkView = null;
            if (obj.ScriptObject.Networked)
                networkView = IdToNetworkView[obj.ScriptObject.Id];
            
            var classInstance = new CustomLogicComponentInstance(className, obj, script, networkView);
            networkView?.RegisterComponentInstance(classInstance);
            RunAssignmentsClassInstance(classInstance);
            classInstance.LoadVariables();
            AddCallbacks(classInstance);
            if (classInstance.UsesCollider())
            {
                var children = new HashSet<GameObject>
                {
                    obj.GameObject 
                };
                
                foreach (var collider in obj.GameObject.GetComponentsInChildren<Collider>())
                {
                    children.Add(collider.gameObject);
                }

                foreach (var go in children)
                {
                    var collisionHandler = go.GetComponent<CustomLogicCollisionHandler>();
                    if (collisionHandler == null)
                        collisionHandler = go.AddComponent<CustomLogicCollisionHandler>();
                    collisionHandler.RegisterInstance(classInstance);
                }
            }

            return classInstance;
        }

        private CustomLogicPhotonSync SetupNetworking(MapObject obj)
        {
            if (obj.ScriptObject.Networked)
            {
                if (!IdToNetworkView.ContainsKey(obj.ScriptObject.Id))
                {
                    IdToNetworkView.Add(obj.ScriptObject.Id, new CustomLogicNetworkViewBuiltin(obj));
                    if (PhotonNetwork.IsMasterClient)
                    {
                        var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncPrefab", Vector3.zero,
                            Quaternion.identity);
                        var photonView = go.GetComponent<CustomLogicPhotonSync>();
                        return photonView;
                    }
                }
            }

            return null;
        }
        
        public List<string> GetComponentNames()
        {
            return _start.Classes.Keys.Where(className =>
                (int)_start.Classes[className].Token.Value == (int)CustomLogicSymbol.Component).ToList();
        }
    }
}