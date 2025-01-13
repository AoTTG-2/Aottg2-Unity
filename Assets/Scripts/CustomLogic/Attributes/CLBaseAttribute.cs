using System;

namespace CustomLogic
{
    internal abstract class CLBaseAttribute : Attribute
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Static { get; set; }
    }
}