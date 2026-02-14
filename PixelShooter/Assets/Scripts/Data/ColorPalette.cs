using UnityEngine;

namespace PixelShooter.Data
{
    /// <summary>
    /// Defines the color palette for the game with accessibility support
    /// </summary>
    public static class ColorPalette
    {
        // Primary game colors
        public static readonly Color Red = new Color(0.9f, 0.2f, 0.2f);
        public static readonly Color Blue = new Color(0.2f, 0.4f, 0.9f);
        public static readonly Color Yellow = new Color(0.95f, 0.85f, 0.2f);
        public static readonly Color Green = new Color(0.3f, 0.8f, 0.3f);
        public static readonly Color Orange = new Color(1f, 0.6f, 0.2f);
        public static readonly Color Purple = new Color(0.7f, 0.3f, 0.9f);
        public static readonly Color Black = new Color(0.15f, 0.15f, 0.15f);
        public static readonly Color White = new Color(0.95f, 0.95f, 0.95f);

        // UI colors
        public static readonly Color UIBackground = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        public static readonly Color UIText = new Color(0.95f, 0.95f, 0.95f);
        public static readonly Color UIHighlight = new Color(1f, 1f, 1f, 0.3f);
        public static readonly Color UISelected = new Color(1f, 0.9f, 0.3f);

        public static string GetColorName(Color color)
        {
            if (ColorDistance(color, Red) < 0.1f) return "Red";
            if (ColorDistance(color, Blue) < 0.1f) return "Blue";
            if (ColorDistance(color, Yellow) < 0.1f) return "Yellow";
            if (ColorDistance(color, Green) < 0.1f) return "Green";
            if (ColorDistance(color, Orange) < 0.1f) return "Orange";
            if (ColorDistance(color, Purple) < 0.1f) return "Purple";
            if (ColorDistance(color, Black) < 0.1f) return "Black";
            if (ColorDistance(color, White) < 0.1f) return "White";
            return "Unknown";
        }

        private static float ColorDistance(Color a, Color b)
        {
            return Mathf.Sqrt(
                Mathf.Pow(a.r - b.r, 2) +
                Mathf.Pow(a.g - b.g, 2) +
                Mathf.Pow(a.b - b.b, 2)
            );
        }
    }
}
