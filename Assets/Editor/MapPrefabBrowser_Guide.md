# Map Prefab Browser - User Guide

## Overview

The **Map Prefab Browser** is a dockable Unity Editor window that provides an organized, searchable interface for browsing and adding Map prefabs to your scenes. It's designed to mimic the in-game map editor's add object menu but optimized for Unity Editor workflow.

## Opening the Browser

**Menu**: Window > Map Prefab Browser

**Recommended Layout**: Dock it next to your Hierarchy or Project window for quick access.

## Features

### Category Organization
- Browse prefabs organized by category:
  - **All** - View all prefabs at once
  - **General** - Spawn points, barriers, lights, references
  - **Geometry** - Basic shapes and primitives
  - **Buildings** - Houses, towers, walls, structures
  - **Nature** - Trees, rocks, terrain features
  - **Decor** - Props, furniture, decorative elements
  - **Arenas** - Pre-built arena layouts
  - **Terrain** - Large terrain pieces
  - **FX** - Particle effects and special effects
  - **Interact** - Interactive objects (supplies, cannons, etc.)
  - **Custom** - Custom/mod prefabs

- Each category shows prefab count: `Buildings (47)`

### Search
- Type in toolbar search box to filter across all categories
- Searches: prefab names, categories, and asset paths
- Clear search with X button

### Prefab Display Options

**Toolbar Buttons:**
- **Refresh**: Reload prefab database from JSON (also clears preview cache)
- **Grid**: Toggle between grid view and list view
- **Size**: Slider to adjust tile size (60-150px, only in grid mode)
- **Preview**: Toggle preview thumbnails (ON/OFF)
- **Variants**: Show/hide prefab variants (ON/OFF)
- **Hidden**: Show/hide hidden prefabs (ON/OFF)

**View Modes:**
- **Grid View** (recommended): Shows prefabs as tiles with large previews
  - Adjustable tile size
  - Click or drag from tile
  - Tooltips show full info
  - Color-coded backgrounds
- **List View**: Traditional list with optional small previews
  - Shows more metadata inline
  - Better for scanning names
  - Variants indent under base prefabs

### Visual Indicators

Prefabs are color-coded:
- **Blue tint**: Networked objects (sync across multiplayer)
- **Yellow tint**: Invisible objects (hidden but still have collision)
- **[Hidden] tag**: Hidden prefabs (development/testing only)

### Preview Icons
- Shows 3D preview thumbnails when enabled
- **Uses pre-generated previews** from `Assets/Resources/Map/Previews/` (instant loading!)
- Falls back to Unity's AssetPreview system if preview image doesn't exist
- Automatically generated asynchronously for new prefabs
- Cached for performance
- Click "Refresh" to regenerate if needed

**Note**: Most prefabs have pre-generated preview images for instant display. New prefabs may take a moment as Unity generates their preview.

### Variant Support
- Prefabs with variants show a **> N** button in the top-right corner (grid) or arrow on left (list)
- **Click the arrow** to expand and show variants as a submenu/folder
- Variants appear as tiles below the base prefab (grid) or indented list items (list)
- Variants have **+** prefix in their names
- Click arrow again to collapse variants
- Toggle "Variants" button in toolbar to show/hide variant system entirely

**Example**: House has House1, House2, House3
- Base tile shows "House" with "> 3" button
- Click arrow -> expands to show House1, House2, House3 tiles below
- Click House2 -> adds House2 to scene
- Click arrow again -> collapses back to just House

## Adding Prefabs to Scene

### Method 1: Click to Add
1. Browse or search for prefab
2. Click prefab button
3. Prefab appears in front of Scene View camera

### Method 2: Drag & Drop
1. Click and drag prefab name
2. Drop into Scene View
3. Prefab appears at mouse cursor position (raycasts to surface)

### Method 3: Context Menu
1. Right-click on prefab
2. Choose "Add to Scene" or "Add Multiple..."

## Context Menu (Right-Click)

- **Add to Scene**: Standard add (same as left-click)
- **Add Multiple...**: Create grid of instances (enter count 1-100)
- **Show Info**: Display full prefab properties
- **Ping Prefab Asset**: Highlight in Project window
- **Copy Prefab Name**: Copy to clipboard
- **Copy Asset Path**: Copy full asset path

## Add Multiple Feature

Creates a grid layout of instances:
1. Right-click prefab
2. Select "Add Multiple..."
3. Enter number of instances (1-100)
4. Grid appears in front of camera with 5-unit spacing
5. All instances selected for easy manipulation

Useful for:
- Creating walls/fences
- Placing trees/props
- Building grids/arrays

## Resizable Layout

- Drag the vertical divider between categories and prefabs
- Adjust category panel width (100px - window width - 200px)
- Layout saved per session

## Prefab Information

Click the **?** button to see:
- Full name and category
- Asset path
- Object type
- Variant information
- Properties: Static, Visible, Networked, Hidden
- Physics: CollideMode, CollideWith
- Components list

## Workflow Examples

### Example 1: Building an Arena
```
1. Open Map Prefab Browser
2. Select "Buildings" category
3. Search "wall"
4. Drag wall pieces into Scene View
5. Right-click House1 > Add Multiple > 5
6. Arrange in circle
7. Switch to "General" category
8. Click spawn points to add
```

### Example 2: Creating a Forest
```
1. Search "tree"
2. Right-click Tree1 > Add Multiple > 20
3. Select all instances
4. Use Unity's random rotation/scale tools
5. Position individually
```

### Example 3: Interactive Course
```
1. "Interact" category
2. Add Supply1 for resupply
3. Add SupplyStation1 for large supply
4. "Geometry" category
5. Search "cube"
6. Add Cube1, configure as kill zone
```

## Tips

? **Dock for efficiency** - Keep browser open while building

? **Use search** - Fastest way to find specific prefabs

? **Preview mode** - Helps identify prefabs visually

? **Right-click exploration** - Discover prefab properties

? **Drag for precision** - Place directly where you want

? **Add Multiple** - Faster than duplicating manually

? **Ping Asset** - Find prefab in Project window for customization

? **Color coding** - Quickly identify special objects

## Keyboard Shortcuts

- **Click**: Add to scene (in front of camera)
- **Drag**: Add at mouse position
- **Right-click**: Context menu
- **Enter** (in search): Focus first result

## Performance

- **Lazy loading**: Previews loaded on-demand
- **Cached**: Thumbnails cached for speed
- **Instant search**: Real-time filtering
- **Lightweight**: Minimal memory footprint

## Integration with MapScript Tools

Works seamlessly with:
- Export tool reads prefab markers
- Sync tool keeps prefabs updated
- Import tool creates compatible objects
- All prefabs pre-configured with markers

## Customization

### Adjusting Preview Size
Edit line in code:
```csharp
private float _previewSize = 50f; // Change to 40-80
```

### Adding Custom Categories
Categories load automatically from MapPrefabList.json

### Filtering Options
Use toolbar toggles:
- Preview: Show/hide thumbnails
- Variants: Show/hide variant prefabs
- Hidden: Show/hide development prefabs

## Troubleshooting

**"No prefabs found"**
- Run Tools > MapScript > Sync Markers on Map Prefabs
- Check Data/Info/MapPrefabList.json exists
- Click Refresh button

**"Could not load prefab"**
- Verify prefab exists at asset path
- Check Resources/Map/[Category]/Prefabs/
- Look at error log for specific path

**Drag & drop not working**
- Make sure you're dragging from prefab name, not background
- Try clicking instead

**Previews not showing**
- Unity generates previews async - wait a moment (window auto-refreshes)
- Some prefabs don't have visual components (Empty Objects) - show "?" icon
- Click Refresh to regenerate preview cache
- Previews generate on first view - may take 10-30 seconds for all prefabs
- Check Console for "Could not load prefab" errors

**Categories empty**
- Check MapPrefabList.json is valid JSON
- Look for parsing errors in Console
- Click Refresh to reload

## See Also

- `README_MapScriptTools.md` - Full system documentation
- `QUICKSTART_MapScript.md` - Quick start guide
- `SUMMARY_MapScript.md` - System summary
