using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicBinopExpressionAst: CustomLogicBaseExpressionAst
    {
        public CustomLogicBaseExpressionAst Left;
        public CustomLogicBaseExpressionAst Right;
        public CustomLogicToken Token;

        public CustomLogicBinopExpressionAst(CustomLogicToken token, int line): base(CustomLogicAstType.BinopExpression, line)
        {
            Token = token;
        }
    }
}
