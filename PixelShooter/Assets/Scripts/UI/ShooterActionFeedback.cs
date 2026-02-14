using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PixelShooter.UI
{
    /// <summary>
    /// Displays active shooter movement direction and action feedback
    /// </summary>
    public class ShooterActionFeedback : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject feedbackPanel;
        [SerializeField] private TextMeshProUGUI shooterNameText;
        [SerializeField] private TextMeshProUGUI ballsRemainingText;
        [SerializeField] private Image shooterColorIndicator;
        [SerializeField] private Image directionArrow;
        [SerializeField] private GameObject movingIndicator;

        [Header("Direction Arrow Sprites")]
        [SerializeField] private Sprite leftArrowSprite;
        [SerializeField] private Sprite rightArrowSprite;

        [Header("Animation Settings")]
        [SerializeField] private bool pulseWhenActive = true;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private float pulseMinScale = 0.95f;
        [SerializeField] private float pulseMaxScale = 1.05f;

        private bool isActive;
        private float pulseTimer;

        private void Update()
        {
            if (isActive && pulseWhenActive && movingIndicator != null)
            {
                pulseTimer += Time.deltaTime * pulseSpeed;
                float scale = Mathf.Lerp(pulseMinScale, pulseMaxScale, (Mathf.Sin(pulseTimer) + 1f) / 2f);
                movingIndicator.transform.localScale = Vector3.one * scale;
            }
        }

        public void ShowFeedback(Data.ShooterData shooter, Data.MovementDirection direction)
        {
            if (shooter == null)
            {
                HideFeedback();
                return;
            }

            isActive = true;
            
            if (feedbackPanel != null)
            {
                feedbackPanel.SetActive(true);
            }

            // Update shooter name
            if (shooterNameText != null)
            {
                shooterNameText.text = $"{shooter.colorName} Shooter";
                shooterNameText.color = shooter.color;
            }

            // Update balls remaining
            if (ballsRemainingText != null)
            {
                ballsRemainingText.text = $"Balls: {shooter.remainingBalls}";
            }

            // Update color indicator
            if (shooterColorIndicator != null)
            {
                shooterColorIndicator.color = shooter.color;
            }

            // Update direction arrow
            if (directionArrow != null)
            {
                UpdateDirectionArrow(direction);
            }

            // Show moving indicator
            if (movingIndicator != null)
            {
                movingIndicator.SetActive(true);
            }
        }

        public void HideFeedback()
        {
            isActive = false;
            
            if (feedbackPanel != null)
            {
                feedbackPanel.SetActive(false);
            }

            if (movingIndicator != null)
            {
                movingIndicator.SetActive(false);
            }
        }

        public void UpdateBallCount(int remainingBalls)
        {
            if (ballsRemainingText != null && isActive)
            {
                ballsRemainingText.text = $"Balls: {remainingBalls}";
            }
        }

        private void UpdateDirectionArrow(Data.MovementDirection direction)
        {
            if (directionArrow == null) return;

            switch (direction)
            {
                case Data.MovementDirection.Left:
                    if (leftArrowSprite != null)
                    {
                        directionArrow.sprite = leftArrowSprite;
                    }
                    directionArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                    directionArrow.gameObject.SetActive(true);
                    break;

                case Data.MovementDirection.Right:
                    if (rightArrowSprite != null)
                    {
                        directionArrow.sprite = rightArrowSprite;
                    }
                    directionArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                    directionArrow.gameObject.SetActive(true);
                    break;

                case Data.MovementDirection.None:
                    directionArrow.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
