using Map;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "MapTargetable", Abstract = true)]
    partial class CustomLogicMapTargetableBuiltin : BuiltinClassInstance
    {
        public GameObject GameObject { get; }
        public MapTargetable Value { get; }

        public CustomLogicMapTargetableBuiltin(GameObject gameObject, MapTargetable mapTargetable)
        {
            GameObject = gameObject;
            Value = mapTargetable;
        }

        [CLProperty(description: "The team of the targetable")]
        public string Team
        {
            get => Value.Team;
            set => Value.Team = value;
        }

        [CLProperty(description: "The position of the targetable")]
        public CustomLogicVector3Builtin Position => new CustomLogicVector3Builtin(Value.GetPosition());

        [CLProperty(description: "Is the targetable enabled")]
        public bool Enabled // This might cause an issue with the name overriding unity's Enabled property.
        {
            get => Value.Enabled;
            set => Value.Enabled = value;
        }
    }
}