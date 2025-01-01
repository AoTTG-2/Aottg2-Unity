using System;

namespace CustomLogic
{
    internal abstract class CLPropertyBinding : ICLMemberBinding
    {
        public abstract bool IsReadOnly { get; }
        public abstract object GetValue(object instance);
        public abstract void SetValue(object instance, object value);
    }

    internal class CLPropertyBinding<T> : CLPropertyBinding where T : CustomLogicClassInstance
    {
        private readonly Func<T, object> _getter;
        private readonly Action<T, object> _setter;
        private readonly bool _isReadOnly;

        public CLPropertyBinding(Func<T, object> getter, Action<T, object> setter)
        {
            _getter = getter;
            _setter = setter;
            _isReadOnly = setter == null;
        }

        public override bool IsReadOnly => _isReadOnly;

        public override object GetValue(object instance) => _getter((T)instance);
        public override void SetValue(object instance, object value) => _setter?.Invoke((T)instance, value);
    }
}