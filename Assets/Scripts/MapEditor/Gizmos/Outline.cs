//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]

public class Outline : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    public enum Mode
    {
        OutlineAll,
        OutlineVisible,
        OutlineHidden,
        OutlineAndSilhouette,
        SilhouetteOnly,
        OutlineAndLightenColor
    }

    public Mode OutlineMode
    {
        get { return outlineMode; }
        set
        {
            outlineMode = value;
            needsUpdate = true;
        }
    }

    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
            needsUpdate = true;
        }
    }

    public float OutlineWidth
    {
        get { return outlineWidth; }
        set
        {
            outlineWidth = value;
            needsUpdate = true;
        }
    }

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    [SerializeField]
    private Mode outlineMode;

    [SerializeField]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;

    [Header("Optional")]

    [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
    + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
    private bool precomputeOutline;

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private HashSet<Renderer> renderers;
    private Material outlineMaskAndFillMaterial;

    private bool needsUpdate;
    private List<string> _namesToIgnore = new List<String>();

    void Awake()
    {

        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>().ToHashSet();

        // Instantiate outline materials
        outlineMaskAndFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMaskAndFill"));

        outlineMaskAndFillMaterial.name = "OutlineMaskAndFill (Instance)";

        // Retrieve or generate smooth normals
        LoadSmoothNormals();

        // Apply material properties immediately
        needsUpdate = true;
    }
    
    // Create a function that takes in a filter predicate where the argument is a Renderer and filters using that
    public void RefreshRenderers(List<string> namesToIgnore)
    {
        _namesToIgnore = namesToIgnore;
        registeredMeshes.Clear();
        bakeValues.Clear();
        bakeKeys.Clear();

        bool isEnabled = enabled;

        OnDisable();
        
        // Update renderer cache
        renderers = GetComponentsInChildren<Renderer>().Where(e => !_namesToIgnore.Where(a => e.name.Contains(a)).Any()).ToHashSet();

        // Retrieve or generate smooth normals
        LoadSmoothNormals();

        OnEnable();

        // Apply material properties immediately
        needsUpdate = true;

        if (isEnabled)
        {
            OnEnable();
        }
        else
        {
            OnDisable();
        }

    }

    void OnEnable()
    {
        renderers.RemoveWhere(e =>
        {
            if (!e == false)
            {
                // Append outline shaders
                var materials = e.sharedMaterials.ToList();

                // Only add if material does not exist
                if (!materials.Contains(outlineMaskAndFillMaterial))
                    materials.Add(outlineMaskAndFillMaterial);
                e.materials = materials.ToArray();
            }
            return !e;
        });
    }

    void OnValidate()
    {

        // Update material properties
        needsUpdate = true;

        // Clear cache when baking is disabled or corrupted
        if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
        {
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        // Generate smooth normals when baking is enabled
        if (precomputeOutline && bakeKeys.Count == 0)
        {
            Bake();
        }
    }

    void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;

            UpdateMaterialProperties();
        }
    }

    void OnDisable()
    {
        renderers.RemoveWhere(e =>
        {
            if (!e == false)
            {
                var materials = e.sharedMaterials.ToList();
                materials.Remove(outlineMaskAndFillMaterial);
                e.materials = materials.ToArray();
            }
            return !e;
        });
    }

    void OnDestroy()
    {

        // Destroy material instances
        Destroy(outlineMaskAndFillMaterial);
    }

    void Bake()
    {

        // Generate smooth normals for each mesh
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {

            // Skip duplicates
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Serialize smooth normals
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(new ListVector3() { data = smoothNormals });
        }
    }

    void LoadSmoothNormals()
    {

        // Retrieve or generate smooth normals
        var filters = GetComponentsInChildren<MeshFilter>().ToHashSet();
        filters.RemoveWhere(e => !e && !_namesToIgnore.Where(a => e.name.Contains(a)).Any());
        foreach (var meshFilter in filters)
        {

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                continue;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Combine submeshes
            var renderer = meshFilter.GetComponent<Renderer>();

            if (renderer != null)
            {
                CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
            }
        }

        // Clear UV3 on skinned mesh renderers
        var skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToHashSet();
        skinnedMeshRenderers.RemoveWhere(e => !e && !_namesToIgnore.Where(a => e.name.Contains(a)).Any());
        foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
        {

            // Skip if UV3 has already been reset
            if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
            {
                continue;
            }

            // Clear UV3
            skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

            // Combine submeshes
            CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
        }
    }

    List<Vector3> SmoothNormals(Mesh mesh)
    {

        // Group vertices by location
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices
        foreach (var group in groups)
        {

            // Skip single vertices
            if (group.Count() == 1)
            {
                continue;
            }

            // Calculate the average normal
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
            {
                smoothNormal += smoothNormals[pair.Value];
            }

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex
            foreach (var pair in group)
            {
                smoothNormals[pair.Value] = smoothNormal;
            }
        }

        return smoothNormals;
    }

    void CombineSubmeshes(Mesh mesh, Material[] materials)
    {

        // Skip meshes with a single submesh
        if (mesh.subMeshCount == 1)
        {
            return;
        }

        // Skip if submesh count exceeds material count
        if (mesh.subMeshCount > materials.Length)
        {
            return;
        }

        // Append combined submesh
        mesh.subMeshCount++;
        mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
    }

    void UpdateMaterialProperties()
    {

        // Apply properties according to mode
        outlineMaskAndFillMaterial.SetColor("_OutlineColor", outlineColor);

        switch (outlineMode)
        {
            case Mode.OutlineAll:
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Never);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineVisible:
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Never);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineHidden:
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Never);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.OutlineAndSilhouette:
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Never);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;

            case Mode.SilhouetteOnly:
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Never);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", 0f);
                break;

            case Mode.OutlineAndLightenColor:
                //outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTestMask", (float)UnityEngine.Rendering.CompareFunction.Always);
                outlineMaskAndFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                outlineMaskAndFillMaterial.SetFloat("_ZTest2", (float)UnityEngine.Rendering.CompareFunction.Greater);
                outlineMaskAndFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                break;
        }
    }
}
