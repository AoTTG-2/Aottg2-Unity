using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class STMSimpleButton : MonoBehaviour {
	public UnityEvent buttonEvent;
	
	public Vector3 normalSize = Vector3.one;
	public Vector3 mouseoverSize = Vector3.one;
	public Vector3 clickSize = Vector3.one;

	public void OnMouseEnter(){
		transform.localScale = mouseoverSize;
	}
	public void OnMouseExit(){
		transform.localScale = normalSize;
	}
	public void OnMouseDown(){
		transform.localScale = clickSize;
		buttonEvent.Invoke();
	}
	public void OnMouseUp(){
		transform.localScale = mouseoverSize;
	}
}
