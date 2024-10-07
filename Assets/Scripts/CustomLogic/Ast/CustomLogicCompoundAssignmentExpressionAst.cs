using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicCompoundAssignmentExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Left;
        public CustomLogicToken Operator;
        public CustomLogicBaseExpressionAst Right;

        public CustomLogicCompoundAssignmentExpressionAst(CustomLogicBaseExpressionAst left, CustomLogicToken @operator, int line): base(CustomLogicAstType.AssignmentExpression, line)
        {
            Left = left;
            Operator = @operator;
        }
    }
}
