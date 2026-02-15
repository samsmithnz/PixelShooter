using UnityEngine;
using PixelShooter.Grid;

public class Shooter : MonoBehaviour
{
    public PixelColor shooterColor;
    public int ballCount;
    
    [Header("Movement Settings")]
    [Tooltip("Boundary X position where shooter is considered out of bounds")]
    public float rightBoundary = 10f;
    
    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh;
    private bool isActive = false;
    private float moveSpeed = 2f;
    private float shootInterval = 0.2f;
    private float lastShootTime;
    private GridRenderer gridRenderer;
    private GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
        gridRenderer = FindAnyObjectByType<GridRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void Initialize(PixelColor color, int balls)
    {
        shooterColor = color;
        ballCount = balls;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = ColorUtility.GetColorFromPixelColor(color);
        }
        
        if (textMesh != null)
        {
            textMesh.text = $"{color}\n{ballCount}";
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

        // Check if out of bounds
        if (transform.position.x > rightBoundary)
        {
            CheckIfShooterComplete();
        }
    }

    private void ShootAtPixels()
    {
        if (ballCount <= 0) return;

        if (gridRenderer != null)
        {
            int gridX, gridY;
            if (gridRenderer.GetPixelInLineOfSight(transform.position, shooterColor, out gridX, out gridY))
            {
                ballCount--;
                gridRenderer.DestroyPixelAt(gridX, gridY);
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
            textMesh.text = $"{shooterColor}\n{ballCount}";
        }
    }
}
