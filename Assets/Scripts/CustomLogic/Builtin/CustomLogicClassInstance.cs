using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using Photon.Pun;
using Unity.VisualScripting;

namespace CustomLogic
{
    class CustomLogicClassInstance
    {
        // Always true for class instances, but for components it can be toggled
        // The reason this is here instead of in CustomLogicComponentInstance is to
        // avoid an extra type check in the evaluator
        public bool Enabled = true;

        public string ClassName;
        public readonly Dictionary<string, object> Variables = new();

        public CustomLogicClassInstance(string name)
        {
            ClassName = name;
            Variables["Type"] = name;
        }

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
                return (bool)evaluator.EvaluateMethod(this, methodName, new List<object> { obj });
            }

            return base.Equals(obj);
        }

        public object GetVariable(string name)
        {
            if (TryGetVariable(name, out var variable))
                return variable;
            
            throw new Exception($"Variable {name} not found in class {ClassName}");
        }
        
        /// <summary>
        /// Tries to get a variable from the class.
        /// If the variable is in the Variables dictionary, it is returned immediately,
        /// Otherwise it tries to create and cache it via reflection
        /// </summary>
        /// <returns>true if the variable was found or created, false otherwise</returns>
        public bool TryGetVariable(string name, out object variable)
        {
            if (Variables.TryGetValue(name, out var value))
            {
                variable = value;
                return true;
            }

            if (CustomLogicBuiltinTypes.IsBuiltinType(ClassName) && CustomLogicBuiltinTypes.TypeMemberNames[ClassName].Contains(name))
            {
                var isField = CustomLogicReflectioner.GetOrCreateField(ClassName, name, out var field);
                var isProperty = CustomLogicReflectioner.GetOrCreateProperty(ClassName, name, out var property);
                var isMethod = CustomLogicReflectioner.GetOrCreateMethod(ClassName, name, out var method);
                
                if (isField || isProperty || isMethod)
                {
                    variable = isField ? field : isProperty ? property : method;
                    Variables[name] = variable;
                    return true;
                }
            }
            
            var c = ClassName;
            
            // Recursively try to get the variable from the base classes
            while (CustomLogicBuiltinTypes.IsBuiltinType(c) && CustomLogicBuiltinTypes.BaseTypeNames.ContainsKey(c))
            {
                c = CustomLogicBuiltinTypes.BaseTypeNames[c];
                if (CustomLogicBuiltinTypes.TypeMemberNames[c].Contains(name))
                {
                    var isField = CustomLogicReflectioner.GetOrCreateField(c, name, out var field);
                    var isProperty = CustomLogicReflectioner.GetOrCreateProperty(c, name, out var property);
                    var isMethod = CustomLogicReflectioner.GetOrCreateMethod(c, name, out var method);
                
                    if (isField || isProperty || isMethod)
                    {
                        variable = isField ? field : isProperty ? property : method;
                        Variables[name] = variable;
                        return true;
                    }
                }
            }

            variable = null;
            return false;
        }

        public bool HasVariable(string name)
        {
            if (Variables.ContainsKey(name) ||
                (CustomLogicBuiltinTypes.IsBuiltinType(ClassName)
                 && CustomLogicBuiltinTypes.TypeMemberNames[ClassName].Contains(name)))
            {
                return true;
            }

            var c = ClassName;
            while (CustomLogicBuiltinTypes.IsBuiltinType(c) && CustomLogicBuiltinTypes.BaseTypeNames.ContainsKey(c))
            {
                c = CustomLogicBuiltinTypes.BaseTypeNames[c];
                if (CustomLogicBuiltinTypes.TypeMemberNames[c].Contains(name))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Match the method signature to the parameters and call the method.
        /// Kwargs/Named Parameters are supported but are slower due to needing to build the relevant function signature.
        /// </summary>
        public static object InvokeMethod(MethodInfo method, object instance, List<object> args, Dictionary<string, object> kwargs)
        {
            // if (kwargs.Count == 0)
            //     return method.Invoke(instance, args.ToArray());

            var paramInfos = method.GetCachedParemeters();

            
            var finalParameters = ArrayPool<object>.New(paramInfos.Length);

            for (int i = 0; i < paramInfos.Length; i++)
            {
                if (i < args.Count)
                {
                    finalParameters[i] = args[i];
                }
                else if (kwargs.ContainsKey(paramInfos[i].Name))
                {
                    finalParameters[i] = kwargs[paramInfos[i].Name];
                }
                else if (paramInfos[i].IsOptional)
                {
                    finalParameters[i] = paramInfos[i].DefaultValue;
                }
            }

            var result = method.Invoke(instance, finalParameters);
            ArrayPool<object>.Free(finalParameters);
            return result;
        }
    }
}
