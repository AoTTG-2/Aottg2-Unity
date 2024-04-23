//Copyright (c) 2016-2022 Kai Clavier [kaiclavier.com] Do Not Distribute
//Super Text Mesh v1.12.3
using UnityEngine;
using UnityEngine.Events; //for the OnComplete event
#if UNITY_EDITOR
using UnityEditor; //for loading default stuff and menu thing
#endif
using System; // For access to the array class
using System.Linq;
using System.Text;
//for sorting inspector stuff by creation date, and removing doubles from lists
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; //for in-game UI stuff
using System.Text.RegularExpressions;

#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement; //for OnSceneLoaded
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SuperTextMesh))]
[CanEditMultipleObjects] //sure why not
public class SuperTextMeshEditor : Editor{
	private SuperTextMesh stm;
	private Rect r;
	private GUIStyle foldoutStyle = null;
	private GUIStyle textDataStyle = null;
	private Texture2D textDataIcon = null;
	private Rect tempRect;
	private GUIContent editTextDataContent = new GUIContent("", "Edit TextData");
	private bool textDataEditMode; //used to be stm.data.textDataEditMode
	override public void OnInspectorGUI(){
		serializedObject.Update(); //for onvalidate stuff!
		stm = target as SuperTextMesh; //get this text mesh as a component
		
	//Actually Drawing it to the inspector:
		r = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, 0f); //get width on inspector, minus scrollbar

		if(foldoutStyle == null)
		{
			foldoutStyle = new GUIStyle(EditorStyles.foldout); //create a new foldout style, for the bold foldout headers
			foldoutStyle.fontStyle = FontStyle.Bold; //set it to look like a header
		}
	//TEXT DATA ICON
		//Object textDataObject = stm.data; //get text data object
		if(textDataStyle == null)
		{
			textDataStyle = new GUIStyle(EditorStyles.label);
		}
		//textDataStyle.fixedWidth = 14;
		//textDataStyle.fixedHeight = 14;
		//Get Texture2D one of these two ways:
		//Texture2D textDataIcon = AssetDatabase.LoadAssetAtPath("Assets/Clavian/SuperTextMesh/Scripts/SuperTextMeshDataIcon.png", typeof(Texture2D)) as Texture2D;
		if(textDataIcon == null)
		{
			textDataIcon = EditorGUIUtility.ObjectContent(stm.data, typeof(SuperTextMeshData)).image as Texture2D;
			textDataStyle.normal.background = textDataIcon; //apply
			textDataStyle.active.background = textDataIcon;
		}
		tempRect.Set(r.width - 2, r.y, 16, 16);
		if(GUI.Button(tempRect, editTextDataContent, textDataStyle)){ //place at exact spot
			//EditorWindow.GetWindow()
			//EditorUtility.FocusProjectWindow();
			//Selection.activeObject = textDataObject; //go to textdata!
			//EditorGUIUtility.PingObject(textDataObject);
			textDataEditMode = !textDataEditMode; //show textdata inspector!
			//if(textDataEditMode){
			//	= null;
			//}
		}
	
		if(textDataEditMode){//show textdata file instead
			var serializedData = new SerializedObject(stm.data);
			serializedData.Update();

		//Draw it!
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.Space(); //////////////////SPACE
			EditorGUILayout.HelpBox("Editing Text Data. Click the [T] to exit!", MessageType.None, true);

			stm.data.showEffectsFoldout = EditorGUILayout.Foldout(stm.data.showEffectsFoldout, "Effects", foldoutStyle);
			if(stm.data.showEffectsFoldout){
				EditorGUI.indentLevel++;
			//Waves:
				stm.data.showWavesFoldout = EditorGUILayout.Foldout(stm.data.showWavesFoldout, "Waves", foldoutStyle);
				if(stm.data.showWavesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMWaveData> i in stm.data.waves.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each wave
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Wave", "STMWaves/New Wave.asset", "STMWaveData", stm);
				}
			//Jitters:
				stm.data.showJittersFoldout = EditorGUILayout.Foldout(stm.data.showJittersFoldout, "Jitters", foldoutStyle);
				if(stm.data.showJittersFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMJitterData> i in stm.data.jitters.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Jitter", "STMJitters/New Jitter.asset", "STMJitterData", stm);
				}
			//Draw Animations:
				stm.data.showDrawAnimsFoldout = EditorGUILayout.Foldout(stm.data.showDrawAnimsFoldout, "DrawAnims", foldoutStyle);
				if(stm.data.showDrawAnimsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDrawAnimData> i in stm.data.drawAnims.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New DrawAnim", "STMDrawAnims/New DrawAnim.asset", "STMDrawAnimData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showTextColorFoldout = EditorGUILayout.Foldout(stm.data.showTextColorFoldout, "Text Color", foldoutStyle);
			if(stm.data.showTextColorFoldout){
				EditorGUI.indentLevel++;
			//Colors:
				stm.data.showColorsFoldout = EditorGUILayout.Foldout(stm.data.showColorsFoldout, "Colors", foldoutStyle);
				if(stm.data.showColorsFoldout){
				//Gather all data
					foreach(KeyValuePair<string, STMColorData> i in stm.data.colors.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Color", "STMColors/New Color.asset", "STMColorData", stm);
				}
			//Gradients:
				stm.data.showGradientsFoldout = EditorGUILayout.Foldout(stm.data.showGradientsFoldout, "Gradients", foldoutStyle);
				if(stm.data.showGradientsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMGradientData> i in stm.data.gradients.OrderBy(x => -x.Value.GetInstanceID())){ //reorder so the order stays consistent
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Gradient", "STMGradients/New Gradient.asset", "STMGradientData", stm);
				}
			//Textures:
				stm.data.showTexturesFoldout = EditorGUILayout.Foldout(stm.data.showTexturesFoldout, "Textures", foldoutStyle);
				if(stm.data.showTexturesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMTextureData> i in stm.data.textures.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Texture", "STMTextures/New Texture.asset", "STMTextureData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showInlineFoldout = EditorGUILayout.Foldout(stm.data.showInlineFoldout, "Inline", foldoutStyle);
			if(stm.data.showInlineFoldout){
				EditorGUI.indentLevel++;
			//Delays:
				stm.data.showDelaysFoldout = EditorGUILayout.Foldout(stm.data.showDelaysFoldout, "Delays", foldoutStyle);
				if(stm.data.showDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMDelayData> i in stm.data.delays.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Delay", "STMDelays/New Delay.asset", "STMDelayData", stm);
				}
			//Voices:
				stm.data.showVoicesFoldout = EditorGUILayout.Foldout(stm.data.showVoicesFoldout, "Voices", foldoutStyle);
				if(stm.data.showVoicesFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMVoiceData> i in stm.data.voices.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Voice", "STMVoices/New Voice.asset", "STMVoiceData", stm);
				}
			//Fonts:
				stm.data.showFontsFoldout = EditorGUILayout.Foldout(stm.data.showFontsFoldout, "Fonts", foldoutStyle);
				if(stm.data.showFontsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMFontData> i in stm.data.fonts.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Font", "STMFonts/New Font.asset", "STMFontData", stm);
				}
			//AudioClips:
				stm.data.showAudioClipsFoldout = EditorGUILayout.Foldout(stm.data.showAudioClipsFoldout, "AudioClips", foldoutStyle);
				if(stm.data.showAudioClipsFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMAudioClipData> i in stm.data.audioClips.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Audio Clip", "STMAudioClips/New Audio Clip.asset", "STMAudioClipData", stm);
				}
			//Sound Clips:
			//This one's a bit different! Since it's folders of clips...
				stm.data.showSoundClipsFoldout = EditorGUILayout.Foldout(stm.data.showSoundClipsFoldout, "Sound Clips", foldoutStyle);
				if(stm.data.showSoundClipsFoldout)
				{
				//Gather all data:
					foreach(KeyValuePair<string, STMSoundClipData> i in stm.data.soundClips.OrderBy(x => -x.Value.GetInstanceID()))
					{
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Sound Clip", "STMSoundClips/New Sound Clip.asset", "STMSoundClipData", stm);
				}
			//Quads:
				stm.data.showQuadsFoldout = EditorGUILayout.Foldout(stm.data.showQuadsFoldout, "Quads", foldoutStyle);
				if(stm.data.showQuadsFoldout)
				{
					EditorGUILayout.HelpBox("For information on how this works, please refer to the sample image under 'Quads' in the documentation.", MessageType.None, true);
				//Gather all data:
					foreach(KeyValuePair<string, STMQuadData> i in stm.data.quads.OrderBy(x => -x.Value.GetInstanceID()))
					{
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Quad", "STMQuads/New Quad.asset", "STMQuadData", stm);
				}
			//Materials:
				stm.data.showMaterialsFoldout = EditorGUILayout.Foldout(stm.data.showMaterialsFoldout, "Materials", foldoutStyle);
				if(stm.data.showMaterialsFoldout)
				{
				//Gather all data:
					foreach(KeyValuePair<string, STMMaterialData> i in stm.data.materials.OrderBy(x => -x.Value.GetInstanceID()))
					{
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Material", "STMMaterials/New Material.asset", "STMMaterialData", stm);
				}

				EditorGUI.indentLevel--;
			}

			stm.data.showAutomaticFoldout = EditorGUILayout.Foldout(stm.data.showAutomaticFoldout, "Automatic", foldoutStyle);
			if(stm.data.showAutomaticFoldout)
			{
				EditorGUI.indentLevel++;
			//AutoDelays:
				stm.data.showAutoDelaysFoldout = EditorGUILayout.Foldout(stm.data.showAutoDelaysFoldout, "AutoDelays", foldoutStyle);
				if(stm.data.showAutoDelaysFoldout){
				//Gather all data:
					foreach(KeyValuePair<string, STMAutoDelayData> i in stm.data.autoDelays.OrderBy(x => -x.Value.GetInstanceID()))
					{
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Delay", "STMAutoDelays/New Auto Delay.asset", "STMDelayData", stm);
				}
			//AutoClips:
				stm.data.showAutoClipsFoldout = EditorGUILayout.Foldout(stm.data.showAutoClipsFoldout, "AutoClips", foldoutStyle);
				if(stm.data.showAutoClipsFoldout)
				{
				//Gather all data:
					//STMSoundClipData[] allAutoClips = Resources.LoadAll<STMSoundClipData>("STMAutoClips").OrderBy(x => -x.GetInstanceID()).ToArray(); //do this so order keeps consistent
					foreach(KeyValuePair<string, STMAutoClipData> i in stm.data.autoClips.OrderBy(x => -x.Value.GetInstanceID())){
						EditorGUI.indentLevel++;
						i.Value.showFoldout = EditorGUILayout.Foldout(i.Value.showFoldout, i.Key, foldoutStyle);
						EditorGUI.indentLevel--;
						if(i.Value.showFoldout) if(i.Value != null)i.Value.DrawCustomInspector(stm); //draw a custom inspector for each Jitter
					}
				//Create new button:
					STMCustomInspectorTools.DrawCreateNewButton("Create New Auto Clip", "STMAutoClips/New Auto Clip.asset", "STMAutoClipData", stm);
				}
				EditorGUI.indentLevel--;
			}
			stm.data.showMasterFoldout = EditorGUILayout.Foldout(stm.data.showMasterFoldout, "Master", foldoutStyle);
			if(stm.data.showMasterFoldout)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(serializedData.FindProperty("disableAnimatedText"), true);
				EditorGUILayout.PropertyField(serializedData.FindProperty("defaultFont"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("boundsColor"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("textBoundsColor"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("finalTextBoundsColor"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("superscriptOffset"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("superscriptSize"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("subscriptOffset"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("subscriptSize"));
				EditorGUILayout.PropertyField(serializedData.FindProperty("inspectorFont"));
				EditorGUI.indentLevel--;
			}
			if(GUILayout.Button("Refresh Database"))
			{
				stm.data = null;
			}
			if(GUI.changed)
			{
				EditorUtility.SetDirty(stm.data);
			}

			serializedData.ApplyModifiedProperties();

			
		}else{ //draw actual text mesh inspector:
			
			Font oldFont = GUI.skin.font;
			if(stm.data.inspectorFont != null)
			{
				GUI.skin.font = stm.data.inspectorFont;
			}
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_text"));
			GUI.skin.font = oldFont;
/*			
			EditorGUILayout.LabelField("Text");
			Vector2 scroll = Vector2.zero;
			scroll = EditorGUILayout.BeginScrollView(scroll, false, true);
			GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea);
			stm._text = EditorGUILayout.TextArea(stm._text, textAreaStyle, new GUILayoutOption[]{GUILayout.MinHeight(80), GUILayout.MaxHeight(200), GUILayout.ExpandHeight(false), GUILayout.Width(Screen.width - 50)});
			EditorGUILayout.EndScrollView();
*/
			stm.showAppearanceFoldout = EditorGUILayout.Foldout(stm.showAppearanceFoldout, "Appearance", foldoutStyle);
			if(stm.showAppearanceFoldout)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_color")); //richtext default stuff...
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_fade"));
				//stm.color = EditorGUILayout.ColorField("Color", stm.color);
				if(stm.bestFit == SuperTextMesh.BestFitMode.Always)//no editing value
				{ 
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField("Size", stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
				}
				else if(stm.bestFit == SuperTextMesh.BestFitMode.SquishAlways)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField(stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
				}
				else if(stm.bestFit == SuperTextMesh.BestFitMode.OverLimit)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField(stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
				}
				else if(stm.bestFit == SuperTextMesh.BestFitMode.SquishOverLimit)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField(stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
				}
				else if(stm.bestFit == SuperTextMesh.BestFitMode.MultilineBETA)
				{ 
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.FloatField(stm.size * stm.bestFitMulti);
					EditorGUI.EndDisabledGroup();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"));
				}
				else //no best fit value
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("size"));
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("bestFit"));

				if(stm.font != null)
				{
					EditorGUI.BeginDisabledGroup(!stm.font.dynamic);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("style"));
					EditorGUI.EndDisabledGroup();
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("richText"));

				EditorGUILayout.Space(); //////////////////SPACE
				
				if(stm.font != null)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup(!stm.font.dynamic || stm.autoQuality);
					EditorGUILayout.PropertyField(serializedObject.FindProperty("quality")); //text rendering
					EditorGUI.EndDisabledGroup();
					if(stm.uiMode)
					{
						EditorGUILayout.PropertyField(serializedObject.FindProperty("autoQuality"));
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("filterMode"));
				if(!stm.uiMode){
					UnityEngine.Rendering.ShadowCastingMode shadowMode = stm.r.shadowCastingMode;
					stm.r.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)EditorGUILayout.EnumPopup("Shadow Casting Mode", shadowMode);
				}else
				{
					//masking options!
					
				}
				
				if(stm.textMaterial.GetTag("STMMaskingSupport", true, "Null") == "Yes")
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("maskMode"));
				}
				
				//EditorGUILayout.BeginHorizontal();
				//if(GUILayout.Button("Ping")){
					//EditorUtility.FocusProjectWindow();
				//	EditorGUIUtility.PingObject(stm.textMaterial); //select this object
				//}
		//Materials
				EditorGUILayout.BeginHorizontal();
				if(GUILayout.Button("New"))
				{
					//Debug.Log(ClavianPath);

					//create new material in the correct folder
					//give it the correct default shader
					//assign it to this text mesh
					string whatShader = stm.uiMode ? "Super Text Mesh/UI/Default" : "Super Text Mesh/Unlit/Default";
					Material newMaterial = new Material(Shader.Find(whatShader));
					if(!AssetDatabase.IsValidFolder(STMCustomInspectorTools.ClavianPath + "Materials")){
						//create folder if it doesn't exist yet
						AssetDatabase.CreateFolder(STMCustomInspectorTools.ClavianPath.Remove(STMCustomInspectorTools.ClavianPath.Length - 1), "Materials");
					}
					AssetDatabase.CreateAsset(newMaterial, AssetDatabase.GenerateUniqueAssetPath(STMCustomInspectorTools.ClavianPath + "Materials/NewMaterial.mat"));
					stm.textMaterial = newMaterial;
				}
				EditorGUILayout.PropertyField(serializedObject.FindProperty("textMaterial")); //appearance
				EditorGUILayout.EndHorizontal();
				//EditorGUILayout.EndHorizontal();

				if(stm.textMaterial != null){
					stm.showMaterialFoldout = EditorGUILayout.Foldout(stm.showMaterialFoldout, "Material", foldoutStyle);
					if(stm.showMaterialFoldout){ //show shader settings
						EditorGUI.BeginChangeCheck();
						//Undo.RecordObject(stm, "Changed Super Text Mesh Material");
						STMCustomInspectorTools.DrawMaterialEditor(stm.textMaterial, stm);
						if(EditorGUI.EndChangeCheck())
						{
							stm.Rebuild();
						}
					}
				}
			}

			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showPositionFoldout = EditorGUILayout.Foldout(stm.showPositionFoldout, "Position", foldoutStyle);
			if(stm.showPositionFoldout){
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("baseOffset")); //physical stuff
				EditorGUILayout.LabelField("Relative", GUILayout.MaxWidth(50f));
				stm.relativeBaseOffset = EditorGUILayout.Toggle(stm.relativeBaseOffset, GUILayout.MaxWidth(16f));
				EditorGUILayout.EndHorizontal();
				if(stm.uiMode){
					string[] anchorNames = new string[]{"Top", "Middle", "Bottom"};
					int[] anchorValues = new int[]{0,3,6};
					EditorGUI.BeginChangeCheck();
					int resultEnumValue = EditorGUILayout.IntPopup("Anchor", (int)Mathf.Floor((float)stm.anchor / 3f) * 3, anchorNames, anchorValues);
					if(EditorGUI.EndChangeCheck())
					{
						serializedObject.FindProperty("anchor").enumValueIndex = resultEnumValue;
					}
				}else{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("anchor"));
				}
				//if(!uiMode.boolValue){ //restrict this to non-ui only...?
					EditorGUILayout.PropertyField(serializedObject.FindProperty("alignment"));

				//}
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lineSpacing")); //text formatting
				EditorGUILayout.PropertyField(serializedObject.FindProperty("characterSpacing"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("tabSize"));
				EditorGUILayout.Space(); //////////////////SPACE
				if(!stm.uiMode){ //wrapping text works differently for UI:
					EditorGUILayout.PropertyField(serializedObject.FindProperty("autoWrap")); //automatic...
					if(stm.autoWrap > 0f){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("breakText"));
					//	EditorGUILayout.PropertyField(serializedObject.FindProperty("smartBreak"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("insertHyphens"));
					}
					EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimit"));
					if(stm.verticalLimit > 0f){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimitMode"));
					}
				}else{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("uiWrap"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("uiLimit"));
					EditorGUILayout.Space(); //////////////////SPACE
					EditorGUILayout.PropertyField(serializedObject.FindProperty("breakText"));
				//	EditorGUILayout.PropertyField(serializedObject.FindProperty("smartBreak"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("insertHyphens"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalLimitMode"));
				}
			}
			//EditorGUILayout.Space(); //////////////////SPACE
			stm.showTimingFoldout = EditorGUILayout.Foldout(stm.showTimingFoldout, "Timing", foldoutStyle);
			if(stm.showTimingFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoreTimeScale"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("disableAnimatedText"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("forceAnimation"));
				EditorGUILayout.Space(); //////////////////SPACE
				EditorGUILayout.PropertyField(serializedObject.FindProperty("readDelay")); //technical stuff
				if(stm.readDelay > 0f){
					EditorGUILayout.PropertyField(serializedObject.FindProperty("autoRead"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("rememberReadPosition"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("drawOrder"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("drawAnimName"));
					//stuff that needs progamming to work:
					stm.showFunctionalityFoldout = EditorGUILayout.Foldout(stm.showFunctionalityFoldout, "Functionality", foldoutStyle);
					if(stm.showFunctionalityFoldout){
						EditorGUILayout.PropertyField(serializedObject.FindProperty("speedReadScale"));
						EditorGUILayout.Space(); //////////////////SPACE
						EditorGUILayout.PropertyField(serializedObject.FindProperty("unreadDelay"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("undrawOrder"));
						EditorGUILayout.PropertyField(serializedObject.FindProperty("undrawAnimName"));
					}
					//GUIContent drawAnimLabel = new GUIContent("Draw Animation", "What draw animation will be used. Can be customized with TextData.");
					//selectedAnim.intValue = EditorGUILayout.Popup("Draw Animation", selectedAnim.intValue, stm.DrawAnimStrings());
					stm.showAudioFoldout = EditorGUILayout.Foldout(stm.showAudioFoldout, "Audio", foldoutStyle);
					if(stm.showAudioFoldout){
						//EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel); //HEADER
						EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"));
						if(stm.audioSource != null){ //flag
							EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClips"), true); //yes, show children
							EditorGUILayout.PropertyField(serializedObject.FindProperty("stopPreviousSound"));
							EditorGUILayout.PropertyField(serializedObject.FindProperty("pitchMode"));
							if(stm.pitchMode == SuperTextMesh.PitchMode.Normal){
								//nothing!
							}
							else if(stm.pitchMode == SuperTextMesh.PitchMode.Single){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("overridePitch"));
							}
							else{ //random between two somethings
								EditorGUILayout.PropertyField(serializedObject.FindProperty("minPitch"));
								EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPitch"));
							}
							if(stm.pitchMode == SuperTextMesh.PitchMode.Perlin){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("perlinPitchMulti"));
							}
							if(stm.speedReadScale < 1000f){
								EditorGUILayout.PropertyField(serializedObject.FindProperty("speedReadPitch"));
							}
						}
					}
				}
			}
			stm.showEventFoldout = EditorGUILayout.Foldout(stm.showEventFoldout, "Events", foldoutStyle);
			if(stm.showEventFoldout){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onRebuildEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onPrintEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onCompleteEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onUndrawnEvent"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onVertexMod"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onPreParse"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("onCustomEvent"));
			}

			stm.showBetaFoldout = EditorGUILayout.Foldout(stm.showBetaFoldout, "Beta", foldoutStyle);
			if(stm.showBetaFoldout)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rtl"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("removeEmoji"));
				/*
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PropertyField(serializedObject.FindProperty("validateMesh"));
				if(!stm.validateMesh)
				{
					if(GUILayout.Button("Validate"))
					{
						//stm.OnValidate();
						stm.OnValidate();
						stm.validateAppearance = true;
						stm.Update();
					}
				}
				EditorGUILayout.EndHorizontal();
				*/
			}

			//EditorGUILayout.Space(); //////////////////SPACE
			//EditorGUILayout.PropertyField(debugMode);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
#endif

[HelpURL("Assets/Clavian/SuperTextMesh/Documentation/SuperTextMesh.html")] //make the help open local documentation
[AddComponentMenu("Mesh/Super Text Mesh", 3)] //allow it to be added as a component
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class SuperTextMesh : MonoBehaviour, ILayoutElement, IMaskable { //MaskableGraphic... rip
	
	
	//foldout bools for editor. not on the GUI script, cause they get forgotten
	public bool showTextFoldout = true;
	public bool showAppearanceFoldout = true;
	public bool showMaterialFoldout = true;
	public bool showPositionFoldout = true;
	public bool showTimingFoldout = false;
	public bool showFunctionalityFoldout = false;
	public bool showAudioFoldout = false;
	public bool showEventFoldout = false;
	public bool showBetaFoldout = false;
	
	#if UNITY_EDITOR
	private static Transform MakeObjectFromAssetPath(MenuCommand menuCommand, string assetName)
	{
		//Create a game object
	   	GameObject objectFab = (GameObject)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/STMPrefabs/" + assetName + ".prefab", typeof(GameObject));
		GameObject newObject = Instantiate(objectFab); //instantiate prefab from assets
		newObject.transform.name = objectFab.name; //so it doesn't have "(Clone)" after
		GameObjectUtility.SetParentAndAlign(newObject, menuCommand.context as GameObject); //Ensure it gets reparented if this was a context click (otherwise does nothing)
		Undo.RegisterCreatedObjectUndo(newObject, "Create " + newObject.name); //Register the creation in the undo system
		Selection.activeObject = newObject;
		return newObject.transform;
	}
	private static void AttachToCanvas(Transform thisTransform, MenuCommand menuCommand)
	{
		//force-attach to canvas if it exists, or auto-create new one.
		Canvas myCanvas = (Canvas)FindObjectOfType(typeof(Canvas)); //find a canvas in the scene
		//just in case this is a prefab canvas
		if(myCanvas == null)
		{
			GameObject myParent = menuCommand.context as GameObject;
			if(myParent != null) myCanvas = myParent.GetComponentInParent<Canvas>();
		}
		if(myCanvas == null){ //create a new canvas to parent to!
			GameObject newObject = new GameObject();
			newObject.transform.name = "Canvas";
			myCanvas = newObject.AddComponent<Canvas>();
			myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			newObject.AddComponent<CanvasScaler>();
			newObject.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo(newObject, "Created New Canvas for UI Super Text");
		}
		if(thisTransform.GetComponentInParent<Canvas>() == null)
		{
			thisTransform.SetParent(myCanvas.transform);
		}
	}
	//Add to the gameobject menu:
	[MenuItem("GameObject/3D Object/Super 3D Text", false, 4000)] //instantiate a prefab of this
	private static void MakeNewText(MenuCommand menuCommand){
		MakeObjectFromAssetPath(menuCommand, "New Super Text");
	}
	[MenuItem("GameObject/UI/Super Text", false, 2001)] //instantiate a prefab of this
	private static void MakeNewUIText(MenuCommand menuCommand){
		AttachToCanvas(MakeObjectFromAssetPath(menuCommand, "Super Text"), menuCommand);
	}
	[MenuItem("GameObject/UI/Super Button", false, 2031)]
	private static void MakeNewUIButton(MenuCommand menuCommand)
	{
		AttachToCanvas(MakeObjectFromAssetPath(menuCommand, "Super Button"), menuCommand);
	}
	[MenuItem("GameObject/UI/Super Toggle", false, 2032)]
	private static void MakeNewUIToggle(MenuCommand menuCommand)
	{
		AttachToCanvas(MakeObjectFromAssetPath(menuCommand, "Super Toggle"), menuCommand);
	}
	[MenuItem("GameObject/UI/Super Dropdown", false, 2036)]
	private static void MakeNewUIDropdown(MenuCommand menuCommand)
	{
		AttachToCanvas(MakeObjectFromAssetPath(menuCommand, "Super Dropdown"), menuCommand);
	}
	/*
	[MenuItem("GameObject/UI/Super Input Field", false, 2038)]
	private static void MakeNewUIInputField(MenuCommand menuCommand)
	{
		AttachToCanvas(MakeObjectFromAssetPath(menuCommand, "Super InputField"));
	}
	*/

	#endif
	
	private static SuperTextMeshData _data; //made this static so it's only loaded once by default
	public SuperTextMeshData data{
		get{
			if(_data == null){
				//sticking with resource folder since it works in builds
				_data = Resources.Load("SuperTextMeshData") as SuperTextMeshData; //load textdata
				//_data = (SuperTextMeshData)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/SuperTextMeshData.asset", typeof(SuperTextMeshData));
				if(_data != null)
					_data.RebuildDictionaries(); //rebuild dictionaries
				else
					Debug.Log("Super Text Mesh Data not initialized. This might happen when first importing or updating Super Text Mesh. If this persists, please make sure Super Text Mesh's 'Resources' folders are left where they were upon import.");
			}
			return _data;
		}set{
			_data = value; //for setting it to be null so this gets redone!
		}
	}
	private Transform _t;
	public Transform t{
		get{
			if(_t == null)
			{
				if(this != null)
				{
					_t = this.transform;
				}
			}
			return _t;
		}
	}
	private MeshFilter _f;
	public MeshFilter f{
		get{
			if(_f == null) _f = t.GetComponent<MeshFilter>();
			if(_f == null) _f = t.gameObject.AddComponent<MeshFilter>();
			return _f;
		}
	}
	private MeshRenderer _r;
	public MeshRenderer r{
		get{
			if(_r == null) _r = t.GetComponent<MeshRenderer>();
			if(_r == null) _r = t.gameObject.AddComponent<MeshRenderer>();
			return _r;
		}
	}
	private CanvasRenderer _c;
	public CanvasRenderer c{
		get{
			if(_c == null) _c = t.GetComponent<CanvasRenderer>();
			if(_c == null) _c = t.gameObject.AddComponent<CanvasRenderer>();
			return _c;
		}
	}
	//public bool uiMode; //is it in UI mode? please don't change this manually
	public bool uiMode{
		get
		{
			if(t != null)
			{
				return t is RectTransform;
			}
			return false;
		}
	}

	public List<STMTextInfo> info = new List<STMTextInfo>(); //switching this out for an array & using temp lists makes less appear in the deep profiler, but has no effect on GC
	private List<int> lineBreaks = new List<int>(); //what characters are line breaks
	public List<float> lineHeights = new List<float>(); //for each line, the size of the tallest character in each
	internal List<float> boxHeights = new List<float>();
	[TextArea(3,10)] //[Multiline] also works, but i like this better
	[UnityEngine.Serialization.FormerlySerializedAs("text")]
	public string _text = "<c=rainbow><w>Hello, World!";
	public string text{
		get{
			return this._text;
		}
		set{
			this._text = value ?? ""; //never set it to be a null string or this causes an error.
			if(t.gameObject.activeInHierarchy)
				Rebuild(); //auto-rebuild
		}
	}
	public string Text{ //legacy fix since v1.6
		get{ //just do the same as text
			return this._text;
		}
		set{
			this._text = value ?? ""; //never set it to be a null string or this causes an error.
			if(t.gameObject.activeInHierarchy)
				Rebuild(); //auto-rebuild
		}
	}
	[HideInInspector] public string drawText; //text, after removing junk
	[HideInInspector] public string hyphenedText; //text, with junk added to it
	[Tooltip("Font to be used by this text mesh. .rtf, .otf, and Unity fonts are supported.")]
	public Font font;

	[UnityEngine.Serialization.FormerlySerializedAs("color")]
	[Tooltip("this was the old value for colour as isn't used by STM anymore. Don't use it!")]
	public Color32 _color32 = Color.white; //these may be parked as public but please don't use them. sorry.
	[Tooltip("Default color of the text mesh. This can be changed with the <c> tag! See the docs for more info.")]
	public Color _color = Color.white; //i wish marking this as internal didn't break stuff (well, you know why this doesn't work........ shoulda used namespaces)
	public Color color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}
	//[UnityEngine.Serialization.FormerlySerializedAs("color")]
	//private Color32 color32 = Color.white;
	[Tooltip("If true, Super Text Mesh will call SetMesh() every frame it is active. This is primarily to be used together with animating a changing color value.")]
	public bool forceAnimation = false;
	[Tooltip("Will the text listen to tags like <b> and <i>? See docs for a full list of tags.")]
	public bool richText = true; //care about formatting like <b>?
	[Tooltip("Delay in seconds between letters getting read out. Disabled if set to 0.")]
	public float readDelay = 0f; //disabled if 0.
	
	[Tooltip("Multiple of time for when speeding up text. Set it to a big number like 1000 to show all text immediately.")]
	public float speedReadScale = 2f; //for speeding thru text, this will be the delay. set to 0 to display instantly.
	[Tooltip("Whether reading uses deltaTime or fixedDeltaTime")]
	public bool ignoreTimeScale = true;
	public float GetDeltaTime{
		get{
			return data.disableAnimatedText || disableAnimatedText || !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public float GetTime{
		get{
			return data.disableAnimatedText || disableAnimatedText || !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledTime : Time.time;
		}
	}
	public float GetDeltaTime2{//for when the text is getting read out
		get{ //don't advance if application is not focused
			return !applicationFocused ? 0f : ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
		}
	}
	public bool disableAnimatedText = false; //disable for just this mesh
	/*
	public float GetTime2{ 
		get{
			return ignoreTimeScale ? Time.unscaledTime : Time.time;
		}
	}*/

	//public int selectedAnim = 0; //what draw animation is selected currently.....
	[Tooltip("Name of what draw animation will be used. Case-sensitive.")]
	public string drawAnimName = "Appear"; //this is a string instead of a custom dropdown so reordering saved animations can't change it

	[Tooltip("Delay between letters, for undrawing.")]
	public float unreadDelay = 0.05f;
	[Tooltip("Undraw order.")]
	public DrawOrder undrawOrder = DrawOrder.AllAtOnce;
	[Tooltip("Undraw animation name.")]
	public string undrawAnimName = "Appear";

	[Tooltip("Audio source for read sound clips. Sound won't be played if null.")]
	public AudioSource audioSource;
	[Tooltip("Default sound to be read by the above audio source. Can be left null to make no sound by default.")]
	public AudioClip[] audioClips;
	[Tooltip("Should a new letter's sound stop a previous one and play, or let the old one keep playing?")]
	public bool stopPreviousSound = true;

	[Tooltip("Pitch options for reading out text.")]
	public PitchMode pitchMode = PitchMode.Normal;
	public enum PitchMode{
		Normal,
		Single,
		Random,
		Perlin
	}
	[Tooltip("New pitch for the sound clip.")]
	[Range(0f,3f)]
	public float overridePitch = 1f;
	[Tooltip("Minimum pitch for random pitches. If same or greater than max pitch, this will be the pitch.")]
	[Range(0f,3f)]
	public float minPitch = 0.9f;
	[Tooltip("Maximum pitch for random pitches.")]
	[Range(0f,3f)]
	public float maxPitch = 1.2f;
	[Range(-2f,2f)]
	[Tooltip("This amount will be ADDED to the pitch when speedreading. Speedreading uses the delay from 'Fast Delay'")]
	public float speedReadPitch = 0f;
	[Tooltip("Multiple for how fast the perlin noise will advance.")]
	public float perlinPitchMulti = 1.0f;
	private bool speedReading = false;
	private bool skippingToEnd = false; //alt version of speedread that just skips to the end

	[HideInInspector] public bool reading = false; //is text currently being read out? this is public so it can be used by other scripts!
	private Coroutine readRoutine; //coroutine that handles reading out text
	[HideInInspector] public bool unreading = false;

	[Tooltip("Size in local space for letters, by default. Can be changed with the <s> tag.")]
	public float size = 1f; //size of letter in local space! not percentage of quality. letters can have diff sizes individually
	
	public float minSize = 0f;
	[HideInInspector] public float bestFitMulti = 1f; //for best fit, size will be multiplied by this...
	
	[Range(1,500)]
	[Tooltip("Point size of text. Try to keep it as small as possible while looking crisp!")]
	public int quality = 64; //actual text size. point size
	[Tooltip("Choose 'Point' for a crisp look. You'll probably want that for pixel fonts!")]
	public FilterMode filterMode = FilterMode.Bilinear;

	[Tooltip("This value is used with how UI Text reacts to masking.")]
	public MaskMode maskMode = MaskMode.Inside;
	public enum MaskMode
	{
		Inside,
		Outside,
		Ignore
	}
	//TODO: completely redraw text texture upon quality change. 2016-06-07 note: might have already done this
	[Tooltip("Default letter style. Can be changed with the <i> and <b> tags, using rich text.")]
	public FontStyle style = FontStyle.Normal;
	[Tooltip("Additional offset for text from the transform, in local space. This does not effect the bounding box, and can be used to better align text with other elements.")]
	public Vector3 baseOffset = Vector3.zero; //for offsetting z, mainly
	public bool relativeBaseOffset = true; //if this offset is relative to letter size
	[Tooltip("Adjust line spacing between multiple lines of text. 1 is the default for the font.")]
	public float lineSpacing = 1.0f;
	[Tooltip("Adjust additional spacing between characters. 0 is default.")]
	public float characterSpacing = 0.0f;
	[Tooltip("How far tabs indent.")]
	public float tabSize = 4.0f;
	[Tooltip("Distance in local space before a line break is automatically inserted at the previous space. Disabled if set to 0.")]
	public float autoWrap = 12f; //if text on one row exceeds this, insert line break at previously available space
	public float AutoWrap{ //get autowrap limit OR ui bounds
		get{
			if(uiMode && uiWrap)
			{
				//LayoutRebuilder.MarkLayoutForRebuild(tr);
				return (float)tr.rect.width; //get wrap limit, within left and right bounds!
				//return preferredWidth;
			}
			else if(uiMode && !uiWrap) return 0f;
			return autoWrap;
		}
	}
	public RectTransform tr{
		get{
			return t as RectTransform;
		}
	}
	[Tooltip("If true, STM will set its bounds based on RectTransform, without need for Content Size Fitter.")]
	public bool uiWrap = true;
	[Tooltip("If true, STM will set its bounds based on RectTransform, without need for Content Size Fitter.")]
	public bool uiLimit = true;

//	[Tooltip("Should text wrap at the edge of the bounding box, or go over?")]
//	public bool wrapText = true; 
	[Tooltip("With auto wrap, should large words be split to fit in the box?")]
	public bool breakText = false;
	//[Tooltip("This value controls how short a line of text can be when autowrapping. If set to 1, it's disabled, if set to 0, lines will try to be as short as possible.")]
	//[Range(0f,1f)]
	//public float smartBreak = 1f;
	[Tooltip("When large words are split, Should a hyphen be inserted?")]
	public bool insertHyphens = true;
	
	[Tooltip("The anchor point of the mesh. For UI text, this also controls the alignment.")]
	public TextAnchor anchor = TextAnchor.UpperLeft;
	[Tooltip("Decides where text should align to. Uses the Auto Wrap box as bounds.")]
	public Alignment alignment = Alignment.Left;
	public enum Alignment{
		Left,
		Center,
		Right,
		Justified,
		ForceJustified
	}
	[Tooltip("Maximum vertical space for this text box. Infinite if set to 0.")]
	public float verticalLimit = 0f;
	private float VerticalLimit{
		get{
			if(uiMode && uiLimit) return (float)tr.rect.height;
			//if(uiMode && uiLimit) return preferredHeight;
			else if(uiMode && !uiLimit) return 0f; //text isn't being limited
			return verticalLimit;
		}
	}
//	[Tooltip("For UI text, will cut off text if it goes beyond the vertical limit.")]
//	public bool limitText = true;
	public enum VerticalLimitMode{
		ShowLast,
		CutOff,
		Ignore,
		AutoPause,
		AutoPauseFull,
		SquishBETA
	}
	[Tooltip("How to treat text that goes over the vertical limit.")]
	public VerticalLimitMode verticalLimitMode = VerticalLimitMode.Ignore;

	public string leftoverText; //if verticalLimitMode is set to CutOff, this will be the text that got cutoff + all tags preceeding it.

	[Tooltip("The material to be used by this text mesh. This is a Material so settings can be shared between multiple text meshes easily.")]
	[UnityEngine.Serialization.FormerlySerializedAs("textMat")]
	public Material textMaterial; //material to use on all submeshes or whatever by default. will always be textMaterials[0]
	public Mesh textMesh; //keep track of mesh

	private bool areWeAnimating = false; //do we need to update every frame?

//bounds stuff! I don't use the Bounds class since it's easier for me to grab the corners from here, 
//and it'll be easier for devs to use, too
	[HideInInspector] public Vector3 rawTopLeftBounds; //bounds before transform is applied
	[HideInInspector] public Vector3 rawBottomRightBounds;
	[HideInInspector] public Vector3 rawBottomRightTextBounds; //widest & furthest point on all text, unclamped to bounds

	//[HideInInspector] private float minY; //the y position of the last letter, including cut letters

	[HideInInspector] public Vector3 topLeftBounds;
	[HideInInspector] public Vector3 topRightBounds;
	[HideInInspector] public Vector3 bottomLeftBounds;
	[HideInInspector] public Vector3 bottomRightBounds;
	[HideInInspector] public Vector3 centerBounds;

	[HideInInspector] public Vector3 topLeftTextBounds;
	[HideInInspector] public Vector3 topRightTextBounds;
	[HideInInspector] public Vector3 bottomLeftTextBounds;
	[HideInInspector] public Vector3 bottomRightTextBounds;

	[HideInInspector] public Vector3 centerTextBounds;

	[HideInInspector] public Vector3 finalTopLeftTextBounds;
	[HideInInspector] public Vector3 finalTopRightTextBounds;
	[HideInInspector] public Vector3 finalBottomLeftTextBounds;
	[HideInInspector] public Vector3 finalBottomRightTextBounds;

	[HideInInspector] public Vector3 finalCenterTextBounds;

	private float lowestPosition = 0f; //this is the lowest position of text that will be drawn, clamped within verticalLimit

	private float lowestDrawnPosition; //as the mesh reads out, this is the lowest position drawn, unclamped
	private float lowestDrawnPositionRaw; //without offset
	private float furthestDrawnPosition;

	private float totalWidth; //for figuring out preferred width


	public Vector3 unwrappedBottomRightTextBounds;

	public UnityEvent onCompleteEvent; //when the mesh is done drawing
	public delegate void OnCompleteAction();
	public event OnCompleteAction OnCompleteEvent; //matching unityevent

	public UnityEvent onUndrawnEvent; //for when undrawing finishes
	public delegate void OnUndrawnAction();
	public event OnUndrawnAction OnUndrawnEvent;

	public UnityEvent onRebuildEvent; //when rebuild() is called
	public delegate void OnRebuildAction();
	public event OnRebuildAction OnRebuildEvent;

	public UnityEvent onPrintEvent; //whenever a new letter is printed.
	public delegate void OnPrintAction();
	public event OnPrintAction OnPrintEvent;

	[System.Serializable] public class CustomEvent : UnityEvent<string, STMTextInfo>{} //tag, textinfo, 
	[UnityEngine.Serialization.FormerlySerializedAs("customEvent")]
	public CustomEvent onCustomEvent;
	public delegate void OnCustomAction(string text, STMTextInfo info);
	public event OnCustomAction OnCustomEvent;
	
	[System.Serializable] public class VertexMod : UnityEvent<Vector3[], Vector3[], Vector3[]>{}
	[UnityEngine.Serialization.FormerlySerializedAs("vertexMod")]
	public VertexMod onVertexMod;
	public delegate void OnVertexModAction(Vector3[] verts, Vector3[] middles, Vector3[] positions);
	public event OnVertexModAction OnVertexMod;

	[System.Serializable] public class PreParse : UnityEvent<STMTextContainer>{} //will change string before stm uses it
	[UnityEngine.Serialization.FormerlySerializedAs("preParse")]
	public PreParse onPreParse;
	public delegate void OnPreParseAction(STMTextContainer container);
	public event OnPreParseAction OnPreParse;

	public bool debugMode = false; //pretty much just here to un-hide inspector stuff

	[HideInInspector] public float totalReadTime = 0f;
	[HideInInspector] public float totalUnreadTime = 0f;
	[HideInInspector] public float currentReadTime = 0f; //what position in the mesh it's currently at. Right now, this is just so jitters don't animate more than they should when you speed past em.

	//generate these with ur vert calls or w/e!!!
	private Vector3[] endVerts = new Vector3[0];
	private Color32[] endCol32 = new Color32[0];
	//private int[] endTriangles = new int[0];
	private Vector2[] endUv = new Vector2[0];
	private Vector2[] endUv2 = new Vector2[0]; //overlay images
	private List<Vector4> ratiosAndUvMids = new List<Vector4>(); //ratios of each letter, to be embedded into uv3
	private List<Vector4> isUvRotated = new List<Vector4>();
	//private Vector2[] uvMids = new Vector2[0]; //centre of the UV on this letter, to be embedded into uv4
	private Vector3[] startVerts = new Vector3[0];
	private Color32[] startCol32 = new Color32[0];
	private Vector3[] midVerts = new Vector3[0];
	private Color32[] midCol32 = new Color32[0];

	//private List<SubMeshData> subMeshes = new List<SubMeshData>();

	#pragma warning disable //hide warning that says this is never used
	private float timeDrawn; //Time.time when the mesh was drawn. or Time.unscaledTime, depending
	#pragma warning restore

	[Tooltip("Decides if the mesh will read out automatically when rebuilt.")]
	public bool autoRead = true;
	[Tooltip("Decides if the mesh will remember where it was if disabled/enabled while reading.")]
	public bool rememberReadPosition = true;

	[Tooltip("For UI text. If true, quality is automatically set to be the same as size.")]
	public bool autoQuality = false;

	public enum DrawOrder
	{
		LeftToRight,
		AllAtOnce,
		OneWordAtATime,
		Random,
		RightToLeft,
		ReverseLTR,
		RTLOneWordAtATime,
		OneLineAtATime
	}
	[Tooltip("What order the text will draw in. 'All At Once' will ignore read delay. 'Robot' displays one word at a time. If set to 'Random', Read Delay becomes the time it'll take to draw the whole mesh.")]
	public DrawOrder drawOrder = DrawOrder.LeftToRight;

	private bool callReadFunction = false; //will the read function need to be called?

	public bool removeEmoji = true;

	public int pauseCount
	{
		get
		{
			return _pauseCount;
		}
	}
	private int _pauseCount = 0; //for <pause>. total amount of <pause>s that were reached while reading multiple times
	public int currentPauseCount
	{
		get
		{
			return _currentPauseCount;
		}
	}
	private int _currentPauseCount = 0; //amount of pauses found this rebuild cycle
	
	private float autoPauseStopPoint = 0f;

	public bool canContinue
	{
		get
		{
			return currentPauseCount > 0 && pauseCount < currentPauseCount;
		}
	}
	public bool canUndoContinue
	{
		get
		{
			return pauseCount > 0 && currentPauseCount > 0;
		}
	}

	private List<KeyValuePair<int,string>> allTags = new List<KeyValuePair<int,string>>();
	private List<Font> allFonts = new List<Font>(); //a list of all fonts used on this mesh

	//characters where a hyphen/linebreak could be inserted at. unicode is soft hyphen, hair space, zero width space, then japanese punctuation and space
	private static List<char> linebreakFriendlyChars = new List<char>(){' ', '\n', '\t', '-', '\u00AD', '\u200A', '\u200B', '。', '〜', '、', '…', '‥', '　'};

	//new linebreak rules! taken from https://en.wikipedia.org/wiki/Line_breaking_rules_in_East_Asian_languages
	//characters that cannot appear at the end of a line.
	private static List<char> linebreakUnfriendlyChars = new List<char>(){'$','(','£','¥','·','\'','\"','〈','《','「','『','【','〔','〖','〝','﹙','﹛','＄','（','．','［','｛','￡','￥', //simplified chinese
														'(','[','{','£','¥','\'','\"','‵','〈','《','「','『','〔','〝','︴','﹙','﹛','（','｛','︵','︷','︹','︻','︽','︿','﹁','﹃','﹏', //traditional chinese
														'(','[','｛','〔','〈','《','「','『','【','〘','〖','〝','\'','\"','｟','«'}; //japanese

	//characters that cannot appear at the start of a line.
	//there are some spaces in here...
	private static List<char> linestartUnfriendlyChars = new List<char>(){
		'!','%',')',',','.',':',';','?',']','}','¢','°','·','\'','\"','†','‡','›','℃','∶','、','。','〃','〆','〕','〗','〞','﹚','﹜','！','＂','％','＇','）','，','．','：','；','？','！','］','｝','～', //simplified chinese
		'!',')',',','.',':',';','?',']','}','¢','·','–','—',' ','\'','\"','•',' ','、','。','〆','〞','〕','〉','》','」','︰','︱','︲','︳','﹐','﹑','﹒','﹓','﹔','﹕','﹖','﹘','﹚','﹜','！','）','，','．','：','；','？','︶','︸','︺','︼','︾','﹀','﹂','﹗','］','｜','｝','､', //traditional chinese
		')',']','｝','〕','〉','》','」','』','】','〙','〗','〟','\'','\"','｠','»',//japanese closing brackets 
		'ヽ','ヾ','ー','ァ','ィ','ゥ','ェ','ォ','ッ','ャ','ュ','ョ','ヮ','ヵ','ヶ','ぁ','ぃ','ぅ','ぇ','ぉ','っ','ゃ','ゅ','ょ','ゎ','ゕ','ゖ','ㇰ','ㇱ','ㇲ','ㇳ','ㇴ','ㇵ','ㇶ','ㇷ','ㇸ','ㇹ','ㇺ','ㇻ','ㇼ','ㇽ','ㇾ','ㇿ','々','〻', //chiisai kana and special marks
		'‐','゠','–','〜', //hyphens
		'？','?','！','!','‼','⁇','⁈','⁉', //delimiters
		'・','、',':',';',',', //mid-sentence punctuation
		'。','.', //sentence-ending punctuation 
		'—','.','‥','〳','〴','〵', //japanese do-not-split-characters
		'0','1','2','3','4','5','6','7','8','9',//numbers (japanese do-not-split rule)
		'０','１','２','３','４','５','６','７','８','９'//numbers again, japanese monospace
	};



																	//different processing rules:
	//move punctuation to end of previous line.
	//send characters not permitted at the end of a line to the next line. or move a character from the last line to the next one.
	//squeeze to fit an appropriate character.
	//do not split

	[Tooltip("Adjusts paragraphs for text that was input right-to-left.")]
	public bool rtl = false;
	//private List<
	[Tooltip("All alpha values on this SUper Text Mesh will be multiplied by this value.")]
	[Range(0f,1f)]
	[SerializeField] private float _fade = 1f;
	public float fade
	{
		get
		{
			return _fade;
		}
		set
		{
			_fade = value;
			SetMesh(currentReadTime);
		}
	}

	[System.Serializable]
	public enum BestFitMode
	{
		Off,
		Always,
		OverLimit,
		SquishAlways,
		SquishOverLimit,
		MultilineBETA
	}
	public BestFitMode bestFit = BestFitMode.Off;

	//public bool validateMesh = true;

	//same as info[info.Count-1].pos.y ut cached

	STMDrawAnimData UndrawAnim{
		get{
			if(data.drawAnims.ContainsKey(undrawAnimName)){
				return data.drawAnims[undrawAnimName];
			}else if(data.drawAnims.ContainsKey("Appear")){
				return data.drawAnims["Appear"];
			}else{
				//Debug.Log("'Appear' draw animation isn't defined!"); //sometimes this'll get called on awake... oH
				data = null;
				return null;
			}
		}
	}
/*
	public string[] DrawAnimStrings(){ //get strings for the dropdown thing
		string[] myStrings = new string[data.drawAnims.Count];
		for(int i=0, iL=myStrings.Length; i<iL; i++){
			myStrings[i] = data.drawAnims[i].name;
		}
		if(selectedAnim >= myStrings.Length){
			selectedAnim = 0; //don't go over if one gets deleted
		}
		return myStrings;
	}
*/
	private bool applicationFocused = true;
	void OnApplicationFocus(bool focused){
		//don't run this in unity editor. text will still skip-forward in-editor with this, though... doesn't seem to be a way around it, unfortunately.
		#if !UNITY_EDITOR
		//this is needed cause without it, if mesh is ignoring time scale and application comes back into focus, time will skip forward.
		if(!Application.runInBackground){ //only care if the application cares
			applicationFocused = focused;
		}
		#endif
	}
	void OnDrawGizmosSelected(){ //draw boundsssss
		//if(autoWrap > 0f){ //bother to draw bounds?
			Gizmos.color = data.boundsColor;
			RecalculateBounds();
			Gizmos.DrawLine(topLeftBounds, topRightBounds); //top
			Gizmos.DrawLine(topLeftBounds, bottomLeftBounds); //left
			Gizmos.DrawLine(topRightBounds, bottomRightBounds); //right
			Gizmos.DrawLine(bottomLeftBounds, bottomRightBounds); //bottom

			Gizmos.color = data.textBoundsColor;
			Gizmos.DrawLine(topLeftTextBounds, topRightTextBounds); //top
			Gizmos.DrawLine(topLeftTextBounds, bottomLeftTextBounds); //left
			Gizmos.DrawLine(topRightTextBounds, bottomRightTextBounds); //right
			Gizmos.DrawLine(bottomLeftTextBounds, bottomRightTextBounds); //bottom

			Gizmos.color = data.finalTextBoundsColor;
			Gizmos.DrawLine(finalTopLeftTextBounds, finalTopRightTextBounds); //top
			Gizmos.DrawLine(finalTopLeftTextBounds, finalBottomLeftTextBounds); //left
			Gizmos.DrawLine(finalTopRightTextBounds, finalBottomRightTextBounds); //right
			Gizmos.DrawLine(finalBottomLeftTextBounds, finalBottomRightTextBounds); //bottom

			Gizmos.color = data.boundsColor;
			//Gizmos.DrawSphere(centerBounds, 0.1f);
			//Gizmos.color = Color.yellow; //draw maxes
			//Gizmos.DrawLine(topRightTextBounds, rawBottomRightTextBounds);
			//Gizmos.DrawLine(bottomLeftTextBounds, rawBottomRightTextBounds);
			//Gizmos.DrawLine()
		//}
	}
	public string RemoveEmoji(string x)
	{
		return Regex.Replace(x, @"[#*0-9]\uFE0F\u20E3|[\u00A9\u00AE\u203C\u2049\u2122\u2139\u2194-\u2199\u21A9\u21AA\u231A\u231B\u2328\u23CF\u23E9-\u23F3\u23F8-\u23FA\u24C2\u25AA\u25AB\u25B6\u25C0\u25FB-\u25FE\u2600-\u2604\u260E\u2611\u2614\u2615\u2618]|\u261D(?:\uD83C[\uDFFB-\uDFFF])?|[\u2620\u2622\u2623\u2626\u262A\u262E\u262F\u2638-\u263A\u2640\u2642\u2648-\u2653\u265F\u2660\u2663\u2665\u2666\u2668\u267B\u267E\u267F\u2692-\u2697\u2699\u269B\u269C\u26A0\u26A1\u26AA\u26AB\u26B0\u26B1\u26BD\u26BE\u26C4\u26C5\u26C8\u26CE\u26CF\u26D1\u26D3\u26D4\u26E9\u26EA\u26F0-\u26F5\u26F7\u26F8]|\u26F9(?:\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?|\uFE0F\u200D[\u2640\u2642]\uFE0F)?|[\u26FA\u26FD\u2702\u2705\u2708\u2709]|[\u270A-\u270D](?:\uD83C[\uDFFB-\uDFFF])?|[\u270F\u2712\u2714\u2716\u271D\u2721\u2728\u2733\u2734\u2744\u2747\u274C\u274E\u2753-\u2755\u2757\u2763\u2764\u2795-\u2797\u27A1\u27B0\u27BF\u2934\u2935\u2B05-\u2B07\u2B1B\u2B1C\u2B50\u2B55\u3030\u303D\u3297\u3299]|\uD83C(?:[\uDC04\uDCCF\uDD70\uDD71\uDD7E\uDD7F\uDD8E\uDD91-\uDD9A]|\uDDE6\uD83C[\uDDE8-\uDDEC\uDDEE\uDDF1\uDDF2\uDDF4\uDDF6-\uDDFA\uDDFC\uDDFD\uDDFF]|\uDDE7\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEF\uDDF1-\uDDF4\uDDF6-\uDDF9\uDDFB\uDDFC\uDDFE\uDDFF]|\uDDE8\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDEE\uDDF0-\uDDF5\uDDF7\uDDFA-\uDDFF]|\uDDE9\uD83C[\uDDEA\uDDEC\uDDEF\uDDF0\uDDF2\uDDF4\uDDFF]|\uDDEA\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDED\uDDF7-\uDDFA]|\uDDEB\uD83C[\uDDEE-\uDDF0\uDDF2\uDDF4\uDDF7]|\uDDEC\uD83C[\uDDE6\uDDE7\uDDE9-\uDDEE\uDDF1-\uDDF3\uDDF5-\uDDFA\uDDFC\uDDFE]|\uDDED\uD83C[\uDDF0\uDDF2\uDDF3\uDDF7\uDDF9\uDDFA]|\uDDEE\uD83C[\uDDE8-\uDDEA\uDDF1-\uDDF4\uDDF6-\uDDF9]|\uDDEF\uD83C[\uDDEA\uDDF2\uDDF4\uDDF5]|\uDDF0\uD83C[\uDDEA\uDDEC-\uDDEE\uDDF2\uDDF3\uDDF5\uDDF7\uDDFC\uDDFE\uDDFF]|\uDDF1\uD83C[\uDDE6-\uDDE8\uDDEE\uDDF0\uDDF7-\uDDFB\uDDFE]|\uDDF2\uD83C[\uDDE6\uDDE8-\uDDED\uDDF0-\uDDFF]|\uDDF3\uD83C[\uDDE6\uDDE8\uDDEA-\uDDEC\uDDEE\uDDF1\uDDF4\uDDF5\uDDF7\uDDFA\uDDFF]|\uDDF4\uD83C\uDDF2|\uDDF5\uD83C[\uDDE6\uDDEA-\uDDED\uDDF0-\uDDF3\uDDF7-\uDDF9\uDDFC\uDDFE]|\uDDF6\uD83C\uDDE6|\uDDF7\uD83C[\uDDEA\uDDF4\uDDF8\uDDFA\uDDFC]|\uDDF8\uD83C[\uDDE6-\uDDEA\uDDEC-\uDDF4\uDDF7-\uDDF9\uDDFB\uDDFD-\uDDFF]|\uDDF9\uD83C[\uDDE6\uDDE8\uDDE9\uDDEB-\uDDED\uDDEF-\uDDF4\uDDF7\uDDF9\uDDFB\uDDFC\uDDFF]|\uDDFA\uD83C[\uDDE6\uDDEC\uDDF2\uDDF3\uDDF8\uDDFE\uDDFF]|\uDDFB\uD83C[\uDDE6\uDDE8\uDDEA\uDDEC\uDDEE\uDDF3\uDDFA]|\uDDFC\uD83C[\uDDEB\uDDF8]|\uDDFD\uD83C\uDDF0|\uDDFE\uD83C[\uDDEA\uDDF9]|\uDDFF\uD83C[\uDDE6\uDDF2\uDDFC]|[\uDE01\uDE02\uDE1A\uDE2F\uDE32-\uDE3A\uDE50\uDE51\uDF00-\uDF21\uDF24-\uDF84]|\uDF85(?:\uD83C[\uDFFB-\uDFFF])?|[\uDF86-\uDF93\uDF96\uDF97\uDF99-\uDF9B\uDF9E-\uDFC1]|\uDFC2(?:\uD83C[\uDFFB-\uDFFF])?|[\uDFC3\uDFC4](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDFC5\uDFC6]|\uDFC7(?:\uD83C[\uDFFB-\uDFFF])?|[\uDFC8\uDFC9]|\uDFCA(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDFCB\uDFCC](?:\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?|\uFE0F\u200D[\u2640\u2642]\uFE0F)?|[\uDFCD-\uDFF0]|\uDFF3(?:\uFE0F\u200D\uD83C\uDF08)?|\uDFF4(?:\u200D\u2620\uFE0F|\uDB40\uDC67\uDB40\uDC62\uDB40(?:\uDC65\uDB40\uDC6E\uDB40\uDC67|\uDC73\uDB40\uDC63\uDB40\uDC74|\uDC77\uDB40\uDC6C\uDB40\uDC73)\uDB40\uDC7F)?|[\uDFF5\uDFF7-\uDFFF])|\uD83D(?:[\uDC00-\uDC14]|\uDC15(?:\u200D\uD83E\uDDBA)?|[\uDC16-\uDC40]|\uDC41(?:\uFE0F\u200D\uD83D\uDDE8\uFE0F)?|[\uDC42\uDC43](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC44\uDC45]|[\uDC46-\uDC50](?:\uD83C[\uDFFB-\uDFFF])?|[\uDC51-\uDC65]|[\uDC66\uDC67](?:\uD83C[\uDFFB-\uDFFF])?|\uDC68(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\u2764\uFE0F\u200D\uD83D(?:\uDC8B\u200D\uD83D)?\uDC68|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|[\uDC68\uDC69]\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|\uD83E[\uDDAF-\uDDB3\uDDBC\uDDBD])|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E[\uDDAF-\uDDB3\uDDBC\uDDBD]))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C\uDFFB|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB\uDFFC]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFD]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFB-\uDFFE]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?))?|\uDC69(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\u2764\uFE0F\u200D\uD83D(?:\uDC8B\u200D\uD83D)?[\uDC68\uDC69]|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?|\uDC69\u200D\uD83D(?:\uDC66(?:\u200D\uD83D\uDC66)?|\uDC67(?:\u200D\uD83D[\uDC66\uDC67])?)|[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92])|\uD83E[\uDDAF-\uDDB3\uDDBC\uDDBD])|\uD83C(?:\uDFFB(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D\uDC68\uD83C[\uDFFC-\uDFFF]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFC(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC68\uD83C[\uDFFB\uDFFD-\uDFFF]|\uDC69\uD83C\uDFFB)|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFD(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC68\uD83C[\uDFFB\uDFFC\uDFFE\uDFFF]|\uDC69\uD83C[\uDFFB\uDFFC])|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFE(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D(?:\uDC68\uD83C[\uDFFB-\uDFFD\uDFFF]|\uDC69\uD83C[\uDFFB-\uDFFD])|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?|\uDFFF(?:\u200D(?:[\u2695\u2696\u2708]\uFE0F|\uD83C[\uDF3E\uDF73\uDF93\uDFA4\uDFA8\uDFEB\uDFED]|\uD83D[\uDCBB\uDCBC\uDD27\uDD2C\uDE80\uDE92]|\uD83E(?:\uDD1D\u200D\uD83D[\uDC68\uDC69]\uD83C[\uDFFB-\uDFFE]|[\uDDAF-\uDDB3\uDDBC\uDDBD])))?))?|\uDC6A|[\uDC6B-\uDC6D](?:\uD83C[\uDFFB-\uDFFF])?|\uDC6E(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDC6F(?:\u200D[\u2640\u2642]\uFE0F)?|\uDC70(?:\uD83C[\uDFFB-\uDFFF])?|\uDC71(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDC72(?:\uD83C[\uDFFB-\uDFFF])?|\uDC73(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDC74-\uDC76](?:\uD83C[\uDFFB-\uDFFF])?|\uDC77(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDC78(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC79-\uDC7B]|\uDC7C(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC7D-\uDC80]|[\uDC81\uDC82](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDC83(?:\uD83C[\uDFFB-\uDFFF])?|\uDC84|\uDC85(?:\uD83C[\uDFFB-\uDFFF])?|[\uDC86\uDC87](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDC88-\uDCA9]|\uDCAA(?:\uD83C[\uDFFB-\uDFFF])?|[\uDCAB-\uDCFD\uDCFF-\uDD3D\uDD49-\uDD4E\uDD50-\uDD67\uDD6F\uDD70\uDD73]|\uDD74(?:\uD83C[\uDFFB-\uDFFF])?|\uDD75(?:\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?|\uFE0F\u200D[\u2640\u2642]\uFE0F)?|[\uDD76-\uDD79]|\uDD7A(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD87\uDD8A-\uDD8D]|[\uDD90\uDD95\uDD96](?:\uD83C[\uDFFB-\uDFFF])?|[\uDDA4\uDDA5\uDDA8\uDDB1\uDDB2\uDDBC\uDDC2-\uDDC4\uDDD1-\uDDD3\uDDDC-\uDDDE\uDDE1\uDDE3\uDDE8\uDDEF\uDDF3\uDDFA-\uDE44]|[\uDE45-\uDE47](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDE48-\uDE4A]|\uDE4B(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDE4C(?:\uD83C[\uDFFB-\uDFFF])?|[\uDE4D\uDE4E](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDE4F(?:\uD83C[\uDFFB-\uDFFF])?|[\uDE80-\uDEA2]|\uDEA3(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDEA4-\uDEB3]|[\uDEB4-\uDEB6](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDEB7-\uDEBF]|\uDEC0(?:\uD83C[\uDFFB-\uDFFF])?|[\uDEC1-\uDEC5\uDECB]|\uDECC(?:\uD83C[\uDFFB-\uDFFF])?|[\uDECD-\uDED2\uDED5\uDEE0-\uDEE5\uDEE9\uDEEB\uDEEC\uDEF0\uDEF3-\uDEFA\uDFE0-\uDFEB])|\uD83E(?:[\uDD0D\uDD0E]|\uDD0F(?:\uD83C[\uDFFB-\uDFFF])?|[\uDD10-\uDD17]|[\uDD18-\uDD1C](?:\uD83C[\uDFFB-\uDFFF])?|\uDD1D|[\uDD1E\uDD1F](?:\uD83C[\uDFFB-\uDFFF])?|[\uDD20-\uDD25]|\uDD26(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDD27-\uDD2F]|[\uDD30-\uDD36](?:\uD83C[\uDFFB-\uDFFF])?|\uDD37(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDD38\uDD39](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDD3A|\uDD3C(?:\u200D[\u2640\u2642]\uFE0F)?|[\uDD3D\uDD3E](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDD3F-\uDD45\uDD47-\uDD71\uDD73-\uDD76\uDD7A-\uDDA2\uDDA5-\uDDAA\uDDAE-\uDDB4]|[\uDDB5\uDDB6](?:\uD83C[\uDFFB-\uDFFF])?|\uDDB7|[\uDDB8\uDDB9](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDDBA|\uDDBB(?:\uD83C[\uDFFB-\uDFFF])?|[\uDDBC-\uDDCA]|[\uDDCD-\uDDCF](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|\uDDD0|\uDDD1(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1|\uD83C(?:\uDFFB(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1\uD83C\uDFFB)?|\uDFFC(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB\uDFFC])?|\uDFFD(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFD])?|\uDFFE(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFE])?|\uDFFF(?:\u200D\uD83E\uDD1D\u200D\uD83E\uDDD1\uD83C[\uDFFB-\uDFFF])?))?|[\uDDD2-\uDDD5](?:\uD83C[\uDFFB-\uDFFF])?|\uDDD6(?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDDD7-\uDDDD](?:\u200D[\u2640\u2642]\uFE0F|\uD83C[\uDFFB-\uDFFF](?:\u200D[\u2640\u2642]\uFE0F)?)?|[\uDDDE\uDDDF](?:\u200D[\u2640\u2642]\uFE0F)?|[\uDDE0-\uDDFF\uDE70-\uDE73\uDE78-\uDE7A\uDE80-\uDE82\uDE90-\uDE95])", string.Empty);
	}

	void RemoveLastCharacter()
	{
		
	}


	private bool fontTextureJustRebuilt = true;
	void OnFontTextureRebuilt(Font changedFont){
    		//if (changedFont != font) //ignore if font doesn't exist on this mesh
    		//    return;
    		//Rebuild(currentReadTime, currentReadTime > 0f ? true : autoRead); //the font texture attached to this mesh has changed. a rebuild is neccesary.
    		if(textMesh != null && hyphenedText.Length > 0 && allFonts.Contains(changedFont))
    		{ 
    			fontTextureJustRebuilt = true;
    			//2018-05-29: only rebuild if this mesh actually contains the font!
    			//RebuildTextInfo();
    			
    			/*
    			//efficient, but rarely this causes text to just... not render
    			GetCharacterInfoForArray();
    			//update mesh
    			SetMesh(currentReadTime);
    			*/
    
    			var newUnreadTime = currentUnReadTime;
    			if(unreading)
    			{
    				Rebuild(currentReadTime, reading ? true : autoRead);
    				UnRead(newUnreadTime);
    			}
    			else
    			{
    				Rebuild(currentReadTime, reading ? true : autoRead);
    			}
    		}
    	}
	#if UNITY_5_4_OR_NEWER
	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
		if(this != null && t.gameObject.activeInHierarchy && this.enabled)
		{
			StartCoroutine(WaitFrameThenRebuild()); //just do it anyway
		/*
			if(loadSceneMode == LoadSceneMode.Single){
				StartCoroutine(WaitFrameThenRebuild());
			}else{
				Rebuild(autoRead); //otherwise, texture goes missing. miiiight not be neccesary in 5.4+?
			}
		*/
		}
	}
	#else
	void OnLevelWasLoaded(int level){
		StartCoroutine(WaitFrameThenRebuild());
		//Rebuild(autoRead); //otherwise, texture goes missing. 
	}
	#endif
	IEnumerator WaitFrameThenRebuild(){
		yield return null;
		//Rebuild(currentReadTime, reading ? true : autoRead);
		SpecialRebuild();
	}

	void Awake()
	{
		//currentReadTime = 0f;
	}
	
	void SpecialRebuild()
	{
		if(Application.isPlaying)
		{ //cause autoread to work
			if(callReadFunction && rememberReadPosition)
			{ //remember read position?
				if(currentReadTime == 0f)//same as Start()
				{
					//Debug.Log(this.name + "FROM START");
					Rebuild(autoRead || reading);
				}
				else if(currentReadTime >= totalReadTime)
				{ //it finished reading?
					//Debug.Log(this.name + "DONE READING");
					Rebuild(currentReadTime, true, false);
					//SetMesh(currentReadTime); //skip to end w/o events
				}
				else
				{ //it was halfway thru
					//Debug.Log(this.name + "PARTYWAY THRU");
					Rebuild(currentReadTime, autoRead || reading);
				}
			}
			else
			{ //act like Start()
				//Debug.Log(this.name + "NOT READING");
				Rebuild(autoRead);
			}
		}
		else
		{
			//Debug.Log(this.name + "EDITOR");
			Rebuild();
		}
	}
	void OnEnable(){
		Init();
		//Debug.Log(this.name + " " + currentReadTime);
		SpecialRebuild();
	}
	void Start(){
		//textMesh = null; //THIS IS NEEDED TO PREVENT THE DUPLICATION GLITCH
		//2024-04-12 ok, it fixed an editor dupe glitch, but also stops text from animating in builds. 
		//so instead, just initialize the mesh here:
		textMesh = new Mesh(); //create the mesh initially
		textMesh.MarkDynamic();


		//currentReadTime = 0f;
		//Init(); 
		/*
		if(uiMode)
			StartCoroutine(WaitFrameThenRebuild(autoRead));
		else
			Rebuild(autoRead);
		*/
	}
	void OnDisable(){
		//Debug.Log("Disabled!");
		UnInit();
		if(uiMode){
			DestroyImmediate(textMesh);
			c.Clear();
			//c.SetMesh(null);
		}else{
			DestroyImmediate(f.sharedMesh);
		}
	}
	void OnDestroy()
	{
		//UnInit();
	}
	void Init()
	{
		//uiMode = t is RectTransform;

		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded += OnSceneLoaded; //hopefully not an issue if called multiple times?
		#endif
		Font.textureRebuilt += OnFontTextureRebuilt;
		#if UNITY_EDITOR
		Undo.undoRedoPerformed += OnUndoRedo;
		#endif
	}
	void UnInit()
	{
		#if UNITY_5_4_OR_NEWER
		SceneManager.sceneLoaded -= OnSceneLoaded;
		#endif
		Font.textureRebuilt -= OnFontTextureRebuilt;
		#if UNITY_EDITOR
		Undo.undoRedoPerformed -= OnUndoRedo;
		#endif
		StopReadRoutine();
	}
	void OnUndoRedo()
	{
		Rebuild();
	}
	void StopReadRoutine(){
		reading = false;
		if(readRoutine != null)
		{
			StopCoroutine(readRoutine); //stop routine, just in case
		}
	}
	/*
	private void AddRebuildCall(){
		if(Font.textureRebuilt == null || !Font.textureRebuilt.GetInvocationList().Contains(OnFontTextureRebuilt)){
			Font.textureRebuilt += OnFontTextureRebuilt;
		}
	}
	private void RemoveRebuildCall(){
		Font.textureRebuilt -= OnFontTextureRebuilt;
	}
	*/
	#if UNITY_EDITOR //just for updating when the editor changes
	internal bool validateAppearance = false;
	#endif
	public void OnValidate() {
		//legacy support, for updating color storage...
		if(_color32 != Color.white)
		{
		//	Debug.Log("Converted old Color32 value on '" + t.name + "' to Color!"); //works so well you don't even need the debug, apparently!
			_color = _color32;
			_color32 = Color.white;
		}
		if(font != null && !font.dynamic)
		{
			if(font.fontSize > 0){
				quality = font.fontSize;
			}else{
				Debug.Log("You're probably using a custom font! \n Unity's got a bug where custom fonts have their size set to 0 by default and there's no way to change that! So to avoid this error, here's a solution: \n * Drag any font into Unity. Set it to be 'Unicode' or 'ASCII' in the inspector, depending on the characters you want your font to have. \n * Set 'Font Size' to whatever size you want 'quality' to be locked at. \n * Click the gear in the corner of the inspector and 'Create Editable Copy'. \n * Now, under the array of 'Character Rects', change size to 0 to clear everything. \n * Now you have a brand new font to edit that has a font size that's not zero! Yeah!");
			}
			//quality = UseThisFont.fontSize == 0 ? 64 : UseThisFont.fontSize; //for getting around fonts with a default size of 0.
			//Debug.Log("Font size is..." + UseThisFont.fontSize);
			style = FontStyle.Normal;
		}
		if(size < 0.0001f){size = 0.0001f;}
		if(readDelay < 0f){readDelay = 0f;}
		if(autoWrap < 0f){autoWrap = 0f;}
		if(verticalLimit < 0f){verticalLimit = 0f;}
		if(minPitch > maxPitch){minPitch = maxPitch;}
		if(maxPitch < minPitch){maxPitch = minPitch;}
		if(speedReadScale < 0.01f){speedReadScale = 0.01f;}
/*
		if(validateMesh)
		{
			#if UNITY_EDITOR
			validateAppearance = true;
			#endif
		}
*/
		#if UNITY_EDITOR
		validateAppearance = true;
		#endif
	}
	#if UNITY_EDITOR
	public void HideInspectorStuff(){
		HideFlags flag = HideFlags.HideInInspector;
		switch(debugMode){
			case true: flag = HideFlags.None; break;//don't hide!
		}
		if(uiMode){
			for(int i=0, iL=c.materialCount; i<iL; i++){
				if(c.GetMaterial(i) != null){
					c.GetMaterial(i).hideFlags = flag;
				}
			}
			c.hideFlags = flag;
		}else{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++){ //hide shared materials
				if(r.sharedMaterials[i] != null){
					r.sharedMaterials[i].hideFlags = flag;
				}
			}
			r.hideFlags = flag; //hide mesh renderer and filter.
			f.hideFlags = flag;
		}
	}
	#endif

	public void InitializeFont()
	{
		if(uiMode)//UI mode
		{ 
			if(font == null && textMaterial == null)
			{
				size = 32;
				color = new Color32(50,50,50,255);
			}
			if(textMaterial == null)
			{
				//textMaterial = (Material)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/DefaultSTMMaterials/UIDefault.mat", typeof(Material));
				textMaterial = Resources.Load<Material>("DefaultSTMMaterials/UIDefault");
				//sticking with resource folder since it works in builds
			}
			if(font == null)
			{
				if(data.defaultFont != null)
				{
					font = data.defaultFont;
				}
				else
				{
					font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
			}
		}
		else
		{
			if(font == null)
			{
				if(data.defaultFont != null)
				{
					font = data.defaultFont;
				}
				else
				{
					font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				}
			}
			if(textMaterial == null){
				//sticking w/ resource folder since it works in builds
				textMaterial = Resources.Load<Material>("DefaultSTMMaterials/Default");
				//textMaterial = (Material)AssetDatabase.LoadAssetAtPath(STMCustomInspectorTools.ClavianPath + "Resources/DefaultSTMMaterials/Default.mat", typeof(Material));
			}
		}
	}
	public static void RebuildAll(){ //this uses FindObjectsOfType and is very intensive! Only use when loading.
		SuperTextMesh[] allSTMs = FindObjectsOfType<SuperTextMesh>();
		for(int i=0, iL=allSTMs.Length; i<iL; i++){
			allSTMs[i].Rebuild();
		}
	}
	public void Rebuild(){
		Rebuild(0f, autoRead);
	}
	public void Rebuild(bool readAutomatically){
		Rebuild(0f, readAutomatically);
	}
	public void Rebuild(float startTime){
		Rebuild(startTime, true);
	}
	public void Rebuild(float startTime, bool readAutomatically)
	{
		Rebuild(startTime, readAutomatically, true);
	}
	private bool doEvents = true;
	private bool currentlyRebuilding = false;
	private static int myColorSpace = -1; //-1 = uninitialized, 0 = gamma, 1 = linear.
	//private bool fontTextureRebuildRequest = false;
	public void Rebuild(float startTime, bool readAutomatically, bool executeEvents)
	{
		if(currentlyRebuilding)
		{
			return; //don't rebuild
		}
		currentlyRebuilding = true;
		//fontTextureRebuildRequest = fontTextureJustRebuilt;
		//if(uiMode) 
		doEvents = executeEvents;
		//if(uiMode) Canvas.ForceUpdateCanvases(); //so that everything gets set correctly on awake/start
		//if(uiMode) LayoutRebuilder.MarkLayoutForRebuild(tr);
		//if(uiMode) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tr.root);
		//if(uiMode && tr.parent != null) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tr.parent);
		
		//together with OnRectTransformDimensionsChange() should work...
		if(uiMode) LayoutRebuilder.MarkLayoutForRebuild(tr);
		//apply automatic quality
		if(uiMode && autoQuality)
		{
			parentCanvas = t.GetComponentInParent<Canvas>();
			if(parentCanvas != null)
			{
				quality = (int)Mathf.Ceil(size * parentCanvas.scaleFactor);
			}
			else
			{
				quality = (int)Mathf.Ceil(size);
			}
		}
		//if(uiMode) LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)tr.root);
		if(!fontTextureJustRebuilt)
		{
			if(onRebuildEvent != null && onRebuildEvent.GetPersistentEventCount() > 0) onRebuildEvent.Invoke(); //is it better to just check for null???
			if(OnRebuildEvent != null) OnRebuildEvent();
		}
		if(startTime < totalReadTime)
		{
			_pauseCount = 0; //only reset if reading from the very start, not appending
		} 
		
		autoPauseStopPoint = 0f;
		_currentPauseCount = 0;
		timeDrawn = GetTime - startTime < 0f ? 0f : startTime; //remember what time it started! (or would have started)
		currentReadTime = startTime;
		totalReadTime = 0f;
		reading = false;
		unreading = false; //reset this, incase it was fading out
		speedReading = false; //2016-06-09 thank u drak
		skippingToEnd = false;
		//Initialize:

		//color space:
		#if UNITY_EDITOR
		if(PlayerSettings.colorSpace == ColorSpace.Linear)
		{
			myColorSpace = 1;
		}
		else if(PlayerSettings.colorSpace == ColorSpace.Gamma)
		{
			myColorSpace = 0;
		}
		#endif
		
		InitializeFont();
		RebuildTextInfo();
		if(audioSource != null)//initialize an audio source, if there's one. these settings are needed to behave properly
		{
			audioSource.loop = false;
			audioSource.playOnAwake = false;
		}
		if(callReadFunction && Application.isPlaying){
			if(readAutomatically){
				Read(startTime);
			}else{
				//StopReadRoutine();
				SetMesh(0f); //show nothing
			}
		}
		else
		{
			StopReadRoutine();
			//SetMesh(-1f); //skip to end
			ShowAllText(false, true);
		}
		ApplyMaterials();
		currentlyRebuilding = false;
/*
		//if a font texture rebuild was requested somewhere within this loop...
		if(fontTextureRebuildRequest == false && fontTextureJustRebuilt)
		{ 
			//do the same kind of rebuild as in OnFontTextureRebuilt
			Rebuild(currentReadTime, currentReadTime > 0f ? true : autoRead);
		}
*/
		fontTextureJustRebuilt = false;
	}
	#if UNITY_EDITOR
	private Transform Validate_lastParent = null;
	//private float Validate_lastWidth = 0f;
	//private float Validate_lastHeight = 0f;
	#endif
	internal void Update() {
		
		#if UNITY_EDITOR
		//not needed anymore because of UI callback
		//if(uiMode && (tr.rect.width != Validate_lastWidth || tr.rect.height != Validate_lastHeight))
		{
		//	Validate_lastWidth = tr.rect.width;
		//	Validate_lastHeight = tr.rect.height;
		//	Rebuild(autoRead);
		}
		//force validate if transform changes
		if(t.parent != Validate_lastParent)
		{
			validateAppearance = true;
			Validate_lastParent = t.parent;
		}
		//i hope this validateMesh call doesn't break anything!!!! 2019/10/20

			//2019.04.22 hope making this rquire validate appearance doesn't break anything...
			//added a null check cause stuff was breaking in older unity versions...?
		if(!Application.isPlaying && validateAppearance && t != null){ //do same thing as onvalidate
			Rebuild(autoRead); //doing it this way avoids the material getting lost when duplicating
			//Debug.Log("rebuilding");
			validateAppearance = false;
		}


		if(validateAppearance && t.gameObject.activeInHierarchy == true && Application.isPlaying){ 
			SpecialRebuild();
			validateAppearance = false;
		}
		#endif
		
		if(font != null && textMaterial != null && textMesh != null){ //TODO: make this only get called if something changed, or it's animating
			//RequestAllCharacters(); //keep characters on font texture 
			//v1.4.2: not sure if neccesary, thanks to OnFontTextureRebuilt()?
			//but I'm not sure. it does seem to use a bit of CPU for more meshes, though
			if(!reading && (areWeAnimating || forceAnimation) && currentReadTime >= totalReadTime){
				currentReadTime += GetDeltaTime; //keep updating this, for the jitters
			}
			if(!reading && !unreading && (areWeAnimating || forceAnimation) && (readDelay == 0f || currentReadTime >= totalReadTime)){ //if the mesh needs to keep updating after it's been read out
				//if(currentReadTime >= totalReadTime){ //as long as it's set to auto read, or the current read time is above total read time
					//Debug.Log(currentReadTime + "/" + totalReadTime);
				SetMesh(-1f);
				//}
			}
		}
	}
	private STMTextInfo UpdateMesh_info;
	void UpdatePreReadMesh(bool undrawingMesh){ //update data needed for pre-existing mesh
		UpdateMesh(0f);

		int arraySize = hyphenedText.Length * 4;

		if (startCol32.Length != arraySize)
			Array.Resize(ref startCol32, arraySize);

		if (startVerts.Length != arraySize)
			Array.Resize(ref startVerts, arraySize);

		STMDrawAnimData myUndrawAnim = UndrawAnim; //just in case...
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			UpdateMesh_info = info[i];
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : UpdateMesh_info.drawAnimData; //which animation data to use?

			if(UpdateMesh_info.drawAnimData.startColor != Color.clear){ //use a specific start color
				startCol32[4*i + 0] = myAnimData.startColor;
				startCol32[4*i + 1] = myAnimData.startColor;
				startCol32[4*i + 2] = myAnimData.startColor;
				startCol32[4*i + 3] = myAnimData.startColor;
			}else{ //same color but transparent, for better lerping
				startCol32[4*i + 0].r = endCol32[4*i + 0].r;
				startCol32[4*i + 0].g = endCol32[4*i + 0].g;
				startCol32[4*i + 0].b = endCol32[4*i + 0].b;
				startCol32[4*i + 0].a = 0;

				startCol32[4*i + 1].r = endCol32[4*i + 1].r;
				startCol32[4*i + 1].g = endCol32[4*i + 1].g;
				startCol32[4*i + 1].b = endCol32[4*i + 1].b;
				startCol32[4*i + 1].a = 0;

				startCol32[4*i + 2].r = endCol32[4*i + 2].r;
				startCol32[4*i + 2].g = endCol32[4*i + 2].g;
				startCol32[4*i + 2].b = endCol32[4*i + 2].b;
				startCol32[4*i + 2].a = 0;

				startCol32[4*i + 3].r = endCol32[4*i + 3].r;
				startCol32[4*i + 3].g = endCol32[4*i + 3].g;
				startCol32[4*i + 3].b = endCol32[4*i + 3].b;
				startCol32[4*i + 3].a = 0;
			}
			Vector3 middle = new Vector3((endVerts[4*i + 0].x + endVerts[4*i + 1].x + endVerts[4*i + 2].x + endVerts[4*i + 3].x) * 0.25f,
														(endVerts[4*i + 0].y + endVerts[4*i + 1].y + endVerts[4*i + 2].y + endVerts[4*i + 3].y) * 0.25f,
														(endVerts[4*i + 0].z + endVerts[4*i + 1].z + endVerts[4*i + 2].z + endVerts[4*i + 3].z) * 0.25f);
			
			startVerts[4*i + 0] = Vector3.Scale((endVerts[4*i + 0] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * UpdateMesh_info.size.y);
			startVerts[4*i + 1] = Vector3.Scale((endVerts[4*i + 1] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * UpdateMesh_info.size.y);
			startVerts[4*i + 2] = Vector3.Scale((endVerts[4*i + 2] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * UpdateMesh_info.size.y);
			startVerts[4*i + 3] = Vector3.Scale((endVerts[4*i + 3] - middle), myAnimData.startScale) + middle + (myAnimData.startOffset * UpdateMesh_info.size.y);
		}
	}
	public void Read(){
		Read(0f);
	}
	public void Read(float startTime){
		StopReadRoutine();
		readRoutine = StartCoroutine(ReadOutText(startTime));
		//now we have a mesh with nothing on it!
	}
	//cause I keep accidentally typing this? I think this name is better, might swap this in the future
	public void Unread(float startUnReadTime = 0f){UnRead(startUnReadTime);}
	public void Undraw(float startUnReadTime = 0f){UnRead(startUnReadTime);}
	public void UnDraw(float startUnReadTime = 0f){UnRead(startUnReadTime);}
	public void UnRead(float startUnReadTime = 0f){
		//Mesh finalMesh = ShowAllText(); //this is working
		readRoutine = StartCoroutine(UnReadOutText(startUnReadTime));
	}
	public void SpeedRead(){
		if(reading){
			speedReading = true;
		}
	}
	public void SkipToEnd(){
		if(reading){
			skippingToEnd = true;
		}
	}
	public void RegularRead(){ //return to normal reading speed
		speedReading = false;
	}
	public void ShowAllText(){
		ShowAllText(false, false); //actually show all text
	}
	private bool wasReadingBefore = false;
	private void ShowAllText(bool unreadingMesh, bool forceCompleteEvent){
		speedReading = false;
		if(unreadingMesh)
		{
			unreading = false; //set to false for the SetMesh() call
			if(currentUnReadTime < totalUnreadTime) currentUnReadTime = totalUnreadTime;
		}
		else
		{
			if(currentReadTime < totalReadTime) currentReadTime = totalReadTime; //this if statement fixes animatefromtimedrawn waves from animating a second time upon OnFontTextureRebuilt
		}
		
		//furthestDrawnPosition = rawBottomRightTextBounds.x;
		//lowestLine = lineBreaks.Count-1;
		wasReadingBefore = reading;
		//make sure reading is true before this is called or else it will read every event
		SetMesh(unreadingMesh ? totalUnreadTime : totalReadTime, unreadingMesh);
		StopReadRoutine();
		//Invoke complete events:
		if(!unreadingMesh){
			
			//if(leftoverText.Length > 0f){ //set leftover text, if any
			//	Debug.Log(leftoverText);
			//}
			if(wasReadingBefore || forceCompleteEvent)
			{
				if(onCompleteEvent != null) onCompleteEvent.Invoke();
				if(OnCompleteEvent != null) OnCompleteEvent();
			}
		}else{
			//unreading = false; //nope! Gotta stay in this state til it gets drawn again
			unreading = true;
			if(onUndrawnEvent != null) onUndrawnEvent.Invoke();
			if(OnUndrawnEvent != null) OnUndrawnEvent();
		}
	}
	public void Append(string newText){
		_text += newText;
		Rebuild(totalReadTime, true); //start reading from this point
	}
	public bool Continue(){ //continue reading after being paused. returns true if more text needs to be read
		//goes one extra so that true/false is returned properly
		if(currentPauseCount > pauseCount){ //still need to continue?
			_pauseCount++;
			Rebuild(totalReadTime, true);
			return true;
		}
		return false;
	}
	public bool UndoContinue(){ //continue reading after being paused. returns true if more text needs to be read
		if(pauseCount > 0) //is able to go back
		{
			//pause count gets reset when rebuilding from the start, so...
			int tmpPauseCount = pauseCount - 1;
			Rebuild(0f, true); //start at beginning, then...
			for(int i=0; i<tmpPauseCount; i++)
			{
				Continue();
			}
			return true;
		}
		return false;
	}

	//int lastNum = -1; //the last index to be invoked on the previous cycle, so sounds can't play twice for the same letter!
	//List<int> alreadyInvoked = new List<int>(); //list on indexes that have already been invoked so events cant happen twice
	public int latestNumber = -1; //declare here as a public variable, so the current character drawn can be reached at any time
	void UpdateDrawnMesh(float myTime, bool undrawingMesh){
		UpdateMesh(myTime);
		UpdatePreReadMesh(undrawingMesh);
		//TODO: ^^^ all this stuff, you only have to call again if areweanimating is true.

		STMDrawAnimData myUndrawAnim = UndrawAnim; //get the undraw animation, locally
		//get modified drawnMesh!
		int arraySize = hyphenedText.Length * 4;

		if (midVerts.Length != arraySize)
			Array.Resize(ref midVerts, arraySize);

		if (midCol32.Length != arraySize)
			Array.Resize(ref midCol32, arraySize);

		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each point
			UpdateMesh_info = info[i];
			//lerp between start and end
			//Debug.Log((myTime - UpdateMesh_info.readTime) / UpdateMesh_info.animTime);
			STMDrawAnimData myAnimData = undrawingMesh ? myUndrawAnim : UpdateMesh_info.drawAnimData; //which anim to use
			float myReadTime = undrawingMesh ? UpdateMesh_info.unreadTime : UpdateMesh_info.readTime; //what timing to use
			//animate properly! (is there a way to do this by manipulating anim time?? yeah probably tbh)
			float divideAnimAmount = myAnimData.animTime == 0f ? 0.0000001f : myAnimData.animTime; //so it doesn't get NaN'd
			float divideFadeAmount = myAnimData.fadeTime == 0f ? 0.0000001f : myAnimData.fadeTime; //how long fading will last...
			float myAnimPos = (myTime - myReadTime) / divideAnimAmount; // on a range between 0-1 on the curve, the position of the animation
			float myFadePos = (myTime - myReadTime) / divideFadeAmount;

			if(undrawingMesh){ //flip the range! so it lerps backwards
				myAnimPos = 1f - myAnimPos;
				if(myAnimData.fadeTime == 0f)
				{
					myFadePos = 1f;
				}
				else
				{
					myFadePos = 1f - myFadePos;
				}
			}
			/*
			It'd be more efficient to update only vertices that need to be updated.
			However... this isn't done, because it looks bad when a mesh gets laggier as it goes on.
			*/
			midVerts[4*i+0] = LerpWithoutClamp(startVerts[4*i+0],endVerts[4*i+0],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+1] = LerpWithoutClamp(startVerts[4*i+1],endVerts[4*i+1],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+2] = LerpWithoutClamp(startVerts[4*i+2],endVerts[4*i+2],myAnimData.animCurve.Evaluate(myAnimPos));
			midVerts[4*i+3] = LerpWithoutClamp(startVerts[4*i+3],endVerts[4*i+3],myAnimData.animCurve.Evaluate(myAnimPos));

			midCol32[4*i+0] = Color.Lerp(startCol32[4*i+0],endCol32[4*i+0],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+1] = Color.Lerp(startCol32[4*i+1],endCol32[4*i+1],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+2] = Color.Lerp(startCol32[4*i+2],endCol32[4*i+2],myAnimData.fadeCurve.Evaluate(myFadePos));
			midCol32[4*i+3] = Color.Lerp(startCol32[4*i+3],endCol32[4*i+3],myAnimData.fadeCurve.Evaluate(myFadePos));
		}
		//ShouldPlaySound(undrawingMesh);
	}
	/*
	void ShouldPlaySound(bool undrawingMesh){
		//int alreadyInvokedCount = alreadyInvoked.Count;
		if(!undrawingMesh){ //dont play sounds if undrawing...
			if(latestNumber != lastNum){ //new number?
				PlaySound(latestNumber); //only play one sound, from the most recent number
				lastNum = latestNumber;
			}
		}else{
			lastNum = -1;
		}
	}
	*/
	Vector3 LerpWithoutClamp(Vector3 A, Vector3 B, float t)
	{
		return A + (B-A)*t;
	}
	bool AreColorsTheSame(Color32 col1, Color32 col2){
		if(col1.r == col2.r && col1.g == col2.g && col1.b == col2.b && col1.a == col2.a){
			return true;
		}
		return false;
	}
	IEnumerator ReadOutText(float startTime){
		//Lerp certain vertices betwwen startmesh and endmesh
		//like, the mesh made by CreatePreReadMesh() and CreateMesh().
		reading = true;
		//float timer = startTime;
		currentReadTime = startTime;		
		if(startTime.Equals(0f)){ //for append()
//			alreadyInvoked.Clear();
			//lastNum = -1;
			latestNumber = -1; //reset to -1, not 0. This would stop first character from making a sound/playing an event (2018-05-29)
			lowestDrawnPosition = 0f;
			lowestDrawnPositionRaw = 0f;
			furthestDrawnPosition = 0f;
			//currentMaxX = 0f;
			//currentMinY = 0f;
			//lowestLine = 0;
		}

		for(int i=0; i<infoCount; i++)
		{
			if(i > latestNumber)
			{
				info[i].invoked = false;
			}
		}
		/*
		else if(startTime.Equals(-1f)){
			DoEvent(0);	//so 1st event doesn't get skipped
			2018-05-29 this shouldn't be a problem anymore because of the above
		}
		*/
		//Debug.Log("Total read time is: " + totalReadTime);
		while(currentReadTime < totalReadTime){ //check for null incase the mesh gets deleted midway
			//Debug.Log("Doing this while loop!" + timer);
			float delta = GetDeltaTime2;
			delta *= speedReading ? speedReadScale : 1f;
			//timer += delta;
			//currentReadTime = timer; //I could just use this as the timer, but w/e
			currentReadTime += delta;
			if(skippingToEnd){
				//timeDrawn -= totalReadTime; //why not (solves jitters not catching up)
				//timer = totalReadTime;
				currentReadTime = totalReadTime;
			}
			SetMesh(currentReadTime);
			
			
			yield return null;
		}
		if(latestNumber != hyphenedText.Length-1){ //catch missed events
			PlaySound(hyphenedText.Length-1); //play final sound! Yep, this seems to fix that 2016-10-13
			DoEvent(hyphenedText.Length-1); //2016-11-02 but only if it hasn't been played yet
		}
		ShowAllText(); //just in case!
	}

	public float currentUnReadTime = 0f;
	IEnumerator UnReadOutText(float startUnReadTime = 0f){
		unreading = true;
		currentUnReadTime = startUnReadTime; //always start at 0
		while(currentUnReadTime < totalUnreadTime){ //check for null incase the mesh gets deleted midway
			SetMesh(currentUnReadTime, true);
			currentUnReadTime += GetDeltaTime2;
			yield return null;
		}
		ShowAllText(true, false);
	}
	/*
	bool IsThisLetterAnimating(int i){ //return true if this letter is animating in some way, not related to drawanim
		if(info[i].waveData != null || info[i].jitterData != null &&
			(info[i].gradientData != null && info[i].gradientData.scrollSpeed != 0) ||
			(info[i].textureData != null && info[i].textureData.scrollSpeed != Vector2.zero)){
			return true;
		}
		return false;
	}
	*/
	private STMTextInfo DoEvent_info;
	void DoEvent(int i){
		if(doEvents)
		{
			DoEvent_info = info[i];
			if(DoEvent_info.ev.Count > 0){ //invoke events...
				for(int j=0, jL=DoEvent_info.ev.Count; j<jL; j++){
					if(onCustomEvent != null) onCustomEvent.Invoke(DoEvent_info.ev[j], DoEvent_info); //call the event!
					if(OnCustomEvent != null) OnCustomEvent.Invoke(DoEvent_info.ev[j], DoEvent_info);
				}
				DoEvent_info.ev.Clear(); //since you only want events to be invoked once, I don't see the harm in clearing them this way instead of keeping track
			}
			if(DoEvent_info.ev2.Count > 0){ //end repeating events!
				for(int j=0, jL=DoEvent_info.ev2.Count; j<jL; j++){
					if(onCustomEvent != null) onCustomEvent.Invoke(DoEvent_info.ev2[j], DoEvent_info); //call the event!
					if(OnCustomEvent != null) OnCustomEvent.Invoke(DoEvent_info.ev2[j], DoEvent_info);
				}
				DoEvent_info.ev2.Clear();
			}
		}
	}
	/*
	char NameToSpecialKey(string name){ //for getting specific autoclips. for things that cant be used in file names
		switch(name.ToLower()){ //also for autodelays
			case "space": return ' ';
			case "tab": return '\t';
			case "line break": case "linebreak": return '\n';
			case "exclamation": case "exclamationpoint": case "exclamation point": return '!';
			case "question": case "questionmark": case "question mark": return '?';
			case "semicolon": return ';';
			case "colon": return ':';
			case "tilde": return '~';
			case "period": return '.';
			case "comma": return ',';
			case "number sign": case "hashtag": return '#';
			case "percent": case "percentsign": case "percent sign": return '%';
			case "ampersand": return '&';
			case "asterix": return '*';
			case "backslash": return '\\';
			case "openbrace": case "open brace": return '{';
			case "closebrace": case "close brace": return '}';
			default: return name[0];
		}
	}
	*/
	/*
	string SpecialKeyToName(char ch){ //since you can't put these in filenames
		switch(ch){
			case ' ': return "space";
			case '\t': return "tab";
			case '\n': return "line break";
			case '!': return "exclamation point";
			case '?': return "question mark";
			case ';': return "semicolon";
			case ':': return "colon";
			case '~': return "tilde";
			case '.': return "period";
			case ',': return "comma";
			case '#': return "number sign";
			case '%': return "percent";
			case '&': return "ampersand";
			case '*': return "asterix";
			case '\\': return "backslash";
			case '/': return "forwardslash";
			case '{': return "openbrace";
			case '}': return "closebrace";
			default: return new string(ch,1).ToLower(); //always ignore case
		}
	}
	*/
	private STMTextInfo PlaySound_info;
	public virtual void PlaySound(int i){
		if(audioSource != null){//audio stuff
			PlaySound_info = info[i];
			if(PlaySound_info.stopPreviousSound || !audioSource.isPlaying){
				audioSource.Stop();
				string nameToSearch = PlaySound_info.isQuad ? PlaySound_info.quadData.name : hyphenedText[i].ToString();
				AudioClip mySoundClip = null;
				if(PlaySound_info.soundClipData != null){
					STMSoundClipData.AutoClip tmpAutoClip = PlaySound_info.soundClipData.clips.Find(x =>
						(x.type == STMSoundClipData.AutoClip.Type.Quad ? x.quadName.ToLower() : x.character.ToString().ToLower()) == nameToSearch); //find auto clip
					
					if(tmpAutoClip != null){
						mySoundClip = tmpAutoClip.clip;
					}
				}
				STMAutoClipData myAutoClip = null;
				if(data.autoClips.ContainsKey(nameToSearch.ToUpper())){ //case
					myAutoClip = data.autoClips[nameToSearch.ToUpper()];
				}else if(data.autoClips.ContainsKey(nameToSearch)){
					myAutoClip = data.autoClips[nameToSearch];
				}

				if(mySoundClip != null){ //use the one attached to this character
					audioSource.clip = mySoundClip;
				}else if(myAutoClip != null){ //if there's a sound clip for this character in Text Data
					audioSource.clip = myAutoClip.clip;
				}else if(PlaySound_info.audioClipData != null){ //use sounds attached to character
					audioSource.clip = PlaySound_info.audioClipData.clips.Length > 0 ? PlaySound_info.audioClipData.clips[UnityEngine.Random.Range(0,PlaySound_info.audioClipData.clips.Length)] : null;
				}else if(audioClips.Length > 0){ //use a random audio clip
					audioSource.clip = audioClips.Length > 0 ? audioClips[UnityEngine.Random.Range(0,audioClips.Length)] : null; //get one of the clips
				}else{
					audioSource.clip = null;
				}
				if(audioSource.clip != null){
					switch(PlaySound_info.pitchMode){
						case PitchMode.Perlin:
							audioSource.pitch = (Mathf.PerlinNoise(GetTime * perlinPitchMulti, 0f) * (PlaySound_info.maxPitch - PlaySound_info.minPitch)) + PlaySound_info.minPitch; //perlin noise
							break;
						case PitchMode.Random:
							audioSource.pitch = UnityEngine.Random.Range(PlaySound_info.minPitch,PlaySound_info.maxPitch);
							break;
						case PitchMode.Single:
							audioSource.pitch = PlaySound_info.overridePitch;
							break;
						default:
							audioSource.pitch = 1f; //because of speedread pitch
							break;
					}
					if(speedReading){
						audioSource.pitch += PlaySound_info.speedReadPitch;
					}
					audioSource.Play();
				}
			}
		}
	}
	//TODO: 2016-06-11 actually, im guessing that these values are a bitmask? you could prob just add & subtract em! but w/e
	FontStyle AddStyle(FontStyle original, FontStyle newStyle){
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(newStyle){
						case FontStyle.Italic:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(newStyle){
						case FontStyle.Bold:
							return FontStyle.BoldAndItalic;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					return original;
				default: //normal text
					return newStyle;
			}	
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	FontStyle SubtractStyle(FontStyle original, FontStyle subStyle){ //only bold and italic can be added and removed
		if(font.dynamic){
			switch(original){
				case FontStyle.Bold:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.Italic:
					switch(subStyle){
						case FontStyle.Italic:
							return FontStyle.Normal;
						default:
							return original;
					}
				case FontStyle.BoldAndItalic:
					switch(subStyle){
						case FontStyle.Bold:
							return FontStyle.Italic;
						case FontStyle.Italic:
							return FontStyle.Bold;
						default:
							return original; //just in case?
					}
				default: //normal
					return FontStyle.Normal;
			}
		}else{
			return FontStyle.Normal; //non-dynamic fonts can't handle bold/italic
		}
	}
	bool ValidHexcode (string hex){ //check if a hex code contains the right amount of characters, and allowed characters
		if(hex.Length < 3)
		{
			return false;
		}
		if(hex.Substring(0,1) == "#"){
			hex = hex.Substring(1,hex.Length-1); //trim off #
		}
		if(hex.Length != 3 && hex.Length != 4 && hex.Length != 6 && hex.Length != 8){ //hex code, alpha hex
			return false;
		}
		string allowedLetters = "0123456789ABCDEFabcdef";
		for(int i=0; i<hex.Length; i++){
			if(!allowedLetters.Contains(hex[i].ToString(System.Globalization.CultureInfo.InvariantCulture))){
				return false; //invalid string!!!
			}
		}
		return true;
	}
	Color32 HexToColor(string hex){ //convert a hex code string to a color
		if(hex.Substring(0,1) == "#"){
			hex = hex.Substring(1,hex.Length-1); //trim off #
		}
		if(hex.Length == 8){ //RGBA (FF00FF00)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 4){ //single-byte for RGBA (F0F0)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			byte a = byte.Parse(hex.Substring(3,1) + hex.Substring(3,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,a);
		}
		if(hex.Length == 3){ //single-byte for RGB (F0F)
			byte r = byte.Parse(hex.Substring(0,1) + hex.Substring(0,1), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(1,1) + hex.Substring(1,1), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(2,1) + hex.Substring(2,1), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
		else{ //RGB (FF00FF)
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b,255);
		}
	}
	STMColorData GetColor(string myCol){
		if(data.colors.ContainsKey(myCol)){ //check textdata for a named color
			return data.colors[myCol];
		}
		if(ValidHexcode(myCol)){ //might be a hexcode?
			STMColorData thisCol2 = ScriptableObject.CreateInstance<STMColorData>();
			thisCol2.color = HexToColor(myCol);
			return thisCol2;
		}
		//still no?
		STMColorData thisCol = ScriptableObject.CreateInstance<STMColorData>();
		switch(myCol){ //see if it's a default unity color
			case "red": thisCol.color = Color.red; break;
			case "green": thisCol.color = Color.green; break;
			case "blue": thisCol.color = Color.blue; break;
			case "yellow": thisCol.color = Color.yellow; break;
			case "cyan": thisCol.color = Color.cyan; break;
			case "magenta": thisCol.color = Color.magenta; break;
			case "grey": thisCol.color = Color.grey; break;
			case "gray": thisCol.color = Color.gray; break;
			case "black": thisCol.color = Color.black; break;
			case "clear": thisCol.color = Color.clear; break;
			case "white": thisCol.color = Color.white; break;
			default: thisCol.color = color; break; //default color of mesh
		}
		return thisCol;
	}

	private string FlipParagraphs(string myText, bool flipInfo)
	{
		
		//string[] paragraphs = myText.Split('\n'); //keep paragraphs in order, but order of words in paragraph
		List<List<string>> paragraphs = new List<List<string>>(); //list of words in a paragraph
		List<List<List<STMTextInfo>>> infographs = new List<List<List<STMTextInfo>>>();

		List<STMTextInfo> newInfo = new List<STMTextInfo>();

		int paragraphIndex = 0;
		int wordIndex = 0; //current word in line
		paragraphs.Add(new List<string>(){""});
		infographs.Add(new List<List<STMTextInfo>>(){new List<STMTextInfo>()});
		for(int i=0; i<myText.Length; i++) //manually go through every letter in the string, looking for end of paragraph.
		{
			if(myText[i] == '\n')
			{
				paragraphIndex++; 
				wordIndex = 0;
				//add linebreak as a paragraph.
				paragraphs.Add(new List<string>(){"\n"});
				if(flipInfo) infographs.Add(new List<List<STMTextInfo>>(){new List<STMTextInfo>(){info[i]}});

				paragraphIndex++;
				
				paragraphs.Add(new List<string>(){""}); //new paragraph, with one word in it by default.
				if(flipInfo) infographs.Add(new List<List<STMTextInfo>>(){new List<STMTextInfo>()}); //new paragraph, with a new word info.
			}
			else if(myText[i] == ' ') //create a new word.
			{
				//add this space as a word.
				wordIndex++;
				paragraphs[paragraphIndex].Add(" "); //new word, it's a space.
				if(flipInfo) infographs[paragraphIndex].Add(new List<STMTextInfo>(){info[i]}); //new word w space info

				//start new word
				wordIndex++;
				paragraphs[paragraphIndex].Add("");
				if(flipInfo) infographs[paragraphIndex].Add(new List<STMTextInfo>());
			}
			else //add to existing word.
			{
				//Debug.Log(paragraphs[paragraphIndex].Count);
				//Debug.Log(wordIndex);
				if(paragraphIndex < paragraphs.Count && wordIndex < paragraphs[paragraphIndex].Count)
				{
					paragraphs[paragraphIndex][wordIndex] += myText[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
				}
				if(flipInfo && paragraphIndex < infographs.Count && wordIndex < infographs[paragraphIndex].Count)
				{
					infographs[paragraphIndex][wordIndex].Add(info[i]);
				}
			}
		}
		if(flipInfo)
		{
			for(int i=0; i<infographs.Count; i++)
			{
				infographs[i].Reverse();
				for(int j=0; j<infographs[i].Count; j++)
				{
					for(int k=0; k<infographs[i][j].Count; k++)
					{
						newInfo.Add(infographs[i][j][k]);
					}
				}
			}

			if(info.Count == newInfo.Count)
			{
				info = newInfo;
			}
			else
			{
				Debug.Log("Something went wrong with the RTL system. Old info length was " + info.Count + " new length is " + newInfo.Count);
			}
			
		}
		string returnString = "";

		for(int i=0; i<paragraphs.Count; i++)
		{
			paragraphs[i].Reverse();
			for(int j=0; j<paragraphs[i].Count; j++)
			{
				for(int k=0; k<paragraphs[i][j].Length; k++)
				{
					returnString += paragraphs[i][j][k];
				}
			}
		}

		if(myText.Length == returnString.Length)
		{
			return returnString;
		}
		else
		{
			Debug.Log("Something went wrong with the RTL system. Old info length was " + info.Count + " new length is " + newInfo.Count);
			return myText;
		}
	}
	
	public string preParsedText = "";
	private STMTextInfo ParseText_info = new STMTextInfo();
	private string[] ParseText_dividedString;
	private string ParseText_myString;
	private string ParseText_myTag;
	string ParseText(string myText)
	{
		//return a cleaned up string and update textinfo!
		info.Clear(); //no more... we cache now

		preParsedText = myText; //save it anyway
		if((onPreParse != null && onPreParse.GetPersistentEventCount() > 0) || OnPreParse != null)
		{
			STMTextContainer tempContainer = new STMTextContainer(myText);
			if(onPreParse != null) onPreParse.Invoke(tempContainer);
			if(OnPreParse != null) OnPreParse.Invoke(tempContainer);
			myText = tempContainer.text;
			preParsedText = tempContainer.text; //remember this state for later
		}
		if(removeEmoji)
		{
			myText = RemoveEmoji(myText); //only remove after preparsing
		}
		//remove extra space character! hopefully this doesn't cause any issues... 
		//it seems to fix line breaks happening much earlier than expected with additional character spacing. 2021-12-06
		//myText = myText.Replace("\r", string.Empty);
/*
		if(rtl)
		{
			myText = FlipParagraphs(myText, false);
		}
*/
		

		//resize list to fit
		//if (info.Count < myText.Length)
		//	info = new List<STMTextInfo>(new STMTextInfo[myText.Length+1]);

		//set defaults:
		//STMTextInfo myInfo = new STMTextInfo(this); //info for this one character, carried over from last
		ParseText_info.SetValues(this);
		allTags.Clear();
		int deletedChars = 0; //for figuring out rawindex
		string insertAfter = "";
		infoCount = 0;
		for(int i=0; i<myText.Length; i++)
		{ //for each character to parse thru,
			if(readDelay > 0f)
			{
				if(infoCount == i && i > 0)
				{ //no other delay yet...? /hasnt checkedAgain yet
					//if a quad got put in last time...
					if(info[i-1].isQuad)
					{
						//no delay data set yet and quad name is a registered autodelay?
						if(data.autoDelays.ContainsKey(info[i-1].quadData.name))
						{
							//put delay on next char\
							//for quads, use the data no matter what
							ParseText_info.delayData = data.autoDelays[info[i-1].quadData.name];
						}
					}
					else if(data.autoDelays.ContainsKey(myText[i-1].ToString()))
					{
						var thisChar = myText[i - 1];
						var nextChar = myText[i];
						var thisDelay = data.autoDelays[thisChar.ToString()];
						switch(thisDelay.ruleset)
						{
							case STMAutoDelayData.Ruleset.FollowedBySpace:
								if((nextChar == ' ' || nextChar == '\n' || nextChar == '\t' || //if it's a "space"
								    (myText.Length - i > 4 && myText.Substring(i, 4) == "<br>")))//only if next character is "free". So strings like "Oh......... okay." only see the last delay on periods!
								{
									ParseText_info.delayData = thisDelay;
								}
								break;
							case STMAutoDelayData.Ruleset.FollowedByDifferentCharacter:
								if(nextChar != thisChar) //only if next character is different. good for slow ellipsis or commas
								{ 
									ParseText_info.delayData = thisDelay;
								}
								break;
							case STMAutoDelayData.Ruleset.FollowedBySameCharacterOrSpace:
								if((nextChar == thisChar || nextChar == ' ' || nextChar == '\n' || nextChar == '\t' || 
								         ( myText.Length - i > 4 && myText.Substring(i,4) == "<br>" ))) //only if next character is "free". So strings like "Oh......... okay." only see the last delay on periods!
								{ 
									ParseText_info.delayData = thisDelay;
								}
								break;
							default:
								ParseText_info.delayData = thisDelay;
								break;
						}
						
					}
				}
			}
			if(myText[i] == '\n')
			{
				ParseText_info.isEndOfParagraph = true;
			}
			else
			{
				ParseText_info.isEndOfParagraph = false;
			}
			bool checkAgain = false;
			if(richText && myText[i] == '<'){ //check for count so a pointless debug doesn't appear on rebuild
				int closingIndex = myText.IndexOf(">",i); 
				int equalsIndex = closingIndex > -1 ? myText.IndexOf("=",i, closingIndex-i) : -1; //only look forward for a specific amount of characters
				//Get either closing index or squals index, depending on the kinda tag:
				int endIndex = (equalsIndex > -1 && closingIndex > -1) ? Mathf.Min(equalsIndex,closingIndex) : closingIndex;//for figuring out what the "tag" is
				if(closingIndex != -1){//don't do anything if there's no closing tag at all
					ParseText_myTag = myText.Substring(i, endIndex-i+1); //this is the "TAG" like "<c=" or "<br>"
					//only if there's for sure a string TO get
					//Debug.Log("Index is " + i + " equals index is " + equalsIndex + " closing index is " + closingIndex);
					ParseText_myString = equalsIndex > -1 ? myText.Substring(equalsIndex+1,closingIndex-equalsIndex-1) : "";//this is the "STRING" like "fire" or "blue"
					//Debug.Log("Found this tag: '" + myTag + "'! And this string: '" + myString + "'.");
					bool clearAfter = true;
					bool exitLoopAfter = false;
					insertAfter = ""; //reset
					switch(ParseText_myTag){
					//Line Break
						case "<br>":
							insertAfter = '\n'.ToString(System.Globalization.CultureInfo.InvariantCulture); //insert a line break
							break;
					//Color
						case "<c=":
							ParseText_info.colorData = null; //clear to default
							ParseText_info.gradientData = null;
							ParseText_info.textureData = null;

							ParseText_dividedString = ParseText_myString.Split(','); //divide it up at commas

							for(int d=0; d<ParseText_dividedString.Length; d++)
							{
								//Debug.Log(ParseText_dividedString[d]);

								if(data.textures.ContainsKey(ParseText_dividedString[d])){// is this a texture?
								
									ParseText_info.textureData = data.textures[ParseText_dividedString[d]];
									ParseText_info.submeshChange = true;
									continue;
									//AddMaterial(ParseText_info.) add material here
								}
								//is it a custom color tag?
								if(data.gradients.ContainsKey(ParseText_dividedString[d])) //is this a gradient?
								{
									ParseText_info.gradientData = data.gradients[ParseText_dividedString[d]];
									continue;
								}
								//no? try for HEX code & default color
								ParseText_info.colorData = GetColor(ParseText_dividedString[d]);
							}
							break;
						case "</c>":
							ParseText_info.colorData = null; //clear to default
							ParseText_info.gradientData = null;
							if(ParseText_info.textureData != null)
							{
								ParseText_info.submeshChange = true;
							}
							ParseText_info.textureData = null;
							break;
					//Size
						case "<s=":
							float thisSize;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize)){ //parse as a float
								ParseText_info.size.x = thisSize * size; //set size relative to the one set in inspector!
								ParseText_info.size.y = thisSize * size;
							}
							break;
						case "<size=":
							float thisSize2;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSize2)){ //parse as a float
								ParseText_info.size.x = thisSize2; //set size directly!
								ParseText_info.size.y = thisSize2;
							}
							break;
						case "</s>":
						case "</size>":
							ParseText_info.size.x = size;
							ParseText_info.size.y = size;
							break;
					//Delay
						case "<d=":
							if(data.delays.ContainsKey(ParseText_myString)){ //is there a delay defined in textdata?
								ParseText_info.delayData = data.delays[ParseText_myString];//NOTE: delays get overridden, not added
							}else{ //see if it's an integer
								int thisDelay2;
								if(int.TryParse(ParseText_myString, out thisDelay2)){ //parse as an int
									ParseText_info.delayData = ScriptableObject.CreateInstance<STMDelayData>(); //create new delay data
									ParseText_info.delayData.count = thisDelay2;
								}
							}
							break;
						case "<d>":
							if(data.delays.ContainsKey("default")){ //is there a delay defined?
								ParseText_info.delayData = data.delays["default"];
							}else{
								Debug.Log("Default delay isn't defined!");
							}
							break;
					//Timing
						case "<t=":
							float thisTiming;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisTiming)){ //parse as a float
								if(thisTiming < 0f) thisTiming = 0f; //or else it'll cause a loop!
								ParseText_info.readTime = thisTiming; //set time to be this float
							}
							break;
					//Event
						case "<e=":
							ParseText_info.ev.Add(ParseText_myString); //remember the event!
							break;
					//Repeating Event
						case "<e2=":
							ParseText_info.ev2.Add(ParseText_myString); //remember the event!
							break;
						case "</e>":
						case "</e2>":
							ParseText_info.ev2.Clear(); //forget all. Kinda janky, but whatever. It'll do for now!
							break;
					//Voice
						case "<v=":
							if(data.voices.ContainsKey(ParseText_myString)){
								insertAfter = data.voices[ParseText_myString].text; //add the text to the string!
							}
							break;
						case "</v>":
							//ParseText_info.voiceData = null; //forget it!
							//return everything to default.
							ParseText_info = new STMTextInfo(this);
							break;
					//Font
						case "<f=": //this switches the font for the whole mesh, but I might as well include it
						case "<font=":
							if(data.fonts.ContainsKey(ParseText_myString)){
								//useFont = data.fonts[ParseText_myString].font; //remember the font in this wayyy
								ParseText_info.fontData = data.fonts[ParseText_myString];
							}else{
								Debug.Log("Unknown font: '" + ParseText_myString + "'. Fonts can be defined within the Text Data Inspector and are case-sensitive.");
							}
							ParseText_info.submeshChange = true;
							break; //switching to a non-dynamic font can return some errors
						case "</f>":
						case "</font>":
							//useFont = null; //forget it!
							ParseText_info.fontData = null;
							ParseText_info.submeshChange = true;
							break;
					//Quad
						case "<q=":
						case "<quad=":
							ParseText_dividedString = ParseText_myString.Split(','); //divide it up at commas
							//if quad exists and if this letter doesn't already have quad data:
							if(data.quads.ContainsKey(ParseText_dividedString[0]) && ParseText_info.quadData == null){
								if(ParseText_dividedString.Length == 1){
									// normal quad
									ParseText_info.quadData = data.quads[ParseText_myString];
									ParseText_info.isQuad = true; //just assign this manually to save eveyone's time
									insertAfter = "\u2000"; //a character to get used for the quad, in hyphenedtext
								}else if(ParseText_dividedString.Length == 2){
									int thisQuadIndex;
									if(int.TryParse(ParseText_dividedString[1], out thisQuadIndex)){
										ParseText_info.quadData = data.quads[ParseText_dividedString[0]];
										ParseText_info.isQuad = true;
										ParseText_info.quadIndex = thisQuadIndex;
										insertAfter = "\u2000"; //insert unicode quad to take its place
									}
									//override index
								}else if(ParseText_dividedString.Length == 3){
									int thisQuadIndexX;
									int thisQuadIndexY;
									if(int.TryParse(ParseText_dividedString[1], out thisQuadIndexX) && int.TryParse(ParseText_dividedString[2], out thisQuadIndexY)){
										//do some math to figure out what index this x and Y value points to
										ParseText_info.quadData = data.quads[ParseText_dividedString[0]];
										ParseText_info.isQuad = true;
										ParseText_info.quadIndex = ParseText_info.quadData.columns * thisQuadIndexX + thisQuadIndexY;
										insertAfter = "\u2000"; //insert unicode quad to take its place
									}
								}
							}
							ParseText_info.submeshChange = true;
							break;
					//Material
						case "<m=":
						case "<material=":
							if(data.materials.ContainsKey(ParseText_myString)){
								ParseText_info.materialData = data.materials[ParseText_myString];
							}
							ParseText_info.submeshChange = true;
							break;
						case "</m>":
						case "</material>":
							ParseText_info.materialData = null;
							ParseText_info.submeshChange = true;
							break;
					//Bold & Italic
						case "<b>":
							ParseText_info.ch.style = AddStyle(ParseText_info.ch.style, FontStyle.Bold); //mark this character as bold
							break;
						case "</b>":
							ParseText_info.ch.style = SubtractStyle(ParseText_info.ch.style, FontStyle.Bold);
							break;
						case "<i>":
							ParseText_info.ch.style = AddStyle(ParseText_info.ch.style, FontStyle.Italic); //mark this character as italic
							break;
						case "</i>":
							ParseText_info.ch.style = SubtractStyle(ParseText_info.ch.style, FontStyle.Italic);
							break;
					//Waves
						case "<w=":
							if(data.waves.ContainsKey(ParseText_myString)){ //is it a preset?
								ParseText_info.waveData = data.waves[ParseText_myString];
							}
							break;
						case "<w>":
							if(data.waves.ContainsKey("default")){
								ParseText_info.waveData = data.waves["default"]; //mark this character as bold
							}else{
//								Debug.Log("Default wave isn't defined!");
								//Resources.UnloadAsset(thisWave); //force it to search again??
							}
							break;
						case "</w>":
							ParseText_info.waveData = null;
							break;
					//Jitters
						case "<j=":
							if(data.jitters.ContainsKey(ParseText_myString)){ //is it a preset?
								ParseText_info.jitterData = data.jitters[ParseText_myString];
							}
							break;
						case "<j>":
							if(data.jitters.ContainsKey("default")){
								ParseText_info.jitterData = data.jitters["default"];
							}else{
								Debug.Log("Default jitter isn't defined!");
								//Resources.UnloadAsset(thisJitter); //force it to search again?
							}
							break;
						case "</j>":
							ParseText_info.jitterData = null;
							break;
					//Alignment
						case "<a=":
							switch(ParseText_myString.ToLower()){ //not case sensitive, for some reason? why not
								case "left": ParseText_info.alignment = Alignment.Left; break;
								case "right": ParseText_info.alignment = Alignment.Right; break;
								case "center": case "centre": ParseText_info.alignment = Alignment.Center; break;
								case "just": case "justify": case "justified": ParseText_info.alignment = Alignment.Justified; break;
								case "just2": case "justify2": case "justified2": ParseText_info.alignment = Alignment.ForceJustified; break;
							}
							break;
						case "</a>":
							ParseText_info.alignment = alignment; //return to default for mesh
							break;
					//Audio Settings
						case "<stopPreviousSound=":
							switch(ParseText_myString.ToLower()){
								case "true": ParseText_info.stopPreviousSound = true; break;
								case "false": ParseText_info.stopPreviousSound = false; break;
							}
							break;
						case "</stopPreviousSound>":
							ParseText_info.stopPreviousSound = stopPreviousSound; //reset to default
							break;
						case "<pitchMode=":
							switch(ParseText_myString.ToLower()){
								case "normal": ParseText_info.pitchMode = PitchMode.Normal; break;
								case "single": ParseText_info.pitchMode = PitchMode.Single; break;
								case "random": ParseText_info.pitchMode = PitchMode.Random; break;
								case "perlin": ParseText_info.pitchMode = PitchMode.Perlin; break;
							}
							break;
						case "</pitchMode>":
							ParseText_info.pitchMode = pitchMode; //return to default
							break;
						case "<overridePitch=":
							float thisOverridePitch;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisOverridePitch)){ //parse as a float
								ParseText_info.overridePitch = thisOverridePitch; //set time to be this float
							}
							break;
						case "</overridePitch>":
							ParseText_info.overridePitch = overridePitch;
							break;
						case "<minPitch=":
							float thisMinPitch;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMinPitch)){ //parse as a float
								ParseText_info.minPitch = thisMinPitch; //set time to be this float
							}
							break;
						case "</minPitch>":
							ParseText_info.minPitch = minPitch;
							break;
						case "<maxPitch=":
							float thisMaxPitch;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisMaxPitch)){ //parse as a float
								ParseText_info.maxPitch = thisMaxPitch; //set time to be this float
							}
							break;
						case "</maxPitch>":
							ParseText_info.maxPitch = maxPitch;
							break;
						case "<speedReadPitch=":
							float thisSpeedReadPitch;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisSpeedReadPitch)){ //parse as a float
								ParseText_info.speedReadPitch = thisSpeedReadPitch; //set time to be this float
							}
							break;
						case "</speedReadPitch>":
							ParseText_info.speedReadPitch = speedReadPitch;
							break;
						case "<readDelay=":
							float thisReadDelay;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisReadDelay)){ //parse as a float
								ParseText_info.readDelay = thisReadDelay; //set time to be this float
							}
							break;
						case "</readDelay>":
							ParseText_info.readDelay = readDelay;
							break;
						case "<drawAnim=":
							if(data.drawAnims.ContainsKey(ParseText_myString)){
								ParseText_info.drawAnimData = data.drawAnims[ParseText_myString]; //set draw animation
							}else if(data.drawAnims.ContainsKey("Appear")){
								ParseText_info.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "</drawAnim>":
							if(data.drawAnims.ContainsKey(drawAnimName)){
								ParseText_info.drawAnimData = data.drawAnims[drawAnimName]; //return to default
							}else if(data.drawAnims.ContainsKey("Appear")){
								ParseText_info.drawAnimData = data.drawAnims["Appear"]; //get first one
							}else{
								Debug.Log("'Appear' draw animation isn't defined!");
							}
							break;
						case "<drawOrder=":
							switch(ParseText_myString.ToLower()){
								case "lefttoright": case "ltr": ParseText_info.drawOrder = DrawOrder.LeftToRight; break;
								case "allatonce": case "all": ParseText_info.drawOrder = DrawOrder.AllAtOnce; break;
								case "onewordatatime": case "robot": ParseText_info.drawOrder = DrawOrder.OneWordAtATime; break;
								case "random": ParseText_info.drawOrder = DrawOrder.Random; break;
								case "righttoleft": case "rtl": ParseText_info.drawOrder = DrawOrder.RightToLeft; break;
								case "reverseltr": ParseText_info.drawOrder = DrawOrder.ReverseLTR; break;
								case "rtlonewordatatime": case "rtlrobot": ParseText_info.drawOrder = DrawOrder.RTLOneWordAtATime; break;
								case "onelineatatime": case "computer": ParseText_info.drawOrder = DrawOrder.OneLineAtATime; break;
							}
							break;
						case "</drawOrder>":
							ParseText_info.drawOrder = drawOrder; //return to default
							break;
						case "<clips=":
							if(data.soundClips.ContainsKey(ParseText_myString)){
								ParseText_info.soundClipData = data.soundClips[ParseText_myString];
							}
							break;
						case "</clips>":
							ParseText_info.soundClipData = null;
							break;
						case "<audioClips=":
							if(data.audioClips.ContainsKey(ParseText_myString)){
								ParseText_info.audioClipData = data.audioClips[ParseText_myString];
							}
							break;
						case "</audioClips>":
							ParseText_info.audioClipData = null;
							break;
						case "<indent=": //set the indent to be here
							float thisIndent;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisIndent)){ //parse as a float
								ParseText_info.indent = thisIndent; //set time to be this float
							}
							break;
						case "</indent>": //return to normal
							ParseText_info.indent = 0f;
							break;
						case "<pause>":
							//tell mesh to pause here
							_currentPauseCount++;
							if(Application.isPlaying && currentPauseCount > pauseCount) exitLoopAfter = true; //only during playmode for display purposes and cause it can break text
							else if(Application.isPlaying) insertAfter = "\u200B"; //2018-09-23 do this to prevent a bug where <e><pause><e> wouldn't play the second event
							break;
						case "<clear>":
							//call all cancel tags

							//color
							ParseText_info.colorData = null;
							ParseText_info.gradientData = null;
							ParseText_info.textureData = null;

							ParseText_info.size.x = size;
							ParseText_info.size.y = size;

							ParseText_info.ev2.Clear();

							ParseText_info.offset.y = 0f;
							break;
						//y offset
						case "<y=":
							float thisyOffset;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisyOffset)){ //parse as a float
								ParseText_info.offset.y = thisyOffset * ParseText_info.size.y; //set time to be this float
							}
							break;
						case "</y>":
							ParseText_info.offset.y = 0f;
							break;
						case "<sup>":
							//set size and y offset at the same time
							ParseText_info.offset.y = data.superscriptOffset * ParseText_info.size.y;
							ParseText_info.size.x = data.superscriptSize * ParseText_info.size.x;
							ParseText_info.size.y = data.superscriptSize * ParseText_info.size.y;
							break;
						case "</sup>":
							ParseText_info.offset.y = 0f;
							ParseText_info.size.x = size;
							ParseText_info.size.y = size;
							break;
						case "<sub>":
							//set size and y offset at the same time
							ParseText_info.offset.y = data.subscriptOffset * ParseText_info.size.y;
							ParseText_info.size.x = data.subscriptSize * ParseText_info.size.x;
							ParseText_info.size.y = data.subscriptSize * ParseText_info.size.y;
							break;
						case "</sub>":
							ParseText_info.offset.y = 0f;
							ParseText_info.size.x = size;
							ParseText_info.size.y = size;
							break;
						//unicode
						case "<u=":
							insertAfter = char.ConvertFromUtf32(int.Parse(ParseText_myString, System.Globalization.NumberStyles.HexNumber));
							break;
						//line spacing
						case "<linespacing=":
						case "<lineSpacing=":
						case "<ls=":
							float thisLineSpacing;
							if(float.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisLineSpacing)){ //parse as a float
								ParseText_info.lineSpacing = thisLineSpacing;
							}
							break;
						//quality?
						case "<quality=":
							int thisQuality;
							if(int.TryParse(ParseText_myString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out thisQuality)){ //parse as an int
								ParseText_info.chSize = thisQuality;
							}
							break;
						case "</quality>":
							ParseText_info.chSize = quality;
							break;
					//Default
						default:
							clearAfter = false;//DONT remove characters and do stuff
							break;
					}
					if(clearAfter){
						switch(ParseText_myTag){
							case "<br>": //ignore single-use tags
							case "<d>":
							case "<d=":
							case "<t=":
							case "<e=":
							case "<q=":
							case "<pause>":
							case "<u=":
								break;
							default: //remember this tage
								allTags.Add(new KeyValuePair<int, string>(i,myText.Substring(i,closingIndex+1-i))); //remember this tag and where it is
								break;
						}
						
						myText = myText.Remove(i,closingIndex-i+1);
						deletedChars += closingIndex-i;
						//Debug.Log("Removing '" + myText.Substring(i,closingIndex+1-i) + "'. The string is now: '" + myText + "'.");
						myText = myText.Insert(i,insertAfter);
						checkAgain = true;
					}
					if(exitLoopAfter){
						//keep track of last pause, and skip over it
						myText = myText.Remove(i,myText.Length-i); //remove everything after
						break;
					}
				}
			}

			if(infoCount - 1 == i)
			{
			//	info[i].SetValues(ParseText_info);
				info[i] = new STMTextInfo(ParseText_info); //update older one, it was checking again
				//Debug.Log("Updating older character " + myText[i].ToString(System.Globalization.CultureInfo.InvariantCulture) + " to be " + info[i].style);
			}
			else
			{
				/*
				if(i >= infoCount)
				{
					info.Add( new STMTextInfo(ParseText_info) ); //add new HAS TO BE NEW OR ELSE IT JUST REFERENCES
				}
				else
				{
					info[i].SetValues(ParseText_info);
				}
				*/
				info.Add( new STMTextInfo(ParseText_info) ); //add new HAS TO BE NEW OR ELSE IT JUST REFERENCES	
				infoCount++;
			}
			if(checkAgain)
			{
				i--;
			}
			else //stuff that gets reset!! single-use tags.
			{ 
				ParseText_info.delayData = null;// reset
				ParseText_info.quadData = null;
				ParseText_info.ev.Clear();
				ParseText_info.readTime = -1f; //say that timing hasn't been set for this letter yet
				ParseText_info.quadIndex = -1;
				if(ParseText_info.isQuad) //DONT reset
				{
					ParseText_info.submeshChange = true;
				}
				else
				{
					ParseText_info.submeshChange = false;
				}
				ParseText_info.isQuad = false;
			}
			ParseText_info.rawIndex = i + deletedChars + allTags.Count +1;
			
		}
		if(infoCount > myText.Length){
			//add extra char to myText for tacked-on event
			myText += "\u200B";
		}
		return myText;
	}
	int GetFontSize(Font myFont, STMTextInfo myInfo){ //so dynamic and non-dynamic fonts can be used together
		if(!myFont.dynamic && myFont.fontSize != 0){
			return myFont.fontSize; //always go w/ non-dynamic size first
		}
		if(myInfo.fontData != null){
			if(myInfo.fontData.overrideQuality){
				return myInfo.fontData.quality; //then set quality
			}else{
				return myInfo.chSize;
			}
		}
		if(myInfo.ch.size != 0){
			return myInfo.ch.size; //then natural quality
		}
		return myInfo.chSize; //default
	}
	private STMTextInfo RequestAllCharacters_info;
	void RequestAllCharacters(){ //by calling this every frame, should keep specific letters in the texture? not sure if it's a waste
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			RequestAllCharacters_info = info[i];
			Font myFont = RequestAllCharacters_info.fontData != null ? RequestAllCharacters_info.fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(myFont,RequestAllCharacters_info), RequestAllCharacters_info.ch.style); //request characters to draw
			//and special characters:
			myFont.RequestCharactersInTexture("-", GetFontSize(myFont,info[i]), FontStyle.Normal); //still call this, for when you're inserting hyphens anyway
		}
	}
	
	private Font Limits_font;
	private CharacterInfo Limits_ch;
	private STMTextInfo Limits_info;
	private int Limits_lineBreaks;
	private float Limits_currentWordWidth = 0f;
	private float Limits_longestWordWidth = 0f;
	void FigureOutUnwrappedLimits(Vector3 pos)
	{
//		if(uiMode){ //remove startup warnings
		unwrappedBottomRightTextBounds.x = 0f;
		unwrappedBottomRightTextBounds.y = 0f;
		Limits_longestWordWidth = 0f;
		Limits_currentWordWidth = 0f;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
			if(hyphenedText.Length != iL)
			{
				return;
			}
			Limits_info = info[i];
			Limits_font = Limits_info.fontData != null ? Limits_info.fontData.font : font; //use info's font, or default?
			Limits_font.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(Limits_font,Limits_info), Limits_info.ch.style); //request characters to draw
			//CharacterInfo ch;
			if(Limits_font.GetCharacterInfo(hyphenedText[i], out Limits_ch, GetFontSize(Limits_font,Limits_info), Limits_info.ch.style)){ //does this character exist?
				Limits_info.ch = Limits_ch; //remember character info!
				Limits_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//			SetTextGenSettings(Limits_info, i);
			}
			else
			{
				Limits_font = data.defaultFont;
				if(Limits_font.GetCharacterInfo(hyphenedText[i], out Limits_ch, GetFontSize(Limits_font,Limits_info), Limits_info.ch.style))
				{
					//change the font on this mesh to the default
					Limits_info.fontData = new STMFontData(data.defaultFont);
					Limits_info.ch = Limits_ch; //remember character info!
					Limits_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//				SetTextGenSettings(Limits_info, i);
				}
			}
		}
		Limits_lineBreaks = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each character to draw...
			Limits_info = info[i];
			Limits_font = Limits_info.fontData != null ? Limits_info.fontData.font : font; //use info's font, or default?
			float myQuality = (float)GetFontSize(Limits_font,Limits_info);

			if(hyphenedText[i] == '\n'){//drop a line
				//start new row at the X position of the indent character
				pos.x = Limits_info.indent; //assume left-orintated for now. go back to start of row
				if(lineHeights.Count > Limits_lineBreaks)
				{
					pos.y -= lineHeights[Limits_lineBreaks+1]; //drop down
				}
				Limits_lineBreaks++;
			}
			else if(hyphenedText[i] == '\r')
			{
				pos.x = Limits_info.indent;
			}
			//is this character linebreak friendly?
	
			/*
			if(hyphenedText[i] == '\n'){//drop a line
				//pos = new Vector3(Limits_info.indent, pos.y, 0); //assume left-orintated for now. go back to start of row
				pos.x = Limits_info.indent;
				pos.y -= lineSpacing * Limits_info.size; //drop down
			}
			*/
			else if(hyphenedText[i] == '\t'){//tab?
				pos.x += myQuality * 0.5f * tabSize * (Limits_info.size.x / myQuality);
				Limits_currentWordWidth += myQuality * 0.5f * tabSize * (Limits_info.size.x / myQuality);
				//pos += new Vector3(myQuality * 0.5f * tabSize, 0,0) * (Limits_info.size / myQuality);
			}
			else{// Advance character position
				//pos += Limits_info.Advance(characterSpacing,myQuality);
				pos.x += Limits_info.Advance(characterSpacing,myQuality).x;
				Limits_currentWordWidth += Limits_info.Advance(characterSpacing,myQuality).x;
			}//remember position data for whatever

			//word width, for multiline best fit
			for(int j=0; j<linebreakFriendlyChars.Count; j++)
			{
				if(hyphenedText[i] == linebreakFriendlyChars[j])
				{
					Limits_currentWordWidth = 0f;
				}
			}

			unwrappedBottomRightTextBounds.x = Mathf.Max(unwrappedBottomRightTextBounds.x, pos.x);
			unwrappedBottomRightTextBounds.y = Mathf.Min(unwrappedBottomRightTextBounds.y, pos.y);
			Limits_longestWordWidth = Mathf.Max(Limits_longestWordWidth, Limits_currentWordWidth);

			
		}
		//hyphenedText.Length > 0 ? -info[0].size * bestFitMulti
//		}
	}
	
	/*
	//just update UVs
	void GetCharacterInfoForArray(){
		//TODO see if setting "quality" to be info[i].ch.size has any GC issues, now: 2016-10-26
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //first, get character info...
			Font myFont = info[i].fontData != null ? info[i].fontData.font : font; //use info's font, or default?
			myFont.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(myFont,info[i]), info[i].ch.style); //request characters to draw
			CharacterInfo ch;
			if(myFont.GetCharacterInfo(hyphenedText[i], out ch, GetFontSize(myFont,info[i]), info[i].ch.style)){ //does this character exist?
				info[i].ch = ch; //remember character info!
			}//else, don't draw anything! this charcter won't have info
		}
	}
	*/
	private float BestFit_vertLimit = 0f;
	void CalculateBestFitMulti()
	{
		BestFit_vertLimit = VerticalLimit;
		bestFitMulti = 1f;
		if(bestFit != BestFitMode.Off)
		{
			//Debug.Log("bounds: " + -unwrappedBottomRightTextBounds.y + " multiplied limit: " + BestFit_vertLimit / bestFitMulti, this);
			

			//autowrap
			if(Rebuild_autoWrap > 0f)
			{
				bestFitMulti = Rebuild_autoWrap / unwrappedBottomRightTextBounds.x * 0.99999f; //use this number to keep it just below xMax
			}

			//vertical limit
			if((bestFit != BestFitMode.SquishAlways && bestFit != BestFitMode.SquishOverLimit) && BestFit_vertLimit > 0f && -unwrappedBottomRightTextBounds.y > BestFit_vertLimit / bestFitMulti)
			{
				bestFitMulti = BestFit_vertLimit / -unwrappedBottomRightTextBounds.y * 0.99999f;
				//pos.y -= VerticalLimit + unwrappedBottomRightTextBounds.y;

				//Debug.Log(-VerticalLimit / unwrappedBottomRightTextBounds.y);
			}

			if((bestFit == BestFitMode.OverLimit || bestFit == BestFitMode.SquishOverLimit) && bestFitMulti > 1f)
			{
				bestFitMulti = 1f; //don't multiply
			}

			if(bestFit == BestFitMode.MultilineBETA)
			{
				//how much space do characters take up
				//what's the biggest size they can be displayed at? within limits
				//insert linebreaks at the most optimal places
				
				//bestFitMulti *= BestFit_vertLimit;
				//bestFitMulti *= Limits_longestWordWidth * Rebuild_autoWrap * 0.99999f / size;
				//bestFitMulti = ((Rebuild_autoWrap / unwrappedBottomRightTextBounds.x) * (BestFit_vertLimit / -unwrappedBottomRightTextBounds.y)) * 0.99999f;
				//best fit multi is single line rn
				//try to fit in as many lines as it thinks it can?
				//float lineCount = BestFit_vertLimit / size;
				//bestFitMulti = (-unwrappedBottomRightTextBounds.y * (BestFit_vertLimit / Rebuild_autoWrap)) * 0.99999f / size;
				//assume box is tall for now... just reduce X bounds from maxY?

				//find biggest number that results in text size being equal or less than vertical limit
				//so first try 1 letter tall.
				//then 2...
				//3...
				//what is the first number that allows all text to fit in?
				//float highestBestFitMulti = 0f;
				/*
				float smallestBestFitMulti = bestFitMulti;
				for (int i=10; i-->1;)
				{
					float temp = (BestFit_vertLimit / -unwrappedBottomRightTextBounds.y) / size / (float)i;
					if(temp <= size)
					{
						if(temp <= smallestBestFitMulti)
							bestFitMulti = smallestBestFitMulti;
						else
							bestFitMulti = temp;
						break;
					}
				}
				*/

				float smallestBestFitMulti = bestFitMulti;
				bestFitMulti = Mathf.Lerp(smallestBestFitMulti, BestFit_vertLimit / -unwrappedBottomRightTextBounds.y, 0.1f);
				if(bestFitMulti > size) bestFitMulti = size; //keep below set size
				else if(bestFitMulti < smallestBestFitMulti) bestFitMulti = smallestBestFitMulti; //keep it above smallest size
				//bestFitMulti = highestBestFitMulti;
			}
			
			bestFitMulti = Mathf.Max(minSize / size, bestFitMulti);

		}
	}
	internal float Rebuild_verticalLimit;
	void CalculateLineHeights()
	{
		//before assigning position to letters, figure out what the biggest character in each row is
		lineHeights.Clear();
		//don't factor in linespacing for the very first line
		float biggest = infoCount > 0 ? info[0].size.y * info[0].lineSpacing : size * lineSpacing; //just in case this gets called somehowo
		for(int i=0, iL=infoCount; i<iL; i++)
		{
			if(hyphenedText[i] == '\n'/* || hyphenedText[i] == '\u200B'*/ || i == infoCount-1)
			{ //linebreak?
				lineHeights.Add(biggest); //don't add linebreak size, but...
				//if there's another character beyond this linebreak...
				if(infoCount-1 > i)
				{
					biggest = info[i+1].size.y * info[i+1].lineSpacing; //start with next row's first character
				}
			}
			else
			{
				biggest = Mathf.Max(biggest, info[i].size.y * info[i].lineSpacing);
			}
		}
		lineHeights.Add(biggest); //final line
		
		//now, box heights!
		boxHeights.Clear();
		float myHeight = 0f;
		float nextLimit = Rebuild_verticalLimit;
		//boxHeights.Add(myHeight);
		for(int i=0; i<lineHeights.Count; i++)
		{
			

			myHeight += lineHeights[i];
			//if this line would push it over the limit...
			if(myHeight > nextLimit)
			{
				//totalHeight += lineHeights[i];
				boxHeights.Add(myHeight - lineHeights[i]);
				nextLimit = myHeight - lineHeights[i] + Rebuild_verticalLimit;
			}
			//add this height after.
			//myHeight += lineHeights[i];
			
			
		}
		//add copies of the last line until it exceeds vertical limit
		/*
		while(myHeight < nextLimit)
		{
			myHeight += lineHeights[lineHeights.Count-1];
		}
		boxHeights.Add(myHeight - lineHeights[lineHeights.Count-1]);
		*/
		boxHeights.Add(myHeight);
	}
	private int[] allLinebreakIndexes;
/*
	private TextGenerator textGen = new TextGenerator(2);
	private TextGenerationSettings textGenSettings = new TextGenerationSettings();
	private List<UICharInfo> textGenInfo = new List<UICharInfo>();
	private void SetTextGenSettings(STMTextInfo myInfo, int textIndex)
	{
		textGenSettings.alignByGeometry = true;
		textGenSettings.color = Color.white;
		textGenSettings.font = myInfo.fontData != null ? myInfo.fontData.font : font;
		textGenSettings.fontSize = myInfo.chSize;
		textGenSettings.fontStyle = myInfo.chStyle;
		textGenSettings.generateOutOfBounds = true;
		textGenSettings.generationExtents = Vector2.one * 500f;
		textGenSettings.horizontalOverflow = HorizontalWrapMode.Overflow;
		textGenSettings.lineSpacing = lineSpacing;
		textGenSettings.pivot = Vector2.zero;
		textGenSettings.resizeTextForBestFit = false;
		textGenSettings.resizeTextMaxSize = 500;
		textGenSettings.resizeTextMinSize = 0;
		textGenSettings.richText = false;
		textGenSettings.scaleFactor = 1;
		textGenSettings.textAnchor = TextAnchor.UpperLeft;
		textGenSettings.updateBounds = false;
		textGenSettings.verticalOverflow = VerticalWrapMode.Overflow;

		if(textIndex != hyphenedText.Length -1)
		{
			textGen.Populate(hyphenedText.Substring(textIndex,2), textGenSettings);
			textGen.GetCharacters(textGenInfo); //populate list
			myInfo.charWidth = textGenInfo[0].charWidth;
			myInfo.cursorPos.x = textGenInfo[0].cursorPos.x;
			myInfo.cursorPos.y = textGenInfo[0].cursorPos.y;
		}
	}
*/
	private Vector3 Rebuild_pos = Vector3.zero;
	private Font Rebuild_font = null;
	private CharacterInfo Rebuild_ch;
	private CharacterInfo Rebuild_hyphenCh;
	private CharacterInfo Rebuild_breakCh;
	private float Rebuild_autoWrap;
	private int infoCount = 0;
	private STMTextInfo Rebuild_info;
	void RebuildTextInfo(){ 
		Rebuild_autoWrap = AutoWrap;
		Rebuild_verticalLimit = VerticalLimit;
		drawText = ParseText(text); //remove parsing junk (<col>, <b>), and fill textinfo again
		
		lineBreaks.Clear(); //index of line break characters, for centering
		hyphenedText = string.Copy(drawText);
		CalculateLineHeights(); //figure out line heigts for unwrapped equation
		

		
		//Debug.Log("lines: " + lineHeights.Count);
		
		//keep track of where to place this text
		//infoCount = hyphenedText.Length;
		Rebuild_pos.x = infoCount > 0 ? info[0].indent : 0f;
		Rebuild_pos.y = lineHeights.Count > 0 ? -lineHeights[0] : -size;
		Rebuild_pos.z = 0f; 
		
		//if(bestFit == BestFitMode.Off)
		//{
			FigureOutUnwrappedLimits(Rebuild_pos);

			
		//}
		//else
		//{
		//	unwrappedBottomRightTextBounds.x = 1f;
		//	unwrappedBottomRightTextBounds.y = 1f;
		//}
		CalculateBestFitMulti();
		//apply this multi to every letter early
		for(int i=0; i<hyphenedText.Length; i++)
		{
			if(bestFit == BestFitMode.SquishAlways || bestFit == BestFitMode.SquishOverLimit)
			{
				info[i].size.x *= bestFitMulti;
			}
			else
			{
				info[i].size.x *= bestFitMulti;
				info[i].size.y *= bestFitMulti;
				info[i].offset.x *= bestFitMulti;
				info[i].offset.y *= bestFitMulti;
			}
			/*
			if(info[i].size.x == 0f)
				info[i].size.x = 0.00001f;
			if(info[i].size.y == 0f)
				info[i].size.y = 0.00001f;
			*/
		}

		CalculateLineHeights(); //now with multi applied, redo line heights

		Rebuild_pos.x = infoCount > 0 ? info[0].indent : 0f;
		Rebuild_pos.y = lineHeights.Count > 0 ? -lineHeights[0] : -size;

		totalWidth = 0f;
		allFonts.Clear();
		if(Rebuild_autoWrap > 0f){ //use autowrap?

			//if(rtl)
				//hyphenedText = FlipParagraphs(hyphenedText, false);
			
			//TODO see if setting "quality" to be info[i].ch.size has any GC issues, now: 2016-10-26
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //first, get character info...
				Rebuild_info = info[i];
				Rebuild_font = Rebuild_info.fontData != null ? Rebuild_info.fontData.font : font; //use info's font, or default?
				
				if(!allFonts.Contains(Rebuild_font)){ //if this font is not listed yet
					allFonts.Add(Rebuild_font);
				}
				//Rebuild_info.size *= bestFitMulti;

				Rebuild_font.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style); 
				if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style)){ //does this character exist?
					Rebuild_info.ch = Rebuild_ch; //remember character info!
					// If the character changed, update the cached sizing values.
					Rebuild_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//				SetTextGenSettings(Rebuild_info, i);
				}
				//else, don't draw anything! this charcter won't have info
				//...is how it USED to work! instead, lets draw it in a fallback font:
				else{
					Rebuild_font = data.defaultFont;
					if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style))
					{
						//change the font on this mesh to the default
						Rebuild_info.fontData = new STMFontData(data.defaultFont);
						Rebuild_info.ch = Rebuild_ch; //remember character info!
						Rebuild_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//					SetTextGenSettings(Rebuild_info, i);
					}
				}
				
			}

			float lineWidth = infoCount > 0 ? info[0].indent : 0f;
			int previousBreak = -1;
			for(int i=0; i<infoCount; i++){
				Rebuild_info = info[i];
				Rebuild_font = Rebuild_info.fontData != null ? Rebuild_info.fontData.font : font; //use info's font, or default?
				//CharacterInfo breakCh; //moved these into this loop 2016-10-26
				Rebuild_font.GetCharacterInfo('\n', out Rebuild_breakCh, GetFontSize(Rebuild_font,Rebuild_info), style); //get data for linebreak
				//CharacterInfo hyphenCh;
				Rebuild_font.RequestCharactersInTexture("\u00AD", GetFontSize(Rebuild_font,Rebuild_info), style); //still call this, for when you're inserting hyphens anyway
				Rebuild_font.GetCharacterInfo('\u00AD', out Rebuild_hyphenCh, GetFontSize(Rebuild_font,Rebuild_info), style);
				//float hyphenWidth = hyphenCh.advance * (Rebuild_info.size / Rebuild_info.ch.size); //have hyphen size match last character in row
				
				
				if(hyphenedText[i] == '\n'){ //is this character a line break?
					lineWidth = Rebuild_info.indent; //new line, reset
					Rebuild_info.pos.x = Rebuild_info.indent;
				}
				else if(hyphenedText[i] == '\r')
				{
					lineWidth = Rebuild_info.indent;
					Rebuild_info.pos.x = Rebuild_info.indent;
				}
				else if(hyphenedText[i] == '\t'){ // linebreak with a tab...
					Rebuild_info.pos.x = lineWidth;
					lineWidth += 0.5f * tabSize * Rebuild_info.size.x;
					totalWidth += 0.5f * tabSize * Rebuild_info.size.x;
					
				}else{
					Rebuild_info.pos.x = lineWidth;
					lineWidth += Rebuild_info.Advance(characterSpacing).x;
					totalWidth += Rebuild_info.Advance(characterSpacing).x;
				}
				//TODO: watch out for natural hyphens going over bounds limits
				if(lineWidth > Rebuild_autoWrap && i > previousBreak+1){
					allLinebreakIndexes = new int[linebreakFriendlyChars.Count]; //garbage but whatever
					//get last index of every linebreak safe character
					for(int j=0; j<linebreakFriendlyChars.Count; j++)
					{
						allLinebreakIndexes[j] = hyphenedText.LastIndexOf(linebreakFriendlyChars[j],i);
					}
					//int myBreak = hyphenedText.LastIndexOf(' ',i); //safe spot to do a line break, can be a hyphen
//					int myHyphenBreak = hyphenedText.LastIndexOf('-',i);
					//int myTabBreak = hyphenedText.LastIndexOf('\t',i); //can break at a tab, too!
					//int myActualBreak = Mathf.Max(new int[]{myBreak, myHyphenBreak, myTabBreak}); //get the largest of all 3
					int myActualBreak = Mathf.Max(allLinebreakIndexes);
/*
					//do it again but with line start unfriendly chars.

					allLinebreakIndexes = new int[linebreakUnfriendlyChars.Count];
					for(int j=0; j<linebreakUnfriendlyChars.Count; j++)
					{
						allLinebreakIndexes[j] = hyphenedText.LastIndexOf(linebreakUnfriendlyChars[j], i)-1; //MINUS ONE
					}
					int oldActualBreak = myActualBreak;
					myActualBreak = Mathf.Max(allLinebreakIndexes); //again
					//combine
					myActualBreak = Mathf.Max(myActualBreak, oldActualBreak);
*/
					int lastBreak = hyphenedText.LastIndexOf('\n',i); //last place a line break happened
					
					if(!breakText && myActualBreak != -1 && myActualBreak > lastBreak){ //is there a space to do a line break? (and no hyphens...) AND we're not breaking text up at all
						
						//special case: if it's a space, remove that space!!! 2019-07-04
						//save for zero-width space. with can happen when a pause tag is added.
						if(hyphenedText[myActualBreak] == ' ' || hyphenedText[myActualBreak] == '　' || hyphenedText[myActualBreak] == '\u200B')
						{
							//replace this character with a linebreak
							hyphenedText = hyphenedText.Remove(myActualBreak, 1);
							hyphenedText = hyphenedText.Insert(myActualBreak, '\n'.ToString(System.Globalization.CultureInfo.InvariantCulture));
							info[myActualBreak].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							i = myActualBreak;
							previousBreak = i;
						}
						//if insert hyphens is true and this character is not a hyphen (and not a space)...
						else if(insertHyphens && hyphenedText[myActualBreak] != '-')
						{
							hyphenedText = hyphenedText.Insert(myActualBreak+1, "\u200B\n"); //soft hyphen and linebreak
							info.Insert(myActualBreak+1,new STMTextInfo(info[myActualBreak], Rebuild_breakCh));
							infoCount++;
							info[myActualBreak+1].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							info.Insert(myActualBreak+1,new STMTextInfo(info[myActualBreak], Rebuild_hyphenCh));
							infoCount++;
							info[myActualBreak+1].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							i = myActualBreak+2; //go back
							previousBreak = i;
						}
						else
						{
							//insert a linebreak after
							hyphenedText = hyphenedText.Insert(myActualBreak+1, '\n'.ToString(System.Globalization.CultureInfo.InvariantCulture));
							info.Insert(myActualBreak+1,new STMTextInfo(info[myActualBreak], Rebuild_breakCh));
							infoCount++;
							info[myActualBreak+1].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							i = myActualBreak+1; //go back
							previousBreak = i;
						}

						
					}
					
					else if(i > 0)
					{ //split it here! but not if it's the first character
						//or if it's an unsafe character. okay how do i do this.... it will involve backtracking thru text
						//so basically.... use LastIndexOf but in reverse?
						//so... find last index available that is NOT on the unfriendly list.
						//iterate backwards from this point in the string.

						var breakPos = i;
						//if(!breakText) //dont format if told to not format! commented out because it must be true at this point...
						{
							int cleanBreak = -1;
							//using previousBreak+1 instead of 0 will stop infinite loop and speed up code.
							for (int j = i; j --> previousBreak+1; )
							{
								//is the character at this point safe for the end of a line?

								//also i want this to prefer line start unfriendly chars!!!!!!!! how do i do this
								if(!linebreakUnfriendlyChars.Contains(hyphenedText[j]))
								{
									//make sure next character won't illegally end up at the start
									if(hyphenedText.Length > j+1 && !linestartUnfriendlyChars.Contains(hyphenedText[j+1]))
									{
										//move break point BACK to safe location. but could it go further for a cleaner result?
										//clean break is 100% not breaking any rules, but might not be the prettiest.
										cleanBreak = j+1;
										break; //stop this for loop
									}
								}
							}
							
							
							
							int dirtyBreak = -1;

							
							for(int j = cleanBreak; j --> previousBreak+1; )
							{
								//if this character is TOO FAR TO THE LEFT, ignore. rudimentary text rag!
								//float widthPercent = info[j].pos.x / Rebuild_autoWrap;
								//if(widthPercent > 0.8f)
								//okay instead of using a float, how about... within X characters of line end?

								//this could be a preferred character to break at, actually.

								//goal of dirty break is a NICER-LOOKING split. (text rag) so only apply to last few characters in a row.
								float widthPercent = info[j].pos.x / Rebuild_autoWrap;
								if(widthPercent > 0.6f)
								{
									//Debug.Log("Calculating " + widthPercent);
									if(hyphenedText.Length > j+1 && linebreakUnfriendlyChars.Contains(hyphenedText[j + 1]))
									{
										//and run thjis check, too
										if(!linebreakUnfriendlyChars.Contains(hyphenedText[j]))
										{
											//make sure next character won't illegally end up at the start
											if(hyphenedText.Length > j+1 && !linestartUnfriendlyChars.Contains(hyphenedText[j+1]))
											{
												//BUT it still needs to actually be safe!!!
												//if(j > previousBreak)
												{
													//safe!
													//i = j+1;
													dirtyBreak = j+1;
													break;
												}
											}
										}
									}
								}
							}
							
							
							if(cleanBreak > -1 && dirtyBreak > -1)
							{
								breakPos = Mathf.Min(cleanBreak, dirtyBreak);
							}
							else if(cleanBreak > -1)
							{
								breakPos = cleanBreak; //use clean
							}
						}


						if(insertHyphens){
							hyphenedText = hyphenedText.Insert(breakPos, "\u00AD\n");
							//Debug.Log("This needs a hyphen: " + hyphenedText);
							info.Insert(breakPos,new STMTextInfo(info[breakPos], Rebuild_breakCh));
							infoCount++;
							//use info[i] here instead of info[breakPos] since the info value is changing.
							info[breakPos].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							//						SetTextGenSettings(info[breakPos], i);
							info.Insert(breakPos,new STMTextInfo(info[breakPos], Rebuild_hyphenCh));
							infoCount++;
							info[breakPos].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							//						SetTextGenSettings(info[breakPos], i);
							previousBreak = breakPos+1;
						}else{
							hyphenedText = hyphenedText.Insert(breakPos, '\n'.ToString(System.Globalization.CultureInfo.InvariantCulture));
							info.Insert(breakPos,new STMTextInfo(info[breakPos], Rebuild_breakCh));
							infoCount++;
							info[breakPos].UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
							//						SetTextGenSettings(Rebuild_info, i);
							previousBreak = breakPos;
							//if(AutoWrap < info[i - indexOffset-1].size){ //otherwise, it'll loop foreverrr
							//i += 1;
							//}
							//iL += 1;
							//indexOffset += 1;
						}
					}//no need to check for following space, it'll come up anyway
					lineWidth = Rebuild_info.indent; //reset
				}
			}
		}else{ //no autowrap, no need to insert linebreaks
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
				Rebuild_info = info[i];
				Rebuild_font = Rebuild_info.fontData != null ? Rebuild_info.fontData.font : font; //use info's font, or default?
				//vvvv very important
				Rebuild_font.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style); //request characters to draw
				//font.RequestCharactersInTexture(System.Text.Encoding.UTF8.GetString(System.BitConverter.GetBytes(Rebuild_info.ch.index)), GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style); //request characters to draw
				//CharacterInfo ch;
				//get character from font if it exists
				if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style)){ //does this character exist?
					Rebuild_info.ch = Rebuild_ch; //remember character info!
					Rebuild_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//				SetTextGenSettings(Rebuild_info, i);
				}
				else{
					//get from default font instead
					Rebuild_font = data.defaultFont;
					if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style))
					{
						//change the font on this mesh to the default
						Rebuild_info.fontData = new STMFontData(data.defaultFont);
						Rebuild_info.ch = Rebuild_ch; //remember character info!
						Rebuild_info.UpdateCachedValuesIfChanged(fontTextureJustRebuilt);
	//					SetTextGenSettings(Rebuild_info, i);
					}
				}
				if(!allFonts.Contains(Rebuild_font)){ //if this font is not listed yet
					allFonts.Add(Rebuild_font);
				}
			}
		}
		
		//yeah, at this point, line breaks have been inserted, without position applied to 
		//so flip order for RTL text
		if(rtl)
		{
			hyphenedText = FlipParagraphs(hyphenedText, true);
		}

		CalculateLineHeights(); //now with multi applied, redo line heights
		//do these again, since line heights have paragraphs now...
		Rebuild_pos.x = infoCount > 0 ? info[0].indent : 0f;
		Rebuild_pos.y = lineHeights.Count > 0 ? -lineHeights[0] : -size;
		
/*
			//redo this bit of code that's above... its just copy-pasted.
			for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //from character info...
				Rebuild_info = info[i];
				Rebuild_font = Rebuild_info.fontData != null ? Rebuild_info.fontData.font : font; //use info's font, or default?
				Rebuild_font.RequestCharactersInTexture(hyphenedText[i].ToString(System.Globalization.CultureInfo.InvariantCulture), GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style); //request characters to draw
				if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style)){ //does this character exist?
					Rebuild_info.ch = Rebuild_ch; //remember character info!
					Rebuild_info.UpdateCachedValuesIfChanged();
				}
				else{
					//get from default font instead
					Rebuild_font = data.defaultFont;
					if(Rebuild_font.GetCharacterInfo(hyphenedText[i], out Rebuild_ch, GetFontSize(Rebuild_font,Rebuild_info), Rebuild_info.ch.style))
					{
						//change the font on this mesh to the default
						Rebuild_info.fontData = new STMFontData(data.defaultFont);
						Rebuild_info.ch = Rebuild_ch; //remember character info!
						Rebuild_info.UpdateCachedValuesIfChanged();
					}
				}
				if(!allFonts.Contains(Rebuild_font)){ //if this font is not listed yet
					allFonts.Add(Rebuild_font);
				}
			}
			
*/
		
		//get position
		int passedLineBreaks = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){ //for each character to draw...
			Rebuild_info = info[i];
			Rebuild_font = Rebuild_info.fontData != null ? Rebuild_info.fontData.font : font; //use info's font, or default?
			//CharacterInfo ch; //moved this code to the loop above^^^^
			//if(myFont.GetCharacterInfo(hyphenedText[i], out ch, quality, Rebuild_info.ch.style)){ //does this character exist?
			//	Rebuild_info.ch = ch; //remember character info!
			//}//else, don't draw anything! this charcter won't have info
			float myQuality = (float)GetFontSize(Rebuild_font,Rebuild_info);
			//switched this to += since tags can control an initial offset now. 2019-05-11
			Rebuild_info.pos.x = Rebuild_pos.x; //save this position data!
			Rebuild_info.pos.y = Rebuild_pos.y;
			Rebuild_info.pos.z = Rebuild_pos.z;
			//Rebuild_info.line = currentLineCount;
			if(hyphenedText[i] == '\n'){//drop a line
				//check for carriage return.
				lineBreaks.Add(i == 0 ? 0 : i-1);//first character is a line break? set default
				//start new row at the X position of the indent character
				Rebuild_pos.x = Rebuild_info.indent; //assume left-orintated for now. go back to start of row
				//Debug.Log(passedLineBreaks);
				if(lineHeights.Count > passedLineBreaks)
				{
					Rebuild_pos.y -= lineHeights[passedLineBreaks+1]; //drop down
				}
				passedLineBreaks++;
				//currentLineCount++;
			}
			else if(hyphenedText[i] == '\r')
			{
				lineBreaks.Add(i == 0 ? 0 : i-1);
				Rebuild_pos.x = Rebuild_info.indent;
			}
			else if(iL - 1 == i){ //last character, and not a line break?
				lineBreaks.Add(i);
			}
			else if(hyphenedText[i] == '\t'){//tab?
				Rebuild_pos.x += (myQuality * 0.5f * tabSize) * (Rebuild_info.size.x / myQuality);
				//Rebuild_pos += new Vector3(myQuality * 0.5f * tabSize, 0,0) * (Rebuild_info.size / myQuality);
			}
			else{// Advance character position
				Rebuild_pos.x += Rebuild_info.Advance(characterSpacing,myQuality).x;
				//Rebuild_pos += Rebuild_info.Advance(characterSpacing,myQuality);
			}//remember position data for whatever
		}
		lineBreaks = lineBreaks.Distinct().ToList(); //remove doubles, preventing horizontal offset glitch

		
		
		ApplyOffsetDataToTextInfo(); //just to clean up this very long function...

		if(verticalLimitMode == VerticalLimitMode.SquishBETA)
		{
			if(BestFit_vertLimit < -rawBottomRightTextBounds.y)
			{
				for(int i=0; i<hyphenedText.Length; i++)
				{
					info[i].size.y *= BestFit_vertLimit / -rawBottomRightTextBounds.y * 0.99999f;
					info[i].pos.y *= BestFit_vertLimit / -rawBottomRightTextBounds.y * 0.99999f;
					info[i].offset.y *= BestFit_vertLimit / -rawBottomRightTextBounds.y * 0.99999f;
				}
				RecalculateBounds();
			}
			
		}


		TrimCutoffText();
		UpdateRTLDrawOrder();
		
		ApplyUnreadTimingDataToTextInfo();
		ApplyTimingDataToTextInfo();
		PrepareSubmeshes();
	}
	
	//
	//public float lowestPosition = 0f;

	void TrimCutoffText(){
		leftoverText = "";
		//remove text that has been pushed BELOW the boundary
		if(VerticalLimit > 0f && verticalLimitMode == VerticalLimitMode.CutOff){
			float cutoffPoint = -VerticalLimit;
			
			//this tells text in cut off mode to adjust accordingly to the anchor point
			switch(anchor){
				case TextAnchor.UpperLeft:
				case TextAnchor.UpperCenter:
				case TextAnchor.UpperRight: break;
				case TextAnchor.MiddleLeft:
				case TextAnchor.MiddleCenter:
				case TextAnchor.MiddleRight:
					//offsetDifference = (-info[info.Count-1].pos.y) * 0.5f;
					//cutoffPoint = (cutoffPoint * 0.5f) + (-info[info.Count-1].pos.y - rawBottomRightBounds.y) * 0.5f;
					cutoffPoint *= 0.5f;
					break;
				case TextAnchor.LowerLeft:
				case TextAnchor.LowerCenter:
				case TextAnchor.LowerRight:
					//wait a min
					//cutoffPoint = -Mathf.Infinity;
					//cutoffPoint = -(-info[info.Count-1].pos.y - rawBottomRightBounds.y);
					cutoffPoint = 0f;
					break;
			}
			cutoffPoint += uiOffset.y;
			
			for(int i=0; i<hyphenedText.Length; i++){
				if(info[i].pos.y < cutoffPoint){ //if this text is below the bounds...
					//cutoffPosition = i-1; //this was the last character before the cutoff
					hyphenedText = hyphenedText.Remove(i, hyphenedText.Length - i); //remove all text after this point
					AssembleLeftoverText();
					return; //found the first character below the limit
				}
				//lowestPosition = info[i].pos.y; //cache it!
			}
		}
		else if(VerticalLimit > 0f && (verticalLimitMode == VerticalLimitMode.AutoPause || 
										verticalLimitMode == VerticalLimitMode.AutoPauseFull))
		
		{
			autoPauseStopPoint = -boxHeights[0]; //set this
			int autoPauseCount = 0;
			for(int i=0; i<hyphenedText.Length; i++)
			{
				while(info[i].pos.y < autoPauseStopPoint - offset.y) //if this text is below the bounds... 
				{
					//for now, just allow another box-length to be drawn
					//finally changing this...
					//_currentPauseCount++;
					autoPauseCount++;
					float myHeight = 0f;
					if(autoPauseCount < boxHeights.Count)
					{
						myHeight = -boxHeights[autoPauseCount];
					}
					else
					{
						myHeight -= lineHeights[lineHeights.Count-1];
					}
					

					//autoPauseStopPoint += info[i].pos.y;
					if(Application.isPlaying && currentPauseCount + autoPauseCount > pauseCount){ //same behaviour as pause tag
						hyphenedText = hyphenedText.Remove(i, hyphenedText.Length - i); //remove all text after this point
						AssembleLeftoverText();

						_currentPauseCount += autoPauseCount;
						return; //found the first character below the limit
					}
					autoPauseStopPoint = myHeight;
					//}
				}
				//lowestPosition = info[i].pos.y; //cache it!
			}
		}
		if(info.Count > 0){
			//lowestPosition = info[info.Count-1].pos.y; //cache it! this is repeated here for show last mode...
		}
	}
	void AssembleLeftoverText(){
		//hyphenedtext has already been shortened, so...
		int cutoffPosition = hyphenedText.Length;
		if(cutoffPosition > 0){
			//first, add all tags up to and including this position
			for(int i=0; i<allTags.Count; i++){ //go thru all tags...
				if(allTags[i].Key <= cutoffPosition){
					leftoverText += allTags[i].Value; //add all tags from before this point
				}else{
					break;
				}
			}
			cutoffPosition = info[cutoffPosition].rawIndex; //translate to raw text index
			//next, add all raw text past this position
			//remove starting spaces from the start here

			if(cutoffPosition <= preParsedText.Length)
				leftoverText += preParsedText.Substring(cutoffPosition)/*.TrimStart()*/;

		}
	}

	private Vector3 offset = Vector3.zero;
	private Vector3 uiOffset = Vector3.zero;
	private float OffsetData_VerticalLimit = 0f;
	private int OffsetData_rowStart = 0;
	private float OffsetData_offsetRight = 0f;
	private int OffsetData_spaceCount = 0;
	private float OffsetData_maxHeight = 0f;
	private float OffsetData_maxWidth = 0f;
	void ApplyOffsetDataToTextInfo(){ //this works!!! ahhhh!!!
		OffsetData_VerticalLimit = VerticalLimit; //cache it
		float[] allMaxes = new float[lineBreaks.Count];
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){
			//get max x data from this line
			allMaxes[i] = info[lineBreaks[i]].BottomRightVert.x;
			//Debug.DrawRay(t.TransformPoint(info[lineBreaks[i]].RelativeAdvance(characterSpacing)), Vector3.right, Color.red, 0f);
			//Debug.Log("pos: " + info[lineBreaks[i]].pos + " advance: " + info[lineBreaks[i]].RelativeAdvance(characterSpacing));
			//allMaxes[i] = info[lineBreaks[i]].TopRightVert.x;
			if(float.IsNaN(allMaxes[i])){
				allMaxes[i] = 0f; //for rows that are just linebreaks! take THAT
			}
		}
		//calculate this below, not from the values above
		rawBottomRightTextBounds.x = Mathf.Max(allMaxes);
		rawBottomRightTextBounds.y = 0f;
		//lowestY = 0f;
		//offset = Vector3.zero; //reset
		offset.x = 0f;
		offset.y = 0f;
		offset.z = 0f;
		//minY = 0f;
		/* */
		if(uiMode){
			//ALIGN TO WHATEVER UI BOX HERE!!!
			//RectTransform tr = t as RectTransform; //(RectTransform(t)) also works!
			//uiOffset = Vector3.zero;
			uiOffset.x = 0f;
			uiOffset.y = 0f;
			uiOffset.z = 0f;
			//TODO: during play mode, this doesn't update right...
			switch(anchor){
				case TextAnchor.UpperLeft: 
					//uiOffset = new Vector3(tr.rect.xMin, tr.rect.yMax, 0f); 
					uiOffset.x = tr.rect.xMin;
					uiOffset.y = tr.rect.yMax;
					break;
				case TextAnchor.UpperCenter: 
					uiOffset.x = (tr.rect.xMin + tr.rect.xMax) / 2f;
					uiOffset.y = tr.rect.yMax;
					//uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMax, 0f); break;
					break;
				case TextAnchor.UpperRight: 
					uiOffset.x = tr.rect.xMax;
					uiOffset.y = tr.rect.yMax;
					//uiOffset = new Vector3(tr.rect.xMax, tr.rect.yMax, 0f); 
					break;
				case TextAnchor.MiddleLeft: 
					uiOffset.x = tr.rect.xMin;
					uiOffset.y = (tr.rect.yMin + tr.rect.yMax) / 2f;
					//uiOffset = new Vector3(tr.rect.xMin, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); 
					break;
				case TextAnchor.MiddleCenter: 
					uiOffset.x = (tr.rect.xMin + tr.rect.xMax) / 2f;
					uiOffset.y = (tr.rect.yMin + tr.rect.yMax) / 2f;
					//uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); 
					break;
				case TextAnchor.MiddleRight: 
					uiOffset.x = tr.rect.xMax;
					uiOffset.y = (tr.rect.yMin + tr.rect.yMax) / 2f;
					//uiOffset = new Vector3(tr.rect.xMax, (tr.rect.yMin + tr.rect.yMax) / 2f, 0f); 
					break;
				case TextAnchor.LowerLeft: 
					uiOffset.x = tr.rect.xMin;
					uiOffset.y = tr.rect.yMin;
					//uiOffset = new Vector3(tr.rect.xMin, tr.rect.yMin, 0f); 
					break;
				case TextAnchor.LowerCenter: 
					uiOffset.x = (tr.rect.xMin + tr.rect.xMax) / 2f;
					uiOffset.y = tr.rect.yMin;
					//uiOffset = new Vector3((tr.rect.xMin + tr.rect.xMax) / 2f, tr.rect.yMin, 0f); 
					break;
				case TextAnchor.LowerRight:
					uiOffset.x = tr.rect.xMax;
					uiOffset.y = tr.rect.yMin;
					//uiOffset = new Vector3(tr.rect.xMax, tr.rect.yMin, 0f); 
					break;
			}
			offset.x -= uiOffset.x;
			offset.y -= uiOffset.y;
			//offset -= uiOffset;
		}
		//float lowestVert = 0f;
		//float rightestVert = 0f;
		//float mostLeftVert = Mathf.Infinity; //this is probably a bad idea
		OffsetData_rowStart = 0; //index of where this row starts
		lowestPosition = 0f;
		for(int i=0, iL=lineBreaks.Count; i<iL; i++){ //for each line of text //2016-06-09 new alignment script
			//OffsetData_offsetRight = 0f; //empty space on this row
			OffsetData_offsetRight = rawBottomRightTextBounds.x - allMaxes[i];
			
			if(Rebuild_autoWrap > 0f){
				OffsetData_offsetRight += Rebuild_autoWrap - rawBottomRightTextBounds.x;
			}
			OffsetData_spaceCount = 0;
			for(int j=OffsetData_rowStart, jL=lineBreaks[i]+1; j<jL; j++){ //see how many spaces there are
				if(hyphenedText[j] == ' '){
					OffsetData_spaceCount++;
				}
			}
			float justifySpace = OffsetData_spaceCount > 0 ? OffsetData_offsetRight / (float)OffsetData_spaceCount : 0f;
			int passedSpaces = 0;
			for(int j=OffsetData_rowStart, jL=lineBreaks[i]+1; j<jL; j++){//if this character is in the range...
				info[j].line = i; //tell info what line the letter is on here
				if(hyphenedText[j] == ' '){
					passedSpaces++;
				}
				//Debug.Log("Aligning character " + j + ", which is: '" + hyphenedText[j] + "'.");
				switch(info[j].alignment){
					case Alignment.Center:
						info[j].pos.x += OffsetData_offsetRight / 2f; //use half of empty space
						break;
					case Alignment.Right:
						info[j].pos.x += OffsetData_offsetRight;
						break;
					case Alignment.Justified:
						if(jL != hyphenedText.Length && drawText[jL - (hyphenedText.Length - drawText.Length)] != '\n'){ //not the very last row, or a row with a linebreak?
							info[j].pos.x += justifySpace * passedSpaces;
						}
						break;
					case Alignment.ForceJustified:
						info[j].pos.x += justifySpace * passedSpaces; //justify no matter what
						break;
					//do nothing for left-aligned
				}
				
				//if(info[j].pos.y > -VerticalLimit){ //only keep counting if it's not past the line count limit
					//if(VerticalLimit > 0f){
						//minY = Mathf.Min(minY, info[j].pos.y - info[j].size); //yeah this works. shouldn't change with waves/weird letters
					//	rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y - info[j].size);
					//}else{
					rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y);
					//}
					if(OffsetData_VerticalLimit == 0f || (OffsetData_VerticalLimit > 0f && verticalLimitMode == VerticalLimitMode.Ignore) || info[j].pos.y >= -OffsetData_VerticalLimit){ //only keep counting if it's not past the line count limit
						lowestPosition = Mathf.Min(lowestPosition, info[j].pos.y);
					}
					//maxX = Mathf.Max(maxX, info[j].BottomRightVert.x);
					//Debug.Log("Character: " + j + " y value: " + info[j].pos.y);
				//}
				//minY = Mathf.Min(minY, info[j].pos.y - info[j].size); //yeah this works. shouldn't change with waves/weird letters
				
				//rawBottomRightTextBounds.y = Mathf.Min(rawBottomRightTextBounds.y, info[j].pos.y - info[j].size);
				//maxX = Mathf.Max(maxX, info[j].BottomRightVert.x);
				//mostLeftVert = Mathf.Min(mostLeftVert, info[j].BottomLeftVert.x);
				//lowestY = Mathf.Min(lowestY, info[j].pos.y); 
			}
			OffsetData_rowStart = lineBreaks[i]+1;
		}
		//2018-01-06 this code is pointless since the limit doesn't matter in this state. But for some reason the limit is wrong so this looks better
/*
		if(uiMode){ //fix boundary ending up too wide on non-wrapped UI text
			RectTransform tr = t as RectTransform;
			maxX -= tr.rect.xMax; //watch the offset! this corrects the width
			//if(!wrapText) tr.rect.xMax;
			//if(VerticalLimit > 0f) minY = tr.rect.yMax;
			//RectTransform rect = c.transform as RectTransform;
			//tr.sizeDelta = new Vector2(!wrapText ? rawBottomRightBounds.x : tr.sizeDelta.x, !limitText ? topLeftBounds.y : tr.sizeDelta.y);
			//LayoutRebuilder.MarkLayoutForRebuild(tr);
		}
*/
		//clamp
		//minY = VerticalLimit > 0f ? -VerticalLimit + (size * bestFitMulti) : minY;
		//float upperY = size; //push down
		//float lowerY = size * (lineBreaks.Count - 1) * lineSpacing;
		OffsetData_maxHeight = OffsetData_VerticalLimit > 0f ? -OffsetData_VerticalLimit : rawBottomRightTextBounds.y;
		OffsetData_maxWidth = Rebuild_autoWrap > 0f ? Rebuild_autoWrap : rawBottomRightTextBounds.x; //if autowrapping, base it on box instead of text
		switch(anchor){
			case TextAnchor.UpperLeft: 
				//offset += new Vector3(0, 0f, 0); 
				break;
			case TextAnchor.UpperCenter: 
				//offset += new Vector3(OffsetData_maxWidth * 0.5f, 0f, 0); 
				offset.x += OffsetData_maxWidth * 0.5f;
				break;
			case TextAnchor.UpperRight: 
				//offset += new Vector3(OffsetData_maxWidth, 0f, 0); 
				offset.x += OffsetData_maxWidth;
				break;
			case TextAnchor.MiddleLeft: 
				offset.y += OffsetData_maxHeight * 0.5f;
				//offset += new Vector3(0, OffsetData_maxHeight * 0.5f, 0); 
				break;
			case TextAnchor.MiddleCenter: 
				offset.x += OffsetData_maxWidth * 0.5f;
				offset.y += OffsetData_maxHeight * 0.5f;
				//offset += new Vector3(OffsetData_maxWidth * 0.5f, OffsetData_maxHeight * 0.5f, 0); 
				break;
			case TextAnchor.MiddleRight: 
				offset.x += OffsetData_maxWidth;
				offset.y += OffsetData_maxHeight * 0.5f;
				//offset += new Vector3(OffsetData_maxWidth, OffsetData_maxHeight * 0.5f, 0); 
				break;
			case TextAnchor.LowerLeft: 
				offset.y += OffsetData_maxHeight;
				//offset += new Vector3(0, OffsetData_maxHeight, 0); 
				break;
			case TextAnchor.LowerCenter: 
				offset.x += OffsetData_maxWidth * 0.5f;
				offset.y += OffsetData_maxHeight;
				//offset += new Vector3(OffsetData_maxWidth * 0.5f, OffsetData_maxHeight, 0); 
				break;
			case TextAnchor.LowerRight: 
				offset.x += OffsetData_maxWidth;
				offset.y += OffsetData_maxHeight;
				//offset += new Vector3(OffsetData_maxWidth, OffsetData_maxHeight, 0); 
				break;
		}
		//infoCount = hyphenedText.Length;
		for(int i=0; i<infoCount; i++){ //apply all offsets
			info[i].pos -= offset;
			//if all text goes beyond the vertical limit, move it up
		}
		

		rawTopLeftBounds.x = offset.x;//scale to show proper bounds even when parent is scaled weird
		rawTopLeftBounds.y = offset.y;
		rawTopLeftBounds.z = offset.z;

		rawBottomRightBounds.x = Rebuild_autoWrap > 0f ? offset.x - Rebuild_autoWrap : offset.x - rawBottomRightTextBounds.x;
		rawBottomRightBounds.y = OffsetData_VerticalLimit > 0f ? OffsetData_VerticalLimit + offset.y : offset.y - OffsetData_maxHeight;
		rawBottomRightBounds.z = offset.z;

		//align text to fit within this new box:
		anchorOffset.x = 0f;
		anchorOffset.y = 0f;
		anchorOffset.z = 0f;
		switch(anchor){
			case TextAnchor.UpperLeft:
			case TextAnchor.UpperCenter:
			case TextAnchor.UpperRight: break;
			case TextAnchor.MiddleLeft:
			case TextAnchor.MiddleCenter:
			case TextAnchor.MiddleRight:
				//offsetDifference = (-lowestPosition) * 0.5f;
				anchorOffset.y = OffsetData_VerticalLimit > -rawBottomRightTextBounds.y ? (-rawBottomRightTextBounds.y + rawTopLeftBounds.y - rawBottomRightBounds.y) * 0.5f : 0f;
				break;
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerRight:
				//Debug.Log(rawBottomRightTextBounds);
				anchorOffset.y = OffsetData_VerticalLimit > -rawBottomRightTextBounds.y ? -rawBottomRightTextBounds.y + rawTopLeftBounds.y - rawBottomRightBounds.y : 0f;
				break;
		}
		RecalculateBounds();
	}
	private Vector3 anchorOffset = Vector3.zero;
	private Vector3 RecalculateBounds_point;
	void RecalculateBounds(){
		RecalculateBounds_point.x = -rawTopLeftBounds.x;
		RecalculateBounds_point.y = -rawTopLeftBounds.y;
		RecalculateBounds_point.z = -rawTopLeftBounds.z;
		topLeftBounds = t.TransformPoint(RecalculateBounds_point);
		RecalculateBounds_point.x = -rawBottomRightBounds.x;
		RecalculateBounds_point.y = -rawTopLeftBounds.y;
		RecalculateBounds_point.z = rawTopLeftBounds.z;
		topRightBounds = t.TransformPoint(RecalculateBounds_point);
		RecalculateBounds_point.x = -rawTopLeftBounds.x;
		RecalculateBounds_point.y = -rawBottomRightBounds.y;
		RecalculateBounds_point.z = rawBottomRightBounds.z;
		bottomLeftBounds = t.TransformPoint(RecalculateBounds_point);
		RecalculateBounds_point.x = -rawBottomRightBounds.x;
		RecalculateBounds_point.y = -rawBottomRightBounds.y;
		RecalculateBounds_point.z = -rawBottomRightBounds.z;
		bottomRightBounds = t.TransformPoint(RecalculateBounds_point);

		centerBounds = Vector3.Lerp(topLeftBounds, bottomRightBounds, 0.5f);

		if(hyphenedText.Length == 0)
		{
			RecalculateTextBounds();
		}
		RecalculateFinalTextBounds();
	}
	private Vector3 TextBounds_leftOffset = Vector3.zero;
	private Vector3 TextBounds_rightOffset = Vector3.zero;
	private float TextBounds_diff = 0f;
	void RecalculateBoundsOffsets(){
		TextBounds_leftOffset.x = 0f;
		TextBounds_leftOffset.y = 0f;
		TextBounds_leftOffset.z = 0f;
		TextBounds_rightOffset.x = 0f;
		TextBounds_rightOffset.y = 0f;
		TextBounds_rightOffset.z = 0f;
		TextBounds_diff = rawBottomRightTextBounds.x + rawBottomRightBounds.x - offset.x; //distance between text bounds and autowrap, if any

		switch(alignment){
			case Alignment.Center:
				TextBounds_leftOffset.x += TextBounds_diff / 2f; //use half of empty space
				TextBounds_rightOffset.x += TextBounds_diff / 2f; //use half of empty space
				break;
			case Alignment.Right:
				TextBounds_leftOffset.x += TextBounds_diff;
				TextBounds_rightOffset.x += TextBounds_diff;
				break;
			case Alignment.Justified:
			case Alignment.ForceJustified:
				TextBounds_rightOffset.x += TextBounds_diff;
				break;
			//do nothing for left-aligned
		}
	}
	private float RecalculateBounds_textBottom = 0f;
	private Transform RecalculateBounds_t;
	void RecalculateTextBounds(){
		if(hyphenedText.Length > 0)
		{
			RecalculateBoundsOffsets();
			RecalculateBounds_t = this.transform; //this is stupid
			//TODO: figure out why subtracting offset in this one spot is so different
			RecalculateBounds_textBottom = Mathf.Max(lowestDrawnPositionRaw - offset.y, lowestPosition - rawTopLeftBounds.y);
			//line up with text...
			//Debug.Log("lowest drawn: " + lowestDrawnPosition + " lowest: " + lowestPosition);
			RecalculateBounds_point.x = -TextBounds_leftOffset.x - rawTopLeftBounds.x + anchorOffset.x;
			RecalculateBounds_point.y = -TextBounds_leftOffset.y - rawTopLeftBounds.y + anchorOffset.y;
			RecalculateBounds_point.z = -TextBounds_leftOffset.z - rawTopLeftBounds.z + anchorOffset.z;
			topLeftTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x =	furthestDrawnPosition - rawTopLeftBounds.x - TextBounds_rightOffset.x + anchorOffset.x;
			RecalculateBounds_point.y = -rawTopLeftBounds.y - TextBounds_rightOffset.y + anchorOffset.y;
			RecalculateBounds_point.z = -rawTopLeftBounds.z - TextBounds_rightOffset.z + anchorOffset.z;
			topRightTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x =	-rawTopLeftBounds.x -TextBounds_leftOffset.x + anchorOffset.x;
			RecalculateBounds_point.y = RecalculateBounds_textBottom -TextBounds_leftOffset.y + anchorOffset.y;
			RecalculateBounds_point.z = -TextBounds_leftOffset.z + anchorOffset.z;
			bottomLeftTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x =	furthestDrawnPosition - rawTopLeftBounds.x -TextBounds_rightOffset.x + anchorOffset.x;
			RecalculateBounds_point.y = RecalculateBounds_textBottom -TextBounds_rightOffset.y + anchorOffset.y;
			RecalculateBounds_point.z = -TextBounds_rightOffset.z + anchorOffset.z;
			bottomRightTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);

			//centerTextBounds = Vector3.Lerp(topLeftTextBounds, bottomRightTextBounds, 0.5f);
			centerTextBounds.x = Mathf.Lerp(topLeftTextBounds.x, bottomRightTextBounds.x, 0.5f);
			centerTextBounds.y = Mathf.Lerp(topLeftTextBounds.y, bottomRightTextBounds.y, 0.5f);
			//centerTextBounds.z = Mathf.Lerp(topLeftTextBounds.z, bottomRightTextBounds.z, 0.5f);
		}
		else
		{
			topLeftTextBounds.x = 0f;
			topLeftTextBounds.y = 0f;
			topLeftTextBounds.z = 0f;
			topRightTextBounds.x = 0f;
			topRightTextBounds.y = 0f;
			topRightTextBounds.z = 0f;
			bottomLeftTextBounds.x = 0f;
			bottomLeftTextBounds.y = 0f;
			bottomLeftTextBounds.z = 0f;
			bottomRightTextBounds.x = 0f;
			bottomRightTextBounds.y = 0f;
			bottomRightTextBounds.z = 0f;
			centerTextBounds.x = 0f;
			centerTextBounds.y = 0f;
			centerTextBounds.z = 0f;
		}
	}
	void RecalculateFinalTextBounds(){
		if(hyphenedText.Length > 0)
		{
			RecalculateBoundsOffsets();
			RecalculateBounds_t = this.transform; //this is stupid
			//calculate final bounds:
			RecalculateBounds_point.x = -rawTopLeftBounds.x - TextBounds_leftOffset.x + anchorOffset.x;
			RecalculateBounds_point.y =  -rawTopLeftBounds.y - TextBounds_leftOffset.y + anchorOffset.y;
			RecalculateBounds_point.z =  -rawTopLeftBounds.z - TextBounds_leftOffset.z + anchorOffset.z;
			finalTopLeftTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x = rawBottomRightTextBounds.x - rawTopLeftBounds.x - TextBounds_rightOffset.x + anchorOffset.x;
			RecalculateBounds_point.y =  - rawTopLeftBounds.y - TextBounds_rightOffset.y + anchorOffset.y;
			RecalculateBounds_point.z =  - rawTopLeftBounds.z - TextBounds_rightOffset.z + anchorOffset.z;
			finalTopRightTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x =  - rawTopLeftBounds.x - TextBounds_leftOffset.x + anchorOffset.x;
			RecalculateBounds_point.y = lowestPosition - rawTopLeftBounds.y - TextBounds_leftOffset.y + anchorOffset.y;
			RecalculateBounds_point.z =  - rawTopLeftBounds.z - TextBounds_leftOffset.z + anchorOffset.z;
			finalBottomLeftTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);
			RecalculateBounds_point.x = rawBottomRightTextBounds.x - rawTopLeftBounds.x - TextBounds_rightOffset.x + anchorOffset.x;
			RecalculateBounds_point.y = lowestPosition - rawTopLeftBounds.y - TextBounds_rightOffset.y + anchorOffset.y;
			RecalculateBounds_point.z =  - rawTopLeftBounds.z - TextBounds_rightOffset.z + anchorOffset.z;
			finalBottomRightTextBounds = RecalculateBounds_t.TransformPoint(RecalculateBounds_point);

			//finalCenterTextBounds = Vector3.Lerp(finalTopLeftTextBounds, finalBottomRightTextBounds, 0.5f);
			finalCenterTextBounds.x = Mathf.Lerp(finalTopLeftTextBounds.x, finalBottomRightTextBounds.x, 0.5f);
			finalCenterTextBounds.y = Mathf.Lerp(finalTopLeftTextBounds.y, finalBottomRightTextBounds.y, 0.5f);
		}
		else
		{
			finalTopLeftTextBounds.x = 0f;
			finalTopLeftTextBounds.y = 0f;
			finalTopLeftTextBounds.z = 0f;
			finalTopRightTextBounds.x = 0f;
			finalTopRightTextBounds.y = 0f;
			finalTopRightTextBounds.z = 0f;
			finalBottomLeftTextBounds.x = 0f;
			finalBottomLeftTextBounds.y = 0f;
			finalBottomLeftTextBounds.z = 0f;
			finalBottomRightTextBounds.x = 0f;
			finalBottomRightTextBounds.y = 0f;
			finalBottomRightTextBounds.z = 0f;
			finalCenterTextBounds.x = 0f;
			finalCenterTextBounds.y = 0f;
			finalCenterTextBounds.z = 0f;
		}
	}

	private int[] drawOrderRTL = new int[0];
	private int RTL_currentLine = -1;
	private int RTL_lastEnd = -1;
	void UpdateRTLDrawOrder (){ //update the RTL draw info, if needed
		//if(drawOrder == DrawOrder.RightToLeft || undrawOrder == DrawOrder.RightToLeft){ //actually calculate? eh, do it anyway
		drawOrderRTL = new int[hyphenedText.Length];
	//if(drawOrderRTL.Length < hyphenedText.Length)
	//		Array.Resize(ref drawOrderRTL, hyphenedText.Length);

		RTL_currentLine = 0;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			RTL_lastEnd = RTL_currentLine > 0 ? lineBreaks[RTL_currentLine-1] + 1 : 0;
			if(RTL_currentLine < lineBreaks.Count){
				drawOrderRTL[i] = -i + lineBreaks[RTL_currentLine] + RTL_lastEnd;
				if(lineBreaks[RTL_currentLine] == i){ //this was the last character in this row
					//Debug.Log("The end of this line was: " + lineBreaks[RTL_currentLine]);
					RTL_currentLine++;
				}
			}
		}
		//}
	}
	private STMTextInfo Timing_textInfo;
	//private STMTextInfo Timing_myTextInfo;
	void ApplyTimingDataToTextInfo(){
		float currentTiming = 0f;
		float furthestPoint = 0f;
		bool needsToRead = false;
		for(int i=0, iL=hyphenedText.Length; i<iL; i++){
			Timing_textInfo = info[i];
			int myIndex = GetDrawOrder(Timing_textInfo.drawOrder, i, iL);
			//Timing_textInfo = info[i];
			Timing_textInfo = info[myIndex];
			if(Timing_textInfo.readDelay > 0f){ //a delay hasn't been set for this letter, yet
				needsToRead = true;
			}
			
			float additionalDelay = Timing_textInfo.delayData != null ? Timing_textInfo.delayData.count : 0f; //if there's no additional delay data attached... no additional delay
			//get the time it'll be drawn at...
			if(Timing_textInfo.readTime < 0f){ //if a time hasn't been set for this letter yet
				switch(Timing_textInfo.drawOrder){
					case DrawOrder.AllAtOnce:
						Timing_textInfo.readTime = currentTiming;
						break;
					case DrawOrder.Random:
						Timing_textInfo.readTime = UnityEngine.Random.Range(0f,Timing_textInfo.readDelay);
						break;
					case DrawOrder.OneWordAtATime:
						if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
							currentTiming += i == 0 ? additionalDelay * Timing_textInfo.readDelay : Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						}
						Timing_textInfo.readTime = currentTiming;
						break;
					case DrawOrder.OneLineAtATime:
						if(hyphenedText[i] == '\n')
						{
							currentTiming += i == 0 ? additionalDelay * Timing_textInfo.readDelay : Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						}
						Timing_textInfo.readTime = currentTiming;
						break;
					case DrawOrder.RightToLeft:
						Timing_textInfo.readTime = currentTiming; //reverse order!
						currentTiming += myIndex == 0 ? additionalDelay * Timing_textInfo.readDelay : Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						break;
					case DrawOrder.ReverseLTR:
						currentTiming += i == 0 ? additionalDelay * Timing_textInfo.readDelay : Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						Timing_textInfo.readTime = currentTiming;
						break;
					case DrawOrder.RTLOneWordAtATime:
						Timing_textInfo.readTime = currentTiming;
						if(myIndex == 0 || hyphenedText[myIndex] == ' ' || hyphenedText[myIndex] == '\n' || hyphenedText[myIndex] == '\t' || hyphenedText[myIndex] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
							currentTiming += Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						}
						break;
					default: //Left To Right
						//dont add extra for first character
						currentTiming += i == 0 ? additionalDelay * Timing_textInfo.readDelay : Timing_textInfo.readDelay + (additionalDelay * Timing_textInfo.readDelay);
						Timing_textInfo.readTime = currentTiming;
						break;
				}
			}else{
				currentTiming = Timing_textInfo.readTime; //pick up from here
			}
			float maxAnimTime = Timing_textInfo.drawAnimData != null ? Mathf.Max(Timing_textInfo.drawAnimData.animTime, Timing_textInfo.drawAnimData.fadeTime) : 0f; //just for initialization, so an error doesn't get returned. drawanim should never be null, really
			furthestPoint = Mathf.Max(Timing_textInfo.readTime + maxAnimTime, furthestPoint);
		}
		totalReadTime = furthestPoint + 0.00001f; //save it!
		callReadFunction = needsToRead;
	}
	private STMTextInfo UnreadTiming_textInfo;
	void ApplyUnreadTimingDataToTextInfo(){
		//the other on the switch statement is different than the function above on purpose... might change in the future
		//things have to be done in a slightly different order
		float currentTiming = 0f;
		float furthestPoint = 0f;
		STMDrawAnimData myDrawAnim = UndrawAnim; //store undrawing animation since it'll be called multiple times
		for(int i=0, iL=hyphenedText.Length; i<iL; i++)
		{
			int myIndex = GetDrawOrder(undrawOrder, i, iL);

			UnreadTiming_textInfo = info[myIndex];
			switch(undrawOrder)
			{
				case DrawOrder.AllAtOnce:
					UnreadTiming_textInfo.unreadTime = currentTiming;
					break;
				case DrawOrder.Random:
					UnreadTiming_textInfo.unreadTime = UnityEngine.Random.Range(0f,unreadDelay);
					break;
				case DrawOrder.OneWordAtATime:
					UnreadTiming_textInfo.unreadTime = currentTiming;
					if(hyphenedText[i] == ' ' || hyphenedText[i] == '\n' || hyphenedText[i] == '\t' || hyphenedText[i] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
						currentTiming += unreadDelay;
					}
					break;
				case DrawOrder.OneLineAtATime:
					if(hyphenedText[i] == '\n')
					{
						currentTiming += unreadDelay;
					}
					UnreadTiming_textInfo.unreadTime = currentTiming;
					break;
				case DrawOrder.RightToLeft:
					currentTiming += unreadDelay;
					UnreadTiming_textInfo.unreadTime = currentTiming;
					break;
				case DrawOrder.ReverseLTR:
					currentTiming += unreadDelay;
					UnreadTiming_textInfo.unreadTime = currentTiming;
					break;
				case DrawOrder.RTLOneWordAtATime:
					UnreadTiming_textInfo.unreadTime = currentTiming;
					if(myIndex == 0 || hyphenedText[myIndex] == ' ' || hyphenedText[myIndex] == '\n' || hyphenedText[myIndex] == '\t' || hyphenedText[myIndex] == '-'){ //only advance timing on a space, line break, or tab, or hyphen!
						currentTiming += unreadDelay;
					}
					break;
				default: //left to right
					UnreadTiming_textInfo.unreadTime = currentTiming;
					currentTiming += unreadDelay; //<<< this is applied in opposite order as normal read info
					break;
			}
			float maxAnimTime = myDrawAnim != null ? Mathf.Max(myDrawAnim.animTime, myDrawAnim.fadeTime) : 0f;
			furthestPoint = Mathf.Max(UnreadTiming_textInfo.unreadTime + maxAnimTime, furthestPoint);
		}
		totalUnreadTime = furthestPoint + 0.00001f; //save it!
	}

	Vector3 WavePosition_Vect = Vector3.zero;
	private float WavePosition_multi = 0f;
	Vector3 WavePosition(STMTextInfo myInfo, STMWaveControl wave, float myTime)
	{
		WavePosition_multi = wave.multiOverTime.Evaluate(myTime);
		//multiply phase by 6 because ??????? seems to work
		//multiply by universal size;
		WavePosition_Vect.x = wave.curveX.Evaluate(((myTime * wave.speed.x) + wave.phase * 6f) + (myInfo.pos.x * wave.density.x / myInfo.size.x)) * wave.strength.x * myInfo.size.x * WavePosition_multi;
		WavePosition_Vect.y = wave.curveY.Evaluate(((myTime * wave.speed.y) + wave.phase * 6f) + (myInfo.pos.x * wave.density.y / myInfo.size.y)) * wave.strength.y * myInfo.size.y * WavePosition_multi;
		WavePosition_Vect.z = wave.curveZ.Evaluate(((myTime * wave.speed.z) + wave.phase * 6f) + (myInfo.pos.x * wave.density.z / myInfo.size.y)) * wave.strength.z * myInfo.size.y * WavePosition_multi;

		return WavePosition_Vect;
	}

	Vector3 WaveRotation_Pivot = Vector3.zero;
	Vector3 WaveRotation_Offset = Vector3.zero;
	Vector3 WaveRotation_ReturnVal = Vector3.zero;
	Vector3 WaveRotation_myRotation = Vector3.zero;
	Quaternion WaveRotation_myQuaternion = new Quaternion();
	Vector3 WaveRotation(STMTextInfo myInfo, STMWaveRotationControl rot, Vector3 vertPos, float myTime)
	{
		//return the offset relative to zero

		//x pivot should be based on letter's width
		//y pivot should be based on local height of mesh
		WaveRotation_Pivot.x = myInfo.pos.x + rot.pivot.x * myInfo.RelativeWidth;
		WaveRotation_Pivot.y = myInfo.pos.y + rot.pivot.y * myInfo.size.y;
		WaveRotation_Pivot.z = 0f;

		WaveRotation_Offset.x = vertPos.x - WaveRotation_Pivot.x;
		WaveRotation_Offset.y = vertPos.y - WaveRotation_Pivot.y;
		WaveRotation_Offset.z = vertPos.z - WaveRotation_Pivot.z;

		WaveRotation_myRotation.x = 0f;
		WaveRotation_myRotation.y = 0f;
		WaveRotation_myRotation.z = rot.curveZ.Evaluate(((myTime * rot.speed) + rot.phase * 6f) + (myInfo.pos.x * rot.density)) * rot.strength;

		WaveRotation_myQuaternion = Quaternion.Euler(WaveRotation_myRotation);

		WaveRotation_Offset = WaveRotation_myQuaternion * WaveRotation_Offset;

		WaveRotation_ReturnVal.x = WaveRotation_Offset.x + WaveRotation_Pivot.x - vertPos.x;
		WaveRotation_ReturnVal.y = WaveRotation_Offset.y + WaveRotation_Pivot.y - vertPos.y;
		WaveRotation_ReturnVal.z = WaveRotation_Offset.z + WaveRotation_Pivot.z - vertPos.z;

		return WaveRotation_ReturnVal;
	}	

	Vector3 WaveScale(STMTextInfo myInfo, STMWaveScaleControl scale, Vector3 vertPos, float myTime){
		
		Vector3 pivot = myInfo.pos + new Vector3(scale.pivot.x * myInfo.RelativeWidth, scale.pivot.y * myInfo.size.y, 0f);
		Vector3 offset = vertPos - pivot;
		Vector3 newScale = new Vector3(scale.curveX.Evaluate(((myTime * scale.speed.x) + scale.phase * 6f) + (myInfo.pos.x * scale.density.x)) * scale.strength.x,
										scale.curveY.Evaluate(((myTime * scale.speed.y) + scale.phase * 6f) + (myInfo.pos.x * scale.density.y)) * scale.strength.y,
										1f);
		offset = Vector3.Scale(offset, newScale);
		return offset + pivot - vertPos;
	}
	/*
	Vector3 WaveValue(STMTextInfo myInfo, STMWaveControl position, STMWaveRotationControl rotation, STMWaveScaleControl scale){ //multiply phase by 6 because ??????? seems to work
		//float currentTime = GetTime;
		//float myTime = myInfo.waveData.animateFromTimeDrawn ? GetTime - timeDrawn - myInfo.readTime : GetTime;
		//Vector3 myPos = myInfo.waveData.positionControl ? WavePosition(myInfo, position, myTime) : Vector3.zero;
		//Vector3 myRot = myInfo.waveData.rotationControl ? WaveRotation(myInfo, rotation, myTime) : Vector3.zero;
		//Vector3 mySca = myInfo.waveData.scaleControl ? WaveScale(myInfo, scale, myTime) : Vector3.one;
		//return Vector3.Scale(Quaternion.Euler(myRot) * myPos, mySca); //add it all together
	}
	*/
	
Vector3 JitterValue_MyJit = Vector3.zero;
	Vector3 JitterValue(STMTextInfo myInfo, STMJitterData jit)
	{
		float myTime = currentReadTime - myInfo.readTime; //time that's different for each letter
		switch(jit.perlin)
		{
			case true:
				//weird perlin jitter... could use some work, but it's a jitter effect that scales with time
				JitterValue_MyJit.x = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x, 0f) - 0.5f)) * myInfo.size.x;
				JitterValue_MyJit.y = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * (jit.distance.Evaluate(Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x + 30f, 0f)) * jit.amount * (Mathf.PerlinNoise(jit.perlinTimeMulti * myTime + myInfo.pos.x + 30f, 0f) - 0.5f)) * myInfo.size.y;
				JitterValue_MyJit.z = 0f;
				break;
			default:
				//ditance over time... so jitters can also only happen AS a letter is drawn.
				JitterValue_MyJit.x = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(UnityEngine.Random.value) * jit.amount * (UnityEngine.Random.value - 0.5f) * myInfo.size.x; //make jit follow curve
				JitterValue_MyJit.y = jit.distanceOverTime.Evaluate(myTime / jit.distanceOverTimeMulti) * jit.distance.Evaluate(UnityEngine.Random.value) * jit.amount * (UnityEngine.Random.value - 0.5f) * myInfo.size.y;
				JitterValue_MyJit.z = 0f;
				break;
		}

		return JitterValue_MyJit;
	}

	

   //--------------------------------------------------
	// Performance updates performed by RedVonix
	// http://www.RedVonix.com/
	//
	// All comments from Red are marked with "RV:"
	//--------------------------------------------------

	// Cache objects here that we'll use in UpdateMesh
	private Vector3 UpdateMesh_waveValue = Vector3.zero; //universal
	private Vector3 UpdateMesh_waveValueTopLeft = Vector3.zero;
	private Vector3 UpdateMesh_waveValueTopRight = Vector3.zero;
	private Vector3 UpdateMesh_waveValueBottomRight = Vector3.zero;
	private Vector3 UpdateMesh_waveValueBottomLeft = Vector3.zero;
	private Vector3 UpdateMesh_lowestLineOffset = Vector3.zero;
	private Vector3 UpdateMesh_wavePosition;
	private Vector2 UpdateMesh_uvOffset = new Vector2();
	private STMTextInfo CurrentTextInfo;
	Vector3[] UpdateMesh_Middles = new Vector3[0];
	Vector3[] UpdateMesh_Positions = new Vector3[0];

	// These are used in UpdateMesh for math conversions to avoid doing casting
	private Vector3 cacheVectThree;
	Vector3 jitterValue;
	private Vector2 vectA;
	private Vector2 vectAA;
	private Vector2 vectB;
	private Vector2 vectBB;
	private Vector2 vectC;
	private Vector2 vectCC;
	private Vector2 vectD;
	private Vector2 vectDD;
	private Vector2 infoVect = new Vector2();

//	private Vector2 ratioHold;
	private Vector2 uvMidHold;
	private Vector4 ratioAndUvHold;

	private bool doPrintEventAfter = false;
	private bool doEventAfter = false;

	//private float tmpRawBottomRightBounds = 0f;

	private Vector3 realBaseOffset = Vector3.zero;

	private int GetDrawOrder(DrawOrder myDrawOrder, int i, int iL)
	{
		switch(myDrawOrder)
		{
			case DrawOrder.RightToLeft: case DrawOrder.RTLOneWordAtATime: return drawOrderRTL[i];
			case DrawOrder.ReverseLTR: return -i + iL - 1;
		}
		return i;
	}

	void UpdateMesh(float myTime) 
	{ //set the data for the endmesh

		// Store the GetTime value so we don't have to calculate it multiple times on every call of UpdateMesh.
		float GetTimeTime = GetTime;
		
		// Same with the VerticalLimit
		float VerticalLimitStored = VerticalLimit;

		int targArraySize = hyphenedText.Length * 4;

		//Mesh mesh = new Mesh();
		areWeAnimating = false;

		//if(hyphenedText.Length > 0){ //bother to draw it?

		// RV: Only update the array sizes here if we need to.
		// Generate a mesh for the characters we want to print.
		if (endVerts.Length != targArraySize)
			Array.Resize(ref endVerts, targArraySize);

		//endTriangles = new int[hyphenedText.Length * 6];
		if (endUv.Length != targArraySize)
			Array.Resize(ref endUv, targArraySize);

		if (endUv2.Length != targArraySize)
			Array.Resize(ref endUv2, targArraySize);//overlay images

		if (endCol32.Length != targArraySize)
			Array.Resize(ref endCol32, targArraySize);

		if (ratiosAndUvMids.Count != targArraySize)
			ratiosAndUvMids = new List<Vector4>(new Vector4[targArraySize]);

		if(isUvRotated.Count != targArraySize)
			isUvRotated = new List<Vector4>(new Vector4[targArraySize]);


		//if (uvMids.Length < targArraySize)
		//	Array.Resize(ref uvMids, targArraySize);

		//int tallestVisibleIndex = 0;
		//float highestVisiblePoint = rawBottomRightBounds.y;
		//bool playedSoundThisFrame = false;
		//float offsetDifference = 0f;
		//info[info.Count-1].pos.y
		
		for(int i=0, iL=hyphenedText.Length; i<iL; i++)
		{
			
			//Timing_textInfo = info[i];
			//myIndex = GetDrawOrder(CurrentTextInfo.drawOrder, i, iL);
			// RV: Grab the current STMTextInfo object only once from the list it's in, as acquiring it over tand over just adds unneeded overhead.
			CurrentTextInfo = info[i];
			int myIndex = GetDrawOrder(CurrentTextInfo.drawOrder, i, iL);
			//Debug.Log(CurrentTextInfo.character);
			
			//used to be used for ultra outlines
			//ratioHold = CurrentTextInfo.ratio;
			
			//ratios[4 * i + 0] = CurrentTextInfo.ratio;
			//ratios[4 * i + 1] = CurrentTextInfo.ratio;
			//ratios[4 * i + 2] = CurrentTextInfo.ratio;
			//ratios[4 * i + 3] = CurrentTextInfo.ratio;

			if(myIndex <= latestNumber) //temp fix events. but i think this breaks rtl text? 
			{

				CurrentTextInfo.invoked = true;
			}


			if(readDelay == 0f)
			{
				//do every event
				DoEvent(i);
			}

			//just to get timing info for events
			//these are used to that positional data is updated before events are called
			doPrintEventAfter = false;
			doEventAfter = false;
			if (reading)
			{
				/*
				okay the point of this is...
				multiple characters can be printed a frame, and all of them should have their events played.
				but only one should play a sound! and I suppose that should be... the... first one drawn this frame?
				so relying on the index number is a problem since indexes can be whatever,
				*/

				float divideAnimAmount = CurrentTextInfo.drawAnimData.animTime == 0f ? 0.0000001f : CurrentTextInfo.drawAnimData.animTime; //so it doesn't get NaN'd
				float myAnimPos = (myTime - CurrentTextInfo.readTime) / divideAnimAmount; // on a range between 0-1 on the curve, the position of the animation
				if(myAnimPos > 0f && !CurrentTextInfo.invoked){ 
					CurrentTextInfo.invoked = true;
					doEventAfter = true;
					//if(!playedSoundThisFrame){
					//	playedSoundThisFrame = true;

					//ignore 0-width space, as it's used for tacked-on events
					if(hyphenedText[i] != '\u200B')
					{
						//}
						doPrintEventAfter = true;
						if(hyphenedText[i] != ' ' && hyphenedText[i] != '\n')
						{
							lowestDrawnPosition = Mathf.Min(lowestDrawnPosition, CurrentTextInfo.pos.y);
							lowestDrawnPositionRaw = Mathf.Min(lowestDrawnPositionRaw, CurrentTextInfo.pos.y + offset.y);
							//Debug.Log(CurrentTextInfo.pos.y);
							furthestDrawnPosition = Mathf.Max(furthestDrawnPosition, CurrentTextInfo.RelativeAdvance(characterSpacing).x + offset.x + TextBounds_rightOffset.x);
						}
					}
					latestNumber = Mathf.Max(latestNumber, myIndex); //find latest number to start from next frame
					/*
					if(info[i].pos.y + info[i].size + lowestLineOffset.y < 0f && //this number is below the limit
						info[i].pos.y + info[i].size + lowestLineOffset.y > highestVisiblePoint){ //is this number taller than the current tallest number?
							highestVisiblePoint = info[i].pos.y + info[i].size + lowestLineOffset.y;
							tallestVisibleIndex = i;
					}
					*/
					
				}
			}else if(!Application.isPlaying || VerticalLimitStored == 0f || !(verticalLimitMode == VerticalLimitMode.AutoPause ||
																		verticalLimitMode == VerticalLimitMode.AutoPauseFull)){ //don't do this for autopauses
				latestNumber = hyphenedText.Length-1;
				lowestDrawnPosition = info[latestNumber].pos.y; //assume the final letter
				lowestDrawnPositionRaw = info[latestNumber].pos.y + offset.y;
				furthestDrawnPosition = rawBottomRightTextBounds.x; //this causes some sizes to be incorrect in-editor, but should be fine as text reads out
			}
			RecalculateTextBounds();
			if(doEventAfter)
			{
				DoEvent(i); //do every event up to this integer
			}
			if(doPrintEventAfter)
			{
				PlaySound(i); //only play one sound this frame, from the first letter drawn this frame
				if(onPrintEvent != null) onPrintEvent.Invoke();
				if(OnPrintEvent != null) OnPrintEvent();
			}
			//if(offsetDifference > 0f) offsetDifference = 0f;
			//this is done here so text grows from middle/lower zones
			UpdateMesh_lowestLineOffset.x = 0f;
			UpdateMesh_lowestLineOffset.y = 0f;
			UpdateMesh_lowestLineOffset.z = 0f;
			
			//push text up if it goes below vertical limit. uses a multiple of size to keep consistent line drop sizes
			//info[i].pos.y < -rawBottomRightBounds.y
			//Debug.Log("lowest drawn position: " + lowestDrawnPosition + "bottom right bounds: " + -rawBottomRightBounds.y);
			if(VerticalLimitStored > 0f && (verticalLimitMode == VerticalLimitMode.ShowLast || 
										verticalLimitMode == VerticalLimitMode.AutoPause ||
										verticalLimitMode == VerticalLimitMode.AutoPauseFull) &&
										//2020-12-28 the uiMode * 2f stuff is a temp fix... I can't figure out exactly what's causing this just yet? ah well.
				lowestDrawnPosition < -rawBottomRightBounds.y/* * (uiMode ? 2f : 1f)*/){ //if the bounds extend beyond the vertical limit
				
				//Debug.Log("pushing up");
				//push text up!
				//and round to nearest multiple of size, so text lines up with top of box
				//UpdateMesh_lowestLineOffset.y = Mathf.Ceil((-lowestDrawnPosition - rawBottomRightBounds.y) / (size * lineSpacing)) * (size * lineSpacing);
				//line up with top of next row...
				//UpdateMesh_lowestLineOffset.y = Mathf.Ceil((-lowestDrawnPosition - rawBottomRightBounds.y) / (size * lineSpacing)) * (size * lineSpacing);
				//UpdateMesh_lowestLineOffset.y = -lowestDrawnPosition;
				//UpdateMesh_lowestLineOffset.y = lineHeights[0];
				//UpdateMesh_lowestLineOffset.y = -lowestDrawnPosition - rawBottomRightBounds.y;
				//if(verticalLimitMode == VerticalLimitMode)
				
				if(verticalLimitMode == VerticalLimitMode.AutoPauseFull)
				{
					for(int j=0; j<boxHeights.Count; j++)
					{
						UpdateMesh_lowestLineOffset.y = boxHeights[j];
						//for every line...
						//lowestBox = Mathf.Max(lowestBox, j);
						//if this offset is greater than the end of the box...?
						if(UpdateMesh_lowestLineOffset.y >= -lowestDrawnPosition - rawBottomRightBounds.y)
						{
							break;
						}
						//push up again
						//UpdateMesh_lowestLineOffset.y += lineHeights[j];

					}
				}
				else
				{
					for(int j=0; j<lineHeights.Count; j++)
					{
						UpdateMesh_lowestLineOffset.y += lineHeights[j];
						//for every line...
						
						//if this offset is greater than the end of the box...?
						if(UpdateMesh_lowestLineOffset.y >= -lowestDrawnPosition - rawBottomRightBounds.y)
						{
							break;
						}
						//push up again
						//UpdateMesh_lowestLineOffset.y += lineHeights[j];

					}
				}
				/*
				if (verticalLimitMode == VerticalLimitMode.AutoPauseFull){
					tmpRawBottomRightBounds = rawBottomRightBounds.y;
					//round to nearest multiple of verticallimit
					if(uiMode) tmpRawBottomRightBounds *= 2f;
					//Debug.Log("This mesh is named " + t.name + " and my rawBottomRightBounds.y is " + tmpRawBottomRightBounds);
				   	UpdateMesh_lowestLineOffset.y = Mathf.Ceil(UpdateMesh_lowestLineOffset.y / tmpRawBottomRightBounds) * tmpRawBottomRightBounds;
					//UpdateMesh_lowestLineOffset.y = Mathf.Ceil(UpdateMesh_lowestLineOffset.y / (size * lineSpacing)) * (size * lineSpacing);
				}
				*/
			}
			UpdateMesh_lowestLineOffset.x += anchorOffset.x;
			UpdateMesh_lowestLineOffset.y += anchorOffset.y + ((info[0].lineSpacing - 1f) * info[0].size.y); //push up to hide line spacing on first row.
			UpdateMesh_lowestLineOffset.z += anchorOffset.z;
			//Debug.Log(lowestDrawnPosition);

		//Vertex data:
		//animated stuffffff
			jitterValue = Vector3.zero;
			if(CurrentTextInfo.jitterData != null && !data.disableAnimatedText && !disableAnimatedText)
			{ //just dont jitter if animating text is overridden
				areWeAnimating = true;
				jitterValue = JitterValue(CurrentTextInfo, CurrentTextInfo.jitterData); //get jitter data
			}

			UpdateMesh_waveValue = Vector3.zero; //universal
			UpdateMesh_waveValueTopLeft = Vector3.zero;
			UpdateMesh_waveValueTopRight = Vector3.zero;
			UpdateMesh_waveValueBottomRight = Vector3.zero;
			UpdateMesh_waveValueBottomLeft = Vector3.zero;
			//Vector3 UpdateMesh_waveValueRotation = Vector3.zero;
			//Vector3 UpdateMesh_waveValueRotPivot = Vector3.zero;
			
			if(CurrentTextInfo.waveData != null && CurrentTextInfo.size.y != 0 && !data.disableAnimatedText && !disableAnimatedText){
				areWeAnimating = true;
				float waveTime = CurrentTextInfo.waveData.animateFromTimeDrawn ? currentReadTime - CurrentTextInfo.readTime : GetTimeTime;
				if(CurrentTextInfo.waveData.positionControl){
					UpdateMesh_waveValue = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.position, waveTime);
				}

				// RV: Following was converted to add individual dimensions of arrays rather than whole arrays, as adding
				//      full arrays causes the creation of a new array which creates additional GC and processing time.
				if(CurrentTextInfo.waveData.individualVertexControl)
				{
					UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.topLeft, waveTime);
					UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.topRight, waveTime);
					UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.bottomRight, waveTime);
					UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WavePosition(CurrentTextInfo, CurrentTextInfo.waveData.bottomLeft, waveTime);
					UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
				}
				
				if(CurrentTextInfo.waveData.rotationControl)
				{
					UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.TopLeftVert, waveTime);
					UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.TopRightVert, waveTime);
					UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.BottomRightVert, waveTime);
					UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveRotation(CurrentTextInfo, CurrentTextInfo.waveData.rotation, CurrentTextInfo.BottomLeftVert, waveTime);
					UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
				}
				if(CurrentTextInfo.waveData.scaleControl)
				{
					UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.TopLeftVert, waveTime);
					UpdateMesh_waveValueTopLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopLeft.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.TopRightVert, waveTime);
					UpdateMesh_waveValueTopRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueTopRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueTopRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.BottomRightVert, waveTime);
					UpdateMesh_waveValueBottomRight.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomRight.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomRight.z += UpdateMesh_wavePosition.z;

					UpdateMesh_wavePosition = WaveScale(CurrentTextInfo, CurrentTextInfo.waveData.scale, CurrentTextInfo.BottomLeftVert, waveTime);
					UpdateMesh_waveValueBottomLeft.x += UpdateMesh_wavePosition.x;
					UpdateMesh_waveValueBottomLeft.y += UpdateMesh_wavePosition.y;
					UpdateMesh_waveValueBottomLeft.z += UpdateMesh_wavePosition.z;
				}
				
			}
			
			//if text isn't different, you don't need to update UVs, or triangles
			//only need to update vertices of animated text
			//only need to update color of text w/ animated colors

			//for cutting off old text
			if ((VerticalLimitStored > 0f && verticalLimitMode != VerticalLimitMode.Ignore) &&  //if vertical limit is on and not set to ignore...
				(CurrentTextInfo.pos.y + CurrentTextInfo.size.y + UpdateMesh_lowestLineOffset.y - anchorOffset.y > -rawTopLeftBounds.y + 0.00001f/* || info[i].pos.y < -rawBottomRightBounds.y*/))
			{ //hide all text that extends beyond the vertical limit. For things like ShowLast mode.
			  //if using a limited vertical space and this text's line is before the text that should be shown
				endVerts[4 * i + 0] = Vector3.zero; //hide it!
				endVerts[4 * i + 1] = Vector3.zero;
				endVerts[4 * i + 2] = Vector3.zero;
				endVerts[4 * i + 3] = Vector3.zero;
			}
			else
			{
				//assign vertices
				if(relativeBaseOffset)
				{
					realBaseOffset.x = baseOffset.x * CurrentTextInfo.size.x;
					realBaseOffset.y = baseOffset.y * CurrentTextInfo.size.y;
					realBaseOffset.z = baseOffset.z;
				}
				else
				{
					realBaseOffset.x = baseOffset.x;
					realBaseOffset.y = baseOffset.y;
					realBaseOffset.z = baseOffset.z;
				}

				endVerts[4 * i + 0].x = ((CurrentTextInfo.TopLeftVert.x + jitterValue.x + UpdateMesh_waveValueTopLeft.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + realBaseOffset.x);
				endVerts[4 * i + 0].y = ((CurrentTextInfo.TopLeftVert.y + jitterValue.y + UpdateMesh_waveValueTopLeft.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + realBaseOffset.y);
				endVerts[4 * i + 0].z = ((CurrentTextInfo.TopLeftVert.z + jitterValue.z + UpdateMesh_waveValueTopLeft.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + realBaseOffset.z);

				endVerts[4 * i + 1].x = ((CurrentTextInfo.TopRightVert.x + jitterValue.x + UpdateMesh_waveValueTopRight.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + realBaseOffset.x);
				endVerts[4 * i + 1].y = ((CurrentTextInfo.TopRightVert.y + jitterValue.y + UpdateMesh_waveValueTopRight.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + realBaseOffset.y);
				endVerts[4 * i + 1].z = ((CurrentTextInfo.TopRightVert.z + jitterValue.z + UpdateMesh_waveValueTopRight.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + realBaseOffset.z);

				endVerts[4 * i + 2].x = ((CurrentTextInfo.BottomRightVert.x + jitterValue.x + UpdateMesh_waveValueBottomRight.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + realBaseOffset.x);
				endVerts[4 * i + 2].y = ((CurrentTextInfo.BottomRightVert.y + jitterValue.y + UpdateMesh_waveValueBottomRight.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + realBaseOffset.y);
				endVerts[4 * i + 2].z = ((CurrentTextInfo.BottomRightVert.z + jitterValue.z + UpdateMesh_waveValueBottomRight.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + realBaseOffset.z);
				
				endVerts[4 * i + 3].x = ((CurrentTextInfo.BottomLeftVert.x + jitterValue.x + UpdateMesh_waveValueBottomLeft.x + UpdateMesh_waveValue.x) + UpdateMesh_lowestLineOffset.x + realBaseOffset.x);
				endVerts[4 * i + 3].y = ((CurrentTextInfo.BottomLeftVert.y + jitterValue.y + UpdateMesh_waveValueBottomLeft.y + UpdateMesh_waveValue.y) + UpdateMesh_lowestLineOffset.y + realBaseOffset.y);
				endVerts[4 * i + 3].z = ((CurrentTextInfo.BottomLeftVert.z + jitterValue.z + UpdateMesh_waveValueBottomLeft.z + UpdateMesh_waveValue.z) + UpdateMesh_lowestLineOffset.z + realBaseOffset.z);

				if (!CurrentTextInfo.isQuad)
				{
					//Assign text UVs
					//OPTO: this only needs to be changed on Rebuild()
					endUv[4 * i + 0] = CurrentTextInfo.ch.uvTopLeft;
					endUv[4 * i + 1] = CurrentTextInfo.ch.uvTopRight;
					endUv[4 * i + 2] = CurrentTextInfo.ch.uvBottomRight;
					endUv[4 * i + 3] = CurrentTextInfo.ch.uvBottomLeft;

					uvMidHold.x = CurrentTextInfo.uvMid.x;
					uvMidHold.y = CurrentTextInfo.uvMid.y;
				}
				else
				{
					//choose whether to use built-in index or an override
					endUv[4 * i + 0] = CurrentTextInfo.quadData.UvTopLeft(GetTimeTime, CurrentTextInfo.quadIndex);
					endUv[4 * i + 1] = CurrentTextInfo.quadData.UvTopRight(GetTimeTime, CurrentTextInfo.quadIndex);
					endUv[4 * i + 2] = CurrentTextInfo.quadData.UvBottomRight(GetTimeTime, CurrentTextInfo.quadIndex);
					endUv[4 * i + 3] = CurrentTextInfo.quadData.UvBottomLeft(GetTimeTime, CurrentTextInfo.quadIndex);

					uvMidHold = CurrentTextInfo.quadData.UvMiddle(GetTimeTime, CurrentTextInfo.quadIndex);

					if (CurrentTextInfo.quadData.columns > 1 && CurrentTextInfo.quadData.animDelay > 0f && CurrentTextInfo.quadIndex < 0)
					{
						areWeAnimating = true;
					}
				}
			}
			var ratioFontTexture = CurrentTextInfo.fontData != null
				? CurrentTextInfo.fontData.font.material.mainTexture
				: font.material.mainTexture;
			
			//combine into one array
			ratioAndUvHold.x = ratioFontTexture.width;
			ratioAndUvHold.y = ratioFontTexture.height;
			ratioAndUvHold.z = uvMidHold.x;
			ratioAndUvHold.w = uvMidHold.y;

			
			//Debug.Log(ratiosAndUvMids.Count);
			ratiosAndUvMids[4 * i + 0] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 1] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 2] = ratioAndUvHold;
			ratiosAndUvMids[4 * i + 3] = ratioAndUvHold;

			//uv rotation data
			ratioAndUvHold.x = endUv[4 * i + 0].x != endUv[4 * i + 3].x ? 1 : 0; //1 if rotated, 0 if not\
			var vertY = Mathf.Sign(CurrentTextInfo.chMaxY);
			if(CurrentTextInfo.isQuad) vertY = -1f;
			ratioAndUvHold.y = CurrentTextInfo.size.y * vertY; //size of letter\
			//ratioAndUvHold.z = CurrentTextInfo.chSize / CurrentTextInfo.fontData.font.; //point size of letter
			
			
			if(CurrentTextInfo.isQuad)
			{
				ratioAndUvHold.z = CurrentTextInfo.quadData.pixelSize.x / ((CurrentTextInfo.quadData.texture.width * CurrentTextInfo.quadData.size.x) / 4.0f);
				ratioAndUvHold.w = CurrentTextInfo.quadData.pixelSize.y / ((CurrentTextInfo.quadData.texture.height * CurrentTextInfo.quadData.size.y) / 4.0f);
			}
			else
			{
				ratioAndUvHold.z = CurrentTextInfo.chSize / ((ratioFontTexture.width) / 4.0f);
				ratioAndUvHold.w = CurrentTextInfo.chSize / ((ratioFontTexture.height) / 4.0f);
			}
			
			
			isUvRotated[4 * i + 0] = ratioAndUvHold;
			isUvRotated[4 * i + 1] = ratioAndUvHold;
			isUvRotated[4 * i + 2] = ratioAndUvHold;
			isUvRotated[4 * i + 3] = ratioAndUvHold;

			
		//Scrolling Textures:
		   //make sure last character isn't a tab, space, or line break.
			if (CurrentTextInfo.textureData != null && (i != iL-1 || (i == iL-1 && CurrentTextInfo.TopRightVert != Vector3.zero))){ //not last character nothing!
				if(CurrentTextInfo.textureData.scrollSpeed != Vector2.zero){
					areWeAnimating = true; //update this every frame
				}

				UpdateMesh_uvOffset.x = GetTimeTime * CurrentTextInfo.textureData.scrollSpeed.x;
				UpdateMesh_uvOffset.y = GetTimeTime * CurrentTextInfo.textureData.scrollSpeed.y;

				//Vector2 uvMulti = Vector2
				
				float uv2Scale = 1f;
				if(CurrentTextInfo.textureData.scaleWithText){
					uv2Scale = 1f / CurrentTextInfo.size.y;
				}

				// Fetch values and store them in existing objects to avoid doing casting
				// or creating new objects.
				cacheVectThree = endVerts[4 * i + 0];
				vectA.x = cacheVectThree.x;
				vectA.y = cacheVectThree.y;

				cacheVectThree = endVerts[4 * i + 1];
				vectB.x = cacheVectThree.x;
				vectB.y = cacheVectThree.y;

				cacheVectThree = endVerts[4 * i + 2];
				vectC.x = cacheVectThree.x;
				vectC.y = cacheVectThree.y;

				cacheVectThree = endVerts[4 * i + 3];
				vectD.x = cacheVectThree.x;
				vectD.y = cacheVectThree.y;

				if (CurrentTextInfo.textureData.relativeToLetter){//keep uvs relative to each letter?
														  //just draw texture as a square
					infoVect.x = CurrentTextInfo.pos.x;
					infoVect.y = CurrentTextInfo.pos.y;

					endUv2[4 * i + 0].x = uv2Scale * (vectA.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 0].y = uv2Scale * (vectA.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 1].x = uv2Scale * (vectB.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 1].y = uv2Scale * (vectB.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 2].x = uv2Scale * (vectC.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 2].y = uv2Scale * (vectC.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 3].x = uv2Scale * (vectD.x - infoVect.x) + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 3].y = uv2Scale * (vectD.y - infoVect.y) + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;
				}
				else
				{
					endUv2[4 * i + 0].x = uv2Scale * vectA.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 0].y = uv2Scale * vectA.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 1].x = uv2Scale * vectB.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 1].y = uv2Scale * vectB.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 2].x = uv2Scale * vectC.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 2].y = uv2Scale * vectC.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;

					endUv2[4 * i + 3].x = uv2Scale * vectD.x + UpdateMesh_uvOffset.x - CurrentTextInfo.textureData.offset.x;
					endUv2[4 * i + 3].y = uv2Scale * vectD.y + UpdateMesh_uvOffset.y - CurrentTextInfo.textureData.offset.y;
				}
			}

			//match UV2 to UV1?
			if(CurrentTextInfo.isQuad){ //quad silhouette?
				if(!CurrentTextInfo.quadData.silhouette){
					endUv2[4*i + 0] = endUv[4*i+0]; //same
					endUv2[4*i + 1] = endUv[4*i+1];
					endUv2[4*i + 2] = endUv[4*i+2];
					endUv2[4*i + 3] = endUv[4*i+3];
				}
			}

			//Color data:
			if (CurrentTextInfo.isQuad && !CurrentTextInfo.quadData.silhouette)
			{ //if it's a quad but not a silhouette
				endCol32[4 * i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
				endCol32[4 * i + 1] = Color.white;
				endCol32[4 * i + 2] = Color.white;
				endCol32[4 * i + 3] = Color.white;
			}
			else if (CurrentTextInfo.gradientData != null)
			{ //gradient speed + gradient spread
				if (CurrentTextInfo.gradientData.scrollSpeed != 0)
				{
					areWeAnimating = true;
				}
				switch (CurrentTextInfo.gradientData.direction)
				{
					case STMGradientData.GradientDirection.Vertical:
						switch (CurrentTextInfo.gradientData.smoothGradient)
						{
							case false: //hard gradient
								endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								break;
							default:
								endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + ((CurrentTextInfo.pos.y + CurrentTextInfo.size.y) * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + ((CurrentTextInfo.pos.y + CurrentTextInfo.size.y) * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (CurrentTextInfo.pos.y * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								break;
						}
						break;
					default: //horizontal
						switch (CurrentTextInfo.gradientData.smoothGradient)
						{
							case false:
								endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f)); //this works!
								endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								break;
							default://smooth gradient
								endCol32[4 * i + 0] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 0].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f)); //this works!
								endCol32[4 * i + 1] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 1].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 2] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 2].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								endCol32[4 * i + 3] = CurrentTextInfo.gradientData.gradient.Evaluate(Mathf.Repeat((GetTimeTime * CurrentTextInfo.gradientData.scrollSpeed) + (endVerts[4 * i + 3].x * CurrentTextInfo.gradientData.gradientSpread / CurrentTextInfo.size.y), 1f));
								break;
						}
						break;
				}
				if(CurrentTextInfo.colorData != null)
				{
					endCol32[4 * i + 0] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 1] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 2] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 3] *= CurrentTextInfo.colorData.color;
				}
			}
			else if (CurrentTextInfo.textureData != null)
			{ //it has a texture
				endCol32[4 * i + 0] = Color.white; //set color to be white, so it doesn't interfere with texture
				endCol32[4 * i + 1] = Color.white;
				endCol32[4 * i + 2] = Color.white;
				endCol32[4 * i + 3] = Color.white;
				
				if(CurrentTextInfo.colorData != null)
				{
					endCol32[4 * i + 0] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 1] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 2] *= CurrentTextInfo.colorData.color;
					endCol32[4 * i + 3] *= CurrentTextInfo.colorData.color;
				}
			}
			else if (CurrentTextInfo.colorData != null)
			{ //use colordata
				endCol32[4 * i + 0] = CurrentTextInfo.colorData.color;
				endCol32[4 * i + 1] = CurrentTextInfo.colorData.color;
				endCol32[4 * i + 2] = CurrentTextInfo.colorData.color;
				endCol32[4 * i + 3] = CurrentTextInfo.colorData.color;
			}
			else
			{ //use default color
				endCol32[4 * i + 0] = color;
				endCol32[4 * i + 1] = color;
				endCol32[4 * i + 2] = color;
				endCol32[4 * i + 3] = color;
			}

			//apply fade.
			endCol32[4 * i + 0].a = (byte)((float)endCol32[4 * i + 0].a * fade);
			endCol32[4 * i + 1].a = (byte)((float)endCol32[4 * i + 1].a * fade);
			endCol32[4 * i + 2].a = (byte)((float)endCol32[4 * i + 2].a * fade);
			endCol32[4 * i + 3].a = (byte)((float)endCol32[4 * i + 3].a * fade);
			//colorArray[i].a = colorArray[i].a * fadeAmount;
			//correct for linear space.
			
			
			if(!uiMode && myColorSpace == 1) //linear space
			{
				endCol32[4 * i + 0] = ((Color)endCol32[4 * i + 0]).linear;
				endCol32[4 * i + 1] = ((Color)endCol32[4 * i + 1]).linear;
				endCol32[4 * i + 2] = ((Color)endCol32[4 * i + 2]).linear;
				endCol32[4 * i + 3] = ((Color)endCol32[4 * i + 3]).linear;			
			}

			
		}

		//If you want to modify vertices (curve, offset, etc) you can do it directly, here
		//ApplyCurveToVertices(endVerts);
		if((onVertexMod != null && onVertexMod.GetPersistentEventCount() > 0) || OnVertexMod != null){
			//Debug.Log("Updating vertex mod");
			if (UpdateMesh_Middles.Length != hyphenedText.Length)
				Array.Resize(ref UpdateMesh_Middles, hyphenedText.Length); //Update the array with the middle of each letter

			if (UpdateMesh_Positions.Length != hyphenedText.Length)
				Array.Resize(ref UpdateMesh_Positions, hyphenedText.Length);

			for(int i=0, iL=hyphenedText.Length; i<iL; i++){
				CurrentTextInfo = info[i];
				UpdateMesh_Middles[i] = CurrentTextInfo.Middle;
				UpdateMesh_Positions[i] = CurrentTextInfo.pos;
			}
			if(onVertexMod != null) onVertexMod.Invoke(endVerts, UpdateMesh_Middles, UpdateMesh_Positions); //modify end verts externally
			if(OnVertexMod != null) OnVertexMod.Invoke(endVerts, UpdateMesh_Middles, UpdateMesh_Positions);
			areWeAnimating = true; //just in case, so things like the sketch effect work.
		}
		//TODO: assign normals by hand instead of using this. but really, whatever. You dont need normals.
		//mesh.RecalculateNormals(); //2016-07-05 i dont need to do this
		//}
		if(data.disableAnimatedText || disableAnimatedText){
			areWeAnimating = false; //override constant updates
		}
		//else{
			//mesh.Optimize(); //not sure if this would actually help, since verts will rarely be shared
		//}
		//return mesh;
	}
	/* 
	public void SetColor(Color32 setColor)
	{

	}
	*/
	public void SetMesh(float timeValue){
		SetMesh(timeValue, false);
	}
	//actually update the mesh attached to the meshfilter
	
	void SetMesh(float timeValue, bool undrawingMesh) //0 == start mesh, < 0 == end mesh, > 0 == midway mesh
	{
		if(textMesh == null)
		{
			textMesh = new Mesh(); //create the mesh initially
			textMesh.MarkDynamic(); //just do it
		}
		textMesh.Clear();
		if(text.Length > 0)
		{
			if(reading || unreading)//which set to use...?
			{ 
				UpdateDrawnMesh(timeValue, undrawingMesh);
				textMesh.vertices = midVerts;
				textMesh.colors32 = midCol32;
			}
			else if(timeValue == 0f || undrawingMesh){//show nothing
			
				UpdatePreReadMesh(undrawingMesh); //pas this so it know which animation to use. always renders a pre-read mesh
				textMesh.vertices = startVerts;
				textMesh.colors32 = startCol32;
				//Debug.Log("showing empty");
			}
			else
			{
				UpdateMesh(totalReadTime+1f);
				textMesh.vertices = endVerts;
				textMesh.colors32 = endCol32;
				//Debug.Log("showing filled");
			}

			//Debug.Log("text mesh verts: " + midVerts.Length + " enduv length: " + endUv.Length);

			
			textMesh.uv = endUv; 
			textMesh.uv2 = endUv2; //use 2nd texture...
			//textMesh.uv3 = ratios; //for new shader!
			textMesh.SetUVs(2, ratiosAndUvMids);
			textMesh.SetUVs(3, isUvRotated);
			
			//apply tris and submeshes
			
			if(submeshes.Count > 1)
			{ 
				//use submeshes instead of setting triangles for entire mesh:
				textMesh.subMeshCount = submeshes.Count;
				for(int i=0, iL=textMesh.subMeshCount; i<iL; i++)
				{
					textMesh.SetTriangles(submeshes[i].tris, i); //apply to mesh
				}
			}
			else if(submeshes.Count > 0)
			{
				//do it this way because of errors with quads
				textMesh.subMeshCount = 1;
				//set triangles for entire mesh:
				//textMesh.triangles = SubMeshes_subMeshes[0].tris.ToArray();
				//Debug.Log(SubMeshes_subMeshes.Count);
				textMesh.SetTriangles(submeshes[0].tris, 0); //causes less garbage?
			}
			//else, do nothing!!
			//textMesh.isReadable = true;
			textMesh.UploadMeshData(false); //send to graphics API manually...?
		}
		ApplyMesh();
	}
	void ApplyMesh()
	{
		if(uiMode) //UI mode
		{
			c.SetMesh(textMesh);
		}
		else
		{
			f.sharedMesh = textMesh; //I dont think this has to be set multiple times but w/e
		}
	}
	[ContextMenu("Clear Materials")]
	public void ClearMaterials()
	{

		//clear r.sharedMaterials, here
		if(uiMode)
		{
			for(int i=0, iL=c.materialCount; i<iL; i++)
			{
				DestroyImmediate(c.GetMaterial(i));
			}
			c.materialCount = 0;
		}
		else
		{
			for(int i=0, iL=r.sharedMaterials.Length; i<iL; i++)
			{
				DestroyImmediate(r.sharedMaterials[i]);
			}
		}

		//for(int i=0, iL=allMaterials.Count; i<iL; i++)
		//{
		//	DestroyImmediate(allMaterials[i]);
		//}
		SharedMaterialDataStorage.allMaterials.Clear();

	}


	

	private Canvas parentCanvas;
	void ApplyMaterials() //turn submesh data into material data
	{
		//do a check first to see if materials need to change
		//ClearMaterials();
		if (submeshMaterials.Length != submeshes.Count)
			Array.Resize(ref submeshMaterials, submeshes.Count);

		//submeshMaterials = new Material[submeshes.Count]; //material array just for this mesh
		for(int i=0, iL=submeshMaterials.Length; i<iL; i++)
		{
			//assign existing material or create a new one here....
			//submeshMaterials[i] = SubMeshes_subMeshes[i].AsMaterial;
			//different details will have to be set here,
	//		submeshMaterials[i] = allMaterials[submeshes[i].materialIndex].AsMaterial;
			submeshMaterials[i] = submeshes[i].sharedMaterialData.AsMaterial;
			//submeshMaterials[i] = submeshes[i].sharedMaterialData.material;
		}
		if(uiMode)//for now, simple way to disallow multiple materials on canvas, since it seems to cause a crash
		{
			//2017-02-12 fixed it??
			//2017-04-14 FIXED IT
			if(this != null && t.gameObject.activeInHierarchy)//prevents text from rendering weird
			{ 
				
				//Debug.Log(t.name + ": " + maskValue);
				#if UNITY_2017_1_OR_NEWER
				parentCanvas = t.GetComponentInParent<Canvas>();
				if(parentCanvas != null)
				{
					parentCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
					parentCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
					parentCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord3;
				}
				c.materialCount = submeshMaterials.Length+1;
				for(int j=0; j<c.materialCount-1; j++)
				{
					//Debug.Log(submeshMaterials[j].name);
					c.SetMaterial(submeshMaterials[j],j);
				}
				#else
				//only show 1st material, multi materials were not supported before 2017.1
				c.materialCount = 1;
				if(submeshMaterials.Length > 0)
				{
					c.SetMaterial(submeshMaterials[0],0);
				}
				#endif
			}
		}
		else
		{
			r.sharedMaterials = submeshMaterials; //update!
		}
		#if UNITY_EDITOR
		HideInspectorStuff(); //this is the only time you're really gonna need this, so OnValidate() makes sense...?
		#endif
	}

	//private int Submesh_Count = 0;

	//tris for submeshes on THIS mesh to use
	private List<SubmeshData> submeshes = new List<SubmeshData>();

	private Material[] submeshMaterials = new Material[1]; //to be used when applying materials to a mesh


	

	//SubmeshData SubmeshExist_submesh = null;
	SubmeshData DoesSubmeshExist(SharedMaterialData materialData)
	{
		for(int i=0; i<submeshes.Count; i++)
		{
			if(submeshes[i].sharedMaterialData == materialData)
			{
			//	Debug.Log("Found a matching submesh and it has this material: " + submeshes[i].sharedMaterialData.debugOriginalName);
				return submeshes[i];
			}
		}
		return null;
	}

	//private SubMeshData SubMesh_Submesh;
	
	//private List<SubMeshData> SubMeshes_subMeshes = new List<SubMeshData>();
	private SharedMaterialData Submesh_sharedMaterial = null;
	private SubmeshData Submesh_submeshData = null;
	private STMTextInfo Submesh_info;
	void PrepareSubmeshes()
	{
		//since this only needs to be calculated during Rebuild(), putting this in its own function.
	//	SubMeshes_subMeshes.Clear();
		//SubMeshes_subMeshes = new List<SubMeshData>(); //include default submesh
	//	SubMeshes_subMeshes.Add(new SubMeshData(this)); //add default submesh
	//	Submesh_Count = 1;
		//assign default
		submeshes.Clear();

		if(info.Count > 0 && info[0] != null/* && !info[0].submeshChange*/)
		{
			Submesh_sharedMaterial = SharedMaterialDataStorage.DoesSharedMaterialExist(this);
			if(Submesh_sharedMaterial == null)
			{
				//Submesh_sharedMaterial = new SharedMaterialData(this);
				SharedMaterialDataStorage.allMaterials.Add(new SharedMaterialData(this));
				Submesh_sharedMaterial = SharedMaterialDataStorage.allMaterials[SharedMaterialDataStorage.allMaterials.Count-1];
			}
			Submesh_submeshData = DoesSubmeshExist(Submesh_sharedMaterial);
			if(Submesh_submeshData == null)
			{
				//Submesh_submeshData = new SubmeshData(submeshes.Count);
				submeshes.Add(new SubmeshData(Submesh_sharedMaterial));
				Submesh_submeshData = submeshes[submeshes.Count-1];
			}
		}



		

		//cache a submesh to check against, and assign a default
		//SubMesh_Submesh = SubMeshes_subMeshes[0];
		for(int i=0, iL=hyphenedText.Length; i<iL; i++) //go thru all info
		{
			Submesh_info = info[i];
			if(Submesh_info.submeshChange) //can still potentially return null, which is good
			{
				//Debug.Log("found a material change at " + i);
				//This also only needs to be changed on rebuild(), move it sometime 2016-10-26 TODO
				//get and assign submesh/triangles for this letter
				//SubMesh_Submesh = DoesSubmeshExist(this,Submesh_info);
				Submesh_sharedMaterial = SharedMaterialDataStorage.DoesSharedMaterialExist(this, Submesh_info); //is there a submesh for this texture yet?
			}
			//Debug.Log("This info's font is " + Submesh_info.fontData);
			if(Submesh_sharedMaterial == null) //doesn't exist yet??
			{
				//Submesh_sharedMaterial = new SharedMaterialData(this, Submesh_info); //create new universal material
				SharedMaterialDataStorage.allMaterials.Add(new SharedMaterialData(this, Submesh_info)); //and add to submesh list
				Submesh_sharedMaterial = SharedMaterialDataStorage.allMaterials[SharedMaterialDataStorage.allMaterials.Count-1];

				
			}
			if(Submesh_info.submeshChange)
			{
				Submesh_submeshData = DoesSubmeshExist(Submesh_sharedMaterial);
				
			}
			if(Submesh_submeshData == null)
			{
				Submesh_submeshData = new SubmeshData(Submesh_sharedMaterial);
				submeshes.Add(Submesh_submeshData);
				//Submesh_Count++;
			}

			//vvvv doing is this way creates garbage
			//SubMesh_Submesh.tris.AddRange(new int[]{4*i+0,4*i+1,4*i+2,4*i+0,4*i+2,4*i+3}); //add tris for this letter
			//vvvv this way seems fine tho
			Submesh_submeshData.tris.Add(4*i + 0);
			Submesh_submeshData.tris.Add(4*i + 1);
			Submesh_submeshData.tris.Add(4*i + 2);
			Submesh_submeshData.tris.Add(4*i + 0);
			Submesh_submeshData.tris.Add(4*i + 2);
			Submesh_submeshData.tris.Add(4*i + 3);
		}
		//subMeshes = new SubMeshData[subMeshCount]; //create an array to hold all these sebmeshes
	}

	//ILayoutElement Stuff. Content Size Fitter.
	//i Don't think these two are needed since accesors are used
	public virtual void CalculateLayoutInputHorizontal() {}
	public virtual void CalculateLayoutInputVertical() {}
	public virtual float minWidth{
		get { return 0; }
	}
	public virtual float preferredWidth{
		get{
			return unwrappedBottomRightTextBounds.x;
			//return (float)tr.rect.width;
		}
	}
	public virtual float flexibleWidth { get { return -1; } }
	public virtual float minHeight{
		get { return 0; }
	}
	public virtual float preferredHeight{
		get{
			//Rebuild();
			return -rawBottomRightTextBounds.y;
			//return (float)tr.rect.height;
		}
	}
	public virtual float flexibleHeight { 
		get { 
			return -rawBottomRightTextBounds.y; 
		} 
	}
	public virtual int layoutPriority { get { return 0; } }

	void OnRectTransformDimensionsChange()
	{
		if(gameObject.activeInHierarchy && uiMode)
		{
			SpecialRebuild();
		}
	}

	public void RecalculateMasking()
	{
		if(!gameObject.activeInHierarchy) return;
		UpdateMaskingOnAllSubmeshes();
		ApplyMaterials();
		//SpecialRebuild(); don't do a full rebuild...
	}
	
	void UpdateMaskingOnAllSubmeshes()
	{
		foreach(var submesh in submeshes)
		{
			submesh.sharedMaterialData.SetMaskingRelatedValues(this);
		}
	}
	
	
}
[System.Serializable]
public class SubmeshData
{
	public SharedMaterialData sharedMaterialData; //material to be used by this submesh
	//public int materialIndex = -1;
	public List<int> tris = new List<int>();
/*
	public SubmeshData(int index)
	{
		this.materialIndex = index;

	}
*/
	public SubmeshData(SharedMaterialData data)
	{
		this.sharedMaterialData = data;
	}
}
[System.Serializable]
public static class SharedMaterialDataStorage
{
	//list of materials, shared with all text meshes. this is asking for trouble...
	public static List<SharedMaterialData> allMaterials = new List<SharedMaterialData>();

	private static Transform Submesh_maskCanvas;
	private static float Submesh_stencilDepth;
	private static SharedMaterialData MaterialExists_material = null;
	public static SharedMaterialData DoesSharedMaterialExist(SuperTextMesh stm, STMTextInfo info) //find a material that this character can exist on
	{
		//masking
		if(stm.uiMode)
		{
			Submesh_maskCanvas = MaskUtilities.FindRootSortOverrideCanvas(stm.t);
			Submesh_stencilDepth = MaskUtilities.GetStencilDepth(stm.t, Submesh_maskCanvas);
		}
		else
		{
			Submesh_stencilDepth = -1f;
		}
	//	Debug.Log("Finding material for a submesh on " + stm.t.name);
		//return subMeshes[0]; //debugging...
		for(int i=0; i<allMaterials.Count; i++)
		{
			//cache a material to check against
			MaterialExists_material = allMaterials[i];
			if(MaterialExists_material == null)
			{
				continue;
			}
			//bool safe = true;
			//Debug.Log("Trying out material named " + MaterialExists_material.debugOriginalName + " Hoping to find something with this font: " + MaterialExists_material.refFont.name + " and this texture: " + (MaterialExists_material.refTex == null ? "ITS NULL" : MaterialExists_material.refTex.name));
		//does this data have material data attached?
			if(info.materialData != null)//it has material data?
			{ 
				if(MaterialExists_material.refMat != info.materialData.material) //if the two materials dont match
				{
					//Debug.Log("materialdata was not null, but refmat did not match.");
					continue;
				}
			}
			else//there's no material data on this letter, so compare to STM default
			{ 
				if(MaterialExists_material.refMat != stm.textMaterial)
				{ //
					//Debug.Log("material data was not null, but didn't match STM's material");
					continue;
				}
			}

			//does masking value match
			if(MaterialExists_material.uiStencilDepth != Submesh_stencilDepth)
			{
				continue;
			}
			if(MaterialExists_material.uiMaskMode != stm.maskMode)
			{
				continue;
			}


			if(info.fontData != null)
			{
				if(info.quadData != null)
				{
					continue;
				}
				if(MaterialExists_material.refFont == null)
				{
					continue;
				}
				if(MaterialExists_material.refFont != info.fontData.font)
				{
					//return SubMesh_mySubMesh;
					//safe = false;
					//Debug.Log("font data was not null but ref font didn't match info font.");
					continue;
					//Debug.Log("Existing fonts dont match.");
				}
				if(info.fontData.overrideFilterMode)
				{
					if(info.quadData != null && MaterialExists_material.refFilter != info.quadData.filterMode) //if filter mode doesn't match
					{
						continue;
					}
				}
				else
				{
					if(MaterialExists_material.refFilter != stm.filterMode) //if filter mode doesn't match
					{
						continue;
					}
				}
				/*
				//you shouldn't be using mismatched filter modes on the same font anyway!!
				if(info.fontData.overrideFilterMode)
				{
					if(MaterialExists_material.refFilter != info.fontData.filterMode) //if filter mode doesn't match
					{
					//	Debug.Log("Woah no matching filter mode");
						continue;
					}
				}
				else
				{
					if(MaterialExists_material.refFilter != stm.filterMode) //if filter mode doesn't match
					{
						continue;
					}
				}
				*/
			}
			//submesh data ALWAYS has font, fontdata might not
			else //no fontdata on the mesh?
			{
				//TODO: check for silhouette differences, too?
				if(info.quadData != null)//if it has quad data 
				{ 
					if(MaterialExists_material.refTex == null)
					{
						continue;
					}
					if(MaterialExists_material.refTex != info.quadData.texture) //if the two textures aren't the same...
					{
						//Debug.Log("info quad data was not null but material's reftex did not match the quad texture");
						continue;
						//safe = false;
					}
					if((MaterialExists_material.refTex == MaterialExists_material.refMask) == info.quadData.silhouette) //if they're not both a silhouette
					{
						//Debug.Log("quad data was not null but something w silhouettes");
						continue;
						//safe = false;
					}
					
					
			
				}
				else//no quad data
				{ 
					if(MaterialExists_material.refFont != stm.font)
					{
						//Debug.Log("font data was null, and material had a font that didn't match stm's");
						continue;
						//Debug.Log("non-Existing fonts dont match.");
					}
					if(MaterialExists_material.refTex != null) //but the submesh does have it
					{
						//Debug.Log("no quad data but ref texture was not null");
						continue;
						//safe = false;
					}
					if(MaterialExists_material.refFilter != stm.filterMode)
					{
						continue;
					}
				}
			}
			

			if(info.textureData != null) //there's texture data?
			{
				if(MaterialExists_material.refMask != info.textureData.texture) //if the two textures dont match...
				{
					//return SubMesh_mySubMesh;
					//Debug.Log("Existing textures dont match.");
					//safe = false; //not the same!
					//Debug.Log("info texture data was not null, but refmask didn't match texturedata");
					continue;
				}
			}
			else //no texture data so...
			{
				/*
				if(MaterialExists_material.refMask != null) //if this material DOES have a texture, dont match
				{
					continue;
				}
				*/
				/*
				//vvv check for this, since quads can use the refmask, too
				if(info.quadData == null && MaterialExists_material.refTex != null) //if this submesh has texture data (is a quad), is not null too
				{
				//	Debug.Log("info texture data was null, but quad was null and ref mask was not. but whatever");
					//safe = false;
					continue;
					//Debug.Log("non-Existing textures dont match.");
				}
				*/

			}
			
			//Debug.Log("Found a matching material! " + MaterialExists_material.materialName);
			return MaterialExists_material; //the two submeshes are the same!
		}
		//Debug.Log("Did NOT find existing material");
		//return new SubMeshData(stm, info);
		return null;
	}
	public static SharedMaterialData DoesSharedMaterialExist(SuperTextMesh stm) //find a material that this character can exist on
	{
		//masking
		if(stm.uiMode)
		{
			Submesh_maskCanvas = MaskUtilities.FindRootSortOverrideCanvas(stm.t);
			Submesh_stencilDepth = MaskUtilities.GetStencilDepth(stm.t, Submesh_maskCanvas);
		}
		else
		{
			Submesh_stencilDepth = -1f;
		}
	//	Debug.Log("Finding material for STM named " + stm.t.name);
		//return subMeshes[0]; //debugging...
		for(int i=0; i<allMaterials.Count; i++)
		{
			//cache a material to check against
			MaterialExists_material = allMaterials[i];
			if(MaterialExists_material == null)
			{
				//Debug.Log("material is null");
				continue;
			}
			//Debug.Log("Okay existing material is not null. So checking material named... " + MaterialExists_material.debugOriginalName);
			//bool safe = true;

			if(MaterialExists_material.refMat != stm.textMaterial)
			{ //
				//Debug.Log("ref material doesn't match");
				continue;
			}

			//does masking value match
			if(MaterialExists_material.uiStencilDepth != Submesh_stencilDepth)
			{
				//Debug.Log("ui mask value doesn't match but whatever");
				continue;
			}
			if(MaterialExists_material.uiMaskMode != stm.maskMode)
			{
				continue;
			}

			if(MaterialExists_material.refMask != null) //there's texture data?
			{
				//Debug.Log("ref mask is NOT null!");
				continue;
			}

			if(MaterialExists_material.refFont != stm.font)
			{
				//Debug.Log("ref font does not match stm font");
				continue;
				//Debug.Log("non-Existing fonts dont match.");
			}

			if(MaterialExists_material.refTex != null) //if the two textures aren't the same...
			{
				//Debug.Log("ref Texture is NOT null");
				continue;
				//safe = false;
			}
			if(MaterialExists_material.refFilter != stm.filterMode)
			{
				//Debug.Log("Filter mode does NOT match default");
				continue;
			}
			if(MaterialExists_material.refMask != null) //if they're not both a silhouette
			{
				//Debug.Log("refmask is NOT null");
				continue;
				//safe = false;
			}	

			//Debug.Log("Found a matching material! " + MaterialExists_material.debugOriginalName);
			return MaterialExists_material; //the two submeshes are the same!
		}
		//return new SubMeshData(stm, info);
		return null;
	}
}
[System.Serializable]
public class SharedMaterialData  //used internally by STM for keeping track of shared materials
{
	private Transform _maskCanvas;
	private Canvas _canvas;

	private SuperTextMesh _stm;
	//public string name;
	//public List<int> tris = new List<int>(); 
	public Material refMat; //material these tris will reference. not the actual material being used, but the one a new material will be created from.
	public Font refFont; //maybe make these FontData, TextureData, ShaderData?
	public Texture refTex; //for quads/inline images
	public Texture refMask; //masks/textures/non-silhouette quads
	public Vector2 maskTiling;
	public FilterMode refFilter;
	public float uiStencilDepth;
	public SuperTextMesh.MaskMode uiMaskMode;

	private Material material;
	//public string debugOriginalName = "";

	//public Material createdMaterial; //is this the best place to store it...?
	
	public SharedMaterialData(SuperTextMesh stm){ //create default
		SetValues(stm);
	//	debugOriginalName = "1: " + refMat.name + " - " + refFont.name + " - " + refFilter;
		//SetMaterial();
	}
	public SharedMaterialData(SuperTextMesh stm, STMTextInfo info){ //from different data types
		SetValues(stm,info);
	//	debugOriginalName = "2: " + refMat.name + " - " + refFont.name + " - " + refFilter;
		//SetMaterial();
	}
	public void SetValues(SuperTextMesh stm)
	{
		this._stm = stm;
		this.refMat = stm.textMaterial; //default text material
		this.refFont = stm.font;
		this.refFilter = stm.filterMode;
		//masking
		if(stm.uiMode)
		{
			_maskCanvas = MaskUtilities.FindRootSortOverrideCanvas(stm.tr);
			_canvas = stm.tr.GetComponentInParent<Canvas>();
			this.uiStencilDepth = MaskUtilities.GetStencilDepth(stm.tr, _maskCanvas);
			this.uiMaskMode = stm.maskMode;
		}
		else
		{
			this.uiStencilDepth = -1;
			//for new non-ui shaders... set this value based on whether parent has a mask
			#if UNITY_2017_1_OR_NEWER
			//sprite masks were introduced in this version~
			var parentSpriteMask = stm.t.GetComponentInParent<SpriteMask>();
			if(parentSpriteMask != null && parentSpriteMask.enabled)
			{
				//only activate under these circumstances
				this.uiStencilDepth = 1;
				this.uiMaskMode = stm.maskMode;
			}
			#endif
		}
	}
	public void SetValues(SuperTextMesh stm, STMTextInfo info)
	{
		this._stm = stm;
		//this.refMask = texData.texture;
		this.refMat = info.materialData != null ? info.materialData.material : stm.textMaterial;
		this.refFont = info.fontData != null ? info.fontData.font : stm.font;
		//this.refFilter = info.quadData != null ? info.quadData.filterMode : info.fontData != null ? info.fontData.overrideFilterMode ? info.fontData.filterMode : stm.filterMode;
		//this one's so long... just write it out this way
		if(info.isQuad)
		{
			this.refFont = null; //no font anymore
			if(info.quadData.overrideFilterMode)
			{
				this.refFilter = info.quadData.filterMode;
			}
			else
			{
				this.refFilter = stm.filterMode;
			}
		}
		else if(info.fontData != null)
		{
			if(info.fontData.overrideFilterMode)
			{
				this.refFilter = info.fontData.filterMode;
			}
			else
			{
				this.refFilter = stm.filterMode;
			}
		}
		else
		{
			this.refFilter = stm.filterMode;
		}
		this.refMask = info.textureData != null ? info.textureData.texture : null;
		this.maskTiling = info.textureData != null ? info.textureData.tiling : Vector2.one;
		if(info.isQuad && !info.quadData.silhouette) //nah, use quad instead...
		{
			this.refMask = info.quadData.texture;
		}
		this.refTex = info.isQuad ? info.quadData.texture : null;
		if(info.isQuad)
		{
			this.maskTiling = Vector2.one;
		}
		SetMaskingRelatedValues(stm);
		
	}
	public void SetMaskingRelatedValues(SuperTextMesh stm)
	{
		//masking
		if(stm.uiMode)
		{
			_maskCanvas = MaskUtilities.FindRootSortOverrideCanvas(stm.tr);
			_canvas = stm.tr.GetComponentInParent<Canvas>();
			this.uiStencilDepth = MaskUtilities.GetStencilDepth(stm.tr, _maskCanvas);
			this.uiMaskMode = stm.maskMode;
		}
		else
		{
			this.uiStencilDepth = -1;
#if UNITY_2017_1_OR_NEWER
			//sprite masks were introduced in this version~
			var parentSpriteMask = stm.t.GetComponentInParent<SpriteMask>();
			if(parentSpriteMask != null && parentSpriteMask.enabled)
			{
				//only activate under these circumstances
				this.uiStencilDepth = 1;
			}
#endif
		}
	}
	private int MaskDepthToID()
	{
		if(uiStencilDepth >= 8)
		{
			Debug.Log("Attempting to use a mask with depth >= 8");
			return 0;
		}
		//proper stencil buffer, minus 1 for outside mask
		var outsideMask = _stm.maskMode == SuperTextMesh.MaskMode.Outside ? -1 : 0;
		return (int)(Mathf.Pow(2,uiStencilDepth)-1+outsideMask);
	}
	public string materialName;
	public Material AsMaterial
	{
		get
		{

			if(material == null)
			{
				material = new Material(refMat.shader);//create new material
			}
			else
			{
				if(material.shader != refMat.shader) //in Unity 2018.4+, apparently this prevents GC from building up.
				{
					material.shader = refMat.shader;
				}
			}
			
			//Material newMat = new Material(refMat.shader);
			material.CopyPropertiesFromMaterial(refMat);
			/*
			//this is a nice thought, but you can't read a font texture unfortunately!

			//if texture filter mode doesn't match ref filter, create new texture
			#if UNITY_5_4_OR_NEWER
			//test this in a later unity version!
			Graphics.CopyTexture(refTex ?? refFont.material.mainTexture, this.texture);
			#else
			//texture = new Texture2D((refTex ?? refFont.material.mainTexture).width, (refTex ?? refFont.material.mainTexture).height);
			texture.Resize((refTex ?? refFont.material.mainTexture).width, (refTex ?? refFont.material.mainTexture).height);
			pixels = ((Texture2D)(refTex ?? refFont.material.mainTexture)).GetPixels32();
			texture.SetPixels32(pixels);
			texture.wrapMode = (refTex ?? refFont.material.mainTexture).wrapMode;
			texture.Apply();
			#endif
			material.SetTexture("_MainTex", texture); //go w/ reftex unless its null, then use font
			*/
			material.SetTexture("_MainTex", refTex ?? refFont.material.mainTexture); //go w/ reftex unless its null, then use font
			material.SetTexture("_MaskTex", refMask);
			material.SetTextureScale("_MaskTex", maskTiling);

			

			if(material.HasProperty("_BaseMap"))
			{
				if(material.GetTexture("_BaseMap") != null)
				{
					material.GetTexture("_BaseMap").filterMode = refFilter;
				}
			}
			else if(material.HasProperty("_MainTex"))
			{
				if(material.GetTexture("_MainTex") != null)
				{
					material.GetTexture("_MainTex").filterMode = refFilter;
				}
			}
			
			//manually set ZTestMode.
			if(material.HasProperty("_ZTestMode"))
			{
				if(_stm.uiMode)
				{
					//unity_GUIZTestMode = 4, unless canvas is set to screen space overlay, then 8
					var zTestMode = 4;
					if(_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						zTestMode = 8;
					}
					material.SetInt("_ZTestMode", zTestMode);
				}
				else if(material.GetInt("_ZTestMode") != 4 && material.GetInt("_ZTestMode") != 8)
				{
					//reset to normal (don't render on top, 2)
					material.SetInt("_ZTestMode", 4);
				}
			}
			//masking
			//ApplyMasking(returnMaterial, uiMaskValue);
			
			if(uiStencilDepth > -1f)
			{
				//3 if masking, 8 if not
				//old code: uiStencilDepth > 0 ? 3f : 8f
				var maskValue = 8;
				if(uiStencilDepth > 0)
				{
					//may have to change this...
					if(uiMaskMode == SuperTextMesh.MaskMode.Inside)
						maskValue = _stm.uiMode ? 3 : 4;
					else if(uiMaskMode == SuperTextMesh.MaskMode.Outside)
						maskValue = 7;
					else
						maskValue = 0;
				}
				material.SetInt("_StencilComp", maskValue);
				//material.SetInt("_MaskComp", maskValue);
				
				//convert always
				material.SetInt("_Stencil", MaskDepthToID());
				//material.SetInt("_MaskMode", MaskDepthToID());
				//always 0
				material.SetInt("_StencilOp", 0);
				//material.SetInt("_MaskOp", 0);
				//0 if masking, 255 if not
				material.SetInt("_StencilWriteMask", uiStencilDepth > 0 ? 255 : 0);
				//material.SetInt("_WriteMask", uiStencilDepth > 0 ? 255 : 0);
				//convert if masking, otherwise 255
				material.SetInt("_StencilReadMask", uiStencilDepth > 0 ? 255 : 0);
				//material.SetInt("_ReadMask", uiStencilDepth > 0 ? 255 : 0);
			}

			if(material.HasProperty("_FakeTexelSize"))
			{
				var mainTex = material.GetTexture("_MainTex");
				//Debug.Log("setting fake texel size");
				material.SetVector("_FakeTexelSize", new Vector4(1f /mainTex.width,
																1f / mainTex.height,
																mainTex.width,
																mainTex.height));
			}

			materialName = "";
			materialName += refMat != null ? refMat.name : "NULL MATERTIAL";
			materialName += " - ";
			if(refFont != null)
			{
				materialName += refFont.name;
			}
			else
			{
				if(refTex != null)
				{
					//materialName += "Quad:";
					if(refMask != null)
					{
						materialName += refTex.name + "|" + refMask.name;
					}
					else
					{
						materialName += refTex.name + "|SILHOUETTE";
					}
				}
				else
				{
					materialName += "NULL";
				}
			}
			materialName += " - ";
			materialName += refFilter;

			material.name = materialName;

			//Debug.Log("Just created a new material. See: " + material);
			return material;
		}
	}
	//LayoutElement Garbage

	//public virtual void CalculateLayoutInputHorizontal() {}
	//public virtual void CalculateLayoutInputVertical() {}
	//public float
}
