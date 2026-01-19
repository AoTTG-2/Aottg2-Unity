using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// Enumeration of scroll elasticity (touch scroll behavior) values for ScrollView.
    /// </summary>
    [CLType(Name = "ScrollElasticityEnum", Static = true, Abstract = true)]
    partial class CustomLogicScrollElasticityEnum : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicScrollElasticityEnum() { }

        /// <summary>
        /// Clamped: scrolling is clamped at the content boundaries.
        /// </summary>
        [CLProperty]
        public static int Clamped => (int)ScrollView.TouchScrollBehavior.Clamped;

        /// <summary>
        /// Elastic: scrolling has elastic behavior when reaching boundaries.
        /// </summary>
        [CLProperty]
        public static int Elastic => (int)ScrollView.TouchScrollBehavior.Elastic;

        /// <summary>
        /// Unrestricted: scrolling is unrestricted and can go beyond boundaries.
        /// </summary>
        [CLProperty]
        public static int Unrestricted => (int)ScrollView.TouchScrollBehavior.Unrestricted;
    }
}
