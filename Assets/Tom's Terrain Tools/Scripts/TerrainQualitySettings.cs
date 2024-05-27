using UnityEngine;
using System.Collections;

public class TerrainQualitySettings : MonoBehaviour {

	void Start() {
		UpdateQuality();
	}

	void UpdateQuality() {
		Debug.Log("updating terrain quality");

//		switch (QualitySettings.currentLevel) {
		switch (QualitySettings.GetQualityLevel()) {
//		case QualityLevel.Fastest:
		case 0: // Fastest
				Terrain.activeTerrain.treeDistance = 250.0f;
				Terrain.activeTerrain.treeBillboardDistance = 30.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 5.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 5;

				Terrain.activeTerrain.detailObjectDistance = 30.0f;

				Terrain.activeTerrain.heightmapPixelError = 20.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 1;
				Terrain.activeTerrain.basemapDistance = 100.0f;
				break;

		case 1: // Fast
				Terrain.activeTerrain.treeDistance = 500.0f;
				Terrain.activeTerrain.treeBillboardDistance = 50.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 10.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 10;

				Terrain.activeTerrain.detailObjectDistance = 40.0f;

				Terrain.activeTerrain.heightmapPixelError = 10.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 1;
				Terrain.activeTerrain.basemapDistance = 250.0f;
				break;

		case 2: //Simple
				Terrain.activeTerrain.treeDistance = 650.0f;
				Terrain.activeTerrain.treeBillboardDistance = 75.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 25.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 20;

				Terrain.activeTerrain.detailObjectDistance = 60.0f;

				Terrain.activeTerrain.heightmapPixelError = 8.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 0;
				Terrain.activeTerrain.basemapDistance = 500.0f;
				break;

		case 3: //Good
				Terrain.activeTerrain.treeDistance = 800.0f;
				Terrain.activeTerrain.treeBillboardDistance = 100.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 40.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 30;

				Terrain.activeTerrain.detailObjectDistance = 75.0f;

				Terrain.activeTerrain.heightmapPixelError = 5.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 0;
				Terrain.activeTerrain.basemapDistance = 800.0f;
				break;

		case 4: //Beautiful
				Terrain.activeTerrain.treeDistance = 1000.0f;
				Terrain.activeTerrain.treeBillboardDistance = 150.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 50.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 50;

				Terrain.activeTerrain.detailObjectDistance = 100.0f;

				Terrain.activeTerrain.heightmapPixelError = 5.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 0;
				Terrain.activeTerrain.basemapDistance = 1000.0f;
				break;

		case 5: //Fantastic
				Terrain.activeTerrain.treeDistance = 2000.0f;
				Terrain.activeTerrain.treeBillboardDistance = 250.0f;
				Terrain.activeTerrain.treeCrossFadeLength = 50.0f;
				Terrain.activeTerrain.treeMaximumFullLODCount = 100;

				Terrain.activeTerrain.detailObjectDistance = 200.0f;

				Terrain.activeTerrain.heightmapPixelError = 5.0f;
				Terrain.activeTerrain.heightmapMaximumLOD = 0;
				Terrain.activeTerrain.basemapDistance = 1000.0f;
				break;
			
		}
	}
	
}
