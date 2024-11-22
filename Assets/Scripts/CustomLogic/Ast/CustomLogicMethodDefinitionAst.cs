using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicMethodDefinitionAst: CustomLogicBlockAst
    {
        public List<string> ParameterNames = new List<string>();
        public bool Coroutine;
        public string Name;
        public CustomLogicClassInstance Owner;

        public CustomLogicMethodDefinitionAst(int line, bool coroutine = false) : base(CustomLogicAstType.MethodDefinition, line)
        {
            Coroutine = coroutine;
        }

        public override string ToString()
        {
            return $"{(Coroutine ? "coroutine" : "function")} {Owner?.ClassName}.{Name}({string.Join(", ", ParameterNames)})";
        }
    }
}
