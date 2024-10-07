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
            if (name == "Init")
                return null;
            throw new System.Exception("No method named " + name + " found.");
        }

        public virtual object GetField(string name)
        {
            if (name == "Type")
                return ClassName;
            if (name == "IsCharacter")
                return false;
            throw new System.Exception("No field named " + name + " found.");
        }
        public virtual void SetField(string name, object value)
        {
            throw new System.Exception("No field named " + name + " found.");
        }
    }
}
