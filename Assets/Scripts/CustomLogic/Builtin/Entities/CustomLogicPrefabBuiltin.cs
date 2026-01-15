using Map;

namespace CustomLogic
{
    /// <summary>
    /// Represents a prefab object that can be instantiated in the map. Currently handles single elements, not children.
    /// </summary>
    [CLType(Name = "Prefab", Static = false)]
    partial class CustomLogicPrefabBuiltin : BuiltinClassInstance, ICustomLogicToString
    {
        public MapScriptSceneObject Value;

        /// <summary>
        /// Creates a new empty Prefab instance.
        /// </summary>
        [CLConstructor]
        public CustomLogicPrefabBuiltin()
        {
            Value = new MapScriptSceneObject();
        }

        /// <summary>
        /// Creates a new Prefab instance from a serialized CSV string.
        /// </summary>
        /// <param name="prefabCSV">The serialized CSV string describing the prefab.</param>
        /// <param name="clearComponents">If true, clears all components from the prefab.</param>
        [CLConstructor]
        public CustomLogicPrefabBuiltin(string prefabCSV, bool clearComponents = false)
        {

            prefabCSV = string.Join("", prefabCSV.Split('\n'));
            Value = new MapScriptSceneObject();
            Value.Deserialize(prefabCSV);
            if (clearComponents)
            {
                ClearComponents();
            }
        }

        /// <summary>
        /// The type of the prefab asset.
        /// </summary>
        [CLProperty]
        public string AssetType
        {
            get => Value.Type;
            set
            {
                Value.Type = value;
                Refresh();
            }
        }

        /// <summary>
        /// The asset path/name of the prefab.
        /// </summary>
        [CLProperty]
        public string Asset
        {
            get => Value.Asset; set
            {
                Value.Asset = value;
                Refresh();
            }
        }

        /// <summary>
        /// Whether the prefab is active.
        /// </summary>
        [CLProperty]
        public bool Active
        {
            get => Value.Active; set
            {
                Value.Active = value;
                Refresh();
            }
        }

        /// <summary>
        /// Whether the prefab is marked as static.
        /// </summary>
        [CLProperty]
        public bool Static
        {
            get => Value.Static; set
            {
                Value.Static = value;
                Refresh();
            }
        }

        /// <summary>
        /// Whether the prefab is visible.
        /// </summary>
        [CLProperty]
        public bool Visible
        {
            get => Value.Visible; set
            {
                Value.Visible = value;
                Refresh();
            }
        }

        /// <summary>
        /// The name of the prefab.
        /// </summary>
        [CLProperty]
        public string Name
        {
            get => Value.Name; set
            {
                Value.Name = value;
                Refresh();
            }
        }

        /// <summary>
        /// The position of the prefab in world space.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.GetPosition());
            set
            {
                Value.SetPosition(value.Value);
                Refresh();
            }
        }

        /// <summary>
        /// The rotation of the prefab in euler angles.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Rotation
        {
            get => new CustomLogicVector3Builtin(Value.GetRotation());
            set
            {
                Value.SetRotation(value.Value);
                Refresh();
            }
        }

        /// <summary>
        /// The scale of the prefab.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Scale
        {
            get => new CustomLogicVector3Builtin(Value.GetScale());
            set
            {
                Value.SetScale(value.Value);
                Refresh();
            }
        }

        /// <summary>
        /// The collision mode of the prefab.
        /// </summary>
        [CLProperty]
        public string CollideMode
        {
            get => Value.CollideMode; set
            {
                Value.CollideMode = value;
                Refresh();
            }
        }

        /// <summary>
        /// The layers that this prefab can collide with.
        /// </summary>
        [CLProperty]
        public string CollideWith
        {
            get => Value.CollideWith; set
            {
                Value.CollideWith = value;
                Refresh();
            }
        }

        /// <summary>
        /// The name of the physics material applied to the prefab.
        /// </summary>
        [CLProperty]
        public string PhysicsMaterial
        {
            get => Value.PhysicsMaterial; set
            {
                Value.PhysicsMaterial = value;
                Refresh();
            }
        }

        /// <summary>
        /// If true and this prefab is spawned at runtime with networking, the ownership will transfer to host
        /// if the player who spawned it leaves.
        /// </summary>
        [CLProperty]
        public bool PersistsOwnership = false;

        /// <summary>
        /// Clears all components from the prefab.
        /// </summary>
        [CLMethod]
        public void ClearComponents()
        {
            Value.Components.Clear();
            Refresh();
        }

        public void Refresh()
        {
            string serialized = Value.Serialize();
            Value.Deserialize(serialized);
        }

        public string __Str__()
        {
            return Value.Serialize();
        }
    }
}
