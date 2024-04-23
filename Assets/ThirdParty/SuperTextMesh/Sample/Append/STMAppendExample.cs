//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public class STMAppendExample : MonoBehaviour {
	public SuperTextMesh text;
	public string appendThis = "Hello!";
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			text.Append(appendThis);
		}
	}
}
