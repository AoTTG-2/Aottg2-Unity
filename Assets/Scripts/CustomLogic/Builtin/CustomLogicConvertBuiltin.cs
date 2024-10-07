using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicConvertBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicConvertBuiltin(): base("Convert")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "ToFloat")
            {
                object param = parameters[0];
                if (param is string)
                    return float.Parse((string)param);
                if (param is float)
                    return (float)param;
                if (param is int)
                    return param.UnboxToFloat();
                if (param is bool)
                    return ((bool)param) ? 1f : 0f;
                return null;
            }
            if (name == "ToInt")
            {
                object param = parameters[0];
                if (param is string)
                    return int.Parse((string)param);
                if (param is float)
                    return (int)(float)param;
                if (param is int)
                    return (int)param;
                if (param is bool)
                    return ((bool)param) ? 1 : 0;
                return null;
            }
            if (name == "ToBool")
            {
                object param = parameters[0];
                if (param is string)
                    return ((string)param).ToLower() == "true";
                if (param is float)
                    return (float)param != 0f;
                if (param is int)
                    return (int)param != 0;
                if (param is bool)
                    return (bool)param;
                return null;
            }
            if (name == "ToString")
            {
                object param = parameters[0];
                if (param == null)
                    return "null";
                if (param is string)
                    return (string)param;
                if (param is bool)
                    return ((bool)param) ? "true" : "false";
                return param.ToString();
            }
            if (name == "IsFloat")
            {
                object param = parameters[0];
                return param != null && param is float;
            }
            if (name == "IsInt")
            {
                object param = parameters[0];
                return param != null && param is int;
            }
            if (name == "IsBool")
            {
                object param = parameters[0];
                return param != null && param is bool;
            }
            if (name == "IsString")
            {
                object param = parameters[0];
                return param != null && param is string;
            }
            if (name == "IsObject")
            {
                object param = parameters[0];
                return param != null && param is CustomLogicBaseBuiltin;
            }
            return base.CallMethod(name, parameters);
        }
    }
}
