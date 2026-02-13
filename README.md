wdf# PixelShooter
A simple mobile puzzle game where players use color-matched shooters to clear layered pixel grids.

## Game Design Document

### Overview
PixelShooter is a puzzle game with a grid of large pixels of different colors. Players can select and use colored shooters that contain a limited number of projectiles. Each projectile can remove one pixel that matches the shooters color - and reduces the shooters projectile by one. The shooter can only shoot if they have direct view of the matching pixel on the x or y axis, with no other colors blocking it. Hitting the pixel removes it - exposing other pixels! The shooter rotates around the grid destroying matching pixels, following a path similar to a square. A game is completed when the pixels are all removed, or the player has more than 5 shooters in play. The game combines color matching, line-of-sight mechanics, and strategic planning to create an engaging puzzle experience.

### Core Concept
Players are presented with a grid of colored pixel squares. Shooters at the bottom of the screen can be selected and activated, which causes them to rotate around a path on the level to shoot balls at matching-colored pixels - when they align vertically or horizontally, destroying them and revealing the pixels behind. The goal is to clear all pixels from the grid using the available shooters.

## Technical Implementation

### 1. Unity3D Project and Core Grid System

**Platform**: Unity3D

**Grid System**:
- Create a grid of colored square pixels displayed from a top-down perspective
- Support pixel stacking/layering to enable multiple designs per cell
- Each cell can contain multiple layers of pixels that are revealed as top layers are destroyed

**Color Palette**:
- Primary colors supported: Red, Blue, Yellow, Green, Orange, Purple, Black, and White
- Each pixel is assigned one of these colors
- Colors are used for matching with shooters

### 2. Shooter Mechanics and Shooting Logic

**Shooter Properties**:
- Each shooter is a moving square assigned a specific color/number
- Shooters hold a quantity of balls equal to the total number of matching pixels in the grid
- Only one shooter can be active at a time

**Movement and Firing**:
- Player selects a shooter from available options
- Selected shooter appears at the bottom row of the grid
- Shooter moves automatically from left to right along the bottom row
- As the shooter moves, it fires at pixels with a direct line of sight
- Line-of-sight is vertical (straight up from shooter position)

**Destruction Logic**:
- When a shooter has line of sight to a matching-colored pixel, it fires a ball
- The targeted pixel is destroyed, exposing the pixel layer behind it
- This process continues as the shooter moves across the bottom
- Shooter continues until all line-of-sight pixels are cleared or balls run out

### 3. Player Interaction and UI Elements

**Player Controls**:
- Select shooters from available options
- Activate selected shooter to begin its movement and firing sequence
- Clear visual indication of which shooter is selected

**UI Display Elements**:
- Available shooters panel showing all unused shooters
- Number of balls remaining for each shooter
- Current state of the pixel grid
- Visual indication of shooter movement and actions
- Highlight destroyable pixels based on current line of sight
- Active shooter position and direction

**Visual Feedback**:
- Clear indication of which pixels can be destroyed by current shooter
- Show shooter trajectory and firing pattern
- Display remaining balls count in real-time

### 4. Pixel Destruction and Visual Effects

**Destruction Mechanics**:
- When a pixel is hit, create visual destruction effect
- Remove destroyed pixel from the grid
- Reveal the pixel layer underneath (if any exists)
- Update grid display immediately after each shot

**Visual Effects**:
- Destruction animations for pixels being removed
- Shooter firing animations
- Ball trajectory visualization
- Pixel removal transitions
- Smooth updates to grid state

**Audio Effects**:
- Sound effects for shooter firing
- Sound effects for pixel destruction
- Ambient game sounds

### 5. Game Loop and Win Condition

**Main Game Loop**:
- Present grid with layered pixels
- Display available shooters
- Wait for player to select and activate a shooter
- Execute shooter movement and firing sequence
- Update grid state and shooter inventory
- Repeat until win or lose condition

**Progress Tracking**:
- Track number of pixels cleared
- Monitor remaining balls for each shooter
- Track which shooters have been used
- Display progress indicators

**Win Condition**:
- Game is won when all pixels are cleared from the grid
- Display win state screen or notification
- Show completion statistics (shooters used, accuracy, etc.)

**Lose Condition** (if applicable):
- Game may end if all shooters are used but pixels remain
- Or the game may always be solvable with perfect strategy

## Game Flow

1. **Initialize**: Display pixel grid and available shooters
2. **Selection**: Player selects a shooter
3. **Activation**: Shooter moves to bottom row and begins left-to-right movement
4. **Firing**: Shooter automatically fires at matching pixels in line of sight
5. **Update**: Grid updates as pixels are destroyed
6. **Repeat**: Return to selection phase with remaining shooters
7. **Complete**: When all pixels cleared, show win screen

## Key Features

- **Puzzle Design**: Strategic color-matching and shooter selection
- **Layered Gameplay**: Multiple pixel layers create depth and challenge
- **Visual Feedback**: Clear indication of game state and available actions
- **Smooth Animations**: Polished visual and audio effects
- **Satisfying Progression**: Clear goals and win conditions

## Development Phases

1. ✅ Core grid system with pixel stacking
2. ✅ Shooter mechanics and line-of-sight logic
3. ✅ Player controls and UI implementation
4. ✅ Visual and audio effects system
5. ✅ Game loop and win condition logic
6. Future: Level design and progression system

## Target Platform

- Mobile devices (iOS and Android)
- Touch-based controls
- Portrait or landscape orientation (TBD based on grid size)

## References

This design document consolidates requirements from GitHub issues #1, #2, #3, #4, #5, and #6.
