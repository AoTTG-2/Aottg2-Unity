using System;

namespace CustomLogic
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CLMethodAttribute : CLBaseAttribute
    {
        /// <summary>
        /// Generic type arguments for return type if it's a pseudo-generic type (e.g., ["Titan"] for List&lt;Titan&gt;)
        /// </summary>
        public string[] ReturnTypeArguments { get; set; }

        /// <summary>
        /// Generic type parameter names for method parameters, in the same order as the method parameters.
        /// Use null or empty string for parameters that should use their default resolved type.
        /// Can specify simple type parameters (e.g., "K", "V") or generic types with arguments (e.g., "List&lt;string&gt;", "List&lt;K&gt;").
        /// For generic types, use angle brackets: "TypeName&lt;Arg1,Arg2&gt;" or "TypeName&lt;Arg1&gt;"
        /// Nested generics are supported (e.g., "Dict&lt;K,List&lt;V&gt;&gt;").
        /// </summary>
        public string[] ParameterTypeArguments { get; set; }

        public CLMethodAttribute(string description = "")
        {
            Description = description;
        }
    }
}
