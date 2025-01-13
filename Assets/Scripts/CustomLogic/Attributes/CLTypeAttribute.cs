using System;

namespace CustomLogic
{
    /// <summary>
    /// Custom logic builtin types must be marked with this attribute,
    /// they must also either have a parameterless constructor or
    /// a constructor with a single parameter of type object[]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class CLTypeAttribute : CLBaseAttribute
    {
        /// <summary>
        /// Should be set to true if the type shouldn't be instantiated
        /// </summary>
        public bool Abstract { get; set; } = false;

        public bool InheritBaseMembers { get; set; } = true;
    }
}