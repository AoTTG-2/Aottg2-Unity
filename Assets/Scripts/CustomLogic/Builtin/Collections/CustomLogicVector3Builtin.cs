using System;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Vector3", Static = true)]
    partial class CustomLogicVector3Builtin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        public Vector3 Value;

        /// <summary>
        /// Default constructor, Initializes the Vector3 to (0, 0, 0).
        /// </summary>
        [CLConstructor]
        public CustomLogicVector3Builtin()
        {
            Value = new Vector3();
        }

        /// <summary>
        /// Initializes the Vector3 to (xyz, xyz, xyz).
        /// </summary>
        [CLConstructor]
        public CustomLogicVector3Builtin(float xyz)
        {
            Value = new Vector3(xyz, xyz, xyz);
        }

        /// <summary>
        /// Initializes the Vector3 to (x, y, 0).
        /// </summary>
        [CLConstructor]
        public CustomLogicVector3Builtin(float x, float y)
        {
            Value = new Vector3(x, y, 0);
        }

        /// <summary>
        /// Initializes the Vector3 to (x, y, z).
        /// </summary>
        [CLConstructor]
        public CustomLogicVector3Builtin(float x, float y, float z)
        {
            Value = new Vector3(x, y, z);
        }

        public CustomLogicVector3Builtin(Vector3 value)
        {
            Value = value;
        }

        /// <inheritdoc cref="Vector3.x"/>
        [CLProperty]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        /// <inheritdoc cref="Vector3.y"/>
        [CLProperty]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        /// <inheritdoc cref="Vector3.z"/>
        [CLProperty]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        /// <inheritdoc cref="Vector3.normalized"/>
        [CLProperty] public CustomLogicVector3Builtin Normalized => Value.normalized;

        /// <inheritdoc cref="Vector3.magnitude"/>
        [CLProperty] public float Magnitude => Value.magnitude;

        /// <inheritdoc cref="Vector3.sqrMagnitude"/>
        [CLProperty] public float SqrMagnitude => Value.sqrMagnitude;

        /// <inheritdoc cref="Vector3.zero"/>
        [CLProperty] public static CustomLogicVector3Builtin Zero => Vector3.zero;

        /// <inheritdoc cref="Vector3.one"/>
        [CLProperty] public static CustomLogicVector3Builtin One => Vector3.one;

        /// <inheritdoc cref="Vector3.up"/>
        [CLProperty] public static CustomLogicVector3Builtin Up => Vector3.up;

        /// <inheritdoc cref="Vector3.down"/>
        [CLProperty] public static CustomLogicVector3Builtin Down => Vector3.down;

        /// <inheritdoc cref="Vector3.left"/>
        [CLProperty] public static CustomLogicVector3Builtin Left => Vector3.left;

        /// <inheritdoc cref="Vector3.right"/>
        [CLProperty] public static CustomLogicVector3Builtin Right => Vector3.right;

        /// <inheritdoc cref="Vector3.forward"/>
        [CLProperty] public static CustomLogicVector3Builtin Forward => Vector3.forward;

        /// <inheritdoc cref="Vector3.back"/>
        [CLProperty] public static CustomLogicVector3Builtin Back => Vector3.back;

        /// <inheritdoc cref="Vector3.positiveInfinity"/>
        [CLProperty] public static CustomLogicVector3Builtin NegativeInfinity => Vector3.negativeInfinity;

        /// <inheritdoc cref="Vector3.positiveInfinity"/>
        [CLProperty] public static CustomLogicVector3Builtin PositiveInfinity => Vector3.positiveInfinity;

        /// <inheritdoc cref="Vector3.Angle"/>
        [CLMethod]
        public static float Angle(CustomLogicVector3Builtin from, CustomLogicVector3Builtin to) => Vector3.Angle(from.Value, to.Value);

        /// <inheritdoc cref="Vector3.ClampMagnitude"/>
        [CLMethod]
        public static CustomLogicVector3Builtin ClampMagnitude(CustomLogicVector3Builtin vector, float maxLength) => Vector3.ClampMagnitude(vector, maxLength);

        /// <inheritdoc cref="Vector3.Cross"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Cross(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Cross(a, b);

        /// <inheritdoc cref="Vector3.Distance"/>
        [CLMethod]
        public static float Distance(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Distance(a, b);

        /// <inheritdoc cref="Vector3.Dot"/>
        [CLMethod]
        public static float Dot(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Dot(a, b);

        /// <inheritdoc cref="Vector3.Lerp"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Lerp(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t) => Vector3.Lerp(a, b, t);

        /// <inheritdoc cref="Vector3.LerpUnclamped"/>
        [CLMethod]
        public static CustomLogicVector3Builtin LerpUnclamped(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t) => Vector3.LerpUnclamped(a, b, t);

        /// <inheritdoc cref="Vector3.Max"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Max(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Max(a, b);

        /// <inheritdoc cref="Vector3.Min"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Min(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Min(a, b);

        /// <inheritdoc cref="Vector3.MoveTowards"/>
        [CLMethod]
        public static CustomLogicVector3Builtin MoveTowards(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, float maxDistanceDelta) => Vector3.MoveTowards(current, target, maxDistanceDelta);

        /// <inheritdoc cref="Vector3.Normalize(Vector3)"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Normalize(CustomLogicVector3Builtin value) => Vector3.Normalize(value);

        /// <summary>
        /// Makes vectors normalized and orthogonal to each other.
        /// Normalizes normal. Normalizes tangent and makes sure it is orthogonal to normal (that is, angle between them is 90 degrees).
        /// Honestly just go look up the unity docs for this one idk.
        /// This one uses references so the Vectors will be modified in place.
        /// </summary>
        [CLMethod]
        public static void OrthoNormalize(CustomLogicVector3Builtin normal, CustomLogicVector3Builtin tangent) => Vector3.OrthoNormalize(ref normal.Value, ref tangent.Value);

        /// <inheritdoc cref="Vector3.Project"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Project(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Vector3.Project(a, b);

        /// <inheritdoc cref="Vector3.ProjectOnPlane"/>
        [CLMethod]
        public static CustomLogicVector3Builtin ProjectOnPlane(CustomLogicVector3Builtin vector, CustomLogicVector3Builtin plane) => Vector3.ProjectOnPlane(vector, plane);

        /// <inheritdoc cref="Vector3.Reflect"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Reflect(CustomLogicVector3Builtin inDirection, CustomLogicVector3Builtin inNormal) => Vector3.Reflect(inDirection, inNormal);

        /// <inheritdoc cref="Vector3.RotateTowards"/>
        [CLMethod]
        public static CustomLogicVector3Builtin RotateTowards(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, float maxRadiansDelta, float maxMagnitudeDelta) => Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta);

        /// <inheritdoc cref="Vector3.SignedAngle"/>
        [CLMethod]
        public static float SignedAngle(CustomLogicVector3Builtin from, CustomLogicVector3Builtin to, CustomLogicVector3Builtin axis) => Vector3.SignedAngle(from, to, axis);

        /// <inheritdoc cref="Vector3.Slerp"/>
        [CLMethod]
        public static CustomLogicVector3Builtin Slerp(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t) => Vector3.Slerp(a, b, t);

        /// <inheritdoc cref="Vector3.SlerpUnclamped"/>
        [CLMethod]
        public static CustomLogicVector3Builtin SlerpUnclamped(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t) => Vector3.SlerpUnclamped(a, b, t);

        /// <summary>
        /// Smoothly damps current towards target with currentVelocity as state. smoothTime and maxSpeed control how aggressive the transition is.
        /// </summary>
        [CLMethod]
        public static CustomLogicVector3Builtin SmoothDamp(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, CustomLogicVector3Builtin currentVelocity, float smoothTime, float maxSpeed) => Vector3.SmoothDamp(current, target, ref currentVelocity.Value, smoothTime, maxSpeed);

        /// <inheritdoc cref="Vector3.Set"/>
        [CLMethod] public void Set(float x, float y, float z) => Value = new Vector3(x, y, z);

        /// <summary>
        /// Returns the Vector3 multiplied by scale.
        /// </summary>
        /// <param name="scale">float | Vector3</param>
        [CLMethod, Obsolete("Use multiply operator instead")]
        public CustomLogicVector3Builtin Scale(object scale)
        {
            if (scale is int iScale)
                return new CustomLogicVector3Builtin(Value * iScale);
            if (scale is float fScale)
                return new CustomLogicVector3Builtin(Value * fScale);
            if (scale is CustomLogicVector3Builtin v3Scale)
            {
                Value.Scale(v3Scale);
                return new CustomLogicVector3Builtin(Value);
            }

            throw new Exception("Parameter must be a float or a Vector3.");
        }

        /// <summary>
        /// Returns the multiplication of two Vector3s.
        /// </summary>
        /// <param name="a">Vector3</param>
        /// <param name="b">Vector3</param>
        /// <returns>Vector3</returns>
        [CLMethod, Obsolete("Use multiply operator instead")]
        public CustomLogicVector3Builtin Multiply(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.MultiplyVectors(a, b));
        }

        /// <summary>
        /// Returns the division of two Vector3s.
        /// </summary>
        /// <param name="a">Vector3</param>
        /// <param name="b">Vector3</param>
        /// <returns>Vector3</returns>
        [CLMethod, Obsolete("Use divide operator instead")]
        public CustomLogicVector3Builtin Divide(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.DivideVectors(a, b));
        }

        /// <summary>
        /// Gets the relational Vector3 "b" using "a" as a reference. This is equivalent to setting MapObject.Forward to Vector "a", and finding the relative "b" vector.
        /// </summary>
        [CLMethod]
        public CustomLogicVector3Builtin GetRotationDirection(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            var direction = Quaternion.Euler(a) * b;
            return new CustomLogicVector3Builtin(direction);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        [CLMethod]
        public object __Copy__()
        {
            var value = new Vector3(Value.x, Value.y, Value.z);
            return new CustomLogicVector3Builtin(value);
        }

        [CLMethod]
        public object __Add__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value + b.Value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other)
            };
        }

        [CLMethod]
        public object __Sub__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value - b.Value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other)
            };
        }

        [CLMethod]
        public object __Mul__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin v3, float f) => new CustomLogicVector3Builtin(v3.Value * f),
                (float f, CustomLogicVector3Builtin v3) => new CustomLogicVector3Builtin(v3.Value * f),
                (CustomLogicVector3Builtin v3, int f) => new CustomLogicVector3Builtin(v3.Value * f),
                (int f, CustomLogicVector3Builtin v3) => new CustomLogicVector3Builtin(v3.Value * f),
                (CustomLogicVector3Builtin v3, CustomLogicVector3Builtin v3Other) => new CustomLogicVector3Builtin(Util.MultiplyVectors(v3, v3Other)),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Mul__), self, other)
            };
        }

        [CLMethod]
        public object __Div__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin v3, float f) => new CustomLogicVector3Builtin(v3.Value / f),
                (CustomLogicVector3Builtin v3, int f) => new CustomLogicVector3Builtin(v3.Value / f),
                (CustomLogicVector3Builtin v3, CustomLogicVector3Builtin v3Other) => new CustomLogicVector3Builtin(Util.DivideVectors(v3, v3Other)),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Div__), self, other)
            };
        }

        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            if (other is not CustomLogicVector3Builtin v3)
                return false;

            return Value == v3.Value;
        }

        [CLMethod] public int __Hash__() => Value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Vector3(CustomLogicVector3Builtin v) => v.Value;
        public static implicit operator CustomLogicVector3Builtin(Vector3 v) => new CustomLogicVector3Builtin(v);
    }
}
