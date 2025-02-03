# UI
Inherits from object
## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|Popups|[List](../Object/List.md)|False|Returns a list of all popups|
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|TopCenter|[String](../Static/String.md)|False|"TopCenter" constant|
|TopLeft|[String](../Static/String.md)|False|"TopLeft" constant|
|TopRight|[String](../Static/String.md)|False|"TopRight" constant|
|MiddleCenter|[String](../Static/String.md)|False|"MiddleCenter" constant|
|MiddleLeft|[String](../Static/String.md)|False|"MiddleLeft" constant|
|MiddleRight|[String](../Static/String.md)|False|"MiddleRight" constant|
|BottomCenter|[String](../Static/String.md)|False|"BottomCenter" constant|
|BottomLeft|[String](../Static/String.md)|False|"BottomLeft" constant|
|BottomRight|[String](../Static/String.md)|False|"BottomRight" constant|
## Methods
|Function|Returns|Description|
|---|---|---|
|SetLabel(label : [String](../Static/String.md), message : [String](../Static/String.md))|none|Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".|
|SetLabelForTime(label : [String](../Static/String.md), message : [String](../Static/String.md), time : float)|none|Sets the label for a certain time, after which it will be cleared.|
|SetLabelAll(label : [String](../Static/String.md), message : [String](../Static/String.md))|none|Sets the label for all players. Master client only. Be careful not to call this often.|
|SetLabelForTimeAll(label : [String](../Static/String.md), message : [String](../Static/String.md), time : float)|none|Sets the label for all players for a certain time. Master client only.|
|CreatePopup(popupName : [String](../Static/String.md), title : [String](../Static/String.md), width : int, height : int)|[String](../Static/String.md)|Creates a new popup. This popup is hidden until shown.|
|ShowPopup(popupName : [String](../Static/String.md))|none|Shows the popup with given name.|
|HidePopup(popupName : [String](../Static/String.md))|none|Hides the popup with given name.|
|ClearPopup(popupName : [String](../Static/String.md))|none|Clears all elements in popup with given name.|
|AddPopupLabel(popupName : [String](../Static/String.md), label : [String](../Static/String.md))|none|Adds a text row to the popup with label as content.|
|AddPopupButton(popupName : [String](../Static/String.md), label : [String](../Static/String.md), callback : [String](../Static/String.md))|none|Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.|
|AddPopupBottomButton(popupName : [String](../Static/String.md), label : [String](../Static/String.md), callback : [String](../Static/String.md))|none|Adds a button to the bottom bar of the popup.|
|AddPopupButtons(popupName : [String](../Static/String.md), labels : [List](../Object/List.md), callbacks : [List](../Object/List.md))|none|Adds a list of buttons in a row to the popup.|
|WrapStyleTag(text : [String](../Static/String.md), style : [String](../Static/String.md), arg : [String](../Static/String.md) = )|[String](../Static/String.md)|Returns a wrapped string given style and args.|
|GetLocale(cat : [String](../Static/String.md), sub : [String](../Static/String.md), key : [String](../Static/String.md))|[String](../Static/String.md)|Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.|
|GetLanguage()|[String](../Static/String.md)|Returns the current language (e.g. "English" or "简体中文").|
|ShowChangeCharacterMenu()|none|Shows the change character menu if main character is Human.|
|SetScoreboardHeader(header : [String](../Static/String.md))|none|Sets the display of the scoreboard header (default "Kills / Deaths...")|
|SetScoreboardProperty(property : [String](../Static/String.md))|none|Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.|
|GetThemeColor(panel : [String](../Static/String.md), category : [String](../Static/String.md), item : [String](../Static/String.md))|[Color](../Static/Color.md)|Gets the color of the specified item. See theme json for reference.|
|IsPopupActive(popupName : [String](../Static/String.md))|bool|Returns if the given popup is active|
