namespace PixelShooter.Data
{
    /// <summary>
    /// Represents a single pixel in the grid with color and layer information
    /// </summary>
    public class Pixel
    {
        public PixelColor Color { get; private set; }
        public GridPosition Position { get; private set; }
        public int Layer { get; private set; }

        public Pixel(PixelColor color, GridPosition position, int layer)
        {
            Color = color;
            Position = position;
            Layer = layer;
        }

        public override string ToString()
        {
            return $"{Color} pixel at {Position}, layer {Layer}";
        }
    }
}
