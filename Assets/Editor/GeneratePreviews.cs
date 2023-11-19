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
        if (Directory.Exists(previewPath))
        {
            Directory.Delete(previewPath, true);
        }
        Directory.CreateDirectory(previewPath);
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
            Texture2D tex = AssetPreview.GetAssetPreview(asset);
            int maxTries = 30;
            while (tex == null)
            {
                System.Threading.Thread.Sleep(100);
                tex = AssetPreview.GetAssetPreview(asset);
                maxTries--;
                if (maxTries <= 0)
                    break;
            }
            if (tex != null)
                File.WriteAllBytes(previewPath + "/" + asset.name + "Preview.png", tex.EncodeToPNG());
        }
    }

    static void GenerateTextures(string assetPath, string previewPath)
    {
        string[] textures = Directory.GetFiles(assetPath, "*.*", SearchOption.TopDirectoryOnly).
            Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg")).ToArray();
        foreach (string texture in textures)
        {
            var asset = AssetDatabase.LoadAssetAtPath(texture, typeof(Texture2D));
            Texture2D tex = AssetPreview.GetAssetPreview(asset);
            int maxTries = 30;
            while (tex == null)
            {
                System.Threading.Thread.Sleep(100);
                tex = AssetPreview.GetAssetPreview(asset);
                maxTries--;
                if (maxTries <= 0)
                    break;
            }
            if (tex != null)
                File.WriteAllBytes(previewPath + "/" + asset.name + "Preview.png", tex.EncodeToPNG());
        }
    }
}