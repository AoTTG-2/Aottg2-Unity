using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CustomLogic
{
    /// <summary>
    /// Responsible for creating and caching <see cref="BuiltinProperty"/> and <see cref="BuiltinMethod"/>
    /// of builtin types
    /// </summary>
    internal class CustomLogicReflectioner
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static CustomLogicReflectioner instance;
        private static CustomLogicReflectioner Instance => instance ??= new CustomLogicReflectioner();

        private readonly Dictionary<string, Dictionary<string, BuiltinProperty>> _fields = new(8);
        private readonly Dictionary<string, Dictionary<string, BuiltinProperty>> _properties = new(32);
        private readonly Dictionary<string, Dictionary<string, BuiltinMethod>> _methods = new(32);

        private readonly Dictionary<string, HashSet<string>> _visited = new();

        #region Converter MethodInfos

        /*
         * The static converter methods (below) as reflection MethodInfo.
         * These method infos are not generic yet.
         */

        private static readonly Type ThisType = typeof(CustomLogicReflectioner);

        private static readonly MethodInfo GetMethodInfo = ThisType.GetMethod(nameof(InstanceGetDelegate), Flags);
        private static readonly MethodInfo SetMethodInfo = ThisType.GetMethod(nameof(InstanceSetDelegate), Flags);

        private static readonly MethodInfo StaticGetMethodInfo = ThisType.GetMethod(nameof(StaticGetDelegate), Flags);
        private static readonly MethodInfo StaticSetMethodInfo = ThisType.GetMethod(nameof(StaticSetDelegate), Flags);

        #endregion

        #region Converters

        /*
         * Converters for converting generic Func and Action to Func and Action of object.
         * Func<TClass, TResult>        ->      Func<object, object>
         * Action<TClass, TResult>      ->      Action<object, object>
         * Func<TResult>                ->      Func<object>
         * Action<TResult>              ->      Action<object>
         */

        private static Func<object, object> InstanceGetDelegate<TClass, TResult>(Func<TClass, TResult> func)
        {
            return classInstance => func((TClass)classInstance);
        }
        private static Action<object, object> InstanceSetDelegate<TClass, TResult>(Action<TClass, TResult> func)
        {
            return (classInstance, result) =>
            {
                func((TClass)classInstance, CustomLogicEvaluator.ConvertTo<TResult>(result));
            };
        }

        private static Func<object> StaticGetDelegate<TResult>(Func<TResult> func) => () => func();
        private static Action<object> StaticSetDelegate<TResult>(Action<TResult> func)
        {
            return result => func(CustomLogicEvaluator.ConvertTo<TResult>(result));
        }

        #endregion

        public static Dictionary<string, Dictionary<string, BuiltinProperty>> Fields => Instance._fields;
        public static Dictionary<string, Dictionary<string, BuiltinProperty>> Properties => Instance._properties;
        public static Dictionary<string, Dictionary<string, BuiltinMethod>> Methods => Instance._methods;

        private static bool IsCached(string typeName, string variableName)
        {
            return Instance._visited.ContainsKey(typeName) && Instance._visited[typeName].Contains(variableName);
        }

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

            if (IsCached(typeName, fieldName))
            {
                field = default;
                return false;
            }

            if (CustomLogicBuiltinTypes.TypeMemberNames[typeName].Contains(fieldName) == false)
            {
                field = default;
                return false;
            }

            // Debug.Log($"Field {typeName}.{fieldName} not found, creating.");

            var type = CustomLogicBuiltinTypes.Types[typeName];
            var fieldInfo = type.GetField(fieldName, Flags);

            if (fieldInfo == null)
            {
                field = default;
                return false;
            }

            var attribute = fieldInfo.GetCustomAttribute<CLPropertyAttribute>();
            attribute.ClearDescription();

            var hasSetter = fieldInfo.IsInitOnly == false && fieldInfo.IsLiteral == false && attribute.ReadOnly == false;
            var builtinField = new BuiltinProperty(Getter, hasSetter ? Setter : null);

            if (Instance._fields.ContainsKey(typeName) == false)
                Instance._fields[typeName] = new Dictionary<string, BuiltinProperty>();

            if (Instance._visited.ContainsKey(typeName) == false)
                Instance._visited[typeName] = new HashSet<string>();

            Instance._visited[typeName].Add(fieldName);
            Instance._fields[typeName][fieldName] = builtinField;
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

            if (IsCached(typeName, propertyName))
            {
                property = default;
                return false;
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
                var getterType = getMethod.IsStatic
                    ? typeof(Func<>).MakeGenericType(propertyInfo.PropertyType)
                    : typeof(Func<,>).MakeGenericType(type, propertyInfo.PropertyType);

                var genericGetMethod = getMethod.IsStatic
                    ? StaticGetMethodInfo.MakeGenericMethod(propertyInfo.PropertyType)
                    : GetMethodInfo.MakeGenericMethod(type, propertyInfo.PropertyType);

                var getterDelegate = getMethod.CreateDelegate(getterType);

                if (getMethod.IsStatic)
                {
                    var staticGetter = (Func<object>)genericGetMethod.Invoke(null, new object[] { getterDelegate });
                    getter = _ => staticGetter();
                }
                else
                    getter = (Func<object, object>)genericGetMethod.Invoke(null, new object[] { getterDelegate });
            }

            if (hasSetter)
            {
                var setterType = setMethod.IsStatic
                    ? typeof(Action<>).MakeGenericType(propertyInfo.PropertyType)
                    : typeof(Action<,>).MakeGenericType(type, propertyInfo.PropertyType);

                var genericSetMethod = setMethod.IsStatic
                    ? StaticSetMethodInfo.MakeGenericMethod(propertyInfo.PropertyType)
                    : SetMethodInfo.MakeGenericMethod(type, propertyInfo.PropertyType);

                var setterDelegate = setMethod.CreateDelegate(setterType);

                if (setMethod.IsStatic)
                {
                    var staticSetter = (Action<object>)genericSetMethod.Invoke(null, new object[] { setterDelegate });
                    setter = (_, value) => staticSetter(value);
                }
                else
                    setter = (Action<object, object>)genericSetMethod.Invoke(null, new object[] { setterDelegate });
            }

            property = new BuiltinProperty(getter, setter);

            if (Instance._properties.ContainsKey(typeName) == false)
                Instance._properties[typeName] = new Dictionary<string, BuiltinProperty>();

            if (Instance._visited.ContainsKey(typeName) == false)
                Instance._visited[typeName] = new HashSet<string>();

            Instance._visited[typeName].Add(propertyName);

            Instance._properties[typeName][propertyName] = property;

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

            if (IsCached(typeName, methodName))
            {
                method = default;
                return false;
            }

            if (CustomLogicBuiltinTypes.TypeMemberNames[typeName].Contains(methodName) == false)
            {
                method = default;
                return false;
            }

            // Debug.Log($"Method {typeName}.{methodName} not found, creating.");

            var type = CustomLogicBuiltinTypes.Types[typeName];
            var methodInfo = type.GetMethods(Flags)
                .FirstOrDefault(x => x.Name == methodName && x.HasAttribute<CLMethodAttribute>());

            if (methodInfo == null)
            {
                method = default;
                return false;
            }

            methodInfo.GetCustomAttribute<CLMethodAttribute>().ClearDescription();

            method = new BuiltinMethod((classInstance, args, kwargs) => CustomLogicClassInstance.InvokeMethod(methodInfo, classInstance, args, kwargs));

            if (Instance._methods.ContainsKey(typeName) == false)
                Instance._methods[typeName] = new Dictionary<string, BuiltinMethod>();

            if (Instance._visited.ContainsKey(typeName) == false)
                Instance._visited[typeName] = new HashSet<string>();

            Instance._visited[typeName].Add(methodName);

            Instance._methods[typeName][methodName] = method;
            return true;
        }
    }
}