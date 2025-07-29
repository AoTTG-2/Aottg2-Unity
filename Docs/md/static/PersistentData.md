# PersistentData
Inherits from [Object](../objects/Object.md)

Store and retrieve persistent data. Persistent data can be saved and loaded from file. Supports float, int, string, and bool types.
Note that any game mode may use the same file names, so it is recommended that you choose unique file names when saving and loading.
Saved files are located in Documents/Aottg2/PersistentData.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetProperty(property: string, value: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> null</code></pre>
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetProperty(property: string, defaultValue: <a data-footnote-ref href="#user-content-fn-37">Object</a>) -> <a data-footnote-ref href="#user-content-fn-37">Object</a></code></pre>
> Gets the property with given name. If property does not exist, returns defaultValue.
> 
<pre class="language-typescript"><code class="lang-typescript">function LoadFromFile(fileName: string, encrypted: bool) -> null</code></pre>
> Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.
> 
<pre class="language-typescript"><code class="lang-typescript">function SaveToFile(fileName: string, encrypted: bool) -> null</code></pre>
> Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.
> 
<pre class="language-typescript"><code class="lang-typescript">function Clear() -> null</code></pre>
> Clears current persistent data.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsValidFileName(fileName: string) -> bool</code></pre>
> Determines whether or not the given fileName will be allowed for use when saving/loading a file.
> 
<pre class="language-typescript"><code class="lang-typescript">function FileExists(fileName: string) -> bool</code></pre>
> Determines whether the file given already exists. Throws an error if given an invalid file name.
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
[^15]: [Map](../static/Map.md)
[^16]: [MapObject](../objects/MapObject.md)
[^17]: [MapTargetable](../objects/MapTargetable.md)
[^18]: [Math](../static/Math.md)
[^19]: [Network](../static/Network.md)
[^20]: [NetworkView](../objects/NetworkView.md)
[^21]: [PersistentData](../static/PersistentData.md)
[^22]: [Physics](../static/Physics.md)
[^23]: [Player](../objects/Player.md)
[^24]: [Quaternion](../objects/Quaternion.md)
[^25]: [Random](../objects/Random.md)
[^26]: [Range](../objects/Range.md)
[^27]: [RoomData](../static/RoomData.md)
[^28]: [Set](../objects/Set.md)
[^29]: [Shifter](../objects/Shifter.md)
[^30]: [String](../static/String.md)
[^31]: [Time](../static/Time.md)
[^32]: [Titan](../objects/Titan.md)
[^33]: [Transform](../objects/Transform.md)
[^34]: [UI](../static/UI.md)
[^35]: [Vector2](../objects/Vector2.md)
[^36]: [Vector3](../objects/Vector3.md)
[^37]: [Object](../objects/Object.md)
[^38]: [Component](../objects/Component.md)
