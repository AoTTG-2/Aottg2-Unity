# Map
Inherits from [Object](../objects/Object.md)

Finding, creating, and destroying map objects.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function FindAllMapObjects() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects.
> 
> **Returns**: A list of all map objects.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by name.
> 
> **Parameters**:
> - `objectName`: The name of the map object to find.
> 
> **Returns**: The map object if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by name.
> 
> **Parameters**:
> - `objectName`: The name of the map objects to find.
> 
> **Returns**: A list of map objects with the specified name.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByRegex(pattern: string, sorted: bool = False) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by regex pattern.
> 
> **Parameters**:
> - `pattern`: The regex pattern to match against map object names.
> - `sorted`: If true, sorts the results by name (default: false).
> 
> **Returns**: A list of map objects matching the regex pattern.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find all map objects by component.
> 
> **Parameters**:
> - `className`: The class name of the component to search for.
> 
> **Returns**: The first map object with the specified component, or null if not found.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by component.
> 
> **Parameters**:
> - `className`: The class name of the component to search for.
> 
> **Returns**: A list of map objects with the specified component.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByID(id: int) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by ID.
> 
> **Parameters**:
> - `id`: The ID of the map object to find.
> 
> **Returns**: The map object if found, null otherwise.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by tag.
> 
> **Parameters**:
> - `tag`: The tag to search for.
> 
> **Returns**: The first map object with the specified tag, or null if not found.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by tag.
> 
> **Parameters**:
> - `tag`: The tag to search for.
> 
> **Returns**: A list of map objects with the specified tag.
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByPlayer(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find a map objects of Player.
> 
> **Parameters**:
> - `player`: The player to find map objects for.
> 
> **Returns**: A list of map objects owned by the player.
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObject(prefab: <a data-footnote-ref href="#user-content-fn-26">Prefab</a>, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, scale: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Create a new map object.
> 
> **Parameters**:
> - `prefab`: The prefab to instantiate.
> - `position`: The position to spawn at (default: null, uses prefab position).
> - `rotation`: The rotation to spawn with (default: null, uses prefab rotation).
> - `scale`: The scale to spawn with (default: null, uses prefab scale).
> 
> **Returns**: The created map object.
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObjectRaw(prefab: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Create a new map object.
> 
> **Parameters**:
> - `prefab`: The serialized prefab string to instantiate.
> 
> **Returns**: The created map object.
<pre class="language-typescript"><code class="lang-typescript">function PrefabFromMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-23">MapObject</a>, clearComponents: bool = False) -> <a data-footnote-ref href="#user-content-fn-26">Prefab</a></code></pre>
> Create a new prefab object from the current object.
> 
> **Parameters**:
> - `mapObject`: The map object to create a prefab from.
> - `clearComponents`: If true, clears all components from the prefab (default: false).
> 
> **Returns**: The created prefab.
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-23">MapObject</a>, includeChildren: bool)</code></pre>
> Destroy a map object.
> 
> **Parameters**:
> - `mapObject`: The map object to destroy.
> - `includeChildren`: If true, also destroys all child objects.
> 
<pre class="language-typescript"><code class="lang-typescript">function CopyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-23">MapObject</a>, includeChildren: bool = True) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Copy a map object.
> 
> **Parameters**:
> - `mapObject`: The map object to copy.
> - `includeChildren`: If true, also copies all child objects (default: true).
> 
> **Returns**: The copied map object.
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapTargetable(targetable: <a data-footnote-ref href="#user-content-fn-17">MapTargetable</a>)</code></pre>
> Destroy a map targetable.
> 
> **Parameters**:
> - `targetable`: The map targetable to destroy.
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMesh()</code></pre>
> Update the nav mesh.
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMeshAsync()</code></pre>
> Update the nav mesh asynchronously.
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
