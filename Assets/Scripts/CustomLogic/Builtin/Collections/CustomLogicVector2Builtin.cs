using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a 2D vector with X and Y components. Supports mathematical operations and implements copy semantics.
    /// </summary>
    [CLType(Name = "Vector2", Static = true)]
    partial class CustomLogicVector2Builtin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        private Vector2 _value;

        /// <summary>
        /// Default constructor, initializes the Vector2 to (0, 0).
        /// </summary>
        [CLConstructor]
        public CustomLogicVector2Builtin() { }

        /// <summary>
        /// Initializes the Vector2 to (xy, xy).
        /// </summary>
        /// <param name="xy">The value for X and Y components.</param>
        [CLConstructor]
        public CustomLogicVector2Builtin(float xy)
        {
            _value = new Vector2(xy, xy);
        }

        /// <summary>
        /// Initializes the Vector2 to (x, y).
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        [CLConstructor]
        public CustomLogicVector2Builtin(float x, float y)
        {
            _value = new Vector2(x, y);
        }

        public CustomLogicVector2Builtin(Vector2 value)
        {
            _value = value;
        }

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        [CLProperty]
        public float X
        {
            get => _value.x;
            set => _value.x = value;
        }

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        [CLProperty]
        public float Y
        {
            get => _value.y;
            set => _value.y = value;
        }

        /// <summary>
        /// Returns a normalized copy of this vector (magnitude of 1).
        /// </summary>
        [CLProperty]
        public CustomLogicVector2Builtin Normalized => _value.normalized;

        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        [CLProperty]
        public float Magnitude => _value.magnitude;

        /// <summary>
        /// Returns the squared length of this vector (faster than Magnitude).
        /// </summary>
        [CLProperty]
        public float SqrMagnitude => _value.sqrMagnitude;

        /// <summary>
        /// Shorthand for writing Vector2(0, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin Zero => Vector2.zero;

        /// <summary>
        /// Shorthand for writing Vector2(1, 1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin One => Vector2.one;

        /// <summary>
        /// Shorthand for writing Vector2(0, 1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin Up => Vector2.up;

        /// <summary>
        /// Shorthand for writing Vector2(0, -1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin Down => Vector2.down;

        /// <summary>
        /// Shorthand for writing Vector2(-1, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin Left => Vector2.left;

        /// <summary>
        /// Shorthand for writing Vector2(1, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin Right => Vector2.right;

        /// <summary>
        /// Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin NegativeInfinity => Vector2.negativeInfinity;

        /// <summary>
        /// Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector2Builtin PositiveInfinity => Vector2.positiveInfinity;

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        [CLMethod]
        public static float Angle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to)
            => Vector2.Angle(from, to);

        /// <summary>
        /// Clamps the magnitude of a vector to a maximum value.
        /// </summary>
        /// <param name="vector">The vector to clamp.</param>
        /// <param name="maxLength">The maximum length of the vector.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin ClampMagnitude(CustomLogicVector2Builtin vector, float maxLength)
            => Vector2.ClampMagnitude(vector, maxLength);

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        [CLMethod]
        public static float Distance(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b)
            => Vector2.Distance(a, b);

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static float Dot(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b)
            => Vector2.Dot(a, b);

        /// <summary>
        /// Linearly interpolates between two vectors.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        [CLMethod]
        public static CustomLogicVector2Builtin Lerp(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t)
            => Vector2.Lerp(a, b, t);

        /// <summary>
        /// Linearly interpolates between two vectors without clamping.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        [CLMethod]
        public static CustomLogicVector2Builtin LerpUnclamped(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b, float t)
            => Vector2.LerpUnclamped(a, b, t);

        /// <summary>
        /// Returns a vector that is made from the largest components of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin Max(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b)
            => Vector2.Max(a, b);

        /// <summary>
        /// Returns a vector that is made from the smallest components of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin Min(CustomLogicVector2Builtin a, CustomLogicVector2Builtin b)
            => Vector2.Min(a, b);

        /// <summary>
        /// Moves a point towards a target position.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="maxDistanceDelta">The maximum distance to move.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin MoveTowards(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, float maxDistanceDelta)
            => Vector2.MoveTowards(current, target, maxDistanceDelta);

        /// <summary>
        /// Reflects a vector off a plane defined by a normal vector.
        /// </summary>
        /// <param name="inDirection">The incoming direction vector.</param>
        /// <param name="inNormal">The normal vector of the surface.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin Reflect(CustomLogicVector2Builtin inDirection, CustomLogicVector2Builtin inNormal)
            => Vector2.Reflect(inDirection, inNormal);

        /// <summary>
        /// Calculates the signed angle between two vectors.
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        [CLMethod]
        public static float SignedAngle(CustomLogicVector2Builtin from, CustomLogicVector2Builtin to)
            => Vector2.SignedAngle(from, to);

        /// <summary>
        /// Smoothly dampens a vector towards a target over time.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="currentVelocity">The current velocity (modified by the function).</param>
        /// <param name="smoothTime">The time it takes to reach the target (approximately).</param>
        /// <param name="maxSpeed">The maximum speed.</param>
        [CLMethod]
        public static CustomLogicVector2Builtin SmoothDamp(CustomLogicVector2Builtin current, CustomLogicVector2Builtin target, CustomLogicVector2Builtin currentVelocity, float smoothTime, float maxSpeed)
            => Vector2.SmoothDamp(current, target, ref currentVelocity._value, smoothTime, maxSpeed);

        /// <summary>
        /// Sets the X and Y components of the vector.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        [CLMethod]
        public void Set(float x, float y)
            => _value.Set(x, y);

        /// <summary>
        /// Normalizes the vector in place.
        /// </summary>
        [CLMethod]
        public void Normalize() => _value.Normalize();

        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Creates a copy of this vector.
        /// </summary>
        /// <returns>A new Vector2 with the same values.</returns>
        [CLMethod]
        public virtual object __Copy__()
        {
            var value = new Vector2(_value.x, _value.y);
            return new CustomLogicVector2Builtin(value);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <returns>A new vector that is the sum of the two vectors.</returns>
        [CLMethod]
        public object __Add__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value + b._value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other)
            };
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <returns>A new vector that is the difference of the two vectors.</returns>
        [CLMethod]
        public object __Sub__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value - b._value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other)
            };
        }

        /// <summary>
        /// Multiplies a vector by a scalar or another vector.
        /// </summary>
        /// <returns>A new vector with the multiplied result.</returns>
        [CLMethod]
        public object __Mul__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value * b._value),
                (CustomLogicVector2Builtin a, float f) => new CustomLogicVector2Builtin(a._value * f),
                (float f, CustomLogicVector2Builtin a) => new CustomLogicVector2Builtin(a._value * f),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Mul__), self, other)
            };
        }

        /// <summary>
        /// Divides a vector by a scalar or another vector.
        /// </summary>
        /// <returns>A new vector with the divided result.</returns>
        [CLMethod]
        public object __Div__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value / b._value),
                (CustomLogicVector2Builtin a, float f) => new CustomLogicVector2Builtin(a._value / f),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Div__), self, other)
            };
        }

        /// <summary>
        /// Checks if two vectors are equal.
        /// </summary>
        /// <returns>True if the vectors are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => a._value == b._value,
                _ => false
            };
        }

        /// <summary>
        /// Gets the hash code of the vector.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__() => _value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator Vector2(CustomLogicVector2Builtin value) => value._value;
        public static implicit operator CustomLogicVector2Builtin(Vector2 value) => new CustomLogicVector2Builtin(value);
    }
}
