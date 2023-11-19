using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicForBlockAst: CustomLogicBlockAst
    {
        public CustomLogicVariableExpressionAst Variable;
        public CustomLogicBaseExpressionAst Iterable;

        public CustomLogicForBlockAst(int line): base(CustomLogicAstType.ForExpression, line)
        {
        }
    }
}
