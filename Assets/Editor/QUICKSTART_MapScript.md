# MapScript Export/Import System - Quick Start

## ?? What This Does

Allows developers to create maps in Unity Editor while ensuring users can edit them in the in-game map editor. The system converts between Unity scenes and MapScript text format.

## ?? Prerequisites

All prefabs must be from:
- `Assets/Resources/Map/[Category]/Prefabs/`

Categories: General, Geometry, Buildings, Decor, Nature, Arenas, Terrain, FX, Interact, Legacy, MomoModels_*

## ?? Quick Start (3 Steps)

### Step 1: Sync Prefab Markers (One-Time Setup)

1. Go to **Tools > MapScript > Sync Markers on Map Prefabs**
2. Check "Overwrite Existing Markers"
3. Click **Apply Markers to All Prefabs**
4. Wait for sync to complete (~1-2 minutes)

? This adds `MapObjectPrefabMarker` to all Map prefabs with their default properties from JSON.

### Step 2: Build Your Map in Unity

1. Create a new scene
2. Drag prefabs from `Assets/Resources/Map/` into scene
3. Position, rotate, scale objects
4. Set GameObject properties:
   - **Name**: Descriptive name (becomes MapScript name)
   - **Active**: On/off state
   - **Static**: Performance optimization
   - **Layer**: For physics collision filtering

? Prefabs already have correct MapScript properties via markers!

### Step 3: Export to MapScript

1. Go to **Tools > Export Scene to MapScript**
2. Set file name
3. Click **Export Scene**
4. Save .txt file

? Done! Users can now edit this map in-game.

## ?? Round-Trip Editing

### Edit Existing User Map

1. **Tools > Import MapScript to Scene**
2. Browse to user's .txt file
3. Check "Add Prefab Markers"
4. Click **Import to Scene**
5. Edit in Unity
6. **Tools > Export Scene to MapScript**
7. Enable "Preserve Existing IDs"
8. Export

## ?? Customizing Objects

### Override Prefab Properties

1. Select object in scene
2. Find `MapObjectPrefabMarker` component in Inspector
3. Enable desired override checkboxes:
   - **Override Visible**: Hide object (still has collision)
   - **Override Networked**: Sync across multiplayer
   - **Override Physics**: Custom collision settings
   - **Override Material**: Custom colors/textures
4. Set values

### Add Custom Components

Example: Make a supply station

1. Add prefab: `Map/Interact/Prefabs/Supply1`
2. Already has `SupplyStation` component via marker ?
3. Export normally

Example: Make a kill region

1. Add: `Map/Geometry/Prefabs/Cube1`
2. Scale to desired kill zone
3. In marker component:
   - **Override Physics**: ?
   - **Collide Mode**: Region
   - **Collide With**: Characters
4. **Custom Components** ? **Add Component**:
   - **Component Name**: `KillRegion`
   - **Parameters**: (leave empty or add custom)
5. **Override Material**: ?
6. **Material Shader**: Transparent
7. **Material Color**: Red with 50% alpha
8. Export

### Change Colors/Materials

1. Select object
2. In `MapObjectPrefabMarker`:
   - **Override Material**: ?
   - **Material Shader**: Choose (Default, Transparent, etc.)
   - **Material Color**: Pick color + alpha
   - **Material Texture**: Texture path (e.g., "Misc/None")
   - **Material Tiling**: UV tiling (default 1, 1)
   - **Material Offset**: UV offset (default 0, 0)
3. Export

### Set Physics Properties

1. Select object
2. In `MapObjectPrefabMarker`:
   - **Override Physics**: ?
   - **Collide Mode**: Physical / Region / None
   - **Collide With**: All / Characters / Titans / Humans / etc.
   - **Physics Material**: Default / Ice / Wood / etc.
3. Set Unity layer to match (for editor visualization)
4. Export

## ??? Common Tasks

### Add Spawn Points

1. Drag `Map/General/Prefabs/EditorHumanSpawnPoint` into scene
2. Position where players spawn
3. Export ? (Already has Tag component)

### Add Barriers (Invisible Walls)

1. Drag `Map/Geometry/Prefabs/Cube1`
2. Scale to wall size
3. Select, change marker **Prefab Name** to "Barrier"
4. Export

### Add Lights

**Directional (sun):**
1. Drag `Map/General/Prefabs/EditorDaylight`
2. Rotate for sun angle
3. Export

**Point light:**
1. Drag `Map/General/Prefabs/EditorPointLight`
2. Position where light should be
3. Adjust Light component in Unity
4. Export

### Create Empty Containers

1. Create Empty GameObject (Right-click ? Create Empty)
2. Name it (e.g., "Buildings", "Trees_Group")
3. Parent other objects under it
4. Export

## ?? Layer Guide

Set Unity layers to match MapScript CollideWith:

- **Layer 24** = All
- **Layer 22** = Characters
- **Layer 29** = Titans
- **Layer 30** = Humans
- **Layer 21** = Projectiles
- **Layer 23** = Entities
- **Layer 20** = MapObjects
- **Layer 13** = Hitboxes
- **Layer 25** = MapEditor

## ?? Menu Commands

### Tools > MapScript >
- **Sync Markers on Map Prefabs**: Update all prefabs with JSON defaults
- **Add Prefab Marker to Selected**: Add marker to selected objects
- **Remove Prefab Marker from Selected**: Remove markers
- **Add Markers to All Scene Objects**: Batch add to entire scene

### Tools >
- **Export Scene to MapScript**: Convert scene to .txt
- **Import MapScript to Scene**: Convert .txt to scene

## ?? Pro Tips

? **Always sync markers first** - Run "Sync Markers on Map Prefabs" after updating JSON

? **Use prefabs, not primitives** - Drag from Resources/Map, not Create > 3D Object

? **Name hierarchically** - "Building_House_01", "Nature_Tree_Oak_05"

? **Test import/export early** - Verify round-trip works

? **Check warnings** - Review export warnings for mismatched prefabs

? **Use markers for special objects** - Supply stations, spawn points, interactive objects

? **Group with empties** - Use Empty Objects as folders

## ?? Important Notes

- **Markers are EDITOR-ONLY** - Stripped from builds automatically
- **Transforms**: Root = world space, Children = local space
- **Names**: Sanitized (no commas, pipes, colons)
- **Scale**: Always local scale
- **Static vs Dynamic**: Static = batched, Dynamic = animated
- **Networked**: Required for interactive/moving objects

## ?? Troubleshooting

**"Could not match GameObject to prefab"**
? Ensure object is from Resources/Map/ or add manual marker

**"Could not load asset"**
? Check asset path matches folder structure

**"Components missing after export"**
? Add CustomComponents in marker

**"Wrong colors/materials"**
? Enable Override Material in marker

**"Objects at wrong position"**
? Check if parent-child hierarchy is correct

**"IDs keep changing"**
? Enable "Preserve Existing IDs" on export

## ?? Examples

### Example 1: Simple Arena

```
Scene hierarchy:
??? Arena_Ground (Plane1)
??? Barriers (Empty Object)
?   ??? Barrier_North (Barrier)
?   ??? Barrier_South (Barrier)
?   ??? Barrier_East (Barrier)
?   ??? Barrier_West (Barrier)
??? Spawns (Empty Object)
?   ??? Human_Spawn_1 (Human SpawnPoint)
?   ??? Human_Spawn_2 (Human SpawnPoint)
??? Lights (Empty Object)
    ??? Sun (Daylight)
```

Export ? Users can edit spawn positions, add objects, etc.

### Example 2: Racing Track

```
Scene:
??? Track (Building5)
??? Start_Line (Racing Start Barrier)  ? marker with Tag component
??? Checkpoint_1 (Racing Checkpoint Region)  ? marker with RacingCheckpointRegion
??? Checkpoint_2 (Racing Checkpoint Region)
??? Finish_Line (Racing Finish Region)  ? marker with RacingFinishRegion
```

### Example 3: Kill Zone

```
Object: Lava_Pool
Prefab: Cube1
Scale: 50, 1, 50
Marker Settings:
  ??? Override Physics: ?
  ?   ??? Collide Mode: Region
  ?   ??? Collide With: Characters
  ??? Override Material: ?
  ?   ??? Shader: Transparent
  ?   ??? Color: Orange (255, 100, 0, 128)
  ?   ??? Texture: Misc/None
  ??? Custom Components:
      ??? KillRegion
```

## ?? Reference

- **Full docs**: `Assets/Editor/README_MapScriptTools.md`
- **Prefab list**: `Assets/Resources/Info/MapPrefabList.json`
- **MapScript format**: `Assets/Scripts/Map/MapScript/`
