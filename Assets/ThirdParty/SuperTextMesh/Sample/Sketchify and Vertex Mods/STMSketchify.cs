//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperTextMesh))]
public class STMSketchify : MonoBehaviour 
{
	public SuperTextMesh stm;
	[Range(0.001f,8f)]
	public float sketchDelay = 0.25f;
	private float sketchLastTime = -1.0f;
	public float sketchAmount = 0.025f;
	private Vector3[] storedOffsets = new Vector3[0];
	public bool unscaledTime = true;

	public void Reset()
	{
		//get stm component
		stm = GetComponent<SuperTextMesh>();
	}
	void Awake()
	{
		sketchLastTime = -1f;
	}
	void OnEnable()
	{
		stm.OnVertexMod += SketchifyVerts;
	}
	void OnDisable()
	{
		stm.OnVertexMod -= SketchifyVerts;
	}
	public void SketchifyVerts(Vector3[] verts, Vector3[] middles, Vector3[] positions)
	{
		//fill array first time
		if(storedOffsets.Length != verts.Length)
		{
			System.Array.Resize(ref storedOffsets, verts.Length); //expand array to fill

			//store verts so they can be applied until the timer rolls over
			for(int i=0, iL=verts.Length; i<iL; i++)
			{
				storedOffsets[i].x = Random.Range(-sketchAmount,sketchAmount);
				storedOffsets[i].y = Random.Range(-sketchAmount,sketchAmount);
				storedOffsets[i].z = Random.Range(-sketchAmount,sketchAmount);
			}
		}
		//round time to nearest multiple of delay
		float newTime = Mathf.Floor((unscaledTime ? Time.unscaledTime : Time.time) / sketchDelay) * sketchDelay;
		if(newTime != sketchLastTime)//update random state if time has changed or it's the first time
		{ 
			sketchLastTime = newTime;
			//update stored verts
			if(storedOffsets.Length != verts.Length)
                System.Array.Resize(ref storedOffsets, verts.Length); //expand array to fill

			for(int i=0, iL=verts.Length; i<iL; i++)
			{
				storedOffsets[i].x = Random.Range(-sketchAmount,sketchAmount);
				storedOffsets[i].y = Random.Range(-sketchAmount,sketchAmount);
				storedOffsets[i].z = Random.Range(-sketchAmount,sketchAmount);
			}
		}
		//use stored verts
		for(int i=0, iL=verts.Length; i<iL; i++)
		{
			verts[i].x += storedOffsets[i].x;
			verts[i].y += storedOffsets[i].y;
			verts[i].z += storedOffsets[i].z;
		}
	}
}
