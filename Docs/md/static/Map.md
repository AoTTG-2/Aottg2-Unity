# Map
Inherits from [Object](../objects/Object.md)

Finding, creating, and destroying map objects.

### Methods
<pre class="language-typescript"><code class="lang-typescript">function FindAllMapObjects() -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Find all map objects
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Find a map object by name
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByName(objectName: string) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Find all map objects by name
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Find all map objects by component
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByComponent(className: string) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Find all map objects by component
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByID(id: int) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Find a map object by ID
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Find a map object by tag
> 
<pre class="language-typescript"><code class="lang-typescript">function FindMapObjectsByTag(tag: string) -> <a data-footnote-ref href="#user-content-fn-14">List</a></code></pre>
> Find all map objects by tag
> 
<pre class="language-typescript"><code class="lang-typescript">function CreateMapObjectRaw(prefab: string) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Create a new map object
> 
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-17">MapObject</a>, includeChildren: bool)</code></pre>
> Destroy a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function CopyMapObject(mapObject: <a data-footnote-ref href="#user-content-fn-17">MapObject</a>, includeChildren: bool) -> <a data-footnote-ref href="#user-content-fn-17">MapObject</a></code></pre>
> Copy a map object
> 
<pre class="language-typescript"><code class="lang-typescript">function DestroyMapTargetable(targetable: <a data-footnote-ref href="#user-content-fn-18">MapTargetable</a>)</code></pre>
> Destroy a map targetable
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMesh()</code></pre>
> Update the nav mesh
> 
<pre class="language-typescript"><code class="lang-typescript">function UpdateNavMeshAsync()</code></pre>
> Update the nav mesh asynchronously
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
[^12]: [LineCastHitResult](../objects/LineCastHitResult.md)
[^13]: [LineRenderer](../objects/LineRenderer.md)
[^14]: [List](../objects/List.md)
[^15]: [Locale](../static/Locale.md)
[^16]: [Map](../static/Map.md)
[^17]: [MapObject](../objects/MapObject.md)
[^18]: [MapTargetable](../objects/MapTargetable.md)
[^19]: [Math](../static/Math.md)
[^20]: [Network](../static/Network.md)
[^21]: [NetworkView](../objects/NetworkView.md)
[^22]: [PersistentData](../static/PersistentData.md)
[^23]: [Physics](../static/Physics.md)
[^24]: [Player](../objects/Player.md)
[^25]: [Quaternion](../objects/Quaternion.md)
[^26]: [Random](../objects/Random.md)
[^27]: [Range](../objects/Range.md)
[^28]: [RoomData](../static/RoomData.md)
[^29]: [Set](../objects/Set.md)
[^30]: [Shifter](../objects/Shifter.md)
[^31]: [String](../static/String.md)
[^32]: [Time](../static/Time.md)
[^33]: [Titan](../objects/Titan.md)
[^34]: [Transform](../objects/Transform.md)
[^35]: [UI](../static/UI.md)
[^36]: [Vector2](../objects/Vector2.md)
[^37]: [Vector3](../objects/Vector3.md)
[^38]: [Object](../objects/Object.md)
[^39]: [Component](../objects/Component.md)
