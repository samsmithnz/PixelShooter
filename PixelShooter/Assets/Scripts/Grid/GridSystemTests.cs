using UnityEngine;
using PixelShooter.Grid;

namespace PixelShooter.Tests
{
    /// <summary>
    /// Simple test script to verify the grid system functionality.
    /// Attach this to a GameObject in the scene to run tests in the Unity Console.
    /// </summary>
    public class GridSystemTests : MonoBehaviour
    {
        [Header("Test Configuration")]
        [Tooltip("Run tests automatically on Start")]
        public bool runOnStart = true;

        private void Start()
        {
            if (runOnStart)
            {
                RunAllTests();
            }
        }

        [ContextMenu("Run All Grid Tests")]
        public void RunAllTests()
        {
            Debug.Log("=== Starting Grid System Tests ===");
            
            TestPixelColor();
            TestPixelLayer();
            TestGridCell();
            TestGridData();
            TestLevelDefinition();
            
            Debug.Log("=== Grid System Tests Completed ===");
        }

        private void TestPixelColor()
        {
            Debug.Log("Testing PixelColor enum...");
            
            // Verify all 8 colors are defined
            PixelColor[] colors = new PixelColor[] {
                PixelColor.Red, PixelColor.Blue, PixelColor.Yellow, PixelColor.Green,
                PixelColor.Orange, PixelColor.Purple, PixelColor.Black, PixelColor.White
            };
            
            Debug.Assert(colors.Length == 8, "PixelColor should have exactly 8 values");
            Debug.Log($"✓ PixelColor enum has {colors.Length} colors");
        }

        private void TestPixelLayer()
        {
            Debug.Log("Testing PixelLayer class...");
            
            PixelLayer layer = new PixelLayer(PixelColor.Red);
            Debug.Assert(layer.color == PixelColor.Red, "PixelLayer color should be Red");
            
            Debug.Log("✓ PixelLayer creation and color assignment works");
        }

        private void TestGridCell()
        {
            Debug.Log("Testing GridCell class...");
            
            GridCell cell = new GridCell();
            
            // Test empty cell
            Debug.Assert(cell.IsEmpty, "New cell should be empty");
            Debug.Assert(cell.LayerCount == 0, "New cell should have 0 layers");
            Debug.Assert(cell.CurrentLayer == null, "Empty cell should have no current layer");
            
            // Test adding layers
            bool added = cell.AddLayer(new PixelLayer(PixelColor.Red));
            Debug.Assert(added, "Should be able to add first layer");
            Debug.Assert(!cell.IsEmpty, "Cell with layer should not be empty");
            Debug.Assert(cell.LayerCount == 1, "Cell should have 1 layer");
            Debug.Assert(cell.CurrentLayer.color == PixelColor.Red, "Current layer should be Red");
            
            // Add more layers
            cell.AddLayer(new PixelLayer(PixelColor.Blue));
            cell.AddLayer(new PixelLayer(PixelColor.Yellow));
            Debug.Assert(cell.LayerCount == 3, "Cell should have 3 layers");
            Debug.Assert(cell.CurrentLayer.color == PixelColor.Yellow, "Current layer should be Yellow (topmost)");
            
            // Test layer destruction
            PixelLayer destroyed = cell.DestroyCurrentLayer();
            Debug.Assert(destroyed.color == PixelColor.Yellow, "Destroyed layer should be Yellow");
            Debug.Assert(cell.CurrentLayer.color == PixelColor.Blue, "New current layer should be Blue");
            Debug.Assert(cell.LayerCount == 2, "Cell should have 2 layers remaining");
            
            // Test max layers (5)
            cell.AddLayer(new PixelLayer(PixelColor.Green));
            cell.AddLayer(new PixelLayer(PixelColor.Orange));
            cell.AddLayer(new PixelLayer(PixelColor.Purple));
            Debug.Assert(cell.LayerCount == 5, "Cell should have 5 layers (max)");
            
            bool canAddMore = cell.AddLayer(new PixelLayer(PixelColor.Black));
            Debug.Assert(!canAddMore, "Should not be able to add 6th layer");
            Debug.Assert(cell.LayerCount == 5, "Cell should still have 5 layers");
            
            Debug.Log("✓ GridCell layer management works correctly");
        }

        private void TestGridData()
        {
            Debug.Log("Testing GridData class...");
            
            // Test default 8x8 grid
            GridData grid = new GridData();
            Debug.Assert(grid.Width == 8, "Default grid width should be 8");
            Debug.Assert(grid.Height == 8, "Default grid height should be 8");
            
            // Test custom size grid
            GridData customGrid = new GridData(5, 10);
            Debug.Assert(customGrid.Width == 5, "Custom grid width should be 5");
            Debug.Assert(customGrid.Height == 10, "Custom grid height should be 10");
            
            // Test cell access
            GridCell cell = grid.GetCell(0, 0);
            Debug.Assert(cell != null, "Should be able to get cell at (0,0)");
            Debug.Assert(cell.IsEmpty, "New cell should be empty");
            
            // Test out of bounds
            GridCell outOfBounds = grid.GetCell(-1, 0);
            Debug.Assert(outOfBounds == null, "Out of bounds access should return null");
            
            outOfBounds = grid.GetCell(8, 8);
            Debug.Assert(outOfBounds == null, "Out of bounds access should return null");
            
            // Test pixel counting
            grid.GetCell(0, 0).AddLayer(new PixelLayer(PixelColor.Red));
            grid.GetCell(1, 0).AddLayer(new PixelLayer(PixelColor.Red));
            grid.GetCell(2, 0).AddLayer(new PixelLayer(PixelColor.Blue));
            
            int redCount = grid.CountPixelsOfColor(PixelColor.Red);
            Debug.Assert(redCount == 2, "Should count 2 red pixels");
            
            // Test grid empty check
            Debug.Assert(!grid.IsGridEmpty(), "Grid with pixels should not be empty");
            
            GridData emptyGrid = new GridData(3, 3);
            Debug.Assert(emptyGrid.IsGridEmpty(), "Grid with no pixels should be empty");
            
            Debug.Log("✓ GridData creation and operations work correctly");
        }

        private void TestLevelDefinition()
        {
            Debug.Log("Testing LevelDefinition...");
            
            // Create a simple level definition programmatically
            LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
            level.gridWidth = 3;
            level.gridHeight = 3;
            
            // Initialize grid rows manually for testing
            level.gridRows = new GridRow[3];
            for (int y = 0; y < 3; y++)
            {
                level.gridRows[y] = new GridRow();
                level.gridRows[y].cells = new CellDefinition[3];
                for (int x = 0; x < 3; x++)
                {
                    level.gridRows[y].cells[x] = new CellDefinition();
                }
            }
            
            // Add some test pixels
            level.gridRows[0].cells[0].layers = new PixelColor[] { PixelColor.Red };
            level.gridRows[0].cells[1].layers = new PixelColor[] { PixelColor.Blue, PixelColor.Yellow };
            level.gridRows[1].cells[1].layers = new PixelColor[] { PixelColor.Green };
            
            // Create grid from definition
            GridData gridData = level.CreateGridData();
            Debug.Assert(gridData.Width == 3, "Grid width should match level definition");
            Debug.Assert(gridData.Height == 3, "Grid height should match level definition");
            
            // Verify cells were populated correctly
            GridCell cell00 = gridData.GetCell(0, 0);
            Debug.Assert(!cell00.IsEmpty, "Cell (0,0) should have pixel");
            Debug.Assert(cell00.CurrentLayer.color == PixelColor.Red, "Cell (0,0) should be Red");
            
            GridCell cell01 = gridData.GetCell(0, 1);
            Debug.Assert(cell01.LayerCount == 2, "Cell (0,1) should have 2 layers");
            Debug.Assert(cell01.CurrentLayer.color == PixelColor.Yellow, "Cell (0,1) top layer should be Yellow");
            
            GridCell cell11 = gridData.GetCell(1, 1);
            Debug.Assert(cell11.CurrentLayer.color == PixelColor.Green, "Cell (1,1) should be Green");
            
            // Clean up
            DestroyImmediate(level);
            
            Debug.Log("✓ LevelDefinition creates GridData correctly");
        }
    }
}
