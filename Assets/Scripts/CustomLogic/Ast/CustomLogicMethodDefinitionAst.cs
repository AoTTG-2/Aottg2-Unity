using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicMethodDefinitionAst: CustomLogicBlockAst
    {
        public List<string> ParameterNames = new List<string>();
        public bool Coroutine;

        public CustomLogicMethodDefinitionAst(int line, bool coroutine = false) : base(CustomLogicAstType.MethodDefinition, line)
        {
            Coroutine = coroutine;
        }
    }
}
