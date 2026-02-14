using NUnit.Framework;
using UnityEngine;
using PixelShooter.Data;

namespace PixelShooter.Tests
{
    /// <summary>
    /// Unit tests for ShooterData functionality
    /// Tests projectile tracking and shooter state management
    /// </summary>
    public class ShooterDataTests
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 10);
            
            Assert.AreEqual("TestShooter", shooter.Id);
            Assert.AreEqual(Color.red, shooter.Color);
            Assert.AreEqual(10, shooter.InitialProjectileCount);
            Assert.AreEqual(10, shooter.CurrentProjectileCount);
            Assert.IsFalse(shooter.HasBeenUsed);
            Assert.IsTrue(shooter.IsAvailable);
        }

        [Test]
        public void UseProjectile_DecrementsCount()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 5);
            
            bool result = shooter.UseProjectile();
            
            Assert.IsTrue(result);
            Assert.AreEqual(4, shooter.CurrentProjectileCount);
        }

        [Test]
        public void UseProjectile_WhenEmpty_ReturnsFalse()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 1);
            
            shooter.UseProjectile(); // Use the last projectile
            bool result = shooter.UseProjectile(); // Try to use when empty
            
            Assert.IsFalse(result);
            Assert.AreEqual(0, shooter.CurrentProjectileCount);
        }

        [Test]
        public void MarkAsUsed_SetsUsedFlag()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 5);
            
            shooter.MarkAsUsed();
            
            Assert.IsTrue(shooter.HasBeenUsed);
            Assert.IsFalse(shooter.IsAvailable);
        }

        [Test]
        public void IsAvailable_WhenUsed_ReturnsFalse()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 5);
            
            shooter.MarkAsUsed();
            
            Assert.IsFalse(shooter.IsAvailable);
        }

        [Test]
        public void IsAvailable_WhenNoProjectiles_ReturnsFalse()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 1);
            
            shooter.UseProjectile();
            
            Assert.IsFalse(shooter.IsAvailable);
        }

        [Test]
        public void Reset_RestoresInitialState()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 5);
            
            // Use projectiles and mark as used
            shooter.UseProjectile();
            shooter.UseProjectile();
            shooter.MarkAsUsed();
            
            // Reset
            shooter.Reset();
            
            Assert.AreEqual(5, shooter.CurrentProjectileCount);
            Assert.IsFalse(shooter.HasBeenUsed);
            Assert.IsTrue(shooter.IsAvailable);
        }

        [Test]
        public void MultipleProjectiles_DecrementCorrectly()
        {
            ShooterData shooter = new ShooterData("TestShooter", Color.red, 10);
            
            for (int i = 0; i < 7; i++)
            {
                shooter.UseProjectile();
            }
            
            Assert.AreEqual(3, shooter.CurrentProjectileCount);
            Assert.IsTrue(shooter.IsAvailable);
        }
    }
}
