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
<pre class="language-typescript"><code class="lang-typescript">function AddPopupButtons(popupName: string, labels: <a data-footnote-ref href="#user-content-fn-15">List</a>, callbacks: <a data-footnote-ref href="#user-content-fn-15">List</a>)</code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function GetThemeColor(panel: string, category: string, item: string) -> <a data-footnote-ref href="#user-content-fn-4">Color</a></code></pre>
> Gets the color of the specified item. See theme json for reference.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPopupActive(popupName: string) -> bool</code></pre>
> Returns if the given popup is active
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
