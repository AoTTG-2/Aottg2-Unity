//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/*
To Do: callback for mouse over and mouse exit, so cursor or link can change appearance?
So... these events would be given to all links attached.

*/

namespace STMTools.Links
{
	public struct CharInfo
	{
		public Bounds bounds;
		public int line;
		public int linkIndex;
		public int charIndex;

		public CharInfo(float xMin, float yMin, float xMax, float yMax, int line, int linkIndex, int charIndex) : this()
		{
			this.bounds = new Bounds();
			this.bounds.SetMinMax(new Vector3(xMin, yMin, 0f), new Vector3(xMax, yMax, 0f));
			this.line = line;
			this.linkIndex = linkIndex;
			this.charIndex = charIndex;
		}
	}
	[System.Serializable]
	public class Link
	{
		public string label = string.Empty; //label used for this <link=TAG>

		public UnityEvent onClick; //unity event, if any, to be called when this link's generated colliders are clicked.
		//delegate event possible?
	}

	[ExecuteInEditMode]
	public class LinkController : MonoBehaviour 
	{
		public SuperTextMesh superTextMesh;
		[Tooltip("If this value is set to 'link', this means that the full tag will be '<link=myLinkLabel>")]
		public string linkString = "link";
		[Tooltip("Additional style to be applied automatically to link text. If set to anything besides an empty string, all tags will be cleared after a link.")]
		public string tagStyle = "<c=blue>"; //if anything besides string.empty, all tags will be cleared after a link!!
		//[Tooltip("Style applied to tags automatically when hovered. Leave blank for no change.")]
		//public string hoverTagStyle = "<c=blue><w>";
		[Tooltip("Additional space added to collider size.")]
		public Vector2 padding = new Vector2(0.1f, 0f); //compared to line's size, how much bigger the collider will be.

		//links that can be handled by this component!
		[Tooltip("These labels will be matched, and objects with the specified UnityEvent will be created.")]
		public List<Link> links = new List<Link>();

		private List<LinkObject> linkObjects = new List<LinkObject>();
		private LinkObject currentLinkObject; //if not null, this will be ADDED to.

		//for all links.
		public UnityEvent onEnter;
		public UnityEvent onExit;
		/*
		//was going to use this for a system where each link would use just one
		//object, sharing colliders with others. but this won't work for UI text.
		public LinkObject GetExistingLinkObject(int linkIndex)
		{
			for(int i=0; i<linkObjects.Count; i++)
			{
				if(linkObjects[i].linkIndex == linkIndex)
				{
					return linkObjects[i];
				}
			}
			return null;
		}
		*/

		private int hoverThis = -1;
		public void PreparseTags(STMTextContainer x)
		{
			string startTag = "<(?<label>" + linkString + ")=(?<tag>.+?)>"; //<link=abc>
			string startReplace = "<e2=${label},${tag}>"; //<e2=link,abc>

			string endTag = "</(?<label>" + linkString + ")>"; //</link>
			string endReplace = "</e2>";

			//if(hoverThis > -1 && hoverTagStyle.Length > 0)
			//{
			//	startReplace += hoverTagStyle;
			//	endReplace += "<clear>";
			//}
			if(tagStyle.Length > 0)
			{
				startReplace += tagStyle;
				endReplace += "<clear>"; //clear ALL tags. I warned ya!
			}

			x.text = Regex.Replace(x.text, startTag, startReplace, RegexOptions.Multiline);
			x.text = Regex.Replace(x.text, endTag, endReplace, RegexOptions.Multiline);

		}

		//not sure if this will be possible...
		//need to rewrite system to allow individual links to be grabbed during rewrite.
		internal void EnterLink(int index)
		{
			hoverThis = index;
			//superTextMesh.Rebuild(superTextMesh.currentReadTime, superTextMesh.reading);
		}
		internal void ExitLink(int index)
		{
			hoverThis = -1;
			//superTextMesh.Rebuild(superTextMesh.currentReadTime, superTextMesh.reading);
		}
		
		void Reset()
		{
			superTextMesh = GetComponent<SuperTextMesh>();
		}
		void OnEnable()
		{
			//in unity versions before 2017.2.1f1, this will cause an error when first added: 
			//https://issuetracker.unity3d.com/issues/onenable-is-called-before-reset-with-executeineditmode-enabled
			superTextMesh.OnPreParse += PreparseTags;
			if(Application.isPlaying)
			{
				superTextMesh.OnCustomEvent += GenerateLink;
				superTextMesh.OnRebuildEvent += ClearLinks;
			}
		}	
		void OnDisable()
		{
			superTextMesh.OnPreParse -= PreparseTags;
			if(Application.isPlaying)
			{
				superTextMesh.OnCustomEvent -= GenerateLink;
				superTextMesh.OnRebuildEvent -= ClearLinks;
			}
		}
		public void ClearLinks()
		{
			//reset and clear all lists.
			for(int i=0; i<linkObjects.Count; i++)
			{
				Destroy(linkObjects[i].go);
			}
			linkObjects.Clear();
			currentLinkObject = null;
		}
		public void GenerateLink(string text, STMTextInfo info)
		{
			string[] splitText = text.Split(',');
			if(splitText.Length == 2 && splitText[0] == "link")
			{
				//do something with splitText[1]

				/*
				ok for now uhh...
				link prefab over every letter?
				or ability to generate a collider at X line that stretches for hoever long,
				and can carry over to new lines...

				*/

				Link myLink = links.Find(x => x.label == splitText[1]);
				if(myLink == null)
				{
					Debug.Log("No link with tag '" + splitText[1] + "' found!");
					return;
				}
				int myLinkIndex = links.IndexOf(myLink);
				float lineHeight = superTextMesh.lineHeights[info.line];
			//	CharBounds myBounds = new CharBounds(info.pos.x, info.BottomRightVert.x, info.pos.y, lineHeight, info.line, info.rawIndex);
				CharInfo myInfo = new CharInfo(info.pos.x - padding.x, info.pos.y - padding.y, info.BottomRightVert.x + padding.x, info.pos.y + lineHeight + padding.y, info.line, myLinkIndex, info.rawIndex);
				//allPos.Add(new CharBounds(info.pos.x, info.BottomRightVert.x, info.line, info.rawIndex));

				//check again to see if it's been cleared or is clear.
				//but dont start on the last character of a line.....
				//bool doEncapsulate = false;
				if(currentLinkObject != null)
				{
					//needs to be...
					//same line
					//character comes just before
					//same link index
					if(currentLinkObject.bounds.min.y == myInfo.bounds.min.y && currentLinkObject.lastCharacterIndex == info.rawIndex-1 && currentLinkObject.linkIndex == myLinkIndex)
					{
						currentLinkObject.Encapsulate(myInfo);
					}
					else
					{
						currentLinkObject = null;
					}
				}
				if(currentLinkObject == null)
				{
					//currentLinkArea = new LinkArea(myBounds);
					GameObject go = new GameObject();
					LinkObject linkObj;
					if(superTextMesh.uiMode)
					{
						linkObj = go.AddComponent<LinkObjectUI>();
					}
					else
					{
						linkObj = go.AddComponent<LinkCollider2D>();
					}
					linkObj.Initialize(myInfo, 
										this, 
										"Link for tag '" + splitText[1] + "' at index " + info.rawIndex.ToString() + " with character " + info.character, 
										links[myLinkIndex], 
										onEnter, 
										onExit);
					linkObjects.Add(linkObj);
					currentLinkObject = linkObj;
				}
				//if the LAST position added is on the same line, and is immediately before this one...
				
			}		
		}
	}
}