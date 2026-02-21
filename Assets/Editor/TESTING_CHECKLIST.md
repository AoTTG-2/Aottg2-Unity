# MapScript System - Testing Checklist

## ? Compilation Tests
- [x] Build compiles without errors
- [x] No warnings about inaccessible types
- [x] MapObjectPrefabMarker properly wrapped with #if UNITY_EDITOR
- [x] All MapScript types accessible from editor scripts

## ?? Functionality Tests

### Prefab Sync Tool
- [ ] Open: Tools > MapScript > Sync Markers on Map Prefabs
- [ ] UI displays correctly
- [ ] Category dropdown populates
- [ ] "Apply Markers to All Prefabs" button works
- [ ] Loads MapPrefabList.json successfully
- [ ] Adds markers to all prefabs without errors
- [ ] Log shows processing details
- [ ] Markers have correct properties from JSON:
  - [ ] Physics (CollideMode, CollideWith, PhysicsMaterial)
  - [ ] Material (Shader, Color, Texture, Tiling, Offset)
  - [ ] Components
  - [ ] Flags (Static, Visible, Networked)
- [ ] "Remove All Markers" button works
- [ ] Category filtering works
- [ ] "Overwrite Existing Markers" toggle works

### Export Tool
- [ ] Create test scene with Map prefabs
- [ ] Open: Tools > Export Scene to MapScript
- [ ] UI displays correctly
- [ ] Export completes without errors
- [ ] Generated MapScript file is valid
- [ ] Check exported objects have correct:
  - [ ] Transforms (Position, Rotation, Scale)
  - [ ] Active state
  - [ ] Static flag
  - [ ] Physics properties (from marker)
  - [ ] Material properties (from marker)
  - [ ] Components (from marker)
- [ ] Parent-child hierarchy preserved
- [ ] Object IDs assigned correctly
- [ ] "Preserve Existing IDs" works

### Import Tool
- [ ] Open: Tools > Import MapScript to Scene
- [ ] Browse and load MapScript file
- [ ] UI displays correctly
- [ ] Import completes without errors
- [ ] Objects created in scene
- [ ] Check imported objects have:
  - [ ] Correct prefabs loaded
  - [ ] Correct transforms
  - [ ] Markers added (if "Add Prefab Markers" enabled)
  - [ ] Markers have all properties populated
  - [ ] Parent-child hierarchy restored
- [ ] "Preserve Existing Objects" works
- [ ] Log shows import details

### Round-Trip Test
- [ ] Create scene with varied objects
- [ ] Export to MapScript
- [ ] Clear scene
- [ ] Import MapScript
- [ ] Verify objects match original
- [ ] Export again
- [ ] Compare both MapScript files (should be identical)

### Marker Inspector
- [ ] Select object with MapObjectPrefabMarker
- [ ] Inspector displays custom UI
- [ ] Prefab Name dropdown works
- [ ] Override checkboxes toggle correctly
- [ ] Physics section shows all options
- [ ] Material section shows all options
- [ ] Material fields conditional on shader type
- [ ] Custom Components section works:
  - [ ] Add component
  - [ ] Remove component
  - [ ] Edit parameters
- [ ] "Copy Values from Prefab Definition" button works

## ?? Integration Tests

### Test Case 1: Simple Geometry
**Setup**:
1. Sync markers
2. Create scene
3. Add: Cube1, Cylinder1, Sphere1
4. Position randomly
5. Export

**Verify**:
- [ ] All objects in MapScript
- [ ] Correct Asset paths
- [ ] Transforms accurate

### Test Case 2: Physics Override
**Setup**:
1. Add Cube1
2. Set marker:
   - Override Physics ?
   - CollideMode: Region
   - CollideWith: Characters
3. Export

**Verify**:
- [ ] MapScript shows CollideMode: Region
- [ ] MapScript shows CollideWith: Characters

### Test Case 3: Material Override
**Setup**:
1. Add Cube1
2. Set marker:
   - Override Material ?
   - Shader: Transparent
   - Color: Red (255, 0, 0, 128)
   - Texture: Misc/None
   - Tiling: (2, 2)
3. Export

**Verify**:
- [ ] MapScript Material line shows Transparent shader
- [ ] Color: 255/0/0/128
- [ ] Tiling: 2/2

### Test Case 4: Custom Components
**Setup**:
1. Add Cube1
2. Add marker component:
   - ComponentName: KillRegion
   - Parameters: (empty)
3. Export

**Verify**:
- [ ] MapScript shows Components: KillRegion

### Test Case 5: Hierarchy
**Setup**:
1. Create Empty Object "Buildings"
2. Add child: House1
3. Add child: House2
4. Export

**Verify**:
- [ ] Parent object in MapScript (ID 0)
- [ ] Children have Parent: 0
- [ ] Children use local transforms

### Test Case 6: Interactive Object
**Setup**:
1. Sync markers (loads SupplyStation component)
2. Add Supply1 prefab
3. Position at (10, 0, 10)
4. Export

**Verify**:
- [ ] MapScript includes SupplyStation component
- [ ] Component has correct default parameters

### Test Case 7: ID Preservation
**Setup**:
1. Export scene (Preserve IDs: OFF)
2. Import back (Add Markers: ON)
3. Move one object
4. Export (Preserve IDs: ON)

**Verify**:
- [ ] Object IDs match between exports
- [ ] Moved object has new position but same ID

## ?? Edge Cases

- [ ] Export scene with no objects
- [ ] Export scene with only Empty Objects
- [ ] Import MapScript with invalid asset paths
- [ ] Import MapScript with missing parent IDs
- [ ] Sync tool with missing MapPrefabList.json
- [ ] Sync tool with invalid JSON format
- [ ] Marker on object that isn't a Map prefab
- [ ] Multiple markers on same object (shouldn't happen but test)
- [ ] Nested prefabs (deep hierarchy)
- [ ] Prefabs with (Clone) in name

## ?? Performance Tests

- [ ] Sync 100+ prefabs completes in <30 seconds
- [ ] Export scene with 500+ objects completes in <10 seconds
- [ ] Import MapScript with 500+ objects completes in <15 seconds
- [ ] No memory leaks during sync/export/import

## ??? Build Tests

- [ ] Build with markers on scene objects ? markers removed ?
- [ ] Build with markers on prefabs ? markers removed ?
- [ ] No MapObjectPrefabMarker in final build
- [ ] Build size not increased by markers
- [ ] Runtime code doesn't reference MapObjectPrefabMarker

## ?? Regression Tests

- [ ] Existing maps still load correctly
- [ ] MapLoader runtime code unchanged
- [ ] BuiltinMapPrefabs still works
- [ ] In-game map editor not affected
- [ ] Multiplayer map sync not affected

## Status: Ready for Testing

All code changes complete. Build successful. Ready for QA testing.
