using System.Collections.Generic;
using Unity.VisualScripting;
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
            if (methodName == "ToHexString")
            {
                return Value.ToColor().ToHexString();
            }
            if (methodName == "Gradient")
            {
                // Expects two colors and a float
                var a = (CustomLogicColorBuiltin)parameters[0];
                var b = (CustomLogicColorBuiltin)parameters[1];
                var ac = a.Value.ToColor();
                var bc = b.Value.ToColor();
                var t = (float)parameters[2];

                var colors = new GradientColorKey[2];
                colors[0] = new GradientColorKey(ac, 0f);
                colors[1] = new GradientColorKey(bc, 1f);

                var alphas = new GradientAlphaKey[2];
                alphas[0] = new GradientAlphaKey(ac.a, ac.a);
                alphas[1] = new GradientAlphaKey(bc.a, bc.a);

                return new CustomLogicColorBuiltin(Color255.Gradient(colors, alphas, GradientMode.Blend, t));
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "R")
                return Value.R;
            if (name == "G")
                return Value.G;
            if (name == "B")
                return Value.B;
            if (name == "A")
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
