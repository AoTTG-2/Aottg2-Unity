namespace CustomLogic
{
    [BuiltinTypeManager]
    internal partial class CustomLogicBuiltinTypes
    {
        public static bool IsBuiltinType(string typeName) => TypeNames.Contains(typeName);
        public static bool IsAbstract(string typeName) => AbstractTypeNames.Contains(typeName);
    }
}