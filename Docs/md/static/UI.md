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
|GetPopups|[List](../objects/List.md)<string>|True|Returns a list of all popups|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetLabel(label: string, message: string)</code></pre>
> Sets the label at a certain location. Valid types: "TopCenter", "TopLeft", "TopRight", "MiddleCenter", "MiddleLeft", "MiddleRight", "BottomLeft", "BottomRight", "BottomCenter".
> 
> **Parameters**:
> - `label`: The label location.
> - `message`: The message to display.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelForTime(label: string, message: string, time: float)</code></pre>
> Sets the label for a certain time, after which it will be cleared.
> 
> **Parameters**:
> - `label`: The label location.
> - `message`: The message to display.
> - `time`: The time in seconds before the label is cleared.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelAll(label: string, message: string)</code></pre>
> Sets the label for all players. Master client only. Be careful not to call this often.
> 
> **Parameters**:
> - `label`: The label location.
> - `message`: The message to display.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelForTimeAll(label: string, message: string, time: float)</code></pre>
> Sets the label for all players for a certain time. Master client only.
> 
> **Parameters**:
> - `label`: The label location.
> - `message`: The message to display.
> - `time`: The time in seconds before the label is cleared.
> 
<pre class="language-typescript"><code class="lang-typescript">function CreatePopup(popupName: string, title: string, width: int, height: int) -> string</code></pre>
> Creates a new popup. This popup is hidden until shown.
> 
> **Parameters**:
> - `popupName`: The name of the popup.
> - `title`: The title of the popup.
> - `width`: The width of the popup.
> - `height`: The height of the popup.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowPopup(popupName: string)</code></pre>
> Shows the popup with given name.
> 
> **Parameters**:
> - `popupName`: The name of the popup to show.
> 
<pre class="language-typescript"><code class="lang-typescript">function HidePopup(popupName: string)</code></pre>
> Hides the popup with given name.
> 
> **Parameters**:
> - `popupName`: The name of the popup to hide.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClearPopup(popupName: string)</code></pre>
> Clears all elements in popup with given name.
> 
> **Parameters**:
> - `popupName`: The name of the popup to clear.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupLabel(popupName: string, label: string)</code></pre>
> Adds a text row to the popup with label as content.
> 
> **Parameters**:
> - `popupName`: The name of the popup.
> - `label`: The label text to add.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupButton(popupName: string, label: string, callback: string)</code></pre>
> Adds a button row to the popup with given button name and display text. When button is pressed, OnButtonClick is called in Main with buttonName parameter.
> 
> **Parameters**:
> - `popupName`: The name of the popup.
> - `label`: The button display text.
> - `callback`: The callback name that will be passed to OnButtonClick in Main.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupBottomButton(popupName: string, label: string, callback: string)</code></pre>
> Adds a button to the bottom bar of the popup.
> 
> **Parameters**:
> - `popupName`: The name of the popup.
> - `label`: The button display text.
> - `callback`: The callback name that will be passed to OnButtonClick in Main.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddPopupButtons(popupName: string, labels: <a data-footnote-ref href="#user-content-fn-4">List</a><string>, callbacks: <a data-footnote-ref href="#user-content-fn-4">List</a><void>)</code></pre>
> Adds a list of buttons in a row to the popup.
> 
> **Parameters**:
> - `popupName`: The name of the popup.
> - `labels`: List of button display texts.
> - `callbacks`: List of callback names that will be passed to OnButtonClick in Main.
> 
<pre class="language-typescript"><code class="lang-typescript">function WrapStyleTag(text: string, style: string, arg: string = null) -> string</code></pre>
> Returns a wrapped string given style and args.
> 
> **Parameters**:
> - `text`: The text to wrap.
> - `style`: The style tag name.
> - `arg`: Optional style argument.
> 
<pre class="language-typescript"><code class="lang-typescript">function ShowChangeCharacterMenu()</code></pre>
> Shows the change character menu if main character is Human.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScoreboardHeader(header: string)</code></pre>
> Sets the display of the scoreboard header (default "Kills / Deaths...")
> 
> **Parameters**:
> - `header`: The header text to display.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetScoreboardProperty(property: string)</code></pre>
> Sets which Player custom property to read from to display on the scoreboard. If set to empty string, will use the default "Kills / Deaths..." display.
> 
> **Parameters**:
> - `property`: The property name to read from Player custom properties.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetThemeColor(panel: string, category: string, item: string) -> <a data-footnote-ref href="#user-content-fn-0">Color</a></code></pre>
> Gets the color of the specified item. See theme json for reference.
> 
> **Parameters**:
> - `panel`: The panel name.
> - `category`: The category name.
> - `item`: The item name.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPopupActive(popupName: string) -> bool</code></pre>
> Returns if the given popup is active
> 
> **Parameters**:
> - `popupName`: The name of the popup to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetLabelActive(label: string, active: bool)</code></pre>
> Sets whether a label is active or not.
> 
> **Parameters**:
> - `label`: The label name.
> - `active`: Whether the label should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKDRPanelActive(active: bool)</code></pre>
> Sets whether the KDR panel (top-left) is active or not.
> 
> **Parameters**:
> - `active`: Whether the KDR panel should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetMinimapActive(active: bool)</code></pre>
> Sets whether the minimap is active or not.
> 
> **Parameters**:
> - `active`: Whether the minimap should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetChatPanelActive(active: bool)</code></pre>
> Sets whether the chat panel is active or not.
> 
> **Parameters**:
> - `active`: Whether the chat panel should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetFeedPanelActive(active: bool)</code></pre>
> Sets whether the feed panel is active or not.
> 
> **Parameters**:
> - `active`: Whether the feed panel should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetBottomHUDActive(active: bool)</code></pre>
> Sets whether the bottom HUD is active or not. This can only be used when the character is alive.
> 
> **Parameters**:
> - `active`: Whether the bottom HUD should be active.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetRootVisualElement() -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Returns the root `VisualElement` which you can add other elements to. Returns: The root `VisualElement`
> 
<pre class="language-typescript"><code class="lang-typescript">function VisualElement() -> <a data-footnote-ref href="#user-content-fn-53">VisualElement</a></code></pre>
> Creates a new `VisualElement`.
> 
<pre class="language-typescript"><code class="lang-typescript">function Button(text: string = "", clickEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-42">Button</a></code></pre>
> Creates a new `Button` with optional text and click event.
> 
> **Parameters**:
> - `text`: The text that the button displays
> - `clickEvent`: The function that will be called when button is clicked
> 
<pre class="language-typescript"><code class="lang-typescript">function Label(text: string = "") -> <a data-footnote-ref href="#user-content-fn-46">Label</a></code></pre>
> Creates a new `Label` with optional text.
> 
> **Parameters**:
> - `text`: The text to be displayed
> 
<pre class="language-typescript"><code class="lang-typescript">function TextField(label: string = "") -> <a data-footnote-ref href="#user-content-fn-50">TextField</a></code></pre>
> Creates a new `TextField` with optional label.
> 
> **Parameters**:
> - `label`: The label text displayed next to the TextField (default: empty).
> 
<pre class="language-typescript"><code class="lang-typescript">function Toggle(label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-51">Toggle</a></code></pre>
> Creates a new `Toggle` with optional label and value changed event.
> 
> **Parameters**:
> - `label`: The label text displayed next to the toggle
> - `valueChangedEvent`: The function that will be called when toggle value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function Slider(lowValue: float = 0, highValue: float = 100, tickInterval: float = 0, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-49">Slider</a></code></pre>
> Creates a new `Slider` for floating-point values with optional range, tick interval, and value changed event. The slider will snap to values at multiples of the tick interval.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the slider
> - `highValue`: The maximum value of the slider
> - `tickInterval`: The interval between allowed values. If 0, no snapping occurs. For example, 0.1 will snap to 0.0, 0.1, 0.2, etc.
> - `label`: The label text displayed next to the slider
> - `valueChangedEvent`: The function that will be called when slider value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function SliderInt(lowValue: int = 0, highValue: int = 100, tickInterval: int = 1, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-49">Slider</a></code></pre>
> Creates a new `Slider` for integer values with optional range, tick interval, and value changed event. The slider will snap to values at multiples of the tick interval.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the slider
> - `highValue`: The maximum value of the slider
> - `tickInterval`: The interval between allowed values. For example, 5 will snap to 0, 5, 10, 15, etc.
> - `label`: The label text displayed next to the slider
> - `valueChangedEvent`: The function that will be called when slider value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function Dropdown(choices: <a data-footnote-ref href="#user-content-fn-4">List</a><string>, defaultIndex: int = 0, label: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-43">Dropdown</a></code></pre>
> Creates a new `Dropdown` with a list of choices and optional label and value changed event.
> 
> **Parameters**:
> - `choices`: List of string options to display in the dropdown
> - `defaultIndex`: The index of the initially selected option (default: 0)
> - `label`: The label text displayed next to the dropdown
> - `valueChangedEvent`: The function that will be called when dropdown value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function ProgressBar(lowValue: float = 0, highValue: float = 100, title: string = "", valueChangedEvent: function = null) -> <a data-footnote-ref href="#user-content-fn-47">ProgressBar</a></code></pre>
> Creates a new `ProgressBar` with optional range, title, and value changed event.
> 
> **Parameters**:
> - `lowValue`: The minimum value of the progress bar (default: 0)
> - `highValue`: The maximum value of the progress bar (default: 100)
> - `title`: The title text displayed on the progress bar
> - `valueChangedEvent`: The function that will be called when progress bar value changes
> 
<pre class="language-typescript"><code class="lang-typescript">function ScrollView() -> <a data-footnote-ref href="#user-content-fn-48">ScrollView</a></code></pre>
> Creates a new `ScrollView` for scrollable content.
> 
<pre class="language-typescript"><code class="lang-typescript">function Icon(iconPath: string = "") -> <a data-footnote-ref href="#user-content-fn-44">Icon</a></code></pre>
> Creates a new `Icon` element for displaying images/icons.
> 
> **Parameters**:
> - `iconPath`: Path to the icon resource (e.g., "Icons/Game/BladeIcon")
> 
<pre class="language-typescript"><code class="lang-typescript">function Image(iconPath: string = "") -> <a data-footnote-ref href="#user-content-fn-45">Image</a></code></pre>
> Creates a new `Image` element for displaying images/icons.
> 
> **Parameters**:
> - `iconPath`: Path to the icon resource (e.g., "Icons/Game/BladeIcon")
> 

[^0]: [Color](../objects/Color.md)
[^1]: [Dict](../objects/Dict.md)
[^2]: [LightBuiltin](../static/LightBuiltin.md)
[^3]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^4]: [List](../objects/List.md)
[^5]: [Quaternion](../objects/Quaternion.md)
[^6]: [Range](../objects/Range.md)
[^7]: [Set](../objects/Set.md)
[^8]: [Vector2](../objects/Vector2.md)
[^9]: [Vector3](../objects/Vector3.md)
[^10]: [Animation](../objects/Animation.md)
[^11]: [Animator](../objects/Animator.md)
[^12]: [AudioSource](../objects/AudioSource.md)
[^13]: [Collider](../objects/Collider.md)
[^14]: [Collision](../objects/Collision.md)
[^15]: [LineRenderer](../objects/LineRenderer.md)
[^16]: [LodBuiltin](../static/LodBuiltin.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^21]: [Character](../objects/Character.md)
[^22]: [Human](../objects/Human.md)
[^23]: [MapObject](../objects/MapObject.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [Player](../objects/Player.md)
[^26]: [Prefab](../objects/Prefab.md)
[^27]: [Shifter](../objects/Shifter.md)
[^28]: [Titan](../objects/Titan.md)
[^29]: [Transform](../objects/Transform.md)
[^30]: [WallColossal](../objects/WallColossal.md)
[^31]: [Camera](../static/Camera.md)
[^32]: [Cutscene](../static/Cutscene.md)
[^33]: [Game](../static/Game.md)
[^34]: [Input](../static/Input.md)
[^35]: [Locale](../static/Locale.md)
[^36]: [Map](../static/Map.md)
[^37]: [Network](../static/Network.md)
[^38]: [PersistentData](../static/PersistentData.md)
[^39]: [Physics](../static/Physics.md)
[^40]: [RoomData](../static/RoomData.md)
[^41]: [Time](../static/Time.md)
[^42]: [Button](../objects/Button.md)
[^43]: [Dropdown](../objects/Dropdown.md)
[^44]: [Icon](../objects/Icon.md)
[^45]: [Image](../objects/Image.md)
[^46]: [Label](../objects/Label.md)
[^47]: [ProgressBar](../objects/ProgressBar.md)
[^48]: [ScrollView](../objects/ScrollView.md)
[^49]: [Slider](../objects/Slider.md)
[^50]: [TextField](../objects/TextField.md)
[^51]: [Toggle](../objects/Toggle.md)
[^52]: [UI](../static/UI.md)
[^53]: [VisualElement](../objects/VisualElement.md)
[^54]: [Convert](../static/Convert.md)
[^55]: [Json](../static/Json.md)
[^56]: [Math](../static/Math.md)
[^57]: [Random](../objects/Random.md)
[^58]: [String](../static/String.md)
[^59]: [Object](../objects/Object.md)
[^60]: [Component](../objects/Component.md)
