# Physics
Inherits from [Object](../objects/Object.md)

Static Physics class. Contains some common physics functions

### Example
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
### Static Methods
<pre class="language-typescript"><code class="lang-typescript">function LineCast(start: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-12">LineCastHitResult</a></code></pre>
> Performs a line cast between two points, returns a LineCastHitResult object
> 
<pre class="language-typescript"><code class="lang-typescript">function SphereCast(start: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, end: <a data-footnote-ref href="#user-content-fn-37">Vector3</a>, radius: float, collideWith: string) -> <a data-footnote-ref href="#user-content-fn-38">Object</a></code></pre>
> Performs a sphere cast between two points, returns the object hit (Human, Titan, etc...).
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
