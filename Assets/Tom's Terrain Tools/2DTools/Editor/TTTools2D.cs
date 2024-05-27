using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

// Experimantal 2D terrain slice builder

namespace TTT
{
	public class TTTools2D : EditorWindow 
	{

		//public Object heightMap;
		private Texture2D sourceSplatMap;

		private int edgeColliderResolution = 16; // 1 = 1 pixel resolution

//		private Color[] splatColor = new Color[3];
//		private Color undergroundColor = Color.black;
		private Texture2D[] splatTextures = new Texture2D[4];

		private bool addEdgeCollider = true;

		int slicePositionY=0; // FIXME: currently only works for 0
//		bool splatHasAlpha=false; // FixTextureSettings() checks if texture contains alpha channel, if yes, it will be used when drawing splatmap


		private const string appName = "2D Terrain Tools (Experimental)";

		[MenuItem ("Window/Terrain Tools/"+appName,false,101)]
		static void Init () {
			TTTools2D window = (TTTools2D)EditorWindow.GetWindow (typeof (TTTools2D));
			window.titleContent = new GUIContent(appName);
			window.minSize = new Vector2(400,300);
		}
		
		void OnGUI () 
		{
			GUILayout.Label (appName, EditorStyles.boldLabel);
			EditorGUILayout.Space();

			GUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Source Splatmap");
			sourceSplatMap = (Texture2D)EditorGUILayout.ObjectField (sourceSplatMap, typeof(Texture2D), false, GUILayout.Height (48));
			GUILayout.EndHorizontal ();
			EditorGUILayout.Space();

			GUILayout.BeginHorizontal();
			edgeColliderResolution = EditorGUILayout.IntField("EdgeCollider resolution",edgeColliderResolution);
			edgeColliderResolution = (int)Mathf.Clamp(edgeColliderResolution,1,128);
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();

			/*
			GUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Terrain Colors (R G B U)"); // U = underground
			GUILayout.FlexibleSpace ();

			splatColor[0] = EditorGUILayout.ColorField(splatColor[0],GUILayout.Height (48));
			splatColor[1] = EditorGUILayout.ColorField(splatColor[1],GUILayout.Height (48));
			splatColor[2] = EditorGUILayout.ColorField(splatColor[2],GUILayout.Height (48));
			undergroundColor = EditorGUILayout.ColorField(undergroundColor,GUILayout.Height (48));
			GUILayout.EndHorizontal ();
			*/

			GUILayout.BeginHorizontal ();
			EditorGUILayout.PrefixLabel ("Terrain Textures (R G B)"); // U = underground
			GUILayout.FlexibleSpace ();
			splatTextures [0] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [0], typeof(Texture2D), false, GUILayout.Height (48));
			splatTextures [1] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [1], typeof(Texture2D), false, GUILayout.Height (48));
			splatTextures [2] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [2], typeof(Texture2D), false, GUILayout.Height (48));
			splatTextures [3] = (Texture2D)EditorGUILayout.ObjectField (splatTextures [3], typeof(Texture2D), false, GUILayout.Height (48));
			GUILayout.EndHorizontal ();

			if (sourceSplatMap==null || splatTextures[0]==null ||splatTextures[1]==null || splatTextures[2]==null ||splatTextures[3]==null) GUI.enabled = false;
			if (GUILayout.Button ("Generate 2D slices", GUILayout.Width (200), GUILayout.Height (32))) 
			{
				Generate2DSlices();
			}
			GUI.enabled = true;

		} // ongui



		void Generate2DSlices()
		{

			if (sourceSplatMap==null) {Debug.LogError("sourceSplatMap is null"); return;}
//			TTT.TerrainTools.FixTextureSettings(sourceSplatMap);

			// textures
			if (splatTextures[0]!=null) FixTextureSettings(splatTextures[0]);
			if (splatTextures[1]!=null) FixTextureSettings(splatTextures[1]);
			if (splatTextures[2]!=null) FixTextureSettings(splatTextures[2]);
			if (splatTextures[3]!=null) FixTextureSettings(splatTextures[3]);


			Color[] splatmapColors = sourceSplatMap.GetPixels();
			int splatWidth = sourceSplatMap.width;

			string path = AssetDatabase.GetAssetPath(sourceSplatMap);
			string pathrev = TTT.TerrainTools.Reverse(Path.GetFileNameWithoutExtension(path));
			string[] parts = pathrev.Split(new char[] { '-' }, 2);
			string basename = TTT.TerrainTools.Reverse(parts[1]);
			string dirname = Path.GetDirectoryName(path);
			path = Path.Combine(dirname, basename);
			string heightmap_name = path+"-heightmap.raw";

			int targetWidth = sourceSplatMap.width;
			int targetHeight = 128; // TODO: allow adjusting this


			float[,] heights = TTT.TerrainTools.ReadRawHeightMap(heightmap_name,  targetWidth+1, targetWidth, targetWidth);

			if (heights==null) {Debug.LogError("Failed to read raw file:"+heightmap_name); return;}

			Texture2D sliceTex = new Texture2D(targetWidth,targetHeight,TextureFormat.ARGB32,false);
			sliceTex.hideFlags = HideFlags.HideAndDontSave;
			sliceTex.filterMode = FilterMode.Point;
			sliceTex.wrapMode = TextureWrapMode.Clamp;

			float surfaceDepth = targetHeight/3f;

			for (int x = 0; x < targetWidth; x++)
			{
				Color splatMapColor = splatmapColors[((splatWidth-1)-x)*splatWidth + slicePositionY];
				float height = heights[x,slicePositionY];

				for (int y = 0; y < targetHeight; y++) 
				{
					int textureScale = 8; // TODO: allow adjusting this

					int yPoint = ((y*(splatTextures[0].height/targetHeight)) ) % splatTextures[0].height;
					int yPointScaled = (yPoint*textureScale) % splatTextures[0].height;

					int yPoint3 = ((y*(splatTextures[3].height/targetHeight)) ) % splatTextures[3].height;
					int yPointScaled3 = (yPoint3) % splatTextures[3].height;

					Color tex1c = splatTextures[0].GetPixel(x,yPointScaled);
					Color tex2c = splatTextures[1].GetPixel(x,yPointScaled);
					Color tex3c = splatTextures[2].GetPixel(x,yPointScaled);
					Color tex4c = splatTextures[3].GetPixel(x,yPointScaled3); // underground

					Color splatMixColor = ((tex1c * splatMapColor.r) + (tex2c * splatMapColor.g) + (tex3c * splatMapColor.b)/3f);
					splatMixColor.a = 1;

					float originalSum = splatMapColor.r+splatMapColor.g+splatMapColor.b;
					float difference = 3f-originalSum;
					float fixValue = difference/3f;


					splatMixColor = CombineColors(tex1c*(splatMapColor.r+fixValue),tex2c*(splatMapColor.g+fixValue),tex3c*(splatMapColor.b+fixValue));

					Color c = new Color(1,1,1,0);

					// we are in the terrain
					if (y<height*targetHeight)
					{
						c = tex4c; 
					}

					if (y>height*targetHeight-surfaceDepth && y<=height*targetHeight) // we are at surface 
					{
						float val = Mathf.Abs(y-height*targetHeight) / surfaceDepth;
						c = Color.Lerp(splatMixColor, tex4c, val);
					}
					sliceTex.SetPixel(x,y,c);

				} // y
			} //x
			EditorUtility.ClearProgressBar();
			sliceTex.Apply(false);




			// save to file
			byte[] texBytes  = sliceTex.EncodeToPNG();
			string newName = Path.GetFileNameWithoutExtension(sourceSplatMap.name);
			string savePath = Path.GetDirectoryName(path) + "/" + newName +"_sliceMap.png";
			File.WriteAllBytes(savePath, texBytes);
			AssetDatabase.Refresh();

			Debug.Log("Slice generated to:"+savePath);



			// set as sprite in importer settings
			TextureImporter spriteImporter = AssetImporter.GetAtPath(savePath) as TextureImporter;
			TextureImporterSettings spriteSettings = new TextureImporterSettings();
			spriteImporter.textureType = TextureImporterType.Sprite;
			spriteImporter.ReadTextureSettings(spriteSettings);
			spriteSettings.filterMode = FilterMode.Point;
			spriteSettings.spriteMode = (int)SpriteImportMode.Single;
			spriteSettings.spritePixelsPerUnit = 100;
			spriteSettings.spriteAlignment  = (int)SpriteAlignment.BottomLeft;
			spriteImporter.SetTextureSettings(spriteSettings);
			AssetDatabase.ImportAsset(savePath, ImportAssetOptions.ForceUpdate );


			// Add to Scene
			GameObject goFolder = new GameObject();
			goFolder.name = "Slices";
			
			GameObject go = new GameObject();
			go.name = "slice";
			go.transform.parent = goFolder.transform;
			go.AddComponent<SpriteRenderer>();

			// take sprite from file
			var tempTex = AssetDatabase.LoadAssetAtPath(savePath, typeof(Sprite)) as Sprite;
			go.GetComponent<SpriteRenderer>().sprite = tempTex;
			if (addEdgeCollider)
			{
				EdgeCollider2D edge = go.AddComponent<EdgeCollider2D>();
				List<Vector2> verts = new List<Vector2>();

				for (int x = 0; x < targetWidth+edgeColliderResolution-1; x+=edgeColliderResolution)
				{
					bool foundedSurface=false;
					for (int y = 0; y < targetHeight; y++) 
					{
						Color c = sliceTex.GetPixel((int)Mathf.Clamp(x,0,targetWidth),y);

						if (Mathf.Approximately(c.a,0))
						{
							verts.Add(new Vector2(x/100f,y/100f));
							foundedSurface = true;
							break;
						}
					} // y
					if (!foundedSurface)
					{
						verts.Add(new Vector2(x/100f,targetHeight/100f));
					}
				} // x
				edge.points = verts.ToArray();
			}
			// cleanup
			UnityEngine.Object.DestroyImmediate(sliceTex);
		} // Generate2DSlices



		// http://answers.unity3d.com/questions/725895/best-way-to-mix-color-values.html
		Color CombineColors(params Color[] aColors)
		{
			Color result = new Color(0,0,0,0);
			foreach(Color c in aColors)
			{
				result += c;
			}
			result /= aColors.Length;
			result.a = 1;
			return result;
		}


		void ProgressBar(int y, int w)
		{
			if (y%10 == 0)
			{
				if (EditorUtility.DisplayCancelableProgressBar("Generating 2D Slices", "Calculating...", Mathf.InverseLerp(0.0f, (float)w, (float)y))) 
				{
					EditorUtility.ClearProgressBar();
					return;
				}
			}
		}

		// remap value from one range to another
		float ReMap(float val, float from1, float from2, float to1, float to2)
		{
			return to1 + (val-from1)*(to2-to1)/(from2-from1);
		}

		// update editor window 
	    void OnInspectorUpdate() 
		{
	    	Repaint();
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
				Debug.Log("fixed texture format for "+path);
			}
			
//			splatHasAlpha = textureImporter.DoesSourceTextureHaveAlpha();

		}

	} //class

} // namespace
