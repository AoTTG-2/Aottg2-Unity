using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicClassDefinitionAst: CustomLogicBaseAst
    {
        public List<CustomLogicAssignmentExpressionAst> Assignments = new List<CustomLogicAssignmentExpressionAst>();
        public Dictionary<string, CustomLogicMethodDefinitionAst> Methods = new Dictionary<string, CustomLogicMethodDefinitionAst>();
        public CustomLogicToken Token;

        public CustomLogicClassDefinitionAst(CustomLogicToken token, int line): base(CustomLogicAstType.ClassDefinition, line)
        {
            Token = token;
            Methods.Add("Init", new CustomLogicMethodDefinitionAst(0));
        }

        public CustomLogicMethodDefinitionAst GetInit()
        {
            return Methods["Init"];
        }

        public void AddMethod(string methodName, CustomLogicMethodDefinitionAst methodAst)
        {
            if (Methods.ContainsKey(methodName))
                Methods[methodName] = methodAst;
            else
                Methods.Add(methodName, methodAst);
        }
    }
}
