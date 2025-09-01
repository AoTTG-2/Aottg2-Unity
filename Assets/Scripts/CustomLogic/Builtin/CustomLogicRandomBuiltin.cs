﻿using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Random can be initialized as a class with an int given as the seed value.
    /// Note that this is optional, and you can reference Random directly as a static class.
    /// </summary>
    /// <code>
    /// # Use random methods directly
    /// r = Random.RandomInt(0, 100);
    /// 
    /// # Or create an instance of Random with a seed
    /// generator = Random(123);
    /// 
    /// # Use it
    /// a = generator.RandomInt(0, 100);
    /// 
    /// # Seed allows repeatable random values
    /// generator2 = Random(123);
    /// b = generator2.RandomInt(0, 100);
    /// compared = a == b;    # Always True
    /// </code>
    [CLType(Name = "Random", Static = true)]
    partial class CustomLogicRandomBuiltin : BuiltinClassInstance
    {
        public Unity.Mathematics.Random Rand;
        public readonly bool UseInstanceRandom;

        [CLConstructor]
        public CustomLogicRandomBuiltin() { }

        [CLConstructor]
        public CustomLogicRandomBuiltin(int seed)
        {
            Rand = new Unity.Mathematics.Random((uint)seed);
            UseInstanceRandom = true;
        }

        [CLMethod("Generates a random integer between the specified range.")]
        public int RandomInt(int min, int max) => UseInstanceRandom ? Rand.NextInt(min, max) : Random.Range(min, max);

        [CLMethod("Generates a random float between the specified range.")]
        public float RandomFloat(float min, float max) => UseInstanceRandom ? Rand.NextFloat(min, max) : Random.Range(min, max);

        [CLMethod("Returns random boolean.")]
        public bool RandomBool() => UseInstanceRandom ? Rand.NextBool() : RandomGen.GetRandomBool();

        [CLMethod("Generates a random Vector3 between the specified ranges.")]
        public CustomLogicVector3Builtin RandomVector3(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(new Vector3(RandomFloat(a.Value.x, b.Value.x), RandomFloat(a.Value.y, b.Value.y), RandomFloat(a.Value.z, b.Value.z)));

        [CLMethod("Generates a random normalized direction vector. If flat is true, the y component will be zero.")]
        public CustomLogicVector3Builtin RandomDirection(bool flat = false)
        {
            var v = new Vector3(RandomFloat(-1f, 1f), RandomFloat(-1f, 1f), RandomFloat(-1f, 1f));
            if (flat)
                v.y = 0f;
            return new CustomLogicVector3Builtin(v.normalized);
        }

        [CLMethod("Generates a random sign, either 1 or -1.")]
        public int RandomSign() => RandomBool() ? 1 : -1;

        [CLMethod("Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)")]
        public float PerlinNoise(float x, float y) => Mathf.PerlinNoise(x, y);
    }
}
