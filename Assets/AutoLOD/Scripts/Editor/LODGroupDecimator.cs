using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace AutoLOD.MeshDecimator.Utils
{
    public class LODGroupDecimator
    {
        [MenuItem("CONTEXT/LODGroup/Generate Missing LODs/AutoLOD (Fast)")]
        public static void ContextMenu(MenuCommand command)
        {
            LODGroup lodGroup = (LODGroup)command.context;
            LODGroupSolvability solvability = LODGroupSolver.GetLODGroupSolvability(lodGroup);
            switch (solvability)
            {
                case LODGroupSolvability.Compatible:
                    ProcessLODGroup(lodGroup, MeshDecimatorBackend.Fast);
                    break;
                case LODGroupSolvability.Empty:
                    Debug.LogWarning("[AutoLOD] Empty LODGroup. Please add LOD levels and/or renderers in LOD 0.");
                    break;
                case LODGroupSolvability.Incompatible:
                default:
                    Debug.LogWarning("[AutoLOD] Incompatible LODGroup. All LODs must have either no renderers or exactly the same number of renderers as LOD0.");
                    break;
            }
        }

        [MenuItem("CONTEXT/LODGroup/Generate Missing LODs/AutoLOD (HQ)")]
        public static void ContextMenuHQ(MenuCommand command)
        {
            LODGroup lodGroup = (LODGroup)command.context;
            LODGroupSolvability solvability = LODGroupSolver.GetLODGroupSolvability(lodGroup);
            switch (solvability)
            {
                case LODGroupSolvability.Compatible:
                    ProcessLODGroup(lodGroup, MeshDecimatorBackend.HighQuality);
                    break;
                case LODGroupSolvability.Empty:
                    Debug.LogWarning("[AutoLOD] Empty LODGroup. Please add LOD levels and/or renderers in LOD 0.");
                    break;
                case LODGroupSolvability.Incompatible:
                default:
                    Debug.LogWarning("[AutoLOD] Incompatible LODGroup. All LODs must have either no renderers or exactly the same number of renderers as LOD0.");
                    break;
            }
        }

        static void ReportProgress(string message, float value)
        {
            EditorUtility.DisplayProgressBar("AutoLOD", message, value);
        }

        static void ReportDecimationStatus(int iteration, int start, int current, int end)
        {
            if (end > start)
            {
                string message = "Blendshapes Interpolation: This process can take a long time";
                ReportProgress(message, 1f);
            }
            else
            {
                double progress = 1.0 - (current - Mathf.Min(start, end)) / (double)Mathf.Abs(start - end);
                string message = "AutoLOD Decimation Process: " + ((int)(100 * progress)).ToString() + "%";
                ReportProgress(message, (float)progress);
            }
        }

        private static void ProcessLODGroup(LODGroup lodGroup, MeshDecimatorBackend backend = MeshDecimatorBackend.Fast)
        {
            Undo.RecordObject(lodGroup, "Generating Missing LODs");
            IMeshDecimator meshDecimator;
            switch (backend)
            {
                case MeshDecimatorBackend.HighQuality:
                    meshDecimator = new CQualityMeshDecimator();
                    break;
                case MeshDecimatorBackend.Fast:
                default:
                    meshDecimator = new CFastMeshDecimator();
                    break;
            }
            meshDecimator.Initialize();
            meshDecimator.statusReportInvoker += ReportDecimationStatus;

            LOD[] lods = lodGroup.GetLODs();
            Renderer[] referenceRenderers = lods[0].renderers;
            for(int i = 1; i < lods.Length; ++i)
            {
                LOD lod = lods[i];
                if (lod.renderers != null && lod.renderers.Length > 0)
                    continue;

                float ratio = lods[i-1].screenRelativeTransitionHeight;
                float reductionRate = 1f / ratio;
                Renderer[] decimatedRenderers = new Renderer[referenceRenderers.Length];
                int rendererIndex = 0;
                foreach(Renderer referenceRenderer in referenceRenderers)
                {
                    Transform commonParent = referenceRenderer.transform.parent;
                    if (commonParent == null)
                        commonParent = referenceRenderer.transform;

                    Mesh sourceMesh;

                    if (referenceRenderer.GetType() == typeof(SkinnedMeshRenderer))
                    {
                        sourceMesh = (referenceRenderer as SkinnedMeshRenderer).sharedMesh;
                    }
                    else
                    {
                        if(referenceRenderer.GetComponent<MeshFilter>() == null)
                        {
                            Debug.LogError("[AutoLOD] Ignoring " + referenceRenderer.name + " which has no Mesh Filter nor SkinnedMeshRenderer");
                            continue;
                        }
                        sourceMesh = referenceRenderer.GetComponent<MeshFilter>().sharedMesh;
                    }
                    Mesh decimatedMesh = meshDecimator.DecimateMesh(sourceMesh, Mathf.RoundToInt(sourceMesh.triangles.Length / (3 * reductionRate)), sourceMesh.blendShapeCount > 1);
                    string savePath = (EditorPrefs.GetString("autolodDefaultExportFolder", "Assets/AutoLOD/Generated")).Replace("//", "/");
                    if (savePath.StartsWith("Assets/"))
                        savePath = savePath.Substring(7);
                    if (!Directory.Exists(Application.dataPath + "/" + savePath))
                    {
                        Directory.CreateDirectory(Application.dataPath + "/" + savePath);
                    }
                    string lodName = referenceRenderer.gameObject.name + "_LOD" + i;
                    AssetDatabase.CreateAsset(decimatedMesh, AssetDatabase.GenerateUniqueAssetPath("Assets/" + savePath + "/" + lodName + ".asset"));
                   
                    GameObject clone = new GameObject(lodName);
                    Undo.RegisterCreatedObjectUndo(clone, "Created LOD" + i);
                    clone.transform.SetParent(commonParent);
                    clone.transform.position = referenceRenderer.transform.position;
                    clone.transform.rotation = referenceRenderer.transform.rotation;
                    clone.transform.localScale = referenceRenderer.transform.localScale;
                    if (referenceRenderer.GetType() == typeof(SkinnedMeshRenderer))
                    {
                        SkinnedMeshRenderer renderer = clone.AddComponent<SkinnedMeshRenderer>();
                        renderer.bones = referenceRenderer.GetComponent<SkinnedMeshRenderer>().bones;
                        decimatedMesh.bindposes = sourceMesh.bindposes;
                        renderer.sharedMesh = decimatedMesh;
                        renderer.sharedMaterials = referenceRenderer.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
                    }
                    else
                    {
                        MeshFilter filter = clone.AddComponent<MeshFilter>();
                        MeshRenderer renderer = clone.AddComponent<MeshRenderer>();
                        filter.sharedMesh = decimatedMesh;
                        renderer.sharedMaterials = referenceRenderer.GetComponent<MeshRenderer>().sharedMaterials;
                    }
                    decimatedRenderers[rendererIndex++] = clone.GetComponent<Renderer>();
                }
                lod.renderers = decimatedRenderers;
                lods[i] = lod;
            }
            lodGroup.SetLODs(lods);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
    }
}
