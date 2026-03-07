using System;
using System.Diagnostics;

namespace CustomLogic
{
    /// <summary>
    /// Custom logic builtin types must be marked with this attribute,
    /// they must also either have a parameterless constructor or
    /// a constructor with a single parameter of type object[]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    internal class CLTypeAttribute : CLBaseAttribute
    {
        /// <summary>
        /// Should be set to true if the type shouldn't be instantiated
        /// </summary>
        public bool Abstract { get; set; } = false;

        public bool InheritBaseMembers { get; set; } = true;

        /// <summary>
        /// Generic type parameter names for types that are conceptually generic in ACL
        /// (e.g., ["K", "V"] for Dict&lt;K, V&gt;)
        /// </summary>
        public string[] TypeParameters { get; set; }

        /// <summary>
        /// Indicates whether this type is a component
        /// </summary>
        public bool IsComponent { get; set; } = false;
    }
}
