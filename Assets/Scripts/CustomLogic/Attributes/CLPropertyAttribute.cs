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
        /// Types of the enum classes to reference (e.g., typeof(CustomLogicEffectNameEnum)).
        /// The enum names will be retrieved from the CLType attribute of the specified types.
        /// </summary>
        public Type[] Enum { get; set; }

        public CLPropertyAttribute(bool readOnly = false)
        {
            ReadOnly = readOnly;
        }
    }
}
