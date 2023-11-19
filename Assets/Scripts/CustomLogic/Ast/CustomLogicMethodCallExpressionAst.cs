using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicMethodCallExpressionAst: CustomLogicBaseExpressionAst
    {
        public string Name;
        public List<CustomLogicBaseAst> Parameters = new List<CustomLogicBaseAst>();
        public CustomLogicBaseExpressionAst Left;

        public CustomLogicMethodCallExpressionAst(string name, int line): base(CustomLogicAstType.MethodCallExpression, line)
        {
            Name = name;
        }
    }
}
