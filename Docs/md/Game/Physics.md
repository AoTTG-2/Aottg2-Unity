# Physics
Inherits from [Object](../objects/Object.md)

Static Physics class. Contains some common physics functions.

### Example
```csharp
start = Vector3(0);
end = Vector3(10);
# Options: All, MapObjects, Characters, Titans, Humans, Projectiles, Entities, Hitboxes, MapEditor
result = Physics.LineCast(start, end, "Entities");
Game.Print(result.IsCharacter);
Game.Print(result.IsMapObject);
Game.Print(result.Point);
Game.Print(result.Normal);
Game.Print(result.Distance);
Game.Print(result.Collider);
```
### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function LineCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a></code></pre>
> Performs a line cast between two points.
> 
> **Parameters**:
> - `start`: The start position of the line cast.
> - `end`: The end position of the line cast.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: A LineCastHitResult object.
<pre class="language-typescript"><code class="lang-typescript">function LineCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the line cast.
> - `end`: The end position of the line cast.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: A list of LineCastHitResult objects.
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-119">Object</a></code></pre>
> Performs a sphere cast between two points.
> 
> **Parameters**:
> - `start`: The start position of the sphere cast.
> - `end`: The end position of the sphere cast.
> - `radius`: The radius of the sphere.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: The object hit (Human, Titan, etc...), or null if nothing was hit.
<pre class="language-typescript"><code class="lang-typescript">function SphereCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the sphere cast.
> - `end`: The end position of the sphere cast.
> - `radius`: The radius of the sphere.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: A list of LineCastHitResult objects.
<pre class="language-typescript"><code class="lang-typescript">function BoxCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-119">Object</a></code></pre>
> Performs a box cast between two points.
> 
> **Parameters**:
> - `start`: The start position of the box cast.
> - `end`: The end position of the box cast.
> - `dimensions`: The dimensions of the box.
> - `orientation`: The orientation of the box.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: The object hit (Human, Titan, etc...), or null if nothing was hit.
<pre class="language-typescript"><code class="lang-typescript">function BoxCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a box cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the box cast.
> - `end`: The end position of the box cast.
> - `dimensions`: The dimensions of the box.
> - `orientation`: The orientation of the box.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
> **Returns**: A list of LineCastHitResult objects.
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collider: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a point on the given collider that is closest to the specified location.
> 
> **Parameters**:
> - `point`: The point to find the closest point to.
> - `collider`: The collider to check.
> - `position`: The position of the collider.
> - `rotation`: The rotation of the collider.
> 
> **Returns**: The closest point on the collider.
<pre class="language-typescript"><code class="lang-typescript">function ComputePenetration(colliderA: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Compute the minimal translation required to separate the given colliders apart at specified poses.
> 
> **Parameters**:
> - `colliderA`: The first collider.
> - `positionA`: The position of the first collider.
> - `rotationA`: The rotation of the first collider.
> - `colliderB`: The second collider.
> - `positionB`: The position of the second collider.
> - `rotationB`: The rotation of the second collider.
> 
> **Returns**: Vector3.Zero if the colliders are not intersecting, otherwise the minimal translation vector.
<pre class="language-typescript"><code class="lang-typescript">function AreCollidersOverlapping(colliderA: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> bool</code></pre>
> Check if the the given colliders at specified poses are apart or overlapping.
> 
> **Parameters**:
> - `colliderA`: The first collider.
> - `positionA`: The position of the first collider.
> - `rotationA`: The rotation of the first collider.
> - `colliderB`: The second collider.
> - `positionB`: The position of the second collider.
> - `rotationB`: The rotation of the second collider.
> 
> **Returns**: True if the colliders are overlapping, false otherwise.

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
