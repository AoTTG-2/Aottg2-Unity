using System.Collections.Generic;

namespace CustomLogic
{
    abstract class CustomLogicStructBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicStructBuiltin(string name): base(name)
        {
        }

        public abstract CustomLogicStructBuiltin Copy();
    }
}
