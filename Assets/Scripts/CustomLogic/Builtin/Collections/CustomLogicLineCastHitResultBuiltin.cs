namespace CustomLogic
{
    /// <summary>
    /// The result of a Physics.LineCast.
    /// </summary>
    [CLType(Name = "LineCastHitResult", Abstract = true)]
    partial class CustomLogicLineCastHitResultBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        /// <summary>
        /// true if the linecast hit a character.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public bool IsCharacter
        {
            get
            {
                if (Variables.TryGetValue("IsCharacter", out var value) && value is bool boolValue)
                {
                    return boolValue;
                }
                return false;
            }
            set
            {
                Variables["IsCharacter"] = value;
            }
        }

        /// <summary>
        /// true if the linecast hit a map object.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public bool IsMapObject { get; set; }

        /// <summary>
        /// The distance to the hit point.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public float Distance { get; set; }

        /// <summary>
        /// The point in world space where the linecast hit.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public CustomLogicVector3Builtin Point { get; set; }

        /// <summary>
        /// The normal of the surface the linecast hit.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public CustomLogicVector3Builtin Normal { get; set; }

        /// <summary>
        /// The collider that was hit.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public BuiltinClassInstance Collider { get; set; }

        /// <summary>
        /// The collider that was hit.
        /// </summary>
        [CLProperty(ReadOnly = true)]
        public CustomLogicColliderBuiltin ColliderInfo { get; set; }

        public BuiltinClassInstance Copy()
        {
            return new CustomLogicLineCastHitResultBuiltin()
            {
                IsCharacter = IsCharacter,
                IsMapObject = IsMapObject,
                Point = (CustomLogicVector3Builtin)Point.__Copy__(),
                Normal = (CustomLogicVector3Builtin)Normal.__Copy__(),
                Distance = Distance,
                Collider = Collider,
                ColliderInfo = ColliderInfo
            };
        }

        /// <summary>
        /// Creates a copy of this linecast hit result.
        /// </summary>
        /// <returns>A new LineCastHitResult with the same values.</returns>
        [CLMethod]
        public object __Copy__()
        {
            return Copy();
        }

        /// <summary>
        /// Checks if two linecast hit results are equal.
        /// </summary>
        /// <returns>True if the hit results are equal, false otherwise.</returns>
        [CLMethod]
        public bool __Eq__(object self, object other)
        {
            return (self, other) switch
            {
                (CustomLogicLineCastHitResultBuiltin selfLineCastHitResult, CustomLogicLineCastHitResultBuiltin otherLineCastHitResult) =>
                     selfLineCastHitResult.Point.__Eq__(selfLineCastHitResult.Point, otherLineCastHitResult.Point) &&
                     selfLineCastHitResult.Normal.__Eq__(selfLineCastHitResult.Normal, otherLineCastHitResult.Normal),
                _ => false
            };
        }

        /// <summary>
        /// Gets the hash code of the linecast hit result.
        /// </summary>
        /// <returns>The hash code.</returns>
        [CLMethod]
        public int __Hash__()
        {
            return Point.__Hash__() ^
                   Normal.__Hash__();
        }
    }
}
