using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PixelShooter.Core
{
    /// <summary>
    /// Demo script to showcase HUD functionality without full game implementation
    /// </summary>
    public class HUDDemo : MonoBehaviour
    {
        [Header("HUD Reference")]
        [SerializeField] private UI.HUDManager hudManager;

        [Header("Demo Controls")]
        [SerializeField] private bool autoDemo = true;
        [SerializeField] private float demoInterval = 2f;

        [Header("UI Demo Controls")]
        [SerializeField] private Button destroyPixelButton;
        [SerializeField] private Button startShooterButton;
        [SerializeField] private TextMeshProUGUI instructionsText;

        private float demoTimer;

        private void Start()
        {
            SetupDemoControls();
        }

        private void Update()
        {
            if (autoDemo)
            {
                demoTimer += Time.deltaTime;
                if (demoTimer >= demoInterval)
                {
                    demoTimer = 0f;
                    SimulatePixelDestruction();
                }
            }
        }

        private void SetupDemoControls()
        {
            if (destroyPixelButton != null)
            {
                destroyPixelButton.onClick.AddListener(SimulatePixelDestruction);
            }

            if (startShooterButton != null)
            {
                startShooterButton.onClick.AddListener(SimulateShooterMovement);
            }

            if (instructionsText != null)
            {
                instructionsText.text = "HUD Demo Active\n" +
                                       "- Select a shooter from the panel\n" +
                                       "- Watch pixels decrease automatically\n" +
                                       "- Observe shooter ball count updates";
            }
        }

        private void SimulatePixelDestruction()
        {
            if (hudManager != null)
            {
                hudManager.NotifyPixelDestroyed();
            }
        }

        private void SimulateShooterMovement()
        {
            if (hudManager != null)
            {
                hudManager.NotifyShooterStarted();
                Invoke(nameof(StopShooterMovement), 3f);
            }
        }

        private void StopShooterMovement()
        {
            if (hudManager != null)
            {
                hudManager.NotifyShooterStopped();
            }
        }
    }
}
