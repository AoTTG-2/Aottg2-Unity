using System.Collections;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of unity component that can be read.
    /// </summary>
    [CLType(Name = "UnityComponentEnum", Static = true, Abstract = true)]
    partial class CustomLogicUnityComponentEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicUnityComponentEnum() { }

        /// <summary>
        /// VideoPlayer unity component.
        /// </summary>
        [CLProperty]
        public static string VideoPlayer => "VideoPlayer";
    }
}