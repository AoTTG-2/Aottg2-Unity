using UnityEngine;
using System.Collections;

// uses raycast to find ground below this gameobject
// Note: if this gameobject is in the samelayer as GroundLayer, raycast might hit this gameobject and not ground

namespace TTT
{
	public class KeepOnSurface : MonoBehaviour 
	{

		public float PivotOffset = 0f;
		public float RayOffset = 100; // how high above we shoot ray down
		public LayerMask GroundLayer;
		RaycastHit hit;

		void Start()
		{
			if(((1<<gameObject.layer) & GroundLayer) != 0) Debug.LogWarning("GameObject is in the same layer as raycasting layer, raycast might hit gameobject instead of ground");
		}

		void Update () 
		{
		
			if (Physics.Raycast(transform.position+Vector3.up*RayOffset, -Vector3.up, out hit, Mathf.Infinity, GroundLayer)) 
			{
				var distanceToGround = hit.distance - PivotOffset - RayOffset;
				transform.Translate(-Vector3.up * distanceToGround, Space.World);
			}

		}
	}
}