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
            spriteRenderer.color = ColorUtility.GetColorFromPixelColor(color);
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
