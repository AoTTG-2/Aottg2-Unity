using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicClassInstantiateExpressionAst: CustomLogicBaseExpressionAst
    {
        public string Name;
        public List<CustomLogicBaseAst> Parameters = new List<CustomLogicBaseAst>();
        public CustomLogicBaseExpressionAst Left;

        public CustomLogicClassInstantiateExpressionAst(string name, int line): base(CustomLogicAstType.ClassInstantiateExpression, line)
        {
            Name = name;
        }
    }
}
