# Physics
Inherits from [Object](../objects/Object.md)

Static Physics class. Contains some common physics functions

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
<pre class="language-typescript"><code class="lang-typescript">function LineCast(start: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-13">LineCastHitResult</a></code></pre>
> Performs a line cast between two points, returns a LineCastHitResult object
> 
<pre class="language-typescript"><code class="lang-typescript">function LineCastAll(start: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCastAll(start: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(point: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, collider: <a data-footnote-ref href="#user-content-fn-2">Collider</a>, position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Returns a point on the given collider that is closest to the specified location.
> 
<pre class="language-typescript"><code class="lang-typescript">function ComputePenetration(colliderA: <a data-footnote-ref href="#user-content-fn-2">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-2">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Compute the minimal translation required to separate the given colliders apart at specified poses.
> 
<pre class="language-typescript"><code class="lang-typescript">function AreCollidersOverlapping(colliderA: <a data-footnote-ref href="#user-content-fn-2">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-2">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-30">Quaternion</a>) -> bool</code></pre>
> Check if the the given colliders at specified poses are apart or overlapping.
> 

[^0]: [Camera](../static/Camera.md)
[^1]: [Character](../objects/Character.md)
[^2]: [Collider](../objects/Collider.md)
[^3]: [Collision](../objects/Collision.md)
[^4]: [Color](../objects/Color.md)
[^5]: [Convert](../static/Convert.md)
[^6]: [Cutscene](../static/Cutscene.md)
[^7]: [Dict](../objects/Dict.md)
[^8]: [Game](../static/Game.md)
[^9]: [Human](../objects/Human.md)
[^10]: [Input](../static/Input.md)
[^11]: [Json](../static/Json.md)
[^12]: [LightBuiltin](../static/LightBuiltin.md)
[^13]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^14]: [LineRenderer](../objects/LineRenderer.md)
[^15]: [List](../objects/List.md)
[^16]: [Locale](../static/Locale.md)
[^17]: [LodBuiltin](../static/LodBuiltin.md)
[^18]: [Map](../static/Map.md)
[^19]: [MapObject](../objects/MapObject.md)
[^20]: [MapTargetable](../objects/MapTargetable.md)
[^21]: [Math](../static/Math.md)
[^22]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^23]: [Network](../static/Network.md)
[^24]: [NetworkView](../objects/NetworkView.md)
[^25]: [PersistentData](../static/PersistentData.md)
[^26]: [Physics](../static/Physics.md)
[^27]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^28]: [Player](../objects/Player.md)
[^29]: [Prefab](../objects/Prefab.md)
[^30]: [Quaternion](../objects/Quaternion.md)
[^31]: [Random](../objects/Random.md)
[^32]: [Range](../objects/Range.md)
[^33]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^34]: [RoomData](../static/RoomData.md)
[^35]: [Set](../objects/Set.md)
[^36]: [Shifter](../objects/Shifter.md)
[^37]: [String](../static/String.md)
[^38]: [Time](../static/Time.md)
[^39]: [Titan](../objects/Titan.md)
[^40]: [Transform](../objects/Transform.md)
[^41]: [UI](../static/UI.md)
[^42]: [Vector2](../objects/Vector2.md)
[^43]: [Vector3](../objects/Vector3.md)
[^44]: [WallColossal](../objects/WallColossal.md)
[^45]: [Object](../objects/Object.md)
[^46]: [Component](../objects/Component.md)
