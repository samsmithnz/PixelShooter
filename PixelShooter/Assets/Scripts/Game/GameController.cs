using System;
using System.Collections.Generic;
using UnityEngine;
using PixelShooter.Data;

namespace PixelShooter.Game
{
    /// <summary>
    /// Main game controller that manages game state and turn flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        private GameState currentState = GameState.Selection;
        private List<ShooterData> shooters = new List<ShooterData>();
        private ShooterData selectedShooter = null;
        private ShooterData activeShooter = null;

        // Events for state changes
        public event Action<GameState> OnGameStateChanged;
        public event Action<ShooterData> OnShooterSelected;
        public event Action<ShooterData> OnShooterActivated;
        public event Action<ShooterData> OnShooterCompleted;
        public event Action<ShooterData, int> OnProjectileCountChanged;

        public GameState CurrentState => currentState;
        public ShooterData SelectedShooter => selectedShooter;
        public ShooterData ActiveShooter => activeShooter;
        public IReadOnlyList<ShooterData> Shooters => shooters;

        /// <summary>
        /// Initializes the game with a list of shooters
        /// </summary>
        public void InitializeGame(List<ShooterData> gameShooters)
        {
            shooters = new List<ShooterData>(gameShooters);
            selectedShooter = null;
            activeShooter = null;
            SetGameState(GameState.Selection);
        }

        /// <summary>
        /// Attempts to select a shooter. Returns true if selection was successful.
        /// </summary>
        public bool SelectShooter(ShooterData shooter)
        {
            // Can only select during Selection state
            if (currentState != GameState.Selection)
            {
                Debug.LogWarning("Cannot select shooter - not in Selection state");
                return false;
            }

            // Can only select available shooters
            if (!shooter.IsAvailable)
            {
                Debug.LogWarning("Cannot select shooter - shooter is not available");
                return false;
            }

            selectedShooter = shooter;
            OnShooterSelected?.Invoke(selectedShooter);
            return true;
        }

        /// <summary>
        /// Attempts to activate the selected shooter. Returns true if activation was successful.
        /// </summary>
        public bool ActivateSelectedShooter()
        {
            // Can only activate during Selection state
            if (currentState != GameState.Selection)
            {
                Debug.LogWarning("Cannot activate shooter - not in Selection state");
                return false;
            }

            // Must have a shooter selected
            if (selectedShooter == null)
            {
                Debug.LogWarning("Cannot activate shooter - no shooter selected");
                return false;
            }

            // Shooter must still be available
            if (!selectedShooter.IsAvailable)
            {
                Debug.LogWarning("Cannot activate shooter - selected shooter is not available");
                return false;
            }

            // Activate the shooter
            activeShooter = selectedShooter;
            selectedShooter = null;
            SetGameState(GameState.ShooterActive);
            OnShooterActivated?.Invoke(activeShooter);
            return true;
        }

        /// <summary>
        /// Deselects the currently selected shooter
        /// </summary>
        public void DeselectShooter()
        {
            if (selectedShooter != null)
            {
                selectedShooter = null;
                OnShooterSelected?.Invoke(null);
            }
        }

        /// <summary>
        /// Called when the active shooter uses a projectile
        /// </summary>
        public void UseProjectile()
        {
            if (activeShooter != null && currentState == GameState.ShooterActive)
            {
                if (activeShooter.UseProjectile())
                {
                    OnProjectileCountChanged?.Invoke(activeShooter, activeShooter.CurrentProjectileCount);
                }
            }
        }

        /// <summary>
        /// Completes the current shooter's run and returns to selection state
        /// </summary>
        public void CompleteShooterRun()
        {
            if (activeShooter == null)
            {
                Debug.LogWarning("Cannot complete shooter run - no active shooter");
                return;
            }

            if (currentState != GameState.ShooterActive)
            {
                Debug.LogWarning("Cannot complete shooter run - not in ShooterActive state");
                return;
            }

            // Mark shooter as used
            activeShooter.MarkAsUsed();
            
            ShooterData completedShooter = activeShooter;
            activeShooter = null;

            OnShooterCompleted?.Invoke(completedShooter);

            // Check if there are any available shooters left
            bool hasAvailableShooters = false;
            foreach (var shooter in shooters)
            {
                if (shooter.IsAvailable)
                {
                    hasAvailableShooters = true;
                    break;
                }
            }

            // Return to selection state if there are available shooters
            if (hasAvailableShooters)
            {
                SetGameState(GameState.Selection);
            }
            else
            {
                // Could potentially set to LevelComplete here if all pixels are cleared
                // For now, just return to selection
                SetGameState(GameState.Selection);
            }
        }

        /// <summary>
        /// Resets the game to initial state
        /// </summary>
        public void ResetGame()
        {
            foreach (var shooter in shooters)
            {
                shooter.Reset();
            }
            selectedShooter = null;
            activeShooter = null;
            SetGameState(GameState.Selection);
        }

        /// <summary>
        /// Sets the game state and notifies listeners
        /// </summary>
        private void SetGameState(GameState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                OnGameStateChanged?.Invoke(currentState);
            }
        }

        /// <summary>
        /// Gets the count of available shooters
        /// </summary>
        public int GetAvailableShooterCount()
        {
            int count = 0;
            foreach (var shooter in shooters)
            {
                if (shooter.IsAvailable)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Gets the count of used shooters
        /// </summary>
        public int GetUsedShooterCount()
        {
            int count = 0;
            foreach (var shooter in shooters)
            {
                if (shooter.HasBeenUsed)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
