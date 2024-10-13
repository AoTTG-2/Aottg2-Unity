using Map;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicMapTargetableBuiltin : CustomLogicBaseBuiltin
    {
        public GameObject GameObject { get; }
        public MapTargetable Value { get; }

        public CustomLogicMapTargetableBuiltin(GameObject gameObject, MapTargetable mapTargetable) : base("MapTargetable")
        {
            GameObject = gameObject;
            Value = mapTargetable;
        }

        public override object GetField(string name)
        {
            if (name == "Team")
                return Value.GetTeam();
            if (name == "Position")
                return new CustomLogicVector3Builtin(Value.GetPosition());
            if (name == "Enabled")
                return Value.Enabled;

            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "Team")
                Value.Team = (string)value;
            else if (name == "Enabled")
                Value.Enabled = (bool)value;
            else
                base.SetField(name, value);
        }
    }
}