using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class GeneratePreviews
{
    [MenuItem("Assets/Generate Previews")]
    static void Generate()
    {
        string rootPath = "Assets/Resources/Map";
        string previewPath = rootPath + "/Previews";
        if (!Directory.Exists(previewPath))
        {
            Directory.CreateDirectory(previewPath);
        }
        foreach (string categoryPath in Directory.GetDirectories(rootPath))
        {
            if (categoryPath.EndsWith("Textures"))
            {
                foreach (string texturePath in Directory.GetDirectories(categoryPath))
                    GenerateTextures(texturePath, previewPath);
            }
            else
            {
                foreach (string prefabPath in Directory.GetDirectories(categoryPath))
                {
                    if (prefabPath.EndsWith("Prefabs"))
                        GeneratePrefabs(prefabPath, previewPath);
                }
            }
        }
    }

    static void GeneratePrefabs(string assetPath, string previewPath)
    {
        string[] prefabs = Directory.GetFiles(assetPath, "*.prefab", SearchOption.TopDirectoryOnly);
        foreach (string prefab in prefabs)
        {
            var asset = AssetDatabase.LoadAssetAtPath(prefab, typeof(GameObject));
            string filePath = previewPath + "/" + asset.name + "Preview.png";
            if (File.Exists(filePath))
                continue;
            
            int instanceID = asset.GetInstanceID();
            Texture2D tex = AssetPreview.GetAssetPreview(asset);
            int maxTries = 100;
            
            while ((tex == null || AssetPreview.IsLoadingAssetPreview(instanceID)) && maxTries > 0)
            {
                System.Threading.Thread.Sleep(100);
                tex = AssetPreview.GetAssetPreview(asset);
                maxTries--;
            }
            
            if (tex != null)
            {
                File.WriteAllBytes(filePath, tex.EncodeToPNG());
            }
            else
            {
                Debug.LogWarning("Failed to generate preview for prefab: " + prefab + " (timeout after 10 seconds)");
            }
        }
    }

    static void GenerateTextures(string assetPath, string previewPath)
    {
        string[] textures = Directory.GetFiles(assetPath, "*.*", SearchOption.TopDirectoryOnly).
            Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg")).ToArray();
        foreach (string texture in textures)
        {
            var asset = AssetDatabase.LoadAssetAtPath(texture, typeof(Texture2D));
            string filePath = previewPath + "/" + asset.name + "Preview.png";
            if (File.Exists(filePath))
                continue;
            
            int instanceID = asset.GetInstanceID();
            Texture2D tex = AssetPreview.GetAssetPreview(asset);
            int maxTries = 100;
            
            while ((tex == null || AssetPreview.IsLoadingAssetPreview(instanceID)) && maxTries > 0)
            {
                System.Threading.Thread.Sleep(100);
                tex = AssetPreview.GetAssetPreview(asset);
                maxTries--;
            }
            
            if (tex != null)
            {
                File.WriteAllBytes(previewPath + "/" + asset.name + "Preview.png", tex.EncodeToPNG());
            }
            else
            {
                Debug.LogWarning("Failed to generate preview for texture: " + texture + " (timeout after 10 seconds)");
            }
        }
    }
}