namespace CustomLogic
{
    class CustomLogicBaseAst
    {
        public CustomLogicAstType Type;
        public int Line;

        public CustomLogicBaseAst(CustomLogicAstType type, int line)
        {
            Type = type;
            Line = line;
        }
    }

    public enum CustomLogicAstType
    {
        Start,
        ClassDefinition,
        MethodDefinition,
        AssignmentExpression,
        MethodCallExpression,
        ClassInstantiateExpression,
        FieldExpression,
        PrimitiveExpression,
        BinopExpression,
        NotExpression,
        VariableExpression,
        ReturnExpression,
        WaitExpression,
        ConditionalExpression,
        ForExpression
    }
}
