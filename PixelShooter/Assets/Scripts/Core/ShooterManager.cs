using System;
using System.Collections.Generic;

namespace PixelShooter.Core
{
    /// <summary>
    /// Manages shooter activation and ensures only one shooter is active at a time
    /// Handles game state and win condition checking
    /// </summary>
    public class ShooterManager
    {
        private GridSystem _grid;
        private Gameplay.Shooter _activeShooter;
        private List<Gameplay.Shooter> _availableShooters;
        private List<Gameplay.Shooter> _usedShooters;

        public Gameplay.Shooter ActiveShooter => _activeShooter;
        public IReadOnlyList<Gameplay.Shooter> AvailableShooters => _availableShooters.AsReadOnly();
        public IReadOnlyList<Gameplay.Shooter> UsedShooters => _usedShooters.AsReadOnly();
        public bool HasActiveShooter => _activeShooter != null && _activeShooter.IsActive;

        public event Action<Gameplay.Shooter> OnShooterActivated;
        public event Action<Gameplay.Shooter, List<Data.GridPosition>> OnShooterCompleted;
        public event Action<Data.GridPosition> OnPixelDestroyed;
        public event Action OnGameWon;

        public ShooterManager(GridSystem grid)
        {
            _grid = grid;
            _availableShooters = new List<Gameplay.Shooter>();
            _usedShooters = new List<Gameplay.Shooter>();
        }

        /// <summary>
        /// Adds a shooter to the available pool
        /// Ball count is typically set to the total number of matching pixels in the grid
        /// </summary>
        public void AddShooter(Data.PixelColor color, int ballCount)
        {
            Gameplay.Shooter shooter = new Gameplay.Shooter(color, ballCount);
            _availableShooters.Add(shooter);
        }

        /// <summary>
        /// Creates and adds shooters based on the current grid state
        /// Each shooter gets balls equal to the number of matching pixels in the grid
        /// </summary>
        public void InitializeShootersFromGrid()
        {
            _availableShooters.Clear();
            _usedShooters.Clear();

            // Get all unique colors in the grid
            HashSet<Data.PixelColor> colorsInGrid = new HashSet<Data.PixelColor>();
            for (int row = 0; row < _grid.Rows; row++)
            {
                for (int col = 0; col < _grid.Columns; col++)
                {
                    Data.GridPosition position = new Data.GridPosition(row, col);
                    Data.GridCell cell = _grid.GetCell(position);
                    foreach (Data.Pixel pixel in cell.GetAllPixels())
                    {
                        colorsInGrid.Add(pixel.Color);
                    }
                }
            }

            // Create a shooter for each color
            foreach (Data.PixelColor color in colorsInGrid)
            {
                int ballCount = _grid.CountPixelsOfColor(color);
                AddShooter(color, ballCount);
            }
        }

        /// <summary>
        /// Activates a shooter if no other shooter is currently active
        /// Returns true if activation was successful
        /// </summary>
        public bool ActivateShooter(Gameplay.Shooter shooter)
        {
            if (HasActiveShooter)
                return false;

            if (!_availableShooters.Contains(shooter))
                return false;

            shooter.Activate(_grid);
            _activeShooter = shooter;
            OnShooterActivated?.Invoke(shooter);
            return true;
        }

        /// <summary>
        /// Executes one step of the active shooter's movement
        /// Returns positions of destroyed pixels
        /// </summary>
        public List<Data.GridPosition> ExecuteShooterStep()
        {
            if (!HasActiveShooter)
                return new List<Data.GridPosition>();

            List<Data.GridPosition> destroyed = _activeShooter.PerformStep();

            // Notify about each destroyed pixel
            foreach (Data.GridPosition position in destroyed)
            {
                OnPixelDestroyed?.Invoke(position);
            }

            // Check if shooter is complete
            if (_activeShooter.IsComplete)
            {
                CompleteActiveShooter(destroyed);
            }

            // Check win condition
            CheckWinCondition();

            return destroyed;
        }

        /// <summary>
        /// Runs the active shooter's complete path from start to finish
        /// Returns all positions where pixels were destroyed
        /// </summary>
        public List<Data.GridPosition> ExecuteShooterCompletePath()
        {
            if (!HasActiveShooter)
                return new List<Data.GridPosition>();

            List<Data.GridPosition> allDestroyed = new List<Data.GridPosition>();

            while (HasActiveShooter)
            {
                List<Data.GridPosition> stepDestroyed = ExecuteShooterStep();
                allDestroyed.AddRange(stepDestroyed);
            }

            return allDestroyed;
        }

        /// <summary>
        /// Marks the active shooter as complete and moves it to used shooters
        /// </summary>
        private void CompleteActiveShooter(List<Data.GridPosition> destroyedInLastStep)
        {
            if (_activeShooter == null)
                return;

            _availableShooters.Remove(_activeShooter);
            _usedShooters.Add(_activeShooter);

            OnShooterCompleted?.Invoke(_activeShooter, destroyedInLastStep);
            _activeShooter = null;
        }

        /// <summary>
        /// Checks if the game has been won (all pixels cleared)
        /// </summary>
        private void CheckWinCondition()
        {
            if (_grid.IsGridEmpty())
            {
                OnGameWon?.Invoke();
            }
        }

        /// <summary>
        /// Checks if the current game state is won
        /// </summary>
        public bool IsGameWon()
        {
            return _grid.IsGridEmpty();
        }

        /// <summary>
        /// Resets the shooter manager to initial state
        /// </summary>
        public void Reset()
        {
            _activeShooter = null;
            _availableShooters.Clear();
            _usedShooters.Clear();
        }
    }
}
