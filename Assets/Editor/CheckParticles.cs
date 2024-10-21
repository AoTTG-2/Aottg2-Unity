using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CheckParticles
{
    [MenuItem("Assets/CheckParticles")]
    static void Check()
    {
        string[] prefabs = Directory.GetFiles("Assets/Resources", "*.prefab", SearchOption.AllDirectories);
        foreach (string prefab in prefabs)
        {
            var asset = (GameObject)AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject));
            foreach (var particle in asset.GetComponentsInChildren<ParticleSystemRenderer>())
            {
                if (particle.renderMode == ParticleSystemRenderMode.Mesh)
                {
                    if (particle.sharedMaterial.shader.name.Contains("Standard"))
                        Debug.Log(prefab);
                }
            }
        }
    }
}