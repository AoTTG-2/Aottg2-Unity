using Settings;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Utility
{
    public static class RandomGen
    {
        public static bool GetRandomBool()
        {
            return Random.Range(0f, 1f) > 0.5f;
        }

        public static float GetRandomSign()
        {
            return GetRandomBool() ? 1f : -1f;
        }

        public static bool Roll(float probability)
        {
            return Random.Range(0f, 1f) < probability;
        }

        public static Vector3 GetRandomDirection(bool flat = false)
        {
            Vector3 v = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            if (flat)
                v.y = 0f;
            return v.normalized;
        }

        public static T ChooseRandom<T>(List<T> items)
        {
            int choose = Random.Range(0, items.Count);
            return items[choose];
        }
    }
}