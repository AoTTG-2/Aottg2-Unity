using System.Collections.Generic;
using UnityEngine;
using Map;
using Utility;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.AI;

namespace CustomLogic
{
    class CustomLogicMapObjectBuiltin: CustomLogicBaseBuiltin
    {
        public MapObject Value;
        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;

        public CustomLogicMapObjectBuiltin(MapObject obj): base("MapObject")
        {
            Value = obj;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "AddBuiltinComponent")
            {
                string name = (string)parameters[0];
                if (name == "Daylight")
                {
                    var light = Value.GameObject.AddComponent<Light>();
                    light.type = LightType.Directional;
                    light.color = ((CustomLogicColorBuiltin)parameters[1]).Value.ToColor();
                    light.intensity = parameters[2].UnboxToFloat();
                    light.shadows = LightShadows.Soft;
                    light.shadowStrength = 0.8f;
                    light.shadowBias = 0.2f;
                    bool weatherControlled = (bool)parameters[3];
                    if (weatherControlled)
                        MapLoader.Daylight.Add(light);
                }
                else if (name == "PointLight")
                {
                    var light = Value.GameObject.AddComponent<Light>();
                    light.type = LightType.Point;
                    light.color = ((CustomLogicColorBuiltin)parameters[1]).Value.ToColor();
                    light.intensity = parameters[2].UnboxToFloat();
                    light.range = parameters[3].UnboxToFloat();
                    light.shadows = LightShadows.None;
                    light.renderMode = LightRenderMode.ForcePixel;
                    light.bounceIntensity = 0f;
                    MapLoader.RegisterMapLight(light);
                }
                else if (name == "Tag")
                {
                    var tag = (string)parameters[1];
                    MapLoader.RegisterTag(tag, Value);
                }
                else if (name == "Rigidbody")
                {
                    float mass = parameters[1].UnboxToFloat();
                    Vector3 gravity = ((CustomLogicVector3Builtin)parameters[2]).Value;
                    var rigidbody = Value.GameObject.AddComponent<Rigidbody>();
                    rigidbody.mass = mass;
                    var force = Value.GameObject.AddComponent<ConstantForce>();
                    force.force = gravity;
                    rigidbody.useGravity = false;
                    rigidbody.freezeRotation = (bool)parameters[3];

                    var interpolate = (bool)parameters[4];
                    rigidbody.interpolation = interpolate
                        ? RigidbodyInterpolation.Interpolate 
                        : RigidbodyInterpolation.None;
                }
                else if (name == "CustomPhysicsMaterial")
                {
                    var customPhysicsMaterial = Value.GameObject.AddComponent<CustomPhysicsMaterial>();
                    customPhysicsMaterial.Setup((bool)parameters[1]);
                }
                else if (name == "NavMeshObstacle")
                {
                    // Add a navmesh obstacle and size to the objects bounds
                    bool carveOnlyStationary = (bool)parameters[1];
                    var navMeshObstacleGo = new GameObject("NavMeshObstacle");
                    navMeshObstacleGo.transform.parent = Value.GameObject.transform;

                    var navMeshObstacle = navMeshObstacleGo.AddComponent<NavMeshObstacle>();
                    navMeshObstacle.carving = true;
                    navMeshObstacle.carveOnlyStationary = carveOnlyStationary;

                    // Value.ColliderCache contains the colliders of the object, merge all colliders to find the bounds
                    Bounds bounds = Value.colliderCache[0].bounds;
                    foreach (var collider in Value.colliderCache)
                    {
                        bounds.Encapsulate(collider.bounds);
                    }

                    // Set size and center based on bounds
                    navMeshObstacle.size = bounds.size;
                    navMeshObstacle.center = bounds.center;
                }
                return null;
            }
            if (methodName == "UpdateBuiltinComponent")
            {
                string name = (string)parameters[0];
                string param = (string)parameters[1];
                if (name == "Rigidbody")
                {
                    var rigidbody = Value.GameObject.GetComponent<Rigidbody>();
                    if (param == "SetVelocity")
                    {
                        Vector3 velocity = ((CustomLogicVector3Builtin)parameters[2]).Value;
                        rigidbody.velocity = velocity;
                    }
                    else if (param == "AddForce")
                    {
                        Vector3 force = ((CustomLogicVector3Builtin)parameters[2]).Value;
                        ForceMode mode = ForceMode.Acceleration;
                        if (parameters.Count >= 4)
                        {
                            string forceMode = (string)parameters[3];
                            switch (forceMode)
                            {
                                case "Force":
                                    mode = ForceMode.Force;
                                    break;
                                case "Acceleration":
                                    mode = ForceMode.Acceleration;
                                    break;
                                case "Impulse":
                                    mode = ForceMode.Impulse;
                                    break;
                                case "VelocityChange":
                                    mode = ForceMode.VelocityChange;
                                    break;
                            }
                        }
                        if (parameters.Count >= 5)
                        {
                            Vector3 position = ((CustomLogicVector3Builtin)parameters[4]).Value;
                            rigidbody.AddForceAtPosition(force, position, mode);
                        }
                        else
                        {
                            rigidbody.AddForce(force, mode);
                        }
                    }
                    else if (param == "AddTorque")
                    {
                        Vector3 force = ((CustomLogicVector3Builtin)parameters[2]).Value;
                        ForceMode mode = ForceMode.Acceleration;
                        if (parameters.Count >= 4)
                        {
                            string forceMode = (string)parameters[3];
                            switch (forceMode)
                            {
                                case "Force":
                                    mode = ForceMode.Force;
                                    break;
                                case "Acceleration":
                                    mode = ForceMode.Acceleration;
                                    break;
                                case "Impulse":
                                    mode = ForceMode.Impulse;
                                    break;
                                case "VelocityChange":
                                    mode = ForceMode.VelocityChange;
                                    break;
                            }
                        }
                        rigidbody.AddTorque(force, mode);
                    }
                }
                else if (name == "CustomPhysicsMaterial")
                {
                    var customPhysicsMaterial = Value.GameObject.GetComponent<CustomPhysicsMaterial>();
                    if (param == "StaticFriction")
                    {
                        customPhysicsMaterial.StaticFriction = parameters[2].UnboxToFloat();
                    }
                    if (param == "DynamicFriction")
                    {
                        customPhysicsMaterial.DynamicFriction = parameters[2].UnboxToFloat();
                    }
                    if (param == "Bounciness")
                    {
                        customPhysicsMaterial.Bounciness = parameters[2].UnboxToFloat();
                    }

                    var isFrictionCombine = param == "FrictionCombine";
                    var isBounceCombine = param == "BounceCombine";
                    if (isFrictionCombine || isBounceCombine)
                    {
                        var combine = parameters[2] switch
                        {
                            "Minimum" => PhysicMaterialCombine.Minimum, 
                            "Multiply" => PhysicMaterialCombine.Multiply,
                            "Maximum" => PhysicMaterialCombine.Maximum, 
                            _ => PhysicMaterialCombine.Average
                        };

                        if (isFrictionCombine)
                            customPhysicsMaterial.FrictionCombine = combine;
                        else
                            customPhysicsMaterial.BounceCombine = combine;

                    }
                }
                return null;
            }
            if (methodName == "ReadBuiltinComponent")
            {
                string name = (string)parameters[0];
                string param = (string)parameters[1];
                if (name == "Rigidbody")
                {
                    var rigidbody = Value.GameObject.GetComponent<Rigidbody>();
                    if (param == "Velocity")
                    {
                        return new CustomLogicVector3Builtin(rigidbody.velocity);
                    }
                    else if (param == "AngularVelocity")
                    {
                        return new CustomLogicVector3Builtin(rigidbody.angularVelocity);
                    }
                }
                return null;
            }
            if (methodName == "AddSphereCollider")
            {
                string collideMode = (string)parameters[0];
                string collideWith = (string)parameters[1];
                Vector3 center = ((CustomLogicVector3Builtin)parameters[2]).Value;
                float radius = (float)parameters[3];
                Vector3 scale = Value.BaseScale;
                center = Util.DivideVectors(center, scale);
                radius = radius / scale.MaxComponent();
                var go = new GameObject();
                go.transform.SetParent(Value.GameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                SphereCollider c = go.AddComponent<SphereCollider>();
                MapLoader.SetCollider(c, collideMode, collideWith);
                c.radius = radius;
                c.center = center;
                var handler = go.AddComponent<CustomLogicCollisionHandler>();
                foreach (var instance in Value.ComponentInstances)
                {
                    if (instance.UsesCollider())
                        handler.RegisterInstance(instance);
                }
                return null;
            }
            if (methodName == "AddBoxCollider")
            {
                string collideMode = (string)parameters[0];
                string collideWith = (string)parameters[1];
                Vector3 center = ((CustomLogicVector3Builtin)parameters[2]).Value;
                Vector3 size = ((CustomLogicVector3Builtin)parameters[3]).Value;
                Vector3 scale = Value.BaseScale;
                center = Util.DivideVectors(center, scale);
                size = Util.DivideVectors(size, scale);
                var go = new GameObject();
                go.transform.SetParent(Value.GameObject.transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                BoxCollider c = go.AddComponent<BoxCollider>();
                MapLoader.SetCollider(c, collideMode, collideWith);
                c.size = size;
                c.center = center;
                var handler = go.AddComponent<CustomLogicCollisionHandler>();
                foreach (var instance in Value.ComponentInstances)
                {
                    if (instance.UsesCollider())
                        handler.RegisterInstance(instance);
                }
                return null;
            }
            if (methodName == "GetComponent")
            {
                string name = (string)parameters[0];
                return Value.FindComponentInstance(name);
            }
            if (methodName == "GetChild")
            {
                string name = (string)parameters[0];
                if (MapLoader.IdToChildren.ContainsKey(Value.ScriptObject.Id))
                {
                    foreach (int childId in MapLoader.IdToChildren[Value.ScriptObject.Id])
                    {
                        if (MapLoader.IdToMapObject.ContainsKey(childId))
                        {
                            var go = MapLoader.IdToMapObject[childId];
                            if (go.ScriptObject.Name == name)
                                return new CustomLogicMapObjectBuiltin(go);
                        }
                    }
                }
                return null;
            }
            if (methodName == "GetChildren")
            {
                CustomLogicListBuiltin listBuiltin = new CustomLogicListBuiltin();
                if (MapLoader.IdToChildren.ContainsKey(Value.ScriptObject.Id))
                {
                    foreach (int childId in MapLoader.IdToChildren[Value.ScriptObject.Id])
                    {
                        if (MapLoader.IdToMapObject.ContainsKey(childId))
                        {
                            var go = MapLoader.IdToMapObject[childId];
                            listBuiltin.List.Add(new CustomLogicMapObjectBuiltin(go));
                        }
                    }
                }
                return listBuiltin;
            }
            if (methodName == "GetTransform")
            {
                string name = (string)parameters[0];
                Transform transform = Value.GameObject.transform.Find(name);
                if (transform != null)
                {
                    return new CustomLogicTransformBuiltin(transform);
                }
                return null;
            }
            if (methodName == "SetColorAll")
            {
                if (Value.ScriptObject.Static)
                {
                    throw new System.Exception(methodName + " cannot be called on a static MapObject.");
                }

                var color = ((CustomLogicColorBuiltin)parameters[0]).Value.ToColor();
                foreach (Renderer r in Value.renderCache)
                {
                    r.material.color = color;
                }
                return null;
            }
            if (methodName == "InBounds")
            {
                // Iterate over colliders and check if the param position is within the bounds
                Vector3 position = ((CustomLogicVector3Builtin)parameters[0]).Value;
                
                // Check all colliders on the object and on its children
                foreach (var collider in Value.colliderCache)
                {
                    if (collider.bounds.Contains(position))
                    {
                        return true;
                    }
                }

                return false;
            }
            if (methodName == "GetBoundsAverageCenter")
            {
                // Check all colliders on the object and on its children
                Vector3 center = Vector3.zero;
                int count = 0;
                foreach (var collider in Value.colliderCache)
                {
                    center += collider.bounds.center;
                    count++;
                }

                if (count > 0)
                {
                    return new CustomLogicVector3Builtin(center / count);
                }
                return new CustomLogicVector3Builtin(Value.GameObject.transform.position);
            }
            if (methodName == "GetBoundsCenter")
            {
                if (Value.colliderCache.Length == 0)
                {
                    return null;
                }
                return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.center);
            }
            if (methodName == "GetBoundsMin")
            {
                if (Value.colliderCache.Length == 0)
                {
                    return null;
                }
                return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.min);
            }
            if (methodName == "GetBoundsMax")
            {
                if (Value.colliderCache.Length == 0)
                {
                    return null;
                }
                return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.max);
            }
            if (methodName == "GetCorners")
            {
                if (Value.colliderCache.Length == 0)
                {
                    return null;
                }
                var firstCollider = Value.colliderCache[0];
                if (firstCollider is BoxCollider == false)
                {
                    // Return the bounds corners
                    var bounds = firstCollider.bounds;
                    var corners = new Vector3[8];
                    corners[0] = bounds.min;
                    corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
                    corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
                    corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
                    corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
                    corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
                    corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
                    corners[7] = bounds.max;
                    var clCorners = new CustomLogicListBuiltin();
                    clCorners.List.AddRange(corners.Select(v => new CustomLogicVector3Builtin(v)));
                    return clCorners;
                }
                BoxCollider boxCollider = (BoxCollider)firstCollider;
                Vector3 size = boxCollider.size;
                var result = new CustomLogicListBuiltin();
                List<Vector3> list = new();
                var signs = new List<int> { -1, 1 };
                signs.ForEach(signX =>
                    signs.ForEach(signY =>
                        signs.ForEach(signZ => {
                            var vector = new Vector3(size.x * signX, size.y * signY, size.z * signZ);
                            result.List.Add(new CustomLogicVector3Builtin(boxCollider.transform.TransformPoint(boxCollider.center + vector * 0.5f)));
                        })));                
                return result;
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Static")
                return Value.ScriptObject.Static;
            if (name == "Position")
                return new CustomLogicVector3Builtin(Value.GameObject.transform.position);
            if (name == "LocalPosition")
                return new CustomLogicVector3Builtin(Value.GameObject.transform.localPosition);
            if (name == "Rotation")
            {
                if (_needSetRotation)
                {
                    _internalRotation = Value.GameObject.transform.rotation.eulerAngles;
                    _needSetRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalRotation);
            }
            if (name == "LocalRotation")
            {
                if (_needSetLocalRotation)
                {
                    _internalLocalRotation = Value.GameObject.transform.localRotation.eulerAngles;
                    _needSetLocalRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalLocalRotation);
            }
            if (name == "Forward")
                return new CustomLogicVector3Builtin(Value.GameObject.transform.forward.normalized);
            if (name == "Up")
                return new CustomLogicVector3Builtin(Value.GameObject.transform.up.normalized);
            if (name == "Right")
                return new CustomLogicVector3Builtin(Value.GameObject.transform.right.normalized);
            if (name == "Scale")
            {
                var localScale = Value.GameObject.transform.localScale;
                var baseScale = Value.BaseScale;
                return new CustomLogicVector3Builtin(new Vector3(localScale.x / baseScale.x, localScale.y / baseScale.y, localScale.z / baseScale.z));
            }
            if (name == "Name")
                return Value.ScriptObject.Name;
            if (name == "Parent")
            {
                int parentId = Value.Parent;
                if (parentId <= 0)
                    return null;
                return new CustomLogicMapObjectBuiltin(MapLoader.IdToMapObject[parentId]);
            }
            if (name == "Active")
            {
                return Value.GameObject.activeSelf;
            }
            if (name == "Transform")
            {
                return new CustomLogicTransformBuiltin(Value.GameObject.transform);
            }
            else if (name == "HasRenderer")
            {
                return Value.renderCache.Length > 0;
            }
            if (name == "Color")
            {
                if (Value.renderCache.Length == 0)
                {
                    throw new System.Exception("MapObject has no renderer.");
                }

                var color = Value.renderCache[0].material.color;
                return new CustomLogicColorBuiltin(new Color255(color));
            }
            if (name == "ID")
            {
                return Value.ScriptObject.Id;
            }
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "Position")
                Value.GameObject.transform.position = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "LocalPosition")
                Value.GameObject.transform.localPosition = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Rotation")
            {
                _internalRotation = ((CustomLogicVector3Builtin)value).Value;
                _needSetRotation = false;
                Value.GameObject.transform.rotation = Quaternion.Euler(_internalRotation);
            }
            else if (name == "LocalRotation")
            {
                _internalLocalRotation = ((CustomLogicVector3Builtin)value).Value;
                _needSetLocalRotation = false;
                Value.GameObject.transform.localRotation = Quaternion.Euler(_internalLocalRotation);
            }
            else if (name == "Forward")
                Value.GameObject.transform.forward = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Up")
                Value.GameObject.transform.up = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Right")
                Value.GameObject.transform.right = ((CustomLogicVector3Builtin)value).Value;
            else if (name == "Scale")
            {
                var localScale = ((CustomLogicVector3Builtin)value).Value;
                var baseScale = Value.BaseScale;
                Value.GameObject.transform.localScale = new Vector3(localScale.x * baseScale.x, localScale.y * baseScale.y, localScale.z * baseScale.z);
            }
            else if (name == "Parent")
            {
                if (value == null)
                {
                    MapLoader.SetParent(Value, null);
                }
                else
                {
                    var parent = (CustomLogicMapObjectBuiltin)value;
                    MapLoader.SetParent(Value, parent.Value);
                }
                _needSetLocalRotation = true;
            }
            else if (name == "Active")
            {
                Value.GameObject.SetActive((bool)value);
            }
            else if (name == "Color")
            {
                if (Value.ScriptObject.Static)
                {
                    throw new System.Exception(name + " cannot be set on a static MapObject.");
                }

                if (Value.renderCache.Length == 0)
                {
                    throw new System.Exception("MapObject has no renderer.");
                }

                var color = ((CustomLogicColorBuiltin)value).Value.ToColor();
                Value.renderCache[0].material.color = color;
            }
            else
            {
                base.SetField(name, value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return Value == null;
            if (!(obj is CustomLogicMapObjectBuiltin))
                return false;
            var other = ((CustomLogicMapObjectBuiltin)obj).Value;
            return Value == other;
        }

        public override string ToString()
        {
            return $"{Value.GameObject.name} (MapObject)";
        }
    }
}
