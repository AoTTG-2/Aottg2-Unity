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
> Performs a line cast between two points, returns a LineCastHitResult object.
> 
> **Parameters**:
> - `start`: The start position of the line cast.
> - `end`: The end position of the line cast.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function LineCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the line cast.
> - `end`: The end position of the line cast.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).
> 
> **Parameters**:
> - `start`: The start position of the sphere cast.
> - `end`: The end position of the sphere cast.
> - `radius`: The radius of the sphere.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the sphere cast.
> - `end`: The end position of the sphere cast.
> - `radius`: The radius of the sphere.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Performs a box cast between two points, returns the object hit (Human, Titan, etc...).
> 
> **Parameters**:
> - `start`: The start position of the box cast.
> - `end`: The end position of the box cast.
> - `dimensions`: The dimensions of the box.
> - `orientation`: The orientation of the box.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a box cast between two points and returns a LineCastHitResult object for each element hit.
> 
> **Parameters**:
> - `start`: The start position of the box cast.
> - `end`: The end position of the box cast.
> - `dimensions`: The dimensions of the box.
> - `orientation`: The orientation of the box.
> - `collideWith`: The collision layer to check against. Refer to [CollideWithEnum](../static/CollideWithEnum.md)
> 
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collider: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a point on the given collider that is closest to the specified location.
> 
> **Parameters**:
> - `point`: The point to find the closest point to.
> - `collider`: The collider to check.
> - `position`: The position of the collider.
> - `rotation`: The rotation of the collider.
> 
<pre class="language-typescript"><code class="lang-typescript">function ComputePenetration(colliderA: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Compute the minimal translation required to separate the given colliders apart at specified poses. Returns Vector3.Zero if the colliders are not intersecting.
> 
> **Parameters**:
> - `colliderA`: The first collider.
> - `positionA`: The position of the first collider.
> - `rotationA`: The rotation of the first collider.
> - `colliderB`: The second collider.
> - `positionB`: The position of the second collider.
> - `rotationB`: The rotation of the second collider.
> 
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
