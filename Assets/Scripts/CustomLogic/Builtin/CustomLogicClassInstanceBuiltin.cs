namespace CustomLogic
{
    abstract class CustomLogicClassInstanceBuiltin : CustomLogicClassInstance
    {
        static BuiltinMethod _builtinMethod = new BuiltinMethod((_, _, _) => null);
        protected CustomLogicClassInstanceBuiltin(string className) : base(className)
        {
            Variables["Init"] = _builtinMethod;
            Variables["IsCharacter"] = false;
        }
    }
}
