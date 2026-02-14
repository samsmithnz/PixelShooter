using UnityEngine;

public class Shooter : MonoBehaviour
{
    public int number;
    public int ballCount;
    public PixelColor shooterColor;
    
    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;
    private bool isActive = false;
    private float moveSpeed = 2f;
    private float shootInterval = 0.2f;
    private float lastShootTime;
    private GridManager gridManager;
    private GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
        gridManager = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Initialize(int num, int balls, PixelColor color)
    {
        number = num;
        ballCount = balls;
        shooterColor = color;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ColorUtility.GetColorFromPixelColor(color);
        }
        
        if (textMesh != null)
        {
            textMesh.text = $"{number}\n{ballCount}";
            textMesh.color = Color.white;
        }
    }

    public void Activate()
    {
        isActive = true;
        lastShootTime = Time.time;
    }

    void Update()
    {
        if (!isActive) return;

        // Move from left to right
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        
        // Shoot at intervals
        if (Time.time - lastShootTime >= shootInterval)
        {
            ShootAtPixels();
            lastShootTime = Time.time;
        }
    }

    private void ShootAtPixels()
    {
        if (ballCount <= 0) return;

        if (gridManager != null)
        {
            Pixel targetPixel = gridManager.GetPixelInLineOfSight(transform.position, number);
            if (targetPixel != null)
            {
                ballCount--;
                targetPixel.Destroy();
                gridManager.RemovePixel(targetPixel);
                UpdateDisplay();
                
                if (ballCount <= 0)
                {
                    CheckIfShooterComplete();
                }
            }
        }
    }

    private void CheckIfShooterComplete()
    {
        if (gameManager != null)
        {
            gameManager.OnShooterComplete(this);
        }
    }

    private void UpdateDisplay()
    {
        if (textMesh != null)
        {
            textMesh.text = $"{number}\n{ballCount}";
        }
    }
}
