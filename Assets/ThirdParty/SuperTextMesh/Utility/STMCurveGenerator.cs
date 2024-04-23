using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
Give a bunch of useful waves, to be copied to presets!
*/
[ExecuteInEditMode]
public class STMCurveGenerator : MonoBehaviour 
{
	public bool redraw = false;
	[Tooltip("A sine wave.")]
	public AnimationCurve sine;
	[Tooltip("A cos wave.")]
	public AnimationCurve cos;

	public AnimationCurve linear;
	public AnimationCurve inverseLinear;
	
	#if UNITY_EDITOR
	void Start () {
		DrawCurves();
	}
	void OnValidate(){
		if(redraw){
			redraw = false;
			DrawCurves();
		}
	}
	void DrawCurves(){
		cos = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
		cos.preWrapMode = WrapMode.Loop;
        cos.postWrapMode = WrapMode.Loop; //generate a cos curve

        sine = new AnimationCurve(new Keyframe(0f, 0.5f, Mathf.PI, Mathf.PI), 
        						new Keyframe(0.25f, 1f),
        						new Keyframe(0.75f, 0f),
        						new Keyframe(1f, 0.5f, Mathf.PI, Mathf.PI));
		sine.preWrapMode = WrapMode.Loop;
        sine.postWrapMode = WrapMode.Loop; //generate a sine curve

        linear = new AnimationCurve(new Keyframe(0f,0f,1f,1f), new Keyframe(1f,1f,1f,1f)); //genereate linear curve

        inverseLinear = new AnimationCurve(new Keyframe(0f,1f,-1f,-1f), new Keyframe(1f,0f,-1f,-1f)); //generate inverse linear curve
	}
	#endif
}
