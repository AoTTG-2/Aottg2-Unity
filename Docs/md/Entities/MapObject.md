# MapObject
Inherits from [Object](../objects/Object.md)

MapObject represents a map object created in the editor or spawned at runtime using Map static methods.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Static|bool|True|Object does not move.|
|Position|[Vector3](../Collections/Vector3.md)|False|The position of the object.|
|LocalPosition|[Vector3](../Collections/Vector3.md)|False|The local position of the object.|
|Rotation|[Vector3](../Collections/Vector3.md)|False|The rotation of the object.|
|LocalRotation|[Vector3](../Collections/Vector3.md)|False|The local rotation of the object.|
|QuaternionRotation|[Quaternion](../Collections/Quaternion.md)|False|The rotation of the object as a quaternion.|
|QuaternionLocalRotation|[Quaternion](../Collections/Quaternion.md)|False|The local rotation of the object as a quaternion.|
|Forward|[Vector3](../Collections/Vector3.md)|False|The forward direction of the object.|
|Up|[Vector3](../Collections/Vector3.md)|False|The up direction of the object.|
|Right|[Vector3](../Collections/Vector3.md)|False|The right direction of the object.|
|Scale|[Vector3](../Collections/Vector3.md)|False|The scale of the object.|
|Name|string|True|The name of the object.|
|Parent|[Object](../objects/Object.md)|False|The parent of the object.|
|Active|bool|False|Whether the object is active.|
|Transform|[Transform](../Entities/Transform.md)|True|The transform of the object.|
|HasRenderer|bool|True|Whether the object has a renderer.|
|Color|[Color](../Collections/Color.md)|False|The color of the object.|
|TextureTilingX|float|False|The x tiling of the object's texture.|
|TextureTilingY|float|False|The y tiling of the object's texture.|
|TextureOffsetX|float|False|The x offset of the object's texture.|
|TextureOffsetY|float|False|The y offset of the object's texture.|
|ID|int|True|The ID of the object.|
|Tag|string|False|The tag of the object.|
|Layer|int|False|The layer of the object.|
|Rigidbody|[RigidbodyBuiltin](../Component/RigidbodyBuiltin.md)|True|The Rigidbody component of the MapObject, is null if not added.|
|NetworkView|[NetworkView](../Entities/NetworkView.md)|False|The NetworkView attached to the MapObject, is null if not initialized yet.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AddComponent(name: string) -> component</code></pre>
> Add a component to the object.
> 
> **Parameters**:
> - `name`: The name of the component to add.
> 
> **Returns**: The added component instance.
<pre class="language-typescript"><code class="lang-typescript">function RemoveComponent(name: string)</code></pre>
> Remove a component from the object.
> 
> **Parameters**:
> - `name`: The name of the component to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetComponent(name: string) -> component</code></pre>
> Get a component from the object.
> 
> **Parameters**:
> - `name`: The name of the component to get.
> 
> **Returns**: The component instance, or null if not found.
<pre class="language-typescript"><code class="lang-typescript">function SetComponentEnabled(name: string, enabled: bool)</code></pre>
> Set whether a component is enabled.
> 
> **Parameters**:
> - `name`: The name of the component.
> - `enabled`: Whether the component should be enabled.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetComponentsEnabled(enabled: bool)</code></pre>
> Set whether all components are enabled.
> 
> **Parameters**:
> - `enabled`: Whether all components should be enabled.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float)</code></pre>
> Add a sphere collider to the object.
> 
> **Parameters**:
> - `collideMode`: The collision mode. Refer to [CollideModeEnum](../Enums/CollideModeEnum.md)
> - `collideWith`: What the collider should collide with. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> - `center`: The center position of the sphere collider.
> - `radius`: The radius of the sphere collider.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBoxCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, size: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null)</code></pre>
> Add a box collider to the object.
> 
> **Parameters**:
> - `collideMode`: The collision mode. Refer to [CollideModeEnum](../Enums/CollideModeEnum.md)
> - `collideWith`: What the collider should collide with. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> - `center`: The center position of the box collider (optional, defaults to calculated bounds).
> - `size`: The size of the box collider (optional, defaults to calculated bounds).
> 
<pre class="language-typescript"><code class="lang-typescript">function SetCollideWith(collideWith: string)</code></pre>
> Set the collideWith property for all colliders on this object and its children.
This changes which layers the colliders can collide with.
> 
> **Parameters**:
> - `collideWith`: What the colliders should collide with. Refer to [CollideWithEnum](../Enums/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float) -> <a data-footnote-ref href="#user-content-fn-17">MapTargetable</a></code></pre>
> Add a sphere target to the object.
> 
> **Parameters**:
> - `team`: The team that can target this. Refer to [TeamEnum](../Enums/TeamEnum.md)
> - `center`: The center position of the sphere target.
> - `radius`: The radius of the sphere target.
> 
> **Returns**: The created targetable object.
<pre class="language-typescript"><code class="lang-typescript">function AddBoxTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, size: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-17">MapTargetable</a></code></pre>
> Add a box target to the object.
> 
> **Parameters**:
> - `team`: The team that can target this. Refer to [TeamEnum](../Enums/TeamEnum.md)
> - `center`: The center position of the box target.
> - `size`: The size of the box target.
> 
> **Returns**: The created targetable object.
<pre class="language-typescript"><code class="lang-typescript">function GetChild(name: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Get a child object by name.
> 
> **Parameters**:
> - `name`: The name of the child object to get.
> 
> **Returns**: The child object if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetChildren() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Get all child objects.
> 
> **Returns**: A list of all child objects.
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-29">Transform</a></code></pre>
> Get a child transform by name.
> 
> **Parameters**:
> - `name`: The name of the transform to get.
> 
> **Returns**: The transform if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function SetColorAll(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>)</code></pre>
> Set the color of all renderers on the object.
> 
> **Parameters**:
> - `color`: The color to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function InBounds(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> bool</code></pre>
> Check if a position is within the object's bounds.
> 
> **Parameters**:
> - `position`: The position to check.
> 
> **Returns**: True if the position is within the bounds, false otherwise.
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsAverageCenter() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds average center.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsCenter() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds center.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsSize() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds size.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsMin() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds min.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsMax() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds max.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsExtents() -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Get the bounds extents.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetCorners() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-9">Vector3</a>></code></pre>
> Get the corners of the bounds.
> 
<pre class="language-typescript"><code class="lang-typescript">function HasTag(tag: string) -> bool</code></pre>
> Whether or not the object has the given tag.
> 
> **Parameters**:
> - `tag`: The tag to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Add a builtin component to the MapObject.
> 
> **Parameters**:
> - `name`: The name of the builtin component to add.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddRigidbody() -> <a data-footnote-ref href="#user-content-fn-20">RigidbodyBuiltin</a></code></pre>
> Add a Rigidbody component to the MapObject.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-116">Object</a></code></pre>
> Gets a builtin component to the MapObject.
> 
> **Parameters**:
> - `name`: The name of the builtin component to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveBuiltinComponent(name: string)</code></pre>
> Remove a builtin component from the MapObject.
> 
> **Parameters**:
> - `name`: The name of the builtin component to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function ConvertToCSV() -> string</code></pre>
> Serialize the current object to a csv.
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
