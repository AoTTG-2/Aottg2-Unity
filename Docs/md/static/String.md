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
> **Parameters**:
> - `val`: The float value to format.
> - `decimals`: The number of decimal places.
> 
<pre class="language-typescript"><code class="lang-typescript">function FormatFromList(str: string, list: <a data-footnote-ref href="#user-content-fn-4">List</a><string>) -> string</code></pre>
> Equivalent to C# string.format(string, List<string>).
> 
> **Parameters**:
> - `str`: The format string.
> - `list`: The list of values to format.
> 
<pre class="language-typescript"><code class="lang-typescript">function Split(toSplit: string, splitter: string|List<string>, removeEmptyEntries: bool = False) -> <a data-footnote-ref href="#user-content-fn-4">List</a><string></code></pre>
> Split the string into a list. Can pass in either a string to split on or a list of strings to split on, the last optional param can remove all empty entries.
> 
> **Parameters**:
> - `toSplit`: The string to split.
> - `splitter`: The separator string or list of separator strings.
> - `removeEmptyEntries`: Whether to remove empty entries from the result.
> 
<pre class="language-typescript"><code class="lang-typescript">function Join(list: <a data-footnote-ref href="#user-content-fn-4">List</a><string>, separator: string) -> string</code></pre>
> Joins a list of strings into a single string with the specified separator.
> 
> **Parameters**:
> - `list`: The list of strings to join.
> - `separator`: The separator string.
> 
<pre class="language-typescript"><code class="lang-typescript">function Substring(str: string, startIndex: int) -> string</code></pre>
> Returns a substring starting from the specified index.
> 
> **Parameters**:
> - `str`: The string to get a substring from.
> - `startIndex`: The starting index.
> 
<pre class="language-typescript"><code class="lang-typescript">function SubstringWithLength(str: string, startIndex: int, length: int) -> string</code></pre>
> Returns a substring of the specified length starting from the specified start index.
> 
> **Parameters**:
> - `str`: The string to get a substring from.
> - `startIndex`: The starting index.
> - `length`: The length of the substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function Length(str: string) -> int</code></pre>
> Length of the string.
> 
> **Parameters**:
> - `str`: The string to get the length of.
> 
<pre class="language-typescript"><code class="lang-typescript">function Replace(str: string, replace: string, with: string) -> string</code></pre>
> Replaces all occurrences of a substring with another substring.
> 
> **Parameters**:
> - `str`: The string to perform replacement on.
> - `replace`: The substring to replace.
> - `with`: The replacement substring.
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(str: string, match: string) -> bool</code></pre>
> Checks if the string contains the specified substring.
> 
> **Parameters**:
> - `str`: The string to check.
> - `match`: The substring to search for.
> 
<pre class="language-typescript"><code class="lang-typescript">function StartsWith(str: string, match: string) -> bool</code></pre>
> Checks if the string starts with the specified substring.
> 
> **Parameters**:
> - `str`: The string to check.
> - `match`: The substring to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function EndsWith(str: string, match: string) -> bool</code></pre>
> Checks if the string ends with the specified substring.
> 
> **Parameters**:
> - `str`: The string to check.
> - `match`: The substring to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function Trim(str: string) -> string</code></pre>
> Trims whitespace from the start and end of the string.
> 
> **Parameters**:
> - `str`: The string to trim.
> 
<pre class="language-typescript"><code class="lang-typescript">function Insert(str: string, insert: string, index: int) -> string</code></pre>
> Inserts a substring at the specified index.
> 
> **Parameters**:
> - `str`: The string to insert into.
> - `insert`: The substring to insert.
> - `index`: The index to insert at.
> 
<pre class="language-typescript"><code class="lang-typescript">function Capitalize(str: string) -> string</code></pre>
> Capitalizes the first letter of the string.
> 
> **Parameters**:
> - `str`: The string to capitalize.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToUpper(str: string) -> string</code></pre>
> Converts the string to uppercase.
> 
> **Parameters**:
> - `str`: The string to convert.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToLower(str: string) -> string</code></pre>
> Converts the string to lowercase.
> 
> **Parameters**:
> - `str`: The string to convert.
> 
<pre class="language-typescript"><code class="lang-typescript">function IndexOf(str: string, substring: string) -> int</code></pre>
> Returns the index of the given string.
> 
> **Parameters**:
> - `str`: The string to search in.
> - `substring`: The substring to find.
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
[^31]: [CharacterTypeEnum](../static/CharacterTypeEnum.md)
[^32]: [CollideModeEnum](../static/CollideModeEnum.md)
[^33]: [CollideWithEnum](../static/CollideWithEnum.md)
[^34]: [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)
[^35]: [EffectNameEnum](../static/EffectNameEnum.md)
[^36]: [ForceModeEnum](../static/ForceModeEnum.md)
[^37]: [HandStateEnum](../static/HandStateEnum.md)
[^38]: [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
[^39]: [InputCategoryEnum](../static/InputCategoryEnum.md)
[^40]: [LanguageEnum](../static/LanguageEnum.md)
[^41]: [LoadoutEnum](../static/LoadoutEnum.md)
[^42]: [OutlineModeEnum](../static/OutlineModeEnum.md)
[^43]: [PhysicMaterialCombineEnum](../static/PhysicMaterialCombineEnum.md)
[^44]: [PlayerStatusEnum](../static/PlayerStatusEnum.md)
[^45]: [ProjectileNameEnum](../static/ProjectileNameEnum.md)
[^46]: [ScaleModeEnum](../static/ScaleModeEnum.md)
[^47]: [ShifterTypeEnum](../static/ShifterTypeEnum.md)
[^48]: [SliderDirectionEnum](../static/SliderDirectionEnum.md)
[^49]: [SteamStateEnum](../static/SteamStateEnum.md)
[^50]: [TeamEnum](../static/TeamEnum.md)
[^51]: [TitanTypeEnum](../static/TitanTypeEnum.md)
[^52]: [TSKillSoundEnum](../static/TSKillSoundEnum.md)
[^53]: [WeaponEnum](../static/WeaponEnum.md)
[^54]: [Camera](../static/Camera.md)
[^55]: [Cutscene](../static/Cutscene.md)
[^56]: [Game](../static/Game.md)
[^57]: [Input](../static/Input.md)
[^58]: [Locale](../static/Locale.md)
[^59]: [Map](../static/Map.md)
[^60]: [Network](../static/Network.md)
[^61]: [PersistentData](../static/PersistentData.md)
[^62]: [Physics](../static/Physics.md)
[^63]: [RoomData](../static/RoomData.md)
[^64]: [Time](../static/Time.md)
[^65]: [Button](../objects/Button.md)
[^66]: [Dropdown](../objects/Dropdown.md)
[^67]: [Icon](../objects/Icon.md)
[^68]: [Image](../objects/Image.md)
[^69]: [Label](../objects/Label.md)
[^70]: [ProgressBar](../objects/ProgressBar.md)
[^71]: [ScrollView](../objects/ScrollView.md)
[^72]: [Slider](../objects/Slider.md)
[^73]: [TextField](../objects/TextField.md)
[^74]: [Toggle](../objects/Toggle.md)
[^75]: [UI](../static/UI.md)
[^76]: [VisualElement](../objects/VisualElement.md)
[^77]: [Convert](../static/Convert.md)
[^78]: [Json](../static/Json.md)
[^79]: [Math](../static/Math.md)
[^80]: [Random](../objects/Random.md)
[^81]: [String](../static/String.md)
[^82]: [Object](../objects/Object.md)
[^83]: [Component](../objects/Component.md)
