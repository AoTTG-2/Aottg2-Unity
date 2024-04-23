//Copyright (c) 2016-2023 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Delay Data", menuName = "Super Text Mesh/Delay Data", order = 1)]
public class STMDelayData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	[Tooltip("Amount of additional delays to be applied. eg. If text delay is normally 0.1 and this value is 3, it will cause a delay of 0.4 seconds in total. (0.1 + (3 * 0.1))")]
	public int count = 0;
	
	#if UNITY_EDITOR
	public virtual void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Delay Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty count = serializedData.FindProperty("count");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(count);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}