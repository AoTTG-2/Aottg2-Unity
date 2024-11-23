using System;

namespace CustomLogic
{
    internal abstract class CLBaseAttribute : Attribute
    {
        public string Description { get; set; } = "";
        
        public void ClearDescription() => Description = "";
    }
}