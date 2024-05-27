using UnityEditor;
using UnityEngine;
using System.Collections;

namespace TTT
{

	public class ClearWaterLevel : ScriptableWizard 
	{

		public Terrain terrain;
		public Transform waterPlane;

		[MenuItem ("Window/Terrain Tools/Clear below water Level",false,109)]
		static void CreateWizard () 
		{
			ScriptableWizard.DisplayWizard<ClearWaterLevel>("Clear now", "Apply");
		}
		
		void OnWizardCreate () 
		{
			if(terrain==null || waterPlane==null) return;

			Undo.RecordObject(terrain.terrainData, "Clear below water level");

			// remove trees
			ArrayList NewTrees = new ArrayList(terrain.terrainData.treeInstances);
			
			for (int i = NewTrees.Count-1; i >= 0; i--) 
			{
				TreeInstance MyTree = (TreeInstance)NewTrees[i];
				if (MyTree.position.y<waterPlane.position.y/terrain.terrainData.size.y)
				{
					NewTrees.RemoveAt(i);
				}
			}
			terrain.terrainData.treeInstances = (TreeInstance[])NewTrees.ToArray(typeof(TreeInstance));

			// remove grass
			int detailRes = terrain.terrainData.detailWidth;
			int[] detailLayers = terrain.terrainData.GetSupportedLayers(0, 0, detailRes, detailRes);
			int LayerCount = detailLayers.Length;
			int detailWidth = terrain.terrainData.detailWidth;
			int detailHeight = terrain.terrainData.detailHeight;

			for (int l = 0; l<LayerCount; l++) 
			{
				int[,] grass = terrain.terrainData.GetDetailLayer(0, 0, detailRes, detailRes, l);

				for (int y = 0; y < detailRes; y++) 
				{
					for (int x = 0; x < detailRes; x++) 
					{
						float localHeight = terrain.terrainData.GetInterpolatedHeight((float)y/(float)detailHeight,(float)x/(float)detailWidth);
						float currentPosHeight = (localHeight-waterPlane.position.y)/(terrain.terrainData.size.y-waterPlane.position.y);

						// below waterplane Y
						if (currentPosHeight<0)
						{
							grass[x,y]=0;
						}else{ // 
							// NOTE: keeps original, ONLY for first run, because if its set to 0, cannot restore it..
							grass[x,y]=2; // for test only
						}
					}
				}
				terrain.terrainData.SetDetailLayer(0, 0, l, grass);
			}

			return;
			
			// TODO: set texture for underwater?
			// TODO: set texture for waterlevel (beach?)
		}


		void OnWizardUpdate () 
		{

			if(terrain==null || waterPlane==null)
			{
				errorString = "No terrain or waterplane assigned";
			}else{
				errorString = "";
			}
		}
		

//		void OnWizardOtherButton () 
//		{
//		}
	}

}

