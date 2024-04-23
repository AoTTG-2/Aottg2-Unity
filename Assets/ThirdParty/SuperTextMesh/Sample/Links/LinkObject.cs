//Copyright (c) 2022 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace STMTools.Links
{
	public abstract class LinkObject : MonoBehaviour 
	{
		internal Transform t;
		internal GameObject go;
		internal Bounds bounds = new Bounds();
		[SerializeField] protected UnityEvent onClick;
		[SerializeField] protected UnityEvent onEnter;
		[SerializeField] protected UnityEvent onExit;

		public delegate void OnClickAction();
		public event OnClickAction OnClickEvent;
		public delegate void OnEnterAction();
		public event OnEnterAction OnEnterEvent;
		public delegate void OnExitAction();
		public event OnExitAction OnExitEvent;

		//internal int line; //used for instantiating
		internal int linkIndex; //same
		internal int lastCharacterIndex;
		internal LinkController controller;

		internal abstract void Initialize(CharInfo charInfo, LinkController controller, string name, Link link, UnityEvent onEnter, UnityEvent onExit);
		internal abstract void Encapsulate(CharInfo charInfo);

		protected virtual void OnClick()
		{
			if(onClick != null) onClick.Invoke();
			if(OnClickEvent != null) OnClickEvent();
		}
		protected virtual void OnEnter()
		{
			controller.EnterLink(linkIndex);
			if(onEnter != null) onEnter.Invoke();
			if(OnEnterEvent != null) OnEnterEvent();
		}
		protected virtual void OnExit()
		{
			controller.ExitLink(linkIndex);
			if(onExit != null) onExit.Invoke();
			if(OnExitEvent != null) OnExitEvent();
		}
	}
}