using UnityEngine;
using UnityEngine.UI;

namespace PixelShooter.UI
{
    /// <summary>
    /// Central controller for the in-game HUD system
    /// Manages all UI components and handles screen scaling for mobile devices
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("Core HUD Components")]
        [SerializeField] private ShooterPanel shooterPanelComponent;
        [SerializeField] private ProgressDisplay progressComponent;
        [SerializeField] private ShooterActionFeedback feedbackComponent;

        [Header("Canvas Configuration")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private CanvasScaler scaler;
        [SerializeField] private Vector2 baseResolution = new Vector2(1080f, 1920f);

        [Header("Safe Area")]
        [SerializeField] private RectTransform safeAreaRect;
        [SerializeField] private bool applySafeArea = true;

        private Data.GameState currentGameState;

        private void Awake()
        {
            ConfigureCanvasScaling();
            ConfigureSafeArea();
        }

        private void Start()
        {
            SetupDemoState();
        }

        private void ConfigureCanvasScaling()
        {
            if (mainCanvas == null)
                mainCanvas = GetComponentInParent<Canvas>();

            if (scaler == null && mainCanvas != null)
                scaler = mainCanvas.GetComponent<CanvasScaler>() ?? mainCanvas.gameObject.AddComponent<CanvasScaler>();

            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = baseResolution;
                scaler.matchWidthOrHeight = 0.5f;
            }
        }

        private void ConfigureSafeArea()
        {
            if (!applySafeArea || safeAreaRect == null) return;

            Rect safe = Screen.safeArea;
            Vector2 minAnchor = safe.position;
            Vector2 maxAnchor = safe.position + safe.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            safeAreaRect.anchorMin = minAnchor;
            safeAreaRect.anchorMax = maxAnchor;
        }

        public void SetupHUD(Data.GameState state)
        {
            currentGameState = state;

            if (shooterPanelComponent != null)
            {
                shooterPanelComponent.InitializeShooters(state.availableShooters, HandleShooterSelection);
                shooterPanelComponent.SetPanelTitle("Available Shooters");
            }

            if (progressComponent != null)
            {
                progressComponent.Initialize(state.totalPixels);
                progressComponent.UpdateProgress(state.remainingPixels);
            }

            if (feedbackComponent != null)
                feedbackComponent.HideFeedback();
        }

        private void HandleShooterSelection(Data.ShooterData selected)
        {
            if (currentGameState == null) return;

            currentGameState.activeShooter = selected;
            
            if (feedbackComponent != null)
                feedbackComponent.ShowFeedback(selected, Data.MovementDirection.Right);

            Debug.Log($"Selected: {selected.colorName} ({selected.remainingBalls} balls)");
        }

        public void RefreshDisplay()
        {
            if (currentGameState == null) return;

            if (shooterPanelComponent != null)
                shooterPanelComponent.UpdateShooterDisplays();

            if (progressComponent != null)
                progressComponent.UpdateProgress(currentGameState.remainingPixels);

            if (feedbackComponent != null && currentGameState.activeShooter != null)
            {
                if (currentGameState.isShooterMoving)
                {
                    feedbackComponent.ShowFeedback(currentGameState.activeShooter, currentGameState.shooterDirection);
                    feedbackComponent.UpdateBallCount(currentGameState.activeShooter.remainingBalls);
                }
                else
                {
                    feedbackComponent.HideFeedback();
                }
            }
        }

        public void NotifyPixelDestroyed()
        {
            if (currentGameState == null) return;

            currentGameState.DestroyPixel();
            
            if (currentGameState.activeShooter != null)
                currentGameState.activeShooter.UseBall();

            RefreshDisplay();
        }

        public void NotifyShooterStarted()
        {
            if (currentGameState != null)
            {
                currentGameState.isShooterMoving = true;
                RefreshDisplay();
            }
        }

        public void NotifyShooterStopped()
        {
            if (currentGameState != null)
            {
                currentGameState.isShooterMoving = false;
                RefreshDisplay();
            }
        }

        private void SetupDemoState()
        {
            var demo = new Data.GameState();
            demo.totalPixels = 64;
            demo.remainingPixels = 64;

            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Red, "Red", 12));
            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Blue, "Blue", 10));
            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Yellow, "Yellow", 8));
            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Green, "Green", 15));
            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Orange, "Orange", 9));
            demo.availableShooters.Add(new Data.ShooterData(Data.ColorPalette.Purple, "Purple", 10));

            SetupHUD(demo);
        }
    }
}
