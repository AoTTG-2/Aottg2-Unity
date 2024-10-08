using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicBreakExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBreakExpressionAst(int line): base(CustomLogicAstType.BreakExpression, line)
        {
        }
    }
}
