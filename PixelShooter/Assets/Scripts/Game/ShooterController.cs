using UnityEngine;
using PixelShooter.Data;

namespace PixelShooter.Game
{
    /// <summary>
    /// Controls an individual shooter's behavior and execution
    /// </summary>
    public class ShooterController : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        [SerializeField] private ShooterData shooterData;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float shootDelay = 0.2f;

        private bool isExecuting = false;
        private float currentPosition = 0f;
        private float shootTimer = 0f;

        public ShooterData ShooterData => shooterData;

        private void Start()
        {
            if (gameController == null)
            {
                gameController = FindObjectOfType<GameController>();
            }
        }

        /// <summary>
        /// Initializes the shooter with data
        /// </summary>
        public void Initialize(ShooterData data)
        {
            shooterData = data;
        }

        /// <summary>
        /// Starts the shooter's execution (movement and firing)
        /// </summary>
        public void BeginExecution()
        {
            if (shooterData == null)
            {
                Debug.LogError("Cannot begin execution - shooter data is null");
                return;
            }

            isExecuting = true;
            currentPosition = 0f;
            shootTimer = 0f;
        }

        /// <summary>
        /// Ends the shooter's execution and notifies the game controller
        /// </summary>
        public void EndExecution()
        {
            isExecuting = false;
            
            if (gameController != null)
            {
                gameController.CompleteShooterRun();
            }
        }

        private void Update()
        {
            if (!isExecuting || shooterData == null)
            {
                return;
            }

            // Simulate movement from left to right
            currentPosition += moveSpeed * Time.deltaTime;

            // Simulate shooting at intervals
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootDelay)
            {
                shootTimer = 0f;
                TryShoot();
            }

            // Example: End execution after 3 seconds (in real game, this would be based on grid width)
            if (currentPosition >= 8f)
            {
                EndExecution();
            }
        }

        /// <summary>
        /// Attempts to shoot a projectile
        /// </summary>
        private void TryShoot()
        {
            if (shooterData == null || !shooterData.IsAvailable)
            {
                return;
            }

            // In real game, this would check for matching pixels in line of sight
            // For now, just simulate shooting
            if (shooterData.CurrentProjectileCount > 0)
            {
                if (gameController != null)
                {
                    gameController.UseProjectile();
                }
                
                // If out of projectiles, end execution early
                if (shooterData.CurrentProjectileCount == 0)
                {
                    EndExecution();
                }
            }
        }

        /// <summary>
        /// Gets the current execution progress (0 to 1)
        /// </summary>
        public float GetExecutionProgress()
        {
            return Mathf.Clamp01(currentPosition / 8f);
        }

        /// <summary>
        /// Checks if the shooter is currently executing
        /// </summary>
        public bool IsExecuting()
        {
            return isExecuting;
        }
    }
}
