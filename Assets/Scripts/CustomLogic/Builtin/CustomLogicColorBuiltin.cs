using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using ColorUtility = UnityEngine.ColorUtility;

namespace CustomLogic
{
    [CLType(InheritBaseMembers = true, Static = true, Description = "")]
    class CustomLogicColorBuiltin : CustomLogicClassInstanceBuiltin, ICustomLogicEquals, ICustomLogicCopyable, ICustomLogicMathOperators, ICustomLogicToString
    {
        public Color255 Value = new Color255();

        public CustomLogicColorBuiltin(object[] parameterValues) : base("Color")
        {
            var color = new Color255();
            
            if (parameterValues.Length == 1)
            {
                if (ColorUtility.TryParseHtmlString((string)parameterValues[0], out var c))
                    color = new Color255(c);
            }
            else if (parameterValues.Length == 3)
            {
                color = new Color255((int)parameterValues[0], (int)parameterValues[1], (int)parameterValues[2]);
            }
            else if (parameterValues.Length == 4)
            {
                color = new Color255((int)parameterValues[0], (int)parameterValues[1], 
                    (int)parameterValues[2], (int)parameterValues[3]);
            }

            Value = color;
        }

        public CustomLogicColorBuiltin(Color255 value) : base("Color")
        {
            Value = value;
        }

        // Convert the above to CLProperties
        [CLProperty(Description = "Red component of the color")]
        public int R
        {
            get => Value.R;
            set => Value.R = value;
        }

        [CLProperty(Description = "Green component of the color")]
        public int G
        {
            get => Value.G;
            set => Value.G = value;
        }

        [CLProperty(Description = "Blue component of the color")]
        public int B
        {
            get => Value.B;
            set => Value.B = value;
        }

        [CLProperty(Description = "Alpha component of the color")]
        public int A
        {
            get => Value.A;
            set => Value.A = value;
        }

        // Convert the above to CLMethods
        [CLMethod(Description = "Converts the color to a hex string")]
        public string ToHexString()
        {
            return Value.ToColor().ToHexString();
        }

        [CLMethod(Description = "Linearly interpolates between colors a and b by t")]
        public static CustomLogicColorBuiltin Lerp(CustomLogicColorBuiltin a, CustomLogicColorBuiltin b, float t)
        {
            return new CustomLogicColorBuiltin(Color255.Lerp(a.Value, b.Value, t));
        }

        [CLMethod(Description = "Creates a gradient color from two colors")]
        public static CustomLogicColorBuiltin Gradient(CustomLogicColorBuiltin a, CustomLogicColorBuiltin b, float t)
        {
            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(a.Value.ToColor(), 0f);
            colors[1] = new GradientColorKey(b.Value.ToColor(), 1f);
            var alphas = new GradientAlphaKey[2];
            alphas[0] = new GradientAlphaKey(a.Value.ToColor().a, a.Value.ToColor().a);
            alphas[1] = new GradientAlphaKey(b.Value.ToColor().a, b.Value.ToColor().a);
            return new CustomLogicColorBuiltin(Color255.Gradient(colors, alphas, GradientMode.Blend, t));
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomLogicColorBuiltin))
                return false;
            var other = ((CustomLogicColorBuiltin)obj).Value;
            return Value.R == other.R && Value.G == other.G && Value.B == other.B && Value.A == other.A;
        }

        public override string ToString()
        {
            return $"({Value.R}, {Value.G}, {Value.B}, {Value.A})";
        }

        public CustomLogicColorBuiltin Copy()
        {
            var value = new Color255(Value.R, Value.G, Value.B, Value.A);
            return new CustomLogicColorBuiltin(value);
        }

        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => a.Value == b.Value,
                _ => false
            };
        }

        public int __Hash__()
        {
            return Value.GetHashCode();
        }

        public object __Copy__()
        {
            return new CustomLogicColorBuiltin(new Color255(Value.R, Value.G, Value.B, Value.A));
        }

        public string __Str__()
        {
            return ToString();
        }

        public object __Add__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => new CustomLogicColorBuiltin(new Color255(
                    Mathf.Clamp(a.R + b.R, 0, 255),
                    Mathf.Clamp(a.G + b.G, 0, 255),
                    Mathf.Clamp(a.B + b.B, 0, 255),
                    Mathf.Clamp(a.A + b.A, 0, 255)
                )),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other)
            };
        }

        public object __Sub__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => new CustomLogicColorBuiltin(new Color255(
                    Mathf.Clamp(a.R - b.R, 0, 255),
                    Mathf.Clamp(a.G - b.G, 0, 255),
                    Mathf.Clamp(a.B - b.B, 0, 255),
                    Mathf.Clamp(a.A - b.A, 0, 255)
                )),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other)
            };
        }

        public object __Mul__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => new CustomLogicColorBuiltin(new Color255(
                    Mathf.Clamp(a.R * b.R, 0, 255),
                    Mathf.Clamp(a.G * b.G, 0, 255),
                    Mathf.Clamp(a.B * b.B, 0, 255),
                    Mathf.Clamp(a.A * b.A, 0, 255)
                )),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Mul__), self, other)
            };
        }

        public object __Div__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => new CustomLogicColorBuiltin(new Color255(
                    Mathf.Clamp(a.R / b.R, 0, 255),
                    Mathf.Clamp(a.G / b.G, 0, 255),
                    Mathf.Clamp(a.B / b.B, 0, 255),
                    Mathf.Clamp(a.A / b.A, 0, 255)
                )),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Div__), self, other)
            };
        }
    }
}
