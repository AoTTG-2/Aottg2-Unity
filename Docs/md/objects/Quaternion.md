# Quaternion
Inherits from [Object](../objects/Object.md)

Represents a quaternion.

### Remarks
Overloads operators: 
`__Copy__`, `*`, `==`, `__Hash__`
### Initialization
```csharp
Quaternion() // Default constructor, creates an identity quaternion (no rotation).
Quaternion(x: float, y: float, z: float, w: float) // Creates a new Quaternion from the given values.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|X|float|False|The X component of the quaternion.|
|Y|float|False|The Y component of the quaternion.|
|Z|float|False|The Z component of the quaternion.|
|W|float|False|The W component of the quaternion.|
|Euler|[Vector3](../objects/Vector3.md)|False|Returns or sets the euler angle representation of the rotation.|


### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Identity|[Quaternion](../objects/Quaternion.md)|True|The identity rotation (no rotation).|


### Methods

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Linearly interpolates between two rotations.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Linearly interpolates between two rotations without clamping.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (not clamped).
> 
<pre class="language-typescript"><code class="lang-typescript">function Slerp(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Spherically interpolates between two rotations.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
<pre class="language-typescript"><code class="lang-typescript">function SlerpUnclamped(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, t: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Spherically interpolates between two rotations without clamping.
> 
> **Parameters**:
> - `a`: The start rotation.
> - `b`: The end rotation.
> - `t`: The interpolation factor (not clamped).
> 
<pre class="language-typescript"><code class="lang-typescript">function FromEuler(euler: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation from euler angles.
> 
> **Parameters**:
> - `euler`: The euler angles in degrees (x, y, z).
> 
<pre class="language-typescript"><code class="lang-typescript">function LookRotation(forward: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, upwards: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation that looks in the specified direction.
> 
> **Parameters**:
> - `forward`: The forward direction vector.
> - `upwards`: Optional. The upwards direction vector (default: Vector3.up).
> 
<pre class="language-typescript"><code class="lang-typescript">function FromToRotation(a: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, b: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation from one direction to another.
> 
> **Parameters**:
> - `a`: The source direction vector.
> - `b`: The target direction vector.
> 
<pre class="language-typescript"><code class="lang-typescript">function Inverse(q: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Returns the inverse of a quaternion.
> 
> **Parameters**:
> - `q`: The quaternion to invert.
> 
<pre class="language-typescript"><code class="lang-typescript">function RotateTowards(from: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, to: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, maxDegreesDelta: float) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Rotates a rotation towards a target rotation.
> 
> **Parameters**:
> - `from`: The current rotation.
> - `to`: The target rotation.
> - `maxDegreesDelta`: The maximum change in degrees.
> 
<pre class="language-typescript"><code class="lang-typescript">function AngleAxis(angle: float, axis: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-5">Quaternion</a></code></pre>
> Creates a rotation that rotates around a specified axis by a specified angle.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> - `axis`: The axis to rotate around.
> 
<pre class="language-typescript"><code class="lang-typescript">function Angle(a: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, b: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> float</code></pre>
> Calculates the angle between two rotations.
> 
> **Parameters**:
> - `a`: The first rotation.
> - `b`: The second rotation.
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
