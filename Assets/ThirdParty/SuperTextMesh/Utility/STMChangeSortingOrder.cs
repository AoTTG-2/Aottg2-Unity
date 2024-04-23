//Copyright (c) 2017 Kai Clavier [kaiclavier.com] Do Not Distribute

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(STMChangeSortingOrder))]
[CanEditMultipleObjects] //sure why not
public class STMChangeSortingOrderEditor : Editor{
	override public void OnInspectorGUI(){
		serializedObject.Update(); //for onvalidate stuff!
		var c = target as STMChangeSortingOrder;

		SerializedProperty sortingOrder = serializedObject.FindProperty("sortingOrder");
		SerializedProperty sortingLayer = serializedObject.FindProperty("sortingLayer");

		EditorGUILayout.PropertyField(sortingOrder);
		EditorGUILayout.PropertyField(sortingLayer);

		serializedObject.ApplyModifiedProperties();

		c.Refresh();
	}
}
#endif

public class STMChangeSortingOrder : MonoBehaviour {
	public int sortingOrder = 0;
	public string sortingLayer = "Default";

	void OnEnable(){
		Refresh();
	}
	void OnValidate(){
		Refresh();
	}
	public void Refresh(){
		Renderer r = GetComponent<Renderer>();
		if(r != null){
			r.sortingOrder = sortingOrder;
			r.sortingLayerName = sortingLayer;
		}
	}
}
