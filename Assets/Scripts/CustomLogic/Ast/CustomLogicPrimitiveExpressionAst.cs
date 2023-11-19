using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicPrimitiveExpressionAst: CustomLogicBaseExpressionAst
    {
        public object Value;

        public CustomLogicPrimitiveExpressionAst(object value, int line): base(CustomLogicAstType.PrimitiveExpression, line)
        {
            Value = value;
        }
    }
}
