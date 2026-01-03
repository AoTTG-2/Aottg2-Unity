# WallColossal
Inherits from [Shifter](../objects/Shifter.md)

Represents a WallColossal character. Only character owner can modify fields and call functions unless otherwise specified.

### Example
```csharp
function OnCharacterSpawn(character) {
    if (character.IsMine && character.Type == "WallColossal") {
        character.Size = 2;
        if (Network.MyPlayer.Status == "Alive" && Network.MyPlayer.Character.Type == "Human") {
            character.Target(Network.MyPlayer, 10);
        }
    }
}
```
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|HandHealth|int|False|Colossal's current hand health (applies to both hands for backward compatibility).|
|MaxHandHealth|int|False|Colossal's maximum hand health (applies to both hands for backward compatibility).|
|LeftHandHealth|int|False|Colossal's current left hand health.|
|MaxLeftHandHealth|int|False|Colossal's maximum left hand health.|
|RightHandHealth|int|False|Colossal's current right hand health.|
|MaxRightHandHealth|int|False|Colossal's maximum right hand health.|
|LeftHandState|string|True|Colossal's left hand state. Refer to [HandStateEnum](../static/HandStateEnum.md)|
|RightHandState|string|True|Colossal's right hand state. Refer to [HandStateEnum](../static/HandStateEnum.md)|
|HandRecoveryTime|float|False|Time in seconds for a hand to fully recover from broken state.|
|LeftHandRecoveryTimeLeft|float|False|Time remaining in seconds for left hand to recover (0 if not recovering).|
|RightHandRecoveryTimeLeft|float|False|Time remaining in seconds for right hand to recover (0 if not recovering).|
|WallAttackCooldown|float|False|Colossal's (AI) wall attack cooldown per attack.|
|WallAttackCooldownLeft|float|False|Colossal's (AI) wall attack cooldown remaining.|
|SteamState|string|True|Colossal's current steam state. Refer to [SteamStateEnum](../static/SteamStateEnum.md)|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AttackSteam()</code></pre>
> Causes the colossal to perform steam attack.
> 
<pre class="language-typescript"><code class="lang-typescript">function StopSteam()</code></pre>
> Causes the colossal to stop steam attack.
> 
<pre class="language-typescript"><code class="lang-typescript">function WallAttack()</code></pre>
> Causes the (AI) colossal to perform a random wall attack.
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
