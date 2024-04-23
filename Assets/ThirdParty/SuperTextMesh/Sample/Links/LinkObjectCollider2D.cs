//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace STMTools.Links
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class LinkCollider2D : LinkObject 
	{
		private BoxCollider2D box;

		internal override void Initialize(CharInfo charInfo, LinkController controller, string name, Link link, UnityEvent onEnter, UnityEvent onExit)
		{
			t = this.transform;
			go = this.gameObject;
			onClick = link.onClick;
			this.onEnter = onEnter;
			this.onExit = onExit;
			t.parent = controller.superTextMesh.t;
			t.name = name;
			box = t.GetComponent<BoxCollider2D>();
			bounds = new Bounds(charInfo.bounds.center, charInfo.bounds.size);
			this.controller = controller;
			SetBoundingBox();

			Encapsulate(charInfo);
		}
		private void SetBoundingBox()
		{
			t.localPosition = bounds.min;
			box.size = bounds.size;
			box.offset = box.size / 2f;
		}
		internal override void Encapsulate(CharInfo charInfo)
		{
			bounds.Encapsulate(charInfo.bounds);
			SetBoundingBox();

			//this.line = charInfo.line;
			this.linkIndex = charInfo.linkIndex;
			this.lastCharacterIndex = charInfo.charIndex;
		}
		private void OnMouseDown()
		{
			OnClick();
		}
		private void OnMouseEnter()
		{
			OnEnter();
		}
		private void OnMouseExit()
		{
			OnExit();
		}
	}
}