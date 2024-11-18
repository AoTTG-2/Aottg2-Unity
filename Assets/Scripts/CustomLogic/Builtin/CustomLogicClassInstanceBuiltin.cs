using System;

namespace CustomLogic
{
    class CustomLogicClassInstanceBuiltin : CustomLogicClassInstance
    {
        public CustomLogicClassInstanceBuiltin(string name) : base(name)
        {
            Variables["Init"] = new BuiltinMethod((_, _, _) => null);
        }
    }
}
