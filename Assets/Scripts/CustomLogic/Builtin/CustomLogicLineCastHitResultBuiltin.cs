namespace CustomLogic
{
    class CustomLogicLineCastHitResultBuiltin : CustomLogicStructBuiltin
    {
        public bool IsCharacter;
        public bool IsMapObject;
        public float Distance;
        public CustomLogicVector3Builtin Point;
        public CustomLogicVector3Builtin Normal;
        public CustomLogicBaseBuiltin Collider;

        public CustomLogicLineCastHitResultBuiltin() : base("LineCastHitResult")
        {
        }

        public override object GetField(string name)
        {
            if (name == "IsCharacter")
                return IsCharacter;
            if (name == "IsMapObject")
                return IsMapObject;
            else if (name == "Point")
                return Point;
            else if (name == "Normal")
                return Normal;
            else if (name == "Distance")
                return Distance;
            else if (name == "Collider")
                return Collider;

            return base.GetField(name);
        }

        public override CustomLogicStructBuiltin Copy()
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
    }
}