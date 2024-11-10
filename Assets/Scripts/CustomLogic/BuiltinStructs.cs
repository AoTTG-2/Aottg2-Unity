using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomLogic
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class CLField : Attribute
    {
        public FieldInfo Field { get; set; } = null;
        public bool ReadOnly { get; set; } = false;
        public string Description { get; set; } = "";

        public CLField(CLField commandAttribute)
        {
            Field = commandAttribute.Field;
            ReadOnly = commandAttribute.ReadOnly;
            Description = commandAttribute.Description;
        }

        public CLField(string description = "", bool readOnly=false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class CLProperty : Attribute
    {
        public PropertyInfo Property { get; set; } = null;
        public bool ReadOnly { get; set; } = false;
        public string Description { get; set; } = "";

        public CLProperty(CLProperty commandAttribute)
        {
            Property = commandAttribute.Property;
            ReadOnly = commandAttribute.ReadOnly;
            Description = commandAttribute.Description;
        }

        public CLProperty(string description="", bool readOnly=false)
        {
            ReadOnly = readOnly;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    class CLMethod : Attribute
    {
        public MethodInfo Method { get; set; } = null;
        public string Description { get; set; } = "";

        public CLMethod(CLMethod commandAttribute)
        {
            Method = commandAttribute.Method;
            Description = commandAttribute.Description;
        }

        public CLMethod(string description="")
        {
            Description = description;
        }
    }

    interface ICustomLogicCallable
    {
        object Call(object instance, object[] parameters, Dictionary<string, object> kwargs);
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

    struct BuiltinField
    {
        private readonly Func<object> _getter;
        private readonly Action<object> _setter;

        public BuiltinField(Func<object> getter, Action<object> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        public object Value
        {
            get => _getter?.Invoke();
            set => _setter?.Invoke(value);
        }
    }

    struct BuiltinFunction : ICustomLogicCallable
    {
        private readonly Func<object, object[], Dictionary<string, object>, object> _function;

        public BuiltinFunction(Func<object, object[], Dictionary<string, object>, object> function)
        {
            _function = function;
        }

        public object Call(object instance, object[] parameters, Dictionary<string, object> kwargs)
        {
            return _function?.Invoke(instance, parameters, kwargs);
        }
    }
}
