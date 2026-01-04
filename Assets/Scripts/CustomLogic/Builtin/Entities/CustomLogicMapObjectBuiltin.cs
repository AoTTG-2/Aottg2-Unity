using Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

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

        /// <summary>
        /// Object does not move.
        /// </summary>
        [CLProperty]
        public bool Static => Value.ScriptObject.Static;

        /// <summary>
        /// The position of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.position);
            set => Value.GameObject.transform.position = value.Value;
        }

        /// <summary>
        /// The local position of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin LocalPosition
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.localPosition);
            set => Value.GameObject.transform.localPosition = value.Value;
        }

        /// <summary>
        /// The rotation of the object.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The local rotation of the object.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The rotation of the object as a quaternion.
        /// </summary>
        [CLProperty]
        public CustomLogicQuaternionBuiltin QuaternionRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.GameObject.transform.rotation);
            set => Value.GameObject.transform.rotation = value.Value;
        }

        /// <summary>
        /// The local rotation of the object as a quaternion.
        /// </summary>
        [CLProperty]
        public CustomLogicQuaternionBuiltin QuaternionLocalRotation
        {
            get => new CustomLogicQuaternionBuiltin(Value.GameObject.transform.localRotation);
            set => Value.GameObject.transform.localRotation = value.Value;
        }

        /// <summary>
        /// The forward direction of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Forward
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.forward.normalized);
            set => Value.GameObject.transform.forward = value.Value;
        }

        /// <summary>
        /// The up direction of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Up
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.up.normalized);
            set => Value.GameObject.transform.up = value.Value;
        }

        /// <summary>
        /// The right direction of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Right
        {
            get => new CustomLogicVector3Builtin(Value.GameObject.transform.right.normalized);
            set => Value.GameObject.transform.right = value.Value;
        }

        /// <summary>
        /// The scale of the object.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The name of the object.
        /// </summary>
        [CLProperty]
        public string Name => Value.ScriptObject.Name;

        /// <summary>
        /// The parent of the object.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Whether the object is active.
        /// </summary>
        [CLProperty]
        public bool Active
        {
            get => Value.GameObject.activeSelf;
            set => Value.GameObject.SetActive(value);
        }

        /// <summary>
        /// The transform of the object.
        /// </summary>
        [CLProperty]
        public CustomLogicTransformBuiltin Transform => new CustomLogicTransformBuiltin(Value.GameObject.transform);

        /// <summary>
        /// Whether the object has a renderer.
        /// </summary>
        [CLProperty]
        public bool HasRenderer => Value.renderCache.Length > 0;

        /// <summary>
        /// The color of the object.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The x tiling of the object's texture.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The y tiling of the object's texture.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The x offset of the object's texture.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The y offset of the object's texture.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The ID of the object.
        /// </summary>
        [CLProperty]
        public int ID => Value.ScriptObject.Id;


        /// <summary>
        /// The tag of the object.
        /// </summary>
        [CLProperty]
        public string Tag
        {
            get => Value.GameObject.tag;
            set => MapLoader.RegisterTag(value, Value);
        }

        /// <summary>
        /// The layer of the object.
        /// </summary>
        [CLProperty]
        public int Layer
        {
            get => Value.GameObject.layer;
            set => Value.GameObject.layer = value;
        }

        /// <summary>
        /// Add a component to the object.
        /// </summary>
        /// <param name="name">The name of the component to add.</param>
        /// <returns>The added component instance.</returns>
        [CLMethod]
        public CustomLogicComponentInstance AddComponent(string name)
        {
            return CustomLogicManager.Evaluator.AddMapObjectComponent(Value, name);
        }

        /// <summary>
        /// Remove a component from the object.
        /// </summary>
        /// <param name="name">The name of the component to remove.</param>
        [CLMethod]
        public void RemoveComponent(string name)
        {
            CustomLogicManager.Evaluator.RemoveComponent(Value.FindComponentInstance(name));
        }

        /// <summary>
        /// Get a component from the object.
        /// </summary>
        /// <param name="name">The name of the component to get.</param>
        /// <returns>The component instance, or null if not found.</returns>
        [CLMethod]
        public CustomLogicComponentInstance GetComponent(string name)
        {
            return Value.FindComponentInstance(name);
        }

        /// <summary>
        /// Set whether a component is enabled.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="enabled">Whether the component should be enabled.</param>
        [CLMethod]
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

        /// <summary>
        /// Set whether all components are enabled.
        /// </summary>
        /// <param name="enabled">Whether all components should be enabled.</param>
        [CLMethod]
        public void SetComponentsEnabled(bool enabled)
        {
            foreach (var instance in Value.ComponentInstances)
            {
                instance.Enabled = enabled;
            }

            if (_builtinCache != null)
            {
                foreach (var comp in _builtinCache.Values)
                {
                    comp.Enabled = enabled;
                }
            }
        }

        /// <summary>
        /// Add a sphere collider to the object.
        /// </summary>
        /// <param name="collideMode">The collision mode.</param>
        /// <param name="collideWith">What the collider should collide with.</param>
        /// <param name="center">The center position of the sphere collider.</param>
        /// <param name="radius">The radius of the sphere collider.</param>
        [CLMethod]
        public void AddSphereCollider(
            [CLParam(Enum = typeof(CustomLogicCollideModeEnum))] string collideMode,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))] string collideWith,
            CustomLogicVector3Builtin center,
            float radius)
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

        /// <summary>
        /// Add a box collider to the object.
        /// </summary>
        /// <param name="collideMode">The collision mode.</param>
        /// <param name="collideWith">What the collider should collide with.</param>
        /// <param name="center">The center position of the box collider (optional, defaults to calculated bounds).</param>
        /// <param name="size">The size of the box collider (optional, defaults to calculated bounds).</param>
        [CLMethod]
        public void AddBoxCollider(
            [CLParam(Enum = typeof(CustomLogicCollideModeEnum))] string collideMode,
            [CLParam(Enum = typeof(CustomLogicCollideWithEnum))] string collideWith,
            CustomLogicVector3Builtin center = null,
            CustomLogicVector3Builtin size = null)
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

        /// <summary>
        /// Add a sphere target to the object.
        /// </summary>
        /// <param name="team">The team that can target this.</param>
        /// <param name="center">The center position of the sphere target.</param>
        /// <param name="radius">The radius of the sphere target.</param>
        /// <returns>The created targetable object.</returns>
        [CLMethod]
        public CustomLogicMapTargetableBuiltin AddSphereTarget(
            [CLParam(Enum = typeof(CustomLogicTeamEnum))] string team,
            CustomLogicVector3Builtin center,
            float radius)
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

        /// <summary>
        /// Add a box target to the object.
        /// </summary>
        /// <param name="team">The team that can target this.</param>
        /// <param name="center">The center position of the box target.</param>
        /// <param name="size">The size of the box target.</param>
        /// <returns>The created targetable object.</returns>
        [CLMethod]
        public CustomLogicMapTargetableBuiltin AddBoxTarget(
            [CLParam(Enum = typeof(CustomLogicTeamEnum))] string team,
            CustomLogicVector3Builtin center,
            CustomLogicVector3Builtin size)
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

        /// <summary>
        /// Get a child object by name.
        /// </summary>
        /// <param name="name">The name of the child object to get.</param>
        /// <returns>The child object if found, null otherwise.</returns>
        [CLMethod]
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

        /// <summary>
        /// Get all child objects.
        /// </summary>
        /// <returns>A list of all child objects.</returns>
        [CLMethod(ReturnTypeArguments = new[] { "MapObject" })]
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

        /// <summary>
        /// Get a child transform by name.
        /// </summary>
        /// <param name="name">The name of the transform to get.</param>
        /// <returns>The transform if found, null otherwise.</returns>
        [CLMethod]
        public CustomLogicTransformBuiltin GetTransform(string name)
        {
            Transform transform = Value.GameObject.transform.Find(name);
            if (transform != null)
            {
                return new CustomLogicTransformBuiltin(transform);
            }
            return null;
        }

        /// <summary>
        /// Set the color of all renderers on the object.
        /// </summary>
        /// <param name="color">The color to set.</param>
        [CLMethod]
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

        /// <summary>
        /// Check if a position is within the object's bounds.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if the position is within the bounds, false otherwise.</returns>
        [CLMethod]
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

        /// <summary>
        /// Get the bounds average center.
        /// </summary>
        [CLMethod]
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

        /// <summary>
        /// Get the bounds center.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetBoundsCenter()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.center);
        }

        /// <summary>
        /// Get the bounds size.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetBoundsSize()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.size);
        }

        /// <summary>
        /// Get the bounds min.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetBoundsMin()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.min);
        }

        /// <summary>
        /// Get the bounds max.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetBoundsMax()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.max);
        }

        /// <summary>
        /// Get the bounds extents.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetBoundsExtents()
        {
            if (Value.colliderCache.Length == 0)
            {
                return null;
            }
            return new CustomLogicVector3Builtin(Value.colliderCache[0].bounds.extents);
        }

        /// <summary>
        /// Get the corners of the bounds.
        /// </summary>
        [CLMethod(ReturnTypeArguments = new[] { "Vector3" })]
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

        /// <summary>
        /// Whether or not the object has the given tag.
        /// </summary>
        /// <param name="tag">The tag to check for.</param>
        [CLMethod]
        public bool HasTag(string tag)
        {
            return MapLoader.HasTag(Value, tag);
        }

        /// <summary>
        /// Add a builtin component to the MapObject.
        /// </summary>
        /// <param name="name">The name of the builtin component to add.</param>
        [CLMethod]
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

            if (name == "DayLight")
            {
                _builtinCache[name] = new CustomLogicLightBuiltin(this, LightType.Directional);
            }
            else if (name == "PointLight")
            {
                _builtinCache[name] = new CustomLogicLightBuiltin(this, LightType.Point);
            }
            else if (name == "SpotLight")
            {
                _builtinCache[name] = new CustomLogicLightBuiltin(this, LightType.Spot);
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

        /// <summary>
        /// Gets a builtin component to the MapObject.
        /// </summary>
        /// <param name="name">The name of the builtin component to get.</param>
        [CLMethod]
        public object GetBuiltinComponent(string name)
        {
            if (_builtinCache == null || !_builtinCache.ContainsKey(name))
            {
                return null;
            }
            return _builtinCache[name];
        }

        /// <summary>
        /// Remove a builtin component from the MapObject.
        /// </summary>
        /// <param name="name">The name of the builtin component to remove.</param>
        [CLMethod]
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

        /// <summary>
        /// Serialize the current object to a csv.
        /// </summary>
        [CLMethod]
        public string ConvertToCSV()
        {
            return Value.ScriptObject.Serialize();
        }

        // Prop to get RigidBody
        /// <summary>
        /// The Rigidbody component of the MapObject, is null if not added.
        /// </summary>
        [CLProperty]
        public CustomLogicRigidbodyBuiltin Rigidbody
        {
            get
            {
                return _rigidBody;
            }
        }

        /// <summary>
        /// The NetworkView attached to the MapObject, is null if not initialized yet.
        /// </summary>
        [CLProperty]
        public CustomLogicNetworkViewBuiltin NetworkView { get; set; }

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
