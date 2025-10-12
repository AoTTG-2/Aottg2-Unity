using CustomLogic.Editor.Models;

namespace CustomLogic.Editor
{
    abstract class BaseCustomLogicDocsGenerator
    {
        protected readonly CLType[] AllTypes;

        protected BaseCustomLogicDocsGenerator(CLType[] allTypes)
        {
            AllTypes = allTypes;
        }

        public abstract string GetRelativeFilePath(CLType type);
        public abstract string Generate(CLType type);
    }
}