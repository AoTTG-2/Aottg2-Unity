using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Custom debug for drawing lines and spawning primitives in the scene.
    /// </summary>
    public class CustomDebug : MonoBehaviour
    {
        private static CustomDebug _instance;
        private Dictionary<string, GameObject> _debugObjects = new Dictionary<string, GameObject>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void RemoveDebugVisual(string name)
        {
            if (_instance._debugObjects.ContainsKey(name))
            {
                Destroy(_instance._debugObjects[name]);
                _instance._debugObjects.Remove(name);
            }
        }

        public static string DrawRay(Vector3 origin, Vector3 direction, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.DrawLineObject(origin, origin + direction, color, 0f);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string DrawLine(Vector3 start, Vector3 end, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.DrawLineObject(start, end, color, 0f);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string SpawnCube(Vector3 position, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.SpawnPrimitiveObject(PrimitiveType.Cube, position, Quaternion.identity, Vector3.one, color);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string SpawnSphere(Vector3 position, float radius, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.SpawnPrimitiveObject(PrimitiveType.Sphere, position, Quaternion.identity, new Vector3(radius, radius, radius), color);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string SpawnCylinder(Vector3 position, float radius, float height, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.SpawnPrimitiveObject(PrimitiveType.Cylinder, position, Quaternion.identity, new Vector3(radius, height, radius), color);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string SpawnCapsule(Vector3 position, float radius, float height, Color color, string name)
        {
            RemoveDebugVisual(name);
            var obj = _instance.SpawnPrimitiveObject(PrimitiveType.Capsule, position, Quaternion.identity, new Vector3(radius, height, radius), color);
            obj.name = name;
            _instance._debugObjects.Add(obj.name, obj);
            return name;
        }

        public static string SpawnCapsuleCollder(CapsuleCollider capsule, Color color, string name)
        {
            if (_instance == null || capsule == null) return null;

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
            var middle = _instance.SpawnPrimitiveObject(PrimitiveType.Cylinder, cylinderCenter, rotation, cylinderScale, color);

            // Spawn top and bottom spheres
            Vector3 sphereScale = Vector3.one * radius * 2;

            var top = _instance.SpawnPrimitiveObject(PrimitiveType.Sphere, topSphereCenter, Quaternion.identity, sphereScale, color);

            var bottom = _instance.SpawnPrimitiveObject(PrimitiveType.Sphere, bottomSphereCenter, Quaternion.identity, sphereScale, color);

            top.transform.parent = middle.transform;
            bottom.transform.parent = middle.transform;

            middle.name = name;
            _instance._debugObjects.Add(name, middle);
            return name;
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
            var obj = SpawnPrimitiveObject(type, position, Rotation, size, color);
            yield return new WaitForSeconds(duration);
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        private GameObject DrawLineObject(Vector3 start, Vector3 end, Color color, float duration)
        {
            GameObject lineObj = new GameObject("DebugLine");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = lr.endColor = color;
            lr.startWidth = lr.endWidth = 0.05f;
            Destroy(lineObj, duration);
            return lineObj;
        }

        private GameObject SpawnPrimitiveObject(PrimitiveType type, Vector3 position, Quaternion Rotation, Vector3 size, Color color)
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

            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            return obj;
        }
    }
}