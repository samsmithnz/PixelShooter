using System;
using UnityEngine;

namespace PixelShooter.Grid
{
    /// <summary>
    /// Serializable representation of a single cell's layers for level definition.
    /// Used to define pixel layers in the Unity Inspector.
    /// </summary>
    [Serializable]
    public class CellDefinition
    {
        [Tooltip("Pixel layers from bottom to top (up to 5 layers)")]
        public PixelColor[] layers = new PixelColor[0];
    }

    /// <summary>
    /// ScriptableObject for defining levels in the Unity Editor.
    /// Provides an editor-friendly way to create and configure pixel grid levels.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevel", menuName = "PixelShooter/Level Definition")]
    public class LevelDefinition : ScriptableObject
    {
        [Header("Grid Configuration")]
        [Tooltip("Width of the grid (5-10+)")]
        [Range(GridData.MinGridSize, 15)]
        public int gridWidth = GridData.DefaultGridSize;

        [Tooltip("Height of the grid (5-10+)")]
        [Range(GridData.MinGridSize, 15)]
        public int gridHeight = GridData.DefaultGridSize;

        [Header("Grid Layout")]
        [Tooltip("Define the grid cells row by row, from top to bottom")]
        public CellDefinition[][] gridCells = new CellDefinition[0][];

        /// <summary>
        /// Creates a GridData instance from this level definition.
        /// </summary>
        public GridData CreateGridData()
        {
            GridData gridData = new GridData(gridWidth, gridHeight);

            // Populate the grid with the defined cells
            for (int y = 0; y < gridHeight && y < gridCells.Length; y++)
            {
                if (gridCells[y] != null)
                {
                    for (int x = 0; x < gridWidth && x < gridCells[y].Length; x++)
                    {
                        CellDefinition cellDef = gridCells[y][x];
                        if (cellDef != null && cellDef.layers != null)
                        {
                            GridCell cell = gridData.GetCell(x, y);
                            if (cell != null)
                            {
                                // Add layers from bottom to top
                                foreach (PixelColor color in cellDef.layers)
                                {
                                    cell.AddLayer(new PixelLayer(color));
                                }
                            }
                        }
                    }
                }
            }

            return gridData;
        }

        /// <summary>
        /// Validates the level definition in the editor.
        /// </summary>
        private void OnValidate()
        {
            // Ensure grid dimensions are within reasonable bounds
            gridWidth = Mathf.Max(GridData.MinGridSize, gridWidth);
            gridHeight = Mathf.Max(GridData.MinGridSize, gridHeight);

            // Initialize grid cells if empty
            if (gridCells == null || gridCells.Length != gridHeight)
            {
                gridCells = new CellDefinition[gridHeight][];
                for (int y = 0; y < gridHeight; y++)
                {
                    gridCells[y] = new CellDefinition[gridWidth];
                    for (int x = 0; x < gridWidth; x++)
                    {
                        gridCells[y][x] = new CellDefinition();
                    }
                }
            }
            else
            {
                // Ensure each row has the correct width
                for (int y = 0; y < gridCells.Length; y++)
                {
                    if (gridCells[y] == null || gridCells[y].Length != gridWidth)
                    {
                        CellDefinition[] newRow = new CellDefinition[gridWidth];
                        int existingWidth = gridCells[y]?.Length ?? 0;
                        
                        if (gridCells[y] != null)
                        {
                            // Copy existing cells
                            int copyCount = Mathf.Min(existingWidth, gridWidth);
                            Array.Copy(gridCells[y], newRow, copyCount);
                        }
                        
                        // Initialize any new cells
                        for (int x = existingWidth; x < gridWidth; x++)
                        {
                            newRow[x] = new CellDefinition();
                        }
                        gridCells[y] = newRow;
                    }
                }
            }
        }
    }
}
