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
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
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
<pre class="language-typescript"><code class="lang-typescript">function BoxCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
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
