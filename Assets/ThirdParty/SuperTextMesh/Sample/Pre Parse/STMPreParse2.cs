using UnityEngine;
using System.Collections;

public class STMPreParse2 : MonoBehaviour {
	public string addToEnd = "";
	public void Parse(STMTextContainer x){

		x.text = x.text + addToEnd;
	}
}
