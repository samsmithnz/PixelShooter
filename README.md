# PixelShooter
A simple mobile puzzle game where players use color-matched shooters to clear layered pixel grids.

## Game Design Document

### Overview
PixelShooter is a puzzle game with a grid of large pixels of different colors. Players can select and use colored shooters that contain a limited number of projectiles. Each projectile can remove one pixel that matches the shooters color - and reduces the shooters projectile by one. The shooter can only shoot if they have direct view of the matching pixel on the vertical axis (straight up), with no other colors blocking it. Hitting the pixel removes it - exposing other pixels! The shooter moves from left to right along the bottom row of the grid, automatically destroying matching pixels that are in its vertical line of sight. A game is completed when all pixels are cleared from the grid. The game combines color matching, line-of-sight mechanics, and strategic planning to create an engaging puzzle experience.

### Core Concept
Players are presented with a grid of colored pixel squares. Shooters at the bottom of the screen can be selected and activated. When activated, the shooter moves from left to right along the bottom row, automatically shooting balls at matching-colored pixels when they align vertically (in a direct line of sight). This destroys the pixels and reveals the layers behind them. The goal is to clear all pixels from the grid using the available shooters strategically.

## Technical Implementation

### 1. Unity3D Project and Core Grid System

**Platform**: Unity3D

**Grid System**:
- Create a grid of colored square pixels displayed from a top-down perspective
- Default grid size: 8x8 cells (configurable for different difficulty levels)
- Support pixel stacking/layering to enable multiple designs per cell
- Each cell can contain multiple layers of pixels that are revealed as top layers are destroyed
- Maximum of 5 layers per cell recommended for visual clarity
- When a pixel is destroyed, the layer beneath it is immediately revealed (no falling/gravity mechanics)
- Grid cells without pixels are displayed as transparent/empty spaces

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
- The game should be designed to always be solvable with optimal strategy
- If a player gets stuck, they can restart the level
- Future enhancement: Implement hint system to help players find solutions

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

## Level Design and Progression

### Grid Configurations
- **Tutorial Levels (1-5)**: Simple 5x5 grids with 1-2 layers, single color focus
- **Easy Levels (6-15)**: 6x6 grids with 2-3 layers, 3-4 colors
- **Medium Levels (16-30)**: 8x8 grids with 3-4 layers, 4-6 colors
- **Hard Levels (31-50)**: 10x10 grids with 4-5 layers, 6-8 colors
- **Expert Levels (51+)**: Variable grid sizes with maximum complexity

### Level Design Principles
- **Solvability**: All levels must be solvable with at least one optimal solution
- **Pixel Distribution**: Balance color distribution to ensure no single color dominates excessively
- **Layer Strategy**: Early layers should provide strategic choices, deeper layers add complexity
- **Shooter Efficiency**: Number of shooters should be carefully balanced (typically 3-6 shooters per level)
- **Visual Patterns**: Incorporate recognizable patterns or images within pixel arrangements for added interest

### Level Progression System
- Players unlock levels sequentially by completing previous levels
- Optional star rating system based on shooter efficiency (1-3 stars)
- Track completion time and shooter usage for leaderboards
- Allow players to replay completed levels for better scores

### Level Validation
- Automated testing to ensure all levels are solvable
- Manual playtest verification for difficulty curve
- Ensure each level introduces or reinforces a strategic concept

## Difficulty and Balancing

### Difficulty Parameters
- **Grid Size**: Larger grids increase complexity
- **Number of Colors**: More colors require more strategic planning
- **Layer Depth**: More layers require multi-step thinking
- **Pixel Density**: Higher density grids are more challenging
- **Shooter Count**: Fewer shooters increase difficulty

### Balancing Strategy
- **Ball-to-Pixel Ratio**: Each shooter gets balls equal to matching pixels in the entire grid
- **Shooter Distribution**: Ensure roughly equal distribution of shooter colors in medium+ levels
- **Line-of-Sight Complexity**: Early levels have clear sight lines, later levels require strategic order
- **Solution Paths**: Easy levels have obvious solutions, harder levels have multiple viable approaches

### Difficulty Curve
- Gradual introduction of new mechanics across first 10 levels
- Steady increase in grid complexity every 5-10 levels
- Periodic "breather" levels to prevent frustration
- Difficulty spikes only after mastery is demonstrated

## User Experience Features

### Core Interactions
- **Touch Controls**: Single tap to select shooter, tap again to activate
- **Visual Preview**: Show available targets before activating shooter
- **Undo Function**: Allow players to undo last shooter action (limited or unlimited based on design choice)
- **Pause/Resume**: Ability to pause mid-level and resume later
- **Level Restart**: Easy restart option without penalty

### Player Assistance
- **Hint System**: 
  - Limited hints per level (3 hints recommended)
  - Hint highlights optimal next shooter to use
  - Optional: Skip level feature after multiple failed attempts
- **Tutorial System**:
  - Interactive tutorial for first 3-5 levels
  - Highlight UI elements and explain mechanics in context
  - Gradual introduction of advanced concepts

### Feedback Systems
- **Progress Indicators**:
  - Remaining pixels count
  - Current level number and total levels
  - Completion percentage
  - Shooters remaining count
- **Achievement System**:
  - Milestones for levels completed
  - Special achievements for perfect clears
  - Achievements for speed runs or minimal shooter usage

### Settings and Options
- **Audio Controls**: Music and sound effect volume sliders
- **Colorblind Mode**: Alternative color palettes or pattern overlays for accessibility
- **Animation Speed**: Option to speed up or slow down animations
- **Haptic Feedback**: Vibration options for mobile devices (on/off toggle)

### Save System
- Auto-save progress after each level
- Cloud save support for cross-device play (future enhancement)
- Level progress and statistics persistence
- Option to reset all progress

## Technical Specifications

### Performance Targets
- **Frame Rate**: Maintain 60 FPS on target mobile devices
- **Load Time**: Level load time < 2 seconds
- **Memory**: Keep memory footprint under 150MB on mobile devices
- **Battery**: Optimize for minimal battery drain during gameplay

### Platform Requirements
- **iOS**: iOS 12.0 or later, iPhone 6 and newer
- **Android**: Android 8.0 (API level 26) or later
- **Screen Support**: 
  - Portrait orientation primary (9:16 to 9:21 aspect ratios)
  - Landscape optional for tablets
  - Support for common resolutions (720p to 1440p+)

### Data and Storage
- **App Size**: Target < 100MB initial download
- **Save Data**: < 5MB for player progress and settings
- **Offline Play**: Full offline functionality required
- **Analytics**: Optional anonymous usage data collection (with user consent)

### Rendering Specifications
- **Resolution Independence**: UI scales appropriately across device sizes
- **Pixel Art Style**: Crisp pixel rendering without anti-aliasing for grid elements
- **UI Scaling**: Dynamic UI scaling based on screen DPI
- **Color Space**: sRGB color space for consistent colors across devices

## Quality Assurance and Testing

### Testing Categories
- **Functional Testing**:
  - All levels are completable
  - Shooter mechanics work as designed
  - UI elements respond correctly to input
  - Save/load system preserves state accurately
  
- **Compatibility Testing**:
  - Test on minimum spec devices
  - Verify on various screen sizes and aspect ratios
  - Test on latest iOS and Android versions
  - Verify colorblind mode accessibility
  
- **Performance Testing**:
  - Frame rate monitoring across all levels
  - Memory leak detection
  - Load time verification
  - Battery consumption monitoring

### Known Limitations
- Complex levels (10x10+ with 5 layers) may have slight performance impact on older devices
- Initial version focuses on core mechanics; advanced features (multiplayer, level editor) are post-launch
- Colorblind mode supports deuteranopia and protanopia; other types in future updates

### Bug Prevention
- Automated level validation to prevent unsolvable puzzles
- Unit tests for core game logic (shooter mechanics, line-of-sight calculations)
- Regression testing after each major feature addition
- Beta testing program for community feedback before launch

## Accessibility Considerations

### Visual Accessibility
- **Colorblind Modes**:
  - Deuteranopia (red-green colorblindness) support
  - Protanopia support
  - Pattern overlays on pixels as alternative to color-only matching
  - High contrast mode option
  
### Usability
- **Font Sizing**: Adjustable UI text size
- **Touch Targets**: Minimum 44x44 point touch targets for all interactive elements
- **Visual Feedback**: Clear visual feedback for all interactions beyond color
- **Reduced Motion**: Option to disable or minimize animations

### Localization Readiness
- Minimal text-based UI to reduce localization complexity
- Support for number format localization
- RTL (right-to-left) language support consideration
- Initial launch: English only, expand based on demand

## Development Phases

### Phase 1: Core Mechanics (✅ Completed)
1. ✅ Core grid system with pixel stacking
2. ✅ Shooter mechanics and line-of-sight logic
3. ✅ Player controls and UI implementation
4. ✅ Visual and audio effects system
5. ✅ Game loop and win condition logic

### Phase 2: Level Design and Content (In Progress)
- Create initial 50 levels with progressive difficulty
- Implement level validation system
- Design tutorial levels with interactive guidance
- Balance difficulty curve through playtesting

### Phase 3: User Experience Enhancement (Planned)
- Implement hint system
- Add undo functionality
- Create achievement system
- Implement settings and accessibility options
- Add colorblind mode support

### Phase 4: Polish and Optimization (Planned)
- Performance optimization for target devices
- Visual and audio polish
- Animation refinement
- User feedback integration
- Bug fixes and edge case handling

### Phase 5: Launch Preparation (Planned)
- Quality assurance testing across devices
- Beta testing program
- App store optimization
- Analytics integration
- Soft launch and iteration

## Target Platform

- Mobile devices (iOS and Android)
- Touch-based controls
- Portrait or landscape orientation (TBD based on grid size)

## References

This design document consolidates requirements from GitHub issues #1, #2, #3, #4, #5, and #6.

## Edge Cases and Design Clarifications

### Gameplay Edge Cases
- **No Valid Targets**: If a shooter has no pixels in line of sight, it should not fire and remain available for later use
- **Multiple Same-Color Shooters**: Each shooter instance is independent; if multiple red shooters exist, they each have their own ball counts
- **Last Pixel Scenario**: Destroying the last pixel triggers win condition immediately
- **Simultaneous Destruction**: While shooter moves left-to-right, destruction happens sequentially (not simultaneous)

### Technical Edge Cases
- **Grid Boundary Handling**: Shooters operate only within grid boundaries, no wrap-around
- **Empty Layers**: If a destroyed pixel reveals an empty cell (no pixel beneath), cell remains empty
- **Shooter Ball Depletion**: When shooter runs out of balls mid-path, it stops and is removed from available shooters
- **Level State Corruption**: Implement save state validation to prevent corruption from app crashes

### Future Enhancements Under Consideration
- **Level Editor**: Allow players to create and share custom levels
- **Daily Challenges**: Special levels with unique constraints or goals
- **Multiplayer Mode**: Competitive or cooperative puzzle solving
- **Power-ups**: Special abilities like multi-shot or color-changing mechanics
- **Themes**: Alternative visual themes beyond pixel art
- **Story Mode**: Light narrative wrapper around level progression

## Frequently Asked Questions (Design)

**Q: Can shooters move in directions other than left-to-right?**  
A: Currently no. The core mechanic is left-to-right movement along the bottom row. Future versions may explore different movement patterns.

**Q: What happens if a level is mathematically unsolvable?**  
A: All levels undergo validation to ensure solvability. If a bug creates an unsolvable state, the restart option is always available.

**Q: Can pixels fall down when pixels below them are destroyed?**  
A: No. This design uses a layering system (like transparency layers in image editors), not a gravity-based falling system. Pixels stay in their cells.

**Q: How many times can a player use the undo feature?**  
A: This is a balance decision still under consideration. Options include: unlimited undo, limited to last 3 actions, or single undo per level.

**Q: Will there be in-app purchases or ads?**  
A: Monetization strategy is not yet defined. Options include: premium (paid upfront), freemium with level packs, or ad-supported with optional ad removal.

**Q: Can players skip difficult levels?**  
A: Not in the current design. However, the hint system and potential skip feature after multiple failures are under consideration for accessibility.
