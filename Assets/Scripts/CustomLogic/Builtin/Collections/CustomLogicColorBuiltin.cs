using Unity.VisualScripting;
using UnityEngine;
using Utility;
using ColorUtility = UnityEngine.ColorUtility;

namespace CustomLogic
{
    [CLType(Name = "Color", Static = true, Description = "Represents a color with RGBA components in the range [0, 255]. Implements copy semantics, acting like a struct.")]
    partial class CustomLogicColorBuiltin : BuiltinClassInstance, ICustomLogicEquals, ICustomLogicCopyable, ICustomLogicMathOperators, ICustomLogicToString
    {
        public Color255 Value = new Color255();

        [CLConstructor("Default constructor, creates a white color.")]
        public CustomLogicColorBuiltin() { }

        [CLConstructor("Creates a color from a hex string.")]
        public CustomLogicColorBuiltin(
            [CLParam("The hex color string.")]
            string hexString)
        {
            if (ColorUtility.TryParseHtmlString(hexString, out var c))
                Value = new Color255(c);
        }

        [CLConstructor("Creates a color from RGB.")]
        public CustomLogicColorBuiltin(
            [CLParam("The red component (0-255).")]
            int r,
            [CLParam("The green component (0-255).")]
            int g,
            [CLParam("The blue component (0-255).")]
            int b)
        {
            Value = new Color255(r, g, b);
        }

        [CLConstructor("Creates a color from RGBA.")]
        public CustomLogicColorBuiltin(
            [CLParam("The red component (0-255).")]
            int r,
            [CLParam("The green component (0-255).")]
            int g,
            [CLParam("The blue component (0-255).")]
            int b,
            [CLParam("The alpha component (0-255).")]
            int a)
        {
            Value = new Color255(r, g, b, a);
        }


        public CustomLogicColorBuiltin(Color color)
        {
            Value = new Color255(color);
        }

        public CustomLogicColorBuiltin(Color255 value)
        {
            Value = value;
        }

        [CLProperty("Red component of the color.")]
        public int R
        {
            get => Value.R;
            set => Value.R = value;
        }

        [CLProperty("Green component of the color.")]
        public int G
        {
            get => Value.G;
            set => Value.G = value;
        }

        [CLProperty("Blue component of the color.")]
        public int B
        {
            get => Value.B;
            set => Value.B = value;
        }

        [CLProperty("Alpha component of the color.")]
        public int A
        {
            get => Value.A;
            set => Value.A = value;
        }

        [CLMethod("Converts the color to a hex string.")]
        public string ToHexString()
        {
            return Value.ToColor().ToHexString();
        }

        [CLMethod("Linearly interpolates between two colors. Returns: A new color between `a` and `b`.")]
        public static CustomLogicColorBuiltin Lerp(
            [CLParam("Color to interpolate from")]
            CustomLogicColorBuiltin a,
            [CLParam("Color to interpolate to")]
            CustomLogicColorBuiltin b,
            [CLParam("Interpolation factor. 0 = `a`, 1 = `b`")]
            float t)
        {
            return new CustomLogicColorBuiltin(Color255.Lerp(a.Value, b.Value, t));
        }

        [CLMethod("Creates a gradient color from two colors. Returns: A new color interpolated between the two colors.")]
        public static CustomLogicColorBuiltin Gradient(
            [CLParam("The first color.")]
            CustomLogicColorBuiltin a,
            [CLParam("The second color.")]
            CustomLogicColorBuiltin b,
            [CLParam("The interpolation factor (0 = a, 1 = b).")]
            float t)
        {
            // TODO: isn't this the same as Lerp?

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

        [CLMethod("Checks if two colors are equal. Returns: True if the colors are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => a.Value == b.Value,
                _ => false
            };
        }

        [CLMethod("Gets the hash code of the color. Returns: The hash code.")]
        public int __Hash__()
        {
            return Value.GetHashCode();
        }

        [CLMethod("Creates a copy of this color. Returns: A new Color with the same RGBA values.")]
        public object __Copy__()
        {
            return new CustomLogicColorBuiltin(new Color255(Value.R, Value.G, Value.B, Value.A));
        }

        [CLMethod("Converts the color to a string. Returns: A string representation of the color.")]
        public string __Str__()
        {
            return ToString();
        }

        [CLMethod("Adds two colors component-wise. Returns: A new color with added components (clamped to 0-255).")]
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

        [CLMethod("Subtracts two colors component-wise. Returns: A new color with subtracted components (clamped to 0-255).")]
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

        [CLMethod("Multiplies two colors component-wise. Returns: A new color with multiplied components (clamped to 0-255).")]
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

        [CLMethod("Divides two colors component-wise. Returns: A new color with divided components (clamped to 0-255).")]
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

        public object __Mod__(object self, object other)
        {
            throw new System.NotImplementedException();
        }
    }
}
