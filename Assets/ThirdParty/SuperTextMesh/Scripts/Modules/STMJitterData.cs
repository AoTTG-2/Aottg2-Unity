//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Jitter Data", menuName = "Super Text Mesh/Jitter Data", order = 1)]
public class STMJitterData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	public float amount;
	public bool perlin = false;
	public float perlinTimeMulti = 20f;
	public AnimationCurve distance = new AnimationCurve(new Keyframe(0f,0f,1f,1f), new Keyframe(1f,1f,1f,1f)); //genereate linear curve //how far the jitter will travel from the origin, on average
	public AnimationCurve distanceOverTime = new AnimationCurve(new Keyframe(0f,1f,0f,0f), new Keyframe(1f,1f,0f,0f));
	[Range(0.0001f, 100f)]
	public float distanceOverTimeMulti = 1f;

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Jitter Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty amount = serializedData.FindProperty("amount");
		SerializedProperty perlin = serializedData.FindProperty("perlin");
		SerializedProperty perlinTimeMulti = serializedData.FindProperty("perlinTimeMulti");
		//SerializedProperty distance = serializedData.FindProperty("distance");
		//SerializedProperty distanceOverTime = serializedData.FindProperty("distanceOverTime");
		SerializedProperty distanceOverTimeMulti = serializedData.FindProperty("distanceOverTimeMulti");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this, stm);
	//the rest:
		EditorGUILayout.PropertyField(amount);
		EditorGUILayout.PropertyField(perlin);
		if(perlin.boolValue){
			EditorGUILayout.PropertyField(perlinTimeMulti);
		}
		//EditorGUILayout.PropertyField(distance);
		//EditorGUILayout.PropertyField(distanceOverTime);
		distance = EditorGUILayout.CurveField("Distance", distance);
		distanceOverTime = EditorGUILayout.CurveField("Distance Over Time", distanceOverTime);
		
		EditorGUILayout.PropertyField(distanceOverTimeMulti);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}