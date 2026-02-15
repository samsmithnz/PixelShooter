using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelShooter.Data;

namespace PixelShooter.UI
{
    /// <summary>
    /// Controls the UI display for shooters and game state
    /// </summary>
    public class UIController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI gameStateText;
        [SerializeField] private TextMeshProUGUI selectedShooterText;
        [SerializeField] private TextMeshProUGUI activeShooterText;
        [SerializeField] private TextMeshProUGUI availableShootersText;
        [SerializeField] private TextMeshProUGUI usedShootersText;

        [Header("Game Reference")]
        [SerializeField] private Game.GameController gameController;

        private void Start()
        {
            if (gameController == null)
            {
                gameController = FindAnyObjectByType<Game.GameController>();
            }

            if (gameController != null)
            {
                // Subscribe to game controller events
                gameController.OnGameStateChanged += HandleGameStateChanged;
                gameController.OnShooterSelected += HandleShooterSelected;
                gameController.OnShooterActivated += HandleShooterActivated;
                gameController.OnShooterCompleted += HandleShooterCompleted;
                gameController.OnProjectileCountChanged += HandleProjectileCountChanged;
            }

            UpdateUI();
        }

        private void OnDestroy()
        {
            if (gameController != null)
            {
                gameController.OnGameStateChanged -= HandleGameStateChanged;
                gameController.OnShooterSelected -= HandleShooterSelected;
                gameController.OnShooterActivated -= HandleShooterActivated;
                gameController.OnShooterCompleted -= HandleShooterCompleted;
                gameController.OnProjectileCountChanged -= HandleProjectileCountChanged;
            }
        }

        private void HandleGameStateChanged(Game.GameState newState)
        {
            UpdateUI();
        }

        private void HandleShooterSelected(ShooterData shooter)
        {
            UpdateUI();
        }

        private void HandleShooterActivated(ShooterData shooter)
        {
            UpdateUI();
        }

        private void HandleShooterCompleted(ShooterData shooter)
        {
            UpdateUI();
        }

        private void HandleProjectileCountChanged(ShooterData shooter, int newCount)
        {
            UpdateUI();
        }

        /// <summary>
        /// Updates all UI elements based on current game state
        /// </summary>
        public void UpdateUI()
        {
            if (gameController == null)
            {
                return;
            }

            // Update game state text
            if (gameStateText != null)
            {
                gameStateText.text = $"Game State: {gameController.CurrentState}";
            }

            // Update selected shooter info
            if (selectedShooterText != null)
            {
                if (gameController.SelectedShooter != null)
                {
                    selectedShooterText.text = $"Selected: {gameController.SelectedShooter.Id} ({gameController.SelectedShooter.CurrentProjectileCount} projectiles)";
                }
                else
                {
                    selectedShooterText.text = "Selected: None";
                }
            }

            // Update active shooter info
            if (activeShooterText != null)
            {
                if (gameController.ActiveShooter != null)
                {
                    activeShooterText.text = $"Active: {gameController.ActiveShooter.Id} ({gameController.ActiveShooter.CurrentProjectileCount} projectiles)";
                }
                else
                {
                    activeShooterText.text = "Active: None";
                }
            }

            // Update available/used shooter counts
            if (availableShootersText != null)
            {
                availableShootersText.text = $"Available Shooters: {gameController.GetAvailableShooterCount()}";
            }

            if (usedShootersText != null)
            {
                usedShootersText.text = $"Used Shooters: {gameController.GetUsedShooterCount()}";
            }
        }
    }
}
