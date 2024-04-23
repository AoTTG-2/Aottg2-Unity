//Copyright (c) 2019-2021 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
/*
at the moment, this script will only work right
if it's NOT attached to the parent of the STM object
Attach it to a sibling object!

v Text Box
| > Background Box
| > Super Text Mesh

*/

[RequireComponent(typeof(RectTransform))]
public class STMMatchRect : MonoBehaviour 
{
	private RectTransform tr; //this object's own transform
	public SuperTextMesh stm; //stm used for reference
	private Vector2 size;
	private Vector2 offset;
	public Vector2 padding = Vector2.zero;


	public enum RectToMatch
	{
		ActiveBounds,
		FinalBounds,
		MaxBounds
	}
	public RectToMatch rectToMatch = RectToMatch.ActiveBounds;
	
	//set up events!
	public void OnEnable()
	{
		tr = GetComponent<RectTransform>();
		stm.OnPrintEvent += Match;
		
	}
	public void OnDisable()
	{
		stm.OnPrintEvent -= Match;
	}
	//make this object's rect match STM's current text rect.
	public void Match()
	{
		//box size, plus offset
		if(rectToMatch == RectToMatch.ActiveBounds)
		{
			size.x = stm.bottomRightTextBounds.x - stm.topLeftTextBounds.x;
			size.y = -stm.bottomRightTextBounds.y + stm.topLeftTextBounds.y;
			
		}
		else if(rectToMatch == RectToMatch.FinalBounds)
		{
			size.x = stm.finalBottomRightTextBounds.x - stm.topLeftTextBounds.x;
			size.y = -stm.finalBottomRightTextBounds.y + stm.topLeftTextBounds.y;
		}
		else
		{
			size.x = stm.bottomRightBounds.x - stm.topLeftBounds.x;
			size.y = -stm.bottomRightBounds.y + stm.topLeftBounds.y;
		}
		size.x += padding.x;
		size.y += padding.y;
		offset.x = stm.t.position.x + stm.rawTopLeftBounds.x + stm.rawBottomRightBounds.x * 2f - (padding.x / 2f);
		offset.y = stm.t.position.y - size.y - stm.rawTopLeftBounds.y + (padding.y / 2f);
		tr.sizeDelta = size;
		tr.position = offset;
		tr.pivot = Vector2.zero;
	}
}
