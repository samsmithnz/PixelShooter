using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PixelShooter.UI
{
    /// <summary>
    /// Displays game progress including remaining pixels and completion percentage
    /// </summary>
    public class ProgressDisplay : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI pixelsRemainingText;
        [SerializeField] private TextMeshProUGUI completionPercentageText;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image progressBarFill;

        [Header("Visual Settings")]
        [SerializeField] private Color progressBarColor = new Color(0.3f, 0.8f, 0.3f);
        [SerializeField] private Color progressBarBackgroundColor = new Color(0.2f, 0.2f, 0.2f);
        [SerializeField] private bool animateChanges = true;
        [SerializeField] private float animationSpeed = 2f;

        private int currentPixels;
        private int targetPixels;
        private int totalPixels;
        private float currentProgress;
        private float targetProgress;

        private void Awake()
        {
            if (progressBarFill != null)
            {
                progressBarFill.color = progressBarColor;
            }
        }

        private void Update()
        {
            if (animateChanges && Mathf.Abs(currentProgress - targetProgress) > 0.001f)
            {
                currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * animationSpeed);
                UpdateProgressBarVisual();
            }
        }

        public void Initialize(int total)
        {
            totalPixels = total;
            currentPixels = total;
            targetPixels = total;
            currentProgress = 0f;
            targetProgress = 0f;
            UpdateDisplay();
        }

        public void UpdateProgress(int remainingPixels)
        {
            targetPixels = remainingPixels;
            
            if (!animateChanges)
            {
                currentPixels = remainingPixels;
            }

            if (totalPixels > 0)
            {
                targetProgress = ((float)(totalPixels - remainingPixels) / totalPixels);
            }

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            // Update pixels remaining text
            if (pixelsRemainingText != null)
            {
                if (animateChanges)
                {
                    pixelsRemainingText.text = $"Pixels: {Mathf.RoundToInt(Mathf.Lerp(currentPixels, targetPixels, currentProgress))} / {totalPixels}";
                }
                else
                {
                    pixelsRemainingText.text = $"Pixels: {targetPixels} / {totalPixels}";
                }
            }

            // Update completion percentage text
            if (completionPercentageText != null)
            {
                float percentage = targetProgress * 100f;
                completionPercentageText.text = $"{percentage:F1}%";
            }

            UpdateProgressBarVisual();
        }

        private void UpdateProgressBarVisual()
        {
            if (progressBar != null)
            {
                progressBar.value = currentProgress;
            }
        }

        public void SetProgressBarColor(Color color)
        {
            progressBarColor = color;
            if (progressBarFill != null)
            {
                progressBarFill.color = progressBarColor;
            }
        }
    }
}
