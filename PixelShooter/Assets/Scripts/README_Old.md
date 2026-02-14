# PixelShooter Game Scripts

This folder contains the core game scripts for the PixelShooter game.

## Game Overview

PixelShooter is a puzzle game where players use numbered shooters to destroy colored pixel blocks in a grid.

## Core Components

### Pixel.cs
Represents a single colored pixel in the grid.
- **Properties**: number (1-8), color (8 primary colors), layer
- **Colors**: Red, Blue, Yellow, Green, Orange, Purple, Black, White
- Displays number on the pixel square

### Shooter.cs
Represents a moving shooter that fires balls at pixels.
- **Properties**: number (matches pixels), ball count, color
- **Behavior**: 
  - Moves left to right across the bottom of the grid
  - Shoots at pixels with matching numbers in line of sight
  - Displays remaining ball count

### GridManager.cs
Manages the pixel grid and spawning.
- Creates multi-layered grid of pixels (default 8x8, 2 layers)
- Handles pixel removal and tracking
- Provides line-of-sight detection for shooters
- Configurable grid size and spacing

### GameManager.cs
Main game controller and coordinator.
- Initializes the game and grid
- Creates shooters with appropriate ball counts
- Handles shooter selection via mouse click
- Manages game state and completion
- Updates status display

## Game Flow

1. Game starts with a grid of colored pixels
2. Shooters are generated at the bottom, one for each unique number (1-8)
3. Each shooter has enough balls to destroy all pixels with matching numbers
4. Player clicks a shooter to activate it
5. Active shooter moves left to right, shooting at matching pixels
6. When all balls are used, shooter disappears
7. Player selects next shooter
8. Game completes when all pixels are cleared

## How to Use

1. Open the GameScene in Unity
2. The GameManager GameObject references:
   - GridManager for pixel grid
   - Shooter Prefab for creating shooters
   - Status Text for UI feedback
3. Press Play to start the game
4. Click shooters to activate them
