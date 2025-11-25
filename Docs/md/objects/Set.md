# Set
Inherits from [Object](../objects/Object.md)

Collection of unique elements

### Initialization
```csharp
Set()
Set(parameterValues: Object)
```

### Properties
|Name|Type|Readonly|Description|
|---|---|---|---|
|Count|int|True|The number of elements in the set|


### Methods
<pre class="language-typescript"><code class="lang-typescript">function Clear()</code></pre>
> Clear all set elements
> 
<pre class="language-typescript"><code class="lang-typescript">function Contains(value: <a data-footnote-ref href="#user-content-fn-59">Object</a>) -> bool</code></pre>
> Check if the set contains the specified element
> 
<pre class="language-typescript"><code class="lang-typescript">function Add(value: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Add an element to the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Remove(value: <a data-footnote-ref href="#user-content-fn-59">Object</a>)</code></pre>
> Remove the element from the set
> 
<pre class="language-typescript"><code class="lang-typescript">function Union(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>)</code></pre>
> Union with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Intersect(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>)</code></pre>
> Intersect with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Difference(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>)</code></pre>
> Difference with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSubsetOf(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set is a subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsSupersetOf(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set is a superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSubsetOf(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set is a proper subset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function IsProperSupersetOf(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set is a proper superset of another set
> 
<pre class="language-typescript"><code class="lang-typescript">function Overlaps(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set overlaps with another set
> 
<pre class="language-typescript"><code class="lang-typescript">function SetEquals(set: <a data-footnote-ref href="#user-content-fn-7">Set</a>) -> bool</code></pre>
> Check if the set has the same elements as another set
> 
<pre class="language-typescript"><code class="lang-typescript">function ToList() -> <a data-footnote-ref href="#user-content-fn-4">List</a></code></pre>
> Convert the set to a list
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
