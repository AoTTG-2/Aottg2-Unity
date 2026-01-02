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
> **Parameters**:
> - `name`: The name of the component to add.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveComponent(name: string)</code></pre>
> Remove a component from the object
> 
> **Parameters**:
> - `name`: The name of the component to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetComponent(name: string) -> component</code></pre>
> Get a component from the object
> 
> **Parameters**:
> - `name`: The name of the component to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetComponentEnabled(name: string, enabled: bool)</code></pre>
> Set whether a component is enabled
> 
> **Parameters**:
> - `name`: The name of the component.
> - `enabled`: Whether the component should be enabled.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetComponentsEnabled(enabled: bool)</code></pre>
> Set whether all components are enabled
> 
> **Parameters**:
> - `enabled`: Whether all components should be enabled.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float)</code></pre>
> Add a sphere collider to the object
> 
> **Parameters**:
> - `collideMode`: The collision mode (e.g., "Region", "Hitboxes").
> - `collideWith`: What the collider should collide with (e.g., "Hitboxes").
> - `center`: The center position of the sphere collider.
> - `radius`: The radius of the sphere collider.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBoxCollider(collideMode: string, collideWith: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null, size: <a data-footnote-ref href="#user-content-fn-9">Vector3</a> = null)</code></pre>
> Add a box collider to the object
> 
> **Parameters**:
> - `collideMode`: The collision mode (e.g., "Region", "Hitboxes").
> - `collideWith`: What the collider should collide with (e.g., "Hitboxes").
> - `center`: The center position of the box collider (optional, defaults to calculated bounds).
> - `size`: The size of the box collider (optional, defaults to calculated bounds).
> 
<pre class="language-typescript"><code class="lang-typescript">function AddSphereTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, radius: float) -> <a data-footnote-ref href="#user-content-fn-17">MapTargetable</a></code></pre>
> Add a sphere target to the object
> 
> **Parameters**:
> - `team`: The team that can target this (e.g., "Human", "Titan").
> - `center`: The center position of the sphere target.
> - `radius`: The radius of the sphere target.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBoxTarget(team: string, center: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, size: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-17">MapTargetable</a></code></pre>
> Add a box target to the object
> 
> **Parameters**:
> - `team`: The team that can target this (e.g., "Human", "Titan").
> - `center`: The center position of the box target.
> - `size`: The size of the box target.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetChild(name: string) -> <a data-footnote-ref href="#user-content-fn-23">MapObject</a></code></pre>
> Get a child object by name
> 
> **Parameters**:
> - `name`: The name of the child object to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetChildren() -> <a data-footnote-ref href="#user-content-fn-4">List</a><<a data-footnote-ref href="#user-content-fn-23">MapObject</a>></code></pre>
> Get all child objects.
> 
<pre class="language-typescript"><code class="lang-typescript">function GetTransform(name: string) -> <a data-footnote-ref href="#user-content-fn-29">Transform</a></code></pre>
> Get a child transform by name
> 
> **Parameters**:
> - `name`: The name of the transform to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function SetColorAll(color: <a data-footnote-ref href="#user-content-fn-0">Color</a>)</code></pre>
> Set the color of all renderers on the object
> 
> **Parameters**:
> - `color`: The color to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function InBounds(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> bool</code></pre>
> Check if a position is within the object's bounds
> 
> **Parameters**:
> - `position`: The position to check.
> 
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
> Whether or not the object has the given tag
> 
> **Parameters**:
> - `tag`: The tag to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function AddBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Add a builtin component to the MapObject
> 
> **Parameters**:
> - `name`: The name of the builtin component to add (e.g., "DayLight").
> 
<pre class="language-typescript"><code class="lang-typescript">function GetBuiltinComponent(name: string) -> <a data-footnote-ref href="#user-content-fn-59">Object</a></code></pre>
> Gets a builtin component to the MapObject
> 
> **Parameters**:
> - `name`: The name of the builtin component to get.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveBuiltinComponent(name: string)</code></pre>
> Remove a builtin component from the MapObject
> 
> **Parameters**:
> - `name`: The name of the builtin component to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function ConvertToCSV() -> string</code></pre>
> Serialize the current object to a csv.
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
