# PixelShooter Architecture

## Game Component Hierarchy

```
GameScene
├── Main Camera
│   └── (Orthographic, positioned to view grid and shooters)
│
├── GameManager
│   ├── GridManager (child)
│   │   └── Pixel instances (created at runtime)
│   │       └── Number (TextMesh child)
│   │
│   └── ShooterSelectionArea (child)
│       └── Shooter instances (created at runtime)
│           └── Label (TextMesh child)
│
└── StatusText (TextMesh)
```

## Data Flow

```
1. Game Start
   ┌─────────────────┐
   │  GameManager    │
   │  .Start()       │
   └────────┬────────┘
            │
            ├──> GridManager.InitializeGrid()
            │    ├─> Creates 8x8x2 grid of Pixels
            │    └─> Assigns random numbers & colors
            │
            └──> CreateShooters()
                 ├─> Counts pixels per number
                 └─> Creates Shooter for each number

2. Player Interaction
   ┌─────────────────┐
   │  Mouse Click    │
   └────────┬────────┘
            │
            v
   ┌─────────────────┐
   │  GameManager    │
   │  .Update()      │
   │  Raycast2D      │
   └────────┬────────┘
            │
            v
   ┌─────────────────┐
   │  ActivateShooter│
   └────────┬────────┘
            │
            v
   ┌─────────────────┐
   │  Shooter        │
   │  .Activate()    │
   └─────────────────┘

3. Shooting Mechanics
   ┌─────────────────┐
   │  Shooter        │
   │  .Update()      │
   └────────┬────────┘
            │
            ├──> Move right (transform.position)
            │
            └──> ShootAtPixels() (every 0.2s)
                 │
                 v
   ┌─────────────────────────────┐
   │  GridManager                │
   │  .GetPixelInLineOfSight()   │
   └────────┬────────────────────┘
            │
            v
   ┌─────────────────┐
   │  Target Pixel   │
   │  found?         │
   └────────┬────────┘
            │
            ├─> Yes: Destroy pixel
            │        Decrement ballCount
            │        Update display
            │
            └─> No:  Continue moving

4. Game Completion
   ┌─────────────────┐
   │  GameManager    │
   │  .Update()      │
   └────────┬────────┘
            │
            v
   ┌──────────────────────────┐
   │  GridManager             │
   │  .AreAllPixelsCleared()  │
   └────────┬─────────────────┘
            │
            v
   ┌─────────────────┐
   │  All cleared?   │
   └────────┬────────┘
            │
            ├─> Yes: Display "Game Complete!"
            │
            └─> No:  Continue gameplay
```

## Key Design Patterns

### Singleton-like Access
- GridManager and GameManager are found via `FindObjectOfType` (cached in Shooter)
- Ensures loose coupling while maintaining access

### Prefab Instantiation
- Pixel and Shooter are instantiated from prefabs
- Allows consistent appearance and behavior

### Component-Based Architecture
- Each GameObject has specific components (MonoBehaviour scripts)
- SpriteRenderer for visuals
- TextMesh for numbers
- BoxCollider2D for click detection

### Event-Driven Updates
- Shooter.Update() drives movement and shooting
- GameManager.Update() handles input and game state

## Color System

```
PixelColor Enum
├─ Red
├─ Blue
├─ Yellow
├─ Green
├─ Orange
├─ Purple
├─ Black
└─ White

ColorUtility.GetColorFromPixelColor()
└─> Converts enum to Unity Color
    Used by both Pixel and Shooter
```

## Grid Layout

```
Layer 0 (Front):          Layer 1 (Back):
[8][7][6][5][4][3][2][1]  [1][2][3][4][5][6][7][8]
[1][2][3][4][5][6][7][8]  [8][7][6][5][4][3][2][1]
[2][3][4][5][6][7][8][1]  [7][6][5][4][3][2][1][8]
...                       ...

Coordinates:
- X: 0 to 7 (left to right)
- Y: 0 to 7 (bottom to top)
- Z: 0, -0.1 (layer depth)
```

## Shooter Behavior

```
Shooter Position over time:
│
│   Pixels: [1] [2] [3] [4] [5] [6] [7] [8]
│           ─────────────────────────────────
│                       ↑ ↑ ↑
│                       │ │ │ (shoots pixels with matching number)
│                       │ │ │
└───────────> [Shooter #3, 12 balls]
             ─────────────────────────>
             (moves left to right)

When shooter's X aligns with a pixel's X (±tolerance):
└─> If pixel.number == shooter.number:
    └─> SHOOT! (destroy pixel, ballCount--)
```

## Performance Optimizations

1. **Cached References**: GridManager and GameManager cached in Shooter.Awake()
2. **Timed Shooting**: Shoots at intervals (0.2s) instead of every frame
3. **Direct Removal**: Pixels removed from active list immediately
4. **Simple Collision**: Uses single count check for game completion

---

This architecture provides a clean separation of concerns while maintaining
good performance for a mobile game.
