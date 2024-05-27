using UnityEngine;
using UnityEditor;

namespace TTT
{

	public class PlaceOnTerrain : ScriptableObject 
	{

		public Texture2D lmap;
		
		[MenuItem ("Window/Terrain Tools/Drop Selection to Terrain",false,103)]
		static void PlaceSelectionOnTerrain() 
		{
			foreach (Transform t in Selection.transforms) 
			{
				if (t.GetComponent<Terrain>()==null)
				{
					Undo.RecordObject(t, "Move " + t.name);
					RaycastHit hit;
					if (Physics.Raycast(t.position, -Vector3.up, out hit)) {
						float distanceToGround = hit.distance;
						t.Translate(-Vector3.up * distanceToGround);
					} else if (Physics.Raycast(t.position, Vector3.up, out hit)) {
						float distanceToGround = hit.distance;
						t.Translate(Vector3.up * distanceToGround);
					}
				}
			}
		}
	 
		[MenuItem ("Window/Terrain Tools/Drop Selection to Terrain", true, 3)]
		static bool ValidatePlaceSelectionOnTerrain () {
			return Selection.activeTransform != null;
		}
	  	
	}
}