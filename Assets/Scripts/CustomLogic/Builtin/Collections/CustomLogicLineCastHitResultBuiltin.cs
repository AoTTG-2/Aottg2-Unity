namespace CustomLogic
{
    [CLType(Name = "LineCastHitResult", Abstract = true, Description = "The result of a Physics.LineCast")]
    partial class CustomLogicLineCastHitResultBuiltin : BuiltinClassInstance, ICustomLogicCopyable, ICustomLogicEquals
    {
        [CLProperty("true if the linecast hit a character", ReadOnly = true)]
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

        [CLProperty("true if the linecast hit a map object", ReadOnly = true)]
        public bool IsMapObject { get; set; }

        [CLProperty("The distance to the hit point", ReadOnly = true)]
        public float Distance { get; set; }

        [CLProperty("The point in world space where the linecast hit", ReadOnly = true)]
        public CustomLogicVector3Builtin Point { get; set; }

        [CLProperty("The normal of the surface the linecast hit", ReadOnly = true)]
        public CustomLogicVector3Builtin Normal { get; set; }

        [CLProperty("The collider that was hit", ReadOnly = true)]
        public BuiltinClassInstance Collider { get; set; }

        [CLProperty("The collider that was hit", ReadOnly = true)]
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

        [CLMethod("Creates a copy of this linecast hit result. Returns: A new LineCastHitResult with the same values.")]
        public object __Copy__()
        {
            return Copy();
        }

        [CLMethod("Checks if two linecast hit results are equal. Returns: True if the hit results are equal, false otherwise.")]
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

        [CLMethod("Gets the hash code of the linecast hit result. Returns: The hash code.")]
        public int __Hash__()
        {
            return Point.__Hash__() ^
                   Normal.__Hash__();
        }
    }
}
