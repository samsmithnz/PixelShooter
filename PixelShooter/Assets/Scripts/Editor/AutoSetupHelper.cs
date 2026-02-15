using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using PixelShooter.Grid;

namespace PixelShooter.Editor
{
    /// <summary>
    /// Automatic setup helper that runs when Unity starts or scenes are loaded.
    /// Checks for missing setup and offers to fix it.
    /// </summary>
    [InitializeOnLoad]
    public class AutoSetupHelper
    {
        static AutoSetupHelper()
        {
            EditorApplication.delayCall += CheckSetup;
        }

        private static void CheckSetup()
        {
            // Only check in edit mode, not play mode
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            // Find all LevelDefinition assets
            string[] guids = AssetDatabase.FindAssets("t:LevelDefinition");
            
            bool foundEmptyLevel = false;
            LevelDefinition emptyLevel = null;

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LevelDefinition levelDef = AssetDatabase.LoadAssetAtPath<LevelDefinition>(path);

                // Check if level is empty or has no populated cells
                if (levelDef != null)
                {
                    bool isEmpty = (levelDef.gridRows == null || levelDef.gridRows.Length == 0);
                    
                    if (!isEmpty)
                    {
                        // Check if any cells are actually populated
                        int populatedCount = 0;
                        for (int y = 0; y < levelDef.gridRows.Length; y++)
                        {
                            if (levelDef.gridRows[y] != null && levelDef.gridRows[y].cells != null)
                            {
                                for (int x = 0; x < levelDef.gridRows[y].cells.Length; x++)
                                {
                                    if (levelDef.gridRows[y].cells[x] != null && 
                                        levelDef.gridRows[y].cells[x].layers != null && 
                                        levelDef.gridRows[y].cells[x].layers.Length > 0)
                                    {
                                        populatedCount++;
                                    }
                                }
                            }
                        }
                        isEmpty = (populatedCount == 0);
                    }
                    
                    if (isEmpty)
                    {
                        foundEmptyLevel = true;
                        emptyLevel = levelDef;
                        Debug.Log($"Found empty level: {levelDef.name}");
                        break;
                    }
                }
            }

            if (foundEmptyLevel && emptyLevel != null)
            {
                bool populate = EditorUtility.DisplayDialog(
                    "Empty Level Detected",
                    $"The level '{emptyLevel.name}' has no pixel data.\n\n" +
                    "Would you like to populate it with sample data now?\n\n" +
                    "(8x8 grid with a colorful checkerboard pattern)",
                    "Yes, Populate It",
                    "No, I'll Do It Later"
                );

                if (populate)
                {
                    PopulateLevel(emptyLevel);
                }
            }
        }

        private static void PopulateLevel(LevelDefinition levelDef)
        {
            // Set grid size
            levelDef.gridWidth = 8;
            levelDef.gridHeight = 8;

            // Initialize the grid rows array
            levelDef.gridRows = new GridRow[levelDef.gridHeight];
            
            // Create a checkerboard pattern
            for (int y = 0; y < levelDef.gridHeight; y++)
            {
                levelDef.gridRows[y] = new GridRow();
                levelDef.gridRows[y].cells = new CellDefinition[levelDef.gridWidth];
                
                for (int x = 0; x < levelDef.gridWidth; x++)
                {
                    levelDef.gridRows[y].cells[x] = new CellDefinition();
                    
                    // Checkerboard with different colors
                    if ((x + y) % 2 == 0)
                    {
                        levelDef.gridRows[y].cells[x].layers = new PixelColor[] { PixelColor.Red };
                    }
                    else
                    {
                        levelDef.gridRows[y].cells[x].layers = new PixelColor[] { PixelColor.Blue };
                    }
                    
                    // Corners get multiple layers
                    if ((x == 0 || x == levelDef.gridWidth - 1) && 
                        (y == 0 || y == levelDef.gridHeight - 1))
                    {
                        levelDef.gridRows[y].cells[x].layers = new PixelColor[] 
                        { 
                            PixelColor.Green, 
                            PixelColor.Yellow 
                        };
                    }
                }
            }

            // Save changes
            EditorUtility.SetDirty(levelDef);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"? Successfully populated '{levelDef.name}' with sample data!");
            Debug.Log($"  Grid: {levelDef.gridWidth}x{levelDef.gridHeight}");
            Debug.Log($"  Colors: Red, Blue, Green, Yellow");
            
            EditorUtility.DisplayDialog(
                "Level Populated!",
                $"'{levelDef.name}' now has sample data.\n\n" +
                "Press Play to see your game in action!",
                "OK"
            );
        }
    }
}
