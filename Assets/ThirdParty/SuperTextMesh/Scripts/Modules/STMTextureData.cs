//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Gradient Data", menuName = "Super Text Mesh/Texture Data", order = 1)]
public class STMTextureData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	public Texture texture;
	public FilterMode filterMode;
	public bool relativeToLetter = false; //will the texture be relative to each letter
	public bool scaleWithText = false;
	public Vector2 tiling = Vector2.one; //or scale
	public Vector2 offset = Vector2.zero;
	public Vector2 scrollSpeed = Vector2.one;
	//public float speed = 0.5f; //scroll speed
	//public float spread = 0.1f; //how far it stretches, in local

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Texture Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty texture = serializedData.FindProperty("texture");
		SerializedProperty filterMode = serializedData.FindProperty("filterMode");
		SerializedProperty relativeToLetter = serializedData.FindProperty("relativeToLetter");
		SerializedProperty scaleWithText = serializedData.FindProperty("scaleWithText");
		SerializedProperty tiling = serializedData.FindProperty("tiling");
		SerializedProperty offset = serializedData.FindProperty("offset");
		SerializedProperty scrollSpeed = serializedData.FindProperty("scrollSpeed");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(texture);
		EditorGUILayout.PropertyField(filterMode);
		EditorGUILayout.PropertyField(relativeToLetter);
		EditorGUILayout.PropertyField(scaleWithText);
		EditorGUILayout.PropertyField(tiling);
		EditorGUILayout.PropertyField(offset);
		EditorGUILayout.PropertyField(scrollSpeed);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}