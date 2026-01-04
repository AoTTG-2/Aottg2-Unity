using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Represents a quaternion.
    /// </summary>
    [CLType(Name = "Quaternion", Static = true)]
    partial class CustomLogicQuaternionBuiltin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        public Quaternion Value = Quaternion.identity;

        /// <summary>
        /// Default constructor, creates an identity quaternion (no rotation).
        /// </summary>
        [CLConstructor]
        public CustomLogicQuaternionBuiltin() { }

        /// <summary>
        /// Creates a new Quaternion from the given values.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        [CLConstructor]
        public CustomLogicQuaternionBuiltin(
            float x,
            float y,
            float z,
            float w)
        {
            Value = new Quaternion(x, y, z, w);
        }

        public CustomLogicQuaternionBuiltin(Quaternion value)
        {
            Value = value;
        }

        /// <summary>
        /// The X component of the quaternion.
        /// </summary>
        [CLProperty]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        /// <summary>
        /// The Y component of the quaternion.
        /// </summary>
        [CLProperty]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        /// <summary>
        /// The Z component of the quaternion.
        /// </summary>
        [CLProperty]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        /// <summary>
        /// The W component of the quaternion.
        /// </summary>
        [CLProperty]
        public float W
        {
            get => Value.w;
            set => Value.w = value;
        }

        /// <summary>
        /// Returns or sets the euler angle representation of the rotation.
        /// </summary>
        [CLProperty]
        public CustomLogicVector3Builtin Euler
        {
            get => Value.eulerAngles;
            set => Value = Quaternion.Euler(value);
        }

        /// <summary>
        /// The identity rotation (no rotation).
        /// </summary>
        [CLProperty]
        public static CustomLogicQuaternionBuiltin Identity => Quaternion.identity;

        /// <summary>
        /// Linearly interpolates between two rotations.
        /// </summary>
        /// <param name="a">The start rotation.</param>
        /// <param name="b">The end rotation.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin Lerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Lerp(a, b, t);

        /// <summary>
        /// Linearly interpolates between two rotations without clamping.
        /// </summary>
        /// <param name="a">The start rotation.</param>
        /// <param name="b">The end rotation.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        /// <returns>The interpolated quaternion.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin LerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.LerpUnclamped(a, b, t);

        /// <summary>
        /// Spherically interpolates between two rotations.
        /// </summary>
        /// <param name="a">The start rotation.</param>
        /// <param name="b">The end rotation.</param>
        /// <param name="t">The interpolation factor (clamped between 0 and 1).</param>
        /// <returns>The interpolated quaternion.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin Slerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Slerp(a, b, t);

        /// <summary>
        /// Spherically interpolates between two rotations without clamping.
        /// </summary>
        /// <param name="a">The start rotation.</param>
        /// <param name="b">The end rotation.</param>
        /// <param name="t">The interpolation factor (not clamped).</param>
        /// <returns>The interpolated quaternion.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin SlerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.SlerpUnclamped(a, b, t);

        /// <summary>
        /// Creates a rotation from euler angles.
        /// </summary>
        /// <param name="euler">The euler angles in degrees (x, y, z).</param>
        /// <returns>A quaternion representing the rotation.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin FromEuler(CustomLogicVector3Builtin euler) => Quaternion.Euler(euler);

        /// <summary>
        /// Creates a rotation that looks in the specified direction.
        /// </summary>
        /// <param name="forward">The forward direction vector.</param>
        /// <param name="upwards">Optional. The upwards direction vector (default: Vector3.up).</param>
        /// <returns>A quaternion representing the rotation.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin LookRotation(CustomLogicVector3Builtin forward, CustomLogicVector3Builtin upwards = null)
        {
            if (upwards == null)
                return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward));
            return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward, upwards));
        }

        /// <summary>
        /// Creates a rotation from one direction to another.
        /// </summary>
        /// <param name="a">The source direction vector.</param>
        /// <param name="b">The target direction vector.</param>
        /// <returns>A quaternion representing the rotation.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin FromToRotation(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Quaternion.FromToRotation(a, b);

        /// <summary>
        /// Returns the inverse of a quaternion.
        /// </summary>
        /// <param name="q">The quaternion to invert.</param>
        /// <returns>The inverse quaternion.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin Inverse(CustomLogicQuaternionBuiltin q) => Quaternion.Inverse(q);

        /// <summary>
        /// Rotates a rotation towards a target rotation.
        /// </summary>
        /// <param name="from">The current rotation.</param>
        /// <param name="to">The target rotation.</param>
        /// <param name="maxDegreesDelta">The maximum change in degrees.</param>
        /// <returns>The rotated quaternion.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin RotateTowards(CustomLogicQuaternionBuiltin from, CustomLogicQuaternionBuiltin to, float maxDegreesDelta)
            => Quaternion.RotateTowards(from, to, maxDegreesDelta);

        /// <summary>
        /// Creates a rotation that rotates around a specified axis by a specified angle.
        /// </summary>
        /// <param name="angle">The angle in degrees.</param>
        /// <param name="axis">The axis to rotate around.</param>
        /// <returns>A quaternion representing the rotation.</returns>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin AngleAxis(float angle, CustomLogicVector3Builtin axis) => Quaternion.AngleAxis(angle, axis);

        /// <summary>
        /// Calculates the angle between two rotations.
        /// </summary>
        /// <param name="a">The first rotation.</param>
        /// <param name="b">The second rotation.</param>
        /// <returns>The angle in degrees.</returns>
        [CLMethod]
        public static float Angle(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b) => Quaternion.Angle(a, b);

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Creates a copy of this quaternion.
        /// </summary>
        /// <returns>A new Quaternion with the same values.</returns>
        [CLMethod]
        public object __Copy__()
        {
            var value = new Quaternion(Value.x, Value.y, Value.z, Value.w);
            return new CustomLogicQuaternionBuiltin(value);
        }

        public object __Add__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other);
        public object __Sub__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other);

        /// <summary>
        /// Multiplies two quaternions or a quaternion by a vector.
        /// </summary>
        /// <param name="self">The first quaternion.</param>
        /// <param name="other">The second quaternion or vector.</param>
        /// <returns>A new quaternion or vector with the multiplied result.</returns>
        [CLMethod]
        public object __Mul__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b) => new CustomLogicQuaternionBuiltin(a.Value * b.Value),
                (CustomLogicQuaternionBuiltin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(a.Value * b.Value),
                _ => throw CustomLogicUtils.OperatorException(nameof(__Mul__), self, other)
            };
        }

        public object __Div__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Div__), self, other);

        /// <summary>
        /// Checks if two quaternions are equal.
        /// </summary>
        /// <param name="self">The first quaternion.</param>
        /// <param name="other">The second quaternion.</param>
        /// <returns>True if the quaternions are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            if (other is CustomLogicQuaternionBuiltin quat && self is CustomLogicQuaternionBuiltin quat2)
                return quat2.Value == quat.Value;

            return false;
        }

        /// <summary>
        /// Gets the hash code of the quaternion.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__() => Value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator Quaternion(CustomLogicQuaternionBuiltin q) => q.Value;
        public static implicit operator CustomLogicQuaternionBuiltin(Quaternion q) => new CustomLogicQuaternionBuiltin(q);
    }
}
