using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

[RequireComponent(typeof(SuperTextMesh))]
[ExecuteInEditMode]
public class STMVerticalWriting : MonoBehaviour
{
	[SerializeField] private SuperTextMesh textMesh;
	[Range(0f, 90f)]
	public float angle = 90f;

	private Vector3 _eulerRotation = Vector3.zero;
	private Vector3 eulerRotation
	{
		get
		{
			_eulerRotation.z = angle;
			return _eulerRotation;
		}
	}

	public bool useEvenSpacing = true;
	public float evenSpacing = 0.1f;
	public char[] keepRotated = { 'ー' }; //Chōonpu
	private void Reset()
	{
		textMesh = GetComponent<SuperTextMesh>();
	}

	private void OnEnable()
	{
		textMesh.OnVertexMod += RotateLetters;
		textMesh.Rebuild();
	}

	private void OnDisable()
	{
		textMesh.OnVertexMod -= RotateLetters;
	}

	public void RotateLetters(Vector3[] verts, Vector3[] middles, Vector3[] positions)
	{
		char thisChar;
		var lastLetterPos = 0f;
		var nextLetterPos = 0f;
		float difference;
		
		for(int i=0, iL=middles.Length; i<iL; i++)
		{
			//reset last letter pos if position is before difference?
			if(verts[4 * i + 0].x <= lastLetterPos)
			{
				nextLetterPos = 0f;
			}
			lastLetterPos = verts[4 * i + 0].x;
			
			thisChar = textMesh.hyphenedText[i];
			//make it so the top vert here is X distance below the last
			//var height = verts[4 * i + 3].y - verts[4*i+0].y + spacing;
			if(!keepRotated.Contains(thisChar)) //DONT rotate these characters???
			{
				verts[4*i+0] = RotateVertAroundMiddle(verts[4*i+0], middles[i], eulerRotation);
				verts[4*i+1] = RotateVertAroundMiddle(verts[4*i+1], middles[i], eulerRotation);
				verts[4*i+2] = RotateVertAroundMiddle(verts[4*i+2], middles[i], eulerRotation);
				verts[4*i+3] = RotateVertAroundMiddle(verts[4*i+3], middles[i], eulerRotation);
			}
			
			
			
			if(useEvenSpacing)
			{
				//start of this letter vs position of last letter with spacing
				difference =  nextLetterPos - verts[4 * i + 0].x;
				
				//then, move down
				verts[4*i+0].x += difference;
				verts[4*i+1].x += difference;
				verts[4*i+2].x += difference;
				verts[4*i+3].x += difference;
				
				//end of this letter with spacing
				nextLetterPos = verts[4 * i + 2].x + evenSpacing;
			}
			
		}
		
		//trying a thing.
		for(int i = 0; i < verts.Length; i++)
		{
			verts[i] = RotateVertAroundMiddle(verts[i], Vector3.zero, -eulerRotation);
		}
	}
	public Vector3 RotateVertAroundMiddle(Vector3 vert, Vector3 middle, Vector3 euler) 
	{
		return Quaternion.Euler(euler) * (vert - middle) + middle;
	}
}
