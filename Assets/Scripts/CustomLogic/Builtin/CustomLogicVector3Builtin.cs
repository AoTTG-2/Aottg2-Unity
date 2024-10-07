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
            var x = 0f;
            var y = 0f;
            var z = 0f;
            
            if (parameterValues.Count == 1)
            {
                x = parameterValues[0].UnboxToFloat();
                y = parameterValues[0].UnboxToFloat();
                z = parameterValues[0].UnboxToFloat();
            }
            else if (parameterValues.Count == 2)
            {
                x = parameterValues[0].UnboxToFloat();
                y = parameterValues[1].UnboxToFloat();
            }
            else if (parameterValues.Count == 3)
            {
                x = parameterValues[0].UnboxToFloat();
                y = parameterValues[1].UnboxToFloat();
                z = parameterValues[2].UnboxToFloat();
            }

            Value = new Vector3(x, y, z);
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
            if (methodName == "LerpUnclamped")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicVector3Builtin(Vector3.LerpUnclamped(a.Value, b.Value, t));
            }
            if (methodName == "Slerp")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicVector3Builtin(Vector3.Slerp(a.Value, b.Value, t));
            }
            if (methodName == "SlerpUnclamped")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                float t = parameters[2].UnboxToFloat();
                return new CustomLogicVector3Builtin(Vector3.SlerpUnclamped(a.Value, b.Value, t));
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
            if (methodName == "Angle")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return Vector3.Angle(a.Value, b.Value);
            }
            if (methodName == "SignedAngle")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                var c = (CustomLogicVector3Builtin)parameters[2];
                return Vector3.SignedAngle(a.Value, b.Value, c.Value);
            }
            if (methodName == "Cross")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return new CustomLogicVector3Builtin(Vector3.Cross(a.Value, b.Value));
            }
            if (methodName == "Dot")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                return Vector3.Dot(a.Value, b.Value);
            }
            if (methodName == "RotateTowards")
            {
                var a = (CustomLogicVector3Builtin)parameters[0];
                var b = (CustomLogicVector3Builtin)parameters[1];
                float angle = Mathf.Deg2Rad * parameters[2].UnboxToFloat();
                float mag = parameters[3].UnboxToFloat();
                return new CustomLogicVector3Builtin(Vector3.RotateTowards(a.Value, b.Value, angle, mag));
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

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
