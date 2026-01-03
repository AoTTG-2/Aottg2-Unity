using UnityEngine;
using Utility;

namespace CustomLogic
{
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
    [CLType(Name = "Random", Static = true, Description = "Random can be initialized as a class with an int given as the seed value. Note that this is optional, and you can reference Random directly as a static class.")]
    partial class CustomLogicRandomBuiltin : BuiltinClassInstance
    {
        public Unity.Mathematics.Random Rand;
        public readonly bool UseInstanceRandom;

        [CLConstructor("Creates a new Random instance with default seed.")]
        public CustomLogicRandomBuiltin() { }

        [CLConstructor("Creates a new Random instance with the specified seed.")]
        public CustomLogicRandomBuiltin(
            [CLParam("The seed value for the random number generator.")]
            int seed)
        {
            Rand = new Unity.Mathematics.Random((uint)seed);
            UseInstanceRandom = true;
        }

        [CLMethod(Hybrid = true, Description = "Generates a random integer between the specified range.")]
        public int RandomInt(
            [CLParam("The minimum value (inclusive).")]
            int min,
            [CLParam("The maximum value (exclusive).")]
            int max) => UseInstanceRandom ? Rand.NextInt(min, max) : Random.Range(min, max);

        [CLMethod(Hybrid = true, Description = "Generates a random float between the specified range.")]
        public float RandomFloat(
            [CLParam("The minimum value (inclusive).")]
            float min,
            [CLParam("The maximum value (inclusive).")]
            float max) => UseInstanceRandom ? Rand.NextFloat(min, max) : Random.Range(min, max);

        [CLMethod(Hybrid = true, Description = "Returns random boolean.")]
        public bool RandomBool() => UseInstanceRandom ? Rand.NextBool() : RandomGen.GetRandomBool();

        [CLMethod(Hybrid = true, Description = "Generates a random Vector3 between the specified ranges.")]
        public CustomLogicVector3Builtin RandomVector3(
            [CLParam("The minimum Vector3 values.")]
            CustomLogicVector3Builtin a,
            [CLParam("The maximum Vector3 values.")]
            CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(new Vector3(RandomFloat(a.Value.x, b.Value.x), RandomFloat(a.Value.y, b.Value.y), RandomFloat(a.Value.z, b.Value.z)));

        [CLMethod(Hybrid = true, Description = "Generates a random normalized direction vector. If flat is true, the y component will be zero.")]
        public CustomLogicVector3Builtin RandomDirection(
            [CLParam("If true, the y component will be zero.")]
            bool flat = false)
        {
            var v = new Vector3(RandomFloat(-1f, 1f), RandomFloat(-1f, 1f), RandomFloat(-1f, 1f));
            if (flat)
                v.y = 0f;
            return new CustomLogicVector3Builtin(v.normalized);
        }

        [CLMethod(Hybrid = true, Description = "Generates a random sign, either 1 or -1.")]
        public int RandomSign() => RandomBool() ? 1 : -1;

        [CLMethod(Hybrid = true, Description = "Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)")]
        public float PerlinNoise(
            [CLParam("The X coordinate for the noise sample.")]
            float x,
            [CLParam("The Y coordinate for the noise sample.")]
            float y) => Mathf.PerlinNoise(x, y);
    }
}
