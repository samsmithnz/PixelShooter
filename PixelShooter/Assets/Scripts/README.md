# Shooter System Implementation

This folder contains the core implementation of the PixelShooter game mechanics as defined in the Game Design Document (GDD).

## Architecture Overview

The shooter system is organized into three main namespaces:

### 1. `PixelShooter.Data` - Core Data Structures
Contains fundamental data types used throughout the game:

- **PixelColor.cs** - Enum defining the 8 supported colors (Red, Blue, Yellow, Green, Orange, Purple, Black, White)
- **GridPosition.cs** - Struct representing a position in the grid (row, column)
- **Pixel.cs** - Class representing a single colored pixel with position and layer information
- **GridCell.cs** - Class managing a single grid cell that can contain up to 5 pixel layers

### 2. `PixelShooter.Core` - Core Game Systems
Contains the main game logic systems:

- **GridSystem.cs** - Manages the 8x8 grid of cells and all grid operations
  - Handles pixel addition and removal
  - Validates positions
  - Tracks grid state and color counts
  
- **ShooterManager.cs** - Manages shooter lifecycle and game state
  - Ensures only one shooter is active at a time
  - Handles shooter activation and completion
  - Detects win conditions
  - Provides events for game state changes

- **ShooterSystemDemo.cs** - MonoBehaviour for testing and validation
  - Contains 8 automated tests validating all requirements
  - Can be attached to any GameObject to run tests in Unity Editor

### 3. `PixelShooter.Gameplay` - Gameplay Mechanics
Contains gameplay-specific logic:

- **Shooter.cs** - Core shooter behavior implementation
  - Movement: Left-to-right along bottom row
  - Line-of-sight: Vertical detection with blocker handling
  - Firing: Automatic when matching pixel is in sight
  - Ball management: Tracks and consumes projectiles

## Key Features Implemented

### Shooter Movement
- Moves left-to-right along the bottom row (last row of grid)
- Starts at column -1 (before first column)
- Advances one column at a time
- Stops when reaching the end of the grid OR when ammo reaches zero

### Vertical Line-of-Sight Firing
- Searches vertically from current column, top to bottom
- Fires ONLY at matching-color pixels
- Non-matching pixels block line of sight
- Destroys pixels and reveals layers beneath
- Continues firing at same column if multiple layers exist

### Ball/Ammo Management
- Each shooter has a specific number of balls (projectiles)
- One ball consumed per successful hit
- Shooter stops mid-path if balls reach zero
- Initial ball count typically equals total matching pixels in grid

### Edge Cases Handled
1. **No Valid Targets**: Shooter completes path without firing, preserving all balls
2. **Last Pixel Win**: Destroying the final pixel triggers immediate win condition
3. **Out of Balls Mid-Path**: Shooter stops immediately when ammo depletes
4. **Line-of-Sight Blocking**: Different-colored pixels block firing at matching pixels behind them
5. **Multiple Layers**: Destroying one layer reveals the next, potentially allowing multiple shots in same column

## Usage Example

```csharp
using PixelShooter.Core;
using PixelShooter.Data;
using PixelShooter.Gameplay;

// Create an 8x8 grid
GridSystem grid = new GridSystem(8, 8);

// Add some pixels
grid.AddPixel(new Pixel(PixelColor.Red, new GridPosition(2, 3), 0));
grid.AddPixel(new Pixel(PixelColor.Blue, new GridPosition(1, 5), 0));

// Create shooter manager
ShooterManager manager = new ShooterManager(grid);

// Initialize shooters based on grid content
manager.InitializeShootersFromGrid();

// Or manually add shooters
manager.AddShooter(PixelColor.Red, 5);

// Subscribe to events
manager.OnPixelDestroyed += (pos) => Debug.Log($"Pixel destroyed at {pos}");
manager.OnGameWon += () => Debug.Log("Game Won!");

// Activate a shooter
Shooter redShooter = manager.AvailableShooters[0];
manager.ActivateShooter(redShooter);

// Execute shooter path step by step
while (manager.HasActiveShooter)
{
    var destroyed = manager.ExecuteShooterStep();
    // Handle each step's results
}

// Or execute complete path at once
// var allDestroyed = manager.ExecuteShooterCompletePath();
```

## Testing

The `ShooterSystemDemo` MonoBehaviour includes comprehensive tests:

1. **Basic Shooter Movement** - Verifies left-to-right movement through all columns
2. **Shooter Firing** - Tests firing at matching-color pixels
3. **Line-of-Sight Blocking** - Validates that non-matching pixels block sight
4. **Multiple Layer Destruction** - Tests destruction of stacked pixels
5. **Out of Balls Mid-Path** - Verifies shooter stops when ammo depletes
6. **No Valid Targets** - Tests shooter behavior with no matching pixels
7. **Last Pixel Win** - Validates win condition on final pixel destruction
8. **Shooter Manager** - Tests one-active-shooter-at-a-time constraint

To run tests:
1. Create an empty GameObject in the scene
2. Attach the `ShooterSystemDemo` component
3. Enter Play mode in Unity Editor
4. Check Console for test results

## Design Principles

- **Separation of Concerns**: Data, Core systems, and Gameplay mechanics are separate
- **Testability**: Pure C# classes (except Demo) allow for easy unit testing
- **Event-Driven**: Manager uses events for loose coupling with UI/visual systems
- **GDD Compliance**: All behavior strictly follows the Game Design Document
- **Performance**: Efficient algorithms for line-of-sight detection and grid management

## Integration Points

This implementation provides the foundation for:
- Visual representation (Unity GameObjects/UI)
- Animation systems (pixel destruction, shooter movement)
- Sound effects (firing, destruction)
- Input handling (shooter selection and activation)
- Level loading and progression
- Save/load systems

## Future Enhancements

While the core mechanics are complete, potential additions include:
- Visual effects integration points
- Replay/recording system
- AI solver for hint system
- Tutorial integration hooks
- Analytics event triggers
