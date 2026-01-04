# List
Inherits from [Object](../objects/Object.md)

Ordered collection of objects.

### Example
```csharp
values = List(1,2,1,4,115);

# Common generic list operations Map, Filter, Reduce, and Sort are supported.
# These methods take in a defined method with an expected signature and return type.

# Filter list to only unique values using set conversion.
uniques = values.ToSet().ToList();

# Accumulate values in list using reduce.
sum = values.Reduce(self.Sum2, 0);  # returns 123

# Filter list using predicate method.
filteredList = values.Filter(self.Filter);  # returns List(115)

# Transform list using mapping method.
newList = values.Map(self.TransformData);   # returns List(2,4,2,8,230)

function Sum2(a, b)
{
    return a + b;
}

function Filter(a)
{
    return a > 20;
}

function TransformData(a)
{
    return a * 2;
}
```
### Initialization
```csharp
List() // Creates an empty list.
List(parameterValues: Object) // Creates a list with the specified values.
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the list.|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clear all list elements.
> 
<pre class="language-typescript"><code class="lang-typescript">function Get(index: int) -> <a data-footnote-ref href="#user-content-fn-82">Object</a><T></code></pre>
> Get the element at the specified index.
> 
> **Parameters**:
> - `index`: The index of the element to get (negative indices count from the end).
> 
<pre class="language-typescript"><code class="lang-typescript">function Set(index: int, value: T)</code></pre>
> Set the element at the specified index.
> 
> **Parameters**:
> - `index`: The index of the element to set (negative indices count from the end).
> - `value`: The value to set.
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: T)</code></pre>
> Add an element to the end of the list.
> 
> **Parameters**:
> - `value`: The element to add.
> 
<pre class="language-typescript"><code class="lang-typescript">function InsertAt(index: int, value: T)</code></pre>
> Insert an element at the specified index.
> 
> **Parameters**:
> - `index`: The index at which to insert (negative indices count from the end).
> - `value`: The element to insert.
> 
<pre class="language-typescript"><code class="lang-typescript">function RemoveAt(index: int)</code></pre>
> Remove the element at the specified index.
> 
> **Parameters**:
> - `index`: The index of the element to remove (negative indices count from the end).
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: T)</code></pre>
> Remove the first occurrence of the specified element.
> 
> **Parameters**:
> - `value`: The element to remove.
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: T) -> bool</code></pre>
> Check if the list contains the specified element.
> 
> **Parameters**:
> - `value`: The element to check for.
> 
<pre class="language-typescript"><code class="lang-typescript">function Sort()</code></pre>
> Sort the list.
> 
<pre class="language-typescript"><code class="lang-typescript">function SortCustom(method: function)</code></pre>
> Sort the list using a custom method, expects a method with the signature int method(a,b).
> 
> **Parameters**:
> - `method`: The comparison method that returns an int: negative if a < b, 0 if a == b, positive if a > b.
> 
<pre class="language-typescript"><code class="lang-typescript">function Filter(method: function) -> <a data-footnote-ref href="#user-content-fn-4">List</a><T></code></pre>
> Filter the list using a custom method, expects a method with the signature bool method(element).
> 
> **Parameters**:
> - `method`: The predicate method that returns true for elements to keep.
> 
<pre class="language-typescript"><code class="lang-typescript">function Map(method: function) -> <a data-footnote-ref href="#user-content-fn-4">List</a><T></code></pre>
> Map the list using a custom method, expects a method with the signature object method(element).
> 
> **Parameters**:
> - `method`: The transformation method that returns the mapped value for each element.
> 
<pre class="language-typescript"><code class="lang-typescript">function Reduce(method: function, initialValue: T) -> <a data-footnote-ref href="#user-content-fn-82">Object</a></code></pre>
> Reduce the list using a custom method, expects a method with the signature object method(acc, element).
> 
> **Parameters**:
> - `method`: The accumulation method that combines the accumulator with each element.
> - `initialValue`: The initial accumulator value.
> 
<pre class="language-typescript"><code class="lang-typescript">function Randomize() -> <a data-footnote-ref href="#user-content-fn-4">List</a><T></code></pre>
> Returns a randomized version of the list.
> 
<pre class="language-typescript"><code class="lang-typescript">function ToSet() -> <a data-footnote-ref href="#user-content-fn-7">Set</a><T></code></pre>
> Convert the list to a set.
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
[^31]: [CharacterTypeEnum](../static/CharacterTypeEnum.md)
[^32]: [CollideModeEnum](../static/CollideModeEnum.md)
[^33]: [CollideWithEnum](../static/CollideWithEnum.md)
[^34]: [CollisionDetectionModeEnum](../static/CollisionDetectionModeEnum.md)
[^35]: [EffectNameEnum](../static/EffectNameEnum.md)
[^36]: [ForceModeEnum](../static/ForceModeEnum.md)
[^37]: [HandStateEnum](../static/HandStateEnum.md)
[^38]: [HumanParticleEffectEnum](../static/HumanParticleEffectEnum.md)
[^39]: [InputCategoryEnum](../static/InputCategoryEnum.md)
[^40]: [LanguageEnum](../static/LanguageEnum.md)
[^41]: [LoadoutEnum](../static/LoadoutEnum.md)
[^42]: [OutlineModeEnum](../static/OutlineModeEnum.md)
[^43]: [PhysicMaterialCombineEnum](../static/PhysicMaterialCombineEnum.md)
[^44]: [PlayerStatusEnum](../static/PlayerStatusEnum.md)
[^45]: [ProjectileNameEnum](../static/ProjectileNameEnum.md)
[^46]: [ScaleModeEnum](../static/ScaleModeEnum.md)
[^47]: [ShifterTypeEnum](../static/ShifterTypeEnum.md)
[^48]: [SliderDirectionEnum](../static/SliderDirectionEnum.md)
[^49]: [SteamStateEnum](../static/SteamStateEnum.md)
[^50]: [TeamEnum](../static/TeamEnum.md)
[^51]: [TitanTypeEnum](../static/TitanTypeEnum.md)
[^52]: [TSKillSoundEnum](../static/TSKillSoundEnum.md)
[^53]: [WeaponEnum](../static/WeaponEnum.md)
[^54]: [Camera](../static/Camera.md)
[^55]: [Cutscene](../static/Cutscene.md)
[^56]: [Game](../static/Game.md)
[^57]: [Input](../static/Input.md)
[^58]: [Locale](../static/Locale.md)
[^59]: [Map](../static/Map.md)
[^60]: [Network](../static/Network.md)
[^61]: [PersistentData](../static/PersistentData.md)
[^62]: [Physics](../static/Physics.md)
[^63]: [RoomData](../static/RoomData.md)
[^64]: [Time](../static/Time.md)
[^65]: [Button](../objects/Button.md)
[^66]: [Dropdown](../objects/Dropdown.md)
[^67]: [Icon](../objects/Icon.md)
[^68]: [Image](../objects/Image.md)
[^69]: [Label](../objects/Label.md)
[^70]: [ProgressBar](../objects/ProgressBar.md)
[^71]: [ScrollView](../objects/ScrollView.md)
[^72]: [Slider](../objects/Slider.md)
[^73]: [TextField](../objects/TextField.md)
[^74]: [Toggle](../objects/Toggle.md)
[^75]: [UI](../static/UI.md)
[^76]: [VisualElement](../objects/VisualElement.md)
[^77]: [Convert](../static/Convert.md)
[^78]: [Json](../static/Json.md)
[^79]: [Math](../static/Math.md)
[^80]: [Random](../objects/Random.md)
[^81]: [String](../static/String.md)
[^82]: [Object](../objects/Object.md)
[^83]: [Component](../objects/Component.md)
