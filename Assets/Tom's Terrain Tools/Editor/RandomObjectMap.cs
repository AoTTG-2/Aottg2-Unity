using UnityEditor;
using UnityEngine;

namespace TTT
{

	public class RandomObjectMap : ScriptableWizard 
	{


		public Texture2D SpawnMap;
		public GameObject SpawnObject;
		public int MaxNumber = 16;
		public int Spacing = 1;
		public float AboveGround = 0.0f;
		public LayerMask UseLayer = 1;
		public bool alignToSurface = false;

		float Offset = 10.0f;
		int MaxAttempts = 100;


		[MenuItem("Window/Terrain Tools/Random Object Map",false,102)]
		static void CreateWindow() {
			ScriptableWizard.DisplayWizard("Random Object Map", typeof(RandomObjectMap), "Distribute objects");
		}

		void OnWizardUpdate() {

			if(Terrain.activeTerrain==null)
			{
				errorString = "No active terrain in the scene";
				return;
			}else{
				errorString = "";
			}

			
			helpString = "Choose a grayscale Overlay map\nand a game object to place.";

			if (SpawnMap != null) { 
				if (SpawnMap.format != TextureFormat.ARGB32 && SpawnMap.format != TextureFormat.RGB24) {
					EditorUtility.DisplayDialog("Wrong format", "SpawnMap must be in RGBA 32 bit or RGB 24 bit format", "Cancel"); 
					return;
				}

				int w = SpawnMap.width;
				if (SpawnMap.height != w) {
					EditorUtility.DisplayDialog("Wrong size", "SpawnMap width and height must be the same", "Cancel"); 
					return;
				}
				if (Mathf.ClosestPowerOfTwo(w) != w) {
					EditorUtility.DisplayDialog("Wrong size", "SpawnMap width and height must be a power of two", "Cancel"); 
					return;	
				}

				
				if (w!=Terrain.activeTerrain.terrainData.alphamapResolution) {
					EditorUtility.DisplayDialog("Wrong size", "SpawnMap must have same size ("+w+") as existing splatmap ("+Terrain.activeTerrain.terrainData.alphamapResolution+")", "Cancel"); 
					return;	
				}
			}
			

		}


		void OnWizardCreate() 
		{

						
			if (Terrain.activeTerrain==null) {
				//EditorUtility.DisplayDialog("Error", "You must have an active terrain in the scene", "Cancel"); 
				Debug.LogError("You must have an active terrain in the scene first");
				return;	
			}


			Vector3 size = Terrain.activeTerrain.terrainData.size;
			Vector3 NewPosition = new Vector3();
			int placed = 0;

			// make "folder" for objects
			GameObject folderGO = new GameObject("RandomObjects");

			for (int i=0; i<MaxNumber; i++) 
			{
				EditorUtility.DisplayProgressBar("Placing objects", "calculating...", Mathf.InverseLerp(0.0f, (float)MaxNumber, (float)i));


				for (int j=0; j<MaxAttempts; j++) 
				{
					// calculate new random position
					NewPosition = Terrain.activeTerrain.transform.position;
					float w = Random.Range(0.0f, size.x);
					float h = Random.Range(0.0f, size.z);
					NewPosition.x += w;
					NewPosition.y += size.y + Offset; // make sure we are above the terrain
					NewPosition.z += h;

					// check against spawnmap
					int xmap = Mathf.RoundToInt((float)SpawnMap.width * w/size.x);
					int ymap = Mathf.RoundToInt((float)SpawnMap.height * h/size.z);
					float value = SpawnMap.GetPixel(xmap, ymap).grayscale;

					if (value>0.0f && Random.Range(0.0f, 1.0f)<value) 
					{
						// verify that position is above terrain/something
						RaycastHit hit;
						if (Physics.Raycast(NewPosition, -Vector3.up, out hit, Mathf.Infinity, UseLayer)) 
						{
							float distanceToGround = hit.distance;
							NewPosition.y -= (distanceToGround-AboveGround);

							// instantiate object
							GameObject clone = Instantiate(SpawnObject, NewPosition, Quaternion.identity) as GameObject;
							if (alignToSurface) clone.transform.rotation = Quaternion.LookRotation(clone.transform.forward, hit.normal);
							clone.transform.parent = folderGO.transform;
							placed++;
							j=MaxAttempts;

							// clear out space
							if (Spacing>0) 
							{
								for (int xx=Mathf.Max(0,xmap-Spacing); xx<=Mathf.Min(SpawnMap.width,xmap+Spacing); xx++) 
								{
									for (int yy=Mathf.Max(0,ymap-Spacing); yy<=Mathf.Min(SpawnMap.height,ymap+Spacing); yy++) 
									{
										SpawnMap.SetPixel(xx, yy, Color.black);
									}
								}
							}
						}
					}
				}
			}
			EditorUtility.ClearProgressBar();
			Debug.Log("placed "+placed+" objects");


		}
	}

}