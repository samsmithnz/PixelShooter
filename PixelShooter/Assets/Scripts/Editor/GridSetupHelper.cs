using UnityEngine;
using UnityEditor;
using PixelShooter.Grid;

namespace PixelShooter.Editor
{
    /// <summary>
    /// Editor utility to help set up the GridRenderer in the scene.
    /// This can be run from Unity's menu: Tools > PixelShooter > Setup Grid Renderer
    /// </summary>
    public class GridSetupHelper
    {
        [MenuItem("Tools/PixelShooter/Setup Grid Renderer")]
        public static void SetupGridRenderer()
        {
            // Check if a GridRenderer already exists
            GridRenderer existingRenderer = FindObjectOfType<GridRenderer>();
            if (existingRenderer != null)
            {
                Debug.LogWarning("A GridRenderer already exists in the scene.");
                Selection.activeGameObject = existingRenderer.gameObject;
                return;
            }

            // Create a new GameObject with GridRenderer
            GameObject gridObj = new GameObject("GridRenderer");
            GridRenderer renderer = gridObj.AddComponent<GridRenderer>();

            // Try to load the sample level
            string[] guids = AssetDatabase.FindAssets("t:LevelDefinition");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                LevelDefinition levelDef = AssetDatabase.LoadAssetAtPath<LevelDefinition>(path);
                renderer.levelDefinition = levelDef;
                Debug.Log($"Loaded level definition: {levelDef.name}");
            }

            // Try to load the pixel sprite
            string[] spriteGuids = AssetDatabase.FindAssets("PixelSprite t:Sprite");
            if (spriteGuids.Length > 0)
            {
                string spritePath = AssetDatabase.GUIDToAssetPath(spriteGuids[0]);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                renderer.pixelSprite = sprite;
                Debug.Log($"Loaded pixel sprite: {sprite.name}");
            }

            // Set default values
            renderer.cellSize = 1f;
            renderer.cellSpacing = 0.1f;

            // Select the newly created object
            Selection.activeGameObject = gridObj;
            
            Debug.Log("GridRenderer setup completed successfully!");
        }
    }
}
