using System;
using System.Diagnostics;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class CLConstructorAttribute : Attribute
    {
        public string Description { get; set; } = "";

        public CLConstructorAttribute() { }

        public CLConstructorAttribute(string description)
        {
            Description = description;
        }
    }
}
