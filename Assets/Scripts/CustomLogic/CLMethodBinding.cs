using System;
using System.Collections.Generic;

namespace CustomLogic
{
    internal abstract class CLMethodBinding : ICLMemberBinding
    {
        public abstract object Call(object instance, object[] parameters);
    }

    internal class CLMethodBinding<T> : CLMethodBinding where T : CustomLogicClassInstance
    {
        private readonly Func<T, object[], object> _function;

        public CLMethodBinding(Func<T, object[], object> function)
        {
            _function = function;
        }

        public override object Call(object instance, object[] parameters)
        {
            return _function((T)instance, parameters);
        }
    }
}