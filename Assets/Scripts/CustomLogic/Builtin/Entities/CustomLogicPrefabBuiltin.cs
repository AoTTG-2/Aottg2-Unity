using Map;

namespace CustomLogic
{
    [CLType(Name = "Prefab", Static = false, Description = "Represents a prefab object that can be instantiated in the map. Currently handles single elements, not children.")]
    partial class CustomLogicPrefabBuiltin : BuiltinClassInstance, ICustomLogicToString
    {
        public MapScriptSceneObject Value;

        [CLConstructor("Creates a new empty Prefab instance.")]
        public CustomLogicPrefabBuiltin()
        {
            Value = new MapScriptSceneObject();
        }

        [CLConstructor("Creates a new Prefab instance from a serialized CSV string.")]
        public CustomLogicPrefabBuiltin(
            [CLParam("The serialized CSV string describing the prefab.")]
            string prefabCSV,
            [CLParam("If true, clears all components from the prefab.")]
            bool clearComponents = false)
        {

            prefabCSV = string.Join("", prefabCSV.Split('\n'));
            Value = new MapScriptSceneObject();
            Value.Deserialize(prefabCSV);
            if (clearComponents)
            {
                ClearComponents();
            }
        }

        [CLProperty("The type of the prefab asset.")]
        public string AssetType
        {
            get => Value.Type;
            set
            {
                Value.Type = value;
                Refresh();
            }
        }

        [CLProperty("The asset path/name of the prefab.")]
        public string Asset
        {
            get => Value.Asset; set
            {
                Value.Asset = value;
                Refresh();
            }
        }

        [CLProperty("Whether the prefab is active.")]
        public bool Active
        {
            get => Value.Active; set
            {
                Value.Active = value;
                Refresh();
            }
        }

        [CLProperty("Whether the prefab is marked as static.")]
        public bool Static
        {
            get => Value.Static; set
            {
                Value.Static = value;
                Refresh();
            }
        }

        [CLProperty("Whether the prefab is visible.")]
        public bool Visible
        {
            get => Value.Visible; set
            {
                Value.Visible = value;
                Refresh();
            }
        }

        [CLProperty("The name of the prefab.")]
        public string Name
        {
            get => Value.Name; set
            {
                Value.Name = value;
                Refresh();
            }
        }

        [CLProperty("The position of the prefab in world space.")]
        public CustomLogicVector3Builtin Position
        {
            get => new CustomLogicVector3Builtin(Value.GetPosition());
            set
            {
                Value.SetPosition(value.Value);
                Refresh();
            }
        }

        [CLProperty("The rotation of the prefab in euler angles.")]
        public CustomLogicVector3Builtin Rotation
        {
            get => new CustomLogicVector3Builtin(Value.GetRotation());
            set
            {
                Value.SetRotation(value.Value);
                Refresh();
            }
        }

        [CLProperty("The scale of the prefab.")]
        public CustomLogicVector3Builtin Scale
        {
            get => new CustomLogicVector3Builtin(Value.GetScale());
            set
            {
                Value.SetScale(value.Value);
                Refresh();
            }
        }

        [CLProperty("The collision mode of the prefab.")]
        public string CollideMode
        {
            get => Value.CollideMode; set
            {
                Value.CollideMode = value;
                Refresh();
            }
        }

        [CLProperty("The layers that this prefab can collide with.")]
        public string CollideWith
        {
            get => Value.CollideWith; set
            {
                Value.CollideWith = value;
                Refresh();
            }
        }

        [CLProperty("The name of the physics material applied to the prefab.")]
        public string PhysicsMaterial
        {
            get => Value.PhysicsMaterial; set
            {
                Value.PhysicsMaterial = value;
                Refresh();
            }
        }

        [CLProperty("If true and this prefab is spawned at runtime with networking, the ownership will transfer to host if the player who spawned it leaves.")]
        public bool PersistsOwnership = false;

        [CLMethod("Clears all components from the prefab.")]
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
