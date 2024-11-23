namespace CustomLogic
{
    /// <summary>
    /// Represents a user defined method. Different from <see cref="BuiltinMethod"/>
    /// </summary>
    internal class UserMethod
    {
        public readonly CustomLogicClassInstance Owner;
        public readonly CustomLogicMethodDefinitionAst Ast;

        public UserMethod(CustomLogicClassInstance owner, CustomLogicMethodDefinitionAst ast)
        {
            Owner = owner;
            Ast = ast;
        }
    }
}