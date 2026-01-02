using UnityEngine;

namespace CustomLogic
{
    [CLType(Name = "Quaternion", Static = true, Description = "Represents a quaternion.")]
    partial class CustomLogicQuaternionBuiltin : BuiltinClassInstance, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        public Quaternion Value = Quaternion.identity;

        [CLConstructor("Default constructor, creates an identity quaternion (no rotation).")]
        public CustomLogicQuaternionBuiltin() { }

        [CLConstructor("Creates a new Quaternion from the given values.")]
        public CustomLogicQuaternionBuiltin(
            [CLParam("The X component.")]
            float x,
            [CLParam("The Y component.")]
            float y,
            [CLParam("The Z component.")]
            float z,
            [CLParam("The W component.")]
            float w)
        {
            Value = new Quaternion(x, y, z, w);
        }

        public CustomLogicQuaternionBuiltin(Quaternion value)
        {
            Value = value;
        }

        [CLProperty("The X component of the quaternion.")]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        [CLProperty("The Y component of the quaternion.")]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        [CLProperty("The Z component of the quaternion.")]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        [CLProperty("The W component of the quaternion.")]
        public float W
        {
            get => Value.w;
            set => Value.w = value;
        }

        [CLProperty("Returns or sets the euler angle representation of the rotation.")]
        public CustomLogicVector3Builtin Euler
        {
            get => Value.eulerAngles;
            set => Value = Quaternion.Euler(value);
        }

        [CLProperty("The identity rotation (no rotation).")]
        public static CustomLogicQuaternionBuiltin Identity => Quaternion.identity;

        [CLMethod("Linearly interpolates between two rotations.")]
        public static CustomLogicQuaternionBuiltin Lerp(
            [CLParam("The start rotation.")]
            CustomLogicQuaternionBuiltin a,
            [CLParam("The end rotation.")]
            CustomLogicQuaternionBuiltin b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Quaternion.Lerp(a, b, t);

        [CLMethod("Linearly interpolates between two rotations without clamping.")]
        public static CustomLogicQuaternionBuiltin LerpUnclamped(
            [CLParam("The start rotation.")]
            CustomLogicQuaternionBuiltin a,
            [CLParam("The end rotation.")]
            CustomLogicQuaternionBuiltin b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
            => Quaternion.LerpUnclamped(a, b, t);

        [CLMethod("Spherically interpolates between two rotations.")]
        public static CustomLogicQuaternionBuiltin Slerp(
            [CLParam("The start rotation.")]
            CustomLogicQuaternionBuiltin a,
            [CLParam("The end rotation.")]
            CustomLogicQuaternionBuiltin b,
            [CLParam("The interpolation factor (clamped between 0 and 1).")]
            float t)
            => Quaternion.Slerp(a, b, t);

        [CLMethod("Spherically interpolates between two rotations without clamping.")]
        public static CustomLogicQuaternionBuiltin SlerpUnclamped(
            [CLParam("The start rotation.")]
            CustomLogicQuaternionBuiltin a,
            [CLParam("The end rotation.")]
            CustomLogicQuaternionBuiltin b,
            [CLParam("The interpolation factor (not clamped).")]
            float t)
            => Quaternion.SlerpUnclamped(a, b, t);

        [CLMethod("Creates a rotation from euler angles.")]
        public static CustomLogicQuaternionBuiltin FromEuler(
            [CLParam("The euler angles in degrees (x, y, z).")]
            CustomLogicVector3Builtin euler)
            => Quaternion.Euler(euler);

        [CLMethod("Creates a rotation that looks in the specified direction.")]
        public static CustomLogicQuaternionBuiltin LookRotation(
            [CLParam("The forward direction vector.")]
            CustomLogicVector3Builtin forward,
            [CLParam("Optional. The upwards direction vector (default: Vector3.up).")]
            CustomLogicVector3Builtin upwards = null)
        {
            if (upwards == null)
                return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward));
            return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward, upwards));
        }

        [CLMethod("Creates a rotation from one direction to another.")]
        public static CustomLogicQuaternionBuiltin FromToRotation(
            [CLParam("The source direction vector.")]
            CustomLogicVector3Builtin a,
            [CLParam("The target direction vector.")]
            CustomLogicVector3Builtin b)
            => Quaternion.FromToRotation(a, b);

        [CLMethod("Returns the inverse of a quaternion.")]
        public static CustomLogicQuaternionBuiltin Inverse(
            [CLParam("The quaternion to invert.")]
            CustomLogicQuaternionBuiltin q)
            => Quaternion.Inverse(q);

        [CLMethod("Rotates a rotation towards a target rotation.")]
        public static CustomLogicQuaternionBuiltin RotateTowards(
            [CLParam("The current rotation.")]
            CustomLogicQuaternionBuiltin from,
            [CLParam("The target rotation.")]
            CustomLogicQuaternionBuiltin to,
            [CLParam("The maximum change in degrees.")]
            float maxDegreesDelta)
            => Quaternion.RotateTowards(from, to, maxDegreesDelta);

        [CLMethod("Creates a rotation that rotates around a specified axis by a specified angle.")]
        public static CustomLogicQuaternionBuiltin AngleAxis(
            [CLParam("The angle in degrees.")]
            float angle,
            [CLParam("The axis to rotate around.")]
            CustomLogicVector3Builtin axis)
            => Quaternion.AngleAxis(angle, axis);

        [CLMethod("Calculates the angle between two rotations.")]
        public static float Angle(
            [CLParam("The first rotation.")]
            CustomLogicQuaternionBuiltin a,
            [CLParam("The second rotation.")]
            CustomLogicQuaternionBuiltin b)
            => Quaternion.Angle(a, b);

        public override string ToString()
        {
            return Value.ToString();
        }

        [CLMethod("Creates a copy of this quaternion. Returns: A new Quaternion with the same values.")]
        public object __Copy__()
        {
            var value = new Quaternion(Value.x, Value.y, Value.z, Value.w);
            return new CustomLogicQuaternionBuiltin(value);
        }

        public object __Add__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other);
        public object __Sub__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other);

        [CLMethod("Multiplies two quaternions or a quaternion by a vector. Returns: A new quaternion or vector with the multiplied result.")]
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

        [CLMethod("Checks if two quaternions are equal. Returns: True if the quaternions are equal, false otherwise.")]
        public bool __Eq__(object self, object other)
        {
            if (other is CustomLogicQuaternionBuiltin quat && self is CustomLogicQuaternionBuiltin quat2)
                return quat2.Value == quat.Value;

            return false;
        }

        [CLMethod("Gets the hash code of the quaternion. Returns: The hash code.")]
        public int __Hash__() => Value.GetHashCode();

        public object __Mod__(object self, object other)
        {
            throw new System.NotImplementedException();
        }

        public static implicit operator Quaternion(CustomLogicQuaternionBuiltin q) => q.Value;
        public static implicit operator CustomLogicQuaternionBuiltin(Quaternion q) => new CustomLogicQuaternionBuiltin(q);
    }
}
