namespace CustomLogic
{
    abstract class CustomLogicClassInstanceBuiltin : CustomLogicClassInstance
    {
        protected CustomLogicClassInstanceBuiltin()
        {
            Variables["Init"] = new BuiltinMethod((_, _, _) => null);
        }
    }
}
