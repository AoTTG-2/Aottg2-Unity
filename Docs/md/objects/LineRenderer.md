# LineRenderer
Inherits from [Object](../objects/Object.md)

Represents a LineRenderer.

### Initialization
```csharp
LineRenderer() // Default constructor, creates a black line with a width of 1.
LineRenderer(color: Color, width: float = 1) // Creates a line with the given color and width.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|StartWidth|float|False|The width of the line at the start.|
|EndWidth|float|False|The width of the line at the end.|
|LineColor|[Color](../objects/Color.md)|False|The color of the line.|
|PositionCount|int|False|The number of points in the line.|
|Enabled|bool|False|Is the line renderer enabled.|
|Loop|bool|False|Is the line renderer a loop.|
|NumCornerVertices|int|False|The number of corner vertices.|
|NumCapVertices|int|False|The number of end cap vertices.|
|Alignment|string|False|The alignment of the line renderer.|
|TextureMode|string|False|The texture mode of the line renderer.|
|UseWorldSpace|bool|False|Is the line renderer in world space.|
|ShadowCastingMode|string|False|Does the line renderer cast shadows.|
|ReceiveShadows|bool|False|Does the line renderer receive shadows.|
|ColorGradient|[List](../objects/List.md)<[Color](../objects/Color.md)>|False|The gradient of the line renderer.|
|AlphaGradient|[List](../objects/List.md)<float>|False|The alpha gradient of the line renderer.|
|WidthCurve|[List](../objects/List.md)<[Vector2](../objects/Vector2.md)>|False|The width curve of the line renderer.|
|WidthMultiplier|float|False|The width multiplier of the line renderer.|
|ColorGradientMode|string|False|The color gradient mode of the line renderer.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Destroy()</code></pre>
> Remove the line renderer (can also be done by removing all references to this object).
> 
<pre class="language-typescript"><code class="lang-typescript">function GetPosition(index: int) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the position of a point in the line renderer.
> 
> **Parameters**:
> - `index`: The index of the point (0 to PositionCount-1).
> 
<pre class="language-typescript"><code class="lang-typescript">function SetPosition(index: int, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>)</code></pre>
> Set the position of a point in the line renderer.
> 
> **Parameters**:
> - `index`: The index of the point to set (0 to PositionCount-1).
> - `position`: The position to set.
> 

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function CreateLineRenderer() -> <a data-footnote-ref href="#user-content-fn-15">LineRenderer</a></code></pre>

{% hint style="warning" %}
**Obsolete**: Create a new instance with LineRenderer() instead.
{% endhint %}

> Create a new LineRenderer.
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
