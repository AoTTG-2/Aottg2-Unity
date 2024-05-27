using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

/*
	Tom's Terrain Tools for Unity 3D
	Incremental Splatmapping
	version 1.0 - February 2015
	
	(C)2015 by Tom Vogt <tom@lemuria.org>
	
	http://lemuria.org/Unity/TTT/

*/

public class IncrementalSplatmapping : EditorWindow {

	public Terrain MyTerrain;

	public Texture2D Splatmap;
	public Texture2D TextureRed;
	public Texture2D TextureGreen;
	public Texture2D TextureBlue;
	public int TileSizeRed = 40;
	public int TileSizeGreen = 40;
	public int TileSizeBlue = 40;

	public float bias = 1.0f;
	public bool Replacing = false;

	// internal variables
	Vector2 scrollPosition = new Vector2(0,0);


	[MenuItem("Window/Terrain Tools/Incremental Splatmapping",false,100)]

	static void Init() {
		IncrementalSplatmapping window = (IncrementalSplatmapping)EditorWindow.GetWindow(typeof(IncrementalSplatmapping));
		window.minSize = new Vector2(400,550);
		window.Show();
	}

	void OnInspectorUpdate() {
		Repaint();
	}

	void OnGUI() {
		EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

		GUILayout.Label("Incremental Splatmapping",EditorStyles.boldLabel);
		EditorGUILayout.Separator();

		// bold titles
		GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		myFoldoutStyle.fontStyle = FontStyle.Bold;

		EditorGUILayout.Separator();
		GUILayout.BeginHorizontal();
			GUILayout.Label("Terrain");
			MyTerrain = (Terrain)EditorGUILayout.ObjectField("", MyTerrain, typeof(Terrain),true);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Splatmap");
			GUILayout.FlexibleSpace();
			Splatmap = (Texture2D)EditorGUILayout.ObjectField("", Splatmap, typeof(Texture2D),false);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Texture Red");
			GUILayout.FlexibleSpace();
			TextureRed = (Texture2D)EditorGUILayout.ObjectField("", TextureRed, typeof(Texture2D),false);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			GUILayout.Label("Tile Size Red");
			GUILayout.FlexibleSpace();
			TileSizeRed = EditorGUILayout.IntSlider(TileSizeRed, 4, 512);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Texture Green");
			GUILayout.FlexibleSpace();
			TextureGreen = (Texture2D)EditorGUILayout.ObjectField("", TextureGreen, typeof(Texture2D),false);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			GUILayout.Label("Tile Size Green");
			GUILayout.FlexibleSpace();
			TileSizeGreen = EditorGUILayout.IntSlider(TileSizeGreen, 4, 512);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Texture Blue");
			GUILayout.FlexibleSpace();
			TextureBlue = (Texture2D)EditorGUILayout.ObjectField("", TextureBlue, typeof(Texture2D),false);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
			GUILayout.Label("Tile Size Blue");
			GUILayout.FlexibleSpace();
			TileSizeBlue = EditorGUILayout.IntSlider(TileSizeBlue, 4, 512);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			GUILayout.Label("Strength of this layer");
			GUILayout.FlexibleSpace();
			bias = EditorGUILayout.Slider(bias, 0.1f, 10.0f);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			GUILayout.Label("replace old textures");
			GUILayout.FlexibleSpace();
			Replacing = EditorGUILayout.Toggle ("", Replacing);
		GUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Add Splat Texture",GUILayout.Width(200), GUILayout.Height(32))) {
				if (CheckSplatmap()) {
					AddSplatTexture();
				}
			}
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
			
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
	
	
	void FixFormat(Texture2D texture) {
		string path = AssetDatabase.GetAssetPath(texture);
		TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
		if (texture.format != TextureFormat.RGB24 || !textureImporter.isReadable) 
		{
			textureImporter.mipmapEnabled = false;
			textureImporter.isReadable = true;
			textureImporter.textureFormat = TextureImporterFormat.RGB24;
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
			Debug.Log("fixed texture format for "+path);
		}
	}
	
	public string Reverse(string text) {
	   if (text == null) return null;

	   // this was posted by petebob as well 
	   char[] array = text.ToCharArray();
	   System.Array.Reverse(array);
      return new string(array);
	}
	public string FindFile(string basename) {
		string[] extensions = {"tif", "tiff", "png", "jpg", "jpeg"};
		foreach (string ext in extensions) {
			string filename = basename + "." + ext;
			if (File.Exists(filename)) {
				return filename;
			}
		}
		return "";
	}
	

	bool CheckSplatmap() {
		if (Splatmap==null) return false;
		FixFormat(Splatmap);

		if (Splatmap.height != Splatmap.width) {
			EditorUtility.DisplayDialog("Wrong size", "Splatmap must be square (width and height must be the same)", "Cancel"); 
			return false;
		}
		if (Mathf.ClosestPowerOfTwo(Splatmap.width) != Splatmap.width) {
			EditorUtility.DisplayDialog("Wrong size", "Splatmap width and height must be a power of two", "Cancel"); 
			return false;	
		}

		return true;
	}
	
	void AddSplatTexture() 
	{
		if (!MyTerrain) MyTerrain = Terrain.activeTerrain;
		TerrainData terrain = MyTerrain.terrainData;

		int splaMaptWidth = Splatmap.width;
		if (splaMaptWidth!=terrain.alphamapWidth)
		{
			Debug.LogError("New splatmap is not same resolution as the current terrain splatmap ("+splaMaptWidth+" != "+terrain.alphamapWidth+")");
			return;
		}

		Undo.RecordObject(terrain, "Add Splattexture");
		Color[] SplatmapColors = Splatmap.GetPixels();
		int layer = MyTerrain.terrainData.alphamapLayers;

		int add_layers = 0;
		int red_layer = layer;
		int green_layer = layer+1;
		int blue_layer = layer+2;

		if (TextureRed) {
			AddTexture(TextureRed, TileSizeRed);
			add_layers++;
		}
		if (TextureGreen) {
			AddTexture(TextureGreen, TileSizeGreen);
			green_layer = layer + add_layers;
			add_layers++;
		}
		if (TextureBlue) {
			AddTexture(TextureBlue, TileSizeBlue);
			blue_layer = layer + add_layers;
			add_layers++;
		}
		float[,,] splatmapData = terrain.GetAlphamaps(0, 0, splaMaptWidth, splaMaptWidth);

		for (int y = 0; y < splaMaptWidth; y++) {
			if (y%10 == 0) {
				EditorUtility.DisplayProgressBar("add splat", "updating terrain", Mathf.InverseLerp(0.0f, (float)splaMaptWidth, (float)y));
			}
			for (int x = 0; x < splaMaptWidth; x++) {
				float red = SplatmapColors[x*splaMaptWidth + y].r * bias;
				float green = SplatmapColors[x*splaMaptWidth + y].g * bias;
				float blue = SplatmapColors[x*splaMaptWidth + y].b * bias;
				float value = 0.0f;

				if (TextureRed) {
					if (red>0.0f) {
						splatmapData[x,y,red_layer] = red;
						value+=red;
					} else {
						splatmapData[x,y,red_layer] = 0.0f;
					}
				}

				if (TextureGreen) {
					if (green>0.0f) {
						splatmapData[x,y,green_layer] = green;
						value+=green;
					} else {
						splatmapData[x,y,green_layer] = 0.0f;
					}
				}

				if (TextureBlue) {
					if (blue>0.0f) {
						splatmapData[x,y,blue_layer] = blue;
						value+=blue;
					} else {
						splatmapData[x,y,blue_layer] = 0.0f;
					}
				}

				// normalize
				float total = 0.0f;
				for (int l = 0; l < layer+add_layers; l++) {
					if (Replacing && value >= 1.0f && l < layer) {
						splatmapData[x,y,l] = 0;
					}
					total += splatmapData[x,y,l];
				}
				if (total != 1.0f) {
					for (int l = 0; l < layer+add_layers; l++) {
						splatmapData[x,y,l] *= 1.0f/total;
					}
				}
			}
		}

		terrain.SetAlphamaps(0, 0, splatmapData);
		EditorUtility.SetDirty(MyTerrain);
		Debug.Log("Splatmap texture applied");

		EditorUtility.ClearProgressBar();
	}

	void AddTexture(Texture2D Texture, int TileSize) 
	{
		SplatPrototype[] oldPrototypes = MyTerrain.terrainData.splatPrototypes;
		SplatPrototype[] newPrototypes = new SplatPrototype[oldPrototypes.Length + 1];
		for (int x=0;x<oldPrototypes.Length;x++) {
			newPrototypes[x] = oldPrototypes[x];
		}
		newPrototypes[oldPrototypes.Length] = new SplatPrototype();
		newPrototypes[oldPrototypes.Length].texture = Texture;
		Vector2 vector = new Vector2(TileSize, TileSize);
		newPrototypes[oldPrototypes.Length].tileSize = vector;
		MyTerrain.terrainData.splatPrototypes = newPrototypes;
	}

}

