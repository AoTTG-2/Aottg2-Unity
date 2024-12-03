namespace CustomLogic
{
    [CLType(InheritBaseMembers = true)]
    class CustomLogicLineCastHitResultBuiltin : CustomLogicBaseBuiltin, ICustomLogicCopyable, ICustomLogicEquals
    {
        [CLProperty(readOnly: true, description: "true if the linecast hit a character")]
        public bool IsCharacter { get; set; }

        [CLProperty(readOnly: true, description: "true if the linecast hit a map object")]
        public bool IsMapObject { get; set; }

        [CLProperty(readOnly: true, description: "The distance to the hit point")]
        public float Distance { get; set; }

        [CLProperty(readOnly: true, description: "The point in world space where the linecast hit")]
        public CustomLogicVector3Builtin Point { get; set; }

        [CLProperty(readOnly: true, description: "The normal of the surface the linecast hit")]
        public CustomLogicVector3Builtin Normal { get; set; }

        [CLProperty(readOnly: true, description: "The collider that was hit")]
        public CustomLogicBaseBuiltin Collider { get; set; }

        public CustomLogicLineCastHitResultBuiltin() : base("LineCastHitResult")
        {
        }

        public CustomLogicBaseBuiltin Copy()
        {
            return new CustomLogicLineCastHitResultBuiltin()
            {
                IsCharacter = IsCharacter,
                IsMapObject = IsMapObject,
                Point = Point.Copy() as CustomLogicVector3Builtin,
                Normal = Normal.Copy() as CustomLogicVector3Builtin,
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
                return IsCharacter == otherLineCastHitResult.IsCharacter &&
                       IsMapObject == otherLineCastHitResult.IsMapObject &&
                       Distance == otherLineCastHitResult.Distance &&
                       Point.__Eq__(otherLineCastHitResult.Point) &&
                       Normal.__Eq__(otherLineCastHitResult.Normal) &&
                       Collider.__Eq__(otherLineCastHitResult.Collider);
            }
            return false;
        }

        public int __Hash__()
        {
            return IsCharacter.GetHashCode() ^
                   IsMapObject.GetHashCode() ^
                   Distance.GetHashCode() ^
                   Point.__Hash__() ^
                   Normal.__Hash__() ^
                   Collider.__Hash__();
        }
    }
}