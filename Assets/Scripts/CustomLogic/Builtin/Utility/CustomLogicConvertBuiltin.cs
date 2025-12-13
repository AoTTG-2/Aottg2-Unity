using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Converting objects to different types.
    /// </summary>
    [CLType(Name = "Convert", Abstract = true, Static = true)]
    partial class CustomLogicConvertBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicConvertBuiltin()
        {
        }

        // Convert CallMethod to the CLMethod format
        [CLMethod(Static = true, Description = "Converts a value to a float")]
        public float ToFloat(object value)
        {
            if (value is string)
                return float.Parse((string)value, CultureInfo.InvariantCulture);
            if (value is float)
                return (float)value;
            if (value is int)
                return (float)(int)value;
            if (value is bool)
                return (bool)value ? 1f : 0f;
            return 0f;
        }

        [CLMethod(Static = true, Description = "Converts a value to an int")]
        public int ToInt(object value)
        {
            if (value is string)
                return int.Parse((string)value);
            if (value is float)
                return (int)(float)value;
            if (value is int)
                return (int)value;
            if (value is bool)
                return (bool)value ? 1 : 0;
            return 0;
        }

        [CLMethod(Static = true, Description = "Converts a value to a bool")]
        public bool ToBool(object value)
        {
            if (value is string)
                return ((string)value).ToLower() == "true";
            if (value is float)
                return (float)value != 0f;
            if (value is int)
                return (int)value != 0;
            if (value is bool)
                return (bool)value;
            return false;
        }

        [CLMethod(Static = true, Description = "Converts a value to a string")]
        public string ToString(object value)
        {
            if (value == null)
                return "null";
            if (value is string)
                return (string)value;
            if (value is bool)
                return (bool)value ? "true" : "false";
            return value.ToString();
        }

        [CLMethod(Static = true, Description = "Checks if the value is a float")]
        public bool IsFloat(object value)
        {
            return value != null && value is float;
        }

        [CLMethod(Static = true, Description = "Checks if the value is an int")]
        public bool IsInt(object value)
        {
            return value != null && value is int;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a bool")]
        public bool IsBool(object value)
        {
            return value != null && value is bool;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a string")]
        public bool IsString(object value)
        {
            return value != null && value is string;
        }

        [CLMethod(Static = true, Description = "Checks if the value is an object")]
        public bool IsObject(object value)
        {
            return value != null && value is CustomLogicClassInstance;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a list")]
        public bool IsList(object value)
        {
            return value != null && value is CustomLogicListBuiltin;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a dictionary")]
        public bool IsDict(object value)
        {
            return value != null && value is CustomLogicDictBuiltin;
        }

        [CLMethod(Static = true, Description = "Checks if the class instance has a variable")]
        public bool HasVariable(CustomLogicClassInstance cInstance, string variableName)
        {
            return cInstance.HasVariable(variableName);
        }

        [CLMethod(Static = true, Description = "Defines a variable for the class instance")]
        public void DefineVariable(CustomLogicClassInstance cInstance, string variableName, object value)
        {
            // Add the variable to the class instance if it doesn't exist
            if (!cInstance.HasVariable(variableName))
            {
                cInstance.Variables[variableName] = value;
            }
        }

        [CLMethod(Static = true, Description = "Removes a variable from the class instance")]
        public void RemoveVariable(CustomLogicClassInstance cInstance, string variableName)
        {
            // Remove the variable from the class instance if it exists
            if (cInstance.HasVariable(variableName))
            {
                cInstance.Variables.Remove(variableName);
            }
        }

        [CLMethod(Static = true, Description = "Gets the type of the class instance")]
        public string GetType(CustomLogicClassInstance cInstance)
        {
            return cInstance.ClassName;
        }


    }
}
