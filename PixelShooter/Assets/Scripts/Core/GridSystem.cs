using System.Collections.Generic;

namespace PixelShooter.Core
{
    /// <summary>
    /// Manages the game grid with layered pixel cells
    /// Default size is 8x8 as per GDD
    /// </summary>
    public class GridSystem
    {
        private Data.GridCell[,] _grid;
        private int _rows;
        private int _columns;

        public int Rows => _rows;
        public int Columns => _columns;

        public GridSystem(int rows = 8, int columns = 8)
        {
            _rows = rows;
            _columns = columns;
            _grid = new Data.GridCell[rows, columns];

            // Initialize all cells
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    _grid[row, col] = new Data.GridCell(new Data.GridPosition(row, col));
                }
            }
        }

        /// <summary>
        /// Gets the cell at the specified position
        /// Returns null if position is out of bounds
        /// </summary>
        public Data.GridCell GetCell(Data.GridPosition position)
        {
            if (!IsValidPosition(position))
                return null;

            return _grid[position.Row, position.Column];
        }

        /// <summary>
        /// Checks if a position is within grid boundaries
        /// </summary>
        public bool IsValidPosition(Data.GridPosition position)
        {
            return position.Row >= 0 && position.Row < _rows &&
                   position.Column >= 0 && position.Column < _columns;
        }

        /// <summary>
        /// Adds a pixel to the grid at the specified position
        /// </summary>
        public bool AddPixel(Data.Pixel pixel)
        {
            Data.GridCell cell = GetCell(pixel.Position);
            if (cell == null)
                return false;

            return cell.AddPixel(pixel);
        }

        /// <summary>
        /// Removes and returns the top pixel at the specified position
        /// </summary>
        public Data.Pixel RemoveTopPixelAt(Data.GridPosition position)
        {
            Data.GridCell cell = GetCell(position);
            if (cell == null)
                return null;

            return cell.RemoveTopPixel();
        }

        /// <summary>
        /// Gets the top (visible) pixel at the specified position
        /// </summary>
        public Data.Pixel GetTopPixelAt(Data.GridPosition position)
        {
            Data.GridCell cell = GetCell(position);
            if (cell == null)
                return null;

            return cell.GetTopPixel();
        }

        /// <summary>
        /// Checks if the grid is completely empty (all pixels cleared)
        /// </summary>
        public bool IsGridEmpty()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    if (!_grid[row, col].IsEmpty)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Counts total number of pixels of a specific color in the entire grid
        /// Counts all layers, not just visible pixels
        /// </summary>
        public int CountPixelsOfColor(Data.PixelColor color)
        {
            int count = 0;
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    Data.GridCell cell = _grid[row, col];
                    foreach (Data.Pixel pixel in cell.GetAllPixels())
                    {
                        if (pixel.Color == color)
                            count++;
                    }
                }
            }
            return count;
        }
    }
}
