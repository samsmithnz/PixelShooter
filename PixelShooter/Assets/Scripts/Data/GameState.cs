using System.Collections.Generic;
using UnityEngine;

namespace PixelShooter.Data
{
    /// <summary>
    /// Manages the overall game state for the current level
    /// </summary>
    public class GameState
    {
        public List<ShooterData> availableShooters;
        public int totalPixels;
        public int remainingPixels;
        public ShooterData activeShooter;
        public Vector2 activeShooterPosition;
        public bool isShooterMoving;
        public MovementDirection shooterDirection;

        public GameState()
        {
            availableShooters = new List<ShooterData>();
            totalPixels = 0;
            remainingPixels = 0;
            activeShooter = null;
            activeShooterPosition = Vector2.zero;
            isShooterMoving = false;
            shooterDirection = MovementDirection.Right;
        }

        public float GetCompletionPercentage()
        {
            if (totalPixels == 0) return 0f;
            return ((float)(totalPixels - remainingPixels) / totalPixels) * 100f;
        }

        public void DestroyPixel()
        {
            if (remainingPixels > 0)
            {
                remainingPixels--;
            }
        }

        public bool IsLevelComplete()
        {
            return remainingPixels == 0;
        }
    }

    public enum MovementDirection
    {
        Left,
        Right,
        None
    }
}
