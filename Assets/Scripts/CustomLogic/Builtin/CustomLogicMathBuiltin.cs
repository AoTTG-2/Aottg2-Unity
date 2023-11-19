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
            else if (name == "Max")
            {
                if (parameters[0] is int && parameters[1] is int)
                    return Mathf.Max((int)parameters[0], (int)parameters[1]);
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Max(a, b);
            }
            else if (name == "Min")
            {
                if (parameters[0] is int && parameters[1] is int)
                    return Mathf.Min((int)parameters[0], (int)parameters[1]);
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Min(a, b);
            }
            else if (name == "Pow")
            {
                float a = parameters[0].UnboxToFloat();
                float b = parameters[1].UnboxToFloat();
                return Mathf.Pow(a, b);
            }
            else if (name == "Abs")
            {
                if (parameters[0] is int)
                    return Mathf.Abs((int)parameters[0]);
                float a = (float)parameters[0];
                return Mathf.Abs(a);
            }
            else if (name == "Sqrt")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Sqrt(a);
            }
            else if (name == "Mod")
            {
                int a = parameters[0].UnboxToInt();
                int b = parameters[1].UnboxToInt();
                return a % b;
            }
            else if (name == "Sin")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Sin(a);
            }
            else if (name == "Cos")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Cos(a);
            }
            else if (name == "Tan")
            {
                float a = parameters[0].UnboxToFloat() * Mathf.Deg2Rad;
                return Mathf.Tan(a);
            }
            else if (name == "Asin")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Asin(a) * Mathf.Rad2Deg;
            }
            else if (name == "Acos")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Acos(a) * Mathf.Rad2Deg;
            }
            else if (name == "Atan")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.Atan(a) * Mathf.Rad2Deg;
            }
            else if (name == "Ceil")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.CeilToInt(a);
            }
            else if (name == "Floor")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.FloorToInt(a);
            }
            else if (name == "Round")
            {
                float a = parameters[0].UnboxToFloat();
                return Mathf.RoundToInt(a);
            }
            return base.CallMethod(name, parameters);
        }
    }
}
