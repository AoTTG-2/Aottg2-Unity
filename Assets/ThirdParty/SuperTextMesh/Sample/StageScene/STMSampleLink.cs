//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class STMSampleLink : MonoBehaviour {
	public string linkName = "Sample Website";
	public void OnMouseDown(){
		Debug.Log("I was clicked!! Going to: " + linkName);
	}
	//constructor:
	public STMSampleLink(string linkName){
		this.linkName = linkName;
	}
}
