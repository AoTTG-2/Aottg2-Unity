namespace CustomLogic
{
    abstract class CustomLogicClassInstanceBuiltin : CustomLogicClassInstance
    {
        protected CustomLogicClassInstanceBuiltin(string className) : base(className)
        {
            Variables["Init"] = new BuiltinMethod((_, _, _) => null);
        }
    }
}
