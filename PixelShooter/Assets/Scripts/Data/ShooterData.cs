using UnityEngine;

namespace PixelShooter.Data
{
    /// <summary>
    /// Represents the data and state for a single shooter
    /// </summary>
    [System.Serializable]
    public class ShooterData
    {
        [SerializeField] private string id;
        [SerializeField] private Color color;
        [SerializeField] private int initialProjectileCount;
        [SerializeField] private int currentProjectileCount;
        [SerializeField] private bool hasBeenUsed;

        public string Id => id;
        public Color Color => color;
        public int InitialProjectileCount => initialProjectileCount;
        public int CurrentProjectileCount => currentProjectileCount;
        public bool HasBeenUsed => hasBeenUsed;
        public bool IsAvailable => !hasBeenUsed && currentProjectileCount > 0;

        public ShooterData(string id, Color color, int projectileCount)
        {
            this.id = id;
            this.color = color;
            this.initialProjectileCount = projectileCount;
            this.currentProjectileCount = projectileCount;
            this.hasBeenUsed = false;
        }

        /// <summary>
        /// Decrements the projectile count by one
        /// </summary>
        /// <returns>True if projectile was decremented, false if no projectiles remain</returns>
        public bool UseProjectile()
        {
            if (currentProjectileCount > 0)
            {
                currentProjectileCount--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Marks this shooter as used
        /// </summary>
        public void MarkAsUsed()
        {
            hasBeenUsed = true;
        }

        /// <summary>
        /// Resets the shooter to initial state (for level restart)
        /// </summary>
        public void Reset()
        {
            currentProjectileCount = initialProjectileCount;
            hasBeenUsed = false;
        }
    }
}
