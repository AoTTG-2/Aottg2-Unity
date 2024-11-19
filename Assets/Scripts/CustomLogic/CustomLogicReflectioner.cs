using System;
using System.Collections.Generic;
using System.Reflection;

namespace CustomLogic
{
    class CustomLogicReflectioner
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        
        private static CustomLogicReflectioner instance;

        private static CustomLogicReflectioner Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = new CustomLogicReflectioner();
                return instance;
            }
        }

        private readonly Dictionary<string, Dictionary<string, BuiltinProperty>> _fields = new();
        private readonly Dictionary<string, Dictionary<string, BuiltinProperty>> _properties = new();
        private readonly Dictionary<string, Dictionary<string, BuiltinMethod>> _methods = new();
        
        private static readonly Type IntType = typeof(int);
        private static readonly Type FloatType = typeof(float);
        
        /// <summary>
        /// <see cref="GenericGetDelegate{TClass,TResult}"/> as a MethodInfo (not generic yet)
        /// </summary>
        private static readonly MethodInfo GenericGetDelegateMethodInfo =
            typeof(CustomLogicReflectioner).GetMethod(nameof(GenericGetDelegate), Flags);
        
        /// <summary>
        /// <see cref="GenericSetDelegate{TClass,TResult}"/> as a MethodInfo (not generic yet)
        /// </summary>
        private static readonly MethodInfo GenericSetDelegateMethodInfo =
            typeof(CustomLogicReflectioner).GetMethod(nameof(GenericSetDelegate), Flags);
        
        /// <summary>
        /// Converts a generic Func&lt;TClass, TResult&gt; to a Func&lt;object, object&gt;
        /// </summary>
        private static Func<object, object> GenericGetDelegate<TClass, TResult>(Func<TClass, TResult> func)
            => classInstance => func((TClass)classInstance);
        
        /// <summary>
        /// Converts a generic Action&lt;TClass, TResult&gt; to an Action&lt;object, object&gt;
        /// </summary>
        private static Action<object, object> GenericSetDelegate<TClass, TResult>(Action<TClass, TResult> func)
        {
            return (classInstance, result) =>
            {
                var res = result;
                if (typeof(TResult) == IntType)
                    res = result.UnboxToInt();
                else if (typeof(TResult) == FloatType)
                    res = result.UnboxToFloat();
                
                func((TClass)classInstance, (TResult)res);
            };
        }

        public static Dictionary<string, Dictionary<string, BuiltinProperty>> Fields => Instance._fields;
        public static Dictionary<string, Dictionary<string, BuiltinProperty>> Properties => Instance._properties;
        public static Dictionary<string, Dictionary<string, BuiltinMethod>> Methods => Instance._methods;
        
        // todo: handle static properties (unlike instance properties, their type is Func<TPropertyType>)

        /// <summary>
        /// Tries to create a BuiltinField from a field of a builtin type
        /// </summary>
        /// <param name="typeName">Name of the builtin type</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="field">Output BuiltinField</param>
        /// <returns>true if the field was found or created, false otherwise</returns>
        public static bool GetOrCreateField(string typeName, string fieldName, out BuiltinProperty field)
        {
            if (Instance._fields.ContainsKey(typeName))
            {
                if (Instance._fields[typeName].ContainsKey(fieldName))
                {
                    field = Instance._fields[typeName][fieldName];
                    return true;
                }
            }
            
            if (CustomLogicBuiltinTypes.TypeMemberNames[typeName].Contains(fieldName) == false)
            {
                field = default;
                return false;
            }
            
            var type = CustomLogicBuiltinTypes.Types[typeName];
            var fieldInfo = type.GetField(fieldName, Flags);

            if (fieldInfo == null)
            {
                field = default;
                return false;
            }
            
            var attribute = fieldInfo.GetCustomAttribute<CLPropertyAttribute>();
            attribute.ClearDescription();
                
            var hasSetter = fieldInfo.IsInitOnly == false && fieldInfo.IsLiteral == false && fieldInfo.IsStatic == false &&
                            attribute.ReadOnly == false;

            var builtinField = new BuiltinProperty(Getter, hasSetter ? Setter : null);
            
            if (Instance._fields.ContainsKey(typeName) == false)
                Instance._fields[typeName] = new Dictionary<string, BuiltinProperty>();
            
            Instance._fields[typeName][fieldInfo.Name] = builtinField;
            field = builtinField;
            return true;

            object Getter(object classInstance) => fieldInfo.GetValue(classInstance);
            void Setter(object classInstance, object value) => fieldInfo.SetValue(classInstance, value);
        }

        /// <summary>
        /// Tries to create a BuiltinProperty from a property of a builtin type
        /// </summary>
        /// <param name="typeName">Name of the builtin type</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="property">Output BuiltinProperty</param>
        /// <returns>true if the property was found or created, false otherwise</returns>
        public static bool GetOrCreateProperty(string typeName, string propertyName, out BuiltinProperty property)
        {
            if (Instance._properties.ContainsKey(typeName))
            {
                if (Instance._properties[typeName].ContainsKey(propertyName))
                {
                    property = Instance._properties[typeName][propertyName];
                    return true;
                }
            }

            if (CustomLogicBuiltinTypes.TypeMemberNames[typeName].Contains(propertyName) == false)
            {
                property = default;
                return false;
            }
            
            var type = CustomLogicBuiltinTypes.Types[typeName];
            var propertyInfo = type.GetProperty(propertyName, Flags);

            if (propertyInfo == null)
            {
                property = default;
                return false;
            }
            
            var attribute = propertyInfo.GetCustomAttribute<CLPropertyAttribute>();
            attribute.ClearDescription();
                
            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();
                
            var hasGetter = getMethod != null;
            var hasSetter = setMethod != null && attribute.ReadOnly == false;
            
            Func<object, object> getter = null;
            Action<object, object> setter = null;

            if (hasGetter)
            {
                var getterType = typeof(Func<,>).MakeGenericType(type, propertyInfo.PropertyType);
                var getterDelegate = getMethod.CreateDelegate(getterType);

                var genericGetMethod = GenericGetDelegateMethodInfo.MakeGenericMethod(type, propertyInfo.PropertyType);
                getter = (Func<object, object>)genericGetMethod.Invoke(null, new object[] { getterDelegate });
            }
            
            if (hasSetter)
            {
                var setterType = typeof(Action<,>).MakeGenericType(type, propertyInfo.PropertyType);
                var setterDelegate = setMethod.CreateDelegate(setterType);
                    
                var genericSetMethod = GenericSetDelegateMethodInfo.MakeGenericMethod(type, propertyInfo.PropertyType);
                setter = (Action<object, object>)genericSetMethod.Invoke(null, new object[] { setterDelegate });
            }
            
            property = new BuiltinProperty(getter, setter);
            
            if (Instance._properties.ContainsKey(typeName) == false)
                Instance._properties[typeName] = new Dictionary<string, BuiltinProperty>();
            
            Instance._properties[typeName][propertyInfo.Name] = property;
            
            return true;
        }
        
        /// <summary>
        /// Tries to create a BuiltinMethod from a method of a builtin type
        /// </summary>
        /// <param name="typeName">Name of the builtin type</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="method">Output BuiltinMethod</param>
        /// <returns>true if the method was found or created, false otherwise</returns>
        public static bool GetOrCreateMethod(string typeName, string methodName, out BuiltinMethod method)
        {
            if (Instance._methods.ContainsKey(typeName))
            {
                if (Instance._methods[typeName].ContainsKey(methodName))
                {
                    method = Instance._methods[typeName][methodName];
                    return true;
                }
            }

            if (CustomLogicBuiltinTypes.TypeMemberNames[typeName].Contains(methodName) == false)
            {
                method = default;
                return false;
            }
            
            var type = CustomLogicBuiltinTypes.Types[typeName];
            var methodInfo = type.GetMethod(methodName, Flags);
            
            if (methodInfo == null)
            {
                method = default;
                return false;
            }
            
            methodInfo.GetCustomAttribute<CLMethodAttribute>().ClearDescription();
                
            method = new BuiltinMethod((classInstance, args, kwargs) => CustomLogicClassInstance.InvokeMethod(methodInfo, classInstance, args, kwargs));
            
            if (Instance._methods.ContainsKey(typeName) == false)
                Instance._methods[typeName] = new Dictionary<string, BuiltinMethod>();
            
            Instance._methods[typeName][methodInfo.Name] = method;
            return true;
        }
    }
}