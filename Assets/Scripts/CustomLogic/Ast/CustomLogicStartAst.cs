using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    internal class CustomLogicStartAst: CustomLogicBaseAst
    {
        public Dictionary<string, CustomLogicClassDefinitionAst> Classes = new Dictionary<string, CustomLogicClassDefinitionAst>();
        
        /// <summary>
        /// Maps class names to their defining namespace for namespace-aware resolution.
        /// Key: ClassName, Value: Namespace (source file type)
        /// </summary>
        public Dictionary<string, CustomLogicSourceType> ClassNamespaces = new Dictionary<string, CustomLogicSourceType>();

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
            
            // Track the namespace if available
            if (classAst.Namespace.HasValue)
            {
                ClassNamespaces[className] = classAst.Namespace.Value;
            }
        }
    }
}
