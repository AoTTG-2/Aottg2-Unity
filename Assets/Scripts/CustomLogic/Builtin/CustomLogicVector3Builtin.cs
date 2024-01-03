using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicVector3Builtin: CustomLogicStructBuiltin
    {
        public Vector3 Value = Vector3.zero;

        public CustomLogicVector3Builtin(List<object> parameterValues): base("Vector3")
        {
            if (parameterValues.Count == 0)
                return;
            Value = new Vector3(parameterValues[0].UnboxToFloat(), parameterValues[1].UnboxToFloat(), parameterValues[2].UnboxToFloat());
        }

        public CustomLogicVector3Builtin(Vector3 value): base("Vector3")
        {
            Value = value;
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Lerp")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicVector3Builtin(Vector3.Lerp(a.Value, b.Value, t));
            }
            if (methodName == "Scale")
            {
                float scale = parameters[0].UnboxToFloat();
                return new CustomLogicVector3Builtin(Value * scale);
            }
            if (methodName == "GetRotationDirection")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                Vector3 direction = Quaternion.Euler(a.Value) * b.Value;
                return new CustomLogicVector3Builtin(direction);
            }
            if (methodName == "Distance")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return Vector3.Distance(a.Value, b.Value);
            }
            if (methodName == "Project")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicVector3Builtin(Vector3.Project(a.Value, b.Value));
            }
            if (methodName == "Multiply")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicVector3Builtin(Util.MultiplyVectors(a.Value, b.Value));
            }
            if (methodName == "Divide")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicVector3Builtin(Util.DivideVectors(a.Value, b.Value));
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
            if (name == "Normalized")
                return new CustomLogicVector3Builtin(Value.normalized);
            if (name == "Magnitude")
                return Value.magnitude;
            if (name == "Up")
                return new CustomLogicVector3Builtin(Vector3.up);
            if (name == "Down")
                return new CustomLogicVector3Builtin(Vector3.down);
            if (name == "Left")
                return new CustomLogicVector3Builtin(Vector3.left);
            if (name == "Right")
                return new CustomLogicVector3Builtin(Vector3.right);
            if (name == "Forward")
                return new CustomLogicVector3Builtin(Vector3.forward);
            if (name == "Back")
                return new CustomLogicVector3Builtin(Vector3.back);
            if (name == "Zero")
                return new CustomLogicVector3Builtin(Vector3.zero);
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
            else
                base.SetField(name, value);
        }

        public override CustomLogicStructBuiltin Copy()
        {
            Vector3 value = new Vector3(Value.x, Value.y, Value.z);
            return new CustomLogicVector3Builtin(value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomLogicVector3Builtin))
                return false;
            var other = ((CustomLogicVector3Builtin)obj).Value;
            return Value == other;
        }
    }
}
