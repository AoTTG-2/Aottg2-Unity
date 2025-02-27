using System.Collections.Generic;

namespace CustomLogic
{
    class CustomLogicClassInstance
    {
        // Always true for class instances, but for components it can be toggled
        // The reason this is here instead of in CustomLogicComponentInstance is to
        // avoid an extra type check in the evaluator
        public bool Enabled = true;

        public string ClassName;
        public Dictionary<string, object> Variables = new Dictionary<string, object>();
        public bool Inited = false;

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
