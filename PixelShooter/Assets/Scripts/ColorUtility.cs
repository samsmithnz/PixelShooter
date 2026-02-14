using UnityEngine;

public static class ColorUtility
{
    public static Color GetColorFromPixelColor(PixelColor color)
    {
        switch (color)
        {
            case PixelColor.Red: return Color.red;
            case PixelColor.Blue: return Color.blue;
            case PixelColor.Yellow: return Color.yellow;
            case PixelColor.Green: return Color.green;
            case PixelColor.Orange: return new Color(1f, 0.5f, 0f);
            case PixelColor.Purple: return new Color(0.5f, 0f, 0.5f);
            case PixelColor.Black: return Color.black;
            case PixelColor.White: return Color.white;
            default: return Color.gray;
        }
    }
}
