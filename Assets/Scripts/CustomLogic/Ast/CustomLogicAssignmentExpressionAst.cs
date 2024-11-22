using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicAssignmentExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Left;
        public CustomLogicToken Operator;
        public CustomLogicBaseExpressionAst Right;

        public CustomLogicAssignmentExpressionAst(CustomLogicBaseExpressionAst left, CustomLogicToken @operator, int line): base(CustomLogicAstType.AssignmentExpression, line)
        {
            Left = left;
            Operator = @operator;
        }
    }
}
