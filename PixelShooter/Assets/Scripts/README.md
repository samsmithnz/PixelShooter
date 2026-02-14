# PixelShooter Game Scripts

This folder contains the core game scripts for the PixelShooter game.

## Game Overview

PixelShooter is a puzzle game where players use color-matched shooters to destroy colored pixel blocks in a layered grid.

## Architecture

The game uses a **data-driven grid system** with the following components:

### Core Grid System (PixelShooter.Grid namespace)

#### GridData.cs
Core data model for the grid.
- **Properties**: width, height, GridCell array
- **Default**: 8x8 grid, configurable 5x5+
- **Layers**: Up to 5 layers per cell
- **Behavior**: Layer-based transparency (no gravity/falling)

#### GridCell.cs
Manages pixel layers within a single cell.
- Supports up to 5 layers per cell (LIFO stack)
- Immediate layer reveal on destruction
- Transparent when empty

#### GridRenderer.cs
Renders the grid visually and manages game interactions.
- Creates PixelRenderer instances for each cell
- Handles visual updates on layer destruction
- Provides line-of-sight detection for shooters
- Counts pixels by color

#### LevelDefinition.cs (ScriptableObject)
Editor-friendly level authoring.
- Define grid size and pixel layouts in Unity Inspector
- Reusable level assets
- Easy to create and test levels

### Game Management

#### GameManager.cs
Main game controller and coordinator.
- Initializes the grid using GridRenderer
- Creates shooters based on pixel colors in the grid
- Handles shooter selection via mouse click
- Manages game state and completion
- Updates status display

#### Shooter.cs
Represents a moving shooter that fires at matching pixels.
- **Properties**: color (matches pixels), ball count
- **Behavior**: 
  - Moves left to right across the bottom of the grid
  - Shoots at pixels with matching colors in line of sight
  - Displays remaining ball count
  - Color-based matching (not number-based)

### Utilities

#### ColorUtility.cs
Helper for converting PixelColor enum to Unity Color.
- Supports all 8 game colors: Red, Blue, Yellow, Green, Orange, Purple, Black, White

## Game Flow

1. Game starts with a grid loaded from a LevelDefinition asset
2. Shooters are generated at the bottom, one for each color present in the grid
3. Each shooter has enough balls to destroy all pixels with matching colors
4. Player clicks a shooter to activate it
5. Active shooter moves left to right, shooting at matching pixels in vertical line of sight
6. When all balls are used, shooter disappears
7. Player selects next shooter
8. Game completes when all pixels are cleared

## How to Use

1. Open the GameScene in Unity
2. Create a LevelDefinition asset (Right-click > Create > PixelShooter > Level Definition)
3. Configure the level in the Inspector
4. The GameManager GameObject references:
   - GridRenderer for pixel grid rendering
   - Shooter Prefab for creating shooters
   - Status Text for UI feedback
5. Assign the LevelDefinition to GridRenderer
6. Press Play to start the game
7. Click shooters to activate them

## Key Differences from Previous Version

- **Color-based matching** instead of number-based
- **Data-driven grid** using ScriptableObject levels instead of random generation
- **Layer-based transparency** system for multi-layer pixels
- **Structured namespace** (PixelShooter.Grid) for better code organization
