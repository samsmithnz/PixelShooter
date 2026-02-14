# PixelShooter
A simple mobile game to solve pixel puzzles

## Game Description

PixelShooter is a puzzle game where players use numbered shooters to destroy colored pixel blocks arranged in a grid.

### Game Features

- **Grid of Colored Pixels**: 8x8 grid with multiple layers of colored squares
- **8 Primary Colors**: Red, Blue, Yellow, Green, Orange, Purple, Black, and White
- **Numbered Shooters**: Moving squares that shoot balls at matching numbered pixels
- **Line-of-Sight Mechanics**: Shooters move left to right and shoot pixels directly in their path
- **Layered Gameplay**: Destroyed pixels expose pixels behind them
- **Strategic Puzzle**: Clear all pixels to win

### How to Play

1. Open the project in Unity (6000.2.12f1 or later)
2. Open the `GameScene` in `PixelShooter/Assets/Scenes/`
3. Press Play
4. Click on a shooter at the bottom of the screen to activate it
5. Watch as it moves left to right, shooting at matching numbered pixels
6. Continue selecting shooters until all pixels are cleared

### Project Structure

```
PixelShooter/
├── Assets/
│   ├── Scripts/
│   │   ├── Pixel.cs          # Colored pixel square component
│   │   ├── Shooter.cs        # Shooter component with ball count
│   │   ├── GridManager.cs    # Manages pixel grid creation
│   │   ├── GameManager.cs    # Main game controller
│   │   └── README.md         # Detailed script documentation
│   ├── Prefabs/
│   │   ├── Pixel.prefab      # Pixel prefab with sprite and number
│   │   └── Shooter.prefab    # Shooter prefab with collider
│   └── Scenes/
│       └── GameScene.unity   # Main game scene
```

### Technical Details

- **Engine**: Unity 6000.2.12f1
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Platform**: Mobile-ready (2D game)
- **Scripts**: C#
