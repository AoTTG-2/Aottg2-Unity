# PersistentData
Inherits from [Object](../objects/Object.md)

Store and retrieve persistent data. Persistent data can be saved and loaded from file. Supports float, int, string, and bool types.
Note that any game mode may use the same file names, so it is recommended that you choose unique file names when saving and loading.
Saved files are located in Documents/Aottg2/PersistentData.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function SetProperty(property: string, value: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Sets the property with given name to the object value. Valid value types are float, string, bool, and int.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetProperty(property: string, defaultValue: <a data-footnote-ref href="#user-content-fn-59">Object</a>) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Gets the property with given name. If property does not exist, returns defaultValue.
> 
<pre class="language-typescript"><code class="lang-typescript">function LoadFromFile(fileName: string, encrypted: bool)</code></pre>
> Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.
> 
<pre class="language-typescript"><code class="lang-typescript">function SaveToFile(fileName: string, encrypted: bool)</code></pre>
> Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.
> 
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clears current persistent data.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsValidFileName(fileName: string) -> bool</code></pre>
> Determines whether or not the given fileName will be allowed for use when saving/loading a file.
> 
<pre class="language-typescript"><code class="lang-typescript">function FileExists(fileName: string) -> bool</code></pre>
> Determines whether the file given already exists. Throws an error if given an invalid file name.
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
