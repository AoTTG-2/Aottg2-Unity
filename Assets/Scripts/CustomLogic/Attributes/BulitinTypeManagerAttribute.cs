using System;
using System.Diagnostics;

namespace CustomLogic
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class BuiltinTypeManagerAttribute : Attribute { }
}