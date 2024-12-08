using ApplicationManagers;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicRandomBuiltin : CustomLogicClassInstanceBuiltin
    {
        public Unity.Mathematics.Random Rand;
        public bool UseInstanceRandom = false;

        public CustomLogicRandomBuiltin(object[] parameterValues) : base("Random")
        {
            if (parameterValues.Length == 1)
            {
                Rand = new Unity.Mathematics.Random((uint)parameterValues[0].UnboxToInt());
                UseInstanceRandom = true;
            }
        }

        [CLMethod("Generates a random integer between the specified range.")]
        public int RandomInt(int min, int max) => UseInstanceRandom ? Rand.NextInt(min, max) : Random.Range(min, max);

        [CLMethod("Generates a random float between the specified range.")]
        public float RandomFloat(float min, float max) => UseInstanceRandom ? Rand.NextFloat(min, max) : Random.Range(min, max);

        [CLMethod("Generates a random boolean value.")]
        public bool RandomBool() => UseInstanceRandom ? Rand.NextBool() : RandomGen.GetRandomBool();

        [CLMethod("Generates a random Vector3 between the specified ranges.")]
        public CustomLogicVector3Builtin RandomVector3(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(new Vector3(RandomFloat(a.Value.x, b.Value.x), RandomFloat(a.Value.y, b.Value.y), RandomFloat(a.Value.z, b.Value.z)));

        [CLMethod("Generates a random direction vector. If flat is true, the y component will be zero.")]
        public CustomLogicVector3Builtin RandomDirection(bool flat = false)
        {
            Vector3 v = new(RandomFloat(-1f, 1f), RandomFloat(-1f, 1f), RandomFloat(-1f, 1f));
            if (flat)
                v.y = 0f;
            return new CustomLogicVector3Builtin(v.normalized);
        }

        [CLMethod("Generates a random sign, either 1 or -1.")]
        public int RandomSign() => RandomBool() ? 1 : -1;

        [CLMethod("Calculates Perlin noise for the specified coordinates.")]
        public float PerlinNoise(float x, float y) => Mathf.PerlinNoise(x, y);
    }
}
