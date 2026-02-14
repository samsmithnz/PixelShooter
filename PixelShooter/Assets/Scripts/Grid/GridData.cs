using System;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Represents the entire game grid with configurable size.
    /// Default size is 8x8, but supports 5x5 to 10x10+ grids.
    /// </summary>
    [Serializable]
    public class GridData
    {
        public const int DefaultGridSize = 8;
        public const int MinGridSize = 5;
        public const int MaxRecommendedGridSize = 10;

        private int width;
        private int height;
        private GridCell[,] cells;

        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int Height => height;

        /// <summary>
        /// Creates a new grid with the specified dimensions.
        /// </summary>
        /// <param name="width">Grid width (default: 8)</param>
        /// <param name="height">Grid height (default: 8)</param>
        public GridData(int width = DefaultGridSize, int height = DefaultGridSize)
        {
            this.width = width;
            this.height = height;
            cells = new GridCell[width, height];

            // Initialize all cells
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new GridCell();
                }
            }
        }

        /// <summary>
        /// Gets the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">X coordinate (0 to Width-1)</param>
        /// <param name="y">Y coordinate (0 to Height-1)</param>
        /// <returns>The GridCell at the specified position, or null if out of bounds</returns>
        public GridCell GetCell(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return null;
            }

            return cells[x, y];
        }

        /// <summary>
        /// Counts the total number of pixels in the grid with the specified color.
        /// </summary>
        /// <param name="color">The color to count</param>
        /// <returns>Total count of pixels with the specified color across all layers</returns>
        public int CountPixelsOfColor(PixelColor color)
        {
            int count = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridCell cell = cells[x, y];
                    if (cell != null)
                    {
                        foreach (var layer in cell.GetAllLayers())
                        {
                            if (layer.color == color)
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Checks if the grid is completely empty (all pixels destroyed).
        /// </summary>
        public bool IsGridEmpty()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!cells[x, y].IsEmpty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
