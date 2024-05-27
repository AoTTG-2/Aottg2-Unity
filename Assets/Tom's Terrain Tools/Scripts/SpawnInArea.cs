using UnityEngine;
using System.Collections;

public class SpawnInArea : MonoBehaviour {

	public Texture2D SpawnMap;
	float Offset = 10.0f;
	float AboveGround = 1.0f;
	bool TerrainOnly = true;

	// Use this for initialization
	void RandomPositionOnTerrain(GameObject obj) {
		Vector3 size = Terrain.activeTerrain.terrainData.size;
		Vector3 NewPosition = new Vector3();

		bool done = false;
		while (!done) {
			// calculate new random position
			NewPosition = Terrain.activeTerrain.transform.position;
			float w = Random.Range(0.0f, size.x);
			float h = Random.Range(0.0f, size.z);
			NewPosition.x += w;
			NewPosition.y += size.y + Offset; // make sure we are above the terrain
			NewPosition.z += h;

			// verify that position is in spawnmap
			if (SpawnMap) {
				int xmap = Mathf.RoundToInt((float)SpawnMap.width * w/size.x);
				int ymap = Mathf.RoundToInt((float)SpawnMap.height * h/size.z);
				float value = SpawnMap.GetPixel(xmap, ymap).grayscale;
				if (value>0.0f && Random.Range(0.0f, 1.0f)<value) {
					done = true;
				} else {
					done = false;
				}
			} else {
				done = true;
			}

			if (done) {
				// verify that position is above terrain/something
				RaycastHit hit;
				if (Physics.Raycast(NewPosition, -Vector3.up, out hit)) {
					float distanceToGround = hit.distance;
					if (hit.transform.name != "Terrain") {
						if (TerrainOnly) done = false; // there is something else beneath us
					}
					// all is good
					NewPosition.y -= (distanceToGround-AboveGround);
				} else {
					done = false; // there's nothing under us as far as we care to check
				}
			}
				
		}

		// put the object in place
		obj.transform.position = NewPosition;
		// also add random rotation
		transform.Rotate(Vector3.up * Random.Range(0, 360), Space.World);
		
	}
	
}
