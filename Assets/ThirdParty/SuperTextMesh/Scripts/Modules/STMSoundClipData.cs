//Copyright (c) 2016-2023 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Sound Clip Data", menuName = "Super Text Mesh/Sound Clip Data", order = 1)]
public class STMSoundClipData : ScriptableObject{ //for auto-clips. replacing text sounds a mesh as a whole is using
	#if UNITY_EDITOR
	public bool showFoldout = true;
	#endif
	//[TextArea(2,3)]
	//public string character;
	[System.Serializable]
	
	public class AutoClip{ //the same as an autoclip
		
		public enum Type
		{
			Character,
			Quad,
			LineBreak,
			Tab
		}

		public Type type;
		[SerializeField] private char _character;

		public char character
		{
			get
			{
				if(type == Type.LineBreak)
				{
					return '\n';
				}
				if(type == Type.Tab)
				{
					return '\t';
				}
				return _character;
			}
		}
		public string quadName; //for matching to a quad
		//public bool ignoreCase;
		public AudioClip clip;

	}
	public List<AutoClip> clips = new List<AutoClip>();

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Sound Clip Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty clips = serializedData.FindProperty("clips");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(clips, true);
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}