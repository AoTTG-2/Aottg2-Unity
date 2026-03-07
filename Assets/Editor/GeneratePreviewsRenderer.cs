using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEditor.SceneManagement;

public class GeneratePreviewsRenderer : EditorWindow
{
    private static string rootPath = "Assets/Resources/Map";
    private static string previewPath;
    private static int previewSize = 128;
    private static float particleWaitTime = 0.1f;
    private static bool isGenerating = false;
    private static int currentIndex = 0;
    private static string[] allPrefabs;
    private static bool fxOnly = true;
    private static bool useLight = true;
    private static Color backgroundColor = new Color(0.322f, 0.322f, 0.322f, 1f);

    [MenuItem("Assets/Generate Previews (Renderer)")]
    static void ShowWindow()
    {
        GetWindow<GeneratePreviewsRenderer>("Preview Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Preview Generator (Renderer)", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("This tool renders prefabs in-scene to generate previews. Works better for particle systems and effects.", MessageType.Info);
        GUILayout.Space(10);

        fxOnly = EditorGUILayout.Toggle("Generate FX Only", fxOnly);
        previewSize = EditorGUILayout.IntField("Preview Size", previewSize);
        particleWaitTime = EditorGUILayout.FloatField("Particle Wait Time (s)", particleWaitTime);
        
        GUILayout.Space(5);
        GUILayout.Label("Rendering Options", EditorStyles.boldLabel);
        useLight = EditorGUILayout.Toggle("Use Directional Light", useLight);
        backgroundColor = EditorGUILayout.ColorField("Background Color", backgroundColor);
        
        GUILayout.Space(10);

        if (isGenerating)
        {
            GUI.enabled = false;
            GUILayout.Label($"Generating... {currentIndex}/{allPrefabs?.Length ?? 0}");
            if (allPrefabs != null && allPrefabs.Length > 0)
            {
                float progress = (float)currentIndex / allPrefabs.Length;
                EditorGUI.ProgressBar(GUILayoutUtility.GetRect(200, 20), progress, $"{currentIndex}/{allPrefabs.Length}");
            }
            GUI.enabled = true;
        }
        else
        {
            if (GUILayout.Button("Generate All Prefab Previews", GUILayout.Height(30)))
            {
                GenerateAllPreviews();
            }
        }
    }

    static void GenerateAllPreviews()
    {
        previewPath = rootPath + "/Previews2";
        if (!Directory.Exists(previewPath))
        {
            Directory.CreateDirectory(previewPath);
        }

        allPrefabs = CollectAllPrefabs();
        if (allPrefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs found to generate previews for.");
            return;
        }

        currentIndex = 0;
        isGenerating = true;
        EditorApplication.update += GenerateNextPreview;
    }

    static string[] CollectAllPrefabs()
    {
        System.Collections.Generic.List<string> prefabList = new System.Collections.Generic.List<string>();
        
        foreach (string categoryPath in Directory.GetDirectories(rootPath))
        {
            string categoryName = Path.GetFileName(categoryPath);
            
            if (fxOnly && categoryName != "FX")
                continue;
            
            if (!categoryPath.EndsWith("Textures") && !categoryPath.EndsWith("Previews"))
            {
                foreach (string prefabPath in Directory.GetDirectories(categoryPath))
                {
                    if (prefabPath.EndsWith("Prefabs"))
                    {
                        string[] prefabs = Directory.GetFiles(prefabPath, "*.prefab", SearchOption.TopDirectoryOnly);
                        prefabList.AddRange(prefabs);
                    }
                }
            }
        }

        return prefabList.ToArray();
    }

    static void GenerateNextPreview()
    {
        if (currentIndex >= allPrefabs.Length)
        {
            EditorApplication.update -= GenerateNextPreview;
            isGenerating = false;
            Debug.Log($"Preview generation complete! Generated previews for {allPrefabs.Length} prefabs.");
            return;
        }

        string prefab = allPrefabs[currentIndex];
        GeneratePrefabPreview(prefab);
        currentIndex++;

        if (GetWindow<GeneratePreviewsRenderer>(false) != null)
        {
            GetWindow<GeneratePreviewsRenderer>().Repaint();
        }
    }

    static void GeneratePrefabPreview(string prefabPath)
    {
        GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefabAsset == null)
        {
            Debug.LogWarning($"Could not load prefab: {prefabPath}");
            return;
        }

        string filePath = previewPath + "/" + prefabAsset.name + "Preview.png";
        if (File.Exists(filePath))
        {
            Debug.Log($"Preview already exists for: {prefabAsset.name}");
            return;
        }

        GameObject instance = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
        if (instance == null)
        {
            Debug.LogWarning($"Could not instantiate prefab: {prefabPath}");
            return;
        }

        try
        {
            Bounds bounds = CalculateBounds(instance);
            
            GameObject lightObj = null;
            if (useLight)
            {
                lightObj = new GameObject("PreviewLight");
                Light light = lightObj.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.5f;
                light.color = Color.white;
                lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            }
            
            GameObject cameraObj = new GameObject("PreviewCamera");
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = backgroundColor;
            camera.orthographic = false;

            float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            if (maxSize < 0.01f) maxSize = 1f;
            
            float distance = maxSize * 2.5f;
            Vector3 cameraPos = bounds.center + new Vector3(distance * 0.7f, distance * 0.5f, -distance * 0.7f);
            cameraObj.transform.position = cameraPos;
            cameraObj.transform.LookAt(bounds.center);

            ParticleSystem[] particleSystems = instance.GetComponentsInChildren<ParticleSystem>();
            bool hasParticles = particleSystems.Length > 0;

            if (hasParticles)
            {
                foreach (ParticleSystem ps in particleSystems)
                {
                    ps.Play();
                    var main = ps.main;
                    main.playOnAwake = true;
                }
                
                System.Threading.Thread.Sleep((int)(particleWaitTime * 1000));
                
                for (int i = 0; i < 10; i++)
                {
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Simulate(0.1f, true, false);
                    }
                }
            }

            RenderTexture rt = new RenderTexture(previewSize, previewSize, 24);
            camera.targetTexture = rt;
            camera.Render();

            RenderTexture.active = rt;
            Texture2D preview = new Texture2D(previewSize, previewSize, TextureFormat.RGB24, false);
            preview.ReadPixels(new Rect(0, 0, previewSize, previewSize), 0, 0);
            preview.Apply();

            byte[] bytes = preview.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);

            RenderTexture.active = null;
            camera.targetTexture = null;
            DestroyImmediate(rt);
            DestroyImmediate(preview);
            DestroyImmediate(cameraObj);
            if (lightObj != null)
                DestroyImmediate(lightObj);

            Debug.Log($"Generated preview for: {prefabAsset.name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating preview for {prefabAsset.name}: {e.Message}");
        }
        finally
        {
            DestroyImmediate(instance);
        }
    }

    static Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        
        if (renderers.Length == 0)
        {
            return new Bounds(obj.transform.position, Vector3.one);
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        if (bounds.size.magnitude < 0.01f)
        {
            bounds.size = Vector3.one;
        }

        return bounds;
    }
}
