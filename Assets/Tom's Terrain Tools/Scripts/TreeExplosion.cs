
using UnityEngine;
using System.Collections;

public class TreeExplosion : MonoBehaviour {

	public float BlastRange = 30.0f;
	public float BlastForce = 30000.0f;
	public GameObject DeadReplace;
	public GameObject Explosion;
	
	void Explode() {
		Instantiate(Explosion, transform.position, Quaternion.identity);
		TerrainData terrain = Terrain.activeTerrain.terrainData;
		
		ArrayList instances = new ArrayList();
		
		foreach (TreeInstance tree in terrain.treeInstances) {
			float distance = Vector3.Distance(Vector3.Scale(tree.position, terrain.size) + Terrain.activeTerrain.transform.position, transform.position);
			if (distance<BlastRange) {
				// the tree is in range - destroy it
				GameObject dead = Instantiate(DeadReplace, Vector3.Scale(tree.position, terrain.size) + Terrain.activeTerrain.transform.position, Quaternion.identity) as GameObject;
				dead.GetComponent<Rigidbody>().maxAngularVelocity = 1;
				dead.GetComponent<Rigidbody>().AddExplosionForce(BlastForce, transform.position, 20+BlastRange*5, -20.0f);
			} else {
				// tree is out of range - keep it
				instances.Add(tree);
				
			}
		}
		terrain.treeInstances = (TreeInstance[])instances.ToArray(typeof(TreeInstance));

	}

	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			Explode();
		}
	}
	
}

