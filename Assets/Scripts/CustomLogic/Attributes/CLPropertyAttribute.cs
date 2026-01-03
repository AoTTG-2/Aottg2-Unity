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

        /// <summary>
        /// Type of the enum class to reference (e.g., typeof(CustomLogicEffectNameEnum)).
        /// The enum name will be retrieved from the CLType attribute of the specified type.
        /// </summary>
        public Type Enum { get; set; }

        public CLPropertyAttribute(string description = "", bool readOnly = false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }
}
