using UnityEngine;

public class Pixel : MonoBehaviour
{
    public int number;
    public PixelColor pixelColor;
    public int layer = 0;
    
    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
    }

    public void Initialize(int num, PixelColor color, int layerIndex)
    {
        number = num;
        pixelColor = color;
        layer = layerIndex;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetColorFromEnum(color);
        }
        
        if (textMesh != null)
        {
            textMesh.text = number.ToString();
            textMesh.color = Color.white;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private Color GetColorFromEnum(PixelColor color)
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

public enum PixelColor
{
    Red,
    Blue,
    Yellow,
    Green,
    Orange,
    Purple,
    Black,
    White
}
