using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        public static List<KeyValuePair<string, object>> builtinCache = new List<KeyValuePair<string, object>>();
        public static bool builtinCacheDirty = true;

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

        public void RegisterBuiltinClass(Type type)
        {
            if (builtinCacheDirty)
            {
                builtinCache.Clear();
                RegisterFields(type);
                RegisterProperties(type);
                RegisterMethods(type);
                builtinCacheDirty = false;
            }

            // init from cache
            foreach (var pair in builtinCache)
            {
                Variables[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Register all CLFields on the class.
        /// </summary>
        /// <exception cref="Exception">Getters/Setters will throw relevant exceptions for readonly/unimplemented behavior</exception>
        private void RegisterFields(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            fields = fields.Where(p => p.GetCustomAttribute<CLField>() != null).ToArray();
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<CLField>();
                attribute.Description = string.Empty;
                bool hasSetter = field.IsInitOnly == false && field.IsLiteral == false && field.IsStatic == false && attribute.ReadOnly == false;
                Func<object> getter = () => field.GetValue(this);
                Action<object> setter = hasSetter ? value => field.SetValue(this, value) : value => throw new Exception($"Property {field.Name} is read-only");
                var builtinField = new BuiltinField(getter, setter);

                builtinCache.Add(new KeyValuePair<string, object>(field.Name, builtinField));
            }
        }

        /// <summary>
        /// Register all CLProperties on the class.
        /// </summary>
        /// <exception cref="Exception">Getters/Setters will throw relevant exceptions for readonly/unimplemented behavior</exception>
        private void RegisterProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            properties = properties.Where(p => p.GetCustomAttribute<CLProperty>() != null).ToArray();
            foreach (var property in properties)
            {
                // Get the attribute
                var attribute = property.GetCustomAttribute<CLProperty>();
                attribute.Description = string.Empty;

                bool hasGetter = property.GetGetMethod() != null;
                bool hasSetter = property.GetSetMethod() != null && attribute.ReadOnly == false;

                Func<object> getter = hasGetter? () => property.GetValue(this) : () => throw new Exception($"Property {property.Name} has no Get Method");
                Action<object> setter = hasSetter? value => property.SetValue(this, value) : value => throw new Exception($"Property {property.Name} is read-only");
                var builtinField = new BuiltinField(getter, setter);

                builtinCache.Add(new KeyValuePair<string, object>(property.Name, builtinField));
            }
        }

        /// <summary>
        /// Register CLMethods on the class.
        /// We will not support function overloading due to our language lacking Types.
        /// Methods will be passed a object[] parameters and a Dictionary<string, object> kwargs from the base language.
        /// InvokeMethod will be responsible for generating the function signature and calling.
        /// </summary>
        private void RegisterMethods(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            methods = methods.Where(p => p.GetCustomAttribute<CLMethod>() != null).ToArray();
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<CLMethod>();
                attribute.Description = string.Empty;
                var parameters = method.GetParameters();
                //var methodSignature = $"{method.Name}({string.Join(",", parameters.Select(p => p.ParameterType.Name))})";
                var builtinFunction = new BuiltinFunction((instance, args, kwargs) => InvokeMethod(method, instance, args, kwargs));

                builtinCache.Add(new KeyValuePair<string, object>(method.Name, builtinFunction));
            }
        }

        /// <summary>
        /// Match the method signature to the parameters and call the method.
        /// Kwargs/Named Parameters are supported but are slower due to needing to build the relevant function signature.
        /// </summary>
        private object InvokeMethod(MethodInfo method, object instance, object[] args, Dictionary<string, object> kwargs)
        {
            if (kwargs.Count == 0)
                return method.Invoke(instance, args);

            var paramInfos = method.GetParameters();
            var finalParameters = new object[paramInfos.Length];

            for (int i = 0; i < paramInfos.Length; i++)
            {
                if (i < args.Length)
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
