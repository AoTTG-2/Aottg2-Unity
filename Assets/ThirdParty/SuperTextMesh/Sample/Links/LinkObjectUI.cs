//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
Having an EventSystem Gameobject in your scene is needed for IPointerClickHandler to work!
*/

namespace  STMTools.Links
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Image))] //needed because "Graphic" accepts raycasts.
	public class LinkObjectUI : LinkObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		private RectTransform tr;
		private Image image; //needed for raycast. ok.

		internal override void Initialize(CharInfo charInfo, LinkController controller, string name, Link link, UnityEvent onEnter, UnityEvent onExit)
		{
			t = this.transform;
			t.SetParent(controller.superTextMesh.t);
			go = this.gameObject;
			tr = t.GetComponent<RectTransform>();
			onClick = link.onClick;
			this.onEnter = onEnter;
			this.onExit = onExit;
			t.name = name;
			image = t.GetComponent<Image>();
			image.color = Color.clear;
			bounds = new Bounds(charInfo.bounds.center, charInfo.bounds.size);
			this.controller = controller;
			SetBoundingBox();

			Encapsulate(charInfo);
		}
		private void SetBoundingBox()
		{
			t.localPosition = bounds.min + bounds.size / 2f;
			tr.sizeDelta = bounds.size;
		}
		internal override void Encapsulate(CharInfo charInfo)
		{
			bounds.Encapsulate(charInfo.bounds);
			SetBoundingBox();

			//this.line = charInfo.line;
			this.linkIndex = charInfo.linkIndex;
			this.lastCharacterIndex = charInfo.charIndex;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			OnClick();
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			OnEnter();
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			OnExit();
		}
	}
}

