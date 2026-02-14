using UnityEngine;
using System.Collections.Generic;
using PixelShooter.Data;
using PixelShooter.Game;
using PixelShooter.UI;

namespace PixelShooter.Examples
{
    /// <summary>
    /// Example setup script demonstrating how to use the shooter selection system
    /// This script shows the complete workflow from initialization to shooter execution
    /// </summary>
    public class GameSetupExample : MonoBehaviour
    {
        [Header("Game Components")]
        [SerializeField] private GameController gameController;
        [SerializeField] private ShooterManager shooterManager;
        [SerializeField] private UIController uiController;

        [Header("Example Configuration")]
        [SerializeField] private int redPixelCount = 5;
        [SerializeField] private int bluePixelCount = 3;
        [SerializeField] private int greenPixelCount = 4;

        private List<ShooterController> shooterControllers = new List<ShooterController>();

        private void Start()
        {
            // Initialize the game with example shooters
            InitializeExampleGame();

            // Subscribe to events for demonstration
            SubscribeToEvents();
        }

        /// <summary>
        /// Sets up an example game with three colored shooters
        /// </summary>
        private void InitializeExampleGame()
        {
            // Create shooter data based on pixel counts
            List<ShooterData> shooters = new List<ShooterData>
            {
                new ShooterData("Red Shooter", Color.red, redPixelCount),
                new ShooterData("Blue Shooter", Color.blue, bluePixelCount),
                new ShooterData("Green Shooter", Color.green, greenPixelCount)
            };

            // Initialize the game controller
            if (gameController != null)
            {
                gameController.InitializeGame(shooters);
                Debug.Log("Game initialized with " + shooters.Count + " shooters");
            }
        }

        /// <summary>
        /// Subscribes to game events for logging and demonstration
        /// </summary>
        private void SubscribeToEvents()
        {
            if (gameController == null) return;

            gameController.OnGameStateChanged += (state) =>
            {
                Debug.Log($"Game state changed to: {state}");
            };

            gameController.OnShooterSelected += (shooter) =>
            {
                if (shooter != null)
                {
                    Debug.Log($"Shooter selected: {shooter.Id}");
                }
                else
                {
                    Debug.Log("Shooter deselected");
                }
            };

            gameController.OnShooterActivated += (shooter) =>
            {
                Debug.Log($"Shooter activated: {shooter.Id} with {shooter.CurrentProjectileCount} projectiles");
                
                // Find or create a ShooterController for execution
                ShooterController controller = FindShooterController(shooter);
                if (controller != null)
                {
                    controller.BeginExecution();
                }
            };

            gameController.OnShooterCompleted += (shooter) =>
            {
                Debug.Log($"Shooter completed: {shooter.Id}, Projectiles remaining: {shooter.CurrentProjectileCount}");
            };

            gameController.OnProjectileCountChanged += (shooter, count) =>
            {
                Debug.Log($"Projectile fired! {shooter.Id} now has {count} projectiles remaining");
            };
        }

        /// <summary>
        /// Finds or creates a ShooterController for the given ShooterData
        /// </summary>
        private ShooterController FindShooterController(ShooterData data)
        {
            // In a real game, this would find the existing GameObject
            // For demonstration, create a temporary one
            GameObject shooterObj = new GameObject($"Shooter_{data.Id}");
            ShooterController controller = shooterObj.AddComponent<ShooterController>();
            controller.Initialize(data);
            shooterControllers.Add(controller);
            return controller;
        }

        /// <summary>
        /// Example: Simulates tapping on the first shooter
        /// </summary>
        public void ExampleTapFirstShooter()
        {
            if (gameController.Shooters.Count > 0)
            {
                ShooterData firstShooter = gameController.Shooters[0] as ShooterData;
                if (shooterManager != null)
                {
                    shooterManager.OnShooterTapped(firstShooter);
                }
            }
        }

        /// <summary>
        /// Example: Simulates activating the selected shooter
        /// </summary>
        public void ExampleActivateSelectedShooter()
        {
            if (gameController.SelectedShooter != null && shooterManager != null)
            {
                shooterManager.OnShooterTapped(gameController.SelectedShooter);
            }
        }

        private void OnDestroy()
        {
            // Clean up shooter controllers
            foreach (var controller in shooterControllers)
            {
                if (controller != null)
                {
                    Destroy(controller.gameObject);
                }
            }
            shooterControllers.Clear();
        }
    }
}
