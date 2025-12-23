using Map;

namespace CustomLogic
{
    /// <summary>
    /// Will expand this further later, currently only handles single elements, not children.
    /// Should be able to take in a serialized CSV describing the object and populate a child array/parent object when done.
    /// </summary>
    [CLType(Name = "Prefab", Static = false, Description = "")]
    partial class CustomLogicPrefabBuiltin : BuiltinClassInstance, ICustomLogicToString
    {
        public MapScriptSceneObject Value;

        [CLConstructor]
        public CustomLogicPrefabBuiltin()
        {
            Value = new MapScriptSceneObject();
        }

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

        [CLProperty]
        public string Type
        {
            get => Value.Type; set
            {
                Value.Type = value;
                Refresh();
            }
        }

        [CLProperty]
        public string Asset
        {
            get => Value.Asset; set
            {
                Value.Asset = value;
                Refresh();
            }
        }

        [CLProperty]
        public bool Active
        {
            get => Value.Active; set
            {
                Value.Active = value;
                Refresh();
            }
        }

        [CLProperty]
        public bool Static
        {
            get => Value.Static; set
            {
                Value.Static = value;
                Refresh();
            }
        }

        [CLProperty]
        public bool Visible
        {
            get => Value.Visible; set
            {
                Value.Visible = value;
                Refresh();
            }
        }

        [CLProperty]
        public string Name
        {
            get => Value.Name; set
            {
                Value.Name = value;
                Refresh();
            }
        }

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

        [CLProperty]
        public string CollideMode
        {
            get => Value.CollideMode; set
            {
                Value.CollideMode = value;
                Refresh();
            }
        }

        [CLProperty]
        public string CollideWith
        {
            get => Value.CollideWith; set
            {
                Value.CollideWith = value;
                Refresh();
            }
        }

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
        /// If true and this prefab is spawned at runtime with networking, the ownership will transfer to host if the player who spawned it leaves.
        /// </summary>
        [CLProperty]
        public bool PersistsOwnership = false;

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
