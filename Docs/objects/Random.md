# Random
Inherits from Object

## Initialization
> Random can be initialized as a class with a string given as the seed value.             Note that this is optional, and you can reference Random directly as a static class.
> Constructors:
```csharp

# Create an instance of Random with a seed of 123
generator = Random(123);
            
# Use it
a = generator.RandomInt(0, 100);
            
# Seed allows repeatable random values
generator2 = Random(123);
b = generator.RandomInt(0, 100);
compared = a == b;    # Always True
```
## Methods
###### function <mark style="color:yellow;">RandomInt</mark>(min: <mark style="color:blue;">int</mark>, max: <mark style="color:blue;">int</mark>) → <mark style="color:blue;">int</mark>
> Generates a random integer between the specified range.

###### function <mark style="color:yellow;">RandomFloat</mark>(min: <mark style="color:blue;">float</mark>, max: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">float</mark>
> Generates a random float between the specified range.

###### function <mark style="color:yellow;">RandomBool</mark>() → <mark style="color:blue;">bool</mark>
> Returns random boolean.

###### function <mark style="color:yellow;">RandomVector3</mark>(a: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, b: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Generates a random Vector3 between the specified ranges.

###### function <mark style="color:yellow;">RandomDirection</mark>(flat: <mark style="color:blue;">bool</mark> = <mark style="color:blue;">False</mark>) → <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>
> Generates a random normalized direction vector. If flat is true, the y component will be zero.

###### function <mark style="color:yellow;">RandomSign</mark>() → <mark style="color:blue;">int</mark>
> Generates a random sign, either 1 or -1.

###### function <mark style="color:yellow;">PerlinNoise</mark>(x: <mark style="color:blue;">float</mark>, y: <mark style="color:blue;">float</mark>) → <mark style="color:blue;">float</mark>
> Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)


---

