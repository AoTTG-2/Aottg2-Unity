# Animation

Represents an Animation component for playing legacy animation clips.

### Methods
<pre class="language-typescript"><code class="lang-typescript">function IsPlaying(anim: string) -> bool</code></pre>
> Checks if the given animation is playing.
> 
> **Parameters**:
> - `anim`: The name of the animation to check.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(anim: string, fade: float = 0.1, layer: int = 0)</code></pre>
> Plays the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation to play.
> - `fade`: The fade time in seconds for cross-fading (default: 0.1).
> - `layer`: The animation layer to play on (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationAt(anim: string, normalizedTime: float, fade: float = 0.1, layer: int = 0)</code></pre>
> Plays the specified animation starting from a normalized time.
> 
> **Parameters**:
> - `anim`: The name of the animation to play.
> - `normalizedTime`: The normalized time (0-1) to start the animation from.
> - `fade`: The fade time in seconds for cross-fading (default: 0.1).
> - `layer`: The animation layer to play on (default: 0).
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationQueued(anim: string)</code></pre>
> Plays the specified animation after the current animation finishes playing.
> 
> **Parameters**:
> - `anim`: The name of the animation to queue.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopAnimation(anim: string = null)</code></pre>
> Stops the specified animation. Will stop all animations if no name is given.
> 
> **Parameters**:
> - `anim`: The name of the animation to stop. If null, stops all animations.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationSpeed(name: string, speed: float)</code></pre>
> Sets the playback speed of the specified animation.
> 
> **Parameters**:
> - `name`: The name of the animation.
> - `speed`: The playback speed multiplier (1.0 = normal speed).
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationSpeed(name: string) -> float</code></pre>
> Gets the playback speed of the specified animation.
> 
> **Parameters**:
> - `name`: The name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(anim: string) -> float</code></pre>
> Gets the length of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationNormalizedTime(anim: string) -> float</code></pre>
> Gets the normalized time of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationNormalizedTime(anim: string, normalizedTime: float)</code></pre>
> Sets the normalized playback time of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation.
> - `normalizedTime`: The normalized time (0-1) to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationWeight(anim: string, weight: float)</code></pre>
> Sets the weight of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation.
> - `weight`: The weight value (0-1) to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationWeight(anim: string) -> float</code></pre>
> Gets the weight of the specified animation.
> 
> **Parameters**:
> - `anim`: The name of the animation.
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
