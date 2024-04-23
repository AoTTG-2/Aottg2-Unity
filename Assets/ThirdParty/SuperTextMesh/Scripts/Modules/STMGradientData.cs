//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Gradient Data", menuName = "Super Text Mesh/Gradient Data", order = 1)]
public class STMGradientData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//public string name;
	public Gradient gradient;
	public float gradientSpread = 0.1f;
	public float scrollSpeed = 0.0f; //can be negative, or 0
	public GradientDirection direction = GradientDirection.Horizontal; //TODO vertical gradients are kinda dumb, maybe remove them? or make em grab from diff, more consistent positions...
	public enum GradientDirection{
		Horizontal,
		Vertical
	}
	public bool smoothGradient = true;

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Gradient Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty gradient = serializedData.FindProperty("gradient");
		SerializedProperty gradientSpread = serializedData.FindProperty("gradientSpread");
		SerializedProperty scrollSpeed = serializedData.FindProperty("scrollSpeed");
		SerializedProperty direction = serializedData.FindProperty("direction");
		SerializedProperty smoothGradient = serializedData.FindProperty("smoothGradient");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(gradient);
		EditorGUILayout.PropertyField(gradientSpread);
		EditorGUILayout.PropertyField(scrollSpeed);
		EditorGUILayout.PropertyField(direction);
		EditorGUILayout.PropertyField(smoothGradient);
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}