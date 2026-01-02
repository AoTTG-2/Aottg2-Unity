using System;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Vector3", Static = true)]
    partial class CustomLogicVector3Builtin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        public Vector3 Value;

        [CLConstructor("Default constructor, Initializes the Vector3 to (0, 0, 0).")]
        public CustomLogicVector3Builtin()
        {
            Value = new Vector3();
        }

        [CLConstructor("Initializes the Vector3 to (xyz, xyz, xyz).")]
        public CustomLogicVector3Builtin(
            [CLParam("The value for X, Y, and Z components.")]
            float xyz)
        {
            Value = new Vector3(xyz, xyz, xyz);
        }

        [CLConstructor("Initializes the Vector3 to (x, y, 0).")]
        public CustomLogicVector3Builtin(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y)
        {
            Value = new Vector3(x, y, 0);
        }

        [CLConstructor("Initializes the Vector3 to (x, y, z).")]
        public CustomLogicVector3Builtin(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y,
            [CLParam("The Z component.")]
            float z)
        {
            Value = new Vector3(x, y, z);
        }

        public CustomLogicVector3Builtin(Vector3 value)
        {
            Value = value;
        }

        [CLProperty("The X component of the vector.")]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        [CLProperty("The Y component of the vector.")]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        [CLProperty("The Z component of the vector.")]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        [CLProperty("Returns a normalized copy of this vector (magnitude of 1).")]
        public CustomLogicVector3Builtin Normalized => Value.normalized;

        [CLProperty("Returns the length of this vector.")]
        public float Magnitude => Value.magnitude;

        [CLProperty("Returns the squared length of this vector (faster than Magnitude).")]
        public float SqrMagnitude => Value.sqrMagnitude;

        [CLProperty("Shorthand for writing Vector3(0, 0, 0).")]
        public static CustomLogicVector3Builtin Zero => Vector3.zero;

        [CLProperty("Shorthand for writing Vector3(1, 1, 1).")]
        public static CustomLogicVector3Builtin One => Vector3.one;

        [CLProperty("Shorthand for writing Vector3(0, 1, 0).")]
        public static CustomLogicVector3Builtin Up => Vector3.up;

        [CLProperty("Shorthand for writing Vector3(0, -1, 0).")]
        public static CustomLogicVector3Builtin Down => Vector3.down;

        [CLProperty("Shorthand for writing Vector3(-1, 0, 0).")]
        public static CustomLogicVector3Builtin Left => Vector3.left;

        [CLProperty("Shorthand for writing Vector3(1, 0, 0).")]
        public static CustomLogicVector3Builtin Right => Vector3.right;

        [CLProperty("Shorthand for writing Vector3(0, 0, 1).")]
        public static CustomLogicVector3Builtin Forward => Vector3.forward;

        [CLProperty("Shorthand for writing Vector3(0, 0, -1).")]
        public static CustomLogicVector3Builtin Back => Vector3.back;

        [CLProperty("Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).")]
        public static CustomLogicVector3Builtin NegativeInfinity => Vector3.negativeInfinity;

        [CLProperty("Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).")]
        public static CustomLogicVector3Builtin PositiveInfinity => Vector3.positiveInfinity;

        [CLMethod("Calculates the angle between two vectors.")]
        public static float Angle(
            [CLParam("The vector from which the angular difference is measured.")]
            CustomLogicVector3Builtin from,
            [CLParam("The vector to which the angular difference is measured.")]
            CustomLogicVector3Builtin to)
            => Vector3.Angle(from.Value, to.Value);

        [CLMethod("Clamps the magnitude of a vector to a maximum value.")]
        public static CustomLogicVector3Builtin ClampMagnitude(
            [CLParam("The vector to clamp.")]
            CustomLogicVector3Builtin vector,
            [CLParam("The maximum length of the vector.")]
            float maxLength)
            => Vector3.ClampMagnitude(vector, maxLength);

        [CLMethod("Calculates the cross product of two vectors.")]
        public static CustomLogicVector3Builtin Cross(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
            => Vector3.Cross(a, b);

        [CLMethod("Calculates the distance between two points.")]
        public static float Distance(
            [CLParam("The first point.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second point.")]
            CustomLogicVector3Builtin b)
            => Vector3.Distance(a, b);

        [CLMethod("Calculates the dot product of two vectors.")]
        public static float Dot(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
            => Vector3.Dot(a, b);

        [CLMethod("Linearly interpolates between two vectors.")]
        public static CustomLogicVector3Builtin Lerp(
            [CLParam("The start value.")]
            CustomLogicVector3Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector3Builtin b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Vector3.Lerp(a, b, t);

        [CLMethod("Linearly interpolates between two vectors without clamping.")]
        public static CustomLogicVector3Builtin LerpUnclamped(
            [CLParam("The start value.")]
            CustomLogicVector3Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector3Builtin b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
            => Vector3.LerpUnclamped(a, b, t);

        [CLMethod("Returns a vector that is made from the largest components of two vectors.")]
        public static CustomLogicVector3Builtin Max(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
            => Vector3.Max(a, b);

        [CLMethod("Returns a vector that is made from the smallest components of two vectors.")]
        public static CustomLogicVector3Builtin Min(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
            => Vector3.Min(a, b);

        [CLMethod("Moves a point towards a target position.")]
        public static CustomLogicVector3Builtin MoveTowards(
            [CLParam("The current position.")]
            CustomLogicVector3Builtin current,
            [CLParam("The target position.")]
            CustomLogicVector3Builtin target,
            [CLParam("The maximum distance to move.")]
            float maxDistanceDelta)
            => Vector3.MoveTowards(current, target, maxDistanceDelta);

        [CLMethod("Returns a normalized copy of the vector.")]
        public static CustomLogicVector3Builtin Normalize(
            [CLParam("The vector to normalize.")]
            CustomLogicVector3Builtin value)
            => Vector3.Normalize(value);

        [CLMethod("Orthonormalizes two vectors (normalizes the normal vector and makes the tangent vector orthogonal to it).")]
        public static void OrthoNormalize(
            [CLParam("The normal vector (will be normalized).")]
            CustomLogicVector3Builtin normal,
            [CLParam("The tangent vector (will be normalized and made orthogonal to normal).")]
            CustomLogicVector3Builtin tangent)
            => Vector3.OrthoNormalize(ref normal.Value, ref tangent.Value);

        [CLMethod("Projects a vector onto another vector.")]
        public static CustomLogicVector3Builtin Project(
            [CLParam("The vector to project.")]
            CustomLogicVector3Builtin a,
            [CLParam("The vector to project onto.")]
            CustomLogicVector3Builtin b)
            => Vector3.Project(a, b);

        [CLMethod("Projects a vector onto a plane defined by a normal vector.")]
        public static CustomLogicVector3Builtin ProjectOnPlane(
            [CLParam("The vector to project.")]
            CustomLogicVector3Builtin vector,
            [CLParam("The plane normal vector.")]
            CustomLogicVector3Builtin plane)
            => Vector3.ProjectOnPlane(vector, plane);

        [CLMethod("Reflects a vector off a plane defined by a normal vector.")]
        public static CustomLogicVector3Builtin Reflect(
            [CLParam("The incoming direction vector.")]
            CustomLogicVector3Builtin inDirection,
            [CLParam("The normal vector of the surface.")]
            CustomLogicVector3Builtin inNormal)
            => Vector3.Reflect(inDirection, inNormal);

        [CLMethod("Rotates a vector towards a target vector.")]
        public static CustomLogicVector3Builtin RotateTowards(
            [CLParam("The current direction vector.")]
            CustomLogicVector3Builtin current,
            [CLParam("The target direction vector.")]
            CustomLogicVector3Builtin target,
            [CLParam("The maximum change in radians.")]
            float maxRadiansDelta,
            [CLParam("The maximum change in magnitude.")]
            float maxMagnitudeDelta)
            => Vector3.RotateTowards(current, target, maxRadiansDelta, maxMagnitudeDelta);

        [CLMethod("Calculates the signed angle between two vectors.")]
        public static float SignedAngle(
            [CLParam("The vector from which the angular difference is measured.")]
            CustomLogicVector3Builtin from,
            [CLParam("The vector to which the angular difference is measured.")]
            CustomLogicVector3Builtin to,
            [CLParam("The axis around which the rotation is measured.")]
            CustomLogicVector3Builtin axis)
            => Vector3.SignedAngle(from, to, axis);

        [CLMethod("Spherically interpolates between two vectors.")]
        public static CustomLogicVector3Builtin Slerp(
            [CLParam("The start value.")]
            CustomLogicVector3Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector3Builtin b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Vector3.Slerp(a, b, t);

        [CLMethod("Spherically interpolates between two vectors without clamping.")]
        public static CustomLogicVector3Builtin SlerpUnclamped(
            [CLParam("The start value.")]
            CustomLogicVector3Builtin a,
            [CLParam("The end value.")]
            CustomLogicVector3Builtin b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
            => Vector3.SlerpUnclamped(a, b, t);

        [CLMethod("Smoothly dampens a vector towards a target over time.")]
        public static CustomLogicVector3Builtin SmoothDamp(
            [CLParam("The current position.")]
            CustomLogicVector3Builtin current,
            [CLParam("The target position.")]
            CustomLogicVector3Builtin target,
            [CLParam("The current velocity (modified by the function).")]
            CustomLogicVector3Builtin currentVelocity,
            [CLParam("The time it takes to reach the target (approximately).")]
            float smoothTime,
            [CLParam("The maximum speed.")]
            float maxSpeed)
            => Vector3.SmoothDamp(current, target, ref currentVelocity.Value, smoothTime, maxSpeed);

        [CLMethod("Sets the X, Y, and Z components of the vector.")]
        public void Set(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y,
            [CLParam("The Z component.")]
            float z)
            => Value = new Vector3(x, y, z);

        [CLMethod("Scales the vector by a float or Vector3. Returns: A new scaled vector."), Obsolete("Use multiply operator instead")]
        public CustomLogicVector3Builtin Scale(
            [CLParam("The scale value (float or Vector3).")]
            object scale)
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

        [CLMethod("Multiplies two vectors component-wise. Returns: A new vector with multiplied components."), Obsolete("Use multiply operator instead")]
        public static CustomLogicVector3Builtin Multiply(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.MultiplyVectors(a, b));
        }

        [CLMethod("Divides two vectors component-wise. Returns: A new vector with divided components."), Obsolete("Use divide operator instead")]
        public static CustomLogicVector3Builtin Divide(
            [CLParam("The first vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The second vector.")]
            CustomLogicVector3Builtin b)
        {
            return new CustomLogicVector3Builtin(Util.DivideVectors(a, b));
        }

        [CLMethod("Gets the direction vector transformed by a rotation. Returns: A new direction vector.")]
        public static CustomLogicVector3Builtin GetRotationDirection(
            [CLParam("The reference rotation vector (e.g., forward direction).")]
            CustomLogicVector3Builtin a,
            [CLParam("The vector to transform relative to the reference.")]
            CustomLogicVector3Builtin b)
        {
            var direction = Quaternion.Euler(a) * b;
            return new CustomLogicVector3Builtin(direction);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        [CLMethod("Creates a copy of this vector. Returns: A new Vector3 with the same values.")]
        public object __Copy__()
        {
            var value = new Vector3(Value.x, Value.y, Value.z);
            return new CustomLogicVector3Builtin(value);
        }

        [CLMethod("Adds two vectors. Returns: A new vector that is the sum of the two vectors.")]
        public object __Add__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value + b.Value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other)
            };
        }

        [CLMethod("Subtracts two vectors. Returns: A new vector that is the difference of the two vectors.")]
        public object __Sub__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value - b.Value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other)
            };
        }

        [CLMethod("Multiplies a vector by a scalar or another vector. Returns: A new vector with the multiplied result.")]
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

        [CLMethod("Divides a vector by a scalar or another vector. Returns: A new vector with the divided result.")]
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

        [CLMethod("Checks if two vectors are equal. Returns: True if the vectors are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            if (other is not CustomLogicVector3Builtin v3)
                return false;

            return Value == v3.Value;
        }

        [CLMethod("Gets the hash code of the vector.")]
        public int __Hash__() => Value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Vector3(CustomLogicVector3Builtin v) => v.Value;
        public static implicit operator CustomLogicVector3Builtin(Vector3 v) => new CustomLogicVector3Builtin(v);
    }
}
