using ApplicationManagers;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Convert", Abstract = true, Static = true, Description = "Converting objects to different types.")]
    partial class CustomLogicConvertBuiltin : BuiltinClassInstance
    {
        [CLConstructor("Creates a new Convert instance.")]
        public CustomLogicConvertBuiltin()
        {
        }

        [CLMethod(Static = true, Description = "Converts a value to a float")]
        public float ToFloat(
            [CLParam("The value to convert (can be string, float, int, or bool).")]
            object value)
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
        public int ToInt(
            [CLParam("The value to convert (can be string, float, int, or bool).")]
            object value)
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
        public bool ToBool(
            [CLParam("The value to convert (can be string, float, int, or bool).")]
            object value)
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
        public string ToString(
            [CLParam("The value to convert.")]
            object value)
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
        public bool IsFloat(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is float;
        }

        [CLMethod(Static = true, Description = "Checks if the value is an int")]
        public bool IsInt(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is int;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a bool")]
        public bool IsBool(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is bool;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a string")]
        public bool IsString(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is string;
        }

        [CLMethod(Static = true, Description = "Checks if the value is an object")]
        public bool IsObject(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is CustomLogicClassInstance;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a list")]
        public bool IsList(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is CustomLogicListBuiltin;
        }

        [CLMethod(Static = true, Description = "Checks if the value is a dictionary")]
        public bool IsDict(
            [CLParam("The value to check.")]
            object value)
        {
            return value != null && value is CustomLogicDictBuiltin;
        }

        [CLMethod(Static = true, Description = "Checks if the class instance has a variable")]
        public bool HasVariable(
            [CLParam("The class instance to check.")]
            CustomLogicClassInstance cInstance,
            [CLParam("The name of the variable to check for.")]
            string variableName)
        {
            return cInstance.HasVariable(variableName);
        }

        [CLMethod(Static = true, Description = "Defines a variable for the class instance")]
        public void DefineVariable(
            [CLParam("The class instance to define the variable on.")]
            CustomLogicClassInstance cInstance,
            [CLParam("The name of the variable to define.")]
            string variableName,
            [CLParam("The value to assign to the variable.")]
            object value)
        {
            // Add the variable to the class instance if it doesn't exist
            if (!cInstance.HasVariable(variableName))
            {
                cInstance.Variables[variableName] = value;
            }
        }

        [CLMethod(Static = true, Description = "Removes a variable from the class instance")]
        public void RemoveVariable(
            [CLParam("The class instance to remove the variable from.")]
            CustomLogicClassInstance cInstance,
            [CLParam("The name of the variable to remove.")]
            string variableName)
        {
            // Remove the variable from the class instance if it exists
            if (cInstance.HasVariable(variableName))
            {
                cInstance.Variables.Remove(variableName);
            }
        }

        [CLMethod(Static = true, Description = "Checks if the class instance has a method")]
        public bool HasMethod(
            [CLParam("The class instance to check.")]
            CustomLogicClassInstance cInstance,
            [CLParam("The name of the method to check for.")]
            string methodName)
        {
            var eval = CustomLogicManager.Evaluator;
            if (eval != null)
            {
                return eval.HasMethod(cInstance, methodName);
            }
            return false;
        }

        [CLMethod(Static = true, Description = "Gets the type of the class instance")]
        public string GetType(
            [CLParam("The class instance to get the type of.")]
            CustomLogicClassInstance cInstance)
        {
            return cInstance.ClassName;
        }
    }
}
