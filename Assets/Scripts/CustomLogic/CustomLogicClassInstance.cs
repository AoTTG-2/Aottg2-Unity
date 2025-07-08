using System;
using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CustomLogicClassInstance
    {
        // Always true for class instances, but for components it can be toggled
        // The reason this is here instead of in CustomLogicComponentInstance is to
        // avoid an extra type check in the evaluator
        public bool Enabled = true;

        public bool Inited = false;

        public readonly Dictionary<string, object> Variables;

        protected CustomLogicClassInstance()
        {
            Variables = new Dictionary<string, object>();
        }

        public abstract string ClassName { get; }
        public virtual bool LookupBaseClassForVariables { get; } = true;

        public override string ToString()
        {
            const string methodName = nameof(ICustomLogicToString.__Str__);
            var evaluator = CustomLogicManager.Evaluator;

            if (evaluator != null && HasVariable(methodName))
            {
                return (string)evaluator.EvaluateMethod(this, methodName);
            }

            return $"(CustomLogicClassInstance){ClassName}";
        }

        // TODO: Expose System.HashCode.Combine(propA, propB) so that users can properly override __Hash__
        public override int GetHashCode()
        {
            const string methodName = nameof(ICustomLogicEquals.__Hash__);
            var evaluator = CustomLogicManager.Evaluator;

            if (evaluator != null && HasVariable(methodName))
            {
                return (int)evaluator.EvaluateMethod(this, methodName);
            }

            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            const string methodName = nameof(ICustomLogicEquals.__Eq__);
            var evaluator = CustomLogicManager.Evaluator;

            if (evaluator != null && HasVariable(methodName))
            {
                return (bool)evaluator.EvaluateMethod(this, methodName, new object[] { obj });
            }

            return base.Equals(obj);
        }

        public object GetVariable(string name)
        {
            if (TryGetVariable(name, out var variable))
                return variable;

            throw new Exception($"Variable {name} not found in class {ClassName}");
        }

        public bool TryGetVariable(string name, out object variable)
        {
            if (Variables.TryGetValue(name, out var value))
            {
                variable = value;
                return true;
            }

            var c = ClassName;
            while (CustomLogicBuiltinTypes.IsBuiltinType(c))
            {
                if (CustomLogicBuiltinTypes.MemberNames[c].Contains(name))
                {
                    if (CLBindingCache.GetOrCreateBinding(c, name, out var binding))
                    {
                        variable = binding;
                        Variables[name] = binding;
                        return true;
                    }
                }
                else if (LookupBaseClassForVariables && CustomLogicBuiltinTypes.BaseTypeNames.ContainsKey(c))
                    c = CustomLogicBuiltinTypes.BaseTypeNames[c];
                else
                    break;
            }

            variable = null;
            return false;
        }

        public bool HasVariable(string name)
        {
            if (Variables.ContainsKey(name))
                return true;

            var c = ClassName;
            while (CustomLogicBuiltinTypes.IsBuiltinType(c))
            {
                if (CustomLogicBuiltinTypes.MemberNames[c].Contains(name))
                    return true;
                else if (LookupBaseClassForVariables && CustomLogicBuiltinTypes.BaseTypeNames.ContainsKey(c))
                    c = CustomLogicBuiltinTypes.BaseTypeNames[c];
                else
                    break;
            }

            return false;
        }
    }
}
