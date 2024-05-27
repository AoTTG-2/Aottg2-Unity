/*
	Tom's Terrain Tools for Unity 3D 
	Version 2.6 (2015)
	(C)2015 by Tom Vogt <tom@lemuria.org> & Unitycoder.com <support@unitycoder.com>
	http://lemuria.org/Unity/TTT/
	http://unitycoder.com/blog/2014/08/14/asset-store-terrain-tools/
*/

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

namespace TTT
{
	public class TerrainTools : EditorWindow 
	{
		public Terrain myTerrain;

		private Object splatMap;
		private bool defaultsDone = false;
		private Texture2D[] splatTextures = new Texture2D[4];
		private int tileSizeX=8;
		private int tileSizeY=8;
		private Texture2D[] splatTexNormals = new Texture2D[4];
		private GameObject[] treeObjects = new GameObject[3];
		private Texture2D[] grassTextures = new Texture2D[3];
		private GameObject[] detailObjects = new GameObject[3]; // bushes, stones

		public Texture2D splatA;
		public Texture2D splatB;

		public Texture2D treemap;
		public bool resetTrees = true;
		public float treeDensity = 0.4f;
		public float treeThreshold = 0.1f;
		public float treeSize = 1f;
		public float sizeVariation = 0.2f;

		public Texture2D grassmap;
		public Texture2D bushmap;
		public float grassDensity = 0.15f;
		public float grassclumping = 0.5f;
		public float bushDensity = 0.02f;
		
		public Texture2D overlayMap;
		public float overlayThreshold = 0.1f;
		public Texture2D overlayTexture;
		public int tileSize = 15;
		public bool clearTrees = false;
		public float clearRadius = 1.0f;
		public bool clearGrass = true;
		public float changeTerrain = 0.0f;

		// internal variables
		GUIStyle boldTitleStyle;
		Vector2 scrollPosition = new Vector2(0,0);
		bool toggleSplat = true;
		bool toggleTrees = true;
		bool toggleGrass = true;
		bool toggleOverlay=false;
		bool toggleAutoMagic=false;
		bool toggleAdvanced=false;

		// default terrain settings
		int terrainInitWidth=2000;
		int terrainInitLength=2000;
		int terrainInitHeight=200;
		float windSettingsSpeed = 0.2f;
		float windSettingsSize = 0.2f;
		float windSettingsBending = 0.2f;

		bool assignNormalMapsIfFounded=true; // checks if texture has normals map in same folder: texturename.png & texturename_normal.png

		string[] texturePackOptionTitles = new string[] {"Default", "Plain colors#1", "Debug"};
		int selectedTexturePack = 0; // which texture pack is selected (file names are hardcoded at AutoMagicDefaults()

		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch(); // timer for timing method durations


		bool timeStampFileNames=false; // if enabled, every time you generate, new terrain is saved with a new name (as new terrain)

		[MenuItem("Window/Terrain Tools/Terrain Tools",false,0)]
		static void Init() 
		{
			TerrainTools window = (TerrainTools)EditorWindow.GetWindow(typeof(TerrainTools));
			window.titleContent = new GUIContent("TerrainTools");
			window.minSize = new Vector2(410,400);
			window.Show();
		}

		void OnInspectorUpdate() 
		{
			Repaint();
		}

		void OnGUI() 
		{
			EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

			DrawTitleGUI();

			boldTitleStyle = new GUIStyle(EditorStyles.foldout);
			boldTitleStyle.fontStyle = FontStyle.Bold;

			EditorGUILayout.Separator();
			GUILayout.BeginHorizontal();
				GUILayout.Label("Terrain");
				myTerrain = (Terrain)EditorGUILayout.ObjectField("", myTerrain, typeof(Terrain),true);
			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();

			DrawAutoMagicGUI();
			DrawTexturingGUI(); 
			DrawTreeDistributionGUI(); 
			DrawGrassGUI(); 
			DrawOverlayGUI(); 

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}


		static void DrawTitleGUI ()
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Terrain Tools v2.6", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Read Manual")) {
				Help.BrowseURL ("http://lemuria.org/Unity/TTT/");
			}
			GUILayout.EndHorizontal ();
			EditorGUILayout.Separator ();
		}


		void DrawAutoMagicGUI()
		{
			toggleAutoMagic = EditorGUILayout.Foldout (toggleAutoMagic, "AutoMagic", boldTitleStyle);
			if (toggleAutoMagic) 
			{
				if (!defaultsDone) AutoMagicDefaults ();
				GUILayout.BeginHorizontal ();
				splatMap = EditorGUILayout.ObjectField (new GUIContent ("Splatmap Object", "Heightmap, Splatmap, Treemap, Grassmap"), splatMap, typeof(Object), false);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Textures");
				GUILayout.FlexibleSpace();
				splatTextures[0] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [0], typeof(Texture2D), false, GUILayout.Height (48));
				splatTextures[1] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [1], typeof(Texture2D), false, GUILayout.Height (48));
				splatTextures[2] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [2], typeof(Texture2D), false, GUILayout.Height (48));
				splatTextures[3] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [3], typeof(Texture2D), false, GUILayout.Height (48));
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Trees");
				GUILayout.FlexibleSpace();
				treeObjects[0] = (GameObject)EditorGUILayout.ObjectField (treeObjects [0], typeof(GameObject), false);
				treeObjects[1] = (GameObject)EditorGUILayout.ObjectField (treeObjects [1], typeof(GameObject), false);
				treeObjects[2] = (GameObject)EditorGUILayout.ObjectField (treeObjects [2], typeof(GameObject), false);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel ("Grass");
				GUILayout.FlexibleSpace ();
				grassTextures[0] = (Texture2D)EditorGUILayout.ObjectField (grassTextures [0], typeof(Texture2D), false, GUILayout.Height (32));
				grassTextures[1] = (Texture2D)EditorGUILayout.ObjectField (grassTextures [1], typeof(Texture2D), false, GUILayout.Height (32));
				grassTextures[2] = (Texture2D)EditorGUILayout.ObjectField (grassTextures [2], typeof(Texture2D), false, GUILayout.Height (32));
				GUILayout.EndHorizontal();
				EditorGUILayout.Separator();

				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Detail meshes");
				GUILayout.FlexibleSpace();
				detailObjects[0] = (GameObject)EditorGUILayout.ObjectField (detailObjects[0], typeof(GameObject), false);
				detailObjects[1] = (GameObject)EditorGUILayout.ObjectField (detailObjects[1], typeof(GameObject), false);
				detailObjects[2] = (GameObject)EditorGUILayout.ObjectField (detailObjects[2], typeof(GameObject), false);
				GUILayout.EndHorizontal();
				EditorGUILayout.Separator();


				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();

				if (GUILayout.Button ("Run AutoMagic", GUILayout.Width (200), GUILayout.Height (32)))
				{
					AutoMagic();
				}

				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();


				toggleAdvanced = EditorGUILayout.Foldout(toggleAdvanced, "Advanced Settings", boldTitleStyle);
				if (toggleAdvanced) 
				{
					// texture pack selection
					GUI.changed = false;
					selectedTexturePack = EditorGUILayout.Popup("Texture pack",selectedTexturePack, texturePackOptionTitles);
					if (GUI.changed) 
					{
						AutoMagicDefaults(); // reload texture on change
					}else{
					}
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace ();
					if (GUILayout.Button ("Apply Textures to Terrain", GUILayout.Width (200), GUILayout.Height (22)))
					{
						ApplySelectedTexturesToTerrain();
					}
					GUILayout.EndHorizontal ();
					EditorGUILayout.Separator();

					// create timestamped filenames for terrain (unique terrains for each generation)
					timeStampFileNames = EditorGUILayout.Toggle("Unique terrain files",timeStampFileNames);
					EditorGUILayout.Separator();

					// terrain default settings
					terrainInitWidth = EditorGUILayout.IntSlider ("Terrain Width",terrainInitWidth,100,8000);
					terrainInitLength = EditorGUILayout.IntSlider ("Terrain Length",terrainInitLength,100,8000);
					terrainInitHeight = EditorGUILayout.IntSlider ("Terrain Height",terrainInitHeight,100,8000);
					tileSizeY = EditorGUILayout.IntField("Texture Tiling Y",(int)Mathf.Clamp(tileSizeY,1,9999));
					tileSizeX = EditorGUILayout.IntField("Texture Tiling X",(int)Mathf.Clamp(tileSizeX,1,9999));
					EditorGUILayout.Separator();

					// misc options
					assignNormalMapsIfFounded = EditorGUILayout.Toggle("Use normal maps if founded",assignNormalMapsIfFounded);


				}
			}
			GUILayout.FlexibleSpace ();
			EditorGUILayout.Separator ();
		}

		void DrawTexturingGUI ()
		{
			toggleSplat = EditorGUILayout.Foldout (toggleSplat, "Texturing", boldTitleStyle);
			if (toggleSplat) {
				GUILayout.BeginHorizontal ();
				EditorGUILayout.PrefixLabel ("First Splatmap");
				GUILayout.FlexibleSpace ();
				splatA = (Texture2D)EditorGUILayout.ObjectField ("", splatA, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				EditorGUILayout.Separator ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Second Splatmap (optional)");
				GUILayout.FlexibleSpace ();
				splatB = (Texture2D)EditorGUILayout.ObjectField ("", splatB, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Apply Splatmap(s)", GUILayout.Width (200), GUILayout.Height (32))) {
					if (CheckSplatmap())
					{
						ApplySplatmap();
					}else{
						Debug.LogError("No first splatmap assigned");
					}
				}
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.Separator ();
		}

		void DrawTreeDistributionGUI ()
		{
			toggleTrees = EditorGUILayout.Foldout (toggleTrees, "Tree Distribution", boldTitleStyle);
			if (toggleTrees) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Tree map");
				GUILayout.FlexibleSpace ();
				treemap = (Texture2D)EditorGUILayout.ObjectField ("", treemap, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Clear Existing Trees?");
				GUILayout.FlexibleSpace ();
				resetTrees = EditorGUILayout.Toggle ("remove trees", resetTrees);
				GUILayout.EndHorizontal ();
				EditorGUILayout.Separator ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Tree Density");
				GUILayout.FlexibleSpace ();
				treeDensity = EditorGUILayout.Slider (treeDensity, 0.05f, 3f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Threshold");
				GUILayout.FlexibleSpace ();
				treeThreshold = EditorGUILayout.Slider (treeThreshold, 0.01f, 0.99f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Tree Size");
				GUILayout.FlexibleSpace ();
				treeSize = EditorGUILayout.Slider (treeSize, 0.2f, 5f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Size Variation");
				GUILayout.FlexibleSpace ();
				sizeVariation = EditorGUILayout.Slider (sizeVariation, 0f, 1f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Generate Trees", GUILayout.Width (200), GUILayout.Height (32))) 
				{
					if (CheckTreemap())	ApplyTreemap();
				}
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.Separator ();
		}

		void DrawGrassGUI ()
		{
			toggleGrass = EditorGUILayout.Foldout (toggleGrass, "Grass and Details", boldTitleStyle);
			if (toggleGrass) {
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Grass map");
				GUILayout.FlexibleSpace ();
				grassmap = (Texture2D)EditorGUILayout.ObjectField ("", grassmap, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				EditorGUILayout.Separator ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Bush/Detail map");
				GUILayout.FlexibleSpace ();
				bushmap = (Texture2D)EditorGUILayout.ObjectField ("", bushmap, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Grass Density");
				GUILayout.FlexibleSpace ();
				grassDensity = EditorGUILayout.Slider (grassDensity, 0.01f, 3f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Grass Clumping");
				GUILayout.FlexibleSpace ();
				grassclumping = EditorGUILayout.Slider (grassclumping, 0f, 1f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Bush/Detail Density");
				GUILayout.FlexibleSpace ();
				bushDensity = EditorGUILayout.Slider (bushDensity, 0.01f, 2f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Generate Grass and Details", GUILayout.Width (200), GUILayout.Height (32))) 
				{
					if (CheckGrassmap ()) ApplyGrassmap();
				}
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
			}
			EditorGUILayout.Separator ();
		}

		void DrawOverlayGUI ()
		{
			toggleOverlay = EditorGUILayout.Foldout (toggleOverlay, "Overlays (roads, rivers, etc)", boldTitleStyle);
			if (toggleOverlay) 
			{
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Overlay map");
				GUILayout.FlexibleSpace ();
				overlayMap = (Texture2D)EditorGUILayout.ObjectField ("", overlayMap, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Threshold");
				GUILayout.FlexibleSpace ();
				overlayThreshold = EditorGUILayout.Slider (overlayThreshold, 0.1f, 0.9f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Overlay texture");
				GUILayout.FlexibleSpace ();
				overlayTexture = (Texture2D)EditorGUILayout.ObjectField ("", overlayTexture, typeof(Texture2D), false);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Tile size");
				GUILayout.FlexibleSpace ();
				tileSize = EditorGUILayout.IntSlider (tileSize, 3, 127);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Clear trees");
				GUILayout.FlexibleSpace ();
				clearTrees = EditorGUILayout.Toggle ("", clearTrees);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Clear tree radius");
				GUILayout.FlexibleSpace ();
				clearRadius = EditorGUILayout.Slider (clearRadius, 0.5f, 10.0f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Clear grass/detail");
				GUILayout.FlexibleSpace ();
				clearGrass = EditorGUILayout.Toggle ("", clearGrass);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Raise/lower terrain");
				GUILayout.FlexibleSpace ();
				changeTerrain = EditorGUILayout.Slider (changeTerrain, -50f, 50f);
				GUILayout.EndHorizontal ();
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Generate Overlay", GUILayout.Width (200), GUILayout.Height (32))) 
				{
					if (CheckOverlaymap()) ApplyOverlaymap ();
				}
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
			}
		}
		
		public void FixTextureSettings(Texture2D texture) 
		{
			if (texture==null) {Debug.LogError("FixFormat failed - Texture is null"); return;}

			string path = AssetDatabase.GetAssetPath(texture);

			if (string.IsNullOrEmpty(path)) {Debug.LogError("FixFormat failed - Texture path is null"); return;}

			
			TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

			if (!textureImporter.isReadable) 
			{
				Debug.Log("File:"+path+" needs fixing: wrong texture format or not marked as read/write allowed");
				textureImporter.mipmapEnabled = false;
				textureImporter.isReadable = true;
				AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
			}

		}
		
		public static string Reverse(string text) 
		{
		   if (text == null) return null;

		   // this was posted by petebob as well 
		   char[] array = text.ToCharArray();
		   System.Array.Reverse(array);
	      return new string(array);
		}

		public string FindFile(string basename) 
		{
			string[] extensions = {"tif", "tiff", "png", "jpg", "jpeg","TIF", "TIFF", "PNG", "JPG", "JPEG"};
			foreach (string ext in extensions) 
			{
				string filename = basename + "." + ext;

//				Debug.Log(filename);
				if (File.Exists(filename)) 
				{
					return filename;
				}
			}
			return null;
			
		}
		
		void AutoMagic() 
		{
			if (splatMap == null) {Debug.LogError("No splatmap assigned"); return;}
			if (!ValidateAutomagicTextures()) {Debug.LogError("Not all textures are assigned"); return;}
			if (!ValidateAutomagicTrees()) {Debug.LogError("Not all trees are assigned"); return;}
			// TODO: validate grass and bushes?

			string origPath = AssetDatabase.GetAssetPath(splatMap);
			string pathrev = Reverse(Path.GetFileNameWithoutExtension(origPath));
			string[] parts = pathrev.Split(new char[] { '-' }, 2);

			if (parts==null || parts.Length<2) {Debug.LogError("Filenames must start with \"number\" and \"-\" character. Example: 1-splatmap.tif"); return;}

			string basename = Reverse(parts[1]);
			string dirname = Path.GetDirectoryName(origPath);
			string fixedPath = Path.Combine(dirname, basename);

			string splatmap_name = FindFile(fixedPath+"-splatmap");

			if (!File.Exists(splatmap_name)) {Debug.LogError("Splatmap file not found:"+splatmap_name+" \n Make sure that your filenames start with \"number\" and \"-\" character. Example: 1-splatmap.tif"); return;}
			splatA = AssetDatabase.LoadAssetAtPath(splatmap_name, typeof(Texture2D)) as Texture2D;
			CheckSplatmap();

			string treemap_name = FindFile(fixedPath+"-treemap");
			if (File.Exists(treemap_name)) 
			{
				//Debug.LogError("Treemap file not found:"+treemap_name+" \n Make sure that your filenames start with \"number\" and \"-\" character. Example: 1-splatmap.tif"); return;}
				treemap = AssetDatabase.LoadAssetAtPath(treemap_name, typeof(Texture2D)) as Texture2D;
				CheckTreemap();
			}else{
				treemap_name=null;
			}

			string grassmap_name = FindFile(fixedPath+"-grassmap");
			if (File.Exists(grassmap_name)) {
				//Debug.LogError("Grassmap file not found:"+grassmap_name+" \n Make sure that your filenames start with \"number\" and \"-\" character. Example: 1-splatmap.tif"); return;}
				grassmap = AssetDatabase.LoadAssetAtPath(grassmap_name, typeof(Texture2D)) as Texture2D;
				CheckGrassmap();
			}else{
				grassmap_name=null;
			}

			string bushmap_name = FindFile(fixedPath+"-bushmap");
			if (File.Exists(bushmap_name)) {
				//Debug.LogError("Grassmap file not found:"+grassmap_name+" \n Make sure that your filenames start with \"number\" and \"-\" character. Example: 1-splatmap.tif"); return;}
				bushmap = AssetDatabase.LoadAssetAtPath(bushmap_name, typeof(Texture2D)) as Texture2D;
				CheckGrassmap();
			}else{
				grassmap_name=null;
			}


			CreateTerrain(fixedPath, splatA.width);

			if (myTerrain.terrainData==null) {Debug.LogError("Terrain generation failed.."); return;};

			string heightmap_name = fixedPath+"-heightmap.raw";
			myTerrain.terrainData.SetHeights(0, 0, ReadRawHeightMap(heightmap_name, splatA.width+1,myTerrain.terrainData.heightmapResolution,myTerrain.terrainData.heightmapResolution));

			ApplySelectedTexturesToTerrain();
			ApplySplatmap();

			if (treemap_name!=null)
			{
				myTerrain.terrainData.treePrototypes = SetupTrees();
				myTerrain.terrainData.RefreshPrototypes();
				ApplyTreemap();
			}

			if (grassmap_name!=null)
			{
				myTerrain.terrainData.detailPrototypes = SetupGrassAndDetails();
				myTerrain.terrainData.RefreshPrototypes();
				ApplyGrassmap();
			}
			/*
			if (bushmap_name!=null)
			{
				myTerrain.terrainData.detailPrototypes = SetupDetailMeshes();
				myTerrain.terrainData.RefreshPrototypes();
				ApplyBushmap();
			}*/

		}
		
		void AutoMagicDefaults() 
		{
			// TODO: add normals also, for now just clear them
			for (int i = 0; i < splatTexNormals.Length; i++) {
				splatTexNormals[i]=null;
			}



			string tempFilePath = "";
			string tempFileName = "";
			switch (selectedTexturePack)
			{
			// Default texture set #1
			case 0:
				tempFilePath = "Assets/Terrain Assets/Textures/";
				tempFileName = "Grass (Meadows).jpg";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[0] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "Grass (Hill).jpg";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[1] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "Grass&Rock.jpg";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[2] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "Grass (Muddy).jpg";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[3] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				break;

			// Default texture set #2
			case 1:
				tempFilePath = "Assets/Terrain Assets/Textures/SingleColor/";
				tempFileName = "green_color_tile-1.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[0] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "brown_color_tile-2.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[1] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "lite_green_color_tile-3.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[2] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "lite_green2_color_tile-4.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[3] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				break;

			// Default texture set #3
			case 2:
				tempFilePath = "Assets/Terrain Assets/Textures/Debug/";
				tempFileName = "debug_red.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[0] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "debug_green.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[1] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "debug_blue.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[2] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				tempFileName = "debug_white.png";
				if (File.Exists(tempFilePath+tempFileName)) splatTextures[3] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));
				break;

			default:
				Debug.LogError("Invalid selectedTexturePack value : "+selectedTexturePack);
				break;
			}

			// default trees
			tempFilePath = "Assets/Terrain Assets/Trees Ambient-Occlusion/";
			tempFileName = "Alder.fbx";
			if (File.Exists(tempFilePath+tempFileName)) treeObjects[0] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));

			tempFileName = "Sycamore.fbx";
			if (File.Exists(tempFilePath+tempFileName)) treeObjects[1] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));

			tempFileName = "Mimosa.fbx";
			if (File.Exists(tempFilePath+tempFileName)) treeObjects[2] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));

			// default grass
			tempFilePath = "Assets/Standard Assets/Environment/TerrainAssets/BillboardTextures/";
			tempFileName = "GrassFrond01AlbedoAlpha.psd";
			if (File.Exists(tempFilePath+tempFileName)) grassTextures[0] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));

			tempFilePath = "Assets/Terrain Assets/Grass/";
			tempFileName = "Fern.psd";
			if (File.Exists(tempFilePath+tempFileName)) grassTextures[1] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));

			tempFilePath = "Assets/Standard Assets/Environment/TerrainAssets/BillboardTextures/";
			tempFileName = "GrassFrond02AlbedoAlpha.psd";
			if (File.Exists(tempFilePath+tempFileName)) grassTextures[2] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(Texture2D));


			// default detail meshes
			tempFilePath = "Assets/Terrain Assets/Bushes/";
			tempFileName = "Bush5.fbx";
			if (File.Exists(tempFilePath+tempFileName)) 
			{
				detailObjects[0] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));
			}else{
				Debug.LogWarning("Default object missing : "+tempFilePath+tempFileName);
			}
			tempFileName = "Bush7.fbx";
			if (File.Exists(tempFilePath+tempFileName)) {
				detailObjects[1] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));
			}else{
				Debug.LogWarning("Default object missing : "+tempFilePath+tempFileName);
			}
			tempFilePath = "Assets/Terrain Assets/Rocks/";
			tempFileName = "RockMesh.fbx";
			if (File.Exists(tempFilePath+tempFileName)) {
					detailObjects[2] = (GameObject)AssetDatabase.LoadAssetAtPath(tempFilePath+tempFileName, typeof(GameObject));
			}else{
				Debug.LogWarning("Default object missing : "+tempFilePath+tempFileName);
			}

			defaultsDone = true;
		}

		bool ValidateAutomagicTextures()
		{
			if (splatTextures[0]==null || splatTextures[1]==null || splatTextures[2]==null || splatTextures[3]==null) return false;
			return true;
		}

		bool ValidateAutomagicTrees()
		{
			if (treeObjects[0]==null || treeObjects[1]==null || treeObjects[2]==null) return false;
			return true;
		}


		SplatPrototype[] SetupTextures() 
		{
			SetupTextureNormals();

			//Vector2 tilesize = new Vector2(40f, 40f);

			SplatPrototype[] Splat = new SplatPrototype[4];

			for (int i = 0; i < 4; i++) 
			{
				Splat[i] = new SplatPrototype();
				Splat[i].texture = splatTextures[i];
				if (assignNormalMapsIfFounded) Splat[i].normalMap = splatTexNormals[i];
				Splat[i].tileSize = new Vector2(tileSizeX,tileSizeY);
			}

			return Splat;
		}

		void SetupTextureNormals()
		{
			for (int i = 0; i < 4; i++) 
			{
				string tempFileName = AssetDatabase.GetAssetPath(splatTextures[i]);
				string tempFileWithoutExtension = Path.GetFileNameWithoutExtension(tempFileName);
				if (assignNormalMapsIfFounded)
				{
					string tempNormalMapFile = Path.GetDirectoryName(tempFileName)+Path.DirectorySeparatorChar+tempFileWithoutExtension+"_normal"+Path.GetExtension(tempFileName);
					if (File.Exists(tempNormalMapFile)) splatTexNormals[i] = (Texture2D)AssetDatabase.LoadAssetAtPath(tempNormalMapFile, typeof(Texture2D));
				}
			}
		}

		TreePrototype[] SetupTrees() {
			TreePrototype[] trees = new TreePrototype[3];
			trees[0] = new TreePrototype();
			trees[0].prefab = treeObjects[0];
			trees[1] = new TreePrototype();
			trees[1].prefab = treeObjects[1];
			trees[2] = new TreePrototype();
			trees[2].prefab = treeObjects[2];
			return trees;
		}
		
		DetailPrototype[] SetupGrassAndDetails() 
		{
			// TODO: only take used amount from automagic, skip nulls
			DetailPrototype[] details = new DetailPrototype[6];


			// grass
			details[0] = new DetailPrototype();
			if (grassTextures[0]!=null)
			{
				details[0].prototypeTexture = grassTextures[0];
				details[0].renderMode = DetailRenderMode.Grass;
				details[0].usePrototypeMesh=false;
				details[0].minHeight = 0.55f;
				details[0].maxHeight = 0.77f;
				details[0].healthyColor = Color.white;
				details[0].dryColor = Color.gray;

			}
			details[1] = new DetailPrototype();
			if (grassTextures[1]!=null)
			{
				details[1].prototypeTexture = grassTextures[1];
				details[1].renderMode = DetailRenderMode.Grass;
				details[1].usePrototypeMesh=false;
				details[1].minHeight = 0.66f;
				details[1].maxHeight = 0.88f;
				details[1].healthyColor = Color.white;
				details[1].dryColor = Color.gray;
			}
			details[2] = new DetailPrototype();
			if (grassTextures[2]!=null)
			{
				details[2].prototypeTexture = grassTextures[2];
				details[2].renderMode = DetailRenderMode.Grass;
				details[2].usePrototypeMesh=false;
				details[2].minHeight = 0.77f;
				details[2].maxHeight = 0.99f;
				details[2].healthyColor = Color.white;
				details[2].dryColor = Color.gray;
			}


			// meshes
			details[3] = new DetailPrototype();
			if (detailObjects[0]!=null)
			{
				details[3].prototype = detailObjects[0];
				details[3].renderMode = DetailRenderMode.Grass; // for default grass
				details[3].usePrototypeMesh=true; 
				details[3].healthyColor = Color.white;
				details[3].dryColor = Color.gray;
			}

			details[4] = new DetailPrototype();
			if (detailObjects[1]!=null)
			{
				details[4].prototype = detailObjects[1];
				details[4].renderMode = DetailRenderMode.Grass; // for default grass
				details[4].usePrototypeMesh=true;
				details[4].healthyColor = Color.white;
				details[4].dryColor = Color.gray;
			}

			details[5] = new DetailPrototype();
			if (detailObjects[2]!=null)
			{
				details[5].prototype = detailObjects[2];
				details[5].renderMode = DetailRenderMode.VertexLit; // for default mesh rock
				details[5].usePrototypeMesh=true;  
				details[5].healthyColor = Color.white;
				details[5].dryColor = Color.gray;
			}

			return details;
		}

		/*
		DetailPrototype[] SetupDetailMeshes() 
		{
			// if we already have some details keep them, for now just skip default 3 grass details
			DetailPrototype[] detailMeshes = new DetailPrototype[3];

			detailMeshes[0] = new DetailPrototype();
			detailMeshes[0].prototype = detailObjects[0];
			detailMeshes[1] = new DetailPrototype();
			if (detailMeshes[1]!=null)
			{
				detailMeshes[1].prototype = detailObjects[1];
			}
			detailMeshes[2] = new DetailPrototype();
			if (detailMeshes[2]!=null)
			{
				detailMeshes[2].prototype = detailObjects[2];
			}
			return detailMeshes;
		}*/

		
		bool CheckSplatmap() 
		{
			if (splatA==null) return false;
			FixTextureSettings(splatA);

			if (splatA.height != splatA.width) 
			{
				EditorUtility.DisplayDialog("Wrong size", "First splatmap must be square (width and height must be the same)", "Cancel"); 
				return false;
			}

			if (Mathf.ClosestPowerOfTwo(splatA.width) != splatA.width) {
				EditorUtility.DisplayDialog("Wrong size", "Splatmap width and height must be a power of two", "Cancel"); 
				return false;	
			}

			if (splatB!=null) 
			{
				FixTextureSettings(splatB);
				if (splatB.height != splatB.width) 
				{
					EditorUtility.DisplayDialog("Wrong size", "Second splatmap must be square (width and height must be the same)", "Cancel"); 
					return false;
				}

				if (Mathf.ClosestPowerOfTwo(splatB.width) != splatB.width) 
				{
					EditorUtility.DisplayDialog("Wrong size", "Second splatmap width and height must be a power of two", "Cancel"); 
					return false;	
				}
			}

			return true;
		}

		void ApplySelectedTexturesToTerrain()
		{
			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			if (myTerrain==null) {Debug.LogError("No terrain selected"); return;}
			myTerrain.terrainData.splatPrototypes = SetupTextures();
		}


		void ApplySplatmap() 
		{
			StartTimer();

			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			if (myTerrain==null) {Debug.LogError("No terrain selected"); return;}

			TerrainData terrainData = myTerrain.terrainData;

			if (terrainData==null) {Debug.LogError("Failed getting terrain data"); return;}

			if (terrainData.alphamapLayers<4) 
			{
				EditorUtility.DisplayDialog("Missing Terrain Textures", "Please set up at least 4 textures in the terrain painter dialog", "Cancel");
				return;	
			}

			if (splatB!=null && terrainData.alphamapLayers<8) 
			{
				EditorUtility.DisplayDialog("Missing Terrain Textures", "Please set up at least 8 textures in the terrain painter dialog", "Cancel");
				return;	
			}

			int width = splatA.width;
			bool TwoMaps = false;

			if (splatB==null) 
			{
				//splatB = new Texture2D(w, w, TextureFormat.ARGB32, false);
			} else {
				if (splatA.width != splatB.width && splatA.height != splatB.height) {
					Debug.LogError("Both splatmaps must have same resolution ("+splatA.width+" != "+splatB.width+")");
					return;
				}
				TwoMaps=true;
			}

			
			Undo.RecordObject(terrainData, "Apply splatmap(s)"); // very slow..

			terrainData.alphamapResolution = width;

			float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, width, width);
			Color[] splatmapColors = splatA.GetPixels();
			Color[] splatmapColors_b=null; //= splatB.GetPixels();

			if (TwoMaps) 
			{
				splatmapColors_b = splatB.GetPixels();
			}else{
				//	DestroyImmediate(splatB);
				//splatB = null;
			}

			Color col=Color.clear;
			Color col_b=Color.clear;

			for (int y = 0; y < width; y++)
			{

				if (y%10 == 0) // update progress bar every now and then
				{
					if (EditorUtility.DisplayCancelableProgressBar("Applying splatmap", "calculating...", Mathf.InverseLerp(0.0f, (float)width, (float)y))) 
					{
						EditorUtility.ClearProgressBar();
						return;
					}
				}

				for (int x = 0; x < width; x++)
				{
					float sum;

//					Color col = splatmapColors[((w-1)-x)*w + y];
//					Color col_b = splatmapColors_b[((w-1)-x)*w + y];
					//col = splatmapColors[ ((w-1)-y)*w + x]; // TODO: get rid of flip
					col = splatmapColors[x*width+y];


					if (TwoMaps) 
					{
//						col_b = splatmapColors_b[((w-1)-y)*w + x];
						col_b = splatmapColors_b[y*width+x];
						sum = col.r+col.g+col.b+col_b.r+col_b.g+col_b.b;
					} else {
						sum = col.r+col.g+col.b;
						//sum = col.r+col.g+col.b+(splatHasAlpha?col.a:0);
					}

					if (sum>1.0f) 
					{
						// no final channel, and scale down
						splatmapData[x,y,0] = col.r / sum;
						splatmapData[x,y,1] = col.g / sum;
						splatmapData[x,y,2] = col.b / sum;
						//splatmapData[x,y,3] = col.a / sum;
						splatmapData[x,y,3] = 1f-sum;

	//					splatmapData[y,x,0] = col.r / sum; // TODO: these parts will be later used for invert options
	//					splatmapData[y,x,1] = col.g / sum;
	//					splatmapData[y,x,2] = col.b / sum;
						if (TwoMaps) 
						{
							splatmapData[x,y,4] = col_b.r / sum;
							splatmapData[x,y,5] = col_b.g / sum;
							splatmapData[x,y,6] = col_b.b / sum;


							splatmapData[x,y,7] = 1f-sum;
//							splatmapData[x,y,6] = 0.0f;
						} else { // single map
							// TODO: need to reset old splats, if previously had 2 splatmaps?

//							splatmapData[x,y,4] = 0.0f;
							// reset old maps
//							splatmapData[x,y,4] = 0;
							//splatmapData[x,y,5] = 0;
							//splatmapData[x,y,6] = 0;
							//splatmapData[x,y,7] = 0;
						}

					} else { // not over 1.0

	//					splatmapData[w-1-y,x,0] = col.r;
	//					splatmapData[w-1-y,x,1] = col.g;
	//					splatmapData[w-1-y,x,2] = col.b;
						// derive final channel from white
						splatmapData[x,y,0] = col.r;
						splatmapData[x,y,1] = col.g;
						splatmapData[x,y,2] = col.b;
						splatmapData[x,y,3] = 1f-sum;

						if (TwoMaps) 
						{
							//Debug.Log("asddfasdf");
							//return;
							splatmapData[x,y,4] = col_b.r;
							splatmapData[x,y,5] = col_b.g;
							splatmapData[x,y,6] = col_b.b;
							splatmapData[x,y,7] = 1f-sum;
						} else {
							//splatmapData[x,y,3] = 1.0f - sum;
							// reset old maps
							//splatmapData[x,y,4] = 0;
							//splatmapData[x,y,5] = 0;
							//splatmapData[x,y,6] = 0;
							//splatmapData[x,y,7] = 0;
						}
					}
				} // for splatmap x pixels
			} // for splatmap y pixels

			EditorUtility.ClearProgressBar();
			terrainData.SetAlphamaps(0, 0, splatmapData);
			Debug.Log("Splatmaps applied "+GetTimerTime());
		}
		
		
		bool CheckTreemap() 
		{
			if (treemap == null) return false;
			FixTextureSettings(treemap);
			
			if (treemap.height != treemap.width) {
				EditorUtility.DisplayDialog("Wrong size", "treemap width and height must be the same", "Cancel"); 
				return false;
			}
			if (Mathf.ClosestPowerOfTwo(treemap.width) != treemap.width) {
				EditorUtility.DisplayDialog("Wrong size", "treemap width and height must be a power of two", "Cancel"); 
				return false;	
			}
			return true;
		}

		
		void ApplyTreemap() 
		{
			StartTimer();
			// set up my data
			if (!myTerrain) myTerrain = Terrain.activeTerrain;

			if (myTerrain==null) {Debug.LogError("No terrain selected"); return;}

			TerrainData terrainData = myTerrain.terrainData;


			// check if trees are assigned
			if (terrainData.treePrototypes.Length<3)
			{
				if (terrainData.treePrototypes.Length<1)
				{
					Debug.LogError("You should assign at least 1 tree to Terrain (at Place Trees tab)");
					return;
				}else{
					Debug.LogWarning("You should assign 3 trees to Terrain to use all 3 splatmap color channels (at Place Trees tab)");
				}
			}



			Undo.RecordObject(terrainData, "Apply tree map"); // slow..

			int width = treemap.width;

			Color[] mapColors = treemap.GetPixels();

			int index = -1;
			int maxIndex=terrainData.treePrototypes.Length;
			int trees = 0;

			float Step = 1.0f/treeDensity;
			float PositionVariation = (float)(Step*0.5f/(float)width);

			if (resetTrees) terrainData.treeInstances = new TreeInstance[0];

			int progress = 0;



			for (float y = 1; y < width-1; y+=Step) 
			{
				progress++;
				if (progress%10 == 0)
				{
					progress=0;
					if (EditorUtility.DisplayCancelableProgressBar("Placing trees", "placed "+trees+" trees so far", Mathf.InverseLerp(0.0f, (float)width, (float)y))) 
					{
						EditorUtility.ClearProgressBar();
						return;
					}
				}

				for (float x = 1; x < width-1; x+=Step)
				{
					// place the chosen tree, if the colours are right
					index = -1;
					//Color col = mapColors[Mathf.RoundToInt(y)*w + Mathf.RoundToInt(x)];
					Color col = mapColors[(width-Mathf.RoundToInt(y))*width+Mathf.RoundToInt(x)];
					//width-x,length-y

					if (col.r>treeThreshold+Random.Range(0.0f, 1.0f))
					{
						index = 0;
					}
					else if (maxIndex>1 && col.g>treeThreshold+Random.Range(0.0f, 1.0f)) 
					{
						index = 1;
					}
					else if (maxIndex>2 && col.b>treeThreshold+Random.Range(0.0f, 1.0f)) 
					{
						index = 2;
					}


					if (index>=0) 
					{
						// place a tree
						trees++;
						TreeInstance treeInstance = new TreeInstance();

						// random placement modifier for a more natural look
						float xpos = x/(float)width;
						float ypos = y/(float)width;
						xpos = Mathf.Clamp01(xpos+Random.Range(-PositionVariation, PositionVariation));
						ypos = Mathf.Clamp01(1-ypos+Random.Range(-PositionVariation, PositionVariation));

						treeInstance.position = new Vector3(xpos, 0f, ypos);

						treeInstance.color = Color.white;
						treeInstance.lightmapColor = Color.white;
						treeInstance.prototypeIndex = index;

						treeInstance.widthScale = treeSize * (1f + Random.Range(-sizeVariation, sizeVariation));
						treeInstance.heightScale = treeSize * (1f + Random.Range(-sizeVariation, sizeVariation));

						myTerrain.AddTreeInstance(treeInstance);

					}
				}
			}

			EditorUtility.ClearProgressBar();
			Debug.Log("placed "+trees+" trees "+GetTimerTime());
		}
		
		
		bool CheckGrassmap() 
		{
			if (grassmap != null) { 
				FixTextureSettings(grassmap);

				int w = grassmap.width;
				if (grassmap.height != w) 
				{
					EditorUtility.DisplayDialog("Wrong size", "grassmap width and height must be the same", "Cancel"); 
					return false;
				}
				if (Mathf.ClosestPowerOfTwo(w) != w) {
					EditorUtility.DisplayDialog("Wrong size", "grassmap width and height must be a power of two", "Cancel"); 
					return false;	
				}
			}

			if (bushmap != null) { 
				FixTextureSettings(bushmap);

				int w = bushmap.width;
				if (bushmap.height != w) {
					EditorUtility.DisplayDialog("Wrong size", "bushmap width and height must be the same", "Cancel"); 
					return false;
				}
				if (Mathf.ClosestPowerOfTwo(w) != w) {
					EditorUtility.DisplayDialog("Wrong size", "bushmap width and height must be a power of two", "Cancel"); 
					return false;	
				}
			}

			return true;
		}
		
		void ApplyGrassmap() 
		{
			StartTimer();

			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			if (myTerrain==null) {Debug.LogError("No terrain selected"); return;}
			TerrainData terrainData = myTerrain.terrainData;

			Undo.RecordObject(terrainData, "Apply grass and bush maps");
			
			if (grassmap!=null) 
			{
				if (SetDetailmap(grassmap, grassDensity, 0, grassclumping, "Grass map")) Debug.Log("Grass map applied "+GetTimerTime());
			}

			if (bushmap!=null) 
			{
				//if (SetDetailmap(bushmap, bushmod, 3, 0.0f, "Bush map")) Debug.Log("Bush map applied.");
				if (SetDetailmap(bushmap, bushDensity, grassmap==null?0:3, 0.0f, "Bush map")) Debug.Log("Bush map applied "+GetTimerTime());
			}
			EditorUtility.ClearProgressBar();
		}

		void ApplyBushmap() 
		{
			StartTimer();
			
			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			if (myTerrain==null) {Debug.LogError("No terrain selected"); return;}
			TerrainData terrainData = myTerrain.terrainData;
			
			Undo.RecordObject(terrainData, "Apply grass and bush maps");
			/*
			if (grassmap!=null) 
			{
				if (SetDetailmap(grassmap, grassDensity, 0, grassclumping, "Grass map")) Debug.Log("Grass map applied "+GetTimerTime());
			}*/
			
			if (bushmap!=null) 
			{
				//if (SetDetailmap(bushmap, bushmod, 3, 0.0f, "Bush map")) Debug.Log("Bush map applied.");
				if (SetDetailmap(bushmap, bushDensity, grassmap==null?0:3, 0.0f, "Bush map")) Debug.Log("Bush map applied "+GetTimerTime());
			}
			EditorUtility.ClearProgressBar();
		}

		
		bool SetDetailmap(Texture2D map, float mod, int firstlayer, float clumping, string MapName) 
		{
			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			TerrainData terrainData = myTerrain.terrainData;

			if (terrainData.detailPrototypes.Length<3)
			{
				if (terrainData.detailPrototypes.Length<1)
				{
					Debug.LogError ("You need to add at least 1 detail textures or 1 detail meshes to Terrain (at Paint Details)");
					return false;
				}
				Debug.LogWarning ("You should add 3 detail textures or 3 detail meshes to Terrain to use all splat map color channels (at Paint Details)");
			}

			// validate terrain details count
			int detailTextureCount=0;
			int detailMeshCount=0;
			int maxDetailMeshes = terrainData.detailPrototypes.Length;

			for (int nn=0; nn<terrainData.detailPrototypes.Length;nn++)
			{
				if (terrainData.detailPrototypes[nn].usePrototypeMesh)
				{
					detailMeshCount++;
				}else{
					detailTextureCount++;
				}
			}

			// check if there are any details for terrain
			if (MapName == "Grass map")
			{
				if (detailTextureCount<1)
				{
					Debug.LogError ("Grass map needs at least 1 detail texture at Terrain - Paint Details tab");
					return false;
				}
			}


			if (MapName == "Bush map")
			{
				if (detailMeshCount<1)
				{
					Debug.LogError ("Bush map at least 1 detail meshes at Terrain - Paint Details tab");
					return false;
				}
			}

			Color[] detailColors = map.GetPixels();
			int width = map.width;
			int res = terrainData.detailResolution;

			int[,] detail_a = new int[res,res];
			int[,] detail_b = new int[res,res];
			int[,] detail_c = new int[res,res];

			float scale = (float)width/(float)res;

			for (int y = 0; y < res; y++) 
			{

				if (EditorUtility.DisplayCancelableProgressBar("Applying "+MapName, "Calculating...", Mathf.InverseLerp(0.0f, (float)res, (float)y))) 
				{
					EditorUtility.ClearProgressBar();
					return false;
				}

				for (int x = 0; x < res; x++) 
				{
					// place detail, depending on colours in map image
					int sx = Mathf.FloorToInt((float)(x)*scale);
					int sy = Mathf.FloorToInt((float)(y)*scale);
					//Color col = detailColors[((width-1)-sx)*width+sy];
					Color col = detailColors[sx*width+sy];


					detail_a[x,y] = DetailValue(col.r*mod);
					if (maxDetailMeshes>2) detail_b[x,y] = DetailValue(col.g*mod);
					if (maxDetailMeshes>3) detail_c[x,y] = DetailValue(col.b*mod);
				}
			}
			
			if (clumping>0.01f) 
			{
				detail_a = MakeClumps(detail_a, clumping);
				if (maxDetailMeshes>2) detail_b = MakeClumps(detail_b, clumping);
				if (maxDetailMeshes>3) detail_c = MakeClumps(detail_c, clumping);
			}

			terrainData.SetDetailLayer(0, 0, firstlayer+0, detail_a);
			if (maxDetailMeshes>2) terrainData.SetDetailLayer(0, 0, firstlayer+1, detail_b);
			if (maxDetailMeshes>3) terrainData.SetDetailLayer(0, 0, firstlayer+2, detail_c);

			return true;

		}


		int DetailValue(float col) 
		{
			// linearly translates a 0.0-1.0 number to a 0-16 integer
			int c = Mathf.FloorToInt(col*16);
			float r = col*16 - Mathf.FloorToInt(col*16);
			
			if (r>Random.Range(0.0f, 1.0f)) c++;
			return Mathf.Clamp(c, 0, 16);
		}
		
		int[,] MakeClumps(int[,] map, float clumping) {
			int res = map.GetLength(0);
			int [,] clumpmap = new int[res,res];

			// init - there's probably a better way to do this in C# that I just don't know
			for (int y = 0; y < res; y++) {
				for (int x = 0; x < res; x++) {
					clumpmap[x,y]=0;
				}
			}

			for (int y = 0; y < res; y++) {
				for (int x = 0; x < res; x++) {
					clumpmap[x,y]+=map[x,y];
					if (map[x,y]>0) {
						// there's grass here, we might want to raise the grass value of our neighbours
						for (int a=-1;a<=1;a++) for (int b=-1;b<=1;b++) {
							if (x+a<0||x+a>=res||y+b<0||y+b>=res) continue;
							if (a!=0||b!=0&&Random.Range(0.0f, 1.0f)<clumping) clumpmap[x+a,y+b]++;
						}
					}
				}
			}

			// values above 9 yield strange results
			for (int y = 0; y < res; y++) {
				for (int x = 0; x < res; x++) {
					if (clumpmap[x,y]>9) clumpmap[x,y]=9;
				}
			}
			
			return clumpmap;
		}
		
		
		bool CheckOverlaymap() 
		{
			if (overlayMap==null) {Debug.LogError("No overlay map assigned"); return false;}

			
			if (!myTerrain) myTerrain = Terrain.activeTerrain;

			FixTextureSettings(overlayMap);

			if (overlayMap.height != overlayMap.width) {
				EditorUtility.DisplayDialog("Wrong size", "OverlayMap width and height must be the same", "Cancel"); 
				return false;
			}
			if (Mathf.ClosestPowerOfTwo(overlayMap.width) != overlayMap.width) {
				EditorUtility.DisplayDialog("Wrong size", "OverlayMap width and height must be a power of two", "Cancel"); 
				return false;	
			}

			if (overlayMap.width!=myTerrain.terrainData.alphamapResolution) {
				EditorUtility.DisplayDialog("Wrong size", "OverlayMap must have same size as existing splatmap ("+myTerrain.terrainData.alphamapResolution+")", "Cancel"); 
				return false;	
			}
			
			return true;
		}
		
		void ApplyOverlaymap() 
		{
			StartTimer();

			if (!myTerrain) myTerrain = Terrain.activeTerrain;
			TerrainData terrainData = myTerrain.terrainData;

			Undo.RecordObject(terrainData, "Apply overlay map");

			int w = overlayMap.width;
			Color[] OverlayMapColors = overlayMap.GetPixels();
			int layer = myTerrain.terrainData.alphamapLayers;

			int detailRes = terrainData.detailWidth;
			int[] detailLayers = terrainData.GetSupportedLayers(0, 0, detailRes, detailRes);
			int LayerCount = detailLayers.Length;

			AddTexture();

			float[,,] splatmapData = terrainData.GetAlphamaps(0, 0, w, w);

			float terrainScale = (float)w / ((float)terrainData.heightmapResolution-1);
			float terrainHeight = terrainData.size.y;
			int terrainSample = Mathf.CeilToInt(terrainScale);

			ArrayList NewTrees = new ArrayList(terrainData.treeInstances);
			int TreesRemoved = 0;

			for (int y = 0; y < w; y++) 
			{
				if (y%10 == 0) 
				{
					if (clearTrees) 
					{
						if (EditorUtility.DisplayCancelableProgressBar("Overlay map", "updating terrain and trees ("+TreesRemoved+" trees removed)", Mathf.InverseLerp(0.0f, (float)w, (float)y))) 
						{
							EditorUtility.ClearProgressBar();
							return;
						}
					} else {

						if (EditorUtility.DisplayCancelableProgressBar("Overlay map", "updating terrain", Mathf.InverseLerp(0.0f, (float)w, (float)y))) 
						{
							EditorUtility.ClearProgressBar();
							return;
						}
					}
				}

				for (int x = 0; x < w; x++) 
				{
//					float value = OverlayMapColors[((w-1)-x)*w + y].grayscale;
					float value = OverlayMapColors[x*w+y].grayscale;
					if (value>overlayThreshold) 
					{
						splatmapData[x,y,layer] = value;
						// fix the other layers

						for (int l = 0; l<layer; l++) {
							splatmapData[x,y,l] *= (1.0f-value);
						}

						if (changeTerrain>0.01f || changeTerrain<-0.01f) 
						{
							if (value>overlayThreshold) {
								float change = changeTerrain * value / terrainHeight;
								int sx = Mathf.FloorToInt((float)y*terrainScale);
								int sy = Mathf.FloorToInt((float)x*terrainScale);
								float [,] data = terrainData.GetHeights(sx, sy, terrainSample, terrainSample);
								for (int a=0;a<terrainSample;a++) for (int b=0;b<terrainSample;b++) {
									data[a,b]=Mathf.Max(0.0f, data[a,b]+change);
								}
								terrainData.SetHeights(sx, sy, data);
							}
						}
						
						if (clearTrees) 
						{
							for (int i = NewTrees.Count -1; i >= 0; i--) {
								TreeInstance MyTree = (TreeInstance)NewTrees[i];
								float distance = Vector2.Distance(new Vector2(MyTree.position.z*w, MyTree.position.x*w), new Vector2((float)x, (float)y));
								if (distance < clearRadius) {
									NewTrees.RemoveAt(i);
									TreesRemoved++;
								}
							}
						}

					} else {
						splatmapData[x,y,layer] = 0.0f;
					}
				}
			}
			if (clearTrees) 
			{
				terrainData.treeInstances = (TreeInstance[])NewTrees.ToArray(typeof(TreeInstance));
			}

			terrainData.SetAlphamaps(0, 0, splatmapData);
			Debug.Log("Overlay map applied "+GetTimerTime());

			if (clearGrass) 
			{
				float scale = (float)w / (float)detailRes;
				for (int l = 0; l<LayerCount; l++) 
				{
					if (EditorUtility.DisplayCancelableProgressBar("Overlay map", "clearing away grass", Mathf.InverseLerp(0.0f, (float)l, (float)LayerCount))) 
					{
						EditorUtility.ClearProgressBar();
						return;
					}
					int[,] grass = terrainData.GetDetailLayer(0, 0, detailRes, detailRes, l);
					for (int y = 0; y < detailRes; y++) {
						for (int x = 0; x < detailRes; x++) {
							int sx = Mathf.FloorToInt((float)(x)*scale);
							int sy = Mathf.FloorToInt((float)(y)*scale);
							//float value = OverlayMapColors[((w-1)-sx)*w + sy].grayscale;
							float value = OverlayMapColors[sx*w+sy].grayscale;
							if (value > overlayThreshold && grass[x,y]>0) 
							{
								if (value > 0.5f) grass[x,y]=0; else grass[x,y]=1;
							}
						}
					}
					terrainData.SetDetailLayer(0, 0, l, grass);
				}
			}

			EditorUtility.ClearProgressBar();
		}

		void AddTexture() 
		{
			if (!myTerrain) myTerrain = Terrain.activeTerrain;

			SplatPrototype[] oldPrototypes = myTerrain.terrainData.splatPrototypes;
			SplatPrototype[] newPrototypes = new SplatPrototype[oldPrototypes.Length + 1];

			for (int x=0;x<oldPrototypes.Length;x++) 
			{
				newPrototypes[x] = oldPrototypes[x];
			}
			newPrototypes[oldPrototypes.Length] = new SplatPrototype();
			newPrototypes[oldPrototypes.Length].texture = overlayTexture;
			Vector2 vector = new Vector2(tileSize, tileSize);
			newPrototypes[oldPrototypes.Length].tileSize = vector;
			myTerrain.terrainData.splatPrototypes = newPrototypes;
			EditorUtility.SetDirty(myTerrain);
		}



		// these are copied from UnityEditor.dll
		private void CreateTerrain(string name, int size) 
		{
			Selection.activeObject = null;

			TerrainData newTerrain = new TerrainData();

			newTerrain.heightmapResolution = size+1;
			newTerrain.size = new Vector3(terrainInitWidth, terrainInitHeight, terrainInitLength);
			newTerrain.heightmapResolution = size;
			newTerrain.baseMapResolution = 1024;
			newTerrain.SetDetailResolution(1024, 16); // recommended value from documentation

			newTerrain.wavingGrassSpeed = windSettingsSize;
			newTerrain.wavingGrassAmount = windSettingsBending;
			newTerrain.wavingGrassStrength = windSettingsSpeed;

			string timeStamp = System.DateTime.Now.ToString("ddMMHHmm");
			AssetDatabase.CreateAsset(newTerrain, name+ "Terrain"+(timeStampFileNames?timeStamp:"") +".asset");
			AssetDatabase.SaveAssets();

			Selection.activeObject = Terrain.CreateTerrainGameObject(newTerrain);


			myTerrain = Terrain.activeTerrains[Terrain.activeTerrains.Length-1]; 

		}

		
		public static float[,] ReadRawHeightMap(string path, int widthPlusOne, int heightmapWidth, int heightmapHeight) 
		{
			if (!File.Exists(path))
		    {
				// try with .r16 extension next
				path = path.Replace(".raw",".r16");
				if (!File.Exists(path))
				{
					Debug.LogWarning("Heightmap file not found: "+path);
					return null;
				}
			}

			byte[] buffer;
			using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read))) 
			{
				buffer = reader.ReadBytes(widthPlusOne * widthPlusOne * 2);
				reader.Close();
			}

//			int heightmapWidth = MyTerrain.terrainData.heightmapWidth;
//			int heightmapHeight = MyTerrain.terrainData.heightmapHeight;

			float[,] heights = new float[heightmapHeight, heightmapWidth];

			float deltaMac = -999.0f;
			float deltaPc = -999.0f;

			int morePc=0;
			int moreMac=0;
			int jj=0;

			// guess endian format by comparing height changes
			for (int n=0;n<heightmapHeight;n+=4)
			{
				float b1 = buffer[jj++];
				float b2 = buffer[jj++];
				float b3 = buffer[jj++];
				float b4 = buffer[jj++];
				float ht1 = (b1*256.0f + b2);
				float ht1mac = (b1*256.0f + b2);
				float ht2 = (b3*256.0f + b4);
				deltaMac=Mathf.Max (deltaMac,Mathf.Abs(ht1/65535.0f-ht2/65535.0f));
				ht1 = (b1+b2*256.0f);
				float ht1pc = (b1+b2*256.0f);
				ht2 = (b3+b4*256.0f);
				deltaPc=Mathf.Max (deltaPc,Mathf.Abs(ht1/65535.0f-ht2/65535.0f));
				if (ht1mac>ht1pc) moreMac++;else morePc+=1;
			}

			int ii=0;
			bool mac = deltaMac<deltaPc;

			for (int i = 0; i < heightmapHeight; i++) 
			{
				if (i%10 == 0)
				{

					if (EditorUtility.DisplayCancelableProgressBar("Importing heightmap", "calculating...", Mathf.InverseLerp(0.0f, (float)heightmapHeight, (float)i))) 
					{
						EditorUtility.ClearProgressBar();
						return null;
					}
				}

				for (int j = 0; j < heightmapWidth; j++) 
				{
					float bufferVal1 = buffer[ii++];
					float bufferVal2 = buffer[ii++];
					float ht = (mac) ? (bufferVal1*256.0f + bufferVal2) : (bufferVal1 + bufferVal2*256.0f);

					//heights[j                 , i] = (ht / 65535.0f); // not flipped * worked before ??
					//heights[j                 , heightmapWidth-i-1] = (ht / 65535.0f); // flip * needs -90 deg
					//heights[heightmapWidth-j-1, i] = (ht / 65535.0f); // flip original *needs +90 deg
					//heights[heightmapWidth-j-1  , heightmapWidth-i-1] = (ht / 65535.0f); // * needs flip y?

					//heights[i                 , j] = (ht / 65535.0f); // needs flip y?
					//heights[i                 , heightmapWidth-j-1] = (ht / 65535.0f); // flip * needs -180 deg?
					heights[heightmapWidth-i-1, j] = (ht / 65535.0f); // new best, with flip fix
					//heights[heightmapWidth-j-1  , heightmapWidth-i-1] = (ht / 65535.0f); // * needs flip y?

				}
			}
			return heights;
		} // ReadRaw()

		void StartTimer()
		{
			stopwatch.Reset();
			stopwatch.Start();
		}

		string GetTimerTime()
		{
			stopwatch.Stop();
			return " ("+stopwatch.Elapsed.Milliseconds+"ms)";
		}

	} // class
} // namespace
