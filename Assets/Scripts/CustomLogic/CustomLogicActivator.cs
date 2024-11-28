using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CustomLogic
{
    internal class CustomLogicActivator
    {
        private delegate CustomLogicClassInstanceBuiltin ClassInstanceActivator(object[] args);

        private static CustomLogicActivator instance;
        private static CustomLogicActivator Instance => instance ??= new CustomLogicActivator();

        private readonly Dictionary<string, ConstructorInfo> _constructors = new();
        private readonly Dictionary<string, ClassInstanceActivator> _activators = new();
        private readonly HashSet<string> _parameterlessConstructors = new();
        
        /// <summary>
        /// Creates a new instance of <see cref="CustomLogicClassInstanceBuiltin"/> (or subclasses).
        /// If the type has a constructor that takes an array of objects (i.e. Ctor(object[])), that constructor will be used,
        /// otherwise the type must have a parameterless constructor which will be used
        /// </summary>
        public static CustomLogicClassInstanceBuiltin CreateInstance(string typeName, object[] args)
        {
            var type = CustomLogicBuiltinTypes.Types[typeName];

            if (TryCreateParameterlessInstance(typeName, out var classInstance))
                return classInstance;

            var ctor = GetConstructor(type);
            if (ctor == null)
            {
                if (TryCreateParameterlessInstance(typeName, out classInstance))
                    return classInstance;

                throw new Exception($"{typeName} must have a parameterless constructor or a constructor that takes an array of objects (object[])");
            }

            ClassInstanceActivator activator;
            if (Instance._activators.ContainsKey(typeName))
                activator = Instance._activators[typeName];
            else
            {
                activator = CreateActivator(ctor);
                Instance._activators[typeName] = activator;
            }

            return CreateInstanceUsingActivator(args, activator);
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            if (Instance._constructors.TryGetValue(type.Name, out var constructor))
                return constructor;

            var ctor = type.GetConstructor(new[] { typeof(object[]) });

            if (ctor == null)
            {
                Instance._parameterlessConstructors.Add(type.Name);
                return null;
            }
            
            Instance._constructors.Add(type.Name, ctor);
            return ctor;
        }

        private static ClassInstanceActivator CreateActivator(ConstructorInfo ctor)
        {
            var param = Expression.Parameter(typeof(object[]), "parameterValues");
            var newExp = Expression.New(ctor, param);
            var lambda = Expression.Lambda(typeof(ClassInstanceActivator), newExp, param);

            var compiled = (ClassInstanceActivator)lambda.Compile();
            return compiled;
        }

        private static bool TryCreateParameterlessInstance(string typeName, out CustomLogicClassInstanceBuiltin classInstance)
        {
            if (!Instance._parameterlessConstructors.Contains(typeName))
            {
                classInstance = null;
                return false;
            }
            
            var type = CustomLogicBuiltinTypes.Types[typeName];
            
            // Activator.CreateInstance is faster for parameterless constructors
            classInstance = (CustomLogicClassInstanceBuiltin)Activator.CreateInstance(type);
            return true;
        }

        private static CustomLogicClassInstanceBuiltin CreateInstanceUsingActivator(object[] args,
            ClassInstanceActivator activator)
        {
            var classInstance = activator(args);
            return classInstance;
        }
    }
}