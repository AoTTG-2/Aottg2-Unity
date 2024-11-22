using System;
using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CLBaseAttribute : Attribute
    {
        public string Description { get; set; } = "";
        
        public void ClearDescription() => Description = "";
    }
    
    /// <summary>
    /// Custom logic builtin types must be marked with this attribute,
    /// they must also either have a parameterless constructor or
    /// a constructor with a single parameter of type object[]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class CLTypeAttribute : CLBaseAttribute
    {
        /// <summary>
        /// Should be set to true if the type has static members
        /// </summary>
        public bool Static { get; set; }
        
        /// <summary>
        /// Should be set to true if the type shouldn't be instantiated
        /// </summary>
        public bool Abstract { get; set; }

        public bool InheritBaseMembers { get; set; } = true;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class CLPropertyAttribute : CLBaseAttribute
    {
        public bool ReadOnly { get; set; }

        public CLPropertyAttribute(string description = "", bool readOnly = false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    class CLMethodAttribute : CLBaseAttribute
    {
        public CLMethodAttribute(string description = "")
        {
            Description = description;
        }
    }

    interface ICustomLogicMathOperators
    {
        object __Add__(object other);
        object __Sub__(object other);
        object __Mul__(object other);
        object __Div__(object other);
    }

    interface ICustomLogicEquals
    {
        bool __Eq__(object other);
        int __Hash__();
    }

    interface ICustomLogicCopyable
    {
        object __Copy__();
    }

    class BuiltinProperty
    {
        private readonly Func<object, object> _getter;
        private readonly Action<object, object> _setter;
        
        public readonly bool IsReadOnly;

        public BuiltinProperty(Func<object, object> getter, Action<object, object> setter)
        {
            _getter = getter;
            _setter = setter;
            IsReadOnly = _setter == null;
        }

        public object GetValue(object instance) => _getter?.Invoke(instance);
        public void SetValue(object instance, object value) => _setter?.Invoke(instance, value);
    }

    class BuiltinMethod
    {
        private readonly Func<object, List<object>, Dictionary<string, object>, object> _function;

        public BuiltinMethod(Func<object, List<object>, Dictionary<string, object>, object> function)
        {
            _function = function;
        }

        public object Call(object instance, List<object> parameters, Dictionary<string, object> kwargs)
        {
            return _function?.Invoke(instance, parameters, kwargs);
        }
    }
}