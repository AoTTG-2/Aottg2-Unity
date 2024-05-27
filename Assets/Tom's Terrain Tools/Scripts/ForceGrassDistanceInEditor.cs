using UnityEngine;
using System.Collections;

namespace TTT
{

	[ExecuteInEditMode]
	public class ForceGrassDistanceInEditor : MonoBehaviour {

		public float distance=250; // 250 is max in terrain settings
		Terrain terrain;

		void Start () {
			terrain = GetComponent<Terrain>();
			if (terrain==null)
			{
				Debug.LogError("This gameobject is not terrain, disabling forced details distance", gameObject);
				this.enabled=false;
				return;
			}

		}

		// WARNING: this runs update loop inside editor, not required, if you call the terrain.detailObjectDistance = distance; at start instead
		void Update()
		{
			terrain.detailObjectDistance = distance;
		}

	}
}