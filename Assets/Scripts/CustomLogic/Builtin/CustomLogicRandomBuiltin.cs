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
            if (name == "RandomFloat")
                return Random.Range(parameters[0].UnboxToFloat(), parameters[1].UnboxToFloat());
            if (name == "RandomBool")
                return RandomGen.GetRandomBool();
            if (name == "RandomVector3")
            {
                Vector3 a = ((CustomLogicVector3Builtin)parameters[0]).Value;
                Vector3 b = ((CustomLogicVector3Builtin)parameters[1]).Value;
                return new CustomLogicVector3Builtin(new Vector3(Random.Range(a.x, b.x), Random.Range(a.y, b.y), Random.Range(a.z, b.z)));
            }
            if (name == "RandomDirection")
                return new CustomLogicVector3Builtin(RandomGen.GetRandomDirection());
            if (name == "RandomSign")
                return (int)RandomGen.GetRandomSign();
            return base.CallMethod(name, parameters);
        }
    }
}
