# Input
Inherits from [Object](../objects/Object.md)

Reading player key inputs. Note that inputs are best handled in OnFrame rather than OnTick, due to being updated every frame and not every physics tick.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKeyName(key: string) -> string</code></pre>
> Gets the key name the player assigned to the key setting
> 
<pre class="language-typescript"><code class="lang-typescript">function GetKeyHold(key: string) -> bool</code></pre>
> Returns true if the key is being held down
> 
<pre class="language-typescript"><code class="lang-typescript">function GetKeyDown(key: string) -> bool</code></pre>
> Returns true if the key was pressed down this frame
> 
<pre class="language-typescript"><code class="lang-typescript">function GetKeyUp(key: string) -> bool</code></pre>
> Returns true if the key was released this frame
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMouseAim() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the position the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetCursorAimDirection() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the ray the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMouseSpeed() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the speed of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMousePosition() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the position of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetScreenDimensions() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns the dimensions of the screen
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKeyDefaultEnabled(key: string, enabled: bool)</code></pre>
> Sets whether the key is enabled by default
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKeyHold(key: string, enabled: bool)</code></pre>
> Sets whether the key is being held down
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCategoryKeysEnabled(category: string, enabled: bool)</code></pre>
> Sets whether all keys in the specified category are enabled by default. Valid categories: General, Human, Titan, Interaction
> 
<pre class="language-typescript"><code class="lang-typescript">function SetGeneralKeysEnabled(enabled: bool)</code></pre>
> Sets whether all General category keys are enabled by default
> 
<pre class="language-typescript"><code class="lang-typescript">function SetInteractionKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Interaction category keys are enabled by default
> 
<pre class="language-typescript"><code class="lang-typescript">function SetTitanKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Titan category keys are enabled by default
> 
<pre class="language-typescript"><code class="lang-typescript">function SetHumanKeysEnabled(enabled: bool)</code></pre>
> Sets whether all Human category keys are enabled by default
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
