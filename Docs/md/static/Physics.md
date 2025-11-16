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
<pre class="language-typescript"><code class="lang-typescript">function LineCast(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-16">LineCastHitResult</a></code></pre>
> Performs a line cast between two points, returns a LineCastHitResult object
> 
<pre class="language-typescript"><code class="lang-typescript">function LineCastAll(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Performs a line cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCastAll(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Performs a sphere cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCast(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-57">Object</a></code></pre>
> Performs a box cast between two points, returns the object hit (Human, Titan, etc...).
> 
<pre class="language-typescript"><code class="lang-typescript">function BoxCastAll(start: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, dimensions: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, orientation: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Performs a box cast between two points and returns a LineCastHitResult object for each element hit.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(point: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, collider: <a data-footnote-ref href="#user-content-fn-5">Collider</a>, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotation: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Returns a point on the given collider that is closest to the specified location.
> 
<pre class="language-typescript"><code class="lang-typescript">function ComputePenetration(colliderA: <a data-footnote-ref href="#user-content-fn-5">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-5">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>) -> <a data-footnote-ref href="#user-content-fn-46">Vector3</a></code></pre>
> Compute the minimal translation required to separate the given colliders apart at specified poses.
> 
<pre class="language-typescript"><code class="lang-typescript">function AreCollidersOverlapping(colliderA: <a data-footnote-ref href="#user-content-fn-5">Collider</a>, positionA: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationA: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>, colliderB: <a data-footnote-ref href="#user-content-fn-5">Collider</a>, positionB: <a data-footnote-ref href="#user-content-fn-46">Vector3</a>, rotationB: <a data-footnote-ref href="#user-content-fn-33">Quaternion</a>) -> bool</code></pre>
> Check if the the given colliders at specified poses are apart or overlapping.
> 

[^0]: [Animation](../objects/Animation.md)
[^1]: [Animator](../objects/Animator.md)
[^2]: [AudioSource](../objects/AudioSource.md)
[^3]: [Camera](../static/Camera.md)
[^4]: [Character](../objects/Character.md)
[^5]: [Collider](../objects/Collider.md)
[^6]: [Collision](../objects/Collision.md)
[^7]: [Color](../objects/Color.md)
[^8]: [Convert](../static/Convert.md)
[^9]: [Cutscene](../static/Cutscene.md)
[^10]: [Dict](../objects/Dict.md)
[^11]: [Game](../static/Game.md)
[^12]: [Human](../objects/Human.md)
[^13]: [Input](../static/Input.md)
[^14]: [Json](../static/Json.md)
[^15]: [LightBuiltin](../static/LightBuiltin.md)
[^16]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^17]: [LineRenderer](../objects/LineRenderer.md)
[^18]: [List](../objects/List.md)
[^19]: [Locale](../static/Locale.md)
[^20]: [LodBuiltin](../static/LodBuiltin.md)
[^21]: [Map](../static/Map.md)
[^22]: [MapObject](../objects/MapObject.md)
[^23]: [MapTargetable](../objects/MapTargetable.md)
[^24]: [Math](../static/Math.md)
[^25]: [NavmeshObstacleBuiltin](../static/NavmeshObstacleBuiltin.md)
[^26]: [Network](../static/Network.md)
[^27]: [NetworkView](../objects/NetworkView.md)
[^28]: [PersistentData](../static/PersistentData.md)
[^29]: [Physics](../static/Physics.md)
[^30]: [PhysicsMaterialBuiltin](../static/PhysicsMaterialBuiltin.md)
[^31]: [Player](../objects/Player.md)
[^32]: [Prefab](../objects/Prefab.md)
[^33]: [Quaternion](../objects/Quaternion.md)
[^34]: [Random](../objects/Random.md)
[^35]: [Range](../objects/Range.md)
[^36]: [RigidbodyBuiltin](../static/RigidbodyBuiltin.md)
[^37]: [RoomData](../static/RoomData.md)
[^38]: [Set](../objects/Set.md)
[^39]: [Shifter](../objects/Shifter.md)
[^40]: [String](../static/String.md)
[^41]: [Time](../static/Time.md)
[^42]: [Titan](../objects/Titan.md)
[^43]: [Transform](../objects/Transform.md)
[^44]: [UI](../static/UI.md)
[^45]: [Vector2](../objects/Vector2.md)
[^46]: [Vector3](../objects/Vector3.md)
[^47]: [WallColossal](../objects/WallColossal.md)
[^48]: [Button](../objects/Button.md)
[^49]: [Dropdown](../objects/Dropdown.md)
[^50]: [Label](../objects/Label.md)
[^51]: [ProgressBar](../objects/ProgressBar.md)
[^52]: [ScrollView](../objects/ScrollView.md)
[^53]: [Slider](../objects/Slider.md)
[^54]: [TextField](../objects/TextField.md)
[^55]: [Toggle](../objects/Toggle.md)
[^56]: [VisualElement](../objects/VisualElement.md)
[^57]: [Object](../objects/Object.md)
[^58]: [Component](../objects/Component.md)
