//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq; //for checking keywords array

static class STMCustomInspectorTools {
	public static void DrawTitleBar(UnityEngine.Object myObject, SuperTextMesh stm){
		if(myObject != null){
			EditorGUILayout.BeginHorizontal();
		//ping button:
			if(GUILayout.Button("Ping")){
				//EditorUtility.FocusProjectWindow(); this doesn't work for some reason
				EditorGUIUtility.PingObject(myObject); //select this object
			}
		//name:
			EditorGUI.BeginChangeCheck();
			myObject.name = EditorGUILayout.DelayedTextField(myObject.name);
			if(EditorGUI.EndChangeCheck()){
				AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(myObject), myObject.name);
				//Undo.RecordObject (myObject, "Change Asset Name");
				AssetDatabase.Refresh();
				stm.data = null;
			}
		//delete button:
			if(GUILayout.Button("X")){
				//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(myObject));
				AssetDatabase.MoveAssetToTrash(AssetDatabase.GetAssetPath(myObject));
				//Undo.DestroyObjectImmediate(myObject);
				AssetDatabase.Refresh();
				stm.data = null; //make this refresh, too
			}
			EditorGUILayout.EndHorizontal();
		}
	}
	public static string ClavianPath
    {
        get
        {
            string searchValue = "Clavian/SuperTextMesh/";
            string returnPath = "";
            string[] allPaths = AssetDatabase.GetAllAssetPaths();
            for (int i = 0; i < allPaths.Length; i++)
            {
                if (allPaths[i].Contains(searchValue))
                {
                    // This is the path we want! Let's strip out everything after the searchValue
                    returnPath = allPaths[i];
                    returnPath = returnPath.Remove(returnPath.IndexOf(searchValue));
                    returnPath += searchValue;
					break;
                }
            }

            return returnPath;
        }
    }
	/*
	public static void OnUndoRedo(){
		AssetDatabase.Refresh();
	}
	*/
	public static void FinishItem(UnityEngine.Object myObject){

	}
	public static void DrawCreateFolderButton(string buttonText, string parentFolder, string newFolder, SuperTextMesh stm){
		if(GUILayout.Button(buttonText)){
			AssetDatabase.CreateFolder(ClavianPath + "Resources/" + parentFolder, newFolder);
			AssetDatabase.Refresh();
			stm.data = null;
		}
	}
	public static void DrawCreateNewButton(string buttonText, string folderName, string typeName, SuperTextMesh stm){
		if(GUILayout.Button(buttonText)){
			ScriptableObject newData = NewData(typeName);
			if(newData != null){
				AssetDatabase.CreateAsset(newData,AssetDatabase.GenerateUniqueAssetPath(ClavianPath + "Resources/" + folderName)); //save to file
				//Undo.undoRedoPerformed += OnUndoRedo; //subscribe to event
				//Undo.RegisterCreatedObjectUndo(newData, buttonText);
				AssetDatabase.Refresh();
				stm.data = null;
			}
		}
	}
	public static ScriptableObject NewData(string myType){
		switch(myType){
			case "STMAudioClipData": return ScriptableObject.CreateInstance<STMAudioClipData>();
			case "STMAutoClipData": return ScriptableObject.CreateInstance<STMAutoClipData>();
			case "STMColorData": return ScriptableObject.CreateInstance<STMColorData>();
			case "STMDelayData": return ScriptableObject.CreateInstance<STMDelayData>();
			case "STMDrawAnimData": return ScriptableObject.CreateInstance<STMDrawAnimData>();
			case "STMFontData": return ScriptableObject.CreateInstance<STMFontData>();
			case "STMGradientData": return ScriptableObject.CreateInstance<STMGradientData>();
			case "STMJitterData": return ScriptableObject.CreateInstance<STMJitterData>();
			case "STMMaterialData": return ScriptableObject.CreateInstance<STMMaterialData>();
			case "STMQuadData": return ScriptableObject.CreateInstance<STMQuadData>();
			case "STMSoundClipData": return ScriptableObject.CreateInstance<STMSoundClipData>();
			case "STMTextureData": return ScriptableObject.CreateInstance<STMTextureData>();
			case "STMVoiceData": return ScriptableObject.CreateInstance<STMVoiceData>();
			case "STMWaveData": return ScriptableObject.CreateInstance<STMWaveData>();
			default: Debug.Log("New data type unknown."); return null;
		}
	}

	public static bool ShaderFeatureToggle(Material mat, string featureName, string variableName, string label)
	{
		bool enabled = mat.IsKeywordEnabled(featureName);
		EditorGUI.BeginChangeCheck();
		enabled = EditorGUILayout.Toggle(label, enabled);//show the toggle
		if(EditorGUI.EndChangeCheck())
		{
			mat.SetInt(variableName, enabled ? 1 : 0); //call this too so newer unity versions don't break
			if(enabled)
			{
				mat.EnableKeyword(featureName);
			}
			else
			{
				mat.DisableKeyword(featureName);
			}
		}
		return enabled;
	}
	public static void DrawMaterialEditor(Material mat, SuperTextMesh stm){
		//Just set these directly, why not. It's a custom inspector already, no need to bog this down even more
		Undo.RecordObject(mat, "Changed Super Text Mesh Material");
		//name changer
		EditorGUI.BeginChangeCheck();
		mat.name = EditorGUILayout.DelayedTextField("Material Name", mat.name);
		if(EditorGUI.EndChangeCheck()){
			AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(mat), mat.name);
			//Undo.RecordObject (myObject, "Change Asset Name");
			AssetDatabase.Refresh();
			//stm.data = null;
		}

		int originalQueue = mat.renderQueue;
		mat.shader = (Shader)EditorGUILayout.ObjectField("Shader", mat.shader, typeof(Shader), false);
		
		//set to correct value
		if(mat.HasProperty("_Cutoff")){
			mat.SetFloat("_Cutoff",0.0001f);
		}
		//set to correct value
		if(mat.HasProperty("_ShadowCutoff")){
			mat.SetFloat("_ShadowCutoff",0.5f);
		}

		//culling mode
		if(mat.HasProperty("_CullMode")){
			UnityEngine.Rendering.CullMode cullMode = (UnityEngine.Rendering.CullMode)mat.GetInt("_CullMode");
			cullMode = (UnityEngine.Rendering.CullMode)EditorGUILayout.EnumPopup("Cull Mode", cullMode);
			mat.SetInt("_CullMode", (int)cullMode);
		}
		//draw on top? dont show for UI mode, where this is set differently based on canvas
		if(!stm.uiMode && mat.HasProperty("_ZTestMode")){
			int zTestMode = mat.GetInt("_ZTestMode");
			bool onTop = zTestMode == 8;
			onTop = EditorGUILayout.Toggle("Render On Top", onTop);
			//Always or LEqual //right now this is 6 and 2, but shouldn't it be 8 and 4??
			mat.SetInt("_ZTestMode", onTop ? 8 : 4);
		}
		/* 
		//masking
		if(mat.HasProperty("_MaskMode")){
			int maskMode = mat.GetInt("_MaskMode");
			//bool masked = maskMode == 1;
			//masked = EditorGUILayout.Toggle("Masked", masked);
			maskMode = EditorGUILayout.Popup("Mask Mode", maskMode, new string[]{"Outside","Inside"});
			//Always or LEqual
			mat.SetInt("_MaskMode", maskMode);
		}
		*/
		if(mat.GetTag("STMUberShader2", true, "Null") == "Yes")
		{
			//GUILayout.Label("Ompuco shader detected!");
			/*
			 * layers:
			 * settings that effect all:
			 *	sdf mode
			 *	pixel snap
			 *  offset space [relative to scale, relative to text]
			 *
			 * not shownnn but it's a layer:
			 * text
			 *  color
			 *
			 * outline
			 *  enabled
			 *  color
			 *  type [circle, square]
			 *  points (1-32)
			 *  (maybe some buttons here for 8-pt and 4pt pixel, and default)
			 *  distance/width
			 *  extrude boolean?
			 *  
			 *  
			 *
			 * blur? outline 2?
			 *
			 * dropshadow
			 *  enabled
			 *  color
			 *	extrude boolean?
			 *  
			 *  type [angle, position]
			 *  if angle...
			 *     angle
			 *     distance
			 *  if position
			 *     vector3 shadow
			 *	blur?
			 *
			 * maybe blur could be if the outline is additive or not?
			 */
			EditorGUILayout.LabelField("Ultra Shader Settings", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
				//for now, no SDF mode for the ultra shader features
				if(mat.GetInt("_Effect") == 0)
				{
					var sdfMode = ShaderFeatureToggle(mat, "SDF_MODE", "_SDFMode", "SDF Mode");
					if(sdfMode)
					{
						EditorGUI.indentLevel++;
						if(mat.HasProperty("_Blend"))
						{
							//EditorGUILayout.PropertyField(shaderBlend);
							mat.SetFloat("_Blend", EditorGUILayout.Slider("Blend", mat.GetFloat("_Blend"), 0.0001f, 1f));
						}

						if(mat.HasProperty("_SDFCutoff"))
						{
							mat.SetFloat("_SDFCutoff",
								EditorGUILayout.Slider("Cutoff", mat.GetFloat("_SDFCutoff"), 0f, 1f));
						}

						EditorGUI.indentLevel--;
					}
				}

			ShaderFeatureToggle(mat, "PIXELSNAP_ON", "PixelSnap", "Pixel Snap");
			

				mat.SetInt("_Effect",
					EditorGUILayout.Popup("Effect", mat.GetInt("_Effect"), new[] { "None", "Dropshadow", "Outline"}));
					/*
				EditorGUILayout.BeginHorizontal();
				//replace this with a shader feature:
				EditorGUILayout.LabelField("Outline", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
				var outlineEnabled = ShaderFeatureToggle(mat, "OUTLINE_ENABLED", "_OutlineEnabled", string.Empty);
				EditorGUILayout.EndHorizontal();
				*/
				if(mat.GetInt("_Effect") == 2)
				{
					EditorGUI.indentLevel++;
					if(mat.HasProperty("_OutlineColor"))
					{
						mat.SetColor("_OutlineColor",
							EditorGUILayout.ColorField("Outline Color", mat.GetColor("_OutlineColor")));
					}

					if(mat.HasProperty("_OutlineWidth"))
					{
						mat.SetFloat("_OutlineWidth",
							EditorGUILayout.FloatField("Outline Width", mat.GetFloat("_OutlineWidth")));
					}

					if(mat.HasProperty("_OutlineType"))
					{
						mat.SetInt("_OutlineType",
							EditorGUILayout.IntPopup("Type", mat.GetInt("_OutlineType"), new[] { "Circle", "Square" }, new[] { 0, 1 }));
					}

					if(mat.HasProperty("_OutlineSamples"))
					{
						//taps used as it goes around
						mat.SetInt("_OutlineSamples",
							EditorGUILayout.IntSlider("Samples", mat.GetInt("_OutlineSamples"), 1, 256));
					}
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Presets:", GUILayout.Width(EditorGUIUtility.labelWidth));
					if(GUILayout.Button("Default"))
					{
						mat.SetInt("_OutlineType", 0);
						mat.SetInt("_OutlineSamples", 32);
					}

					if(GUILayout.Button("Pixel 4"))
					{
						mat.SetInt("_OutlineType", 1);
						mat.SetInt("_OutlineSamples", 4);
					}

					if(GUILayout.Button("Pixel 8"))
					{
						mat.SetInt("_OutlineType", 1);
						mat.SetInt("_OutlineSamples", 8);
					}

					EditorGUILayout.EndHorizontal();
					EditorGUI.indentLevel--;
				}
				
				/*
				EditorGUILayout.BeginHorizontal();
				//replace this with a shader feature:
				EditorGUILayout.LabelField("Dropshadow", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.labelWidth));
				var dropshadowEnabled = ShaderFeatureToggle(mat, "DROPSHADOW_ENABLED", "_DropshadowEnabled", string.Empty);
				EditorGUILayout.EndHorizontal();
				*/
				//make this a shader feature:
				if(mat.GetInt("_Effect") == 1)
				{
					EditorGUI.indentLevel++;
					if(mat.HasProperty("_DropshadowColor"))
					{
						//EditorGUILayout.PropertyField(shadowColor);
						mat.SetColor("_DropshadowColor",
							EditorGUILayout.ColorField("Shadow Color", mat.GetColor("_DropshadowColor")));
					}

					//is the value set with a vector or rotation?
					//this is a setting on the shader too now
					if(mat.HasProperty("_DropshadowType"))
					{
						mat.SetInt("_DropshadowType",
							EditorGUILayout.IntPopup("Type", mat.GetInt("_DropshadowType"), new[] { "Angle", "Vector" }, new[] { 0, 1 }));
						if(mat.GetInt("_DropshadowType") == 0)
						{
							if(mat.HasProperty("_DropshadowAngle"))
							{
								//EditorGUILayout.PropertyField(shadowAngle);
								mat.SetFloat("_DropshadowAngle",
									EditorGUILayout.Slider("Dropshadow Angle", mat.GetFloat("_DropshadowAngle"), 0f, 360f));
							}

							if(mat.HasProperty("_DropshadowDistance"))
							{
								//EditorGUILayout.PropertyField(shadowDistance);
								mat.SetFloat("_DropshadowDistance",
									EditorGUILayout.FloatField("Dropshadow Distance", mat.GetFloat("_DropshadowDistance")));
							}
						}
						else
						{
							if(mat.HasProperty("_DropshadowAngle2"))
							{
								mat.SetVector("_DropshadowAngle2",
									EditorGUILayout.Vector2Field("Dropshadow Vector", mat.GetVector("_DropshadowAngle2")));
							}
						}
					}
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
		}
		//if this is the multishader
		if(mat.GetTag("STMUberShader", true, "Null") == "Yes")
		{

		//toggle SDF
			var sdfMode = ShaderFeatureToggle(mat, "SDF_MODE", "_SDFMode", "SDF Mode");
			//#endif

			if(sdfMode)
			{//draw SDF-related properties
				if(mat.HasProperty("_Blend")){
					//EditorGUILayout.PropertyField(shaderBlend);
					mat.SetFloat("_Blend",EditorGUILayout.Slider("Blend",mat.GetFloat("_Blend"),0.0001f,1f));
				}
				if(mat.HasProperty("_SDFCutoff")){
					mat.SetFloat("_SDFCutoff",EditorGUILayout.Slider("SDF Cutoff",mat.GetFloat("_SDFCutoff"),0f,1f));
				}
			}
			//toggle Pixel Snap
			ShaderFeatureToggle(mat, "PIXELSNAP_ON", "PixelSnap", "Pixel Snap");
			
			//#endif
			if(mat.HasProperty("_ShadowColor")){
				//EditorGUILayout.PropertyField(shadowColor);
				mat.SetColor("_ShadowColor",EditorGUILayout.ColorField("Shadow Color",mat.GetColor("_ShadowColor")));
			}
			if(mat.HasProperty("_ShadowDistance")){
				//EditorGUILayout.PropertyField(shadowDistance);
				mat.SetFloat("_ShadowDistance",EditorGUILayout.FloatField("Shadow Distance",mat.GetFloat("_ShadowDistance")));
			}
			if(mat.HasProperty("_Vector3Dropshadow"))
			{
				
				//toggle use vector 3
				var useVector3 =
					ShaderFeatureToggle(mat, "VECTOR3_DROPSHADOW", "_Vector3Dropshadow", "Vector3 Dropshadow");
				if(useVector3 && mat.HasProperty("_ShadowAngle3"))
				{
					mat.SetVector("_ShadowAngle3",EditorGUILayout.Vector3Field("Shadow Angle3", mat.GetVector("_ShadowAngle3")));
				}
				else
				{
					//same as before
					if(mat.HasProperty("_ShadowAngle")){
						//EditorGUILayout.PropertyField(shadowAngle);
						mat.SetFloat("_ShadowAngle",EditorGUILayout.Slider("Shadow Angle",mat.GetFloat("_ShadowAngle"),0f,360f));
					}
				}
			}
			else
			{
				if(mat.HasProperty("_ShadowAngle")){
					//EditorGUILayout.PropertyField(shadowAngle);
					mat.SetFloat("_ShadowAngle",EditorGUILayout.Slider("Shadow Angle",mat.GetFloat("_ShadowAngle"),0f,360f));
				}
			}
			if(mat.HasProperty("_OutlineColor")){
				//EditorGUILayout.PropertyField(outlineColor);
				mat.SetColor("_OutlineColor",EditorGUILayout.ColorField("Outline Color",mat.GetColor("_OutlineColor")));
			}
			if(mat.HasProperty("_OutlineWidth")){
				//EditorGUILayout.PropertyField(outlineWidth);
				mat.SetFloat("_OutlineWidth",EditorGUILayout.FloatField("Outline Width",mat.GetFloat("_OutlineWidth")));
			}
			if(mat.HasProperty("_SquareOutline"))
			{
				ShaderFeatureToggle(mat, "SQUARE_OUTLINE", "_SquareOutline", "Square Outline");
			}
		}
		

		

		

		EditorGUILayout.BeginHorizontal();
		mat.renderQueue = EditorGUILayout.IntField("Render Queue", originalQueue);
		if(GUILayout.Button("Reset"))
		{
			mat.renderQueue = mat.shader.renderQueue;
		}
		EditorGUILayout.EndHorizontal();
	}
}
#endif
