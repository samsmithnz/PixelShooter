using UnityEngine;
using PixelShooter.Data;

namespace PixelShooter.Game
{
    /// <summary>
    /// Manages shooter selection and activation based on player input
    /// </summary>
    public class ShooterManager : MonoBehaviour
    {
        [SerializeField] private GameController gameController;
        
        private ShooterData lastTappedShooter = null;

        private void Start()
        {
            if (gameController == null)
            {
                gameController = FindObjectOfType<GameController>();
            }
        }

        /// <summary>
        /// Called when a shooter is tapped by the player
        /// First tap selects, second tap activates
        /// </summary>
        public void OnShooterTapped(ShooterData shooter)
        {
            if (gameController == null)
            {
                Debug.LogError("GameController is not assigned");
                return;
            }

            // Only process taps during Selection state
            if (gameController.CurrentState != GameState.Selection)
            {
                return;
            }

            // If this is the first tap or a different shooter was tapped
            if (gameController.SelectedShooter != shooter)
            {
                // Select the shooter
                if (gameController.SelectShooter(shooter))
                {
                    lastTappedShooter = shooter;
                }
            }
            // If the same shooter was tapped again (second tap)
            else if (gameController.SelectedShooter == shooter && lastTappedShooter == shooter)
            {
                // Activate the selected shooter
                gameController.ActivateSelectedShooter();
                lastTappedShooter = null;
            }
        }

        /// <summary>
        /// Called when the player taps outside of any shooter to deselect
        /// </summary>
        public void OnBackgroundTapped()
        {
            if (gameController != null && gameController.CurrentState == GameState.Selection)
            {
                gameController.DeselectShooter();
                lastTappedShooter = null;
            }
        }
    }
}
