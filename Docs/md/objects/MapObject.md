# MapObject
Inherits from [Object](../objects/Object.md)

MapObject represents a map object created in the editor or spawned at runtime using Map static methods.

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Static|bool|True|Object does not move|
|Position|[Vector3](../objects/Vector3.md)|False|The position of the object|
|LocalPosition|[Vector3](../objects/Vector3.md)|False|The local position of the object|
|Rotation|[Vector3](../objects/Vector3.md)|False|The rotation of the object|
|LocalRotation|[Vector3](../objects/Vector3.md)|False|The local rotation of the object|
|QuaternionRotation|[Quaternion](../objects/Quaternion.md)|False|The rotation of the object as a quaternion|
|QuaternionLocalRotation|[Quaternion](../objects/Quaternion.md)|False|The local rotation of the object as a quaternion|
|Forward|[Vector3](../objects/Vector3.md)|False|The forward direction of the object|
|Up|[Vector3](../objects/Vector3.md)|False|The up direction of the object|
|Right|[Vector3](../objects/Vector3.md)|False|The right direction of the object|
|Scale|[Vector3](../objects/Vector3.md)|False|The scale of the object|
|Name|string|True|The name of the object|
|Parent|[Object](../objects/Object.md)|False|The parent of the object|
|Active|bool|False|Whether the object is active|
|Transform|[Transform](../objects/Transform.md)|True|The transform of the object|
|HasRenderer|bool|True|Whether the object has a renderer|
|Color|[Color](../objects/Color.md)|False|The color of the object|
|TextureTilingX|float|False|The x tiling of the object's texture|
|TextureTilingY|float|False|The y tiling of the object's texture|
|TextureOffsetX|float|False|The x offset of the object's texture|
|TextureOffsetY|float|False|The y offset of the object's texture|
|ID|int|True|The ID of the object|
|Tag|string|False|The tag of the object|
|Layer|int|False|The layer of the object|
|Rigidbody|[RigidbodyBuiltin](../static/RigidbodyBuiltin.md)|True|The Rigidbody component of the MapObject, is null if not added.|
|NetworkView|[NetworkView](../objects/NetworkView.md)|False|The NetworkView attached to the MapObject, is null if not initialized yet.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function AddComponent(name: string) -> component</code></pre>
> Add a component to the object
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveComponent(name: string)</code></pre>
> Remove a component from the object
> 
<pre class="language-typescript"><code class="lang-typescript">function GetComponent(name: string) -> component</code></pre>
> Get a component from the object
> 
<pre class="language-typescript"><code class="lang-typescript">function SetComponentEnabled(name: string, enabled: bool)</code></pre>
> Set whether a component is enabled
> 
<pre class="language-typescript"><code class="lang-typescript">function SetComponentsEnabled(enabled: bool)</code></pre>
> Set whether all components are enabled
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, radius: float)</code></pre>
> Add a sphere collider to the object
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBoxCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-43">Vector3</a> = null, size: <a data-footnote-ref href="#user-content-fn-43">Vector3</a> = null)</code></pre>
> Add a box collider to the object
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, radius: float) -> <a data-footnote-ref href="#user-content-fn-20">MapTargetable</a></code></pre>
> Add a sphere target to the object
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBoxTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>, size: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-20">MapTargetable</a></code></pre>
> Add a box target to the object
> 
<pre class="language-typescript"><code class="lang-typescript">function GetChild(name: string) -> <a data-footnote-ref href="#user-content-fn-19">MapObject</a></code></pre>
> Get a child object by name
> 
<pre class="language-typescript"><code class="lang-typescript">function GetChildren() -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Get all child objects
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-40">Transform</a></code></pre>
> Get a child transform by name
> 
<pre class="language-typescript"><code class="lang-typescript">function SetColorAll(color: <a data-footnote-ref href="#user-content-fn-4">Color</a>)</code></pre>
> Set the color of all renderers on the object
> 
<pre class="language-typescript"><code class="lang-typescript">function InBounds(position: <a data-footnote-ref href="#user-content-fn-43">Vector3</a>) -> bool</code></pre>
> Check if a position is within the object's bounds
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsAverageCenter() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds average center
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsCenter() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds center
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsSize() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds size
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsMin() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds min
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsMax() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds max
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBoundsExtents() -> <a data-footnote-ref href="#user-content-fn-43">Vector3</a></code></pre>
> Get the bounds extents
> 
<pre class="language-typescript"><code class="lang-typescript">function GetCorners() -> <a data-footnote-ref href="#user-content-fn-15">List</a></code></pre>
> Get the corners of the bounds
> 
<pre class="language-typescript"><code class="lang-typescript">function HasTag(tag: string) -> bool</code></pre>
> Whether or not the object has the given tag
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Add a builtin component to the MapObject
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-45">Object</a></code></pre>
> Gets a builtin component to the MapObject
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveBuiltinComponent(name: string)</code></pre>
> Remove a builtin component from the MapObject
> 
<pre class="language-typescript"><code class="lang-typescript">function ConvertToCSV() -> string</code></pre>
> Serialize the current object to a csv.
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
