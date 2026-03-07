namespace CustomLogic
{
    class CustomLogicUnaryExpressionAst : CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Next;
        public CustomLogicToken Token;

        public CustomLogicUnaryExpressionAst(CustomLogicToken token, int line) : base(CustomLogicAstType.UnaryExpression, line)
        {
            Token = token;
        }
    }
}
