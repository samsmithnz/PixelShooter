using UnityEngine;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Manages rendering of the entire pixel grid.
    /// Creates and positions pixel renderers for each cell in the grid.
    /// </summary>
    public class GridRenderer : MonoBehaviour
    {
        [Header("Grid Configuration")]
        [Tooltip("The level definition to load")]
        public LevelDefinition levelDefinition;

        [Header("Rendering Settings")]
        [Tooltip("Size of each pixel cell in world units")]
        public float cellSize = 1f;

        [Tooltip("Spacing between cells")]
        public float cellSpacing = 0.1f;

        [Tooltip("Sprite to use for pixels (white square recommended)")]
        public Sprite pixelSprite;

        [Header("Runtime Data")]
        private GridData gridData;
        private PixelRenderer[,] pixelRenderers;
        private bool isInitialized = false;

        /// <summary>
        /// Event fired when the grid has been fully initialized.
        /// </summary>
        public event System.Action OnGridInitialized;

        /// <summary>
        /// Gets whether the grid has been initialized.
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Gets the current grid data.
        /// </summary>
        public GridData GridData => gridData;

        private void Start()
        {
            if (levelDefinition != null)
            {
                LoadLevel(levelDefinition);
            }
            else
            {
                Debug.LogWarning("No level definition assigned to GridRenderer");
            }
        }

        /// <summary>
        /// Loads a level from a LevelDefinition and creates the visual grid.
        /// </summary>
        public void LoadLevel(LevelDefinition level)
        {
            // Clear any existing grid
            ClearGrid();

            // Create grid data from level definition
            gridData = level.CreateGridData();
            levelDefinition = level;

            // Create visual representation
            CreateGridVisuals();
            
            // Mark as initialized and fire event
            isInitialized = true;
            OnGridInitialized?.Invoke();
        }

        /// <summary>
        /// Creates the visual representation of the grid.
        /// </summary>
        private void CreateGridVisuals()
        {
            if (gridData == null)
            {
                Debug.LogError("GridData is null, cannot create visuals");
                return;
            }

            pixelRenderers = new PixelRenderer[gridData.Width, gridData.Height];

            // Calculate grid centering offset
            float totalWidth = gridData.Width * (cellSize + cellSpacing) - cellSpacing;
            float totalHeight = gridData.Height * (cellSize + cellSpacing) - cellSpacing;
            Vector3 offset = new Vector3(-totalWidth / 2f, -totalHeight / 2f, 0f);

            // Create pixel renderers for each cell
            for (int x = 0; x < gridData.Width; x++)
            {
                for (int y = 0; y < gridData.Height; y++)
                {
                    Vector3 position = transform.position + offset + new Vector3(
                        x * (cellSize + cellSpacing),
                        y * (cellSize + cellSpacing),
                        0f
                    );

                    GameObject pixelObj = CreatePixelObject(x, y, position);
                    PixelRenderer renderer = pixelObj.GetComponent<PixelRenderer>();
                    pixelRenderers[x, y] = renderer;

                    // Update the visual based on current cell state
                    UpdateCellVisual(x, y);
                }
            }
        }

        /// <summary>
        /// Creates a pixel GameObject at the specified position.
        /// </summary>
        private GameObject CreatePixelObject(int x, int y, Vector3 position)
        {
            GameObject pixelObj = new GameObject($"Pixel_{x}_{y}");
            pixelObj.transform.parent = transform;
            pixelObj.transform.position = position;
            pixelObj.transform.localScale = Vector3.one * cellSize;

            SpriteRenderer spriteRenderer = pixelObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = pixelSprite;
            spriteRenderer.sortingOrder = 0;

            PixelRenderer renderer = pixelObj.AddComponent<PixelRenderer>();

            return pixelObj;
        }

        /// <summary>
        /// Updates the visual representation of a specific cell.
        /// </summary>
        private void UpdateCellVisual(int x, int y)
        {
            if (pixelRenderers == null || x < 0 || x >= pixelRenderers.GetLength(0) || y < 0 || y >= pixelRenderers.GetLength(1))
            {
                return;
            }

            GridCell cell = gridData.GetCell(x, y);
            PixelRenderer renderer = pixelRenderers[x, y];

            if (cell == null || renderer == null)
            {
                return;
            }

            if (cell.IsEmpty)
            {
                renderer.Hide();
            }
            else
            {
                renderer.Show();
                renderer.SetColor(cell.CurrentLayer.color);
            }
        }

        /// <summary>
        /// Destroys the current layer at the specified cell and updates the visual.
        /// </summary>
        public void DestroyPixelAt(int x, int y)
        {
            GridCell cell = gridData.GetCell(x, y);
            if (cell != null && !cell.IsEmpty)
            {
                cell.DestroyCurrentLayer();
                UpdateCellVisual(x, y);
            }
        }

        /// <summary>
        /// Gets the pixel in line of sight from a given position (vertical line upward).
        /// </summary>
        public bool GetPixelInLineOfSight(Vector3 shooterPosition, PixelColor targetColor, out int gridX, out int gridY)
        {
            gridX = -1;
            gridY = -1;

            // Convert world position to grid coordinates
            float totalWidth = gridData.Width * (cellSize + cellSpacing) - cellSpacing;
            float totalHeight = gridData.Height * (cellSize + cellSpacing) - cellSpacing;
            Vector3 offset = new Vector3(-totalWidth / 2f, -totalHeight / 2f, 0f);
            Vector3 gridPos = shooterPosition - transform.position - offset;

            // Find the X column
            int column = Mathf.RoundToInt(gridPos.x / (cellSize + cellSpacing));
            
            if (column < 0 || column >= gridData.Width)
                return false;

            // Search upward from bottom for first non-empty cell with matching color
            for (int y = 0; y < gridData.Height; y++)
            {
                GridCell cell = gridData.GetCell(column, y);
                if (cell != null && !cell.IsEmpty && cell.CurrentLayer.color == targetColor)
                {
                    gridX = column;
                    gridY = y;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Counts total number of pixels with a specific color across all layers.
        /// </summary>
        public int CountPixelsOfColor(PixelColor color)
        {
            if (gridData == null)
                return 0;

            return gridData.CountPixelsOfColor(color);
        }

        /// <summary>
        /// Clears the entire grid and destroys all pixel renderers.
        /// </summary>
        private void ClearGrid()
        {
            if (pixelRenderers != null)
            {
                for (int x = 0; x < pixelRenderers.GetLength(0); x++)
                {
                    for (int y = 0; y < pixelRenderers.GetLength(1); y++)
                    {
                        if (pixelRenderers[x, y] != null)
                        {
                            Destroy(pixelRenderers[x, y].gameObject);
                        }
                    }
                }
                pixelRenderers = null;
            }

            gridData = null;
        }

        private void OnDestroy()
        {
            ClearGrid();
        }
    }
}
