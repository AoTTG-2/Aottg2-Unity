using System.Collections.Generic;
using UnityEngine;
using Map;
using Utility;
using System.ComponentModel;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.AI;
using CustomLogic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System;

namespace CustomLogic
{
    /// <summary>
    /// MapObject represents a map object created in the editor or spawned at runtime using Map static methods.
    /// </summary>
    [CLType(Name = "MapObject", Abstract = true)]
    partial class CustomLogicMapObjectBuiltin : BuiltinClassInstance
    {
        public MapObject Value;
        private CustomLogicRigidbodyBuiltin _rigidBody;
        private Vector3 _internalRotation;
        private Vector3 _internalLocalRotation;
        private bool _needSetRotation = true;
        private bool _needSetLocalRotation = true;
        private Dictionary<string, BuiltinComponentInstance> _builtinCache;

        public CustomLogicMapObjectBuiltin(MapObject obj)
        {
            Value = obj;
        }

        [CLProperty(description: "Object does not move")]
        public bool Static => Value.ScriptObject.Static;

        [CLProperty(description: "The position of the object")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.position);
            set => Value.GameObject.transform.position = value.Value;
        }
        
        [CLProperty(description: "The local position of the object")]
        public CustomLogicVector3Builtin LocalPosition
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.localPosition);
            set => Value.GameObject.transform.localPosition = value.Value;
        }

        [CLProperty(description: "The rotation of the object")]
        public CustomLogicVector3Builtin Rotation
        {
            get
            {
                if (_needSetRotation)
                {
                    _internalRotation = Value.GameObject.transform.rotation.eulerAngles;
                    _needSetRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalRotation);
            }
            set
            {
                _internalRotation = value.Value;
                _needSetRotation = false;
                Value.GameObject.transform.rotation = Quaternion.Euler(_internalRotation);
            }
        }

        [CLProperty(description: "The local rotation of the object")]
        public CustomLogicVector3Builtin LocalRotation
        {
            get
            {
                if (_needSetLocalRotation)
                {
                    _internalLocalRotation = Value.GameObject.transform.localRotation.eulerAngles;
                    _needSetLocalRotation = false;
                }
                return new CustomLogicVector3Builtin(_internalLocalRotation);
            }
            set
            {
                _internalLocalRotation = value.Value;
                _needSetLocalRotation = false;
                Value.GameObject.transform.localRotation = Quaternion.Euler(_internalLocalRotation);
            }
        }

        [CLProperty(description: "The rotation of the object as a quaternion")]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.GameObject.transform.rotation);
            set => Value.GameObject.transform.rotation = value.Value;
        }

        [CLProperty(description: "The local rotation of the object as a quaternion")]
        public CustomLogicQuaternionBuiltin QuaternionLocalRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.GameObject.transform.localRotation);
            set => Value.GameObject.transform.localRotation = value.Value;
        }

        [CLProperty(description: "The forward direction of the object")]
        public CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.forward.normalized);
            set => Value.GameObject.transform.forward = value.Value;
        }

        [CLProperty(description: "The up direction of the object")]
        public CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.up.normalized);
            set => Value.GameObject.transform.up = value.Value;
        }

        [CLProperty(description: "The right direction of the object")]
        public CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.right.normalized);
            set => Value.GameObject.transform.right = value.Value;
        }

        [CLProperty(description: "The scale of the object")]
        public CustomLogicVector3Builtin Scale
        {
            get
            {
                var localScale = Value.GameObject.transform.localScale;
                var baseScale = Value.BaseScale;
                return new CustomLogicVector3Builtin(new Vector3(localScale.x / baseScale.x, localScale.y / baseScale.y, localScale.z / baseScale.z));
            }
            set
            {
                var localScale = value.Value;
                var baseScale = Value.BaseScale;
                Value.GameObject.transform.localScale = new Vector3(localScale.x * baseScale.x, localScale.y * baseScale.y, localScale.z * baseScale.z);
            }
        }

        [CLProperty(description: "The name of the object")]
        public string Name => Value.ScriptObject.Name;

        [CLProperty(description: "The parent of the object")]
        public object Parent
        {
            get
            {
                int parentId = Value.Parent;
                if (parentId <= 0)
                    return null;
                return new CustomLogicMapObjectBuiltin(MapLoader.IdToMapObject[parentId]);
            }
            set
            {
                if (value == null)
                {
                    MapLoader.SetParent(Value, null);
                }
                else if (value is CustomLogicMapObjectBuiltin)
                {
                    var parent = (CustomLogicMapObjectBuiltin)value;
                    MapLoader.SetParent(Value, parent.Value);
                }
                else if (value is CustomLogicTransformBuiltin)
                {
                    MapLoader.SetParent(Value, null);
                    var parent = (CustomLogicTransformBuiltin)value;
                    Value.GameObject.transform.SetParent(parent.Value);
                }
                _needSetLocalRotation = true;
            }
        }

        [CLProperty(description: "Whether the object is active")]
        public bool Active
        {
            get => Value.GameObject.activeSelf;
            set => Value.GameObject.SetActive(value);
        }

        [CLProperty(description: "The transform of the object")]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(Value.GameObject.transform);

        [CLProperty(description: "Whether the object has a renderer")]
        public bool HasRenderer => Value.renderCache.Length > 0;

        [CLProperty(description: "The color of the object")]
        public CustomLogicColorBuiltin Color
        {
            get
            {
                AssertRendererGet();
                var color = Value.renderCache[0].material.color;
                return new CustomLogicColorBuiltin(new Color255(color));
            }
            set
            {
                AssertRendererSet();
                var color = value.Value.ToColor();
                Value.renderCache[0].material.color = color;
            }
        }

        [CLProperty(description: "The x tiling of the object's texture")]
        public float TextureTilingX
        {
            get
            {
                AssertRendererGet();
                return Value.renderCache[0].material.mainTextureScale.x;
            }
            set
            {
                AssertRendererSet();
                var tiling = Value.renderCache[0].material.mainTextureScale;
                Value.renderCache[0].material.mainTextureScale = new Vector2(value, tiling.y);
            }
        }

        [CLProperty(description: "The y tiling of the object's texture")]
        public float TextureTilingY
        {
            get
            {
                AssertRendererGet();
                return Value.renderCache[0].material.mainTextureScale.y;
            }
            set
            {
                AssertRendererSet();
                var tiling = Value.renderCache[0].material.mainTextureScale;
                Value.renderCache[0].material.mainTextureScale = new Vector2(tiling.x, value);
            }
        }

        [CLProperty(description: "The x offset of the object's texture")]
        public float TextureOffsetX
        {
            get
            {
                AssertRendererGet();
                return Value.renderCache[0].material.mainTextureOffset.x;
            }
            set
            {
                AssertRendererSet();
                var offset = Value.renderCache[0].material.mainTextureOffset;
                Value.renderCache[0].material.mainTextureOffset = new Vector2(value, offset.y);
            }
        }

        [CLProperty(description: "The y offset of the object's texture")]
        public float TextureOffsetY
        {
            get
            {
                AssertRendererGet();
                return Value.renderCache[0].material.mainTextureOffset.y;
            }
            set
            {
                AssertRendererSet();
                var offset = Value.renderCache[0].material.mainTextureOffset;
                Value.renderCache[0].material.mainTextureOffset = new Vector2(offset.x, value);
            }
        }

        [CLProperty(description: "The ID of the object")]
        public int ID => Value.ScriptObject.Id;


        [CLProperty(description: "The tag of the object")]
        public string Tag
        {
            get => Value.GameObject.tag;
            set => MapLoader.RegisterTag(value, Value);
        }

        [CLProperty(description: "The layer of the object")]
        public int Layer
        {
            get => Value.GameObject.layer;
            set => Value.GameObject.layer = value;
        }

        [CLMethod(description: "Add a component to the object")]
        public CustomLogicComponentInstance AddComponent(string name)
        {
            return CustomLogicManager.Evaluator.AddMapObjectComponent(Value, name);
        }

        [CLMethod(description: "Remove a component from the object")]
        public void RemoveComponent(string name)
        {
            CustomLogicManager.Evaluator.RemoveComponent(Value.FindComponentInstance(name));
        }

        [CLMethod(description: "Get a component from the object")]
        public CustomLogicComponentInstance GetComponent(string name)
        {
            return Value.FindComponentInstance(name);
        }

        [CLMethod(description: "Set whether a component is enabled")]
        public void SetComponentEnabled(string name, bool enabled)
        {
            var clComp = Value.FindComponentInstance(name);
            if (clComp != null)
            {
                clComp.Enabled = enabled;
            }
            else
            {
                if (_builtinCache == null)
                {
                    return;
                }
                if (_builtinCache.TryGetValue(name, out var builtinComp))
                {
                    builtinComp.Enabled = enabled;
                }
                else
                {
                    throw new System.Exception($"Component '{name}' not found on MapObject '{Value.ScriptObject.Name}'.");
                }
            }
        }

        [CLMethod(description: "Set whether all components are enabled")]
        public void SetComponentsEnabled(bool enabled)
        {
            foreach (var instance in Value.ComponentInstances)
            {
                instance.Enabled = enabled;
            }

            foreach (var comp in _builtinCache.Values)
            {
                comp.Enabled = enabled;
            }
        }

        [CLMethod(description: "Add a sphere collider to the object")]
        public void AddSphereCollider(string collideMode, string collideWith, CustomLogicVector3Builtin center, float radius)
        {
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
        }

        [CLMethod(description: "Add a box collider to the object")]
        public void AddBoxCollider(string collideMode, string collideWith, CustomLogicVector3Builtin center = null, CustomLogicVector3Builtin size = null)
        {
            Vector3 centerV;
            Vector3 sizeV;
            Vector3 scale = Value.BaseScale;
            if (center != null && size != null)
            {
                centerV = center.Value;
                sizeV = size.Value;
                centerV = Util.DivideVectors(centerV, scale);
                sizeV = Util.DivideVectors(sizeV, scale);
            }
            else
            {
                // size based on all renderers
                Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
                bool hasBounds = false;
                foreach (var renderer in Value.renderCache)
                {
                    if (renderer != null)
                    {
                        if (hasBounds)
                        {
                            bounds.Encapsulate(renderer.bounds);
                        }
                        else
                        {
                            var rendererBounds = renderer.bounds;
                            bounds = renderer.bounds;
                            hasBounds = true;
                        }
                    }
                }

                centerV = bounds.center - Value.GameObject.transform.position;
                sizeV = bounds.size;

                // resize based on localscale
                sizeV = new Vector3(sizeV.x / Value.GameObject.transform.localScale.x, sizeV.y / Value.GameObject.transform.localScale.y, sizeV.z / Value.GameObject.transform.localScale.z);
            }

            var go = new GameObject();
            go.transform.SetParent(Value.GameObject.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            BoxCollider c = go.AddComponent<BoxCollider>();
            MapLoader.SetCollider(c, collideMode, collideWith);
            c.size = sizeV;
            c.center = centerV;
            var handler = go.AddComponent<CustomLogicCollisionHandler>();
            foreach (var instance in Value.ComponentInstances)
            {
                if (instance.UsesCollider())
                    handler.RegisterInstance(instance);
            }
        }

        [CLMethod(description: "Add a sphere target to the object")]
        public CustomLogicMapTargetableBuiltin AddSphereTarget(string team, CustomLogicVector3Builtin center, float radius)
        {
            string collideMode = "Region";
            string collideWith = "Hitboxes";
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
            var targetable = new MapTargetable(go.transform, c.center, team);
            MapLoader.MapTargetables.Add(targetable);
            return new CustomLogicMapTargetableBuiltin(go, targetable);
        }

        [CLMethod(description: "Add a box target to the object")]
        public CustomLogicMapTargetableBuiltin AddBoxTarget(string team, CustomLogicVector3Builtin center, CustomLogicVector3Builtin size)
        {
            string collideMode = "Region";
            string collideWith = "Hitboxes";
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
            var targetable = new MapTargetable(go.transform, c.center, team);
            MapLoader.MapTargetables.Add(targetable);
            return new CustomLogicMapTargetableBuiltin(go, targetable);
        }

        [CLMethod(description: "Get a child object by name")]
        public CustomLogicMapObjectBuiltin GetChild(string name)
        {
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

        [CLMethod(description: "Get all child objects")]
        public CustomLogicListBuiltin GetChildren()
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

        [CLMethod(description: "Get a child transform by name")]
        public CustomLogicTransformBuiltin GetTransform(string name)
        {
            Transform transform = Value.GameObject.transform.Find(name);
            if (transform != null)
            {
                return new CustomLogicTransformBuiltin(transform);
            }
            return null;
        }

        [CLMethod(description: "Set the color of all renderers on the object")]
        public void SetColorAll(CustomLogicColorBuiltin color)
        {
            if (Value.ScriptObject.Static)
            {
                throw new System.Exception("SetColorAll cannot be called on a static MapObject.");
            }
            foreach (Renderer r in Value.renderCache)
            {
                r.material.color = color.Value.ToColor();
            }
        }

        [CLMethod(description: "Check if a position is within the object's bounds")]
        public bool InBounds(CustomLogicVector3Builtin position)
        {
            // Iterate over colliders and check if the param position is within the bounds
            // Check all colliders on the object and on its children
            foreach (var collider in Value.colliderCache)
            {
                if (collider.bounds.Contains(position.Value))
                {
                    return true;
                }
            }
            return false;
        }

        [CLMethod(Description = "Get the bounds average center")]
        public CustomLogicVector3Builtin GetBoundsAverageCenter()
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

        [CLMethod(Description = "Get the bounds center")]
        public CustomLogicVector3Builtin GetBoundsCenter()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.center);
        }

        [CLMethod(Description = "Get the bounds size")]
        public CustomLogicVector3Builtin GetBoundsSize()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.size);
        }

        [CLMethod(Description = "Get the bounds min")]
        public CustomLogicVector3Builtin GetBoundsMin()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.min);
        }

        [CLMethod(Description = "Get the bounds max")]
        public CustomLogicVector3Builtin GetBoundsMax()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.max);
        }

        [CLMethod(Description = "Get the bounds extents")]
        public CustomLogicVector3Builtin GetBoundsExtents()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.extents);
        }

        [CLMethod(Description = "Get the corners of the bounds")]
        public CustomLogicListBuiltin GetCorners()
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
                    signs.ForEach(signZ =>
                    {
                        var vector = new Vector3(size.x * signX, size.y * signY, size.z * signZ);
                        result.List.Add(new CustomLogicVector3Builtin(boxCollider.transform.TransformPoint(boxCollider.center + vector * 0.5f)));
                    })));
            return result;
        }

        [CLMethod(description: "Whether or not the object has the given tag")]
        public bool HasTag(string tag)
        {
            return MapLoader.HasTag(Value, tag);
        }

        [CLMethod(description: "Add a builtin component to the MapObject")]
        public object AddBuiltinComponent(string name)
        {
            // Add the component and add to cache.
            if (_builtinCache == null)
            {
                _builtinCache = new Dictionary<string, BuiltinComponentInstance>();
            }
            if (_builtinCache.ContainsKey(name))
            {
                throw new System.Exception($"MapObject already has a {name} component.");
            }

            if (name == "Light")
            {
                _builtinCache[name] = new CustomLogicLightBuiltin(this);
            }
            else if (name == "Rigidbody")
            {
                if (Value.ScriptObject.Static)
                {
                    throw new System.Exception("AddRigidbody cannot be called on a static MapObject.");
                }
                _rigidBody = new CustomLogicRigidbodyBuiltin(this);
                _builtinCache[name] = _rigidBody;
            }
            else if (name == "CustomPhysicsMaterial")
            {
                _builtinCache[name] = new CustomLogicPhysicsMaterialBuiltin(this);
            }
            else if (name == "NavMeshObstacle")
            {
                _builtinCache[name] = new CustomLogicNavmeshObstacleBuiltin(this);
            }
            else if (name == "Lod")
            {
                _builtinCache[name] = new CustomLogicLodBuiltin(this);
            }
            else
            {
                throw new System.Exception($"Unknown builtin component: {name}");
            }
            return _builtinCache[name];
        }

        [CLMethod(description: "Gets a builtin component to the MapObject")]
        public object GetBuiltinComponent(string name)
        {
            if (_builtinCache == null || !_builtinCache.ContainsKey(name))
            {
                return null;
            }
            return _builtinCache[name];
        }

        [CLMethod(description: "Remove a builtin component from the MapObject")]
        public void RemoveBuiltinComponent(string name)
        {
            if (_builtinCache == null || !_builtinCache.ContainsKey(name))
            {
                throw new System.Exception($"MapObject does not have a {name} component.");
            }
            _builtinCache[name].Unload();
            _builtinCache.Remove(name);
            if (name == "Rigidbody")
            {
                _rigidBody = null;
            }
        }

        // Prop to get RigidBody
        [CLProperty(description: "The Rigidbody component of the MapObject, is null if not added.")]
        public CustomLogicRigidbodyBuiltin Rigidbody
        {
            get
            {
                return _rigidBody;
            }
        }

        private void AssertRendererGet()
        {
            if (Value.renderCache.Length == 0)
            {
                throw new System.Exception("MapObject has no renderer.");
            }
        }

        private void AssertRendererSet()
        {
            if (Value.ScriptObject.Static)
            {
                throw new System.Exception("Property cannot be set on a static MapObject.");
            }
            if (Value.renderCache.Length == 0)
            {
                throw new System.Exception("MapObject has no renderer.");
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

        public override int GetHashCode()
        {
            return Value.ScriptObject.Id;
        }

        public override string ToString()
        {
            return $"{Value.GameObject.name} (MapObject)";
        }
    }
}
