using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    class CustomLogicStartAst: CustomLogicBaseAst
    {
        public Dictionary<string, CustomLogicClassDefinitionAst> Classes = new Dictionary<string, CustomLogicClassDefinitionAst>();

        public CustomLogicStartAst(): base(CustomLogicAstType.Start, 0)
        {
        }

        public void AddEmptyMain()
        {
            AddClass("Main", new CustomLogicClassDefinitionAst(new CustomLogicToken(CustomLogicTokenType.Symbol, (int)CustomLogicSymbol.Class, 0), 0));
        }

        public void AddClass(string className, CustomLogicClassDefinitionAst classAst)
        {
            if (Classes.ContainsKey(className))
            {
                if (className != "Main")
                    Classes[className] = classAst;
            }
            else
                Classes.Add(className, classAst);
        }
    }
}
