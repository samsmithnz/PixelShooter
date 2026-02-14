# Layered Pixel Grid System

This document explains how to use the layered pixel grid system in PixelShooter.

## Core Components

### Data Structures

1. **PixelColor** (Enum)
   - Defines the 8 colors available: Red, Blue, Yellow, Green, Orange, Purple, Black, White

2. **PixelLayer** (Class)
   - Represents a single pixel layer within a grid cell
   - Contains a color property

3. **GridCell** (Class)
   - Represents a single cell in the grid
   - Can contain up to 5 layers of pixels
   - Layers are stacked bottom-to-top
   - The topmost layer is the visible one
   - When destroyed, reveals the layer beneath

4. **GridData** (Class)
   - Represents the entire game grid
   - Default size: 8x8 cells
   - Configurable from 5x5 to 10x10+ cells
   - Provides methods to access cells, count pixels, and check grid state

5. **LevelDefinition** (ScriptableObject)
   - Editor-friendly way to define levels
   - Create new levels via: Assets > Create > PixelShooter > Level Definition
   - Configure grid size and pixel layers in the Unity Inspector

### Renderer Components

1. **PixelRenderer** (MonoBehaviour)
   - Renders a single pixel with the appropriate color
   - Automatically added by GridRenderer for each cell

2. **GridRenderer** (MonoBehaviour)
   - Manages rendering of the entire pixel grid
   - Requires a LevelDefinition and a pixel sprite
   - Automatically creates and positions pixel renderers

## Quick Setup Guide

### Creating a Level

1. **Create a Level Definition Asset**
   - Right-click in the Project window
   - Select Create > PixelShooter > Level Definition
   - Name it (e.g., "Level1")

2. **Configure the Level**
   - Select the level asset in the Project window
   - In the Inspector, set Grid Width and Grid Height (default: 8x8)
   - For each cell, add layers by expanding the gridCells array
   - Each layer is specified as a PixelColor enum value (0=Red, 1=Blue, etc.)
   - Layers are ordered bottom-to-top

3. **Add Grid to Scene** (Option 1: Menu)
   - Go to Tools > PixelShooter > Setup Grid Renderer
   - This will automatically create and configure a GridRenderer

4. **Add Grid to Scene** (Option 2: Manual)
   - Create an empty GameObject in your scene
   - Add the GridRenderer component
   - Assign your Level Definition asset
   - Assign the PixelSprite (found in Assets/Sprites/)
   - Set Cell Size (default: 1.0)
   - Set Cell Spacing (default: 0.1)

### Using the Grid at Runtime

```csharp
// Get the GridRenderer
GridRenderer gridRenderer = FindObjectOfType<GridRenderer>();

// Access grid data
GridData gridData = gridRenderer.GridData;

// Get a specific cell
GridCell cell = gridData.GetCell(x, y);

// Check if cell is empty
if (!cell.IsEmpty)
{
    // Get the current visible layer
    PixelLayer currentLayer = cell.CurrentLayer;
    PixelColor color = currentLayer.color;
}

// Destroy a pixel (reveals next layer)
gridRenderer.DestroyPixelAt(x, y);

// Count pixels of a specific color
int redPixelCount = gridData.CountPixelsOfColor(PixelColor.Red);

// Check if grid is empty (win condition)
bool isComplete = gridData.IsGridEmpty();
```

## Grid System Features

### Layering System
- Each cell can have 0-5 layers
- Layers are stacked bottom-to-top
- Only the topmost layer is visible
- Destroying a layer immediately reveals the one beneath
- Empty cells are transparent (no pixel rendered)

### No Gravity
- Pixels do not fall when pixels below them are destroyed
- Each cell maintains its layers independently
- This is a layering system, not a physics-based system

### Configurable Grid Sizes
- Default: 8x8 cells
- Minimum: 5x5 cells
- Maximum: 10x10+ cells (adjust via LevelDefinition)

### Color System
- 8 distinct colors for color-matching gameplay
- Colors are represented as enum values for type safety
- Colors are rendered using Unity's SpriteRenderer with color tinting

## Example Level Layout

Here's an example of a simple 3x3 level with multiple layers:

```
Row 0: [Red], [Blue, Red], [Yellow]
Row 1: [Green], [], [Orange]
Row 2: [Purple], [Black, White], [Blue]
```

In this example:
- Cell (1, 0) has 2 layers: Blue on top, Red underneath
- Cell (1, 1) is empty (no pixels)
- Cell (1, 2) has 2 layers: Black on top, White underneath

## Tips

1. **Testing Levels**: Use the editor setup helper to quickly test levels in the Unity Editor
2. **Visual Debugging**: The GridRenderer shows pixel layers in real-time
3. **Performance**: The system is optimized for grids up to 10x10 with 5 layers per cell
4. **Level Design**: Start with simple levels and gradually add complexity with more layers

## Future Enhancements

- Custom level editor UI
- Level validation tools
- Procedural level generation
- Pattern-based level templates
