using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CustomLogicBaseExpressionAst: CustomLogicBaseAst
    {
        public CustomLogicBaseExpressionAst(CustomLogicAstType type, int line): base(type, line)
        {
        }
    }
}
