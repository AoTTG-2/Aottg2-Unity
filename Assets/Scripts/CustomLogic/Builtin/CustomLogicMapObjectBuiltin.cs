using System.Collections.Generic;
using UnityEngine;
using Map;
using Utility;
using System.ComponentModel;

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
                        rigidbody.AddForce(force, ForceMode.Acceleration);
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
            if (name == "Color")
            {
                var color = Value.GameObject.GetComponent<Renderer>().material.color;
                return new CustomLogicColorBuiltin(new Color255(color));
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
                var color = ((CustomLogicColorBuiltin)value).Value.ToColor();
                // Set my renderer's color
                Value.GameObject.GetComponent<Renderer>().material.color = color;
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
    }
}
