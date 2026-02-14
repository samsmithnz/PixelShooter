using System;

namespace PixelShooter.Data
{
    /// <summary>
    /// Represents an immutable position in the grid
    /// </summary>
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int Row { get; }
        public int Column { get; }

        public GridPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"({Row}, {Column})";
        }

        public bool Equals(GridPosition other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Row, Column).GetHashCode();
        }

        public static bool operator ==(GridPosition left, GridPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GridPosition left, GridPosition right)
        {
            return !left.Equals(right);
        }
    }
}
