//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public class STMVertexMod : MonoBehaviour 
{
	//directly modify vertices
	[Header("Curve")]
	public Vector3 positionOffset;
	public float angleOffset = -1f;
	public Vector3 pivot;
	public float letterRotation = 1f;

	[Header("Circle")]
	public float radius = 2f;
	[Range(0.0001f,1f)]
	public float amountOfCircle = 1f;
	public bool positionsFillCircle = true;


	
	public void AlignToGrid(Vector3[] verts, Vector3[] middles, Vector3[] positions){
		//figure out offset of first position to grid
		int rowStart = 0;
		Vector3 posDifference = RoundDifference(positions[0]);
		for(int i=0, iL=positions.Length; i<iL; i++){
			if(positions[i].y != positions[rowStart].y){
				rowStart = i; //new row
				posDifference = RoundDifference(positions[rowStart]);
			}
			verts[4*i+0] += posDifference; //apply this offset to every vertice
			verts[4*i+1] += posDifference;
			verts[4*i+2] += posDifference;
			verts[4*i+3] += posDifference;
		}
	}
	private Vector3 RoundDifference(Vector3 original){
		Vector3 roundedPos = new Vector3(Mathf.Round(original.x), Mathf.Round(original.y), Mathf.Round(original.z));
		return original - roundedPos;
	}
	public void ApplyCurveToVertices(Vector3[] verts, Vector3[] middles, Vector3[] positions){
		for(int i=0, iL=verts.Length / 4; i<iL; i++){
			//offset position...
			//In this for loop,
			//verts[4*i+0] is the top-left corner
			//verts[4*i+1] is the top-right corner
			//verts[4*i+2] is the bottom-right corner
			//verts[4*i+3] is the bottom-left corner... of a single letter!
			
			verts[4*i+0] -= new Vector3(positionOffset.x,verts[4*i+0].x * positionOffset.y,positionOffset.z);
			verts[4*i+1] -= new Vector3(positionOffset.x,verts[4*i+0].x * positionOffset.y,positionOffset.z);
			verts[4*i+2] -= new Vector3(positionOffset.x,verts[4*i+0].x * positionOffset.y,positionOffset.z);
			verts[4*i+3] -= new Vector3(positionOffset.x,verts[4*i+0].x * positionOffset.y,positionOffset.z);
			
			//rotate letter...
			//Vector3 middleOfLetter = Vector3.Lerp(verts[4*i+0], verts[4*i+2], 0.5f);
			//this isn't neccesarily the middle of the letter, as some points go below
			Vector3 rotationPoint = new Vector3(middles[i].x, positions[i].y, middles[i].z);
			Vector3 angle = new Vector3(0f,0f,angleOffset * middles[i].x);
			if(float.IsNaN(angle.z)){
				angle = Vector3.zero; //awful
			}
			
			verts[4*i+0] = RotatePointAroundPivot(verts[4*i+0], rotationPoint, angle);
			verts[4*i+1] = RotatePointAroundPivot(verts[4*i+1], rotationPoint, angle);
			verts[4*i+2] = RotatePointAroundPivot(verts[4*i+2], rotationPoint, angle);
			verts[4*i+3] = RotatePointAroundPivot(verts[4*i+3], rotationPoint, angle);
			//Vector3 pivot = info[i].Middle;
			angle.z += (positions[i].y * letterRotation);
			verts[4*i+0] = RotatePointAroundPivot(verts[4*i+0], pivot, angle);
			verts[4*i+1] = RotatePointAroundPivot(verts[4*i+1], pivot, angle);
			verts[4*i+2] = RotatePointAroundPivot(verts[4*i+2], pivot, angle);
			verts[4*i+3] = RotatePointAroundPivot(verts[4*i+3], pivot, angle);
		}
	}
	private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 myPivot, Vector3 angles) {
		Vector3 dir = point - myPivot; // get point direction relative to myPivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + myPivot; // calculate rotated point
		return point; // return it
	}

	public void WrapAroundCircle(Vector3[] verts, Vector3[] middles, Vector3[] positions){

		//get widest point.
		float furthestPoint = 0.00001f;
		for(int i=0; i<verts.Length / 4; i++)
		{
			furthestPoint = Mathf.Max(furthestPoint, verts[4*i+2].x);
		}
		
		float circleCircumference = radius * 2f * Mathf.PI;
		float myAmountOfCircle = amountOfCircle;
		if(!positionsFillCircle)
		{
			myAmountOfCircle = furthestPoint / circleCircumference;
			if(myAmountOfCircle > 1f)
			{
				myAmountOfCircle = 1f;
			}
		}
		//for every vert...
		for(int i=0, iL=verts.Length / 4; i<iL; i++)
		{
			
			//how far downt he circle the letter is 
			float amt = positions[i].x / furthestPoint * Mathf.PI * myAmountOfCircle;
			//same but in degrees
			float eulerAmt = positions[i].x / furthestPoint * myAmountOfCircle * -180f - 90f;
			//get local position of verts
			Vector3 localVert0 = Vector3.zero;
			Vector3 localVert1 = verts[4*i+1] - verts[4*i+0];
			Vector3 localVert2 = verts[4*i+2] - verts[4*i+0];
			Vector3 localVert3 = verts[4*i+3] - verts[4*i+0];

			
			verts[4*i+0] = new Vector3(Mathf.Cos(amt) * radius + localVert0.x, -Mathf.Sin(amt) * radius + verts[4*i+0].y, verts[4*i+0].z);
			verts[4*i+1] = new Vector3(Mathf.Cos(amt) * radius + localVert1.x, -Mathf.Sin(amt) * radius + verts[4*i+1].y, verts[4*i+1].z);
			verts[4*i+2] = new Vector3(Mathf.Cos(amt) * radius + localVert2.x, -Mathf.Sin(amt) * radius + verts[4*i+2].y, verts[4*i+2].z);
			verts[4*i+3] = new Vector3(Mathf.Cos(amt) * radius + localVert3.x, -Mathf.Sin(amt) * radius + verts[4*i+3].y, verts[4*i+3].z);
			//recalculate middle point based on new position
			//so... this position is the "base line" of the letter vertically, and the middle horizontally.
			Vector3 rotationPoint = new Vector3(Mathf.Cos(amt) * radius + middles[i].x - positions[i].x, -Mathf.Sin(amt) * radius + positions[i].y, positions[i].z);
			
			//rotate letter...
			Vector3 angle = new Vector3(0f,0f,eulerAmt);
			if(float.IsNaN(angle.z))
			{
				angle = Vector3.zero; //awful
			}
			verts[4*i+0] = RotatePointAroundPivot(verts[4*i+0], rotationPoint, angle);
			verts[4*i+1] = RotatePointAroundPivot(verts[4*i+1], rotationPoint, angle);
			verts[4*i+2] = RotatePointAroundPivot(verts[4*i+2], rotationPoint, angle);
			verts[4*i+3] = RotatePointAroundPivot(verts[4*i+3], rotationPoint, angle);
			
		}
	}

}
