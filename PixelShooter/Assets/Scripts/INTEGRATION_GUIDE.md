# PixelShooter HUD Integration Guide

## Quick Start

### 1. Unity Editor Setup

The HUD system includes an editor utility to help you set up the UI quickly:

1. Open Unity Editor (Unity 6000.2.12f1 or later)
2. Go to menu: **PixelShooter > Setup HUD Prefabs**
3. Click **"Setup HUD in Scene"** to automatically create the HUD hierarchy

This will create:
- HUD Canvas with proper scaling settings
- Safe area panel for notch support
- Top panel with Progress Display
- Bottom panel with Shooter Panel
- Center panel for Action Feedback

### 2. Manual Setup (Alternative)

If you prefer manual setup:

#### Create Canvas
1. Create a new Canvas GameObject
2. Set Canvas render mode to "Screen Space - Overlay"
3. Add Canvas Scaler component:
   - UI Scale Mode: Scale With Screen Size
   - Reference Resolution: 1080 x 1920
   - Match: 0.5 (balanced)

#### Create HUD Manager
1. Create empty GameObject under Canvas named "HUDManager"
2. Add the `HUDManager` component
3. Create child panels as described below

#### Create Shooter Panel
1. Create GameObject "ShooterPanel" in bottom area of screen
2. Add `ShooterPanel` component
3. Create child GameObject "ShooterContainer"
4. Add `GridLayoutGroup` to container:
   - Cell Size: 100 x 120
   - Spacing: 10 x 10
   - Constraint: Fixed Column Count (3)
5. Assign container reference to ShooterPanel

#### Create Progress Display
1. Create GameObject "ProgressDisplay" in top area
2. Add `ProgressDisplay` component
3. Create child UI elements:
   - TextMeshPro for pixels remaining
   - TextMeshPro for completion percentage
   - Slider for progress bar
4. Assign references in ProgressDisplay component

#### Create Action Feedback
1. Create GameObject "ActionFeedbackPanel" in center
2. Add `ShooterActionFeedback` component
3. Create child UI elements:
   - TextMeshPro for shooter name
   - TextMeshPro for ball count
   - Image for color indicator
   - Image for direction arrow
4. Assign references in ShooterActionFeedback component

#### Create Shooter UI Element Prefab
1. Use menu: **PixelShooter > Setup HUD Prefabs**
2. Click **"Create Shooter UI Element Prefab"**
3. This creates the prefab at Assets/Prefabs/ShooterUIElement.prefab
4. Assign this prefab to the ShooterPanel's "Shooter Element Prefab" field

### 3. Connecting to Your Game

Once the HUD is set up, integrate it with your game logic:

```csharp
using PixelShooter.Data;
using PixelShooter.UI;

public class GameController : MonoBehaviour
{
    private HUDManager hudManager;
    private GameState gameState;
    
    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        
        // Create game state
        gameState = new GameState();
        gameState.totalPixels = CalculateTotalPixels();
        gameState.remainingPixels = gameState.totalPixels;
        
        // Add shooters based on level design
        foreach (var shooterConfig in levelShooters)
        {
            var shooter = new ShooterData(
                shooterConfig.color,
                shooterConfig.colorName,
                shooterConfig.ballCount
            );
            gameState.availableShooters.Add(shooter);
        }
        
        // Initialize HUD
        hudManager.SetupHUD(gameState);
    }
    
    void OnPixelDestroyed()
    {
        hudManager.NotifyPixelDestroyed();
    }
    
    void OnShooterActivated()
    {
        hudManager.NotifyShooterStarted();
    }
    
    void OnShooterCompleted()
    {
        hudManager.NotifyShooterStopped();
    }
}
```

## Component Details

### HUDManager
Main controller that manages all HUD components.

**Public Methods:**
- `SetupHUD(GameState state)` - Initialize HUD with game state
- `RefreshDisplay()` - Update all HUD elements
- `NotifyPixelDestroyed()` - Call when a pixel is destroyed
- `NotifyShooterStarted()` - Call when shooter begins moving
- `NotifyShooterStopped()` - Call when shooter finishes

**Inspector Fields:**
- Shooter Panel Component
- Progress Component
- Feedback Component
- Main Canvas
- Scaler
- Safe Area Rect

### ShooterPanel
Displays available shooters in a grid layout.

**Public Methods:**
- `InitializeShooters(List<ShooterData>, callback)` - Set up shooter buttons
- `UpdateShooterDisplays()` - Refresh all shooter displays
- `SetPanelTitle(string)` - Set panel title text

**Inspector Fields:**
- Shooter Element Prefab
- Shooter Container
- Panel Title (TextMeshPro)
- Element Spacing
- Max Columns

### ProgressDisplay
Shows game progress with pixel count and percentage.

**Public Methods:**
- `Initialize(int totalPixels)` - Set up with total pixel count
- `UpdateProgress(int remainingPixels)` - Update current progress
- `SetProgressBarColor(Color)` - Change progress bar color

**Inspector Fields:**
- Pixels Remaining Text (TextMeshPro)
- Completion Percentage Text (TextMeshPro)
- Progress Bar (Slider)
- Progress Bar Fill (Image)
- Animate Changes (bool)
- Animation Speed (float)

### ShooterActionFeedback
Displays active shooter information and movement.

**Public Methods:**
- `ShowFeedback(ShooterData, MovementDirection)` - Show shooter info
- `HideFeedback()` - Hide feedback panel
- `UpdateBallCount(int)` - Update displayed ball count

**Inspector Fields:**
- Feedback Panel
- Shooter Name Text (TextMeshPro)
- Balls Remaining Text (TextMeshPro)
- Shooter Color Indicator (Image)
- Direction Arrow (Image)
- Moving Indicator
- Pulse When Active (bool)
- Pulse Speed (float)

## Data Models

### ShooterData
Represents a shooter's state.

**Properties:**
- `Color color` - Visual color
- `string colorName` - Display name
- `int totalBalls` - Initial ball count
- `int remainingBalls` - Current ball count
- `bool isUsed` - Whether shooter is depleted
- `bool isSelected` - Whether shooter is selected

**Methods:**
- `UseBall()` - Decrement ball count
- `GetPercentageRemaining()` - Get remaining percentage

### GameState
Manages overall game state.

**Properties:**
- `List<ShooterData> availableShooters` - All shooters
- `int totalPixels` - Level total
- `int remainingPixels` - Current count
- `ShooterData activeShooter` - Currently active
- `bool isShooterMoving` - Movement state
- `MovementDirection shooterDirection` - Direction

**Methods:**
- `GetCompletionPercentage()` - Get completion %
- `DestroyPixel()` - Decrement pixel count
- `IsLevelComplete()` - Check win condition

### ColorPalette
Static color definitions.

**Available Colors:**
- Red, Blue, Yellow, Green, Orange, Purple, Black, White
- UI colors for backgrounds, text, highlights

**Methods:**
- `GetColorName(Color)` - Get color name from Color value

## Screen Scaling

The HUD automatically scales across different resolutions:

### Supported Resolutions
- 720p (1280x720) - Minimum
- 1080p (1920x1080) - Target
- 1440p (2560x1440) - High-end
- 2160p (3840x2160) - Ultra

### Scaling Configuration
Canvas Scaler settings in HUDManager:
- **Reference Resolution**: 1080 x 1920 (portrait)
- **Match Width/Height**: 0.5 (balanced)
- **Screen Match Mode**: Match Width Or Height

### Safe Area
Automatically adjusts for:
- iPhone notches
- Android punch-holes
- Tablet navigation bars
- Curved screen edges

## Touch Optimization

All interactive elements follow mobile best practices:
- **Minimum touch target**: 100 x 120 points
- **Spacing**: 10 points between elements
- **Visual feedback**: Immediate on touch
- **Touch-friendly grid**: 3 columns max on phone

## Accessibility Features

### Current
- High contrast UI elements
- Large, readable text
- Color-coded with visual indicators
- Touch targets exceed minimum standards

### Planned (via ColorPalette support)
- Colorblind mode patterns
- Adjustable text size
- Reduced motion option
- Audio feedback

## Demo Mode

The HUD includes demo functionality for testing:

1. Add `HUDDemo` component to your scene
2. Reference the HUDManager
3. Run the scene

**Demo Features:**
- Auto-generates 6 colored shooters
- Creates 64-pixel level
- Auto-destroys pixels every 2 seconds
- Allows manual shooter selection
- Shows all HUD states

## Troubleshooting

### Shooters Not Displaying
- Ensure ShooterUIElement prefab is assigned to ShooterPanel
- Check that shooter container has GridLayoutGroup
- Verify ShooterData objects are properly created

### Progress Bar Not Updating
- Check that ProgressDisplay has all references set
- Ensure slider value range is 0-1
- Verify progress bar fill image is assigned

### Action Feedback Not Showing
- Ensure feedback panel starts inactive
- Check that all text and image references are set
- Verify shooter is set as activeShooter in GameState

### Scaling Issues
- Verify Canvas Scaler settings match documentation
- Check reference resolution (1080x1920)
- Ensure match value is 0.5
- Test on actual device, not just editor

### Safe Area Not Applied
- Enable "Apply Safe Area" on HUDManager
- Assign Safe Area Rect Transform reference
- Test on device with notch/punch-hole

## Performance Considerations

The HUD is optimized for mobile:
- **Batching**: Uses single atlas for UI
- **Updates**: Only on state changes
- **Animations**: Lightweight lerp-based
- **Memory**: Minimal allocations after initialization

**Frame Budget:**
- HUD updates: < 1ms
- Animations: < 0.5ms
- Total overhead: < 2ms per frame

## Next Steps

1. ✅ Core HUD implementation complete
2. 🔄 Create example scenes
3. 📋 Add colorblind mode patterns
4. 📋 Implement hint system UI
5. 📋 Add undo button
6. 📋 Create settings panel
7. 📋 Add sound effect hooks
8. 📋 Implement haptic feedback

## Support

For issues or questions:
- Check HUD_README.md in Scripts folder
- Review example scenes
- Consult Unity documentation for UI components
