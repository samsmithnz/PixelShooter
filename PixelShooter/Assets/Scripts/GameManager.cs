using System.Collections.Generic;
using UnityEngine;
using PixelShooter.Grid;

public class GameManager : MonoBehaviour
{
    [Header("Grid System")]
    public GridRenderer gridRenderer;

    [Header("Shooter Configuration")]
    public GameObject shooterPrefab;
    public Transform shooterSelectionArea;
    public TextMesh statusText;
    
    [Header("Shooter Positioning")]
    [Tooltip("Horizontal spacing between shooters")]
    public float shooterSpacing = 1.2f;
    [Tooltip("Starting X position for first shooter")]
    public float shooterStartX = -4f;
    
    private List<Shooter> availableShooters = new List<Shooter>();
    private Shooter activeShooter = null;

    void Start()
    {
        Debug.Log("[GameManager] Start() called");
        
        // Validate required references
        if (gridRenderer == null)
        {
            Debug.LogError("[GameManager] GridRenderer is NOT assigned! Please assign it in the Inspector.");
            return;
        }
        
        if (shooterPrefab == null)
        {
            Debug.LogError("[GameManager] ShooterPrefab is NOT assigned! Please assign it in the Inspector.");
            return;
        }
        
        if (shooterSelectionArea == null)
        {
            Debug.LogWarning("[GameManager] ShooterSelectionArea is not assigned. Shooters may not appear correctly.");
        }
        
        // Subscribe to grid initialization event
        gridRenderer.OnGridInitialized += OnGridReady;
        
        // If grid is already initialized, initialize game immediately
        if (gridRenderer.IsInitialized)
        {
            OnGridReady();
        }
    }

    private void OnDestroy()
    {
        if (gridRenderer != null)
        {
            gridRenderer.OnGridInitialized -= OnGridReady;
        }
    }

    private void OnGridReady()
    {
        Debug.Log("[GameManager] Grid is ready, initializing game...");
        InitializeGame();
    }

    public void InitializeGame()
    {
        Debug.Log("[GameManager] InitializeGame() called");
        
        if (gridRenderer.GridData == null)
        {
            Debug.LogError("[GameManager] GridRenderer.GridData is null! Grid may not have initialized properly.");
            return;
        }

        // Create shooters based on colors present in grid
        CreateShooters();
        
        Debug.Log($"[GameManager] Created {availableShooters.Count} shooters");
        UpdateStatus();
    }

    private void CreateShooters()
    {
        // Clear existing shooters
        foreach (Shooter shooter in availableShooters)
        {
            if (shooter != null)
            {
                Destroy(shooter.gameObject);
            }
        }
        availableShooters.Clear();

        // Count pixels per color to determine ball counts
        Dictionary<PixelColor, int> pixelCounts = CountPixelsPerColor();
        Debug.Log($"[GameManager] Found {pixelCounts.Count} different pixel colors");
        
        int shooterIndex = 0;
        foreach (var kvp in pixelCounts)
        {
            PixelColor color = kvp.Key;
            int ballCount = kvp.Value;
            
            if (ballCount > 0)
            {
                Debug.Log($"[GameManager] Creating shooter for color {color} with {ballCount} balls");
                CreateShooter(color, ballCount, shooterIndex);
                shooterIndex++;
            }
        }
    }

    private Dictionary<PixelColor, int> CountPixelsPerColor()
    {
        Dictionary<PixelColor, int> counts = new Dictionary<PixelColor, int>();
        
        if (gridRenderer != null && gridRenderer.GridData != null)
        {
            // Count pixels of each color
            foreach (PixelColor color in System.Enum.GetValues(typeof(PixelColor)))
            {
                int count = gridRenderer.CountPixelsOfColor(color);
                if (count > 0)
                {
                    counts[color] = count;
                }
            }
        }
        
        return counts;
    }

    private void CreateShooter(PixelColor color, int ballCount, int index)
    {
        Vector3 position = new Vector3(
            index * shooterSpacing + shooterStartX, 
            -2f, 
            0f
        );

        GameObject shooterObj = Instantiate(shooterPrefab, position, Quaternion.identity, shooterSelectionArea);
        Shooter shooter = shooterObj.GetComponent<Shooter>();
        
        if (shooter != null)
        {
            shooter.Initialize(color, ballCount);
            availableShooters.Add(shooter);
            
            // Add click detection
            BoxCollider2D collider = shooterObj.GetComponent<BoxCollider2D>();
            if (collider == null)
            {
                collider = shooterObj.AddComponent<BoxCollider2D>();
            }
        }
    }

    void Update()
    {
        // Handle shooter selection with mouse click
        if (Input.GetMouseButtonDown(0) && activeShooter == null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null)
            {
                Shooter shooter = hit.collider.GetComponent<Shooter>();
                if (shooter != null && availableShooters.Contains(shooter))
                {
                    ActivateShooter(shooter);
                }
            }
        }

        // Check if game is complete
        if (gridRenderer != null && gridRenderer.GridData != null && gridRenderer.GridData.IsGridEmpty())
        {
            UpdateStatus("All pixels cleared! Game complete!");
        }
    }

    private void ActivateShooter(Shooter shooter)
    {
        activeShooter = shooter;
        availableShooters.Remove(shooter);
        
        // Move shooter to active position
        shooter.transform.position = new Vector3(-5f, 0f, 0f);
        shooter.Activate();
        
        UpdateStatus($"Shooter {shooter.shooterColor} activated!");
    }

    public void OnShooterComplete(Shooter shooter)
    {
        if (shooter == activeShooter)
        {
            activeShooter = null;
            Destroy(shooter.gameObject);
            UpdateStatus($"Shooter {shooter.shooterColor} complete! Select next shooter.");
        }
    }

    private void UpdateStatus(string message = null)
    {
        if (statusText != null)
        {
            if (message != null)
            {
                statusText.text = message;
            }
            else
            {
                int pixelCount = gridRenderer != null && gridRenderer.GridData != null 
                    ? GetTotalPixelCount() : 0;
                statusText.text = $"Pixels remaining: {pixelCount} | Shooters: {availableShooters.Count}";
            }
        }
    }

    private int GetTotalPixelCount()
    {
        int total = 0;
        GridData grid = gridRenderer.GridData;
        
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                GridCell cell = grid.GetCell(x, y);
                if (cell != null)
                {
                    total += cell.LayerCount;
                }
            }
        }
        
        return total;
    }
}
