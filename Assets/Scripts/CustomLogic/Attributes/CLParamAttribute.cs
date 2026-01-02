using System;
using System.Diagnostics;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class CLParamAttribute : Attribute
    {
        public string Description { get; set; }

        /// <summary>
        /// Generic type parameter name for this parameter.
        /// Use null or empty string for parameters that should use their default resolved type.
        /// Can specify simple type parameters (e.g., "K", "V") or generic types with arguments (e.g., "List&lt;string&gt;", "List&lt;K&gt;").
        /// For generic types, use angle brackets: "TypeName&lt;Arg1,Arg2&gt;" or "TypeName&lt;Arg1&gt;"
        /// Nested generics are supported (e.g., "Dict&lt;K,List&lt;V&gt;&gt;").
        /// </summary>
        public string Type { get; set; }

        public CLParamAttribute(string description = "")
        {
            Description = description;
        }
    }
}

