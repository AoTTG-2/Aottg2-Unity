using System;
using System.Diagnostics;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class CLPropertyAttribute : CLBaseAttribute
    {
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Generic type arguments for pseudo-generic types (e.g., ["Titan"] for List&lt;Titan&gt;)
        /// </summary>
        public string[] TypeArguments { get; set; }

        public CLPropertyAttribute(string description = "", bool readOnly = false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }
}
