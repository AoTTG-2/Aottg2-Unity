using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicQuaternionBuiltin: CustomLogicClassInstanceBuiltin, ICustomLogicMathOperators, ICustomLogicEquals, ICustomLogicCopyable
    {
        public Quaternion Value = Quaternion.identity;

        public CustomLogicQuaternionBuiltin(object[] parameterValues): base("Quaternion")
        {
            if (parameterValues.Length == 0)
                return;
            Value = new Quaternion(parameterValues[0].UnboxToFloat(), parameterValues[1].UnboxToFloat(), parameterValues[2].UnboxToFloat(), parameterValues[3].UnboxToFloat());
        }

        public CustomLogicQuaternionBuiltin(Quaternion value): base("Quaternion")
        {
            Value = value;
        }

        [CLProperty]
        public float X
        {
            get => Value.x;
            set => Value.x = value;
        }
        
        [CLProperty]
        public float Y
        {
            get => Value.y;
            set => Value.y = value;
        }
        
        [CLProperty]
        public float Z
        {
            get => Value.z;
            set => Value.z = value;
        }
        
        [CLProperty]
        public float W
        {
            get => Value.w;
            set => Value.w = value;
        }
        
        [CLProperty]
        public CustomLogicVector3Builtin EulerAngles
        {
            get => new CustomLogicVector3Builtin(Value.eulerAngles);
            set => Value = Quaternion.Euler(value);
        }
        
        [CLProperty]
        public static CustomLogicQuaternionBuiltin Identity => Quaternion.identity;

        [CLMethod]
        public static CustomLogicQuaternionBuiltin Lerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Lerp(a, b, t);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin LerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.LerpUnclamped(a, b, t);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin Slerp(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.Slerp(a, b, t);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin SlerpUnclamped(CustomLogicQuaternionBuiltin a, CustomLogicQuaternionBuiltin b, float t) => Quaternion.SlerpUnclamped(a, b, t);

        [CLMethod]
        public static CustomLogicQuaternionBuiltin FromEuler(CustomLogicVector3Builtin euler) => Quaternion.Euler(euler);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin LookRotation(CustomLogicVector3Builtin forward, CustomLogicVector3Builtin upwards) => Quaternion.LookRotation(forward, upwards);
        [CLMethod]
        public static CustomLogicQuaternionBuiltin FromToRotation(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => Quaternion.FromToRotation(a, b);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin Inverse(CustomLogicQuaternionBuiltin q) => Quaternion.Inverse(q);
        
        [CLMethod]
        public static CustomLogicQuaternionBuiltin RotateTowards(CustomLogicQuaternionBuiltin from, CustomLogicQuaternionBuiltin to, float maxDegreesDelta) => Quaternion.RotateTowards(from, to, maxDegreesDelta);

        public override string ToString()
        {
            return Value.ToString();
        }

        [CLMethod]
        public object __Copy__()
        {
            Quaternion value = new Quaternion(Value.x, Value.y, Value.z, Value.w);
            return new CustomLogicQuaternionBuiltin(value);
        }

        [CLMethod]
        public object __Add__(object other) => throw CustomLogicUtils.OperatorException(nameof(__Add__), this, other);
        [CLMethod]
        public object __Sub__(object other) => throw CustomLogicUtils.OperatorException(nameof(__Sub__), this, other);

        [CLMethod]
        public object __Mul__(object other)
        {
            if (other is CustomLogicVector3Builtin v3)
                return new CustomLogicVector3Builtin(Value * v3.Value);
            if (other is CustomLogicQuaternionBuiltin quat)
                return new CustomLogicQuaternionBuiltin(Value * quat.Value);
            
            throw CustomLogicUtils.OperatorException(nameof(__Mul__), this, other);
        }

        [CLMethod]
        public object __Div__(object other) => throw CustomLogicUtils.OperatorException(nameof(__Div__), this, other);

        [CLMethod]
        public bool __Eq__(object other)
        {
            if (other is CustomLogicQuaternionBuiltin quat)
                return Value == quat.Value;
            
            return false;
        }

        [CLMethod]
        public int __Hash__() => Value.GetHashCode();
        
        public static implicit operator Quaternion(CustomLogicQuaternionBuiltin q) => q.Value;
        public static implicit operator CustomLogicQuaternionBuiltin(Quaternion q) => new(q);
    }
}
