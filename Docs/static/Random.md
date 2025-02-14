# Random
Inherits from object
## Methods
##### int RandomInt(int min, int max)
- **Description:** Generates a random integer between the specified range.
##### float RandomFloat(float min, float max)
- **Description:** Generates a random float between the specified range.
##### bool RandomBool()
- **Description:** Returns random boolean.
##### [Vector3](../objects/Vector3.md) RandomVector3([Vector3](../objects/Vector3.md) a, [Vector3](../objects/Vector3.md) b)
- **Description:** Generates a random Vector3 between the specified ranges.
##### [Vector3](../objects/Vector3.md) RandomDirection(bool flat = False)
- **Description:** Generates a random normalized direction vector. If flat is true, the y component will be zero.
##### int RandomSign()
- **Description:** Generates a random sign, either 1 or -1.
##### float PerlinNoise(float x, float y)
- **Description:** Returns a point sampled from generated 2d perlin noise. (see Unity Mathf.PerlinNoise for more information)

---

