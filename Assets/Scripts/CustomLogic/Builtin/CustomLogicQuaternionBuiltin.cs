using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicQuaternionBuiltin: CustomLogicStructBuiltin
    {
        public Quaternion Value = Quaternion.identity;

        public CustomLogicQuaternionBuiltin(List<object> parameterValues): base("Quaternion")
        {
            if (parameterValues.Count == 0)
                return;
            Value = new Quaternion(parameterValues[0].UnboxToFloat(), parameterValues[1].UnboxToFloat(), parameterValues[2].UnboxToFloat(), parameterValues[3].UnboxToFloat());
        }

        public CustomLogicQuaternionBuiltin(Quaternion value): base("Quaternion")
        {
            Value = value;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Lerp")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                var b = (CustomLogicQuaternionBuiltin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicQuaternionBuiltin(Quaternion.Lerp(a.Value, b.Value, t));
            }
            if (methodName == "LerpUnclamped")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                var b = (CustomLogicQuaternionBuiltin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicQuaternionBuiltin(Quaternion.LerpUnclamped(a.Value, b.Value, t));
            }
            if (methodName == "Slerp")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                var b = (CustomLogicQuaternionBuiltin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicQuaternionBuiltin(Quaternion.Slerp(a.Value, b.Value, t));
            }
            if (methodName == "SlerpUnclamped")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                var b = (CustomLogicQuaternionBuiltin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicQuaternionBuiltin(Quaternion.SlerpUnclamped(a.Value, b.Value, t));
            }
            if (methodName == "FromEuler")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                return new CustomLogicQuaternionBuiltin(Quaternion.Euler(a.Value));
            }
            if (methodName == "LookRotation")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                if (parameters.Count == 1)
                    return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(a.Value));

                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicQuaternionBuiltin(Quaternion.LookRotation(a.Value, b.Value));
            }
            if (methodName == "FromToRotation")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicQuaternionBuiltin(Quaternion.FromToRotation(a.Value, b.Value));
            }
            if (methodName == "Inverse")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                return new CustomLogicQuaternionBuiltin(Quaternion.Inverse(a.Value));
            }
            if (methodName == "RotateTowards")
            {
                var a = (CustomLogicQuaternionBuiltin)parameters[0];
                var b = (CustomLogicQuaternionBuiltin)parameters[1];
                float maxDegreesDelta = parameters[2].UnboxToFloat();
                return new CustomLogicQuaternionBuiltin(Quaternion.RotateTowards(a.Value, b.Value, maxDegreesDelta));
            }
            return base.CallMethod(methodName, parameters);
        }
      
        public override object GetField(string name)
        {
            if (name == "X")
                return Value.x;
            if (name == "Y")
                return Value.y;
            if (name == "Z")
                return Value.z;
            if (name == "W")
                return Value.w;
            if (name == "Identity")
                return new CustomLogicQuaternionBuiltin(Quaternion.identity);
            if (name == "Euler")
                return new CustomLogicVector3Builtin(Value.eulerAngles);
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            if (name == "X")
                Value.x = value.UnboxToFloat();
            else if (name == "Y")
                Value.y = value.UnboxToFloat();
            else if (name == "Z")
                Value.z = value.UnboxToFloat();
            else if (name == "W")
                Value.w = value.UnboxToFloat();
            else
                base.SetField(name, value);
        }

        public override CustomLogicStructBuiltin Copy()
        {
            Quaternion value = new Quaternion(Value.x, Value.y, Value.z, Value.w);
            return new CustomLogicQuaternionBuiltin(value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomLogicQuaternionBuiltin))
                return false;
            var other = ((CustomLogicQuaternionBuiltin)obj).Value;
            return Value == other;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
