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
<pre class="language-typescript"><code class="lang-typescript">function FormatFromList(str: string, list: <a data-footnote-ref href="#user-content-fn-14">List</a>) -> string</code></pre>
> Equivalent to C# string.format(string, List<string>).
> 
<pre class="language-typescript"><code class="lang-typescript">function Split(toSplit: string, splitter: <a data-footnote-ref href="#user-content-fn-38">Object</a>, removeEmptyEntries: bool = False) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.
> 
<pre class="language-typescript"><code class="lang-typescript">function Join(list: <a data-footnote-ref href="#user-content-fn-14">List</a>, separator: string) -> string</code></pre>
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
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
