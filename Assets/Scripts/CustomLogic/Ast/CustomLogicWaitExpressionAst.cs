using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicWaitExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst WaitTime;

        public CustomLogicWaitExpressionAst(CustomLogicBaseExpressionAst waitTime, int line): base(CustomLogicAstType.WaitExpression, line)
        {
            WaitTime = waitTime;
        }
    }
}
