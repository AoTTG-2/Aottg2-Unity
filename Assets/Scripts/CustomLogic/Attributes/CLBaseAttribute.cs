using System;

namespace CustomLogic
{
    internal abstract class CLBaseAttribute : Attribute
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Static { get; set; }

        /// <summary>
        /// If true, the method/property/field can be called as a static method or an instance method.
        /// </summary>
        public bool Hybrid { get; set; }
    }
}
