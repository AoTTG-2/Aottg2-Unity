using System;
using System.Collections.Generic;

namespace CustomLogic
{
    internal class BuiltinMethod
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