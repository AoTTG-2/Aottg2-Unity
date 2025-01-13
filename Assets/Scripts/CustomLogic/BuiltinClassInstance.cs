namespace CustomLogic
{
    abstract class BuiltinClassInstance : CustomLogicClassInstance
    {
        private static readonly CLMethodBinding<BuiltinClassInstance> Init = new((_, _) => null);

        protected BuiltinClassInstance()
        {
            Variables["Type"] = ClassName;
            Variables["Init"] = Init;
            Variables["IsCharacter"] = false;
        }

        public override bool LookupBaseClassForVariables => InheritBaseMembers;

        // These are overridded by the source generator, do not override manually
        public override string ClassName => throw new System.NotImplementedException();
        public virtual bool IsAbstract => throw new System.NotImplementedException();
        public virtual bool IsStatic => throw new System.NotImplementedException();
        public virtual bool InheritBaseMembers => throw new System.NotImplementedException();
    }
}
