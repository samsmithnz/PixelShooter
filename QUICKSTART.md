# PixelShooter Game - Quick Start Guide

## Overview
PixelShooter is now fully implemented! The game features a grid of colored pixels that players clear by selecting and activating numbered shooters.

## What's Included

### Scripts (Assets/Scripts/)
- **ColorUtility.cs**: Shared utility for converting PixelColor enum to Unity colors
- **Pixel.cs**: Represents colored squares in the grid with numbers
- **Shooter.cs**: Moving squares that shoot balls at matching pixels
- **GridManager.cs**: Creates and manages the multi-layer pixel grid
- **GameManager.cs**: Main controller coordinating game flow

### Prefabs (Assets/Prefabs/)
- **Pixel.prefab**: Reusable pixel with sprite renderer and text mesh
- **Shooter.prefab**: Reusable shooter with collider for click detection

### Scenes (Assets/Scenes/)
- **GameScene.unity**: Main playable game scene with all setup complete

## How to Play (in Unity Editor)

1. **Open the Project**
   - Open Unity Hub
   - Add this project (requires Unity 6000.2.12f1 or compatible version)
   - Open the project

2. **Open the Game Scene**
   - Navigate to `Assets/Scenes/GameScene.unity`
   - Double-click to open

3. **Play the Game**
   - Click the Play button in Unity Editor
   - The grid of colored pixels will appear with numbers
   - Shooters appear at the bottom of the screen

4. **Game Controls**
   - Click on any shooter at the bottom to activate it
   - The shooter will move left to right
   - It automatically shoots at pixels with matching numbers
   - Continue until all pixels are cleared

## Game Mechanics

### Pixel Grid
- 8x8 grid with 2 layers of pixels
- Each pixel has:
  - A number (1-8)
  - A color (Red, Blue, Yellow, Green, Orange, Purple, Black, White)
  - A visible number displayed on it

### Shooters
- One shooter for each unique number (1-8)
- Each shooter contains enough balls to destroy all matching pixels
- Displays: `[Number]\n[Ball Count]`

### Gameplay Flow
1. Grid is generated with random colored pixels and numbers
2. Shooters are created based on pixel distribution
3. Player selects a shooter by clicking
4. Shooter moves and shoots at matching pixels in its path
5. When balls run out, select next shooter
6. Game completes when all pixels are destroyed

## Customization

You can customize the game by editing values in the Unity Inspector:

### GridManager Settings
- `gridWidth`: Width of the pixel grid (default: 8)
- `gridHeight`: Height of the pixel grid (default: 8)
- `numberOfLayers`: Number of pixel layers (default: 2)
- `pixelSize`: Size of each pixel square (default: 1.0)
- `pixelSpacing`: Space between pixels (default: 0.1)

### Shooter Settings (in code)
- `moveSpeed`: How fast shooter moves (default: 2.0)
- `shootInterval`: Time between shots (default: 0.2 seconds)

## Technical Notes

- The game uses Unity's built-in sprites (squares)
- TextMesh is used for displaying numbers
- 2D physics with BoxCollider2D for click detection
- Orthographic camera for 2D gameplay
- Universal Render Pipeline (URP) compatible

## Troubleshooting

**Issue**: Shooters not responding to clicks
- **Solution**: Ensure Main Camera has Physics2D raycaster enabled

**Issue**: Pixels not showing colors correctly
- **Solution**: Check that ColorUtility.cs is compiled without errors

**Issue**: Game doesn't start
- **Solution**: Verify GameManager references are set in Inspector (GridManager, Shooter Prefab, Status Text)

## Next Steps

This implementation provides a complete, working game. Possible enhancements:
- Add visual effects for shooting
- Add sound effects
- Create different level designs
- Add scoring system
- Implement save/load functionality
- Create UI menus for game start/restart
- Add animations for pixel destruction

---

**Ready to play!** Open GameScene.unity in Unity and press Play!
