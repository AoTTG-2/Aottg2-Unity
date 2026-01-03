# Convert
Inherits from [Object](../objects/Object.md)

Converting objects to different types.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function ToFloat(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> float</code></pre>
> Converts a value to a float
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
<pre class="language-typescript"><code class="lang-typescript">function ToInt(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> int</code></pre>
> Converts a value to an int
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
<pre class="language-typescript"><code class="lang-typescript">function ToBool(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Converts a value to a bool
> 
> **Parameters**:
> - `value`: The value to convert (can be string, float, int, or bool).
> 
<pre class="language-typescript"><code class="lang-typescript">function ToString(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> string</code></pre>
> Converts a value to a string
> 
> **Parameters**:
> - `value`: The value to convert.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsFloat(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is a float
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsInt(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is an int
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsBool(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is a bool
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsString(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is a string
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsObject(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is an object
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsList(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is a list
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsDict(value: <a data-footnote-ref href="#user-content-fn-82">Object</a>) -> bool</code></pre>
> Checks if the value is a dictionary
> 
> **Parameters**:
> - `value`: The value to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function HasVariable(cInstance: class, variableName: string) -> bool</code></pre>
> Checks if the class instance has a variable
> 
> **Parameters**:
> - `cInstance`: The class instance to check.
> - `variableName`: The name of the variable to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function DefineVariable(cInstance: class, variableName: string, value: <a data-footnote-ref href="#user-content-fn-82">Object</a>)</code></pre>
> Defines a variable for the class instance
> 
> **Parameters**:
> - `cInstance`: The class instance to define the variable on.
> - `variableName`: The name of the variable to define.
> - `value`: The value to assign to the variable.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveVariable(cInstance: class, variableName: string)</code></pre>
> Removes a variable from the class instance
> 
> **Parameters**:
> - `cInstance`: The class instance to remove the variable from.
> - `variableName`: The name of the variable to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function HasMethod(cInstance: class, methodName: string) -> bool</code></pre>
> Checks if the class instance has a method
> 
> **Parameters**:
> - `cInstance`: The class instance to check.
> - `methodName`: The name of the method to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetType(cInstance: class) -> string</code></pre>
> Gets the type of the class instance
> 
> **Parameters**:
> - `cInstance`: The class instance to get the type of.
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
