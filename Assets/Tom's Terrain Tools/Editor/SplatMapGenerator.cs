using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

// create splatmaps from existing terrain, work in progress, not much features here yet

namespace TTT_Tools
{

	public class SplatMapGenerator : EditorWindow
	{
		Terrain myTerrain;

		// options
		float redThresholdMin=0f;
		float redThresholdMax=25f;

		float greenThresholdMin=25f;
		float greenThresholdMax=75f;

		float blueThresholdMin=75f;
		float blueThresholdMax=100f;

		float minHeight=0f;
		float maxHeight=100f;

		float? lowestPoint=null;
		float? highestPoint=null;

		[MenuItem ("Window/Terrain Tools/Create Splatmap from Terrain",false,105)]
		public static void  ShowWindow () 
		{
			var window = EditorWindow.GetWindow(typeof(SplatMapGenerator));
			window.titleContent = new GUIContent("SplatmapGenerator");
			window.minSize = new Vector2(450,300);

		}
		
		void OnGUI () 
		{
			DrawGUITitle();
			DrawGUITerrainSelection();


			DrawGUISettings();

		}

		void DrawGUITitle ()
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Splatmap Generator", EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();
			EditorGUILayout.Separator ();
		}

		void DrawGUITerrainSelection()
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Terrain");
			myTerrain = (Terrain)EditorGUILayout.ObjectField("", myTerrain, typeof(Terrain),true);
			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}

		void DrawGUISettings()
		{

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("World height");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(lowestPoint!=null?((int)lowestPoint).ToString("0"):"-");
			GUILayout.FlexibleSpace();
			GUILayout.Label(highestPoint!=null?((int)highestPoint).ToString("0"):"-");
			GUILayout.EndHorizontal();

			GUI.enabled = false;
			GUILayout.BeginHorizontal();
			GUILayout.Label("R",GUILayout.Width(20));
			EditorGUILayout.MinMaxSlider(ref redThresholdMin,ref redThresholdMax,minHeight,maxHeight);
			GUILayout.EndHorizontal();

			redThresholdMin=0;
			redThresholdMax=greenThresholdMin;

			GUI.enabled = true;
			GUILayout.BeginHorizontal();
			GUILayout.Label("G",GUILayout.Width(20));
			EditorGUILayout.MinMaxSlider(ref greenThresholdMin,ref greenThresholdMax,minHeight,maxHeight);
			GUILayout.EndHorizontal();

			GUI.enabled = false;
			GUILayout.BeginHorizontal();
			GUILayout.Label("B",GUILayout.Width(20));
			EditorGUILayout.MinMaxSlider(ref blueThresholdMin,ref blueThresholdMax,minHeight,maxHeight);
			GUILayout.EndHorizontal();
			blueThresholdMin=greenThresholdMax;
			blueThresholdMax = maxHeight;
			GUI.enabled = true;

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			//float val = (greenThresholdMax-greenThresholdMin);
			if (lowestPoint!=null)
			{
				float val = Mathf.Lerp((float)lowestPoint,(float)highestPoint,greenThresholdMin/100f);
				GUILayout.Label(((int)val).ToString("0"));
				GUILayout.FlexibleSpace();
				val = Mathf.Lerp((float)lowestPoint,(float)highestPoint,greenThresholdMax/100f);
				GUILayout.Label(((int)val).ToString("0"));
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Generate Splatmap", GUILayout.Width (200), GUILayout.Height (32)))
			{
				GenerateSplatmap();
			}
			
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
		}


		void GenerateSplatmap()
		{
			if (!ValidateTerrain()) return;

			// TODO: read heightdata
			var terrainData = myTerrain.terrainData;
			var width = terrainData.heightmapResolution;
			var length = terrainData.heightmapResolution;
//			var height = terrainData.size.y;

			var newSplatmap = new Texture2D(width,length,TextureFormat.RGB24,false);
			var pixels = newSplatmap.GetPixels();

			Vector2 terrainBounds = GetTerrainHeightRange();

			lowestPoint = terrainBounds.x;
			highestPoint = terrainBounds.y;

			float worldHeight = 0;
			float normalizedHeight=0;
			float r=0,g=0,b=0;

			for (int x = 0; x < width; x++) 
			{
				for (int y = 0; y < length; y++) 
				{
					worldHeight = terrainData.GetHeight(y,x);

					normalizedHeight = Remap(worldHeight,lowestPoint,highestPoint,0,100);

					if (normalizedHeight<=redThresholdMax)
					{
						r=1;
					}else{
						r=0;
					}
			
					if (normalizedHeight>=greenThresholdMin && normalizedHeight<=greenThresholdMax)
					{
						g=1;
					}else{
						g=0;
					}

					if (normalizedHeight>=blueThresholdMin)
					{
						b=1;

					}else{
						b=0;
					}

					pixels[x*width+y] = new Color(r,g,b,0);
				}
			}

			newSplatmap.SetPixels(pixels);
			newSplatmap.Apply(false);

			byte[] texBytes  = newSplatmap.EncodeToPNG();

			string basename = Path.GetFileNameWithoutExtension(terrainData.name);
			string fullPath = AssetDatabase.GetAssetPath(terrainData);
			string assetDirectory = Path.GetDirectoryName(fullPath);

			fullPath = Path.Combine(assetDirectory, basename);

			string savePath = Path.GetDirectoryName(fullPath) + "/" + basename +"_splatmap.png";

			File.WriteAllBytes(savePath, texBytes);
			AssetDatabase.Refresh();
			
			Debug.Log("New splatmap saved to: "+savePath);

			DestroyImmediate(newSplatmap);
		}

		// checks if terrain is assigned and terrainData is accessible
		bool ValidateTerrain()
		{
			if (myTerrain==null) 
			{
				Debug.LogError("No terrain assigned");
				return false;
			}

			if (myTerrain.terrainData==null)
			{
				Debug.LogError("Unable to get terrainData from "+myTerrain.name);
				return false;
			}

			return true;
		}


		// HELPERS

		float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
		{
			return targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom);
		}

		float Remap(float source, float? sourceFrom, float? sourceTo, float targetFrom, float targetTo)
		{
			return (float)(targetFrom + (source-sourceFrom)*(targetTo-targetFrom)/(sourceTo-sourceFrom));
		}

		// returns min and max height
		Vector2 GetTerrainHeightRange()
		{
			var terrainData = myTerrain.terrainData;
			var width = terrainData.heightmapResolution;
			var length = terrainData.heightmapResolution;

			float minY = Mathf.Infinity;
			float maxY = Mathf.NegativeInfinity;

			for (int x = 0; x < width; x++) 
			{
				for (int y = 0; y < length; y++) 
				{
					float worldHeight = terrainData.GetHeight(y,x);

					minY = Mathf.Min(worldHeight,minY);
					maxY = Mathf.Max(worldHeight,maxY);
				}
			}
			return new Vector2(minY,maxY);
		}


	}
}