# MapScript System Update Summary

## Changes Made

### 1. MapObjectPrefabMarker - Made Editor-Only ?
**File**: `Assets/Scripts/MapEditor/MapObjectPrefabMarker.cs`

- Wrapped entire class with `#if UNITY_EDITOR` directive
- Component is now completely stripped from builds
- Added comprehensive property support:
  - **Object Properties**: Active, Static, Visible, Networked
  - **Physics**: CollideMode, CollideWith, PhysicsMaterial
  - **Material**: Shader, Color, Texture, Tiling, Offset, ReflectColor
  - **Components**: Full component support

### 2. MapObjectPrefabMarkerEditor - Enhanced Inspector ?
**File**: `Assets/Editor/MapObjectPrefabMarkerEditor.cs`

- Added Physics settings section with dropdowns
- Added Material settings section with shader picker
- Added "Copy Values from Prefab Definition" button
- Shows/hides fields based on override checkboxes
- Proper UI with foldout sections
- Material parsing from JSON format

### 3. MapPrefabMarkerSync - New Sync Tool ?
**File**: `Assets/Editor/MapPrefabMarkerSync.cs` (NEW)

**Menu**: Tools > MapScript > Sync Markers on Map Prefabs

Features:
- **Apply Markers to All Prefabs**: Reads `MapPrefabList.json` and adds/updates markers on all Map prefabs
- **Remove All Markers**: Cleans up all markers from prefabs
- **Category filtering**: Sync only specific categories
- **Overwrite control**: Choose to preserve or overwrite existing markers
- **Progress logging**: Shows which prefabs were updated/skipped/failed

This tool ensures prefabs in `Assets/Resources/Map/` always match their JSON definitions.

### 4. ExportSceneToMapScript - Updated for New Marker System ?
**File**: `Assets/Editor/ExportSceneToMapScript.cs`

- Added conditional compilation for marker access
- Full support for marker's physics properties
- Full support for marker's material properties
- New `ApplyMarkerMaterial()` method for converting marker ? MapScript material
- Respects all override flags (Active, Static, Visible, Networked, Physics, Material)
- Falls back to scene extraction when markers not present

### 5. ImportMapScriptToScene - Updated for New Marker System ?
**File**: `Assets/Editor/ImportMapScriptToScene.cs`

- Added conditional compilation for marker creation
- Imports full physics properties to markers
- Imports material properties to markers
- New `ApplyMaterialToMarker()` method for converting MapScript ? marker material
- Creates markers with all properties when "Add Prefab Markers" is enabled

### 6. Documentation Updated ?

**QUICKSTART_MapScript.md** (NEW):
- Step-by-step workflow guide
- Common task examples
- Troubleshooting tips
- Layer reference guide

**README_MapScriptTools.md** (UPDATED):
- Added sync tool documentation
- Updated workflow to emphasize prefab sync
- Documented new properties (Physics, Material)
- Updated limitations and tips

## Workflow Changes

### Old Workflow:
1. Drag prefabs into scene
2. Manually add markers if needed
3. Export

### New Workflow:
1. **One-time**: Sync all Map prefabs with JSON ?
2. Drag prefabs into scene (already have markers)
3. Override properties only if needed
4. Export

## Key Benefits

? **Editor-only markers**: Zero impact on builds
? **JSON as source of truth**: Sync tool ensures consistency
? **Full property support**: Physics, materials, textures fully supported
? **Less manual work**: Prefabs come pre-configured
? **Easier maintenance**: Update JSON ? re-sync ? done

## Testing Checklist

- [x] Build compiles successfully
- [x] Markers stripped from builds (conditional compilation)
- [ ] Sync tool populates markers correctly
- [ ] Export respects all marker properties
- [ ] Import creates markers with all properties
- [ ] Round-trip maintains fidelity
- [ ] Physics properties export correctly
- [ ] Material properties export correctly
- [ ] Texture/tiling export correctly
- [ ] Component data preserved

## Migration Path for Existing Users

**No breaking changes!**

Existing scenes and prefabs will continue to work:
- Scenes without markers: Export extracts from Unity components
- Prefabs without markers: Still work, just add via sync tool
- Existing markers: Compatible (just missing new fields, which default)

To upgrade:
1. Run sync tool to add new properties to prefabs
2. Re-export scenes to include physics/material data

## API Reference

### MapObjectPrefabMarker

```csharp
#if UNITY_EDITOR
public class MapObjectPrefabMarker : MonoBehaviour
{
    // Identity
    string PrefabName;
    string CustomAsset;
    int ObjectId;

    // Object Properties
    bool OverrideActive; bool Active;
    bool OverrideStatic; bool Static;
    bool OverrideVisible; bool Visible;
    bool OverrideNetworked; bool Networked;

    // Physics
    bool OverridePhysics;
    string CollideMode; // Physical, Region, None
    string CollideWith; // All, Characters, Titans, etc.
    string PhysicsMaterial; // Default, Ice, Wood, etc.

    // Material
    bool OverrideMaterial;
    string MaterialShader; // Default, Transparent, Basic, etc.
    Color MaterialColor;
    string MaterialTexture;
    Vector2 MaterialTiling;
    Vector2 MaterialOffset;
    Color ReflectColor;

    // Components
    List<ComponentData> CustomComponents;
}
#endif
```

### MapPrefabMarkerSync Methods

```csharp
// Apply markers to all prefabs from JSON
void ApplyMarkersToAllPrefabs()

// Remove all markers from prefabs
void RemoveAllMarkers()

// Parse material string from JSON
void ParseMaterialString(MapObjectPrefabMarker marker, string materialString)
```

## Files Modified

1. `Assets/Scripts/MapEditor/MapObjectPrefabMarker.cs` - Made editor-only, added properties
2. `Assets/Editor/MapObjectPrefabMarkerEditor.cs` - Enhanced inspector
3. `Assets/Editor/ExportSceneToMapScript.cs` - Added marker property support
4. `Assets/Editor/ImportMapScriptToScene.cs` - Added marker property support
5. `Assets/Editor/README_MapScriptTools.md` - Updated documentation

## Files Created

1. `Assets/Editor/MapPrefabMarkerSync.cs` - Prefab sync tool
2. `Assets/Editor/QUICKSTART_MapScript.md` - Quick start guide

## Testing Commands

```csharp
// In Unity Editor Console:

// Test marker sync
MapPrefabMarkerSync window = EditorWindow.GetWindow<MapPrefabMarkerSync>();

// Test export
ExportSceneToMapScript exporter = EditorWindow.GetWindow<ExportSceneToMapScript>();

// Test import
ImportMapScriptToScene importer = EditorWindow.GetWindow<ImportMapScriptToScene>();
```
