# Map
Inherits from [Object](../objects/Object.md)

Finding, creating, and destroying map objects.

### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function FindAllMapObjects() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by name.
> 
> **Parameters**:
> - `objectName`: The name of the map object to find.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by name.
> 
> **Parameters**:
> - `objectName`: The name of the map objects to find.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByRegex(pattern: string, sorted: bool = False) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by regex pattern.
> 
> **Parameters**:
> - `pattern`: The regex pattern to match against map object names.
> - `sorted`: If true, sorts the results by name (default: false).
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find all map objects by component.
> 
> **Parameters**:
> - `className`: The class name of the component to search for.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by component.
> 
> **Parameters**:
> - `className`: The class name of the component to search for.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByID(id: int) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by ID.
> 
> **Parameters**:
> - `id`: The ID of the map object to find.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Find a map object by tag.
> 
> **Parameters**:
> - `tag`: The tag to search for.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find all map objects by tag.
> 
> **Parameters**:
> - `tag`: The tag to search for.
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByPlayer(player: <a data-footnote-ref href="#user-content-fn-25">Player</a>) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Find a map objects of Player.
> 
> **Parameters**:
> - `player`: The player to find map objects for.
> 
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObject(prefab: <a data-footnote-ref href="#user-content-fn-26">Prefab</a>, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, rotation: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, scale: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Create a new map object.
> 
> **Parameters**:
> - `prefab`: The prefab to instantiate.
> - `position`: The position to spawn at (default: null, uses prefab position).
> - `rotation`: The rotation to spawn with (default: null, uses prefab rotation).
> - `scale`: The scale to spawn with (default: null, uses prefab scale).
> 
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObjectRaw(prefab: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Create a new map object.
> 
> **Parameters**:
> - `prefab`: The serialized prefab string to instantiate.
> 
<pre class="language-typescript"><code class="lang-typescript">function PrefabFromMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-23">MapObject</a>, clearComponents: bool = False) -> <a data-footnote-ref href="#user-content-fn-26">Prefab</a></code></pre>
> Create a new prefab object from the current object.
> 
> **Parameters**:
> - `mapObject`: The map object to create a prefab from.
> - `clearComponents`: If true, clears all components from the prefab (default: false).
> 
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
