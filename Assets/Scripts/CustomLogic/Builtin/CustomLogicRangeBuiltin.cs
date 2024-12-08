using System.Collections.Generic;
using UnityEngine;

namespace CustomLogic
{
    // todo: implement some kind of catching/pool for this
    // we really don't need to create a new list every time
    
    [CLType(InheritBaseMembers = true)]
    class CustomLogicRangeBuiltin: CustomLogicListBuiltin
    {
        public CustomLogicRangeBuiltin(object[] parameterValues) : base("Range")
        {
            if (parameterValues.Length < 3)
                return;
            int start = parameterValues[0].UnboxToInt();
            int end = parameterValues[1].UnboxToInt();
            int step = parameterValues[2].UnboxToInt();
            if (step == 0)
                return;
            if (step > 0)
            {
                for (int i = start; i < end; i += step)
                    List.Add(i);
            }
            else
            {
                for (int i = start; i > end; i += step)
                    List.Add(i);
            }
        }
    }
}
