using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicRandomBuiltin : CustomLogicBaseBuiltin
    {
        public Unity.Mathematics.Random Rand;
        public bool UseInstanceRandom = false;

        public CustomLogicRandomBuiltin(List<object> parameterValues) : base("Random")
        {
            if (parameterValues.Count == 1)
            {
                Rand = new Unity.Mathematics.Random((uint)parameterValues[0].UnboxToInt());
                UseInstanceRandom = true;
            }
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "RandomInt")
                return RandomInt(parameters[0].UnboxToInt(), parameters[1].UnboxToInt());
            if (name == "RandomFloat")
                return RandomFloat(parameters[0].UnboxToFloat(), parameters[1].UnboxToFloat());
            if (name == "RandomBool")
                return RandomBool();
            if (name == "RandomVector3")
            {
                Vector3 a = ((CustomLogicVector3Builtin)parameters[0]).Value;
                Vector3 b = ((CustomLogicVector3Builtin)parameters[1]).Value;
                return new CustomLogicVector3Builtin(RandomVector3(a, b));
            }
            if (name == "RandomDirection")
                return new CustomLogicVector3Builtin(RandomDirection());
            if (name == "RandomSign")
                return RandomSign();
            if (name == "PerlinNoise")
                return Mathf.PerlinNoise(parameters[0].UnboxToFloat(), parameters[1].UnboxToFloat());
            return base.CallMethod(name, parameters);
        }

        private int RandomInt(int min, int max) => UseInstanceRandom ? Rand.NextInt(min, max) : Random.Range(min, max);
        private float RandomFloat(float min, float max) => UseInstanceRandom ? Rand.NextFloat(min, max) : Random.Range(min, max);
        private bool RandomBool() => UseInstanceRandom ? Rand.NextBool() : RandomGen.GetRandomBool();
        private Vector3 RandomVector3(Vector3 a, Vector3 b) => new(RandomFloat(a.x, b.x), RandomFloat(a.y, b.y), RandomFloat(a.z, b.z));

        private Vector3 RandomDirection(bool flat = false)
        {
            Vector3 v = new(RandomFloat(-1f, 1f), RandomFloat(-1f, 1f), RandomFloat(-1f, 1f));
            if (flat)
                v.y = 0f;
            return v.normalized;
        }
        private int RandomSign() => RandomBool() ? 1 : -1;
    }
}
