using System.Collections.Generic;
using UnityEngine;

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
            else if (methodName == "Scale")
            {
                float scale = parameters[0].UnboxToFloat();
                return new CustomLogicVector3Builtin(Value * scale);
            }
            else if (methodName == "GetRotationDirection")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                Vector3 direction = Quaternion.Euler(a.Value) * b.Value;
                return new CustomLogicVector3Builtin(direction);
            }
            else if (methodName == "Distance")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return Vector3.Distance(a.Value, b.Value);
            }
            else if (methodName == "Project")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicVector3Builtin(Vector3.Project(a.Value, b.Value));
            }
            return null;
        }
      
        public override object GetField(string name)
        {
            if (name == "X")
                return Value.x;
            else if (name == "Y")
                return Value.y;
            else if (name == "Z")
                return Value.z;
            else if (name == "Normalized")
                return new CustomLogicVector3Builtin(Value.normalized);
            else if (name == "Magnitude")
                return Value.magnitude;
            else if (name == "Up")
                return new CustomLogicVector3Builtin(Vector3.up);
            else if (name == "Down")
                return new CustomLogicVector3Builtin(Vector3.down);
            else if (name == "Left")
                return new CustomLogicVector3Builtin(Vector3.left);
            else if (name == "Right")
                return new CustomLogicVector3Builtin(Vector3.right);
            else if (name == "Forward")
                return new CustomLogicVector3Builtin(Vector3.forward);
            else if (name == "Back")
                return new CustomLogicVector3Builtin(Vector3.back);
            else if (name == "Zero")
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
