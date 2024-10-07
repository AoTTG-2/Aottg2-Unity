using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicVariableExpressionAst: CustomLogicBaseExpressionAst
    {
        public string Name;

        public CustomLogicVariableExpressionAst(string name, int line): base(CustomLogicAstType.VariableExpression, line)
        {
            Name = name;
        }
    }
}
