# Random
Inherits from object
## Initialization
```csharp
# Random(Object[])
example = Random(Object[])
```
## Methods
#### function <span style="color:yellow;">RandomInt</span>(min: <span style="color:blue;">int</span>, max: <span style="color:blue;">int</span>) → <span style="color:blue;">int</span>
> Generates a random integer between the specified range.

#### function <span style="color:yellow;">RandomFloat</span>(min: <span style="color:blue;">float</span>, max: <span style="color:blue;">float</span>) → <span style="color:blue;">float</span>
> Generates a random float between the specified range.

#### function <span style="color:yellow;">RandomBool</span>() → <span style="color:blue;">bool</span>
> Returns random boolean.

#### function <span style="color:yellow;">RandomVector3</span>(a: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>, b: <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Generates a random Vector3 between the specified ranges.

#### function <span style="color:yellow;">RandomDirection</span>(flat: <span style="color:blue;">bool</span> = <span style="color:blue;">False</span>) → <span style="color:blue;">[Vector3](../objects/Vector3.md)</span>
> Generates a random normalized direction vector. If flat is true, the y component will be zero.

#### function <span style="color:yellow;">RandomSign</span>() → <span style="color:blue;">int</span>
> Generates a random sign, either 1 or -1.

#### function <span style="color:yellow;">PerlinNoise</span>(x: <span style="color:blue;">float</span>, y: <span style="color:blue;">float</span>) → <span style="color:blue;">float</span>
> Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)


---

