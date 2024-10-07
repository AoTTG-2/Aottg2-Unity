namespace Settings
{
    abstract class TypedSetting<T>: BaseSetting
    {
        protected T DefaultValue;
        protected T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = SanitizeValue(value);
            }
        }

        public TypedSetting()
        {
        }

        public TypedSetting(T defaultValue)
        {
            DefaultValue = SanitizeValue(defaultValue);
            SetDefault();
        }
       
        public override void SetDefault()
        {
            Value = DefaultValue;
        }

        protected virtual T SanitizeValue(T value)
        {
            return value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
