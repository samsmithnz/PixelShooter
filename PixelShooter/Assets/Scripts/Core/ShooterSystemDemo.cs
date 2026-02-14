using UnityEngine;
using System.Collections.Generic;

namespace PixelShooter.Core
{
    /// <summary>
    /// Demo/Test script to validate shooter movement and firing logic
    /// Attach to a GameObject to run automated tests in the Unity Editor
    /// </summary>
    public class ShooterSystemDemo : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool runTestsOnStart = true;
        [SerializeField] private bool logDetailedOutput = true;

        private void Start()
        {
            if (runTestsOnStart)
            {
                RunAllTests();
            }
        }

        public void RunAllTests()
        {
            Debug.Log("========== Starting Shooter System Tests ==========");

            TestBasicShooterMovement();
            TestShooterFiring();
            TestLineOfSightBlocking();
            TestMultipleLayerDestruction();
            TestOutOfBallsMidPath();
            TestNoValidTargets();
            TestLastPixelWin();
            TestShooterManager();

            Debug.Log("========== All Tests Complete ==========");
        }

        private void TestBasicShooterMovement()
        {
            Debug.Log("\n--- Test: Basic Shooter Movement ---");

            GridSystem grid = new GridSystem(8, 8);
            Gameplay.Shooter shooter = new Gameplay.Shooter(Data.PixelColor.Red, 10);
            shooter.Activate(grid);

            int moveCount = 0;
            while (shooter.MoveNext())
            {
                moveCount++;
            }

            bool success = moveCount == 8; // Should move through all 8 columns
            Debug.Log($"Shooter moved {moveCount} times. Expected 8. {(success ? "PASS" : "FAIL")}");
        }

        private void TestShooterFiring()
        {
            Debug.Log("\n--- Test: Shooter Firing ---");

            GridSystem grid = new GridSystem(8, 8);

            // Add a red pixel at row 2, column 3
            Data.GridPosition pixelPos = new Data.GridPosition(2, 3);
            Data.Pixel redPixel = new Data.Pixel(Data.PixelColor.Red, pixelPos, 0);
            grid.AddPixel(redPixel);

            Gameplay.Shooter shooter = new Gameplay.Shooter(Data.PixelColor.Red, 5);
            shooter.Activate(grid);

            // Move to column 3
            for (int i = 0; i <= 3; i++)
            {
                shooter.MoveNext();
            }

            // Should have target in sight
            bool hasTarget = shooter.HasTargetInSight();
            Data.GridPosition? destroyed = shooter.Fire();

            bool success = hasTarget && destroyed.HasValue && destroyed.Value.Equals(pixelPos);
            Debug.Log($"Fired at target. Target detected: {hasTarget}, Destroyed at {destroyed}. {(success ? "PASS" : "FAIL")}");
        }

        private void TestLineOfSightBlocking()
        {
            Debug.Log("\n--- Test: Line-of-Sight Blocking ---");

            GridSystem grid = new GridSystem(8, 8);

            // Add a blue pixel at row 2, column 3 (blocker)
            Data.GridPosition blockerPos = new Data.GridPosition(2, 3);
            Data.Pixel bluePixel = new Data.Pixel(Data.PixelColor.Blue, blockerPos, 0);
            grid.AddPixel(bluePixel);

            // Add a red pixel behind it at row 1, column 3
            Data.GridPosition redPos = new Data.GridPosition(1, 3);
            Data.Pixel redPixel = new Data.Pixel(Data.PixelColor.Red, redPos, 0);
            grid.AddPixel(redPixel);

            // Red shooter should not fire because blue pixel blocks it
            Gameplay.Shooter redShooter = new Gameplay.Shooter(Data.PixelColor.Red, 5);
            redShooter.Activate(grid);

            for (int i = 0; i <= 3; i++)
            {
                redShooter.MoveNext();
            }

            bool redHasTarget = redShooter.HasTargetInSight();

            // Blue shooter should fire at the blocker
            Gameplay.Shooter blueShooter = new Gameplay.Shooter(Data.PixelColor.Blue, 5);
            blueShooter.Activate(grid);

            for (int i = 0; i <= 3; i++)
            {
                blueShooter.MoveNext();
            }

            bool blueHasTarget = blueShooter.HasTargetInSight();

            bool success = !redHasTarget && blueHasTarget;
            Debug.Log($"Red blocked: {!redHasTarget}, Blue has target: {blueHasTarget}. {(success ? "PASS" : "FAIL")}");
        }

        private void TestMultipleLayerDestruction()
        {
            Debug.Log("\n--- Test: Multiple Layer Destruction ---");

            GridSystem grid = new GridSystem(8, 8);
            Data.GridPosition pos = new Data.GridPosition(2, 3);

            // Add 3 red pixels at the same position (layered)
            grid.AddPixel(new Data.Pixel(Data.PixelColor.Red, pos, 0));
            grid.AddPixel(new Data.Pixel(Data.PixelColor.Red, pos, 1));
            grid.AddPixel(new Data.Pixel(Data.PixelColor.Red, pos, 2));

            Gameplay.Shooter shooter = new Gameplay.Shooter(Data.PixelColor.Red, 10);
            List<Data.GridPosition> destroyed = shooter.RunCompletePath();

            bool success = destroyed.Count == 3; // Should destroy all 3 layers at column 3
            Debug.Log($"Destroyed {destroyed.Count} pixels. Expected 3. {(success ? "PASS" : "FAIL")}");
        }

        private void TestOutOfBallsMidPath()
        {
            Debug.Log("\n--- Test: Out of Balls Mid-Path ---");

            GridSystem grid = new GridSystem(8, 8);

            // Add 5 red pixels
            for (int col = 0; col < 5; col++)
            {
                Data.GridPosition pos = new Data.GridPosition(2, col);
                grid.AddPixel(new Data.Pixel(Data.PixelColor.Red, pos, 0));
            }

            // Shooter with only 3 balls (not enough for all 5)
            Gameplay.Shooter shooter = new Gameplay.Shooter(Data.PixelColor.Red, 3);
            List<Data.GridPosition> destroyed = shooter.RunCompletePath();

            bool success = destroyed.Count == 3 && shooter.IsComplete && shooter.BallCount == 0;
            Debug.Log($"Destroyed {destroyed.Count} pixels with 3 balls. Shooter complete: {shooter.IsComplete}. {(success ? "PASS" : "FAIL")}");
        }

        private void TestNoValidTargets()
        {
            Debug.Log("\n--- Test: No Valid Targets ---");

            GridSystem grid = new GridSystem(8, 8);

            // Add only blue pixels
            Data.GridPosition pos = new Data.GridPosition(2, 3);
            grid.AddPixel(new Data.Pixel(Data.PixelColor.Blue, pos, 0));

            // Red shooter should not fire at all
            Gameplay.Shooter shooter = new Gameplay.Shooter(Data.PixelColor.Red, 10);
            List<Data.GridPosition> destroyed = shooter.RunCompletePath();

            bool success = destroyed.Count == 0 && shooter.BallCount == 10; // No shots fired
            Debug.Log($"Destroyed {destroyed.Count} pixels. Balls remaining: {shooter.BallCount}. {(success ? "PASS" : "FAIL")}");
        }

        private void TestLastPixelWin()
        {
            Debug.Log("\n--- Test: Last Pixel Instant Win ---");

            GridSystem grid = new GridSystem(8, 8);

            // Add exactly one red pixel
            Data.GridPosition pos = new Data.GridPosition(2, 3);
            grid.AddPixel(new Data.Pixel(Data.PixelColor.Red, pos, 0));

            ShooterManager manager = new ShooterManager(grid);
            manager.AddShooter(Data.PixelColor.Red, 1);

            bool winTriggered = false;
            manager.OnGameWon += () => winTriggered = true;

            Gameplay.Shooter shooter = manager.AvailableShooters[0];
            manager.ActivateShooter(shooter);
            manager.ExecuteShooterCompletePath();

            bool success = winTriggered && grid.IsGridEmpty();
            Debug.Log($"Win triggered: {winTriggered}, Grid empty: {grid.IsGridEmpty()}. {(success ? "PASS" : "FAIL")}");
        }

        private void TestShooterManager()
        {
            Debug.Log("\n--- Test: Shooter Manager (One Active at a Time) ---");

            GridSystem grid = new GridSystem(8, 8);
            ShooterManager manager = new ShooterManager(grid);

            manager.AddShooter(Data.PixelColor.Red, 5);
            manager.AddShooter(Data.PixelColor.Blue, 5);

            Gameplay.Shooter shooter1 = manager.AvailableShooters[0];
            Gameplay.Shooter shooter2 = manager.AvailableShooters[1];

            bool activated1 = manager.ActivateShooter(shooter1);
            bool activated2 = manager.ActivateShooter(shooter2); // Should fail - one already active

            bool success = activated1 && !activated2 && manager.ActiveShooter == shooter1;
            Debug.Log($"First activation: {activated1}, Second activation: {activated2}. {(success ? "PASS" : "FAIL")}");
        }
    }
}
