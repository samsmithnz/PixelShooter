using NUnit.Framework;
using UnityEngine;
using PixelShooter.Data;
using PixelShooter.Game;
using System.Collections.Generic;

namespace PixelShooter.Tests
{
    /// <summary>
    /// Unit tests for GameController functionality
    /// Tests shooter selection, activation, and turn-state management
    /// </summary>
    public class GameControllerTests
    {
        private GameController gameController;
        private List<ShooterData> testShooters;

        [SetUp]
        public void Setup()
        {
            // Create a test GameObject with GameController
            GameObject go = new GameObject("TestGameController");
            gameController = go.AddComponent<GameController>();

            // Create test shooters
            testShooters = new List<ShooterData>
            {
                new ShooterData("RedShooter", Color.red, 5),
                new ShooterData("BlueShooter", Color.blue, 3),
                new ShooterData("GreenShooter", Color.green, 4)
            };

            gameController.InitializeGame(testShooters);
        }

        [TearDown]
        public void Teardown()
        {
            if (gameController != null)
            {
                Object.DestroyImmediate(gameController.gameObject);
            }
        }

        [Test]
        public void InitializeGame_SetsSelectionState()
        {
            Assert.AreEqual(GameState.Selection, gameController.CurrentState);
        }

        [Test]
        public void InitializeGame_AllShootersAvailable()
        {
            Assert.AreEqual(3, gameController.GetAvailableShooterCount());
            Assert.AreEqual(0, gameController.GetUsedShooterCount());
        }

        [Test]
        public void SelectShooter_InSelectionState_Succeeds()
        {
            bool result = gameController.SelectShooter(testShooters[0]);
            
            Assert.IsTrue(result);
            Assert.AreEqual(testShooters[0], gameController.SelectedShooter);
            Assert.AreEqual(GameState.Selection, gameController.CurrentState);
        }

        [Test]
        public void SelectShooter_WhileShooterActive_Fails()
        {
            // First select and activate a shooter
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();

            // Try to select another shooter while one is active
            bool result = gameController.SelectShooter(testShooters[1]);
            
            Assert.IsFalse(result);
            Assert.AreEqual(GameState.ShooterActive, gameController.CurrentState);
        }

        [Test]
        public void ActivateSelectedShooter_WithSelection_Succeeds()
        {
            gameController.SelectShooter(testShooters[0]);
            bool result = gameController.ActivateSelectedShooter();
            
            Assert.IsTrue(result);
            Assert.AreEqual(testShooters[0], gameController.ActiveShooter);
            Assert.IsNull(gameController.SelectedShooter);
            Assert.AreEqual(GameState.ShooterActive, gameController.CurrentState);
        }

        [Test]
        public void ActivateSelectedShooter_WithoutSelection_Fails()
        {
            bool result = gameController.ActivateSelectedShooter();
            
            Assert.IsFalse(result);
            Assert.IsNull(gameController.ActiveShooter);
            Assert.AreEqual(GameState.Selection, gameController.CurrentState);
        }

        [Test]
        public void CompleteShooterRun_MarksShooterAsUsed()
        {
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();
            gameController.CompleteShooterRun();
            
            Assert.IsTrue(testShooters[0].HasBeenUsed);
            Assert.AreEqual(2, gameController.GetAvailableShooterCount());
            Assert.AreEqual(1, gameController.GetUsedShooterCount());
        }

        [Test]
        public void CompleteShooterRun_ReturnsToSelectionState()
        {
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();
            gameController.CompleteShooterRun();
            
            Assert.AreEqual(GameState.Selection, gameController.CurrentState);
            Assert.IsNull(gameController.ActiveShooter);
        }

        [Test]
        public void UseProjectile_DecrementsCount()
        {
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();
            
            int initialCount = testShooters[0].CurrentProjectileCount;
            gameController.UseProjectile();
            
            Assert.AreEqual(initialCount - 1, testShooters[0].CurrentProjectileCount);
        }

        [Test]
        public void DeselectShooter_ClearsSelection()
        {
            gameController.SelectShooter(testShooters[0]);
            gameController.DeselectShooter();
            
            Assert.IsNull(gameController.SelectedShooter);
        }

        [Test]
        public void ResetGame_RestoresAllShooters()
        {
            // Use a shooter
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();
            gameController.UseProjectile();
            gameController.UseProjectile();
            gameController.CompleteShooterRun();
            
            // Reset
            gameController.ResetGame();
            
            Assert.AreEqual(GameState.Selection, gameController.CurrentState);
            Assert.AreEqual(3, gameController.GetAvailableShooterCount());
            Assert.AreEqual(0, gameController.GetUsedShooterCount());
            Assert.AreEqual(5, testShooters[0].CurrentProjectileCount);
        }

        [Test]
        public void PreventMultipleActiveShooters()
        {
            // Activate first shooter
            gameController.SelectShooter(testShooters[0]);
            gameController.ActivateSelectedShooter();
            
            // Try to select and activate another
            bool selectResult = gameController.SelectShooter(testShooters[1]);
            
            Assert.IsFalse(selectResult);
            Assert.AreEqual(testShooters[0], gameController.ActiveShooter);
            Assert.AreEqual(GameState.ShooterActive, gameController.CurrentState);
        }
    }
}
