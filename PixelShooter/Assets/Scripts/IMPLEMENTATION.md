# Shooter Selection & Turn-State Controller

This document describes the implementation of the shooter selection, activation, and turn-state controller system for PixelShooter.

## Overview

The system implements the complete gameplay flow from shooter selection to action resolution:
- **Tap to select** shooter and **second tap to activate**
- **Prevents activation** while another shooter is active
- **Tracks used vs available** shooters
- **Updates remaining projectile counts** in real time
- **Returns cleanly to selection state** after each shooter run

## Architecture

### Core Components

#### 1. Data Layer (`Assets/Scripts/Data/`)

**ShooterData.cs**
- Represents the data and state for a single shooter
- Tracks:
  - Shooter ID and color
  - Initial and current projectile counts
  - Usage state (used vs available)
- Methods:
  - `UseProjectile()` - Decrements projectile count
  - `MarkAsUsed()` - Marks shooter as used
  - `Reset()` - Resets to initial state
  - `IsAvailable` - Property to check if shooter can be used

#### 2. Game Logic Layer (`Assets/Scripts/Game/`)

**GameState.cs**
- Enum defining game states:
  - `Selection` - Waiting for player to select a shooter
  - `ShooterActive` - A shooter is currently executing
  - `LevelComplete` - Level has been completed
  - `Paused` - Game is paused

**GameController.cs**
- Main game controller managing overall turn-state flow
- Responsibilities:
  - Maintains current game state
  - Manages shooter selection and activation
  - Prevents multiple active shooters
  - Tracks used vs available shooters
  - Updates projectile counts
  - Provides events for UI updates
- Key Methods:
  - `InitializeGame(shooters)` - Sets up the game
  - `SelectShooter(shooter)` - Selects a shooter (first tap)
  - `ActivateSelectedShooter()` - Activates selected shooter (second tap)
  - `DeselectShooter()` - Deselects current selection
  - `UseProjectile()` - Uses a projectile from active shooter
  - `CompleteShooterRun()` - Completes shooter execution and returns to selection
  - `ResetGame()` - Resets to initial state
- Events:
  - `OnGameStateChanged` - Fired when game state changes
  - `OnShooterSelected` - Fired when a shooter is selected
  - `OnShooterActivated` - Fired when a shooter is activated
  - `OnShooterCompleted` - Fired when a shooter completes
  - `OnProjectileCountChanged` - Fired when projectile count changes

**ShooterManager.cs**
- Handles player input for shooter selection and activation
- Implements the two-tap behavior:
  - First tap on a shooter selects it
  - Second tap on the same shooter activates it
  - Tapping a different shooter changes selection
- Key Methods:
  - `OnShooterTapped(shooter)` - Called when player taps a shooter
  - `OnBackgroundTapped()` - Called when player taps outside shooters

**ShooterController.cs**
- Controls individual shooter behavior during execution
- Manages:
  - Movement simulation (left to right)
  - Shooting timing
  - Projectile usage
  - Execution completion
- Key Methods:
  - `Initialize(data)` - Sets up the shooter with data
  - `BeginExecution()` - Starts shooter execution
  - `EndExecution()` - Ends execution and notifies game controller
  - `GetExecutionProgress()` - Returns progress (0 to 1)
  - `IsExecuting()` - Checks if shooter is currently active

#### 3. UI Layer (`Assets/Scripts/UI/`)

**UIController.cs**
- Manages UI display for game state and shooter information
- Subscribes to GameController events for real-time updates
- Displays:
  - Current game state
  - Selected shooter info
  - Active shooter info and projectile count
  - Available shooter count
  - Used shooter count
- Key Methods:
  - `UpdateUI()` - Refreshes all UI elements
  - Event handlers for all GameController events

## Workflow

### Selection Flow

1. Game starts in `Selection` state
2. Player taps a shooter → GameController selects it
3. ShooterManager tracks which shooter was last tapped
4. Player taps same shooter again → GameController activates it
5. Game transitions to `ShooterActive` state

### Execution Flow

1. GameController activates shooter
2. ShooterController begins execution
3. As shooter moves:
   - Projectiles are fired (simulated)
   - GameController.UseProjectile() is called
   - UI updates in real-time via events
4. When execution completes:
   - ShooterController.EndExecution() is called
   - Shooter is marked as used
   - Game returns to `Selection` state

### State Protection

- **Cannot select** while in `ShooterActive` state
- **Cannot activate** if no shooter is selected
- **Cannot activate** a shooter that's already been used
- **Cannot activate** a shooter with no projectiles

## Usage Example

```csharp
// Initialize game with shooters
List<ShooterData> shooters = new List<ShooterData>
{
    new ShooterData("Red", Color.red, 5),
    new ShooterData("Blue", Color.blue, 3)
};
gameController.InitializeGame(shooters);

// Player taps red shooter (first tap - selects)
shooterManager.OnShooterTapped(shooters[0]);

// Player taps red shooter again (second tap - activates)
shooterManager.OnShooterTapped(shooters[0]);

// During execution, projectiles are used
gameController.UseProjectile(); // Updates count

// When finished
gameController.CompleteShooterRun(); // Returns to selection
```

## Testing

Unit tests are provided for core logic:

**GameControllerTests.cs**
- Tests game state transitions
- Tests shooter selection and activation
- Tests prevention of multiple active shooters
- Tests projectile usage and tracking
- Tests reset functionality

**ShooterDataTests.cs**
- Tests projectile count management
- Tests shooter availability logic
- Tests reset functionality

Run tests in Unity Test Runner:
- Window → General → Test Runner
- Select "EditMode"
- Run All

## Integration Points

### For Future Grid System Integration

The shooter system is designed to integrate with a grid system:

1. **Initialization**: Create ShooterData based on matching pixel counts in grid
2. **Line of Sight**: ShooterController.TryShoot() should check grid for matching pixels
3. **Projectile Impact**: When a projectile is used, update the grid to remove the pixel
4. **Win Condition**: After CompleteShooterRun(), check if all pixels are cleared

### For Future Input System Integration

Current implementation uses direct method calls. To integrate with Unity Input System:

1. Create input actions for tap/click
2. Use raycasting to detect tapped shooter
3. Call `ShooterManager.OnShooterTapped()` with the tapped shooter
4. Call `ShooterManager.OnBackgroundTapped()` when background is tapped

## Events & Extensibility

All major actions fire events, allowing easy extension:

- Add visual feedback on selection
- Add animations on activation
- Add sound effects on projectile fire
- Add particle effects on shooter completion
- Add analytics tracking

Example:
```csharp
gameController.OnShooterActivated += (shooter) => {
    PlayActivationAnimation(shooter);
    PlaySound("shooter_activate");
};

gameController.OnProjectileCountChanged += (shooter, count) => {
    UpdateProjectileUI(shooter, count);
    PlaySound("shoot");
};
```

## Future Enhancements

Possible extensions to the system:
- **Undo functionality**: Store shooter states before activation
- **Hint system**: Suggest which shooter to use next
- **Multiple shooter types**: Different behaviors or special abilities
- **Projectile animations**: Visual trajectories for fired projectiles
- **Combo system**: Rewards for efficient shooter usage
