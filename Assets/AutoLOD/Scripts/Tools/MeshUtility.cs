using System.Collections.Generic;
using UnityEngine;

namespace AutoLOD.MeshDecimator {
    public class AutoLODMeshUtility
    {
        public static void Smooth2FlatShading(Mesh mesh)
        {
            Vector3[] oldVerts = mesh.vertices;
            Vector2[] oldUv = mesh.uv;
            Vector2[] oldUv2 = mesh.uv2;
            Vector2[] oldUv3 = mesh.uv3;
            Vector2[] oldUv4 = mesh.uv4;
            Color[] oldColors = mesh.colors;
            BoneWeight[] oldBoneWeights = mesh.boneWeights;

            bool hasUv = oldUv.Length != 0;
            bool hasUv2 = oldUv2.Length != 0;
            bool hasUv3 = oldUv3.Length != 0;
            bool hasUv4 = oldUv4.Length != 0;
            bool hasColors = oldColors.Length != 0;
            bool hasBones = oldBoneWeights.Length != 0;

            List<Vector3> newVertices = new List<Vector3>();
            List<int>[] newTriangles = new List<int>[mesh.subMeshCount];
            List<Vector2> newUv = new List<Vector2>();
            List<Vector2> newUv2 = new List<Vector2>();
            List<Vector2> newUv3 = new List<Vector2>();
            List<Vector2> newUv4 = new List<Vector2>();
            List<Color> newColors = new List<Color>();
            List<BoneWeight> newBoneWeights = new List<BoneWeight>();

            for (int submesh = 0; submesh < mesh.subMeshCount; submesh++)
            {
                int[] submeshTriangles = mesh.GetTriangles(submesh);
                newTriangles[submesh] = new List<int>();

                for (int i = 0; i < submeshTriangles.Length; i++)
                {
                    int vertexIndex = submeshTriangles[i];

                    newVertices.Add(oldVerts[vertexIndex]);
                    newTriangles[submesh].Add(newVertices.Count - 1);

                    if (hasUv) newUv.Add(oldUv[vertexIndex]);
                    if (hasUv2) newUv2.Add(oldUv2[vertexIndex]);
                    if (hasUv3) newUv3.Add(oldUv3[vertexIndex]);
                    if (hasUv4) newUv4.Add(oldUv4[vertexIndex]);
                    if (hasColors) newColors.Add(oldColors[vertexIndex]);
                    if (hasBones) newBoneWeights.Add(oldBoneWeights[vertexIndex]);
                }
            }

            if (newVertices.Count > 65535)
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            mesh.vertices = newVertices.ToArray();
            if (hasUv) mesh.uv = newUv.ToArray();
            if (hasUv2) mesh.uv2 = newUv2.ToArray();
            if (hasUv3) mesh.uv3 = newUv3.ToArray();
            if (hasUv4) mesh.uv4 = newUv4.ToArray();
            if (hasColors) mesh.colors = newColors.ToArray();
            if (hasBones) mesh.boneWeights = newBoneWeights.ToArray();

            for (int submesh = 0; submesh < mesh.subMeshCount; submesh++)
            {
                mesh.SetTriangles(newTriangles[submesh].ToArray(), submesh);
            }

            mesh.RecalculateNormals();
        }
    }
}
