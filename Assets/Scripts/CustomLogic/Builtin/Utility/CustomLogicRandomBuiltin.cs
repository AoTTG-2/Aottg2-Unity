using UnityEngine;
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

        /// <summary>
        /// Creates a new Random instance with default seed.
        /// </summary>
        [CLConstructor]
        public CustomLogicRandomBuiltin() { }

        /// <summary>
        /// Creates a new Random instance with the specified seed.
        /// </summary>
        /// <param name="seed">The seed value for the random number generator.</param>
        [CLConstructor]
        public CustomLogicRandomBuiltin(int seed)
        {
            Rand = new Unity.Mathematics.Random((uint)seed);
            UseInstanceRandom = true;
        }

        /// <summary>
        /// Generates a random integer between the specified range.
        /// </summary>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (exclusive).</param>
        /// <returns>A random integer in the specified range.</returns>
        [CLMethod(Hybrid = true)]
        public int RandomInt(int min, int max) => UseInstanceRandom ? Rand.NextInt(min, max) : Random.Range(min, max);

        /// <summary>
        /// Generates a random float between the specified range.
        /// </summary>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (inclusive).</param>
        /// <returns>A random float in the specified range.</returns>
        [CLMethod(Hybrid = true)]
        public float RandomFloat(float min, float max) => UseInstanceRandom ? Rand.NextFloat(min, max) : Random.Range(min, max);

        /// <summary>
        /// Returns random boolean.
        /// </summary>
        /// <returns>A random boolean value.</returns>
        [CLMethod(Hybrid = true)]
        public bool RandomBool() => UseInstanceRandom ? Rand.NextBool() : RandomGen.GetRandomBool();

        /// <summary>
        /// Generates a random Vector3 between the specified ranges.
        /// </summary>
        /// <param name="a">The minimum Vector3 values.</param>
        /// <param name="b">The maximum Vector3 values.</param>
        /// <returns>A random Vector3 in the specified range.</returns>
        [CLMethod(Hybrid = true)]
        public CustomLogicVector3Builtin RandomVector3(CustomLogicVector3Builtin a, CustomLogicVector3Builtin b) => new CustomLogicVector3Builtin(new Vector3(RandomFloat(a.Value.x, b.Value.x), RandomFloat(a.Value.y, b.Value.y), RandomFloat(a.Value.z, b.Value.z)));

        /// <summary>
        /// Generates a random normalized direction vector. If flat is true, the y component will be zero.
        /// </summary>
        /// <param name="flat">If true, the y component will be zero.</param>
        /// <returns>A random normalized direction vector.</returns>
        [CLMethod(Hybrid = true)]
        public CustomLogicVector3Builtin RandomDirection(bool flat = false)
        {
            var v = new Vector3(RandomFloat(-1f, 1f), RandomFloat(-1f, 1f), RandomFloat(-1f, 1f));
            if (flat)
                v.y = 0f;
            return new CustomLogicVector3Builtin(v.normalized);
        }

        /// <summary>
        /// Generates a random sign, either 1 or -1.
        /// </summary>
        /// <returns>Either 1 or -1.</returns>
        [CLMethod(Hybrid = true)]
        public int RandomSign() => RandomBool() ? 1 : -1;

        /// <summary>
        /// Returns a point sampled from generated 2d perlin noise.
        /// (see Unity Mathf.PerlinNoise for more information)
        /// </summary>
        /// <param name="x">The X coordinate for the noise sample.</param>
        /// <param name="y">The Y coordinate for the noise sample.</param>
        /// <returns>A perlin noise value between 0 and 1.</returns>
        [CLMethod(Hybrid = true)]
        public float PerlinNoise(float x, float y) => Mathf.PerlinNoise(x, y);
    }
}
