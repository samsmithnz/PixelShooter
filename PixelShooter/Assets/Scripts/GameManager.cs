using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject shooterPrefab;
    public Transform shooterSelectionArea;
    public Transform activeShooterPosition;
    public TextMesh statusText;
    
    private List<Shooter> availableShooters = new List<Shooter>();
    private Shooter activeShooter = null;
    private int numberOfShooters = 8;

    void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        // Initialize the grid
        if (gridManager != null)
        {
            gridManager.InitializeGrid();
        }

        // Create shooters
        CreateShooters();
        
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

        // Count pixels per number to determine ball counts
        Dictionary<int, int> pixelCounts = CountPixelsPerNumber();

        // Create shooters with appropriate ball counts
        for (int i = 0; i < numberOfShooters; i++)
        {
            int shooterNumber = i + 1;
            int ballCount = pixelCounts.ContainsKey(shooterNumber) ? pixelCounts[shooterNumber] : 0;
            
            if (ballCount > 0)
            {
                CreateShooter(shooterNumber, ballCount, i);
            }
        }
    }

    private Dictionary<int, int> CountPixelsPerNumber()
    {
        Dictionary<int, int> counts = new Dictionary<int, int>();
        
        if (gridManager != null)
        {
            // Get all pixels and count by number
            Pixel[] allPixels = FindObjectsOfType<Pixel>();
            foreach (Pixel pixel in allPixels)
            {
                if (!counts.ContainsKey(pixel.number))
                {
                    counts[pixel.number] = 0;
                }
                counts[pixel.number]++;
            }
        }
        
        return counts;
    }

    private void CreateShooter(int number, int ballCount, int index)
    {
        Vector3 position = new Vector3(
            index * 1.2f - 4f, 
            -2f, 
            0f
        );

        GameObject shooterObj = Instantiate(shooterPrefab, position, Quaternion.identity, shooterSelectionArea);
        Shooter shooter = shooterObj.GetComponent<Shooter>();
        
        if (shooter != null)
        {
            PixelColor color = (PixelColor)(index % System.Enum.GetValues(typeof(PixelColor)).Length);
            shooter.Initialize(number, ballCount, color);
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
        if (gridManager != null && gridManager.AreAllPixelsCleared())
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
        
        UpdateStatus($"Shooter {shooter.number} activated!");
    }

    public void OnShooterComplete(Shooter shooter)
    {
        if (shooter == activeShooter)
        {
            activeShooter = null;
            Destroy(shooter.gameObject);
            UpdateStatus($"Shooter {shooter.number} complete! Select next shooter.");
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
                int pixelCount = gridManager != null ? gridManager.GetPixelCount() : 0;
                statusText.text = $"Pixels remaining: {pixelCount} | Shooters: {availableShooters.Count}";
            }
        }
    }
}
