using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//updates STM text to match the value of UI text on enable. Mainly for integrating with hard-coded components, notably, UI dropdowns
public class STMUITextUpdater : MonoBehaviour {

	public Text uiText;
	public SuperTextMesh stm;
	private bool needsUpdate = true;

	void OnEnable()
	{
		needsUpdate = true;
	}
	void LateUpdate()
	{
		if(needsUpdate)
		{
			needsUpdate = true;
			stm.text = uiText.text;
			stm.gameObject.SetActive(uiText.enabled); //copy enabled status too
		}
	}
	public void UpdateText()
	{
		needsUpdate = true;
	}
}
