using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetParentPosition : MonoBehaviour
{
    public Material material; // The material that uses the Shader Graph

    void Update()
    {
        if (material != null)
        {
            Vector3 parentWorldPosition = transform.parent != null ? transform.parent.position : Vector3.zero;
            Vector3 parentWorldScale = transform.parent != null ? transform.parent.lossyScale : Vector3.zero;
            material.SetVector("_Parent_Scale", parentWorldScale);
            material.SetVector("_Parent_World_Position", parentWorldPosition); // Pass to shader
        }
    }
}
