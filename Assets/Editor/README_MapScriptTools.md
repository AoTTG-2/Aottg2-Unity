# Unity Scene to MapScript Export/Import Tools

## Overview
These tools allow developers to create maps using Unity's editor and export them to the MapScript format that users can edit in-game. You can also import MapScript files back into Unity for round-trip editing.

## Quick Start

**See `QUICKSTART_MapScript.md` for the fastest way to get started!**

## Menu Commands

### Window Menu
- **Window > Map Prefab Browser** - Dockable prefab browser with categories, search, and drag-drop support

### Tools > MapScript Menu
- **Sync Markers on Map Prefabs** - Synchronize prefab markers with JSON definitions
- **Add Prefab Marker to Selected** - Manually add markers to selected objects
- **Remove Prefab Marker from Selected** - Remove markers from selected objects

### Tools Menu
- **Export Scene to MapScript** - Export current scene to MapScript format
- **Import MapScript to Scene** - Import MapScript file into current scene

## Map Prefab Browser

A dockable editor window for browsing and adding Map prefabs to scenes, similar to the in-game add object menu.

**Features:**
- **Category Organization**: Browse by General, Geometry, Buildings, Nature, etc.
- **Search**: Filter prefabs by name, category, or asset path
- **Variants**: Shows prefab variants grouped under base prefabs
- **Preview Icons**: Visual thumbnails for quick identification
- **Drag & Drop**: Drag prefabs directly into Scene View
- **Context Menu**: Right-click for options (Add Multiple, Show Info, Ping Asset, Copy Name/Path)
- **Visual Indicators**: Color-coded for networked (blue) and invisible (yellow) objects
- **Resizable**: Adjust category panel width to your preference

**Usage:**
1. Open: **Window > Map Prefab Browser**
2. Dock it next to Project window or Hierarchy
3. Select category or search for prefabs
4. Click to add to scene, or drag into Scene View
5. Right-click for more options

**Keyboard Shortcuts:**
- Click + Drag = Move prefab into scene at mouse position
- Right Click = Context menu
- Double-click Info (?) = Show detailed properties

## Files Created
- **Assets/Editor/ExportSceneToMapScript.cs** - Export Unity scenes to MapScript format
- **Assets/Editor/ImportMapScriptToScene.cs** - Import MapScript files into Unity scenes
- **Assets/Scripts/MapEditor/MapObjectPrefabMarker.cs** - EDITOR-ONLY component for marking GameObjects with MapScript metadata
- **Assets/Editor/MapObjectPrefabMarkerEditor.cs** - Custom inspector for MapObjectPrefabMarker
- **Assets/Editor/MapPrefabMarkerSync.cs** - Tool to synchronize markers on all Map prefabs with JSON definitions
- **Assets/Editor/QUICKSTART_MapScript.md** - Quick start guide

## Important: Prefab Marker System

The `MapObjectPrefabMarker` component is **EDITOR-ONLY** and uses conditional compilation to ensure it's stripped from builds. All prefabs should be pre-configured with markers using the sync tool.

### First-Time Setup

1. Go to **Tools > MapScript > Sync Markers on Map Prefabs**
2. Enable "Overwrite Existing Markers"
3. Click **Apply Markers to All Prefabs**
4. Wait for sync to complete

This loads all default properties from `MapPrefabList.json` onto the prefabs, including:
- Physics settings (CollideMode, CollideWith, PhysicsMaterial)
- Material properties (Shader, Color, Texture, Tiling)
- Default components
- Visibility and networking flags

### Refreshing After JSON Changes

Whenever you update `MapPrefabList.json`:
1. Open **Tools > MapScript > Sync Markers on Map Prefabs**
2. Click **Apply Markers to All Prefabs**
3. Markers on all prefabs will update to match JSON

### Removing Markers

To clean up prefabs:
1. Open **Tools > MapScript > Sync Markers on Map Prefabs**
2. Click **Remove All Markers**
3. Confirm

Note: You'll need to re-sync before exporting again.

## Workflow for Creating Maps

### Recommended Workflow: Prefab-Based

1. **Setup** (one time): Sync markers on all Map prefabs
2. **Build**: Drag prefabs from `Assets/Resources/Map/` into scene
3. **Arrange**: Position, rotate, scale in Unity
4. **Customize**: Override properties in marker component if needed
5. **Export**: Tools > Export Scene to MapScript
6. **Test**: Load in-game and let users edit

Benefits:
- ? All physics, materials, components pre-configured
- ? No manual marker management needed
- ? JSON is the source of truth
- ? Prefabs stay in sync with game logic

## Using MapObjectPrefabMarker

Markers are automatically added to prefabs during sync. You rarely need to manually add them to scene objects unless you want to override defaults.

### Overriding Prefab Defaults

After placing a prefab in scene, you can override its properties:

1. Select object in Hierarchy
2. Find `MapObjectPrefabMarker` component in Inspector
3. Enable override checkboxes:
   - **Override Active**: Control active state
   - **Override Static**: Change static flag
   - **Override Visible**: Hide object (keeps collision)
   - **Override Networked**: Change multiplayer sync
   - **Override Physics**: Custom collision behavior
   - **Override Material**: Custom colors/textures
4. Set custom values
5. Export normally

### Adding Custom Components

For interactive objects:

1. Select object
2. Expand **Custom Components** section
3. Click **Add Component**
4. Set **Component Name** (e.g., "KillRegion", "SupplyStation")
5. Add **Parameters** if needed (format: `Key:Value`)

Example: Make a custom spawn point
```
Component Name: Tag
Parameters:
  - Name:CustomSpawnPoint
  - Team:Blue
```

### Manual Marker Management (Advanced)

**Add markers to selected objects:**
- **Tools > MapScript > Add Prefab Marker to Selected**

**Remove markers:**
- **Tools > MapScript > Remove Prefab Marker from Selected**

**Sync all scene objects:**
- **Tools > MapScript > Add Markers to All Scene Objects**

## Prefab Matching

The exporter tries to match GameObjects to MapScript prefabs using:

1. **MapObjectPrefabMarker.PrefabName** (if present) - Most reliable
2. **Prefab source asset name** - Matches against Asset field in JSON
3. **GameObject name** - Matches against Name field in JSON
4. **Component detection** - Lights are auto-detected
5. **Empty Object fallback** - Objects without matches become Empty Objects

## Supported Prefabs

All prefabs from `Info/MapPrefabList.json` are supported, organized in categories:
- **General**: Barriers, spawn points, lights, references
- **Geometry**: Basic shapes and geometric primitives
- **Buildings**: Houses, towers, walls, structures
- **Decor**: Props, furniture, decorative elements
- **Nature**: Trees, rocks, terrain features
- **Arenas**: Pre-built arena layouts
- **Terrain**: Large terrain pieces
- **FX**: Particle effects and special effects
- **Interact**: Interactive objects (supplies, cannons, etc.)
- **Legacy**: Backwards-compatible objects
- **MomoModels**: Community-created models

## Custom Components

MapScript supports custom components for interactive objects. These are defined in BaseLogic.txt and include:

Common components:
- **Tag** - Tagging system for spawn points and special objects
- **Daylight** - Directional lighting
- **PointLight** - Point light source
- **SupplyStation** - Gas/blade supply
- **Cannon** - Player-controllable cannon
- **Rigidbody** - Physics simulation
- **KillRegion** - Kills players who enter
- **DamageRegion** - Damages players who enter
- **RacingCheckpointRegion** - Racing checkpoint
- **RacingFinishRegion** - Racing finish line

To add custom components:
1. Add MapObjectPrefabMarker to your GameObject
2. In the Inspector, expand **Custom Components**
3. Click **Add Component**
4. Set **Component Name** and **Parameters**

Parameters use the format: `Key:Value` (e.g., `Name:HumanSpawnPoint`)

## Transform Handling

- **Root objects**: World space position/rotation, local scale
- **Child objects**: Local space for all transforms (relative to parent)
- **Scale**: Applied as local scale (will be multiplied by prefab's base scale at runtime)

## Material System

Materials can be fully customized via `MapObjectPrefabMarker`:
- **Shader types**: Default, DefaultNoTint, DefaultTiled, Basic, Transparent, Reflective
- **Color**: RGBA color with full alpha support
- **Texture**: Path to texture in Resources (e.g., "Misc/None")
- **Tiling/Offset**: UV mapping control
- **Reflect Color**: For reflective materials

The exporter uses marker material settings if **Override Material** is enabled, otherwise attempts to extract from Unity materials.

## Physics System

Physics can be fully controlled via `MapObjectPrefabMarker`:
- **CollideMode**: Physical, Region (trigger), None
- **CollideWith**: All, MapObjects, Characters, Titans, Humans, Projectiles, Entities, Hitboxes, MapEditor
- **PhysicsMaterial**: Default, Ice, Wood, etc.

If no marker override, physics is extracted from:
- **Collider.enabled** ? CollideMode (None if disabled)
- **Collider.isTrigger** ? CollideMode (Region vs Physical)
- **GameObject.layer** ? CollideWith (determines what it collides with)
- **Collider.sharedMaterial** ? PhysicsMaterial (e.g., Ice)

## Component System

All MapScript components can be added via markers:
- **Tag** - Tagging for spawn points and special objects
- **Daylight** - Directional lighting
- **PointLight** - Point light source
- **SupplyStation** - Gas/blade resupply
- **Cannon** - Player-controllable cannon
- **Rigidbody** - Physics simulation
- **KillRegion** - Kills players on contact
- **DamageRegion** - Damages players
- **RacingCheckpointRegion** - Racing checkpoint
- **RacingFinishRegion** - Racing finish line
- And many more...

Components from prefab JSON are automatically included. Additional components can be added in the marker's **Custom Components** section.

## Limitations

1. **Must use Resources/Map prefabs**: Custom asset bundles require manual marker setup
2. **Editor-only markers**: Markers are stripped from builds (by design)
3. **Material matching**: Automatic extraction is best-effort; use markers for precision
4. **Component detection**: Limited without markers; add components via marker for export

## Tips

- **Sync first**: Always run "Sync Markers on Map Prefabs" after JSON updates
- **Use prefab instances**: Drag from Resources/Map, not Create > 3D Object primitives
- **Name objects clearly**: Object names become MapScript names (sanitized)
- **Test round-trip**: Import ? Edit ? Export ? Test to verify fidelity
- **Preserve IDs**: Use "Preserve Existing IDs" for iterative editing
- **Override sparingly**: Only override marker properties when needed
- **Check warnings**: Review warnings after export for issues
- **Use hierarchies**: Group objects with Empty Object parents for organization

## Troubleshooting

**Objects not exporting:**
- Check if they're inactive (enable "Include Inactive Objects")
- Verify they're not system objects (Camera, EventSystem, etc.)
- Ensure they're from Resources/Map/ or have markers

**Wrong prefab detected:**
- Run "Sync Markers on Map Prefabs" to refresh
- Check that prefab exists in MapPrefabList.json
- Verify prefab asset path matches folder structure

**Materials/Physics wrong after export:**
- Open prefab, check marker component
- Enable appropriate Override checkboxes
- Set desired values
- Re-export scene

**Components missing:**
- Check if component is in prefab JSON definition
- If not, add manually to marker's CustomComponents
- Verify component name matches BaseLogic.txt definitions

**Markers visible in build:**
- ? This is impossible - markers use conditional compilation
- They exist only in editor and are automatically excluded from builds

## Advanced Usage

### Syncing Specific Categories

In the sync tool, you can filter by category:
1. Open **Tools > MapScript > Sync Markers on Map Prefabs**
2. Set **Filter Category** dropdown
3. Only that category will be synced

### Custom Asset Override
For objects using custom asset bundles:
1. Select prefab or scene object
2. Find MapObjectPrefabMarker component
3. Set **Custom Asset** field to asset bundle path (e.g., "Custom/MyBundle/MyAsset")
4. Export normally

### Preserving IDs Across Exports
1. First export with "Preserve Existing IDs" OFF
2. Import back to scene (adds markers with IDs to scene instances)
3. Make edits
4. Export with "Preserve Existing IDs" ON
5. IDs remain stable for networking/logic references

### Batch Processing Map Prefabs
```csharp
// Get all prefabs in a category
string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/Map/Buildings/Prefabs" });
foreach (string guid in guids)
{
    string path = AssetDatabase.GUIDToAssetPath(guid);
    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
    
    var marker = prefab.GetComponent<MapObjectPrefabMarker>();
    if (marker != null)
    {
        // Modify marker properties
        marker.OverridePhysics = true;
        marker.CollideMode = "Physical";
        
        PrefabUtility.SavePrefabAsset(prefab);
    }
}
```

### Programmatic Export
Create custom editor scripts using the core functionality:
```csharp
// Export programmatically
var exporter = CreateInstance<ExportSceneToMapScript>();
// Configure and call ExportScene()
```

## See Also

- `Assets/Scripts/Map/BuiltinMapPrefabs.cs` - Prefab database loading
- `Assets/Scripts/Map/MapLoader.cs` - Runtime MapScript loading
- `Assets/Scripts/Map/MapScript/` - MapScript data structures
- `Assets/Resources/Data/Info/MapPrefabList.json` - Prefab definitions
- `Assets/Resources/Data/Modes/BaseLogic.txt` - Component definitions
- `Assets/Resources/Map/` - Map prefab storage
