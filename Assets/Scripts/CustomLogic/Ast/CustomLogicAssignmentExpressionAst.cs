using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicAssignmentExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Left;
        public CustomLogicBaseExpressionAst Right;

        public CustomLogicAssignmentExpressionAst(CustomLogicBaseExpressionAst left, int line): base(CustomLogicAstType.AssignmentExpression, line)
        {
            Left = left;
        }
    }
}
