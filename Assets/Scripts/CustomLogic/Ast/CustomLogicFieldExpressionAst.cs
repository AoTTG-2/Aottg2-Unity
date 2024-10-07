using System;
using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicFieldExpressionAst: CustomLogicBaseExpressionAst
    {
        public string FieldName;
        public CustomLogicBaseExpressionAst Left;

        public CustomLogicFieldExpressionAst(string name, int line): base(CustomLogicAstType.FieldExpression, line)
        {
            FieldName = name;
        }
    }
}
