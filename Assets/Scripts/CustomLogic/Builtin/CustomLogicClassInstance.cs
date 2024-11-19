using System;
using System.Collections.Generic;
using System.Reflection;

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
                var isProperty = CustomLogicReflectioner.TryCreateProperty(ClassName, name, out var property);
                var isMethod = CustomLogicReflectioner.TryCreateMethod(ClassName, name, out var method);
                
                if (isProperty || isMethod)
                {
                    variable = isProperty ? property : method;
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
                    var isProperty = CustomLogicReflectioner.TryCreateProperty(c, name, out var property);
                    var isMethod = CustomLogicReflectioner.TryCreateMethod(c, name, out var method);
                
                    if (isProperty || isMethod)
                    {
                        variable = isProperty ? property : method;
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
