# PixelShooter Setup Instructions

## Required Setup Steps in Unity

The game requires several GameObjects and component references to be set up in the Unity Editor. Follow these steps:

### 1. Create Core GameObjects

In the Unity Hierarchy, create the following empty GameObjects:

1. **GameManager** (if not exists)
   - Add Component ? Scripts ? GameManager
   
2. **GridManager** (if not exists)
   - Add Component ? Scripts ? GridManager
   
3. **ShooterSelectionArea** (if not exists)
   - This is just an empty Transform to organize shooter objects

### 2. Create Prefabs

You need to create two prefabs:

#### Pixel Prefab
1. Create a new GameObject in the scene
2. Name it "Pixel"
3. Add Component ? Sprite Renderer
4. Add Component ? Scripts ? Pixel
5. Drag this GameObject from Hierarchy to the Project window (Assets folder) to create a prefab
6. Delete the Pixel GameObject from the scene (we only need the prefab)

#### Shooter Prefab
1. Create a new GameObject in the scene
2. Name it "Shooter"
3. Add Component ? Sprite Renderer
4. Add Component ? Box Collider 2D
5. Add Component ? Scripts ? Shooter
6. Create a child GameObject under Shooter
7. Name the child "Text"
8. Add Component ? Text Mesh to the child
9. Drag the Shooter GameObject from Hierarchy to the Project window (Assets folder) to create a prefab
10. Delete the Shooter GameObject from the scene (we only need the prefab)

### 3. Assign References in Inspector

#### GameManager Component
Select the GameManager GameObject in Hierarchy, then in the Inspector:
- **Grid Manager**: Drag the GridManager GameObject from Hierarchy here
- **Shooter Prefab**: Drag the Shooter prefab from Assets folder here
- **Shooter Selection Area**: Drag the ShooterSelectionArea GameObject from Hierarchy here
- **Active Shooter Position**: (Optional) Can leave empty
- **Status Text**: (Optional) Create a TextMesh GameObject and drag it here to show game status

#### GridManager Component
Select the GridManager GameObject in Hierarchy, then in the Inspector:
- **Pixel Prefab**: Drag the Pixel prefab from Assets folder here
- **Grid Width**: Set to 8 (default)
- **Grid Height**: Set to 8 (default)
- **Number Of Layers**: Set to 2 (default)
- **Pixel Size**: Set to 1 (default)
- **Pixel Spacing**: Set to 0.1 (default)

### 4. Camera Setup

Make sure your Main Camera is positioned to see the game area:
- Position: (4, 4, -10)
- This centers the camera on an 8x8 grid

### 5. Test the Setup

Press Play. Check the Console window for:
- ? `[GameManager] Start() called`
- ? `[GameManager] GridManager found, initializing grid...`
- ? `[GridManager] InitializeGrid() called`
- ? `[GridManager] Grid initialized with X total pixels`
- ? `[GameManager] Created X shooters`

If you see errors, they will tell you exactly what's missing.

## Quick Troubleshooting

| Error | Solution |
|-------|----------|
| `GridManager is NULL!` | Drag GridManager GameObject to GameManager's "Grid Manager" field |
| `PixelPrefab is NULL!` | Create a Pixel prefab and assign it to GridManager's "Pixel Prefab" field |
| `ShooterPrefab is NULL!` | Create a Shooter prefab and assign it to GameManager's "Shooter Prefab" field |
| Nothing appears on screen | Check camera position and make sure prefabs have Sprite Renderers |
| Pixels don't have the Pixel script | The Pixel prefab needs the Pixel.cs script component |
| Shooters don't have the Shooter script | The Shooter prefab needs the Shooter.cs script component |
