using System.Globalization;

namespace CustomLogic
{
    /// <summary>
    /// Converting objects to different types.
    /// </summary>
    [CLType(Name = "Convert", Abstract = true, Static = true)]
    partial class CustomLogicConvertBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicConvertBuiltin(){}

        /// <summary>
        /// Converts a value to a float.
        /// </summary>
        /// <param name="value">The value to convert (can be string, float, int, or bool).</param>
        /// <returns>The converted float value.</returns>
        [CLMethod(Static = true)]
        public static float ToFloat(object value)
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

        /// <summary>
        /// Converts a value to an int.
        /// </summary>
        /// <param name="value">The value to convert (can be string, float, int, or bool).</param>
        /// <returns>The converted int value.</returns>
        [CLMethod(Static = true)]
        public static int ToInt(object value)
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

        /// <summary>
        /// Converts a value to a bool.
        /// </summary>
        /// <param name="value">The value to convert (can be string, float, int, or bool).</param>
        /// <returns>The converted bool value.</returns>
        [CLMethod(Static = true)]
        public static bool ToBool(object value)
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

        /// <summary>
        /// Converts a value to a string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted string value.</returns>
        [CLMethod(Static = true)]
        public static string ToString(object value)
        {
            if (value == null)
                return "null";
            if (value is string)
                return (string)value;
            if (value is bool)
                return (bool)value ? "true" : "false";
            return value.ToString();
        }

        /// <summary>
        /// Checks if the value is a float.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is a float, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsFloat(object value)
        {
            return value != null && value is float;
        }

        /// <summary>
        /// Checks if the value is an int.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is an int, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsInt(object value)
        {
            return value != null && value is int;
        }

        /// <summary>
        /// Checks if the value is a bool.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is a bool, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsBool(object value)
        {
            return value != null && value is bool;
        }

        /// <summary>
        /// Checks if the value is a string.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is a string, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsString(object value)
        {
            return value != null && value is string;
        }

        /// <summary>
        /// Checks if the value is an object.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is an object, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsObject(object value)
        {
            return value != null && value is CustomLogicClassInstance;
        }

        /// <summary>
        /// Checks if the value is a list.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is a list, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsList(object value)
        {
            return value != null && value is CustomLogicListBuiltin;
        }

        /// <summary>
        /// Checks if the value is a dictionary.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is a dictionary, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool IsDict(object value)
        {
            return value != null && value is CustomLogicDictBuiltin;
        }

        /// <summary>
        /// Checks if the class instance has a variable.
        /// </summary>
        /// <param name="cInstance">The class instance to check.</param>
        /// <param name="variableName">The name of the variable to check for.</param>
        /// <returns>True if the class instance has the variable, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool HasVariable(CustomLogicClassInstance cInstance, string variableName)
        {
            return cInstance.HasVariable(variableName);
        }

        /// <summary>
        /// Defines a variable for the class instance.
        /// </summary>
        /// <param name="cInstance">The class instance to define the variable on.</param>
        /// <param name="variableName">The name of the variable to define.</param>
        /// <param name="value">The value to assign to the variable.</param>
        [CLMethod(Static = true)]
        public static void DefineVariable(CustomLogicClassInstance cInstance, string variableName, object value)
        {
            // Add the variable to the class instance if it doesn't exist
            if (!cInstance.HasVariable(variableName))
            {
                cInstance.Variables[variableName] = value;
            }
        }

        /// <summary>
        /// Removes a variable from the class instance.
        /// </summary>
        /// <param name="cInstance">The class instance to remove the variable from.</param>
        /// <param name="variableName">The name of the variable to remove.</param>
        [CLMethod(Static = true)]
        public static void RemoveVariable(CustomLogicClassInstance cInstance, string variableName)
        {
            // Remove the variable from the class instance if it exists
            if (cInstance.HasVariable(variableName))
            {
                cInstance.Variables.Remove(variableName);
            }
        }

        /// <summary>
        /// Checks if the class instance has a method.
        /// </summary>
        /// <param name="cInstance">The class instance to check.</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns>True if the class instance has the method, false otherwise.</returns>
        [CLMethod(Static = true)]
        public static bool HasMethod(CustomLogicClassInstance cInstance, string methodName)
        {
            var eval = CustomLogicManager.Evaluator;
            if (eval != null)
            {
                return eval.HasMethod(cInstance, methodName);
            }
            return false;
        }

        /// <summary>
        /// Gets the type of the class instance.
        /// </summary>
        /// <param name="cInstance">The class instance to get the type of.</param>
        /// <returns>The class name of the instance.</returns>
        [CLMethod(Static = true)]
        public static string GetType(CustomLogicClassInstance cInstance)
        {
            return cInstance.ClassName;
        }
    }
}
