using UnityEngine;
using System.Collections;

public class STMPreparse3 : MonoBehaviour {

	public string textTag = "transcribe";
	public void Parse(STMTextContainer x)
	{
		string startTag = "<" + textTag + ">";
		string endTag = "</" + textTag + ">";

		int startingPoint;
		int endingPoint;

		do
		{
			startingPoint = x.text.IndexOf(startTag);
			endingPoint = startingPoint > -1 ? x.text.IndexOf(endTag,startingPoint) : -1; //get tag after starting tag point
			//optional, where this tag ends
			if(endingPoint == -1)
			{
				endingPoint = x.text.Length;
			}
			else
			{
				//remove tag
				x.text = x.text.Remove(endingPoint, endTag.Length);
				//ending point is already accurate
			}
			//if this tag exists in STM's string...
			if(startingPoint > -1)
			{
				//remove tag
				x.text = x.text.Remove(startingPoint, startTag.Length);
				//push backwards
				endingPoint -= startTag.Length;

				//actually modify text
				Replace(x, startingPoint, endingPoint);
			}
		}
		//repeat while the tag still exists in the string
		while(startingPoint > -1);

	}
	void Replace(STMTextContainer x, int startingPoint, int endingPoint)
	{
		//int originalLength = x.text.Length;
		int skippedChars = startingPoint;
		bool parsingOn = true;
		//go thru string
		for(int i=startingPoint; i<endingPoint; i++) //for each letter in the original string...
		{
			
			string replaceValue = x.text[skippedChars].ToString(); //default value

			if(replaceValue == "<")
			{
				//turn off parsing
				parsingOn = false;
			}
			else if(replaceValue == ">")
			{
				//turn back on
				parsingOn = true;
			}
			//is this a letter that should be replaced?
			if(parsingOn)
			{
				//replace specific characters with sequences
				//for this example, compare all letters as uppercase letters
				switch(x.text[skippedChars].ToString().ToUpper())
				{
					case "A": replaceValue = "aaa"; break;
					case "B": replaceValue = "bbb"; break;
					//etc etc...
				}
				//remove original character
				x.text = x.text.Remove(skippedChars, 1);
				//replace with sequence
				x.text = x.text.Insert(skippedChars, replaceValue);
			}

			//1 by default, but adds up if more characters are inserted
			skippedChars += replaceValue.Length;
		}
	}

}
