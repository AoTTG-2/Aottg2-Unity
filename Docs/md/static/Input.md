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
<pre class="language-typescript"><code class="lang-typescript">function GetMouseAim() -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns the position the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetCursorAimDirection() -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns the ray the player is aiming at
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMouseSpeed() -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns the speed of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetMousePosition() -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns the position of the mouse
> 
<pre class="language-typescript"><code class="lang-typescript">function GetScreenDimensions() -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns the dimensions of the screen
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKeyDefaultEnabled(key: string, enabled: bool)</code></pre>
> Sets whether the key is enabled by default
> 
<pre class="language-typescript"><code class="lang-typescript">function SetKeyHold(key: string, enabled: bool)</code></pre>
> Sets whether the key is being held down
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
