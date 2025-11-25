# String
Inherits from [Object](../objects/Object.md)

String manipulation functions.

### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Newline|string|True|Returns the newline character.|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function FormatFloat(val: float, decimals: int) -> string</code></pre>
> Formats a float to a string with the specified number of decimal places.
> 
<pre class="language-typescript"><code class="lang-typescript">function FormatFromList(str: string, list: <a data-footnote-ref href="#user-content-fn-4">List</a>) -> string</code></pre>
> Equivalent to C# string.format(string, List<string>).
> 
<pre class="language-typescript"><code class="lang-typescript">function Split(toSplit: string, splitter: <a data-footnote-ref href="#user-content-fn-59">Object</a>, removeEmptyEntries: bool = False) -> <a data-footnote-ref href="#user-content-fn-4">List</a></code></pre>
> Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.
> 
<pre class="language-typescript"><code class="lang-typescript">function Join(list: <a data-footnote-ref href="#user-content-fn-4">List</a>, separator: string) -> string</code></pre>
> Joins a list of strings into a single string with the specified separator.
> 
<pre class="language-typescript"><code class="lang-typescript">function Substring(str: string, startIndex: int) -> string</code></pre>
> Returns a substring starting from the specified index.
> 
<pre class="language-typescript"><code class="lang-typescript">function SubstringWithLength(str: string, startIndex: int, length: int) -> string</code></pre>
> Returns a substring of the specified length starting from the specified start index.
> 
<pre class="language-typescript"><code class="lang-typescript">function Length(str: string) -> int</code></pre>
> Length of the string.
> 
<pre class="language-typescript"><code class="lang-typescript">function Replace(str: string, replace: string, with: string) -> string</code></pre>
> Replaces all occurrences of a substring with another substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(str: string, match: string) -> bool</code></pre>
> Checks if the string contains the specified substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function StartsWith(str: string, match: string) -> bool</code></pre>
> Checks if the string starts with the specified substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function EndsWith(str: string, match: string) -> bool</code></pre>
> Checks if the string ends with the specified substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function Trim(str: string) -> string</code></pre>
> Trims whitespace from the start and end of the string.
> 
<pre class="language-typescript"><code class="lang-typescript">function Insert(str: string, insert: string, index: int) -> string</code></pre>
> Inserts a substring at the specified index.
> 
<pre class="language-typescript"><code class="lang-typescript">function Capitalize(str: string) -> string</code></pre>
> Capitalizes the first letter of the string.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToUpper(str: string) -> string</code></pre>
> Converts the string to uppercase.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToLower(str: string) -> string</code></pre>
> Converts the string to lowercase.
> 
<pre class="language-typescript"><code class="lang-typescript">function IndexOf(str: string, substring: string) -> int</code></pre>
> Returns the index of the given string.
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
