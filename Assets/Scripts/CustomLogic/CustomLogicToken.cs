namespace CustomLogic
{
    class CustomLogicToken
    {
        public CustomLogicTokenType Type;
        public object Value;
        public int Line;

        public CustomLogicToken(CustomLogicTokenType type, object value, int line)
        {
            Type = type;
            Value = value;
            Line = line;
        }
    }
    public enum CustomLogicTokenType
    {
        Symbol,
        Primitive,
        Name
    }
}