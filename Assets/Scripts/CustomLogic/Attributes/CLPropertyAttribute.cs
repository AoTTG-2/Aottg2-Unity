using System;

namespace CustomLogic
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class CLPropertyAttribute : CLBaseAttribute
    {
        public bool ReadOnly { get; set; }

        public CLPropertyAttribute(string description = "", bool readOnly = false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }
}