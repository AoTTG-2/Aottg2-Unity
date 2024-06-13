using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicMathBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicMathBuiltin(): base("Math")
        {
        }

        public override object GetField(string name)
        {
            if (name == "PI")
            {
                return Mathf.PI;
            }
            if (name == "Infinity")
            {
                return Mathf.Infinity;
            }
            
            return base.GetField(name);
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "Clamp")
            {
                if (parameters[0] is int && parameters[1] is int && parameters[2] is int)
                    return Mathf.Clamp((int)parameters[0], (int)parameters[1], (int)parameters[2]);
                float value = parameters[0].UnboxToFloat();
                float min = parameters[1].UnboxToFloat();
                float max = parameters[2].UnboxToFloat();
                return Mathf.Clamp(value, min, max);
            }
            if (name == "Max")
            {
                if (parameters[0] is int && parameters[1] is int)
                    return Mathf.Max((int)parameters[0], (int)parameters[1]);
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Max(a, b);
            }
            if (name == "Min")
            {
                if (parameters[0] is int && parameters[1] is int)
                    return Mathf.Min((int)parameters[0], (int)parameters[1]);
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Min(a, b);
            }
            if (name == "Pow")
            {
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Pow(a, b);
            }
            if (name == "Abs")
            {
                if (parameters[0] is int)
                    return Mathf.Abs((int)parameters[0]);
                float a = (float)parameters[0];
                return Mathf.Abs(a);
            }
            if (name == "Sqrt")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Sqrt(a);
            }
            if (name == "Mod")
            {
                int a = parameters[0].UnboxToInt();
                int b = parameters[1].UnboxToInt();
                return a % b;
            }
            if (name == "Sin")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Sin(a);
            }
            if (name == "Cos")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Cos(a);
            }
            if (name == "Tan")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Tan(a);
            }
            if (name == "Asin")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Asin(a) * Mathf.Rad2Deg;
            }
            if (name == "Acos")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Acos(a) * Mathf.Rad2Deg;
            }
            if (name == "Atan")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Atan(a) * Mathf.Rad2Deg;
            }
            if (name == "Ceil")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.CeilToInt(a);
            }
            if (name == "Floor")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.FloorToInt(a);
            }
            if (name == "Round")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.RoundToInt(a);
            }
            if (name == "Deg2Rad")
            {
                float a = parameters[0].UnboxToFloat();
                return a * Mathf.Deg2Rad;
            }
            if (name == "Rad2Deg")
            {
                float a = parameters[0].UnboxToFloat();
                return a * Mathf.Rad2Deg;
            }
            if (name == "Lerp")
            {
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                float t = parameters[2].UnboxToFloat();
                return Mathf.Lerp(a, b, t);
            }
            if (name == "LerpUnclamped")
            {
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                float t = parameters[2].UnboxToFloat();
                return Mathf.LerpUnclamped(a, b, t);
            }
            if (name == "Sign")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Sign(a);
            }

            return base.CallMethod(name, parameters);
        }
    }
}
