namespace CustomLogic
{
    class UserClassInstance : CustomLogicClassInstance
    {
        private readonly string _className;

        public UserClassInstance(string className)
        {
            _className = className;
            Variables["Type"] = className;
        }

        public override string ClassName => _className;
    }
}
