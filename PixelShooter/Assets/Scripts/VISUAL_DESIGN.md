# PixelShooter HUD Visual Design Mockup

## Screen Layout (Portrait - 1080x1920)

```
┌─────────────────────────────────────┐
│         PROGRESS DISPLAY            │ ← Top Panel (200px height)
│  Pixels: 45/64      Completion: 70% │
│  [████████████░░░░░░]               │
├─────────────────────────────────────┤
│                                     │
│                                     │
│         GAME GRID AREA              │
│      (Center playable area)         │
│                                     │
│         ┌───────────┐               │
│         │  ACTIVE   │               │ ← Action Feedback (center)
│         │ Red ➜ 8   │               │   (shown when shooter moving)
│         └───────────┘               │
│                                     │
│                                     │
├─────────────────────────────────────┤
│      AVAILABLE SHOOTERS             │ ← Bottom Panel (300px height)
│                                     │
│   ┌───┐  ┌───┐  ┌───┐              │
│   │ 🟥│  │ 🟦│  │ 🟨│              │
│   │ 12│  │ 10│  │ 8 │              │
│   └───┘  └───┘  └───┘              │
│                                     │
│   ┌───┐  ┌───┐  ┌───┐              │
│   │ 🟩│  │ 🟧│  │ 🟪│              │
│   │ 15│  │ 9 │  │ 10│              │
│   └───┘  └───┘  └───┘              │
└─────────────────────────────────────┘
```

## Component Breakdown

### 1. Progress Display (Top Panel)
```
┌─────────────────────────────────────┐
│  Pixels Remaining: 45 / 64          │
│  Completion: 70.3%                  │
│  ████████████████████░░░░░░░░░      │
└─────────────────────────────────────┘
```
**Elements:**
- Text label: "Pixels: [remaining]/[total]"
- Text label: "Completion: [percentage]%"
- Progress bar (green fill)
- Background: Semi-transparent dark (rgba 0.1, 0.1, 0.1, 0.8)

**Dimensions:**
- Height: 200px
- Padding: 16px all sides
- Text size: 20-24pt
- Progress bar height: 40px

### 2. Shooter Panel (Bottom Panel)
```
┌─────────────────────────────────────┐
│      Available Shooters             │
│                                     │
│   ┌────────┐  ┌────────┐  ┌────────┐
│   │   🔴   │  │   🔵   │  │   🟡   │
│   │        │  │        │  │        │
│   │  Red   │  │  Blue  │  │ Yellow │
│   │   12   │  │   10   │  │    8   │
│   └────────┘  └────────┘  └────────┘
│                                     │
│   ┌────────┐  ┌────────┐  ┌────────┐
│   │   🟢   │  │   🟠   │  │   🟣   │
│   │        │  │        │  │        │
│   │ Green  │  │ Orange │  │ Purple │
│   │   15   │  │    9   │  │   10   │
│   └────────┘  └────────┘  └────────┘
└─────────────────────────────────────┘
```

**Shooter Element Structure:**
```
┌──────────────┐
│ ╔══════════╗ │ ← Selection border (yellow, 4px)
│ ║  ┌────┐  ║ │    when selected
│ ║  │ 🔴 │  ║ │ ← Color indicator (60x60px)
│ ║  └────┘  ║ │
│ ║          ║ │
│ ║   Red    ║ │ ← Color name (optional)
│ ║    12    ║ │ ← Ball count (large text)
│ ╚══════════╝ │
└──────────────┘
```

**Dimensions:**
- Panel height: 300px
- Element size: 100x120px
- Grid: 3 columns, auto rows
- Spacing: 10px between elements
- Color indicator: 60x60px centered
- Ball count text: 24pt, bold, centered

**States:**
1. **Normal**: Gray background, white text
2. **Selected**: Highlighted background, yellow border
3. **Used/Depleted**: Semi-transparent overlay (0.7 opacity black)

### 3. Shooter Action Feedback (Center Overlay)
```
┌─────────────────────┐
│  Red Shooter   ➜    │
│  Balls: 8           │
│  ◉ Moving...        │
└─────────────────────┘
```

**Elements:**
- Shooter name with color
- Balls remaining count
- Direction arrow (left ← or right →)
- Moving indicator (pulsing dot)

**Dimensions:**
- Size: 300x100px
- Position: Center of screen
- Background: Semi-transparent (0.9 opacity)
- Pulse animation: 0.95x to 1.05x scale

**Display Conditions:**
- Shows when shooter is active/moving
- Hides when shooter completes or is inactive
- Updates ball count in real-time

## Color Specifications

### Game Colors
```
Red:     rgb(230, 51,  51)   #E63333
Blue:    rgb(51,  102, 230)  #3366E6
Yellow:  rgb(242, 217, 51)   #F2D933
Green:   rgb(77,  204, 77)   #4DCC4D
Orange:  rgb(255, 153, 51)   #FF9933
Purple:  rgb(179, 77,  230)  #B34DE6
Black:   rgb(38,  38,  38)   #262626
White:   rgb(242, 242, 242)  #F2F2F2
```

### UI Colors
```
Background:     rgba(26,  26,  26,  0.8)  #1A1A1A with 80% opacity
Text:           rgb(242, 242, 242)        #F2F2F2
Highlight:      rgba(255, 255, 255, 0.3)  #FFFFFF with 30% opacity
Selection:      rgb(255, 230, 77)         #FFE64D
Progress Bar:   rgb(77,  204, 77)         #4DCC4D
Disabled:       rgba(0,   0,   0,   0.7)  #000000 with 70% opacity
```

## Typography

### Font: TextMeshPro Default
- **Title**: 28pt, Bold
- **Primary**: 24pt, Regular
- **Secondary**: 20pt, Regular
- **Small**: 16pt, Regular

### Text Hierarchy
1. Panel titles: 28pt bold
2. Shooter names: 20pt regular
3. Ball counts: 24pt bold
4. Progress text: 22pt regular
5. Percentages: 20pt regular

## Responsive Scaling

### 720p (1280x720)
- Scale factor: 0.67x
- Shooter elements: 67x80px
- Text: -4pt from base

### 1080p (1920x1080) - Base
- Scale factor: 1.0x
- Reference design

### 1440p (2560x1440)
- Scale factor: 1.33x
- Shooter elements: 133x160px
- Text: +4pt from base

### 4K (3840x2160)
- Scale factor: 2.0x
- All elements double size

## Animation Specifications

### Progress Bar
- **Type**: Lerp animation
- **Speed**: 2.0 (smooth transition)
- **Easing**: Linear

### Shooter Selection
- **Type**: Instant
- **Border**: Fade in 0.2s
- **Background**: Tint change 0.15s

### Moving Indicator
- **Type**: Pulse (scale)
- **Range**: 0.95x to 1.05x
- **Speed**: 2.0 cycles/second
- **Easing**: Sine wave

### Pixel Count Update
- **Type**: Lerp (optional)
- **Speed**: Fast (5.0)
- **Display**: Integer values only

## Touch Interaction

### Touch Targets
- Minimum size: 100x120px (exceeds iOS 44pt, Android 48dp)
- Active area: Full element bounds
- Feedback: Immediate visual change

### Touch States
1. **Normal**: Default appearance
2. **Pressed**: Slight scale down (0.95x)
3. **Selected**: Border + background change
4. **Disabled**: Grayed out, no interaction

## Safe Area Handling

### Notch/Punch-hole Devices
```
┌─────────────────────────────────────┐
│     [Notch Area - Keep Clear]       │ ← Safe area inset
├─────────────────────────────────────┤
│         PROGRESS DISPLAY            │
│                                     │
│         ... HUD Content ...         │
│                                     │
│       AVAILABLE SHOOTERS            │
├─────────────────────────────────────┤
│  [Navigation Bar - Keep Clear]      │ ← Safe area inset
└─────────────────────────────────────┘
```

**Insets Applied:**
- Top: Screen.safeArea.top
- Bottom: Screen.safeArea.bottom
- Left: Screen.safeArea.left
- Right: Screen.safeArea.right

## Accessibility Considerations

### Visual
- High contrast mode ready
- Colorblind patterns (future)
- Large touch targets
- Clear visual hierarchy

### Patterns for Colorblind Mode (Planned)
```
Red:     Diagonal lines ////
Blue:    Dots ····
Yellow:  Solid (no pattern)
Green:   Horizontal lines ═══
Orange:  Cross-hatch ####
Purple:  Vertical lines ||||
```

## Implementation Notes

1. All UI uses Unity UI Canvas system
2. TextMeshPro for all text rendering
3. Canvas Scaler handles resolution scaling
4. RectTransform anchors for responsive layout
5. Minimal GameObject hierarchy for performance
6. Single material/atlas for batching
7. No per-frame allocations

## Testing Checklist

- [ ] Test on 720p resolution
- [ ] Test on 1080p resolution
- [ ] Test on 1440p resolution
- [ ] Test on device with notch
- [ ] Verify touch target sizes
- [ ] Check text readability
- [ ] Validate color contrast
- [ ] Test selection feedback
- [ ] Verify animations smooth at 60fps
- [ ] Check safe area handling
- [ ] Test portrait orientation
- [ ] Validate all states (normal, selected, used)
