# UI
Inherits from object
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|TopCenter|[String](../static/String.md)|False|"TopCenter" constant|
|TopLeft|[String](../static/String.md)|False|"TopLeft" constant|
|TopRight|[String](../static/String.md)|False|"TopRight" constant|
|MiddleCenter|[String](../static/String.md)|False|"MiddleCenter" constant|
|MiddleLeft|[String](../static/String.md)|False|"MiddleLeft" constant|
|MiddleRight|[String](../static/String.md)|False|"MiddleRight" constant|
|BottomCenter|[String](../static/String.md)|False|"BottomCenter" constant|
|BottomLeft|[String](../static/String.md)|False|"BottomLeft" constant|
|BottomRight|[String](../static/String.md)|False|"BottomRight" constant|
|Popups|[List](../objects/List.md)|False|Returns a list of all popups|
## Static Methods
<table>
<colgroup><col style="width: 30%"/>
<col style="width: 20%"/>
<col style="width: 50%"/>
</colgroup>
<thead>
<tr>
<th>Function</th>
<th>Returns</th>
<th>Description</th>
</tr>
</thead>
<tbody>
<tr>
<td>SetLabel(label : [String](../static/String.md),message : [String](../static/String.md))</td>
<td>none</td>
<td>Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".</td>
</tr>
<tr>
<td>SetLabelForTime(label : [String](../static/String.md),message : [String](../static/String.md),time : float)</td>
<td>none</td>
<td>Sets the label for a certain time, after which it will be cleared.</td>
</tr>
<tr>
<td>SetLabelAll(label : [String](../static/String.md),message : [String](../static/String.md))</td>
<td>none</td>
<td>Sets the label for all players. Master client only. Be careful not to call this often.</td>
</tr>
<tr>
<td>SetLabelForTimeAll(label : [String](../static/String.md),message : [String](../static/String.md),time : float)</td>
<td>none</td>
<td>Sets the label for all players for a certain time. Master client only.</td>
</tr>
<tr>
<td>CreatePopup(popupName : [String](../static/String.md),title : [String](../static/String.md),width : int,height : int)</td>
<td>[String](../static/String.md)</td>
<td>Creates a new popup. This popup is hidden until shown.</td>
</tr>
<tr>
<td>ShowPopup(popupName : [String](../static/String.md))</td>
<td>none</td>
<td>Shows the popup with given name.</td>
</tr>
<tr>
<td>HidePopup(popupName : [String](../static/String.md))</td>
<td>none</td>
<td>Hides the popup with given name.</td>
</tr>
<tr>
<td>ClearPopup(popupName : [String](../static/String.md))</td>
<td>none</td>
<td>Clears all elements in popup with given name.</td>
</tr>
<tr>
<td>AddPopupLabel(popupName : [String](../static/String.md),label : [String](../static/String.md))</td>
<td>none</td>
<td>Adds a text row to the popup with label as content.</td>
</tr>
<tr>
<td>AddPopupButton(popupName : [String](../static/String.md),label : [String](../static/String.md),callback : [String](../static/String.md))</td>
<td>none</td>
<td>Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.</td>
</tr>
<tr>
<td>AddPopupBottomButton(popupName : [String](../static/String.md),label : [String](../static/String.md),callback : [String](../static/String.md))</td>
<td>none</td>
<td>Adds a button to the bottom bar of the popup.</td>
</tr>
<tr>
<td>AddPopupButtons(popupName : [String](../static/String.md),labels : [List](../objects/List.md),callbacks : [List](../objects/List.md))</td>
<td>none</td>
<td>Adds a list of buttons in a row to the popup.</td>
</tr>
<tr>
<td>WrapStyleTag(text : [String](../static/String.md),style : [String](../static/String.md),arg : [String](../static/String.md) = )</td>
<td>[String](../static/String.md)</td>
<td>Returns a wrapped string given style and args.</td>
</tr>
<tr>
<td>GetLocale(cat : [String](../static/String.md),sub : [String](../static/String.md),key : [String](../static/String.md))</td>
<td>[String](../static/String.md)</td>
<td>Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.</td>
</tr>
<tr>
<td>GetLanguage()</td>
<td>[String](../static/String.md)</td>
<td>Returns the current language (e.g. "English" or "简体中文").</td>
</tr>
<tr>
<td>ShowChangeCharacterMenu()</td>
<td>none</td>
<td>Shows the change character menu if main character is Human.</td>
</tr>
<tr>
<td>SetScoreboardHeader(header : [String](../static/String.md))</td>
<td>none</td>
<td>Sets the display of the scoreboard header (default "Kills / Deaths...")</td>
</tr>
<tr>
<td>SetScoreboardProperty(property : [String](../static/String.md))</td>
<td>none</td>
<td>Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.</td>
</tr>
<tr>
<td>GetThemeColor(panel : [String](../static/String.md),category : [String](../static/String.md),item : [String](../static/String.md))</td>
<td>[Color](../objects/Color.md)</td>
<td>Gets the color of the specified item. See theme json for reference.</td>
</tr>
<tr>
<td>IsPopupActive(popupName : [String](../static/String.md))</td>
<td>bool</td>
<td>Returns if the given popup is active</td>
</tr>
</tbody>
</table>
