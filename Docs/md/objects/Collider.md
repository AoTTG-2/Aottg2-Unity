# Collider
Inherits from [Object](../objects/Object.md)
### Remarks
Overloads operators: 
`__Copy__`, `==`, `__Hash__`
### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|AttachedArticulationBody|[Transform](../objects/Transform.md)|True|The transform of the rigidbody this collider is attached to.|
|ContactOffset|float|False|The contact offset used by the collider to avoid tunneling.|
|Enabled|bool|False|Whether the collider is enabled.|
|ExludeLayers|int|False|The layers that this Collider should exclude when deciding if the Collider can contact another Collider.|
|IncludeLayers|int|False|The additional layers that this Collider should include when deciding if the Collider can contact another Collider.|
|IsTrigger|bool|False|Whether the collider is a trigger. Triggers don't cause physical collisions.|
|Center|[Vector3](../objects/Vector3.md)|True|The center of the collider's bounding box in world space.|
|ProvidesContacts|bool|False|Whether the collider provides contact information.|
|MaterialName|string|True|The name of the physics material on the collider.|
|SharedMaterialName|string|True|The name of the shared physics material on this collider.|
|Transform|[Transform](../objects/Transform.md)|True|The collider's transform.|
|GameObjectTransform|[Transform](../objects/Transform.md)|True|The transform of the gameobject this collider is attached to.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function ClosestPoint(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Gets the closest point on the collider to the given position. Returns: The closest point on the collider.
> 
> **Parameters**:
> - `position`: The position to find the closest point to.
> 
<pre class="language-typescript"><code class="lang-typescript">function ClosestPointOnBounds(position: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>) -> <a data-footnote-ref href="#user-content-fn-9">Vector3</a></code></pre>
> Gets the closest point on the collider's bounding box to the given position. Returns: The closest point on the bounding box.
> 
> **Parameters**:
> - `position`: The position to find the closest point on bounds to.
> 
<pre class="language-typescript"><code class="lang-typescript">function Raycast(start: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-9">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-3">LineCastHitResult</a></code></pre>
> Runs a raycast physics check between start to end and checks if it hits any collider with the given collideWith layer. Returns: A LineCastHitResult if it hit something, otherwise returns null.
> 
> **Parameters**:
> - `start`: The start position of the raycast.
> - `end`: The end position of the raycast.
> - `collideWith`: The layer name to check collisions with.
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
