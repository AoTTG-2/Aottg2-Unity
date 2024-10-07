using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicNotExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Next;

        public CustomLogicNotExpressionAst(int line): base(CustomLogicAstType.NotExpression, line)
        {
        }
    }
}
