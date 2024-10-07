using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicReturnExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst ReturnValue;

        public CustomLogicReturnExpressionAst(CustomLogicBaseExpressionAst returnValue, int line): base(CustomLogicAstType.ReturnExpression, line)
        {
            ReturnValue = returnValue;
        }
    }
}
