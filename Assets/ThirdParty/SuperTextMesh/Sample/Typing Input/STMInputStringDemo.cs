//Copyright (c) 2016 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public class STMInputStringDemo : MonoBehaviour {
	public SuperTextMesh rawstm;
	public SuperTextMesh stm;
	void Update () {
		string c = Input.inputString;
		if(Input.GetKeyDown(KeyCode.Backspace)){
			rawstm.text = rawstm.text.Substring(0, rawstm.text.Length-1); //remove last character
			rawstm.Rebuild();
		}
		for(int i=0, iL=c.Length; i<iL; i++){
			if(c[i] == '\b'){ //backspace?
				//if(rawstm.text.Length > 0){
				//	rawstm.text = rawstm.text.Substring(0, rawstm.text.Length-1); //remove last character
				//}
			}else{
				//if(c == '\n' || c == '\r'){ //enter key was pressed

				//}
				rawstm.text += c[i].ToString();
			}
		}
		if(c.Length > 0){
			rawstm.Rebuild();
		}
	}
	public void UpdateBox(){
		stm.Text = rawstm.text;
	}
}
