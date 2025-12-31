using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject pixelPrefab;
    public int gridWidth = 8;
    public int gridHeight = 8;
    public int numberOfLayers = 2;
    public float pixelSize = 1f;
    public float pixelSpacing = 0.1f;
    
    private List<Pixel> activePixels = new List<Pixel>();
    private Dictionary<Vector2Int, List<Pixel>> pixelGrid = new Dictionary<Vector2Int, List<Pixel>>();

    public void InitializeGrid()
    {
        ClearGrid();
        
        for (int layer = 0; layer < numberOfLayers; layer++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    CreatePixel(x, y, layer);
                }
            }
        }
    }

    private void CreatePixel(int x, int y, int layer)
    {
        Vector3 position = new Vector3(
            x * (pixelSize + pixelSpacing),
            y * (pixelSize + pixelSpacing) + 2f, // Offset to be above shooters
            -layer * 0.1f // Layering in Z
        );

        GameObject pixelObj = Instantiate(pixelPrefab, position, Quaternion.identity, transform);
        Pixel pixel = pixelObj.GetComponent<Pixel>();
        
        if (pixel != null)
        {
            int number = Random.Range(1, 9); // Numbers 1-8 matching shooter count
            PixelColor color = (PixelColor)Random.Range(0, System.Enum.GetValues(typeof(PixelColor)).Length);
            pixel.Initialize(number, color, layer);
            
            activePixels.Add(pixel);
            
            Vector2Int gridPos = new Vector2Int(x, y);
            if (!pixelGrid.ContainsKey(gridPos))
            {
                pixelGrid[gridPos] = new List<Pixel>();
            }
            pixelGrid[gridPos].Add(pixel);
        }
    }

    public Pixel GetPixelInLineOfSight(Vector3 shooterPosition, int shooterNumber)
    {
        // Find the closest pixel directly above the shooter with matching number
        float shooterX = shooterPosition.x;
        Pixel closestPixel = null;
        float closestDistance = float.MaxValue;

        foreach (Pixel pixel in activePixels)
        {
            if (pixel == null) continue;
            if (pixel.number != shooterNumber) continue;
            
            float pixelX = pixel.transform.position.x;
            float pixelY = pixel.transform.position.y;
            
            // Check if pixel is in line of sight (same X position within tolerance)
            float tolerance = (pixelSize + pixelSpacing) / 2f;
            if (Mathf.Abs(pixelX - shooterX) < tolerance && pixelY > shooterPosition.y)
            {
                float distance = pixelY - shooterPosition.y;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPixel = pixel;
                }
            }
        }

        return closestPixel;
    }

    public void RemovePixel(Pixel pixel)
    {
        if (pixel == null) return;
        
        activePixels.Remove(pixel);
        
        // Remove from grid dictionary
        foreach (var kvp in pixelGrid)
        {
            if (kvp.Value.Contains(pixel))
            {
                kvp.Value.Remove(pixel);
                break;
            }
        }
    }

    public bool AreAllPixelsCleared()
    {
        return activePixels.Count == 0;
    }

    public int GetPixelCount()
    {
        return activePixels.Count;
    }

    private void ClearGrid()
    {
        foreach (Pixel pixel in activePixels)
        {
            if (pixel != null)
            {
                Destroy(pixel.gameObject);
            }
        }
        activePixels.Clear();
        pixelGrid.Clear();
    }

    void OnDestroy()
    {
        ClearGrid();
    }
}
