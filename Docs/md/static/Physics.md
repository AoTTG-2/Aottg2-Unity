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
### Static Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|CollideWithAll|string|True|All Collide Mode|
|CollideWithMapObjects|string|True|MapObject Collide Mode|
|CollideWithCharacters|string|True|Characters Collide Mode|
|CollideWithTitans|string|True|Titans Collide Mode|
|CollideWithHumans|string|True|Humans Collide Mode|
|CollideWithProjectiles|string|True|Projectiles Collide Mode|
|CollideWithEntities|string|True|Entities Collide Mode|
|CollideWithHitboxes|string|True|Hitboxes Collide Mode|
|CollideWithMapEditor|string|True|MapEditor Collide Mode|


### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function LineCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a></code></pre>
> Performs a line cast between two points, returns a LineCastHitResult object.
> 
<pre class="language-typescript"><code class="lang-typescript">function LineCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Performs a box cast between two points, returns the object hit (Human, Titan, etc...).
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCastAll(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a>></code></pre>
> Performs a box cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(point: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collider: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Returns a point on the given collider that is closest to the specified location.
> 
<pre class="language-typescript"><code class="lang-typescript">function ComputePenetration(colliderA: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Compute the minimal translation required to separate the given colliders apart at specified poses. Returns Vector3.Zero if the colliders are not intersecting.
> 
<pre class="language-typescript"><code class="lang-typescript">function AreCollidersOverlapping(colliderA: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-13">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-5">Quaternion</a>) -> bool</code></pre>
> Check if the the given colliders at specified poses are apart or overlapping.
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
