using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicBlockAst: CustomLogicBaseAst
    {
        public List<CustomLogicBaseAst> Statements = new List<CustomLogicBaseAst>();

        public CustomLogicBlockAst(CustomLogicAstType type, int line): base(type, line)
        {
        }
    }
}
