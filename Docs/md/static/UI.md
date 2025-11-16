# UI
Inherits from [Object](../objects/Object.md)

UI label functions.

### Static Properties
|Name|Type|Readonly|Description|
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


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetLabel(label: string, message: string)</code></pre>
> Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelForTime(label: string, message: string, time: float)</code></pre>
> Sets the label for a certain time, after which it will be cleared.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelAll(label: string, message: string)</code></pre>
> Sets the label for all players. Master client only. Be careful not to call this often.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelForTimeAll(label: string, message: string, time: float)</code></pre>
> Sets the label for all players for a certain time. Master client only.
> 
<pre class="language-typescript"><code class="lang-typescript">function CreatePopup(popupName: string, title: string, width: int, height: int) -> string</code></pre>
> Creates a new popup. This popup is hidden until shown.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowPopup(popupName: string)</code></pre>
> Shows the popup with given name.
> 
<pre class="language-typescript"><code class="lang-typescript">function HidePopup(popupName: string)</code></pre>
> Hides the popup with given name.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearPopup(popupName: string)</code></pre>
> Clears all elements in popup with given name.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupLabel(popupName: string, label: string)</code></pre>
> Adds a text row to the popup with label as content.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupButton(popupName: string, label: string, callback: string)</code></pre>
> Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupBottomButton(popupName: string, label: string, callback: string)</code></pre>
> Adds a button to the bottom bar of the popup.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupButtons(popupName: string, labels: <a data-footnote-ref href="#user-content-fn-18">List</a>, callbacks: <a data-footnote-ref href="#user-content-fn-18">List</a>)</code></pre>
> Adds a list of buttons in a row to the popup.
> 
<pre class="language-typescript"><code class="lang-typescript">function WrapStyleTag(text: string, style: string, arg: string = null) -> string</code></pre>
> Returns a wrapped string given style and args.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowChangeCharacterMenu()</code></pre>
> Shows the change character menu if main character is Human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScoreboardHeader(header: string)</code></pre>
> Sets the display of the scoreboard header (default "Kills / Deaths...")
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScoreboardProperty(property: string)</code></pre>
> Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetThemeColor(panel: string, category: string, item: string) -> <a data-footnote-ref href="#user-content-fn-7">Color</a></code></pre>
> Gets the color of the specified item. See theme json for reference.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPopupActive(popupName: string) -> bool</code></pre>
> Returns if the given popup is active
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelActive(label: string, active: bool)</code></pre>
> Sets whether a label is active or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKDRPanelActive(active: bool)</code></pre>
> Sets whether the KDR panel (top-left) is active or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetMinimapActive(active: bool)</code></pre>
> Sets whether the minimap is active or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetChatPanelActive(active: bool)</code></pre>
> Sets whether the chat panel is active or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetFeedPanelActive(active: bool)</code></pre>
> Sets whether the feed panel is active or not.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetBottomHUDActive(active: bool)</code></pre>
> Sets whether the bottom HUD is active or not.
This can only be used when the character is alive.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetRootVisualElement() -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Returns the root `VisualElement` which you can add other elements to.
> 
> **Returns**: The root `VisualElement`
<pre class="language-typescript"><code class="lang-typescript">function VisualElement() -> <a data-footnote-ref href="#user-content-fn-56">VisualElement</a></code></pre>
> Creates a new `VisualElement`.
> 
<pre class="language-typescript"><code class="lang-typescript">function Button(text: string = "", clickEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-48">Button</a></code></pre>
> Creates a new `Button` with optional text and click event.
> 
> **Parameters**:
> - `text`: The text that the button displays
> - `clickEvent`: The function that will be called when button is clicked
> 
<pre class="language-typescript"><code class="lang-typescript">function Label(text: string = "") -> <a data-footnote-ref href="#user-content-fn-50">Label</a></code></pre>
> Creates a new `Label` with optional text.
> 
> **Parameters**:
> - `text`: The text to be displayed
> 
<pre class="language-typescript"><code class="lang-typescript">function TextField(label: string = "") -> <a data-footnote-ref href="#user-content-fn-54">TextField</a></code></pre>
> Creates a new `TextField` with optional label.
> 
<pre class="language-typescript"><code class="lang-typescript">function Toggle(label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-55">Toggle</a></code></pre>
> Creates a new `Toggle` with optional label and value changed event.
> 
> **Parameters**:
> - `label`: The label text displayed next to the toggle
> - `valueChangedEvent`: The function that will be called when toggle value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function Slider(lowValue: float = 0, highValue: float = 100, tickInterval: float = 0, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-53">Slider</a></code></pre>
> Creates a new `Slider` for floating-point values with optional range, tick interval, and value changed event.
The slider will snap to values at multiples of the tick interval.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the slider
> - `highValue`: The maximum value of the slider
> - `tickInterval`: The interval between allowed values. If 0, no snapping occurs. For example, 0.1 will snap to 0.0, 0.1, 0.2, etc.
> - `label`: The label text displayed next to the slider
> - `valueChangedEvent`: The function that will be called when slider value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function SliderInt(lowValue: int = 0, highValue: int = 100, tickInterval: int = 1, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-53">Slider</a></code></pre>
> Creates a new `Slider` for integer values with optional range, tick interval, and value changed event.
The slider will snap to values at multiples of the tick interval.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the slider
> - `highValue`: The maximum value of the slider
> - `tickInterval`: The interval between allowed values. For example, 5 will snap to 0, 5, 10, 15, etc.
> - `label`: The label text displayed next to the slider
> - `valueChangedEvent`: The function that will be called when slider value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function Dropdown(choices: <a data-footnote-ref href="#user-content-fn-18">List</a>, defaultIndex: int = 0, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-49">Dropdown</a></code></pre>
> Creates a new `Dropdown` with a list of choices and optional label and value changed event.
> 
> **Parameters**:
> - `choices`: List of string options to display in the dropdown
> - `defaultIndex`: The index of the initially selected option (default: 0)
> - `label`: The label text displayed next to the dropdown
> - `valueChangedEvent`: The function that will be called when dropdown value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function ProgressBar(lowValue: float = 0, highValue: float = 100, title: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-51">ProgressBar</a></code></pre>
> Creates a new `ProgressBar` with optional range, title, and value changed event.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the progress bar (default: 0)
> - `highValue`: The maximum value of the progress bar (default: 100)
> - `title`: The title text displayed on the progress bar
> - `valueChangedEvent`: The function that will be called when progress bar value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function ScrollView() -> <a data-footnote-ref href="#user-content-fn-52">ScrollView</a></code></pre>
> Creates a new `ScrollView` for scrollable content.
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
