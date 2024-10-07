using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicConditionalBlockAst: CustomLogicBlockAst
    {
        public CustomLogicBaseExpressionAst Condition;
        public CustomLogicToken Token;

        public CustomLogicConditionalBlockAst(CustomLogicToken token, int line): base(CustomLogicAstType.ConditionalExpression, line)
        {
            Token = token;
        }
    }
}
