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
<pre class="language-typescript"><code class="lang-typescript">function GetMouseAim() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Returns the position the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetCursorAimDirection() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Returns the ray the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMouseSpeed() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Returns the speed of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMousePosition() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Returns the position of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetScreenDimensions() -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
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
