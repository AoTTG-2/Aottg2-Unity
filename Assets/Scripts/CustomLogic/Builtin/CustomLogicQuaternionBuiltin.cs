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

        [CLConstructor]
        public CustomLogicQuaternionBuiltin() { }

        /// <summary>
        /// Creates a new Quaternion from the given values.
        /// </summary>
        [CLConstructor]
        public CustomLogicQuaternionBuiltin(float x, float y, float z, float w)
        {
            Value = new Quaternion(x, y, z, w);
        }

        public CustomLogicQuaternionBuiltin(Quaternion value)
        {
            Value = value;
        }

        /// <inheritdoc cref="Quaternion.x"/>
        [CLProperty]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }

        /// <inheritdoc cref="Quaternion.y"/>
        [CLProperty]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }

        /// <inheritdoc cref="Quaternion.z"/>
        [CLProperty]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }

        /// <inheritdoc cref="Quaternion.w"/>
        [CLProperty]
        public float W
        {
            get => Value.w;
            set => Value.w = value;
        }

        /// <inheritdoc cref="Quaternion.eulerAngles"/>
        [CLProperty]
        public CustomLogicVector3Builtin Euler
        {
            get => Value.eulerAngles;
            set => Value = Quaternion.Euler(value);
        }

        /// <inheritdoc cref="Quaternion.identity"/>
        [CLProperty] public static CustomLogicQuaternionBuiltin Identity => Quaternion.identity;

        /// <inheritdoc cref="Quaternion.Lerp"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin Lerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Lerp(a, b, t);

        /// <inheritdoc cref="Quaternion.LerpUnclamped"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin LerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.LerpUnclamped(a, b, t);

        /// <inheritdoc cref="Quaternion.Slerp"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin Slerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Slerp(a, b, t);

        /// <inheritdoc cref="Quaternion.SlerpUnclamped"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin SlerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.SlerpUnclamped(a, b, t);

        /// <summary>
        /// Returns the Quaternion rotation from the given euler angles.
        /// </summary>
        [CLMethod] public static CustomLogicQuaternionBuiltin FromEuler(CustomLogicVector3Builtin euler) => Quaternion.Euler(euler);

        /// <inheritdoc cref="Quaternion.LookRotation(Vector3, Vector3)"/>
        [CLMethod]
        public static CustomLogicQuaternionBuiltin LookRotation(CustomLogicVector3Builtin forward, CustomLogicVector3Builtin upwards = null)
        {
            if (upwards == null)
                return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward));
            return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(forward, upwards));
        }

        /// <inheritdoc cref="Quaternion.FromToRotation(Vector3, Vector3)"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin FromToRotation(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Quaternion.FromToRotation(a, b);

        /// <inheritdoc cref="Quaternion.Inverse"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin Inverse(CustomLogicQuaternionBuiltin q) => Quaternion.Inverse(q);

        /// <inheritdoc cref="Quaternion.RotateTowards(Quaternion, Quaternion, float)"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin RotateTowards(CustomLogicQuaternionBuiltin from, CustomLogicQuaternionBuiltin to, float maxDegreesDelta) => Quaternion.RotateTowards(from, to, maxDegreesDelta);

        /// <inheritdoc cref="Quaternion.AngleAxis(float, Vector3)"/>
        [CLMethod] public static CustomLogicQuaternionBuiltin AngleAxis(float angle, CustomLogicVector3Builtin axis) => Quaternion.AngleAxis(angle, axis);

        public override string ToString()
        {
            return Value.ToString();
        }

        [CLMethod]
        public object __Copy__()
        {
            var value = new Quaternion(Value.x, Value.y, Value.z, Value.w);
            return new CustomLogicQuaternionBuiltin(value);
        }

        public object __Add__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Add__), self, other);
        public object __Sub__(object self, object other) => throw CustomLogicUtils.OperatorException(nameof(__Sub__), self, other);

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

        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            if (other is CustomLogicQuaternionBuiltin quat && self is CustomLogicQuaternionBuiltin quat2)
                return quat2.Value == quat.Value;

            return false;
        }

        [CLMethod]
        public int __Hash__() => Value.GetHashCode();

        public static implicit operator Quaternion(CustomLogicQuaternionBuiltin q) => q.Value;
        public static implicit operator CustomLogicQuaternionBuiltin(Quaternion q) => new CustomLogicQuaternionBuiltin(q);
    }
}
