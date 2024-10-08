using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicContinueExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicContinueExpressionAst(int line): base(CustomLogicAstType.ContinueExpression, line)
        {
        }
    }
}
