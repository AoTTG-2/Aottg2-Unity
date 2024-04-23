//Copyright (c) 2016-2020 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SuperTextMesh))]
public class STMPagination : MonoBehaviour {
	public SuperTextMesh originalText;
	public SuperTextMesh overflowText;
	
	public void Awake(){
		//clear on runtime
		overflowText.text = "";
	}
	public void OverflowLeftovers(){
		overflowText.text = originalText.leftoverText.TrimStart();
		//if there's no text, Rebuild() doesn't get called anyway
		//use TrimStart() to remove any spaces that might have carried over.
	}
	//is this implementation better? causes errors when the object isn't defined...
	//sort of a non-issue tho, an error *should* be returned if the next text field isn't defined!
	public void OverflowLeftovers(SuperTextMesh stm)
	{
		overflowText.text = stm.leftoverText.TrimStart();
	}

	public void Reset()
	{
		originalText = GetComponent<SuperTextMesh>();
	}
	//update, just use delegate events to subscibe automatically!
	public void OnEnable()
	{
		originalText.OnCompleteEvent += OverflowLeftovers;
	}
	public void OnDisable()
	{
		originalText.OnCompleteEvent -= OverflowLeftovers;
	}
}
