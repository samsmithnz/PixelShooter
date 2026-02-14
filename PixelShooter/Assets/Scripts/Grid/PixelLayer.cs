using System;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Represents a single pixel layer within a grid cell.
    /// Each layer has a color and can be destroyed to reveal the layer beneath.
    /// </summary>
    [Serializable]
    public class PixelLayer
    {
        public PixelColor color;

        public PixelLayer(PixelColor color)
        {
            this.color = color;
        }
    }
}
