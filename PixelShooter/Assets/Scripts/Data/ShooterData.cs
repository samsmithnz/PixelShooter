using UnityEngine;

namespace PixelShooter.Data
{
    /// <summary>
    /// Represents a shooter with its color, ball count, and state
    /// </summary>
    [System.Serializable]
    public class ShooterData
    {
        public Color color;
        public string colorName;
        public int totalBalls;
        public int remainingBalls;
        public bool isUsed;
        public bool isSelected;

        public ShooterData(Color color, string colorName, int ballCount)
        {
            this.color = color;
            this.colorName = colorName;
            this.totalBalls = ballCount;
            this.remainingBalls = ballCount;
            this.isUsed = false;
            this.isSelected = false;
        }

        public void UseBall()
        {
            if (remainingBalls > 0)
            {
                remainingBalls--;
            }
            
            if (remainingBalls == 0)
            {
                isUsed = true;
            }
        }

        public float GetPercentageRemaining()
        {
            if (totalBalls == 0) return 0f;
            return (float)remainingBalls / totalBalls;
        }
    }
}
