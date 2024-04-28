using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicClassInstance
    {
        public Dictionary<string, object> Variables = new Dictionary<string, object>();
        public string ClassName;

        public CustomLogicClassInstance(string name)
        {
            ClassName = name;
        }

        public void Setvariable(string variableName, object value)
        {
            if (Variables.ContainsKey(variableName))
                Variables[variableName] = value;
            else
                Variables.Add(variableName, value);
        }

        public override string ToString()
        {
            return $"{ClassName} (CustomLogicClassInstance)";
        }
    }
}
