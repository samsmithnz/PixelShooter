using UnityEngine;
using UnityEditor;
using PixelShooter.Grid;

namespace PixelShooter.Editor
{
    /// <summary>
    /// Editor utility to populate a LevelDefinition with sample data.
    /// Menu: Tools > PixelShooter > Populate Level with Sample Data
    /// </summary>
    public class LevelDataPopulator
    {
        [MenuItem("Tools/PixelShooter/Populate Level with Sample Data")]
        public static void PopulateSampleLevel()
        {
            // Find the first LevelDefinition asset
            string[] guids = AssetDatabase.FindAssets("t:LevelDefinition");
            if (guids.Length == 0)
            {
                Debug.LogError("No LevelDefinition assets found! Create one first: Assets > Create > PixelShooter > Level Definition");
                return;
            }

            // List all found levels
            Debug.Log($"Found {guids.Length} LevelDefinition assets:");
            LevelDefinition[] allLevels = new LevelDefinition[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                allLevels[i] = AssetDatabase.LoadAssetAtPath<LevelDefinition>(path);
                Debug.Log($"  [{i}] {allLevels[i].name} at {path}");
            }

            // If only one, use it. Otherwise, let user choose
            LevelDefinition levelDef;
            if (guids.Length == 1)
            {
                levelDef = allLevels[0];
            }
            else
            {
                // Show selection dialog
                string[] options = new string[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                {
                    options[i] = allLevels[i].name;
                }
                
                // Use EditorUtility to let user pick
                int choice = EditorUtility.DisplayDialogComplex(
                    "Multiple Levels Found",
                    $"Found {guids.Length} level assets. Which one do you want to populate?\n\n" +
                    string.Join("\n", options),
                    options.Length > 0 ? options[0] : "First",
                    "Cancel",
                    options.Length > 1 ? options[1] : ""
                );

                if (choice == 1) // Cancel
                {
                    Debug.Log("Cancelled level population");
                    return;
                }
                
                int index = choice == 0 ? 0 : (choice == 2 && options.Length > 1 ? 1 : 0);
                levelDef = allLevels[index];
            }

            if (levelDef == null)
            {
                Debug.LogError("Failed to load LevelDefinition!");
                return;
            }

            Debug.Log($"Selected level to populate: {levelDef.name}");

            // Set grid size
            levelDef.gridWidth = 8;
            levelDef.gridHeight = 8;

            // Initialize the grid rows array
            levelDef.gridRows = new GridRow[levelDef.gridHeight];
            
            // Create a simple pattern with different colored pixels
            for (int y = 0; y < levelDef.gridHeight; y++)
            {
                levelDef.gridRows[y] = new GridRow();
                levelDef.gridRows[y].cells = new CellDefinition[levelDef.gridWidth];
                
                for (int x = 0; x < levelDef.gridWidth; x++)
                {
                    levelDef.gridRows[y].cells[x] = new CellDefinition();
                    
                    // Create a checkerboard pattern with different colors
                    if ((x + y) % 2 == 0)
                    {
                        // Even squares get red pixels
                        levelDef.gridRows[y].cells[x].layers = new PixelColor[] { PixelColor.Red };
                    }
                    else
                    {
                        // Odd squares get blue pixels
                        levelDef.gridRows[y].cells[x].layers = new PixelColor[] { PixelColor.Blue };
                    }
                    
                    // Add some variety - make corners have multiple layers
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

            // Mark as dirty and save
            EditorUtility.SetDirty(levelDef);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Successfully populated {levelDef.name} with sample data!");
            Debug.Log($"Grid: {levelDef.gridWidth}x{levelDef.gridHeight}");
            Debug.Log("Colors used: Red, Blue, Green, Yellow");
            
            // Select the asset so user can see it
            Selection.activeObject = levelDef;
        }
    }
}
