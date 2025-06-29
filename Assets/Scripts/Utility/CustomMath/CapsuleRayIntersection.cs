using UnityEngine;
using System.Collections.Generic;

namespace Utility.CustomMath
{
    public static class CapsuleRayIntersection
    {
        public static bool RayIntersectsCapsule(Ray ray, CapsuleCollider capsule, out List<Vector3> intersections, out float ratio)
        {
            intersections = new List<Vector3>();

            Matrix4x4 worldToLocal = capsule.transform.worldToLocalMatrix;
            Vector3 localRayOrigin = worldToLocal.MultiplyPoint(ray.origin);
            Vector3 localRayDirection = worldToLocal.MultiplyVector(ray.direction).normalized;
            ray = new Ray(localRayOrigin, localRayDirection);


            // Get capsule world-space endpoints
            Transform t = capsule.transform;

            Vector3 center = capsule.center;
            float height = Mathf.Max(0, capsule.height * 0.5f - capsule.radius);
            Vector3 up = Vector3.up;

            //Vector3 scale = capsule.transform.lossyScale;
            Vector3 scale = Vector3.one;
            Vector3 p1 = center + Vector3.Scale(up * height, scale);
            Vector3 p2 = center - Vector3.Scale(up * height, scale);


            //float radius = capsule.radius * Mathf.Max(
            // Mathf.Abs(t.lossyScale.x),
            // Mathf.Abs(t.lossyScale.y),
            // Mathf.Abs(t.lossyScale.z)
            //);

            float radius = capsule.radius;

            // Check intersection with cylindrical body
            if (RayIntersectsCapsuleBody(ray.origin, ray.direction, p1, p2, radius, out Vector3 hit1, out Vector3 hit2))
            {
                if (hit1 != Vector3.positiveInfinity && PointIsBetweenPoints(hit1, p1, p2, true)) intersections.Add(hit1);
                if (hit2 != Vector3.positiveInfinity && PointIsBetweenPoints(hit2, p1, p2, true)) intersections.Add(hit2);
            }

            // Check intersection with spherical caps
            if (RayIntersectsSphere(ray, p1, radius, out Vector3 capHit1) && !PointIsBetweenPoints(capHit1, p1, p2)) intersections.Add(capHit1);
            if (RayIntersectsSphere(ray, p2, radius, out Vector3 capHit2) && !PointIsBetweenPoints(capHit2, p1, p2)) intersections.Add(capHit2);

            ratio = 1;
            if (intersections.Count == 2)
            {
                float distance = Vector3.Distance(intersections[0], intersections[1]);
                ratio = distance / radius;
            }


            Matrix4x4 localToWorld = capsule.transform.localToWorldMatrix;
            for (int i = 0; i < intersections.Count; i++)
            {
                intersections[i] = localToWorld.MultiplyPoint(intersections[i]);
            }

            return intersections.Count > 0;
        }

        private static bool PointIsBetweenPoints(Vector3 point, Vector3 top, Vector3 bottom, bool inclusive=false)
        {
            if (inclusive)
            {
                return point.y <= top.y && point.y >= bottom.y;
            }
            return point.y < top.y && point.y > bottom.y;
        }

        private static bool RayIntersectsSphere(Ray ray, Vector3 center, float radius, out Vector3 hit)
        {
            Vector3 oc = ray.origin - center;
            float a = Vector3.Dot(ray.direction, ray.direction);
            float b = 2.0f * Vector3.Dot(oc, ray.direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                hit = Vector3.positiveInfinity;
                return false;
            }

            float sqrtDisc = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtDisc) / (2.0f * a);
            float t2 = (-b + sqrtDisc) / (2.0f * a);

            float t = (t1 >= 0) ? t1 : (t2 >= 0 ? t2 : -1);
            if (t >= 0)
            {
                hit = ray.origin + t * ray.direction;
                return true;
            }

            hit = Vector3.positiveInfinity;
            return false;
        }

        public static bool RayIntersectsCapsuleBody(Vector3 rayOrigin, Vector3 rayDir, Vector3 p1, Vector3 p2, float radius, out Vector3 hit1, out Vector3 hit2)
        {
            hit1 = hit2 = Vector3.positiveInfinity;

            Vector3 d = p2 - p1;
            Vector3 m = rayOrigin - p1;
            Vector3 n = rayDir;

            float dd = Vector3.Dot(d, d);
            float nd = Vector3.Dot(n, d);
            float md = Vector3.Dot(m, d);

            float a = dd - nd * nd;
            float b = dd * Vector3.Dot(m, n) - md * nd;
            float c = dd * Vector3.Dot(m, m) - md * md - radius * radius * dd;

            if (Mathf.Abs(a) < Mathf.Epsilon)
            {
                return false; // Ray is parallel to capsule axis
            }

            float discriminant = b * b - a * c;
            if (discriminant < 0)
            {
                return false;
            }

            float sqrtDisc = Mathf.Sqrt(discriminant);
            float t1 = (-b - sqrtDisc) / a;
            float t2 = (-b + sqrtDisc) / a;

            Vector3 i1 = rayOrigin + t1 * rayDir;
            Vector3 i2 = rayOrigin + t2 * rayDir;

            if (IsPointOnSegment(i1, p1, p2)) hit1 = i1;
            if (IsPointOnSegment(i2, p1, p2)) hit2 = i2;

            return hit1 != Vector3.positiveInfinity || hit2 != Vector3.positiveInfinity;
        }

        private static bool IsPointOnSegment(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector3 ab = b - a;
            Vector3 ap = point - a;
            float dot = Vector3.Dot(ap, ab.normalized);
            return dot >= 0 && dot <= ab.magnitude;
        }
    }

}
