using UnityEngine;

public class TerrainHeightModifier : MonoBehaviour
{
    // Replace these values with your actual parameters
    public Terrain terrain;
    public float heightIncrement = 10.0f;

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain reference not set. Assign the Terrain component to the 'terrain' variable in the inspector.");
            return;
        }

        ModifyTerrainHeight();
    }

    void ModifyTerrainHeight()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        // Loop through each point on the terrain
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                // Get current height at (x, y)
                float currentHeight = heights[x, y];

                // Increase the height by the specified increment
                float newHeight = Mathf.Clamp01(currentHeight + (heightIncrement / terrainData.size.y));

                // Set the new height at (x, y)
                heights[x, y] = newHeight;
            }
        }

        // Apply the modified heights to the terrain
        terrainData.SetHeights(0, 0, heights);
    }
}
