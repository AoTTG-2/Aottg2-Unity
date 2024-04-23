//Copyright (c) 2020 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperTextMesh))]
public class STMPreParseEffect : MonoBehaviour 
{

	public SuperTextMesh superTextMesh;
	public string colorName = "rainbow";

	void Reset()
	{
		superTextMesh = GetComponent<SuperTextMesh>();
	}

	void OnEnable()
	{
		superTextMesh.OnPreParse += AddTag;
	}
	void OnDisable()
	{
		superTextMesh.OnPreParse -= AddTag;
	}
	void AddTag(STMTextContainer container)
	{
		//add tag to start of container text
		container.text = "<c=" + colorName + ">" + container.text;
	}
}
