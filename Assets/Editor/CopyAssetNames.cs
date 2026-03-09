using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class CopyAssetNames
{
    [MenuItem("Assets/Copy Asset Names", false, 2000)]
    static void CopyNames()
    {
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("No assets selected.");
            return;
        }

        List<string> assetNames = new List<string>();
        
        foreach (Object obj in Selection.objects)
        {
            if (obj != null)
            {
                assetNames.Add(obj.name);
            }
        }

        if (assetNames.Count > 0)
        {
            string result = string.Join(", ", assetNames);
            GUIUtility.systemCopyBuffer = result;
            Debug.Log($"Copied {assetNames.Count} asset name(s) to clipboard: {result}");
        }
        else
        {
            Debug.LogWarning("No valid assets to copy.");
        }
    }

    // Validation method - only show menu item when assets are selected
    [MenuItem("Assets/Copy Asset Names", true)]
    static bool ValidateCopyNames()
    {
        return Selection.objects.Length > 0;
    }

    [MenuItem("Assets/Copy Asset Names (JSON Format)", false, 2001)]
    static void CopyNamesJSON()
    {
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("No assets selected.");
            return;
        }

        List<string> assetNames = new List<string>();
        
        foreach (Object obj in Selection.objects)
        {
            if (obj != null)
            {
                assetNames.Add($"{{\"Name\": \"{obj.name}\"}}");
            }
        }

        if (assetNames.Count > 0)
        {
            string result = string.Join(",\n\t\t\t", assetNames) + ",";
            GUIUtility.systemCopyBuffer = result;
            Debug.Log($"Copied {assetNames.Count} asset name(s) to clipboard in JSON format.");
        }
        else
        {
            Debug.LogWarning("No valid assets to copy.");
        }
    }

    [MenuItem("Assets/Copy Asset Names (JSON Format)", true)]
    static bool ValidateCopyNamesJSON()
    {
        return Selection.objects.Length > 0;
    }

    [MenuItem("Assets/Copy Asset Names (JSON with Variants)", false, 2002)]
    static void CopyNamesJSONWithVariants()
    {
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("No assets selected.");
            return;
        }

        List<string> assetNames = new List<string>();
        string baseAssetName = null;
        
        foreach (Object obj in Selection.objects)
        {
            if (obj != null)
            {
                if (baseAssetName == null)
                {
                    // First asset is the base
                    baseAssetName = obj.name;
                    assetNames.Add($"{{\"Name\": \"{obj.name}\"}}");
                }
                else
                {
                    // Subsequent assets are variants
                    assetNames.Add($"{{\"Name\": \"{obj.name}\", \"Variant\": \"{baseAssetName}\"}}");
                }
            }
        }

        if (assetNames.Count > 0)
        {
            string result = string.Join(",\n\t\t\t", assetNames);
            GUIUtility.systemCopyBuffer = result;
            Debug.Log($"Copied {assetNames.Count} asset name(s) to clipboard in JSON format with variants (base: {baseAssetName}).");
        }
        else
        {
            Debug.LogWarning("No valid assets to copy.");
        }
    }

    [MenuItem("Assets/Copy Asset Names (JSON with Variants)", true)]
    static bool ValidateCopyNamesJSONWithVariants()
    {
        return Selection.objects.Length > 0;
    }
}
