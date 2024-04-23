//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Draw Animation", menuName = "Super Text Mesh/Draw Animation Data", order = 1)]
public class STMDrawAnimData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	[Tooltip("How long the Draw Animation will last.")]
	public float animTime = 0f; //time it will take to animate
	public AnimationCurve animCurve = new AnimationCurve(new Keyframe(0f,0f,1f,1f), new Keyframe(1f,1f,1f,1f)); //genereate linear curve
	public Vector3 startScale = Vector3.one; //how big the letter will start at, relative to letter size
	public Vector3 startOffset = Vector3.zero;
	[Tooltip("How long the fade animation will last.")]
	public float fadeTime = 0f; //time it will take to fade in
	public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0f,0f,1f,1f), new Keyframe(1f,1f,1f,1f)); //genereate linear curve
	[Tooltip("Starting color for read out text.")]
	public Color32 startColor = Color.clear; //for fill/fade. 

	//add curves for this stuff!

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Draw Animation Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty animTime = serializedData.FindProperty("animTime");
		//SerializedProperty animCurve = serializedData.FindProperty("animCurve");
		SerializedProperty startScale = serializedData.FindProperty("startScale");
		SerializedProperty startOffset = serializedData.FindProperty("startOffset");
		SerializedProperty fadeTime = serializedData.FindProperty("fadeTime");
		//SerializedProperty fadeCurve = serializedData.FindProperty("fadeCurve");
		SerializedProperty startColor = serializedData.FindProperty("startColor");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(animTime);
		if(animTime.floatValue > 0f){
			//EditorGUILayout.PropertyField(animCurve);
			animCurve = EditorGUILayout.CurveField("Anim Curve", animCurve);
			EditorGUILayout.PropertyField(startScale);
			EditorGUILayout.PropertyField(startOffset);
		}
		EditorGUILayout.PropertyField(fadeTime);
		if(fadeTime.floatValue > 0f){
			//EditorGUILayout.PropertyField(fadeCurve);
			fadeCurve = EditorGUILayout.CurveField("Fade Curve", fadeCurve);
		}
		EditorGUILayout.PropertyField(startColor);
		
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}