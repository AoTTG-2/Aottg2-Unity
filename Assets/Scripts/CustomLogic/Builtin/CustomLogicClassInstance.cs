using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

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
        }

        public override string ToString()
        {
            return $"{ClassName} (CustomLogicClassInstance)";
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
                if (CustomLogicReflectioner.TryCreateProperty(ClassName, name, out var property))
                {
                    Variables[name] = property;
                    variable = property;
                    return true;
                }

                if (CustomLogicReflectioner.TryCreateMethod(ClassName, name, out var method))
                {
                    Variables[name] = method;
                    variable = method;
                    return true;
                }
            }

            variable = null;
            return false;
        }

        public bool HasVariable(string name)
        {
            return Variables.ContainsKey(name) ||
                   (CustomLogicBuiltinTypes.IsBuiltinType(ClassName) &&
                    CustomLogicBuiltinTypes.TypeMemberNames[ClassName].Contains(name));
        }

        /// <summary>
        /// Match the method signature to the parameters and call the method.
        /// Kwargs/Named Parameters are supported but are slower due to needing to build the relevant function signature.
        /// </summary>
        public static object InvokeMethod(MethodInfo method, object instance, List<object> args, Dictionary<string, object> kwargs)
        {
            if (kwargs.Count == 0)
                return method.Invoke(instance, args.ToArray());

            var paramInfos = method.GetParameters();
            var finalParameters = new object[paramInfos.Length];

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

            return method.Invoke(instance, finalParameters);
        }
    }
}
