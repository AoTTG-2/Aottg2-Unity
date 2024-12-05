namespace CustomLogic
{
    [CLType(Abstract = true, InheritBaseMembers = true)]
    class CustomLogicLineCastHitResultBuiltin : CustomLogicClassInstanceBuiltin, ICustomLogicCopyable, ICustomLogicEquals
    {
        [CLProperty("true if the linecast hit a character", ReadOnly = true)]
        public bool IsCharacter;

        [CLProperty("true if the linecast hit a map object", ReadOnly = true)]
        public bool IsMapObject;

        [CLProperty("The distance to the hit point", ReadOnly = true)]
        public float Distance;

        [CLProperty("The point in world space where the linecast hit", ReadOnly = true)]
        public CustomLogicVector3Builtin Point;

        [CLProperty("The normal of the surface the linecast hit", ReadOnly = true)]
        public CustomLogicVector3Builtin Normal;

        [CLProperty("The collider that was hit", ReadOnly = true)]
        public CustomLogicClassInstanceBuiltin Collider;

        public CustomLogicLineCastHitResultBuiltin() : base("LineCastHitResult")
        {
        }

        public CustomLogicClassInstanceBuiltin Copy()
        {
            return new CustomLogicLineCastHitResultBuiltin()
            {
                IsCharacter = IsCharacter,
                IsMapObject = IsMapObject,
                Point = (CustomLogicVector3Builtin)Point.__Copy__(),
                Normal = (CustomLogicVector3Builtin)Normal.__Copy__(),
                Distance = Distance,
                Collider = Collider
            };
        }

        public object __Copy__()
        {
            return Copy();
        }

        public bool __Eq__(object other)
        {
            if (other is CustomLogicLineCastHitResultBuiltin otherLineCastHitResult)
            {
                return Point.__Eq__(otherLineCastHitResult.Point) &&
                       Normal.__Eq__(otherLineCastHitResult.Normal);
            }
            return false;
        }

        public int __Hash__()
        {
            return Point.__Hash__() ^
                   Normal.__Hash__();
        }
    }
}