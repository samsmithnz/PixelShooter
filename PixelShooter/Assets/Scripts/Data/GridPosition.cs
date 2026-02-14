namespace PixelShooter.Data
{
    /// <summary>
    /// Represents a position in the grid
    /// </summary>
    public struct GridPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public GridPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }

        public override bool Equals(object obj)
        {
            if (obj is GridPosition other)
            {
                return Row == other.Row && Column == other.Column;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Row, Column).GetHashCode();
        }
    }
}
