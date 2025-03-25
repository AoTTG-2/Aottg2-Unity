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
|GetPopups|[List](../objects/List.md)|False|Returns a list of all popups|
## Static Methods
#### function <mark style="color:yellow;">SetLabel</mark>(label: <mark style="color:blue;">[String](../static/String.md)</mark>, message: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".

#### function <mark style="color:yellow;">SetLabelForTime</mark>(label: <mark style="color:blue;">[String](../static/String.md)</mark>, message: <mark style="color:blue;">[String](../static/String.md)</mark>, time: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">void</mark>
> Sets the label for a certain time, after which it will be cleared.

#### function <mark style="color:yellow;">SetLabelAll</mark>(label: <mark style="color:blue;">[String](../static/String.md)</mark>, message: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the label for all players. Master client only. Be careful not to call this often.

#### function <mark style="color:yellow;">SetLabelForTimeAll</mark>(label: <mark style="color:blue;">[String](../static/String.md)</mark>, message: <mark style="color:blue;">[String](../static/String.md)</mark>, time: <mark style="color:blue;">float</mark>) -> <mark style="color:blue;">void</mark>
> Sets the label for all players for a certain time. Master client only.

#### function <mark style="color:yellow;">CreatePopup</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>, title: <mark style="color:blue;">[String](../static/String.md)</mark>, width: <mark style="color:blue;">int</mark>, height: <mark style="color:blue;">int</mark>) -> <mark style="color:blue;">[String](../static/String.md)</mark>
> Creates a new popup. This popup is hidden until shown.

#### function <mark style="color:yellow;">ShowPopup</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Shows the popup with given name.

#### function <mark style="color:yellow;">HidePopup</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Hides the popup with given name.

#### function <mark style="color:yellow;">ClearPopup</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Clears all elements in popup with given name.

#### function <mark style="color:yellow;">AddPopupLabel</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>, label: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Adds a text row to the popup with label as content.

#### function <mark style="color:yellow;">AddPopupButton</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>, label: <mark style="color:blue;">[String](../static/String.md)</mark>, callback: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.

#### function <mark style="color:yellow;">AddPopupBottomButton</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>, label: <mark style="color:blue;">[String](../static/String.md)</mark>, callback: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Adds a button to the bottom bar of the popup.

#### function <mark style="color:yellow;">AddPopupButtons</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>, labels: <mark style="color:blue;">[List](../objects/List.md)</mark>, callbacks: <mark style="color:blue;">[List](../objects/List.md)</mark>) -> <mark style="color:blue;">void</mark>
> Adds a list of buttons in a row to the popup.

#### function <mark style="color:yellow;">WrapStyleTag</mark>(text: <mark style="color:blue;">[String](../static/String.md)</mark>, style: <mark style="color:blue;">[String](../static/String.md)</mark>, arg: <mark style="color:blue;">[String](../static/String.md)</mark> = <mark style="color:blue;">null</mark>) -> <mark style="color:blue;">[String](../static/String.md)</mark>
> Returns a wrapped string given style and args.

#### function <mark style="color:yellow;">GetLocale</mark>(cat: <mark style="color:blue;">[String](../static/String.md)</mark>, sub: <mark style="color:blue;">[String](../static/String.md)</mark>, key: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">[String](../static/String.md)</mark>
> Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.

#### function <mark style="color:yellow;">GetLanguage</mark>() -> <mark style="color:blue;">[String](../static/String.md)</mark>
> Returns the current language (e.g. "English" or "简体中文").

#### function <mark style="color:yellow;">ShowChangeCharacterMenu</mark>() -> <mark style="color:blue;">void</mark>
> Shows the change character menu if main character is Human.

#### function <mark style="color:yellow;">SetScoreboardHeader</mark>(header: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets the display of the scoreboard header (default "Kills / Deaths...")

#### function <mark style="color:yellow;">SetScoreboardProperty</mark>(property: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">void</mark>
> Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.

#### function <mark style="color:yellow;">GetThemeColor</mark>(panel: <mark style="color:blue;">[String](../static/String.md)</mark>, category: <mark style="color:blue;">[String](../static/String.md)</mark>, item: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">[Color](../objects/Color.md)</mark>
> Gets the color of the specified item. See theme json for reference.

#### function <mark style="color:yellow;">IsPopupActive</mark>(popupName: <mark style="color:blue;">[String](../static/String.md)</mark>) -> <mark style="color:blue;">bool</mark>
> Returns if the given popup is active


---

