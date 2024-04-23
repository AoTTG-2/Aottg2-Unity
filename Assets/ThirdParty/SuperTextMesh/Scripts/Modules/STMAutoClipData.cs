//Copyright (c) 2016-2023 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Auto Clip Data", menuName = "Super Text Mesh/Audo Clip Data", order = 1)]
public class STMAutoClipData : ScriptableObject{ //for auto-clips. replacing text sounds on specific characters
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	
	public Type type = Type.Character;
	//[TextArea(2,3)]
	//public string character;
	//public bool ignoreCase = true; //lets just make it always ignore case
	public char character;
	public string quadName; //for matching to a quad
	//public bool caseSensitive = false;
	public AudioClip clip;
	
	public enum Type
	{
		Character,
		Quad
	}
	
	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Auto Clip Data");
		var serializedData = new SerializedObject(this);
		var goalObject = serializedData.targetObject as STMAutoClipData;
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty type = serializedData.FindProperty("type");
		SerializedProperty character = serializedData.FindProperty("character");
		SerializedProperty quadName = serializedData.FindProperty("quadName");
		SerializedProperty clip = serializedData.FindProperty("clip");
		//SerializedProperty ignoreCase = serializedData.FindProperty("ignoreCase");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(character);
		//EditorGUILayout.PropertyField(caseSensitive);
		EditorGUILayout.PropertyField(clip);
		EditorGUILayout.PropertyField(type);
		if(type.enumValueIndex == 0)
		{
			EditorGUILayout.PropertyField(character);
			GUILayout.Label("Set to:");
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Line Break"))
			{
				goalObject.character = '\n';
				EditorUtility.SetDirty(goalObject);
			}
			if(GUILayout.Button("Tab"))
			{
				goalObject.character = '\t';
				EditorUtility.SetDirty(goalObject);
			}
			if(GUILayout.Button("Clear"))
			{
				goalObject.character = '\0';
				EditorUtility.SetDirty(goalObject);
			}
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.PropertyField(quadName);
		}
		//EditorGUILayout.PropertyField(caseSensitive);
		
		//EditorGUILayout.PropertyField(ignoreCase);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}