using UnityEngine;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Renders a single pixel with the specified color.
    /// Uses Unity's SpriteRenderer to display the pixel as a colored square.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PixelRenderer : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private PixelColor pixelColor;

        /// <summary>
        /// Gets the current pixel color.
        /// </summary>
        public PixelColor PixelColor => pixelColor;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Sets the pixel color and updates the visual representation.
        /// </summary>
        public void SetColor(PixelColor color)
        {
            pixelColor = color;
            spriteRenderer.color = GetColorFromPixelColor(color);
        }

        /// <summary>
        /// Hides the pixel (for empty cells).
        /// </summary>
        public void Hide()
        {
            spriteRenderer.enabled = false;
        }

        /// <summary>
        /// Shows the pixel.
        /// </summary>
        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        /// <summary>
        /// Converts a PixelColor enum to a Unity Color.
        /// </summary>
        private Color GetColorFromPixelColor(PixelColor pixelColor)
        {
            switch (pixelColor)
            {
                case PixelColor.Red:
                    return new Color(1f, 0f, 0f, 1f);
                case PixelColor.Blue:
                    return new Color(0f, 0f, 1f, 1f);
                case PixelColor.Yellow:
                    return new Color(1f, 1f, 0f, 1f);
                case PixelColor.Green:
                    return new Color(0f, 1f, 0f, 1f);
                case PixelColor.Orange:
                    return new Color(1f, 0.5f, 0f, 1f);
                case PixelColor.Purple:
                    return new Color(0.5f, 0f, 0.5f, 1f);
                case PixelColor.Black:
                    return new Color(0f, 0f, 0f, 1f);
                case PixelColor.White:
                    return new Color(1f, 1f, 1f, 1f);
                default:
                    return Color.magenta; // Error color
            }
        }
    }
}
