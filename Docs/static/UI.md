# UI
Inherits from object
## Initialization
<span style="color:red;">This class is abstract and cannot be instantiated.</span>
## Static Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|TopCenter|string|True|"TopCenter" constant|
|TopLeft|string|True|"TopLeft" constant|
|TopRight|string|True|"TopRight" constant|
|MiddleCenter|string|True|"MiddleCenter" constant|
|MiddleLeft|string|True|"MiddleLeft" constant|
|MiddleRight|string|True|"MiddleRight" constant|
|BottomCenter|string|True|"BottomCenter" constant|
|BottomLeft|string|True|"BottomLeft" constant|
|BottomRight|string|True|"BottomRight" constant|
|GetPopups|[List](../objects/List.md)|True|Returns a list of all popups|
## Static Methods
#### function <span style="color:yellow;">SetLabel</span>(label: <span style="color:blue;">string</span>, message: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".

#### function <span style="color:yellow;">SetLabelForTime</span>(label: <span style="color:blue;">string</span>, message: <span style="color:blue;">string</span>, time: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Sets the label for a certain time, after which it will be cleared.

#### function <span style="color:yellow;">SetLabelAll</span>(label: <span style="color:blue;">string</span>, message: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets the label for all players. Master client only. Be careful not to call this often.

#### function <span style="color:yellow;">SetLabelForTimeAll</span>(label: <span style="color:blue;">string</span>, message: <span style="color:blue;">string</span>, time: <span style="color:blue;">float</span>) → <span style="color:blue;">null</span>
> Sets the label for all players for a certain time. Master client only.

#### function <span style="color:yellow;">CreatePopup</span>(popupName: <span style="color:blue;">string</span>, title: <span style="color:blue;">string</span>, width: <span style="color:blue;">int</span>, height: <span style="color:blue;">int</span>) → <span style="color:blue;">string</span>
> Creates a new popup. This popup is hidden until shown.

#### function <span style="color:yellow;">ShowPopup</span>(popupName: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Shows the popup with given name.

#### function <span style="color:yellow;">HidePopup</span>(popupName: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Hides the popup with given name.

#### function <span style="color:yellow;">ClearPopup</span>(popupName: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Clears all elements in popup with given name.

#### function <span style="color:yellow;">AddPopupLabel</span>(popupName: <span style="color:blue;">string</span>, label: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Adds a text row to the popup with label as content.

#### function <span style="color:yellow;">AddPopupButton</span>(popupName: <span style="color:blue;">string</span>, label: <span style="color:blue;">string</span>, callback: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.

#### function <span style="color:yellow;">AddPopupBottomButton</span>(popupName: <span style="color:blue;">string</span>, label: <span style="color:blue;">string</span>, callback: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Adds a button to the bottom bar of the popup.

#### function <span style="color:yellow;">AddPopupButtons</span>(popupName: <span style="color:blue;">string</span>, labels: <span style="color:blue;">[List](../objects/List.md)</span>, callbacks: <span style="color:blue;">[List](../objects/List.md)</span>) → <span style="color:blue;">null</span>
> Adds a list of buttons in a row to the popup.

#### function <span style="color:yellow;">WrapStyleTag</span>(text: <span style="color:blue;">string</span>, style: <span style="color:blue;">string</span>, arg: <span style="color:blue;">string</span> = <span style="color:blue;">null</span>) → <span style="color:blue;">string</span>
> Returns a wrapped string given style and args.

#### function <span style="color:yellow;">GetLocale</span>(cat: <span style="color:blue;">string</span>, sub: <span style="color:blue;">string</span>, key: <span style="color:blue;">string</span>) → <span style="color:blue;">string</span>
> Gets translated locale from the current Language.json file with given category, subcategory, and key pattern.

#### function <span style="color:yellow;">GetLanguage</span>() → <span style="color:blue;">string</span>
> Returns the current language (e.g. "English" or "简体中文").

#### function <span style="color:yellow;">ShowChangeCharacterMenu</span>() → <span style="color:blue;">null</span>
> Shows the change character menu if main character is Human.

#### function <span style="color:yellow;">SetScoreboardHeader</span>(header: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets the display of the scoreboard header (default "Kills / Deaths...")

#### function <span style="color:yellow;">SetScoreboardProperty</span>(property: <span style="color:blue;">string</span>) → <span style="color:blue;">null</span>
> Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.

#### function <span style="color:yellow;">GetThemeColor</span>(panel: <span style="color:blue;">string</span>, category: <span style="color:blue;">string</span>, item: <span style="color:blue;">string</span>) → <span style="color:blue;">[Color](../objects/Color.md)</span>
> Gets the color of the specified item. See theme json for reference.

#### function <span style="color:yellow;">IsPopupActive</span>(popupName: <span style="color:blue;">string</span>) → <span style="color:blue;">bool</span>
> Returns if the given popup is active


---

