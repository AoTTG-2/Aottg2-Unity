using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicColorBuiltin : CustomLogicStructBuiltin
    {
        public Color255 Value = new Color255();

        public CustomLogicColorBuiltin(List<object> parameterValues) : base("Color")
        {
            if (parameterValues.Count == 0)
                return;
            Value = new Color255((int)parameterValues[0], (int)parameterValues[1], 
                (int)parameterValues[2], (int)parameterValues[3]);
        }

        public CustomLogicColorBuiltin(Color255 value) : base("Color")
        {
            Value = value;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "R")
                return Value.R;
            else if (name == "G")
                return Value.G;
            else if (name == "B")
                return Value.B;
            else if (name == "A")
                return Value.A;
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "R")
                Value.R = (int)value;
            else if (name == "G")
                Value.G = (int)value;
            else if (name == "B")
                Value.B = (int)value;
            else if (name == "A")
                Value.A = (int)value;
            else
                base.SetField(name, value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomLogicColorBuiltin))
                return false;
            var other = ((CustomLogicColorBuiltin)obj).Value;
            return Value.R == other.R && Value.G == other.G && Value.B == other.B && Value.A == other.A;
        }

        public override CustomLogicStructBuiltin Copy()
        {
            var value = new Color255(Value.R, Value.G, Value.B, Value.A);
            return new CustomLogicColorBuiltin(value);
        }
    }
}
