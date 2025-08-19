# Collider
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
- `__Copy__`
- `==`
- `__Hash__`
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|AttachedArticulationBody|[Transform](../objects/Transform.md)|True||
|ContactOffset|float|False|Contact offset value of this collider.|
|Enabled|bool|False|Enabled Colliders will collide with other Colliders, disabled Colliders won't.|
|ExludeLayers|int|False|The additional layers that this Collider should exclude when deciding if the Collider can contact another Collider.|
|IncludeLayers|int|False|The additional layers that this Collider should include when deciding if the Collider can contact another Collider.|
|IsTrigger|bool|False|Specify if this collider is configured as a trigger.|
|Center|[Vector3](../objects/Vector3.md)|True|The center of the bounding box.|
|ProvidesContacts|bool|False|Whether or not this Collider generates contacts for Physics.ContactEvent.|
|MaterialName|string|True|The name of the physics material on the collider.|
|SharedMaterialName|string|True|The name of the shared physics material on this collider.|
|Transform|[Transform](../objects/Transform.md)|True|The collider's transform.|
|GameObjectTransform|[Transform](../objects/Transform.md)|True|The transform of the gameobject this collider is attached to.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> Returns a point on the collider that is closest to a given location.
> 
> **Returns**: The point on the collider that is closest to the specified location.
<pre class="language-typescript"><code class="lang-typescript">function ClosestPointOnBounds(position: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-37">Vector3</a></code></pre>
> The closest point to the bounding box of the attached collider.
> 
<pre class="language-typescript"><code class="lang-typescript">function Raycast(start: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, maxDistance: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-12">LineCastHitResult</a></code></pre>

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
