//Copyright (c) 2017-2018 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
Make SURE!!!!!! that this is added as a DYNAMIC event!!!
When adding events, it'll give you two lists to choose from, with a divider in the middle.
Dynamic events are ABOVE static events in the menu!
*/
public class STMEventInterpreter : MonoBehaviour {
	private SuperTextMesh _stm;
	public SuperTextMesh stm
	{
		get
		{
			if(_stm == null) _stm = transform.GetComponent<SuperTextMesh>();
			return _stm;
		}
	}
	public GameObject confetti;
	public AudioSource au;
	public AudioClip myClip;
	public STMSampleLink link;
	private List<STMSampleLink> allLinks = new List<STMSampleLink>(); //so links can be deleted on rebuild
	private List<SpriteRenderer> allBGs = new List<SpriteRenderer>();
	public SpriteRenderer bgPrefab;
	//public string seperator = "=";
	//public string audioTag = "a";
	public void SayMessage()
	{
		Debug.Log("Saying message!");
	}

	public void DoEvent(string s, STMTextInfo info){ //the string from the event, index of the letter in the string, world position of this letter, position of bottom-left corner
		//To get specific parts of a letter, if this script is on the same gameObject as the text mesh. (for position)
		//Vector3 myPos = info.pos + transform.position;
		//Vector3 centerPos = info.Middle + transform.position;
		//Vector2 letterSize = new Vector2(info.TopRightVert.x - info.pos.x, info.TopRightVert.y - info.pos.y);
		Vector3 pos = info.Middle + transform.position;
		Vector3 rawPos = info.BottomLeftVert + transform.position;
		/*
		string myTag = audioTag + seperator;
		if(myTag.Length <= s.Length && s.Substring(0,myTag.Length) == myTag){ //first two characters are "a="?
			string playString = "mySound";
			if(myTag.Length + playString.Length <= s.Length && s.Substring(audioTag.Length + seperator.Length, playString.Length) == playString){
				Debug.Log("Playing sound!");
			}else{
				Debug.Log("Unknown audio event!");
			}
		*/
		if(s.Contains("printpos")){
			//Debug.Log(rawPos); //print the position of this letter.
			Debug.Log(info.rawIndex.ToString() + " " + info.readTime.ToString() + " " + s); //print raw integer of letter
			Debug.DrawLine(rawPos, rawPos+Vector3.down, Color.red, 10.0f, false); //draw a line from the corner of this letter, down
		}
		else if(s == "transcribe")
		{
		//	info.
		}
		else if(s == "link"){
			Vector3 cornerPos = info.pos + transform.position + new Vector3((info.TopRightVert.x - info.pos.x) / 2f, info.size.y / 2f, 0f); //align to row
			STMSampleLink newLink = Instantiate(link, cornerPos, link.transform.rotation) as STMSampleLink; //create line position
			newLink.linkName = "Custom Link Address!"; //change the string
			newLink.transform.localScale = new Vector3(info.size.x, info.size.y, 0.5f); //just make it square
			allLinks.Add(newLink); //remember, so it can be destroyed
		}
		else if(s.Length >= 2 && s.Substring(0,2) == "bg"){
			Vector3 cornerPos = info.pos + transform.position + new Vector3((info.TopRightVert.x - info.pos.x) / 2f, info.size.y / 2f, 0.2f); //align to row
			SpriteRenderer newBG = Instantiate(bgPrefab, cornerPos, bgPrefab.transform.rotation) as SpriteRenderer; //create line position
			newBG.color = Color.red; //change the color
			newBG.transform.localScale = new Vector3(info.size.x, info.size.y, 0.5f); //just make it square
			allBGs.Add(newBG); //remember, so it can be destroyed
		}
		else if(s == "confetti"){
			Instantiate(confetti,pos,confetti.transform.rotation); //spawn a prefab at the letter's location
		}
		else if(s == "playSound"){
			Debug.Log("Playing sound!");
			au.PlayOneShot(myClip,1f); //alt way of playing clips, for example
		}
		else{
			Debug.Log("Unknown event: '" + s + "'");
		}
	}
	public void ClearLinks(){ //destroy all link objects that were generated
		for(int i=0; i<allLinks.Count; i++){
			Destroy(allLinks[i].gameObject);
		}
		allLinks.Clear();
	}
	public void ClearBGs(){ //destroy all link objects that were generated
		for(int i=0; i<allBGs.Count; i++){
			Destroy(allBGs[i].gameObject);
		}
		allBGs.Clear();
	}
	
}
