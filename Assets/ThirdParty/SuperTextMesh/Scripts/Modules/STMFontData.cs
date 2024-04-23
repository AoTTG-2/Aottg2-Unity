//Copyright (c) 2016-2018 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Font Data", menuName = "Super Text Mesh/Font Data", order = 1)]
public class STMFontData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	public Font font;
	[Tooltip("if new quality level should be used, or to use mesh default. Automatically disabled for non-dynamic fonts.")]
	public bool overrideQuality = false;
	[Tooltip("Only affects dynamic fonts.")]
	[Range(1,512)] public int quality = 64; 
	[Tooltip("Whether or not the filter mode should be overridden for this font. Be wary that having the same font use different filter modes in a scene might render strange.")]
	public bool overrideFilterMode = false;
	public FilterMode filterMode = FilterMode.Bilinear; //default

	public STMFontData(Font font){
		this.font = font;
	}

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Font Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(serializedData.FindProperty("font"));
		if(this.font != null){
			if(!this.font.dynamic){
				EditorGUI.BeginDisabledGroup(!this.font.dynamic);
				EditorGUILayout.PropertyField(serializedData.FindProperty("overrideQuality")); //for show
				this.quality = EditorGUILayout.IntSlider("Quality",this.font.fontSize,1,512); //set to default
				EditorGUI.EndDisabledGroup();
			}else{
				EditorGUILayout.PropertyField(serializedData.FindProperty("overrideQuality"));
				EditorGUI.BeginDisabledGroup(!this.overrideQuality);
				EditorGUILayout.PropertyField(serializedData.FindProperty("quality"));
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.PropertyField(serializedData.FindProperty("overrideFilterMode"));
			EditorGUI.BeginDisabledGroup(!this.overrideFilterMode);
			EditorGUILayout.PropertyField(serializedData.FindProperty("filterMode"));
			EditorGUI.EndDisabledGroup();
		}
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}