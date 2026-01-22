# Math
Inherits from [Object](../objects/Object.md)
### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|PI|float|True|The value of PI.|
|Infinity|float|True|The value of Infinity.|
|NegativeInfinity|float|True|The value of Negative Infinity.|
|Rad2DegConstant|float|True|The value of Rad2Deg constant.|
|Deg2RadConstant|float|True|The value of Deg2Rad constant.|
|Epsilon|float|True|The value of Epsilon.|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function Clamp(value: <a data-footnote-ref href="#user-content-fn-116">Object</a>, min: <a data-footnote-ref href="#user-content-fn-116">Object</a>, max: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Clamp a value between a minimum and maximum value.
> 
> **Parameters**:
> - `value`: The value to clamp. Can be int or float.
> - `min`: The minimum value. Can be int or float.
> - `max`: The maximum value. Can be int or float.
> 
> **Returns**: The clamped value. Will be the same type as the inputs.
<pre class="language-typescript"><code class="lang-typescript">function Max(a: <a data-footnote-ref href="#user-content-fn-116">Object</a>, b: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get the maximum of two values.
> 
> **Parameters**:
> - `a`: The first value. Can be int or float.
> - `b`: The second value. Can be int or float.
> 
> **Returns**: The maximum of the two values. Will be the same type as the inputs.
<pre class="language-typescript"><code class="lang-typescript">function Min(a: <a data-footnote-ref href="#user-content-fn-116">Object</a>, b: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get the minimum of two values.
> 
> **Parameters**:
> - `a`: The first value. Can be int or float.
> - `b`: The second value. Can be int or float.
> 
> **Returns**: The minimum of the two values. Will be the same type as the inputs.
<pre class="language-typescript"><code class="lang-typescript">function Pow(a: float, b: float) -> float</code></pre>
> Raise a value to the power of another value.
> 
> **Parameters**:
> - `a`: The base value.
> - `b`: The exponent value.
> 
> **Returns**: The result of raising a to the power of b.
<pre class="language-typescript"><code class="lang-typescript">function Abs(value: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Get the absolute value of a number.
> 
> **Parameters**:
> - `value`: The number. Can be int or float.
> 
> **Returns**: The absolute value. Will be the same type as the input.
<pre class="language-typescript"><code class="lang-typescript">function Sqrt(value: float) -> float</code></pre>
> Get the square root of a number.
> 
> **Parameters**:
> - `value`: The value to get the square root of.
> 
> **Returns**: The square root of the value.
<pre class="language-typescript"><code class="lang-typescript">function Repeat(value: <a data-footnote-ref href="#user-content-fn-116">Object</a>, max: <a data-footnote-ref href="#user-content-fn-116">Object</a>) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Modulo for floats.
> 
> **Parameters**:
> - `value`: The value to repeat.
> - `max`: The maximum value to repeat to.
> 
> **Returns**: The repeated value.
<pre class="language-typescript"><code class="lang-typescript">function Mod(a: int, b: int) -> int</code></pre>
> Get the remainder of a division operation.
> 
> **Parameters**:
> - `a`: The dividend.
> - `b`: The divisor.
> 
> **Returns**: The remainder of the division.
<pre class="language-typescript"><code class="lang-typescript">function Sin(angle: float) -> float</code></pre>
> Get the sine of an angle.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> 
> **Returns**: Value between -1 and 1.
<pre class="language-typescript"><code class="lang-typescript">function Cos(angle: float) -> float</code></pre>
> Get the cosine of an angle.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> 
> **Returns**: Value between -1 and 1.
<pre class="language-typescript"><code class="lang-typescript">function Tan(angle: float) -> float</code></pre>
> Get the tangent of an angle in radians.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> 
> **Returns**: The tangent of the angle.
<pre class="language-typescript"><code class="lang-typescript">function Asin(value: float) -> float</code></pre>
> Get the arcsine of a value in degrees.
> 
> **Parameters**:
> - `value`: The value (must be between -1 and 1).
> 
> **Returns**: The arcsine in degrees.
<pre class="language-typescript"><code class="lang-typescript">function Acos(value: float) -> float</code></pre>
> Get the arccosine of a value in degrees.
> 
> **Parameters**:
> - `value`: The value (must be between -1 and 1).
> 
> **Returns**: The arccosine in degrees.
<pre class="language-typescript"><code class="lang-typescript">function Atan(value: float) -> float</code></pre>
> Get the arctangent of a value in degrees.
> 
> **Parameters**:
> - `value`: The value to get the arctangent of.
> 
> **Returns**: The arctangent in degrees.
<pre class="language-typescript"><code class="lang-typescript">function Atan2(a: float, b: float) -> float</code></pre>
> Get the arctangent of a value in degrees.
> 
> **Parameters**:
> - `a`: The Y component.
> - `b`: The X component.
> 
> **Returns**: The arctangent in degrees.
<pre class="language-typescript"><code class="lang-typescript">function Ceil(value: float) -> int</code></pre>
> Get the smallest integer greater than or equal to a value.
> 
> **Parameters**:
> - `value`: The value to round up.
> 
> **Returns**: The smallest integer greater than or equal to the value.
<pre class="language-typescript"><code class="lang-typescript">function Floor(value: float) -> int</code></pre>
> Get the largest integer less than or equal to a value.
> 
> **Parameters**:
> - `value`: The value to round down.
> 
> **Returns**: The largest integer less than or equal to the value.
<pre class="language-typescript"><code class="lang-typescript">function Round(value: float) -> int</code></pre>
> Round a value to the nearest integer.
> 
> **Parameters**:
> - `value`: The value to round.
> 
> **Returns**: The rounded integer value.
<pre class="language-typescript"><code class="lang-typescript">function Deg2Rad(angle: float) -> float</code></pre>
> Convert an angle from degrees to radians.
> 
> **Parameters**:
> - `angle`: The angle in degrees.
> 
> **Returns**: The angle in radians.
<pre class="language-typescript"><code class="lang-typescript">function Rad2Deg(angle: float) -> float</code></pre>
> Convert an angle from radians to degrees.
> 
> **Parameters**:
> - `angle`: The angle in radians.
> 
> **Returns**: The angle in degrees.
<pre class="language-typescript"><code class="lang-typescript">function Lerp(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two values.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
> **Returns**: The interpolated value.
<pre class="language-typescript"><code class="lang-typescript">function LerpUnclamped(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two values without clamping.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (not clamped).
> 
> **Returns**: The interpolated value.
<pre class="language-typescript"><code class="lang-typescript">function Sign(value: float) -> float</code></pre>
> Get the sign of a value.
> 
> **Parameters**:
> - `value`: The value to get the sign of.
> 
> **Returns**: The sign of the value (-1, 0, or 1).
<pre class="language-typescript"><code class="lang-typescript">function InverseLerp(a: float, b: float, value: float) -> float</code></pre>
> Get the inverse lerp of two values.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `value`: The value to find the interpolation factor for.
> 
> **Returns**: The interpolation factor.
<pre class="language-typescript"><code class="lang-typescript">function LerpAngle(a: float, b: float, t: float) -> float</code></pre>
> Linearly interpolate between two angles.
> 
> **Parameters**:
> - `a`: The start angle in degrees.
> - `b`: The end angle in degrees.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
> **Returns**: The interpolated angle.
<pre class="language-typescript"><code class="lang-typescript">function Log(value: float) -> float</code></pre>
> Get the natural logarithm of a value.
> 
> **Parameters**:
> - `value`: The value to get the logarithm of.
> 
> **Returns**: The natural logarithm of the value.
<pre class="language-typescript"><code class="lang-typescript">function MoveTowards(current: float, target: float, maxDelta: float) -> float</code></pre>
> Move a value towards a target value.
> 
> **Parameters**:
> - `current`: The current value.
> - `target`: The target value.
> - `maxDelta`: The maximum change allowed.
> 
> **Returns**: The new value moved towards the target.
<pre class="language-typescript"><code class="lang-typescript">function MoveTowardsAngle(current: float, target: float, maxDelta: float) -> float</code></pre>
> Move an angle towards a target angle.
> 
> **Parameters**:
> - `current`: The current angle in degrees.
> - `target`: The target angle in degrees.
> - `maxDelta`: The maximum change in degrees allowed.
> 
> **Returns**: The new angle moved towards the target.
<pre class="language-typescript"><code class="lang-typescript">function PingPong(t: float, length: float) -> float</code></pre>
> Get the ping pong value of a time value.
> 
> **Parameters**:
> - `t`: The time value.
> - `length`: The length of the ping pong cycle.
> 
> **Returns**: The ping pong value.
<pre class="language-typescript"><code class="lang-typescript">function Exp(value: float) -> float</code></pre>
> Get the exponential value of a number.
> 
> **Parameters**:
> - `value`: The value to exponentiate.
> 
> **Returns**: The exponential value.
<pre class="language-typescript"><code class="lang-typescript">function SmoothStep(a: float, b: float, t: float) -> float</code></pre>
> Smoothly step between two values.
> 
> **Parameters**:
> - `a`: The start value.
> - `b`: The end value.
> - `t`: The interpolation factor (clamped between 0 and 1).
> 
> **Returns**: The smoothly interpolated value.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseAnd(a: int, b: int) -> int</code></pre>
> Perform a bitwise AND operation.
> 
> **Parameters**:
> - `a`: The first integer.
> - `b`: The second integer.
> 
> **Returns**: The result of the bitwise AND operation.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseOr(a: int, b: int) -> int</code></pre>
> Perform a bitwise OR operation.
> 
> **Parameters**:
> - `a`: The first integer.
> - `b`: The second integer.
> 
> **Returns**: The result of the bitwise OR operation.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseXor(a: int, b: int) -> int</code></pre>
> Perform a bitwise XOR operation.
> 
> **Parameters**:
> - `a`: The first integer.
> - `b`: The second integer.
> 
> **Returns**: The result of the bitwise XOR operation.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseNot(value: int) -> int</code></pre>
> Perform a bitwise NOT operation.
> 
> **Parameters**:
> - `value`: The integer to negate.
> 
> **Returns**: The result of the bitwise NOT operation.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseLeftShift(value: int, shift: int) -> int</code></pre>
> Shift bits to the left.
> 
> **Parameters**:
> - `value`: The integer to shift.
> - `shift`: The number of bits to shift left.
> 
> **Returns**: The result of the left shift operation.
<pre class="language-typescript"><code class="lang-typescript">function BitwiseRightShift(value: int, shift: int) -> int</code></pre>
> Shift bits to the right.
> 
> **Parameters**:
> - `value`: The integer to shift.
> - `shift`: The number of bits to shift right.
> 
> **Returns**: The result of the right shift operation.

[^0]: [Color](../Collections/Color.md)
[^1]: [Dict](../Collections/Dict.md)
[^2]: [LightBuiltin](../Collections/LightBuiltin.md)
[^3]: [LineCastHitResult](../Collections/LineCastHitResult.md)
[^4]: [List](../Collections/List.md)
[^5]: [Quaternion](../Collections/Quaternion.md)
[^6]: [Range](../Collections/Range.md)
[^7]: [Set](../Collections/Set.md)
[^8]: [Vector2](../Collections/Vector2.md)
[^9]: [Vector3](../Collections/Vector3.md)
[^10]: [Animation](../Component/Animation.md)
[^11]: [Animator](../Component/Animator.md)
[^12]: [AudioSource](../Component/AudioSource.md)
[^13]: [Collider](../Component/Collider.md)
[^14]: [Collision](../Component/Collision.md)
[^15]: [LineRenderer](../Component/LineRenderer.md)
[^16]: [LodBuiltin](../Component/LodBuiltin.md)
[^17]: [MapTargetable](../Component/MapTargetable.md)
[^18]: [NavmeshObstacleBuiltin](../Component/NavmeshObstacleBuiltin.md)
[^19]: [PhysicsMaterialBuiltin](../Component/PhysicsMaterialBuiltin.md)
[^20]: [RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)
[^21]: [Character](../Entities/Character.md)
[^22]: [Human](../Entities/Human.md)
[^23]: [MapObject](../Entities/MapObject.md)
[^24]: [NetworkView](../Entities/NetworkView.md)
[^25]: [Player](../Entities/Player.md)
[^26]: [Prefab](../Entities/Prefab.md)
[^27]: [Shifter](../Entities/Shifter.md)
[^28]: [Titan](../Entities/Titan.md)
[^29]: [Transform](../Entities/Transform.md)
[^30]: [WallColossal](../Entities/WallColossal.md)
[^31]: [AlignEnum](../Enums/AlignEnum.md)
[^32]: [AngleUnitEnum](../Enums/AngleUnitEnum.md)
[^33]: [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md)
[^34]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^35]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^36]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^37]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^38]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^39]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^40]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^41]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^42]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^43]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^44]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^45]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^46]: [HandStateEnum](../Enums/HandStateEnum.md)
[^47]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^48]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^49]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^50]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^51]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^52]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^53]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^54]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^55]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^56]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^57]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^58]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^59]: [JustifyEnum](../Enums/JustifyEnum.md)
[^60]: [LanguageEnum](../Enums/LanguageEnum.md)
[^61]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^62]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^63]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^64]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^65]: [OverflowEnum](../Enums/OverflowEnum.md)
[^66]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^67]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^68]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^69]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^70]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^71]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^72]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^73]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^74]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^75]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^76]: [SpecialEnum](../Enums/SpecialEnum.md)
[^77]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^78]: [TeamEnum](../Enums/TeamEnum.md)
[^79]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^80]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^81]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^82]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^83]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^84]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^85]: [UILabelEnum](../Enums/UILabelEnum.md)
[^86]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^87]: [WeaponEnum](../Enums/WeaponEnum.md)
[^88]: [Camera](../Game/Camera.md)
[^89]: [Cutscene](../Game/Cutscene.md)
[^90]: [Game](../Game/Game.md)
[^91]: [Input](../Game/Input.md)
[^92]: [Locale](../Game/Locale.md)
[^93]: [Map](../Game/Map.md)
[^94]: [Network](../Game/Network.md)
[^95]: [PersistentData](../Game/PersistentData.md)
[^96]: [Physics](../Game/Physics.md)
[^97]: [RoomData](../Game/RoomData.md)
[^98]: [Time](../Game/Time.md)
[^99]: [Button](../UIElements/Button.md)
[^100]: [Dropdown](../UIElements/Dropdown.md)
[^101]: [Icon](../UIElements/Icon.md)
[^102]: [Image](../UIElements/Image.md)
[^103]: [Label](../UIElements/Label.md)
[^104]: [ProgressBar](../UIElements/ProgressBar.md)
[^105]: [ScrollView](../UIElements/ScrollView.md)
[^106]: [Slider](../UIElements/Slider.md)
[^107]: [TextField](../UIElements/TextField.md)
[^108]: [Toggle](../UIElements/Toggle.md)
[^109]: [UI](../UIElements/UI.md)
[^110]: [VisualElement](../UIElements/VisualElement.md)
[^111]: [Convert](../Utility/Convert.md)
[^112]: [Json](../Utility/Json.md)
[^113]: [Math](../Utility/Math.md)
[^114]: [Random](../Utility/Random.md)
[^115]: [String](../Utility/String.md)
[^116]: [Object](../objects/Object.md)
[^117]: [Component](../objects/Component.md)
