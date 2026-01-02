using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Vector2", Static = true)]
    partial class CustomLogicVector2Builtin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        private Vector2 _value;

        [CLConstructor("Default constructor, initializes the Vector2 to (0, 0).")]
        public CustomLogicVector2Builtin() { }

        [CLConstructor("Initializes the Vector2 to (xy, xy).")]
        public CustomLogicVector2Builtin(
            [CLParam("The value for X and Y components.")]
            float xy)
        {
            _value = new Vector2(xy, xy);
        }

        [CLConstructor("Initializes the Vector2 to (x, y).")]
        public CustomLogicVector2Builtin(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y)
        {
            _value = new Vector2(x, y);
        }

        public CustomLogicVector2Builtin(Vector2 value)
        {
            _value = value;
        }

        [CLProperty("The X component of the vector.")]
        public float X
        {
            get => _value.x;
            set => _value.x = value;
        }

        [CLProperty("The Y component of the vector.")]
        public float Y
        {
            get => _value.y;
            set => _value.y = value;
        }

        [CLProperty("Returns a normalized copy of this vector (magnitude of 1).")]
        public CustomLogicVector2Builtin Normalized => _value.normalized;

        [CLProperty("Returns the length of this vector.")]
        public float Magnitude => _value.magnitude;

        [CLProperty("Returns the squared length of this vector (faster than Magnitude).")]
        public float SqrMagnitude => _value.sqrMagnitude;

        [CLProperty("Shorthand for writing Vector2(0, 0).")]
        public static CustomLogicVector2Builtin Zero => Vector2.zero;

        [CLProperty("Shorthand for writing Vector2(1, 1).")]
        public static CustomLogicVector2Builtin One => Vector2.one;

        [CLProperty("Shorthand for writing Vector2(0, 1).")]
        public static CustomLogicVector2Builtin Up => Vector2.up;

        [CLProperty("Shorthand for writing Vector2(0, -1).")]
        public static CustomLogicVector2Builtin Down => Vector2.down;

        [CLProperty("Shorthand for writing Vector2(-1, 0).")]
        public static CustomLogicVector2Builtin Left => Vector2.left;

        [CLProperty("Shorthand for writing Vector2(1, 0).")]
        public static CustomLogicVector2Builtin Right => Vector2.right;

        [CLProperty("Shorthand for writing Vector2(float.NegativeInfinity, float.NegativeInfinity).")]
        public static CustomLogicVector2Builtin NegativeInfinity => Vector2.negativeInfinity;

        [CLProperty("Shorthand for writing Vector2(float.PositiveInfinity, float.PositiveInfinity).")]
        public static CustomLogicVector2Builtin PositiveInfinity => Vector2.positiveInfinity;

        [CLMethod("Calculates the angle between two vectors.")]
        public static float Angle(
            [CLParam("The vector from which the angular difference is measured.")]
            CustomLogicVector2Builtin from,
            [CLParam("The vector to which the angular difference is measured.")]
            CustomLogicVector2Builtin to)
            => Vector2.Angle(from, to);

        [CLMethod("Clamps the magnitude of a vector to a maximum value.")]
        public static CustomLogicVector2Builtin ClampMagnitude(
            [CLParam("The vector to clamp.")]
            CustomLogicVector2Builtin vector,
            [CLParam("The maximum length of the vector.")]
            float maxLength)
            => Vector2.ClampMagnitude(vector, maxLength);

        [CLMethod("Calculates the distance between two points.")]
        public static float Distance(
            [CLParam("The first point.")]
            CustomLogicVector2Builtin a,
            [CLParam("The second point.")]
            CustomLogicVector2Builtin b)
            => Vector2.Distance(a, b);

        [CLMethod("Calculates the dot product of two vectors.")]
        public static float Dot(
            [CLParam("The first vector.")]
            CustomLogicVector2Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector2Builtin b)
            => Vector2.Dot(a, b);

        [CLMethod("Linearly interpolates between two vectors.")]
        public static CustomLogicVector2Builtin Lerp(
            [CLParam("The start value.")]
            CustomLogicVector2Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector2Builtin b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Vector2.Lerp(a, b, t);

        [CLMethod("Linearly interpolates between two vectors without clamping.")]
        public static CustomLogicVector2Builtin LerpUnclamped(
            [CLParam("The start value.")]
            CustomLogicVector2Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector2Builtin b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
            => Vector2.LerpUnclamped(a, b, t);

        [CLMethod("Returns a vector that is made from the largest components of two vectors.")]
        public static CustomLogicVector2Builtin Max(
            [CLParam("The first vector.")]
            CustomLogicVector2Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector2Builtin b)
            => Vector2.Max(a, b);

        [CLMethod("Returns a vector that is made from the smallest components of two vectors.")]
        public static CustomLogicVector2Builtin Min(
            [CLParam("The first vector.")]
            CustomLogicVector2Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector2Builtin b)
            => Vector2.Min(a, b);

        [CLMethod("Moves a point towards a target position.")]
        public static CustomLogicVector2Builtin MoveTowards(
            [CLParam("The current position.")]
            CustomLogicVector2Builtin current,
            [CLParam("The target position.")]
            CustomLogicVector2Builtin target,
            [CLParam("The maximum distance to move.")]
            float maxDistanceDelta)
            => Vector2.MoveTowards(current, target, maxDistanceDelta);

        [CLMethod("Reflects a vector off a plane defined by a normal vector.")]
        public static CustomLogicVector2Builtin Reflect(
            [CLParam("The incoming direction vector.")]
            CustomLogicVector2Builtin inDirection,
            [CLParam("The normal vector of the surface.")]
            CustomLogicVector2Builtin inNormal)
            => Vector2.Reflect(inDirection, inNormal);

        [CLMethod("Calculates the signed angle between two vectors.")]
        public static float SignedAngle(
            [CLParam("The vector from which the angular difference is measured.")]
            CustomLogicVector2Builtin from,
            [CLParam("The vector to which the angular difference is measured.")]
            CustomLogicVector2Builtin to)
            => Vector2.SignedAngle(from, to);

        [CLMethod("Smoothly dampens a vector towards a target over time.")]
        public static CustomLogicVector2Builtin SmoothDamp(
            [CLParam("The current position.")]
            CustomLogicVector2Builtin current,
            [CLParam("The target position.")]
            CustomLogicVector2Builtin target,
            [CLParam("The current velocity (modified by the function).")]
            CustomLogicVector2Builtin currentVelocity,
            [CLParam("The time it takes to reach the target (approximately).")]
            float smoothTime,
            [CLParam("The maximum speed.")]
            float maxSpeed)
            => Vector2.SmoothDamp(current, target, ref currentVelocity._value, smoothTime, maxSpeed);

        [CLMethod("Sets the X and Y components of the vector.")]
        public void Set(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y)
            => _value.Set(x, y);

        [CLMethod("Normalizes the vector in place.")]
        public void Normalize() => _value.Normalize();

        public override string ToString()
        {
            return _value.ToString();
        }

        [CLMethod("Creates a copy of this vector. Returns: A new Vector2 with the same values.")]
        public virtual object __Copy__()
        {
            var value = new Vector2(_value.x, _value.y);
            return new CustomLogicVector2Builtin(value);
        }

        [CLMethod("Adds two vectors. Returns: A new vector that is the sum of the two vectors.")]
        public object __Add__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value + b._value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other)
            };
        }

        [CLMethod("Subtracts two vectors. Returns: A new vector that is the difference of the two vectors.")]
        public object __Sub__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value - b._value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other)
            };
        }

        [CLMethod("Multiplies a vector by a scalar or another vector. Returns: A new vector with the multiplied result.")]
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

        [CLMethod("Divides a vector by a scalar or another vector. Returns: A new vector with the divided result.")]
        public object __Div__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => new CustomLogicVector2Builtin(a._value / b._value),
                (CustomLogicVector2Builtin a, float f) => new CustomLogicVector2Builtin(a._value / f),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Div__), self, other)
            };
        }

        [CLMethod("Checks if two vectors are equal. Returns: True if the vectors are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector2Builtin a, CustomLogicVector2Builtin b) => a._value == b._value,
                _ => false
            };
        }

        [CLMethod("Gets the hash code of the vector. Returns: The hash code.")]
        public int __Hash__() => _value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator Vector2(CustomLogicVector2Builtin value) => value._value;
        public static implicit operator CustomLogicVector2Builtin(Vector2 value) => new CustomLogicVector2Builtin(value);
    }
}
