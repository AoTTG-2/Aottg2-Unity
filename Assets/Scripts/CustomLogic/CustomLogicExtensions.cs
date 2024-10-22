namespace CustomLogic
{
    static class CustomLogicExtensions
    {
        public static bool IsExtension(this CustomLogicClassDefinitionAst classAst)
        {
            return (int)classAst.Token.Value == (int)CustomLogicSymbol.Extension;
        }
        
        public static bool IsCutscene(this CustomLogicClassDefinitionAst classAst)
        {
            return (int)classAst.Token.Value == (int)CustomLogicSymbol.Cutscene;
        }

        public static bool TokenValueIsSymbol(this CustomLogicToken token, CustomLogicSymbol symbol)
        {
            return (int)token.Value == (int)symbol;
        }
    }
}