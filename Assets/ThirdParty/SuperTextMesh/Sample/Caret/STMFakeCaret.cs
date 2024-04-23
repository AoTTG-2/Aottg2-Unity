using UnityEngine;
using System.Collections;

public class STMFakeCaret : MonoBehaviour {

	public SuperTextMesh stm; //the mesh to follow
	public Vector3 offset;

	void OnEnable()
	{
		stm.OnPrintEvent += MoveCaret;

	}

	void OnDisable()
	{
		stm.OnPrintEvent -= MoveCaret;
	}
	void MoveCaret()
	{
		if(stm != null && //if there's a defined text mesh...
			stm.info.Count > 0 && //and it has textinfo to follow...
			stm.latestNumber > -1 && //and it's either drawing or done drawing...
			stm.hyphenedText[stm.latestNumber] != '\n'){ //ignore line breaks
			STMTextInfo myInfo = stm.info[stm.latestNumber]; //all info for this one character...
			//put the caret in the right place!
			transform.localPosition = myInfo.pos + myInfo.Advance(stm.characterSpacing, stm.quality) + offset;
		}
	}
}
