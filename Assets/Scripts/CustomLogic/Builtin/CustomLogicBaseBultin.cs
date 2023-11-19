using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CustomLogicBaseBuiltin: CustomLogicClassInstance
    {
        public CustomLogicBaseBuiltin(string name): base(name)
        {
        }

        public virtual object CallMethod(string name, List<object> parameters)
        {
            return null;
        }
        public virtual object GetField(string name)
        {
            if (name == "Type")
                return ClassName;
            if (name == "IsCharacter")
                return false;
            return null;
        }
        public virtual void SetField(string name, object value)
        {
        }
    }
}
