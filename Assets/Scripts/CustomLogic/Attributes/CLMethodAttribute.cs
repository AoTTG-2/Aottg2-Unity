using System;

namespace CustomLogic
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CLMethodAttribute : CLBaseAttribute
    {
        public CLMethodAttribute(string description = "")
        {
            Description = description;
        }
    }
}