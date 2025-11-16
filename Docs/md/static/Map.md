# Map
Inherits from [Object](../objects/Object.md)

Finding, creating, and destroying map objects.

### Methods
<pre class="language-typescript"><code class="lang-typescript">function FindAllMapObjects() -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find all map objects
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Find a map object by name
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find all map objects by name
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByRegex(pattern: string, sorted: bool = False) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find all map objects by regex pattern
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Find all map objects by component
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find all map objects by component
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByID(id: int) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Find a map object by ID
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Find a map object by tag
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find all map objects by tag
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByPlayer(player: <a data-footnote-ref href="#user-content-fn-31">Player</a>) -> <a data-footnote-ref href="#user-content-fn-18">List</a></code></pre>
> Find a map objects of Player
> 
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObject(prefab: <a data-footnote-ref href="#user-content-fn-32">Prefab</a>, position: <a data-footnote-ref href="#user-content-fn-46">Vector3</a> = null, rotation: <a data-footnote-ref href="#user-content-fn-46">Vector3</a> = null, scale: <a data-footnote-ref href="#user-content-fn-46">Vector3</a> = null) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Create a new map object
> 
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObjectRaw(prefab: string) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Create a new map object
> 
<pre class="language-typescript"><code class="lang-typescript">function PrefabFromMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-22">MapObject</a>, clearComponents: bool = False) -> <a data-footnote-ref href="#user-content-fn-32">Prefab</a></code></pre>
> Create a new prefab object from the current object
> 
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-22">MapObject</a>, includeChildren: bool)</code></pre>
> Destroy a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function CopyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-22">MapObject</a>, includeChildren: bool = True) -> <a data-footnote-ref href="#user-content-fn-22">MapObject</a></code></pre>
> Copy a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapTargetable(targetable: <a data-footnote-ref href="#user-content-fn-23">MapTargetable</a>)</code></pre>
> Destroy a map targetable
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMesh()</code></pre>
> Update the nav mesh
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMeshAsync()</code></pre>
> Update the nav mesh asynchronously
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
