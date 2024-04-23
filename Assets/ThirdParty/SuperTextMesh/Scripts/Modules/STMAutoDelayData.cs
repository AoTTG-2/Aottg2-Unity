//Copyright (c) 2023 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New AutoDelay Data", menuName = "Super Text Mesh/AutoDelay Data", order = 0)]
public class STMAutoDelayData : STMDelayData
{

	public Type type = Type.Character;
	//public string name;
	//eventually make this able to detect a set of characters?
	public char character;
	public string quadName; //for matching to a quad
	//public bool caseSensitive = false;
	
	public enum Ruleset
	{
		Always, //commas
		FollowedBySpace, //fast ellipsis or ?!?!?!?!
		FollowedBySameCharacterOrSpace, //slow ellipsis
		FollowedByDifferentCharacter //fast ellipsis without spaces
		
	}

	public enum Type
	{
		Character,
		Quad
	}

	public Ruleset ruleset;
	
	#if UNITY_EDITOR
	public override void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Delay Data");
		var serializedData = new SerializedObject(this);
		var goalObject = serializedData.targetObject as STMAutoClipData;
		serializedData.Update();
		//gather parts for this data:
		SerializedProperty type = serializedData.FindProperty("type");
		SerializedProperty character = serializedData.FindProperty("character");
		SerializedProperty quadName = serializedData.FindProperty("quadName");

		SerializedProperty count = serializedData.FindProperty("count");
		SerializedProperty ruleset = serializedData.FindProperty("ruleset");
		//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
		//the rest:
		
		EditorGUILayout.PropertyField(count);
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
			
			EditorGUILayout.PropertyField(ruleset);
		}
		else
		{
			EditorGUILayout.PropertyField(quadName);
		}
		//EditorGUILayout.PropertyField(caseSensitive);
		
		
		
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}
