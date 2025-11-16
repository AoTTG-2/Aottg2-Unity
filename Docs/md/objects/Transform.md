# Transform
Inherits from [Object](../objects/Object.md)

Represents a transform.

### Remarks
Overloads operators: 
`==`, `__Hash__`
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
|Parent|[Transform](../objects/Transform.md)|False|Sets the parent of the transform|
|Name|string|True|Gets the name of the transform.|
|Layer|int|False|Gets the Physics Layer of the transform.|
|Animation|[Animation](../objects/Animation.md)|True|The Animation attached to this transform, returns null if there is none.|
|Animator|[Animator](../objects/Animator.md)|True|The Animator attached to this transform, returns null if there is none.|
|AudioSource|[AudioSource](../objects/AudioSource.md)|True|The AudioSource attached to this transform, returns null if there is none.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-43">Transform</a></code></pre>
> Gets the transform of the specified child.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTransforms() -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Gets all child transforms.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingAnimation(anim: string) -> bool</code></pre>
> Checks if the given animation is playing.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(anim: string, fade: float = 0.1)</code></pre>
> Plays the specified animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(anim: string) -> float</code></pre>
> Gets the length of the specified animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlaySound()</code></pre>
> Plays the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound()</code></pre>
> Stops the sound.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToggleParticle(enabled: bool)</code></pre>
> Toggles the particle system.
> 
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformDirection(direction: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function InverseTransformPoint(point: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function TransformDirection(direction: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function TransformPoint(point: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
<pre class="language-typescript"><code class="lang-typescript">function Rotate(rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function RotateAround(point: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, axis: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, angle: float)</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function LookAt(target: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>)</code></pre>
<pre class="language-typescript"><code class="lang-typescript">function SetRenderersEnabled(enabled: bool)</code></pre>
> Sets the enabled state of all child renderers.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetColliders(recursive: bool = False) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Gets colliders of the transform.
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
