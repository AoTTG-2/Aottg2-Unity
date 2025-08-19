# Range

Range allows you to create lists of integers for convenient iteration, particularly in for loops.

### Example
```csharp
for (a in Range(10))
{
    Game.Print(a);
}
    
for (a in Range(1, 10))
{
    Game.Print(a);
}
    
for (a in Range(1, 10, 2))
{
    Game.Print(a);
}
```
### Initialization
```csharp
Range(end: int) // Creates a range from 0 to end-1
Range(start: int, end: int) // Creates a range from start to end-1
Range(start: int, end: int, step: int) // Creates a range from start to end-1 with the specified step
```

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
