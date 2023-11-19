using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicRandomBuiltin : CustomLogicBaseBuiltin
    {
        public CustomLogicRandomBuiltin(): base("Random")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "RandomInt")
                return Random.Range(parameters[0].UnboxToInt(), parameters[1].UnboxToInt());
            else if (name == "RandomFloat")
                return Random.Range(parameters[0].UnboxToFloat(), parameters[1].UnboxToFloat());
            else if (name == "RandomBool")
                return RandomGen.GetRandomBool();
            else if (name == "RandomVector3")
            {
                Vector3 a = ((CustomLogicVector3Builtin)parameters[0]).Value;
                Vector3 b = ((CustomLogicVector3Builtin)parameters[1]).Value;
                return new CustomLogicVector3Builtin(new Vector3(Random.Range(a.x, b.x), Random.Range(a.y, b.y), Random.Range(a.z, b.z)));
            }
            else if (name == "RandomDirection")
                return new CustomLogicVector3Builtin(RandomGen.GetRandomDirection());
            else if (name == "RandomSign")
                return (int)RandomGen.GetRandomSign();
            return base.CallMethod(name, parameters);
        }
    }
}
