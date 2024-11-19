using System;
using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CLBaseAttribute : Attribute
    {
        public string Description { get; set; } = "";
        
        public void ClearDescription() => Description = "";
    }
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    class CLTypeAttribute : CLBaseAttribute
    {
        public CLTypeAttribute(string description = "")
        {
            Description = description;
        }
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

        public BuiltinProperty(Func<object, object> getter, Action<object, object> setter)
        {
            _getter = getter;
            _setter = setter;
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