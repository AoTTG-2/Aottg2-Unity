namespace CustomLogic
{
    abstract class CustomLogicClassInstanceBuiltin : CustomLogicClassInstance
    {
        // static BuiltinMethod _builtinMethod = new BuiltinMethod((_, _, _) => null);
        static CLMethodBinding<CustomLogicClassInstanceBuiltin> _init = new((_, _) => null);
        protected CustomLogicClassInstanceBuiltin(string className) : base(className)
        {
            Variables["Init"] = _init;
            Variables["IsCharacter"] = false;
        }
    }
}
