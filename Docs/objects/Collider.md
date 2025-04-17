# Collider
Inherits from Object

## Initialization
<mark style="color:red;">This class is abstract and cannot be instantiated.</mark>

## Fields
|Field|Type|Readonly|Description|
|---|---|---|---|
|AttachedArticulationBody|[Transform](../objects/Transform.md)|True||
|ContactOffset|float|False|Contact offset value of this collider.|
|Enabled|bool|False|Enabled Colliders will collide with other Colliders, disabled Colliders won't.|
|ExludeLayers|int|False|The additional layers that this Collider should exclude when deciding if the Collider can contact another Collider.|
|includeLayers|int|False|The additional layers that this Collider should include when deciding if the Collider can contact another Collider.|
|IsTrigger|bool|False|Specify if this collider is configured as a trigger.|
|Center|[Vector3](../objects/Vector3.md)|True|The center of the bounding box.|
|ProvidesContacts|bool|False|Whether or not this Collider generates contacts for Physics.ContactEvent.|
|MaterialName|string|True|The name of the physics material on the collider.|
|SharedMaterialName|string|True|The name of the shared physics material on this collider.|
|Transform|[Transform](../objects/Transform.md)|True|The collider's transform.|
|GameObjectTransform|[Transform](../objects/Transform.md)|True|The transform of the gameobject this collider is attached to.|
## Methods
###### function <mark style="color:yellow;">ClosestPoint</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Returns a point on the collider that is closest to a given location.

###### function <mark style="color:yellow;">ClosestPointOnBounds</mark>(position: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> The closest point to the bounding box of the attached collider.

###### function <mark style="color:yellow;">Raycast</mark>(start: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, end: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, maxDistance: <mark style="color:blue;">float</mark>, collideWith: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[LineCastHitResult](../objects/LineCastHitResult.md)</mark>
> <mark style="color:red;">Missing description, please ping dev to fix this or if you need clarification :)</mark>

###### function <mark style="color:yellow;">\_\_Copy\_\_</mark>() → <mark style="color:blue;">Object</mark>
> Overrides the assignment operator to create a deep copy of the object.

###### function <mark style="color:yellow;">\_\_Eq\_\_</mark>(self: <mark style="color:blue;">Object</mark>, other: <mark style="color:blue;">Object</mark>) → <mark style="color:blue;">bool</mark>
> Overrides the equality comparison, used for == and != operators. Ex: a == b is equivalent to a.\_\_Eq\_\_(a, b)

###### function <mark style="color:yellow;">\_\_Hash\_\_</mark>() → <mark style="color:blue;">int</mark>
> Overrides hashing, used for GetHashCode - Used for Dictionaries/Sets. Ex: hash = obj.GetHashCode() is equivalent to hash = obj.\_\_Hash\_\_()


---

