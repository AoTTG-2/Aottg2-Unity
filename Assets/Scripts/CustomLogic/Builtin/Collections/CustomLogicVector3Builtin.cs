using System;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Represents a 3D vector with X, Y, and Z components. Supports mathematical operations and implements copy semantics.
    /// </summary>
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
        /// <param name="xyz">The value for X, Y, and Z components.</param>
        [CLConstructor]
        public CustomLogicVector3Builtin(float xyz)
        {
            Value = new Vector3(xyz, xyz, xyz);
        }

        /// <summary>
        /// Initializes the Vector3 to (x, y, 0).
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        [CLConstructor]
        public CustomLogicVector3Builtin(float x, float y)
        {
            Value = new Vector3(x, y, 0);
        }

        /// <summary>
        /// Initializes the Vector3 to (x, y, z).
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        [CLConstructor]
        public CustomLogicVector3Builtin(float x, float y, float z)
        {
            Value = new Vector3(x, y, z);
        }

        public CustomLogicVector3Builtin(Vector3 value)
        {
            Value = value;
        }

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        [CLProperty]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        [CLProperty]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        [CLProperty]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        /// <summary>
        /// Returns a normalized copy of this vector (magnitude of 1).
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Normalized => Value.normalized;

        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        [CLProperty]
        public float Magnitude => Value.magnitude;

        /// <summary>
        /// Returns the squared length of this vector (faster than Magnitude).
        /// </summary>
        [CLProperty]
        public float SqrMagnitude => Value.sqrMagnitude;

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Zero => Vector3.zero;

        /// <summary>
        /// Shorthand for writing Vector3(1, 1, 1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin One => Vector3.one;

        /// <summary>
        /// Shorthand for writing Vector3(0, 1, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Up => Vector3.up;

        /// <summary>
        /// Shorthand for writing Vector3(0, -1, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Down => Vector3.down;

        /// <summary>
        /// Shorthand for writing Vector3(-1, 0, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Left => Vector3.left;

        /// <summary>
        /// Shorthand for writing Vector3(1, 0, 0).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Right => Vector3.right;

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, 1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Forward => Vector3.forward;

        /// <summary>
        /// Shorthand for writing Vector3(0, 0, -1).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin Back => Vector3.back;

        /// <summary>
        /// Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin NegativeInfinity => Vector3.negativeInfinity;

        /// <summary>
        /// Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).
        /// </summary>
        [CLProperty]
        public static CustomLogicVector3Builtin PositiveInfinity => Vector3.positiveInfinity;

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        [CLMethod]
        public static float Angle(CustomLogicVector3Builtin from, CustomLogicVector3Builtin to)
            => Vector3.Angle(from.Value, to.Value);

        /// <summary>
        /// Clamps the magnitude of a vector to a maximum value.
        /// </summary>
        /// <param name="vector">The vector to clamp.</param>
        /// <param name="maxLength">The maximum length of the vector.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin ClampMagnitude(CustomLogicVector3Builtin vector, float maxLength)
            => Vector3.ClampMagnitude(vector, maxLength);

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Cross(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Cross(a, b);

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        [CLMethod]
        public static float Distance(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Distance(a, b);

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static float Dot(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Dot(a, b);

        /// <summary>
        /// Linearly interpolates between two vectors.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Lerp(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t)
            => Vector3.Lerp(a, b, t);

        /// <summary>
        /// Linearly interpolates between two vectors without clamping.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        [CLMethod]
        public static CustomLogicVector3Builtin LerpUnclamped(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t)
            => Vector3.LerpUnclamped(a, b, t);

        /// <summary>
        /// Returns a vector that is made from the largest components of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Max(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Max(a, b);

        /// <summary>
        /// Returns a vector that is made from the smallest components of two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Min(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Min(a, b);

        /// <summary>
        /// Moves a point towards a target position.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="maxDistanceDelta">The maximum distance to move.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin MoveTowards(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, float maxDistanceDelta)
            => Vector3.MoveTowards(current, target, maxDistanceDelta);

        /// <summary>
        /// Returns a normalized copy of the vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Normalize(CustomLogicVector3Builtin value)
            => Vector3.Normalize(value);

        /// <summary>
        /// Orthonormalizes two vectors (normalizes the normal vector and makes the tangent vector orthogonal to it).
        /// </summary>
        /// <param name="normal">The normal vector (will be normalized).</param>
        /// <param name="tangent">The tangent vector (will be normalized and made orthogonal to normal).</param>
        [CLMethod]
        public static void OrthoNormalize(CustomLogicVector3Builtin normal, CustomLogicVector3Builtin tangent)
            => Vector3.OrthoNormalize(ref normal.Value, ref tangent.Value);

        /// <summary>
        /// Projects a vector onto another vector.
        /// </summary>
        /// <param name="a">The vector to project.</param>
        /// <param name="b">The vector to project onto.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Project(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
            => Vector3.Project(a, b);

        /// <summary>
        /// Projects a vector onto a plane defined by a normal vector.
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="plane">The plane normal vector.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin ProjectOnPlane(CustomLogicVector3Builtin vector, CustomLogicVector3Builtin plane)
            => Vector3.ProjectOnPlane(vector, plane);

        /// <summary>
        /// Reflects a vector off a plane defined by a normal vector.
        /// </summary>
        /// <param name="inDirection">The incoming direction vector.</param>
        /// <param name="inNormal">The normal vector of the surface.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Reflect(CustomLogicVector3Builtin inDirection, CustomLogicVector3Builtin inNormal)
            => Vector3.Reflect(inDirection, inNormal);

        /// <summary>
        /// Rotates a vector towards a target vector.
        /// </summary>
        /// <param name="current">The current direction vector.</param>
        /// <param name="target">The target direction vector.</param>
        /// <param name="maxRadiansDelta">The maximum change in radians.</param>
        /// <param name="maxMagnitudeDelta">The maximum change in magnitude.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin RotateTowards(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, float maxRadiansDelta, float maxMagnitudeDelta)
            => Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta);

        /// <summary>
        /// Calculates the signed angle between two vectors.
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <param name="axis">The axis around which the rotation is measured.</param>
        [CLMethod]
        public static float SignedAngle(CustomLogicVector3Builtin from, CustomLogicVector3Builtin to, CustomLogicVector3Builtin axis)
            => Vector3.SignedAngle(from, to, axis);

        /// <summary>
        /// Spherically interpolates between two vectors.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        [CLMethod]
        public static CustomLogicVector3Builtin Slerp(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t)
            => Vector3.Slerp(a, b, t);

        /// <summary>
        /// Spherically interpolates between two vectors without clamping.
        /// </summary>
        /// <param name="a">The start value.</param>
        /// <param name="b">The end value.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        [CLMethod]
        public static CustomLogicVector3Builtin SlerpUnclamped(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b, float t)
            => Vector3.SlerpUnclamped(a, b, t);

        /// <summary>
        /// Smoothly dampens a vector towards a target over time.
        /// </summary>
        /// <param name="current">The current position.</param>
        /// <param name="target">The target position.</param>
        /// <param name="currentVelocity">The current velocity (modified by the function).</param>
        /// <param name="smoothTime">The time it takes to reach the target (approximately).</param>
        /// <param name="maxSpeed">The maximum speed.</param>
        [CLMethod]
        public static CustomLogicVector3Builtin SmoothDamp(CustomLogicVector3Builtin current, CustomLogicVector3Builtin target, CustomLogicVector3Builtin currentVelocity, float smoothTime, float maxSpeed)
            => Vector3.SmoothDamp(current, target, ref currentVelocity.Value, smoothTime, maxSpeed);

        /// <summary>
        /// Sets the X, Y, and Z components of the vector.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        [CLMethod]
        public void Set(float x, float y, float z)
            => Value = new Vector3(x, y, z);

        /// <summary>
        /// Scales the vector by a float or Vector3.
        /// </summary>
        /// <param name="scale">The scale value (float or Vector3).</param>
        /// <returns>A new scaled vector.</returns>
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
        /// Multiplies two vectors component-wise.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>A new vector with multiplied components.</returns>
        [CLMethod, Obsolete("Use multiply operator instead")]
        public static CustomLogicVector3Builtin Multiply(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.MultiplyVectors(a, b));
        }

        /// <summary>
        /// Divides two vectors component-wise.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>A new vector with divided components.</returns>
        [CLMethod, Obsolete("Use divide operator instead")]
        public static CustomLogicVector3Builtin Divide(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.DivideVectors(a, b));
        }

        /// <summary>
        /// Gets the direction vector transformed by a rotation.
        /// </summary>
        /// <param name="a">The reference rotation vector (e.g., forward direction).</param>
        /// <param name="b">The vector to transform relative to the reference.</param>
        /// <returns>A new direction vector.</returns>
        [CLMethod]
        public static CustomLogicVector3Builtin GetRotationDirection(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b)
        {
            var direction = Quaternion.Euler(a) * b;
            return new CustomLogicVector3Builtin(direction);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Creates a copy of this vector.
        /// </summary>
        /// <returns>A new Vector3 with the same values.</returns>
        [CLMethod]
        public object __Copy__()
        {
            var value = new Vector3(Value.x, Value.y, Value.z);
            return new CustomLogicVector3Builtin(value);
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
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value + b.Value),
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
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value - b.Value),
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
                (CustomLogicVector3Builtin v3, float f) => new CustomLogicVector3Builtin(v3.Value * f),
                (float f, CustomLogicVector3Builtin v3) => new CustomLogicVector3Builtin(v3.Value * f),
                (CustomLogicVector3Builtin v3, int f) => new CustomLogicVector3Builtin(v3.Value * f),
                (int f, CustomLogicVector3Builtin v3) => new CustomLogicVector3Builtin(v3.Value * f),
                (CustomLogicVector3Builtin v3, CustomLogicVector3Builtin v3Other) => new CustomLogicVector3Builtin(Util.MultiplyVectors(v3, v3Other)),
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
                (CustomLogicVector3Builtin v3, float f) => new CustomLogicVector3Builtin(v3.Value / f),
                (CustomLogicVector3Builtin v3, int f) => new CustomLogicVector3Builtin(v3.Value / f),
                (CustomLogicVector3Builtin v3, CustomLogicVector3Builtin v3Other) => new CustomLogicVector3Builtin(Util.DivideVectors(v3, v3Other)),
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
            if (other is not CustomLogicVector3Builtin v3)
                return false;

            return Value == v3.Value;
        }

        /// <summary>
        /// Gets the hash code of the vector.
        /// </summary>
        [CLMethod]
        public int __Hash__() => Value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Vector3(CustomLogicVector3Builtin v) => v.Value;
        public static implicit operator CustomLogicVector3Builtin(Vector3 v) => new CustomLogicVector3Builtin(v);
    }
}
