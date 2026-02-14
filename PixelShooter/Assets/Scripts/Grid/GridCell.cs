using System;
using System.Collections.Generic;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Represents a single cell in the grid that can contain up to 5 layers of pixels.
    /// Layers are stored bottom-to-top, with the topmost layer being the visible one.
    /// </summary>
    [Serializable]
    public class GridCell
    {
        private const int MaxLayers = 5;
        private List<PixelLayer> layers = new List<PixelLayer>();

        /// <summary>
        /// Gets the current visible layer (the topmost layer).
        /// Returns null if the cell is empty.
        /// </summary>
        public PixelLayer CurrentLayer => layers.Count > 0 ? layers[layers.Count - 1] : null;

        /// <summary>
        /// Gets the number of layers in this cell.
        /// </summary>
        public int LayerCount => layers.Count;

        /// <summary>
        /// Checks if the cell is empty (has no layers).
        /// </summary>
        public bool IsEmpty => layers.Count == 0;

        /// <summary>
        /// Adds a new layer to the cell.
        /// Layers are added from bottom to top.
        /// </summary>
        /// <param name="layer">The pixel layer to add</param>
        /// <returns>True if the layer was added, false if max layers reached</returns>
        public bool AddLayer(PixelLayer layer)
        {
            if (layers.Count >= MaxLayers)
            {
                return false;
            }

            layers.Add(layer);
            return true;
        }

        /// <summary>
        /// Destroys the current visible layer, revealing the layer beneath.
        /// </summary>
        /// <returns>The destroyed layer, or null if the cell was empty</returns>
        public PixelLayer DestroyCurrentLayer()
        {
            if (layers.Count == 0)
            {
                return null;
            }

            PixelLayer destroyedLayer = layers[layers.Count - 1];
            layers.RemoveAt(layers.Count - 1);
            return destroyedLayer;
        }

        /// <summary>
        /// Gets all layers in this cell (for editor/debug purposes).
        /// Returns layers from bottom to top.
        /// </summary>
        public IReadOnlyList<PixelLayer> GetAllLayers()
        {
            return layers.AsReadOnly();
        }
    }
}
