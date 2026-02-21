# MapScript System - Accessibility Fix Summary

## Problem
MapScript-related classes were `internal` by default (no access modifier), making them inaccessible from editor scripts in the `Assets/Editor/` folder.

## Solution
Made all MapScript and utility classes `public` to enable editor tool access.

## Classes Changed to Public

### Map Namespace (MapScript System)
? `MapScript` - Main container
? `MapScriptBaseObject` - Base for all objects
? `MapScriptSceneObject` - Scene object definition
? `MapScriptOptions` - Map metadata
? `MapScriptObjects` - Object container
? `MapScriptCustomAssets` - Custom assets
? `MapScriptComponent` - Component definition
? `MapScriptBaseMaterial` - Base material
? `MapScriptBasicMaterial` - Basic/Transparent material
? `MapScriptDefaultTiledMaterial` - Tiled material
? `MapScriptReflectiveMaterial` - Reflective material

### Map Namespace (Constants in MapLoader.cs)
? `MapObjectShader` - Shader constants
? `MapObjectCollideMode` - Collision mode constants
? `MapObjectCollideWith` - Collision filter constants
? `MapObjectPhysicsMaterial` - Physics material constants

### Utility Namespace (CSV System)
? `BaseCSVObject` - Base serialization class
? `BaseCSVRow` - Row serialization
? `BaseCSVRowItem` - Row item serialization
? `BaseCSVContainer` - Container serialization
? `Color255` - 255-based color type

### Settings Namespace
?? `WeatherSet` - Left as internal (editor tools don't need weather access)
- Changed `MapScript.Weather` field to `internal` to maintain compatibility

## Files Modified

1. `Assets/Scripts/Map/MapScript/MapScript.cs` - Made class public, Weather field internal
2. `Assets/Scripts/Map/MapScript/MapScriptBaseObject.cs` - Made class public
3. `Assets/Scripts/Map/MapScript/MapScriptSceneObject.cs` - Made class public
4. `Assets/Scripts/Map/MapScript/MapScriptOptions.cs` - Made class public
5. `Assets/Scripts/Map/MapScript/MapScriptObjects.cs` - Made class public
6. `Assets/Scripts/Map/MapScript/MapScriptCustomAssets.cs` - Made class public
7. `Assets/Scripts/Map/MapScript/MapScriptComponent.cs` - Made class public
8. `Assets/Scripts/Map/MapScript/Materials/MapScriptBaseMaterial.cs` - Made class public
9. `Assets/Scripts/Map/MapScript/Materials/MapScriptBasicMaterial.cs` - Made class public
10. `Assets/Scripts/Map/MapScript/Materials/MapScriptDefaultTiledMaterial.cs` - Made class public
11. `Assets/Scripts/Map/MapScript/Materials/MapScriptReflectiveMaterial.cs` - Made class public
12. `Assets/Scripts/Utility/CSV/BaseCSVObject.cs` - Made class public
13. `Assets/Scripts/Utility/CSV/BaseCSVRow.cs` - Made class public
14. `Assets/Scripts/Utility/CSV/BaseCSVRowItem.cs` - Made class public
15. `Assets/Scripts/Utility/CSV/BaseCSVContainer.cs` - Made class public
16. `Assets/Scripts/Utility/Color255.cs` - Made class public
17. `Assets/Scripts/Map/MapLoader.cs` - Made static constant classes public

## Impact

### ? Positive
- Editor tools can now access all MapScript types
- No runtime impact - same behavior in builds
- Enables robust tooling ecosystem
- Other editor extensions can use MapScript API

### ?? Considerations
- More types are now part of the public API surface
- Future breaking changes to these types could affect editor tools
- WeatherSet remains internal - editor tools don't manipulate weather

## Testing
? Build compiles successfully
? Editor scripts can access all necessary types
? Runtime behavior unchanged

## Recommendation for Future

Consider creating a proper assembly definition setup:
- `AoTTG2.Runtime.asmdef` for Scripts folder
- `AoTTG2.Editor.asmdef` for Editor folder
- Explicit references between assemblies

This would allow finer control over visibility without making everything public.
