using ApplicationManagers;
using UnityEngine;
using Utility;

namespace MapEditor
{
    /// <summary>
    /// Provides a visual handle and collider for empty GameObjects in the map editor.
    /// This is automatically added by MapLoader for objects without visible renderers.
    /// </summary>
    public class EditorGizmoIcon : MonoBehaviour
    {
        private GameObject _iconObject;
        private SphereCollider _collider;

        public void Setup()
        {
            _iconObject = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Map, "Gizmos/MissingRenderer");
            if (_iconObject == null)
            {
                Debug.LogWarning("EditorGizmoIcon: Could not load Gizmos/MissingRenderer prefab - creating fallback");
                
                // Create a simple visual fallback
                _iconObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                DestroyImmediate(_iconObject.GetComponent<SphereCollider>()); // Remove the primitive's collider
                _iconObject.transform.localScale = Vector3.one * 0.3f;
                
                var renderer = _iconObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(Shader.Find("Unlit/Color"));
                    renderer.material.color = new Color(1f, 1f, 0f, 0.7f); // Yellow, semi-transparent
                }
            }

            _iconObject.transform.SetParent(transform);
            _iconObject.transform.localPosition = Vector3.zero;
            _iconObject.transform.localScale = Vector3.one;

            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.radius = 0.5f;

            gameObject.layer = PhysicsLayer.MapEditorObject;
        }

        void LateUpdate()
        {
            if (_iconObject != null && SceneLoader.CurrentCamera != null)
            {
                _iconObject.transform.LookAt(SceneLoader.CurrentCamera.Cache.Transform);
                _iconObject.transform.Rotate(0, 180, 0);
            }
        }

        void OnDestroy()
        {
            if (_iconObject != null)
            {
                Destroy(_iconObject);
            }
        }
    }
}