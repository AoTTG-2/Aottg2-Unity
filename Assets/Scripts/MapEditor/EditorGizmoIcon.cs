using ApplicationManagers;
using UnityEngine;
using Utility;

namespace MapEditor
{
    /// <summary>
    /// Provides a visual handle and collider for empty GameObjects in the map editor
    /// </summary>
    public class EditorGizmoIcon : MonoBehaviour
    {
        private static Mesh _iconMesh;
        private static Material _iconMaterial;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private SphereCollider _collider;

        public void Setup()
        {
            // Create a simple billboard icon mesh
            if (_iconMesh == null)
            {
                _iconMesh = CreateIconMesh();
            }

            if (_iconMaterial == null)
            {
                _iconMaterial = new Material(Shader.Find("Unlit/Transparent"));
                // You can load a custom icon texture here
                _iconMaterial.color = new Color(1f, 1f, 0f, 0.8f);
            }

            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshFilter.mesh = _iconMesh;

            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = _iconMaterial;

            _collider = gameObject.AddComponent<SphereCollider>();
            _collider.radius = 0.5f;

            gameObject.layer = PhysicsLayer.MapEditorObject;
        }

        private static Mesh CreateIconMesh()
        {
            Mesh mesh = new Mesh();
            
            // Create a simple quad
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-0.25f, -0.25f, 0),
                new Vector3(0.25f, -0.25f, 0),
                new Vector3(-0.25f, 0.25f, 0),
                new Vector3(0.25f, 0.25f, 0)
            };

            int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
            
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();

            return mesh;
        }

        void LateUpdate()
        {
            // Billboard effect - always face camera
            if (SceneLoader.CurrentCamera != null)
            {
                transform.LookAt(SceneLoader.CurrentCamera.Cache.Transform);
                transform.Rotate(0, 180, 0); // Flip to face camera
            }
        }
    }
}