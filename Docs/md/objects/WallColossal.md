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
|LeftHandState|string|True|Colossal's left hand state (Healthy or Broken).|
|RightHandState|string|True|Colossal's right hand state (Healthy or Broken).|
|HandRecoveryTime|float|False|Time in seconds for a hand to fully recover from broken state.|
|LeftHandRecoveryTimeLeft|float|False|Time remaining in seconds for left hand to recover (0 if not recovering).|
|RightHandRecoveryTimeLeft|float|False|Time remaining in seconds for right hand to recover (0 if not recovering).|
|WallAttackCooldown|float|False|Colossal's (AI) wall attack cooldown per attack.|
|WallAttackCooldownLeft|float|False|Colossal's (AI) wall attack cooldown remaining.|
|SteamState|string|True|Colossal's current steam state (Off, Warning, or Damage).|


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
