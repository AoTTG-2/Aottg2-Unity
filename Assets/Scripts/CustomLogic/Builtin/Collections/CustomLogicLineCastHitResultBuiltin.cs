namespace CustomLogic
{
    /// <summary>
    /// The result of a Physics.LineCast
    /// </summary>
    [CLType(Name = "LineCastHitResult", Abstract = true)]
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

        [CLMethod]
        public object __Copy__()
        {
            return Copy();
        }

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

        [CLMethod]
        public int __Hash__()
        {
            return Point.__Hash__() ^
                   Normal.__Hash__();
        }
    }
}