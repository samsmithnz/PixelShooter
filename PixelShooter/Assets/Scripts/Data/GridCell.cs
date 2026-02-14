using System.Collections.Generic;
using System.Linq;

namespace PixelShooter.Data
{
    /// <summary>
    /// Represents a single cell in the grid that can contain multiple pixel layers
    /// Maximum of 5 layers per cell as per GDD
    /// </summary>
    public class GridCell
    {
        private const int MaxLayers = 5;
        private List<Pixel> _pixels;

        public GridPosition Position { get; private set; }
        public bool IsEmpty => _pixels.Count == 0;
        public int LayerCount => _pixels.Count;

        public GridCell(GridPosition position)
        {
            Position = position;
            _pixels = new List<Pixel>();
        }

        /// <summary>
        /// Gets the top-most (visible) pixel in this cell
        /// </summary>
        public Pixel GetTopPixel()
        {
            if (IsEmpty)
                return null;

            return _pixels[_pixels.Count - 1];
        }

        /// <summary>
        /// Adds a pixel to this cell at the specified layer
        /// Layers are 0-based, with 0 being the bottom layer
        /// </summary>
        public bool AddPixel(Pixel pixel)
        {
            if (_pixels.Count >= MaxLayers)
                return false;

            _pixels.Add(pixel);
            return true;
        }

        /// <summary>
        /// Removes the top-most pixel from this cell
        /// Returns the removed pixel, or null if cell was empty
        /// </summary>
        public Pixel RemoveTopPixel()
        {
            if (IsEmpty)
                return null;

            Pixel removedPixel = _pixels[_pixels.Count - 1];
            _pixels.RemoveAt(_pixels.Count - 1);
            return removedPixel;
        }

        /// <summary>
        /// Gets all pixels in this cell, ordered from bottom to top
        /// </summary>
        public IReadOnlyList<Pixel> GetAllPixels()
        {
            return _pixels.AsReadOnly();
        }
    }
}
