// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Settings;

// public class EMSceneSettingsManager : MonoBehaviour
// {
//     void Start()
//     {
//         // Make this GameObject persistent
//         DontDestroyOnLoad(gameObject);
//     }

//     void OnEnable()
//     {
//         Debug.Log("TerrainDetailManager enabled");
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     void OnDisable()
//     {
//         Debug.Log("TerrainDetailManager disabled");
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         Debug.Log("Scene loaded: " + scene.name);
//         SetTerrainDetails(GraphicsSettings.Instance.DetailDistance.Value, GraphicsSettings.Instance.DetailDensity.Value);
//         Debug.Log("Update Terrain Settings ");
//     }

//     // Added by Snake for Terrain Detail Slider 28 may 24 
//     public void SetTerrainDetails(int DetailDistance, int DetailDensity)
//     {
//         // Find all Terrain objects in the scene
//         Terrain[] terrains = GameObject.FindObjectsOfType<Terrain>();
//         foreach (Terrain terrain in terrains)
//         {
//             // Adjust the detailObjectDistance property
//             terrain.detailObjectDistance = DetailDistance;
//             terrain.detailObjectDensity = DetailDensity; // Set this to your desired value
//         }
//     }
//  }
