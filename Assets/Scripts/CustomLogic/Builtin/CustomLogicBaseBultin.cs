using System.Collections.Generic;

namespace CustomLogic
{
    [CLType(Abstract = true)]
    abstract class CustomLogicBaseBuiltin: CustomLogicClassInstanceBuiltin
    {
        [CLProperty(description: "Type of the Object")]
        public string Type => ClassName;

        [CLProperty(description: "Is this object a character?")]
        public bool IsCharacter => false;


        public CustomLogicBaseBuiltin(string name): base(name)
        {
        }
    }
}
