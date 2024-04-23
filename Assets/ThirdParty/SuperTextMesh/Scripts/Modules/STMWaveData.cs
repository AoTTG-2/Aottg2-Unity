//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class STMWaveControl{
	#if UNITY_EDITOR
	public bool showFoldout = false;
	#endif

	public AnimationCurve curveX = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f)); //unfortunately, there isn't a way to completely instantiate a sine/cos wave that loops in just one line of code, so I can't do that, here. no way to loop
	public AnimationCurve curveY = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f));
	public AnimationCurve curveZ = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f));

	public AnimationCurve multiOverTime = new AnimationCurve(new Keyframe(0f,1f,0f,0f), new Keyframe(1f,1f,0f,0f));
	[UnityEngine.Serialization.FormerlySerializedAs("offset")]
	[Range(0f,1f)]
	[Tooltip("Timing offset compared to other waves.")]
	public float phase = 0f; //to be multiplied by 6. timing offset
	[Tooltip("How fast the wave will move over time.")]
	public Vector3 speed = Vector3.zero; //how wide the curve is... so how fast it'll animate
	[Tooltip("Multiplier on the current wave value.")]
	public Vector3 strength = Vector3.zero; //how far the curve will move the letters
	[Tooltip("Timing difference between letters.")]
	public Vector3 density = Vector3.zero; //how wide the curve is on letters?
	//public Vector3 pivot = Vector3.zero; //origin point on this letter
	#if UNITY_EDITOR
	public void DrawInspector(string title)
	{
		if(showFoldout = EditorGUILayout.Foldout(showFoldout, title))
		{
			//EditorGUILayout.PropertyField(position,true);
			//draw manually to get around wave bug

			EditorGUI.indentLevel++;

			this.curveX = EditorGUILayout.CurveField("Curve X", this.curveX);
			this.curveY = EditorGUILayout.CurveField("Curve Y", this.curveY);
			this.curveZ = EditorGUILayout.CurveField("Curve Z", this.curveZ);

			this.phase = EditorGUILayout.Slider("Phase", this.phase, 0f, 1f);
			this.speed = EditorGUILayout.Vector3Field("Speed", this.speed);
			this.strength = EditorGUILayout.Vector3Field("Strength", this.strength);
			this.density = EditorGUILayout.Vector3Field("Density", this.density);

			this.multiOverTime = EditorGUILayout.CurveField("Multi Over Time", this.multiOverTime);

			EditorGUI.indentLevel--;
		}
	}
	#endif
}
[System.Serializable]
public class STMWaveRotationControl{
	#if UNITY_EDITOR
	public bool showFoldout = false;
	#endif

	public AnimationCurve curveZ = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f));
	[Range(0f,1f)]
	[Tooltip("Timing offset compared to other waves.")]
	public float phase = 0f; //to be multiplied by 6. timing offset
	[Tooltip("How fast the wave will move over time.")]
	public float speed = 0f; //how wide the curve is... so how fast it'll animate
	[Tooltip("Multiplier on the current wave value.")]
	public float strength = 0f; //how far the curve will move the letters
	[Tooltip("Timing difference between letters.")]
	public float density = 0f; //how wide the curve is on letters?
	[Tooltip("Origin position of this animation.")]
	public Vector2 pivot = Vector2.zero; //origin point on this letter

	#if UNITY_EDITOR
	public void DrawInspector(string title)
	{
		if(showFoldout = EditorGUILayout.Foldout(showFoldout, title))
		{
			EditorGUI.indentLevel++;

			this.curveZ = EditorGUILayout.CurveField("Curve Z", this.curveZ);

			this.phase = EditorGUILayout.Slider("Phase", this.phase, 0f, 1f);
			this.speed = EditorGUILayout.FloatField("Speed", this.speed);
			this.strength = EditorGUILayout.FloatField("Strength", this.strength);
			this.density = EditorGUILayout.FloatField("Density", this.density);
			this.pivot = EditorGUILayout.Vector2Field("Pivot", this.pivot);

			EditorGUI.indentLevel--;
		}
	}
	#endif
}
[System.Serializable]
public class STMWaveScaleControl{
	#if UNITY_EDITOR
	public bool showFoldout = false;
	#endif

	public AnimationCurve curveX = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f));
	public AnimationCurve curveY = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f,0f));
	[Range(0f,1f)]
	[Tooltip("Timing offset compared to other waves.")]
	public float phase = 0f; //to be multiplied by 6. timing offset
	[Tooltip("How fast the wave will move over time.")]
	public Vector2 speed = Vector2.zero; //how wide the curve is... so how fast it'll animate
	[Tooltip("Multiplier on the current wave value.")]
	public Vector2 strength = Vector2.zero; //how far the curve will move the letters
	[Tooltip("Timing difference between letters.")]
	public Vector2 density = Vector2.zero; //how wide the curve is on letters?
	[Tooltip("Origin position of this animation.")]
	public Vector2 pivot = Vector2.zero; //origin point on this letter

	#if UNITY_EDITOR
	public void DrawInspector(string title)
	{
		if(showFoldout = EditorGUILayout.Foldout(showFoldout, title))
		{
			//EditorGUILayout.PropertyField(position,true);
			//draw manually to get around wave bug

			EditorGUI.indentLevel++;

			this.curveX = EditorGUILayout.CurveField("Curve X", this.curveX);
			this.curveY = EditorGUILayout.CurveField("Curve Y", this.curveY);

			this.phase = EditorGUILayout.Slider("Phase", this.phase, 0f, 1f);
			this.speed = EditorGUILayout.Vector2Field("Speed", this.speed);
			this.strength = EditorGUILayout.Vector2Field("Strength", this.strength);
			this.density = EditorGUILayout.Vector2Field("Density", this.density);
			this.pivot = EditorGUILayout.Vector2Field("Pivot", this.pivot);

			EditorGUI.indentLevel--;
		}
	}
	#endif
}
[CreateAssetMenu(fileName = "New Wave Data", menuName = "Super Text Mesh/Wave Data", order = 1)]
public class STMWaveData : ScriptableObject{
	#if UNITY_EDITOR
	public bool showFoldout = false;
	#endif
	//public string name;
	public bool animateFromTimeDrawn = false;


	public bool positionControl = true;
	[UnityEngine.Serialization.FormerlySerializedAs("main")]
	public STMWaveControl position;
	[Tooltip("Use these below values?")] //this should be getting hidden but it's not...
	public bool individualVertexControl = false;
	//public bool showIndividualFoldouts = false;
	public STMWaveControl topLeft;
	public STMWaveControl topRight;
	public STMWaveControl bottomLeft;
	public STMWaveControl bottomRight;

	public bool rotationControl = false;
	public STMWaveRotationControl rotation;
	public bool scaleControl = false;
	public STMWaveScaleControl scale;
	

	#if UNITY_EDITOR
	public void DrawCustomInspector(SuperTextMesh stm){
		Undo.RecordObject(this, "Edited STM Wave Data");
		var serializedData = new SerializedObject(this);
		serializedData.Update();
	//gather parts for this data:
		SerializedProperty animateFromTimeDrawn = serializedData.FindProperty("animateFromTimeDrawn");
		//SerializedProperty position = serializedData.FindProperty("position");
		//SerializedProperty rotation = serializedData.FindProperty("rotation");
		//SerializedProperty scale = serializedData.FindProperty("scale");
		SerializedProperty individualVertexControl = serializedData.FindProperty("individualVertexControl");
		SerializedProperty positionControl = serializedData.FindProperty("positionControl");
		SerializedProperty rotationControl = serializedData.FindProperty("rotationControl");
		SerializedProperty scaleControl = serializedData.FindProperty("scaleControl");
		//SerializedProperty topLeft = serializedData.FindProperty("topLeft");
		//SerializedProperty topRight = serializedData.FindProperty("topRight");
		//SerializedProperty bottomLeft = serializedData.FindProperty("bottomLeft");
		//SerializedProperty bottomRight = serializedData.FindProperty("bottomRight");
	//Title bar:
		STMCustomInspectorTools.DrawTitleBar(this,stm);
	//the rest:
		EditorGUILayout.PropertyField(animateFromTimeDrawn);
		EditorGUILayout.PropertyField(positionControl);
		if(positionControl.boolValue){
			position.DrawInspector("Position");
		}
		EditorGUILayout.PropertyField(rotationControl);
		if(rotationControl.boolValue){
			rotation.DrawInspector("Rotation");
		}
		EditorGUILayout.PropertyField(scaleControl);
		if(scaleControl.boolValue){
			scale.DrawInspector("Scale");
		}
		EditorGUILayout.PropertyField(individualVertexControl);
		if(individualVertexControl.boolValue){
			//EditorGUILayout.PropertyField(topLeft,true);
			//EditorGUILayout.PropertyField(topRight,true);
			//EditorGUILayout.PropertyField(bottomLeft,true);
			//EditorGUILayout.PropertyField(bottomRight,true);
			topLeft.DrawInspector("Top Left");
			topRight.DrawInspector("Top Right");
			bottomLeft.DrawInspector("Bottom Left");
			bottomRight.DrawInspector("Bottom Right");
		}
		EditorGUILayout.Space(); //////////////////SPACE
		if(this != null)serializedData.ApplyModifiedProperties(); //since break; cant be called
	}
	#endif
}