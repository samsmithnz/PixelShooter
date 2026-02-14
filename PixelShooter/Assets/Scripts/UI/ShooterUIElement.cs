using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PixelShooter.UI
{
    /// <summary>
    /// UI component representing a single shooter in the shooter panel
    /// Shows color, ball count, and selection state
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ShooterUIElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image colorIndicator;
        [SerializeField] private TextMeshProUGUI ballCountText;
        [SerializeField] private Image selectionBorder;
        [SerializeField] private Image backgroundPanel;
        [SerializeField] private GameObject usedOverlay;

        [Header("Visual Settings")]
        [SerializeField] private Color normalBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        [SerializeField] private Color selectedBackgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        [SerializeField] private Color selectionBorderColor = new Color(1f, 0.9f, 0.3f, 1f);
        [SerializeField] private float borderThickness = 4f;

        private Button button;
        private Data.ShooterData shooterData;
        private System.Action<Data.ShooterData> onShooterSelected;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        public void Initialize(Data.ShooterData data, System.Action<Data.ShooterData> selectionCallback)
        {
            shooterData = data;
            onShooterSelected = selectionCallback;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            if (shooterData == null) return;

            // Update color indicator
            if (colorIndicator != null)
            {
                colorIndicator.color = shooterData.color;
            }

            // Update ball count text
            if (ballCountText != null)
            {
                ballCountText.text = shooterData.remainingBalls.ToString();
            }

            // Update selection state
            if (selectionBorder != null)
            {
                selectionBorder.gameObject.SetActive(shooterData.isSelected);
                selectionBorder.color = selectionBorderColor;
            }

            // Update background
            if (backgroundPanel != null)
            {
                backgroundPanel.color = shooterData.isSelected ? selectedBackgroundColor : normalBackgroundColor;
            }

            // Update used overlay
            if (usedOverlay != null)
            {
                usedOverlay.SetActive(shooterData.isUsed);
            }

            // Update button interactability
            button.interactable = !shooterData.isUsed;
        }

        private void OnButtonClicked()
        {
            if (shooterData != null && !shooterData.isUsed)
            {
                onShooterSelected?.Invoke(shooterData);
            }
        }

        public Data.ShooterData GetShooterData()
        {
            return shooterData;
        }
    }
}
