using System;
using System.Collections.Generic;

namespace PixelShooter.Gameplay
{
    /// <summary>
    /// Represents a shooter that moves left-to-right along the bottom row
    /// and fires at matching-color pixels with vertical line-of-sight
    /// </summary>
    public class Shooter
    {
        public Data.PixelColor Color { get; private set; }
        public int BallCount { get; private set; }
        public int CurrentColumn { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsComplete { get; private set; }

        private Core.GridSystem _grid;
        private int _bottomRow;

        public Shooter(Data.PixelColor color, int initialBallCount)
        {
            Color = color;
            BallCount = initialBallCount;
            CurrentColumn = -1;
            IsActive = false;
            IsComplete = false;
        }

        /// <summary>
        /// Activates the shooter on the given grid
        /// Shooter starts at column -1 (before the first column)
        /// </summary>
        public void Activate(Core.GridSystem grid)
        {
            if (IsActive || IsComplete)
                return;

            _grid = grid;
            _bottomRow = grid.Rows - 1; // Bottom row is the last row
            CurrentColumn = -1;
            IsActive = true;
        }

        /// <summary>
        /// Moves the shooter one step to the right
        /// Returns true if move was successful, false if at end or out of ammo
        /// </summary>
        public bool MoveNext()
        {
            if (!IsActive || IsComplete)
                return false;

            // Check if we've reached the end
            if (CurrentColumn >= _grid.Columns - 1)
            {
                IsComplete = true;
                IsActive = false;
                return false;
            }

            // Check if we're out of balls
            if (BallCount <= 0)
            {
                IsComplete = true;
                IsActive = false;
                return false;
            }

            CurrentColumn++;
            return true;
        }

        /// <summary>
        /// Checks if there's a matching-color pixel in vertical line-of-sight
        /// from current position
        /// </summary>
        public bool HasTargetInSight()
        {
            if (!IsActive || CurrentColumn < 0)
                return false;

            Data.GridPosition targetPosition = FindTargetInSight();
            return targetPosition.Row >= 0;
        }

        /// <summary>
        /// Finds the position of the first matching-color pixel in vertical line-of-sight
        /// Searches from top to bottom (row 0 to bottomRow - 1)
        /// Returns a position with row -1 if no target found
        /// </summary>
        private Data.GridPosition FindTargetInSight()
        {
            // Search from top to bottom
            for (int row = 0; row < _bottomRow; row++)
            {
                Data.GridPosition position = new Data.GridPosition(row, CurrentColumn);
                Data.Pixel pixel = _grid.GetTopPixelAt(position);

                if (pixel != null)
                {
                    // Found a pixel - check if it matches our color
                    if (pixel.Color == Color)
                    {
                        return position;
                    }
                    else
                    {
                        // Different color blocks line of sight
                        return new Data.GridPosition(-1, CurrentColumn);
                    }
                }
            }

            // No pixels in this column
            return new Data.GridPosition(-1, CurrentColumn);
        }

        /// <summary>
        /// Fires at the target in line-of-sight if one exists
        /// Returns the position of the destroyed pixel, or null if no shot fired
        /// </summary>
        public Data.GridPosition? Fire()
        {
            if (!IsActive || BallCount <= 0)
                return null;

            Data.GridPosition targetPosition = FindTargetInSight();
            if (targetPosition.Row < 0)
                return null; // No valid target

            // Fire at the target
            Data.Pixel destroyedPixel = _grid.RemoveTopPixelAt(targetPosition);
            if (destroyedPixel != null)
            {
                BallCount--;

                // Check if we're out of balls after this shot
                if (BallCount <= 0)
                {
                    IsComplete = true;
                    IsActive = false;
                }

                return targetPosition;
            }

            return null;
        }

        /// <summary>
        /// Performs a complete movement step: move to next position and fire if target available
        /// Returns list of positions where pixels were destroyed
        /// </summary>
        public List<Data.GridPosition> PerformStep()
        {
            List<Data.GridPosition> destroyedPositions = new List<Data.GridPosition>();

            if (!MoveNext())
                return destroyedPositions;

            // Fire at targets while we have line-of-sight
            // This handles the case where destroying one pixel reveals another
            while (HasTargetInSight() && BallCount > 0)
            {
                Data.GridPosition? destroyed = Fire();
                if (destroyed.HasValue)
                {
                    destroyedPositions.Add(destroyed.Value);
                }
                else
                {
                    break; // No more targets in sight
                }
            }

            return destroyedPositions;
        }

        /// <summary>
        /// Runs the complete shooter path from start to finish
        /// Returns all positions where pixels were destroyed
        /// </summary>
        public List<Data.GridPosition> RunCompletePath()
        {
            List<Data.GridPosition> allDestroyedPositions = new List<Data.GridPosition>();

            while (IsActive && !IsComplete)
            {
                List<Data.GridPosition> stepResults = PerformStep();
                allDestroyedPositions.AddRange(stepResults);
            }

            return allDestroyedPositions;
        }
    }
}
