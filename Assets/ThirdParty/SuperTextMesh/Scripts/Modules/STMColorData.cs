//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Color Data", menuName = "Super Text Mesh/Color Data", order = 1)]
public class STMColorData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	public Color color = Color.white;
	public STMColorData(){
		this.color = Color.white; //sure?
	}
	public STMColorData(Color color){
		this.color = color;
	}
	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Color Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty color = serializedData.FindProperty("color");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(color);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}