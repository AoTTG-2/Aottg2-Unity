# Physics
Inherits from object
## Initialization
> Physics class for custom logic.
```csharp
# Physics()
example = Physics()
```
> Example:
```csharp

start = Vector3(0);
end = Vector3(10);
result = Physics.LineCast(start, end, "Entities");
Game.Print(result.IsCharacter);
Game.Print(result.IsMapObject);
Game.Print(result.Point);
Game.Print(result.Normal);
Game.Print(result.Distance);
Game.Print(result.Collider);
```
## Static Methods
###### function <mark style="color:yellow;">LineCast</mark>(start: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, end: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, collideWith: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">[LineCastHitResult](../objects/LineCastHitResult.md)</mark>
> Performs a line cast between two points, returns a LineCastHitResult object

###### function <mark style="color:yellow;">SphereCast</mark>(start: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, end: <mark style="color:blue;">[Vector3](../objects/Vector3.md)</mark>, radius: <mark style="color:blue;">float</mark>, collideWith: <mark style="color:blue;">string</mark>) → <mark style="color:blue;">Object</mark>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).


---

