//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Audio Clip Data", menuName = "Super Text Mesh/Audio Clip Data", order = 1)]
public class STMAudioClipData : ScriptableObject{
	public bool showFoldout = true;
	//public string name;
	public AudioClip[] clips;

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Audio Clip Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty clips = serializedData.FindProperty("clips");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this, stm);
	//the rest:
		EditorGUILayout.PropertyField(clips,true);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}