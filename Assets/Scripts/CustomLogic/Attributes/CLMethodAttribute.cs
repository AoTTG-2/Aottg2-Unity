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

        public CLMethodAttribute(string description = "")
        {
            Description = description;
        }
    }
}
