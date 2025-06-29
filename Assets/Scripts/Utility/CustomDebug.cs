using System.Collections;
using UnityEngine;

namespace Utility
{
    public class CustomDebug : MonoBehaviour
    {
        private static CustomDebug _instance;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void DrawRay(Vector3 origin, Vector3 direction, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.DrawLineCoroutine(origin, origin + direction, color, duration));
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.DrawLineCoroutine(start, end, color, duration));
        }

        public static void SpawnCube(Vector3 position, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(PrimitiveType.Cube, position, Quaternion.identity, Vector3.one, color, duration));
        }

        public static void SpawnSphere(Vector3 position, float radius, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(PrimitiveType.Sphere, position, Quaternion.identity, new Vector3(radius, radius, radius), color, duration));
        }

        public static void SpawnCylinder(Vector3 position, float radius, float height, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(PrimitiveType.Cylinder, position, Quaternion.identity, new Vector3(radius, height, radius), color, duration));
        }

        public static void SpawnCapsule(Vector3 position, float radius, float height, Color color, float duration = 1f)
        {
            if (_instance != null)
                _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(PrimitiveType.Capsule, position, Quaternion.identity, new Vector3(radius, height, radius), color, duration));
        }

        public static void SpawnCapsuleCollider(CapsuleCollider capsule, Color color, float duration = 1f)
        {
            if (_instance == null || capsule == null) return;

            // Get world position and rotation
            Transform t = capsule.transform;
            Vector3 center = t.TransformPoint(capsule.center);
            float radius = capsule.radius * Mathf.Max(t.lossyScale.x, t.lossyScale.z); // Assuming uniform XZ scale
            float height = capsule.height * t.lossyScale.y;

            // Determine direction
            Vector3 up = Vector3.up;
            switch (capsule.direction)
            {
                case 0: up = t.right; break;   // X-axis
                case 1: up = t.up; break;      // Y-axis
                case 2: up = t.forward; break; // Z-axis
            }

            float cylinderHeight = Mathf.Max(0, height - 2 * radius);

            // Cylinder center
            Vector3 cylinderCenter = center;

            // Top and bottom sphere centers
            Vector3 offset = up * (cylinderHeight / 2 + radius);
            Vector3 topSphereCenter = center + offset;
            Vector3 bottomSphereCenter = center - offset;

            // Spawn cylinder
            Quaternion rotation = Quaternion.LookRotation(up);
            Vector3 cylinderScale = new Vector3(radius * 2, cylinderHeight / 2, radius * 2);
            _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(
                PrimitiveType.Cylinder, cylinderCenter, rotation, cylinderScale, color, duration));

            // Spawn top and bottom spheres
            Vector3 sphereScale = Vector3.one * radius * 2;
            _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(
                PrimitiveType.Sphere, topSphereCenter, Quaternion.identity, sphereScale, color, duration));
            _instance.StartCoroutine(_instance.SpawnPrimitiveCoroutine(
                PrimitiveType.Sphere, bottomSphereCenter, Quaternion.identity, sphereScale, color, duration));
        }



        private IEnumerator DrawLineCoroutine(Vector3 start, Vector3 end, Color color, float duration)
        {
            GameObject lineObj = new GameObject("DebugLine");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = lr.endColor = color;
            lr.startWidth = lr.endWidth = 0.05f;
            yield return new WaitForSeconds(duration);
            Destroy(lineObj);
        }

        private IEnumerator SpawnPrimitiveCoroutine(PrimitiveType type, Vector3 position, Quaternion Rotation, Vector3 size, Color color, float duration)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = "DebugPrimitive";
            obj.transform.position = position;
            obj.transform.rotation = Rotation;
            obj.transform.localScale = size;
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = color;
            }
            yield return new WaitForSeconds(duration);
            Destroy(obj);
        }
    }
}
