using Unity.VisualScripting;
using UnityEngine;
using Utility;
using ColorUtility = UnityEngine.ColorUtility;

namespace CustomLogic
{
    /// <summary>
    /// Represents a color with RGBA components in the range [0, 255]. Implements copy semantics, acting like a struct.
    /// </summary>
    [CLType(Name = "Color", Static = true)]
    partial class CustomLogicColorBuiltin : BuiltinClassInstance, ICustomLogicEquals, ICustomLogicCopyable, ICustomLogicMathOperators, ICustomLogicToString
    {
        public Color255 Value = new Color255();

        /// <summary>
        /// Default constructor, creates a white color.
        /// </summary>
        [CLConstructor]
        public CustomLogicColorBuiltin() { }

        /// <summary>
        /// Creates a color from a hex string.
        /// </summary>
        /// <param name="hexString">The hex color string.</param>
        [CLConstructor]
        public CustomLogicColorBuiltin(string hexString)
        {
            if (ColorUtility.TryParseHtmlString(hexString, out var c))
                Value = new Color255(c);
        }

        /// <summary>
        /// Creates a color from RGB.
        /// </summary>
        /// <param name="r">The red component (0-255).</param>
        /// <param name="g">The green component (0-255).</param>
        /// <param name="b">The blue component (0-255).</param>
        [CLConstructor]
        public CustomLogicColorBuiltin(int r, int g, int b)
        {
            Value = new Color255(r, g, b);
        }

        /// <summary>
        /// Creates a color from RGBA.
        /// </summary>
        /// <param name="r">The red component (0-255).</param>
        /// <param name="g">The green component (0-255).</param>
        /// <param name="b">The blue component (0-255).</param>
        /// <param name="a">The alpha component (0-255).</param>
        [CLConstructor]
        public CustomLogicColorBuiltin(int r, int g, int b, int a)
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

        /// <summary>
        /// Red component of the color.
        /// </summary>
        [CLProperty]
        public int R
        {
            get => Value.R;
            set => Value.R = value;
        }

        /// <summary>
        /// Green component of the color.
        /// </summary>
        [CLProperty]
        public int G
        {
            get => Value.G;
            set => Value.G = value;
        }

        /// <summary>
        /// Blue component of the color.
        /// </summary>
        [CLProperty]
        public int B
        {
            get => Value.B;
            set => Value.B = value;
        }

        /// <summary>
        /// Alpha component of the color.
        /// </summary>
        [CLProperty]
        public int A
        {
            get => Value.A;
            set => Value.A = value;
        }

        /// <summary>
        /// Converts the color to a hex string.
        /// </summary>
        [CLMethod]
        public string ToHexString()
        {
            return Value.ToColor().ToHexString();
        }

        /// <summary>
        /// Linearly interpolates between two colors.
        /// </summary>
        /// <param name="a">Color to interpolate from.</param>
        /// <param name="b">Color to interpolate to.</param>
        /// <param name="t">Interpolation factor. 0 = `a`, 1 = `b`.</param>
        /// <returns>A new color between `a` and `b`.</returns>
        [CLMethod]
        public static CustomLogicColorBuiltin Lerp(CustomLogicColorBuiltin a, CustomLogicColorBuiltin b, float t)
        {
            return new CustomLogicColorBuiltin(Color255.Lerp(a.Value, b.Value, t));
        }

        /// <summary>
        /// Creates a gradient color from two colors.
        /// </summary>
        /// <param name="a">The first color.</param>
        /// <param name="b">The second color.</param>
        /// <param name="t">The interpolation factor (0 = a, 1 = b).</param>
        /// <returns>A new color interpolated between the two colors.</returns>
        [CLMethod]
        public static CustomLogicColorBuiltin Gradient(CustomLogicColorBuiltin a, CustomLogicColorBuiltin b, float t)
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

        /// <summary>
        /// Checks if two colors are equal.
        /// </summary>
        /// <returns>True if the colors are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicColorBuiltin a, CustomLogicColorBuiltin b) => a.Value == b.Value,
                _ => false
            };
        }

        /// <summary>
        /// Gets the hash code of the color.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Creates a copy of this color.
        /// </summary>
        /// <returns>A new Color with the same RGBA values.</returns>
        [CLMethod]
        public object __Copy__()
        {
            return new CustomLogicColorBuiltin(new Color255(Value.R, Value.G, Value.B, Value.A));
        }

        /// <summary>
        /// Converts the color to a string.
        /// </summary>
        /// <returns>A string representation of the color.</returns>
        [CLMethod]
        public string __Str__()
        {
            return ToString();
        }

        /// <summary>
        /// Adds two colors component-wise.
        /// </summary>
        /// <returns>A new color with added components (clamped to 0-255).</returns>
        [CLMethod]
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

        /// <summary>
        /// Subtracts two colors component-wise.
        /// </summary>
        /// <returns>A new color with subtracted components (clamped to 0-255).</returns>
        [CLMethod]
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

        /// <summary>
        /// Multiplies two colors component-wise.
        /// </summary>
        /// <returns>A new color with multiplied components (clamped to 0-255).</returns>
        [CLMethod]
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

        /// <summary>
        /// Divides two colors component-wise.
        /// </summary>
        /// <returns>A new color with divided components (clamped to 0-255).</returns>
        [CLMethod]
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
