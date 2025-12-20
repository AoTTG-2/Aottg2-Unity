using System;
using System.Diagnostics;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class CLConstructorAttribute : Attribute { }
}