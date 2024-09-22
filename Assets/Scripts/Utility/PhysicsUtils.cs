using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class PhysicsUtils
    {
        public static readonly RaycastHit[] Raycasts = new RaycastHit[16];
        public static readonly Collider[] Colliders = new Collider[16];
        public static int Count;

        private static readonly IComparer Comparer = Comparer<RaycastHit>.Create((x, y) => x.distance.CompareTo(y.distance));

        public static int RaycastAll(Ray ray, float distance, int layerMask = -5) =>
            Count = Physics.RaycastNonAlloc(ray, Raycasts, distance, layerMask);
        
        public static int RaycastAll(Vector3 origin, Vector3 direction, float distance, int layerMask = -5) =>
            Count = Physics.RaycastNonAlloc(origin, direction, Raycasts, distance, layerMask);

        public static int OverlapSphere(Vector3 position, float radius, int layerMask) =>
            Count = Physics.OverlapSphereNonAlloc(position, radius, Colliders, layerMask);

        public static void SortByDistanceAsc() => Array.Sort(Raycasts, 0, Count, Comparer);

        public static IEnumerable<RaycastHit> GetRaycasts() => GetRaycasts(Count);

        public static IEnumerable<RaycastHit> GetRaycasts(int count)
        {
            for (int i = 0; i < count; ++i)
                yield return Raycasts[i];
        }

        public static IEnumerable<Collider> GetColliders() => GetColliders(Count);

        public static IEnumerable<Collider> GetColliders(int count)
        {
            for (int i = 0; i < count; i++)
                yield return Colliders[i];
        }
    }
}
