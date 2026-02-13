# Character
Inherits from [Object](../objects/Object.md)

Character is the base class that `Human`, `Titan`, and `Shifter` inherit from.
Only character owner can modify properties and call functions unless otherwise specified.

### Remarks
Overloads operators: 
`==`, `__Hash__`
### Example
```csharp
function OnCharacterSpawn(character)
{
    if (character.IsMine)
    {
        # Character is owned (network-wise) by the person running this script.
        # Ex: If user is host, this could either be their actual player character or AI titans/shifters.
    }

    if (character.IsMainCharacter)
    {
        # Character is the main character (the camera-followed player).
    }

    if (character.IsAI)
    {
        # Character is AI and likely controlled via MasterClient.

        if (character.Player.ID == Network.MasterClient.ID)
        {
            # Character is owned by masterclient, if we're not masterclient, we cannot modify props.
        }
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Name|string|False|Character's name.|
|Guild|string|False|Character's guild.|
|Player|[Player](../Entities/Player.md)|True|Player who owns this character.|
|IsAI|bool|True|Is this character AI?|
|IsAlive|bool|True|Is this character alive? Value is set to false before despawn.|
|ViewID|int|True|Network view ID of the character.|
|IsMine|bool|True|Is this character mine?|
|IsMainCharacter|bool|True|Character belongs to my player and is the main character (the camera-followed player).|
|Transform|[Transform](../Entities/Transform.md)|True|Unity transform of the character.|
|Position|[Vector3](../Collections/Vector3.md)|False|Position of the character.|
|Rotation|[Vector3](../Collections/Vector3.md)|False|Rotation of the character.|
|QuaternionRotation|[Quaternion](../Collections/Quaternion.md)|False|Quaternion rotation of the character.|
|Velocity|[Vector3](../Collections/Vector3.md)|False|Velocity of the character.|
|Forward|[Vector3](../Collections/Vector3.md)|False|Forward direction of the character.|
|Right|[Vector3](../Collections/Vector3.md)|False|Right direction of the character.|
|Up|[Vector3](../Collections/Vector3.md)|False|Up direction of the character.|
|HasTargetDirection|bool|True|If the character has a target direction it is turning towards.|
|TargetDirection|[Vector3](../Collections/Vector3.md)|False|The character's target direction.|
|Team|string|False|Team character belongs to. Using enum is not mandatory, value can be any string. Refer to [TeamEnum](../Enums/TeamEnum.md)|
|Health|float|False|Character's current health.|
|MaxHealth|float|False|Character's maximum health.|
|CustomDamageEnabled|bool|False|Is custom damage dealing enabled.|
|CustomDamage|int|False|Amount of custom damage to deal per attack.|
|CurrentAnimation|string|True|Character's current playing animation.|
|Grounded|bool|True|Character's grounded status.|
|Rigidbody|[RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)|True|Character's rigidbody component (if available).|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function GetKilled(killer: string)</code></pre>
> Kills the character.
> 
> **Parameters**:
> - `killer`: Killer name.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetDamaged(killer: string, damage: int)</code></pre>
> Damages the character.
> 
> **Parameters**:
> - `killer`: Killer name.
> - `damage`: Damage amount.
> 
<pre class="language-typescript"><code class="lang-typescript">function Emote(emote: string)</code></pre>
> Causes the character to emote. The list of available emotes is the same as those shown in the in-game emote menu.
> 
> **Parameters**:
> - `emote`: Name of the emote to play.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimation(animation: string, fade: float = 0.1)</code></pre>
> Causes the character to play an animation. Available animations can be found here: Human, Titan, Annie, Eren.
Use the right-hand string value for the animation. Note that shifters also have all titan animations.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep.
> 
<pre class="language-typescript"><code class="lang-typescript">function PlayAnimationAt(animation: string, t: float, fade: float = 0.1, force: bool = False)</code></pre>
> Causes the character to play an animation at a specific time.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> - `t`: Time in the animation to start playing.
> - `fade`: Fade time.
> - `force`: Whether to force the animation even if it's already playing.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationSpeed(animation: string) -> float</code></pre>
> Gets the animation speed of a given animation.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: 1.0 if the character is not owned by the player or is dead, otherwise the animation speed.
<pre class="language-typescript"><code class="lang-typescript">function SetAnimationSpeed(animation: string, speed: float, synced: bool = True)</code></pre>
> Sets the animation speed of a given animation.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> - `speed`: The animation speed multiplier.
> - `synced`: Whether to sync the speed across the network.
> 
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingAnimation(animation: string) -> bool</code></pre>
> Returns true if the animation is playing.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: True if the animation is playing, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationNormalizedTime(animation: string) -> float</code></pre>
> Gets the normalized time of the currently playing animation.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: The normalized time (0-1) of the animation.
<pre class="language-typescript"><code class="lang-typescript">function ForceAnimation(animation: string, fade: float = 0.1)</code></pre>
> Forces the character to play an animation, even if it's already playing.
Available animations can be found here: Human, Titan, Annie, Eren.
Use the right-hand string value for the animation. Note that shifters also have all titan animations.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> - `fade`: Fade time. If provided, will crossfade the animation by this timestep.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetAnimationLength(animation: string) -> float</code></pre>
> Gets the length of animation.
> 
> **Parameters**:
> - `animation`: Name of the animation. Refer to [HumanAnimationEnum](../Enums/HumanAnimationEnum.md), [TitanAnimationEnum](../Enums/TitanAnimationEnum.md), [AnnieAnimationEnum](../Enums/AnnieAnimationEnum.md), [ErenAnimationEnum](../Enums/ErenAnimationEnum.md), [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md), [DummyAnimationEnum](../Enums/DummyAnimationEnum.md), [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
> 
> **Returns**: The length of the animation in seconds.
<pre class="language-typescript"><code class="lang-typescript">function IsPlayingSound(sound: string) -> bool</code></pre>
> Returns true if the character is playing a sound.
Available sound names can be found here: Humans, Shifters, Titans.
Note that shifters also have all titan sounds.
> 
> **Parameters**:
> - `sound`: Name of the sound. Refer to [HumanSoundEnum](../Enums/HumanSoundEnum.md), [TitanSoundEnum](../Enums/TitanSoundEnum.md), [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
> 
> **Returns**: True if the sound is playing, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function PlaySound(sound: string)</code></pre>
> Plays a sound for the character.
Available sound names can be found here: Human, Shifters, Titans.
Note that shifters also have all titan sounds.
> 
> **Parameters**:
> - `sound`: Name of the sound to play. Refer to [HumanSoundEnum](../Enums/HumanSoundEnum.md), [TitanSoundEnum](../Enums/TitanSoundEnum.md), [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSound(sound: string)</code></pre>
> Stops a sound for the character.
> 
> **Parameters**:
> - `sound`: Name of the sound to stop. Refer to [HumanSoundEnum](../Enums/HumanSoundEnum.md), [TitanSoundEnum](../Enums/TitanSoundEnum.md), [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function FadeSound(sound: string, volume: float, time: float)</code></pre>
> Fades the sound volume to a specific volume between 0.0 and 1.0 over [time] seconds.
Does not play or stop the sound.
> 
> **Parameters**:
> - `sound`: Name of the sound. Refer to [HumanSoundEnum](../Enums/HumanSoundEnum.md), [TitanSoundEnum](../Enums/TitanSoundEnum.md), [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
> - `volume`: Target volume (0.0 to 1.0).
> - `time`: Time in seconds to fade over.
> 
<pre class="language-typescript"><code class="lang-typescript">function LookAt(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Rotates the character such that it is looking towards a world position.
> 
> **Parameters**:
> - `position`: The world position to look at.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddForce(force: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, mode: int)</code></pre>
> Adds a force to the character with given force vector and optional mode.
> 
> **Parameters**:
> - `force`: Force vector.
> - `mode`: Force mode. Default is Acceleration. Refer to [ForceModeEnum](../Enums/ForceModeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function Reveal(delay: float)</code></pre>
> Reveal the titan for a set number of seconds.
> 
> **Parameters**:
> - `delay`: Delay in seconds before revealing.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddOutline(color: <a data-footnote-ref href="#user-content-fn-0">Color</a> = null, mode: string = "OutlineAll")</code></pre>
> Adds an outline effect with the given color and mode.
> 
> **Parameters**:
> - `color`: Outline color.
> - `mode`: Outline mode. Default is OutlineAll. Refer to [OutlineModeEnum](../Enums/OutlineModeEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveOutline()</code></pre>
> Removes the outline effect from the character.
> 

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
[^34]: [AspectRatioEnum](../Enums/AspectRatioEnum.md)
[^35]: [CameraModeEnum](../Enums/CameraModeEnum.md)
[^36]: [CharacterTypeEnum](../Enums/CharacterTypeEnum.md)
[^37]: [CollideModeEnum](../Enums/CollideModeEnum.md)
[^38]: [CollideWithEnum](../Enums/CollideWithEnum.md)
[^39]: [CollisionDetectionModeEnum](../Enums/CollisionDetectionModeEnum.md)
[^40]: [DummyAnimationEnum](../Enums/DummyAnimationEnum.md)
[^41]: [EffectNameEnum](../Enums/EffectNameEnum.md)
[^42]: [ErenAnimationEnum](../Enums/ErenAnimationEnum.md)
[^43]: [FlexDirectionEnum](../Enums/FlexDirectionEnum.md)
[^44]: [FontScaleModeEnum](../Enums/FontScaleModeEnum.md)
[^45]: [FontStyleEnum](../Enums/FontStyleEnum.md)
[^46]: [ForceModeEnum](../Enums/ForceModeEnum.md)
[^47]: [GradientModeEnum](../Enums/GradientModeEnum.md)
[^48]: [HandStateEnum](../Enums/HandStateEnum.md)
[^49]: [HorseAnimationEnum](../Enums/HorseAnimationEnum.md)
[^50]: [HumanAnimationEnum](../Enums/HumanAnimationEnum.md)
[^51]: [HumanParticleEffectEnum](../Enums/HumanParticleEffectEnum.md)
[^52]: [HumanSoundEnum](../Enums/HumanSoundEnum.md)
[^53]: [HumanStateEnum](../Enums/HumanStateEnum.md)
[^54]: [InputAnnieShifterEnum](../Enums/InputAnnieShifterEnum.md)
[^55]: [InputCategoryEnum](../Enums/InputCategoryEnum.md)
[^56]: [InputErenShifterEnum](../Enums/InputErenShifterEnum.md)
[^57]: [InputGeneralEnum](../Enums/InputGeneralEnum.md)
[^58]: [InputHumanEnum](../Enums/InputHumanEnum.md)
[^59]: [InputInteractionEnum](../Enums/InputInteractionEnum.md)
[^60]: [InputTitanEnum](../Enums/InputTitanEnum.md)
[^61]: [JustifyEnum](../Enums/JustifyEnum.md)
[^62]: [LanguageEnum](../Enums/LanguageEnum.md)
[^63]: [LineAlignmentEnum](../Enums/LineAlignmentEnum.md)
[^64]: [LineTextureModeEnum](../Enums/LineTextureModeEnum.md)
[^65]: [LoadoutEnum](../Enums/LoadoutEnum.md)
[^66]: [OutlineModeEnum](../Enums/OutlineModeEnum.md)
[^67]: [OverflowEnum](../Enums/OverflowEnum.md)
[^68]: [PhysicMaterialCombineEnum](../Enums/PhysicMaterialCombineEnum.md)
[^69]: [PlayerStatusEnum](../Enums/PlayerStatusEnum.md)
[^70]: [ProfileIconEnum](../Enums/ProfileIconEnum.md)
[^71]: [ProjectileNameEnum](../Enums/ProjectileNameEnum.md)
[^72]: [ScaleModeEnum](../Enums/ScaleModeEnum.md)
[^73]: [ScrollElasticityEnum](../Enums/ScrollElasticityEnum.md)
[^74]: [ShadowCastingModeEnum](../Enums/ShadowCastingModeEnum.md)
[^75]: [ShifterSoundEnum](../Enums/ShifterSoundEnum.md)
[^76]: [ShifterTypeEnum](../Enums/ShifterTypeEnum.md)
[^77]: [SliderDirectionEnum](../Enums/SliderDirectionEnum.md)
[^78]: [SpecialEnum](../Enums/SpecialEnum.md)
[^79]: [SteamStateEnum](../Enums/SteamStateEnum.md)
[^80]: [StunStateEnum](../Enums/StunStateEnum.md)
[^81]: [TeamEnum](../Enums/TeamEnum.md)
[^82]: [TextAlignEnum](../Enums/TextAlignEnum.md)
[^83]: [TextOverflowEnum](../Enums/TextOverflowEnum.md)
[^84]: [TitanAnimationEnum](../Enums/TitanAnimationEnum.md)
[^85]: [TitanSoundEnum](../Enums/TitanSoundEnum.md)
[^86]: [TitanTypeEnum](../Enums/TitanTypeEnum.md)
[^87]: [TSKillSoundEnum](../Enums/TSKillSoundEnum.md)
[^88]: [UILabelEnum](../Enums/UILabelEnum.md)
[^89]: [WallColossalAnimationEnum](../Enums/WallColossalAnimationEnum.md)
[^90]: [WeaponEnum](../Enums/WeaponEnum.md)
[^91]: [Camera](../Game/Camera.md)
[^92]: [Cutscene](../Game/Cutscene.md)
[^93]: [Game](../Game/Game.md)
[^94]: [Input](../Game/Input.md)
[^95]: [Locale](../Game/Locale.md)
[^96]: [Map](../Game/Map.md)
[^97]: [Network](../Game/Network.md)
[^98]: [PersistentData](../Game/PersistentData.md)
[^99]: [Physics](../Game/Physics.md)
[^100]: [RoomData](../Game/RoomData.md)
[^101]: [Time](../Game/Time.md)
[^102]: [Button](../UIElements/Button.md)
[^103]: [Dropdown](../UIElements/Dropdown.md)
[^104]: [Icon](../UIElements/Icon.md)
[^105]: [Image](../UIElements/Image.md)
[^106]: [Label](../UIElements/Label.md)
[^107]: [ProgressBar](../UIElements/ProgressBar.md)
[^108]: [ScrollView](../UIElements/ScrollView.md)
[^109]: [Slider](../UIElements/Slider.md)
[^110]: [TextField](../UIElements/TextField.md)
[^111]: [Toggle](../UIElements/Toggle.md)
[^112]: [UI](../UIElements/UI.md)
[^113]: [VisualElement](../UIElements/VisualElement.md)
[^114]: [Convert](../Utility/Convert.md)
[^115]: [Json](../Utility/Json.md)
[^116]: [Math](../Utility/Math.md)
[^117]: [Random](../Utility/Random.md)
[^118]: [String](../Utility/String.md)
[^119]: [Object](../objects/Object.md)
[^120]: [Component](../objects/Component.md)
