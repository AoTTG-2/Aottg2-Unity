# Transform
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
- `==`
- `__Hash__`
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Position|[Vector3](../objects/Vector3.md)|False|Gets or sets the position of the transform.|
|LocalPosition|[Vector3](../objects/Vector3.md)|False|Gets or sets the local position of the transform.|
|Rotation|[Vector3](../objects/Vector3.md)|False|Gets or sets the rotation of the transform.|
|LocalRotation|[Vector3](../objects/Vector3.md)|False|Gets or sets the local rotation of the transform.|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|Gets or sets the quaternion rotation of the transform.|
|QuaternionLocalRotation|[Quaternion](../objects/Quaternion.md)|False|Gets or sets the quaternion local rotation of the transform.|
|Scale|[Vector3](../objects/Vector3.md)|False|Gets or sets the scale of the transform.|
|Forward|[Vector3](../objects/Vector3.md)|False|Gets the forward vector of the transform.|
|Up|[Vector3](../objects/Vector3.md)|False|Gets the up vector of the transform.|
|Right|[Vector3](../objects/Vector3.md)|False|Gets the right vector of the transform.|
|Name|string|True|Gets the name of the transform.|
|Layer|int|False|Gets the Physics Layer of the transform.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-33">Transform</a></code></pre>
> Gets the transform of the specified child.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTransforms() -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Gets all child transforms.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(anim: string, fade: float = 0.1) -> null</code></pre>
> Plays the specified animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationAt(anim: string, normalizedTime: float, fade: float = 0.1) -> null</code></pre>
> Plays the specified animation starting from a normalized time.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationSpeed(speed: float) -> null</code></pre>
> Sets the animation playback speed
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(anim: string) -> float</code></pre>
> Gets the length of the specified animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlaySound() -> null</code></pre>
> Plays the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound() -> null</code></pre>
> Stops the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToggleParticle(enabled: bool) -> null</code></pre>
> Toggles the particle system.
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformDirection(direction: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector3</a></code></pre>
> Transforms a direction from world space to local space. The opposite of Transform.TransformDirection.
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformPoint(point: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector3</a></code></pre>
> Transforms position from world space to local space.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformDirection(direction: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector3</a></code></pre>
> Transforms direction from local space to world space.
> 
<pre class="language-typescript"><code class="lang-typescript">function TransformPoint(point: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-36">Vector3</a></code></pre>
> Transforms position from local space to world space.
> 
<pre class="language-typescript"><code class="lang-typescript">function Rotate(rotation: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> null</code></pre>
> Applies a rotation of eulerAngles.z degrees around the z-axis, eulerAngles.x degrees around the x-axis, and eulerAngles.y degrees around the y-axis (in that order).
> 
<pre class="language-typescript"><code class="lang-typescript">function RotateAround(point: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>, angle: float) -> null</code></pre>
> Rotates the transform about axis passing through point in world coordinates by angle degrees.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(target: <a data-footnote-ref href="#user-content-fn-36">Vector3</a>) -> null</code></pre>
> Rotates the transform so the forward vector points at worldPosition.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetRenderersEnabled(enabled: bool) -> null</code></pre>
> Sets the enabled state of all child renderers.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetColliders(recursive: bool = False) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Gets colliders of the transform.
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
