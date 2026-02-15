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
    /// Serializable row wrapper for grid cells.
    /// Unity cannot serialize jagged arrays (Type[][]), so we wrap each row.
    /// </summary>
    [Serializable]
    public class GridRow
    {
        public CellDefinition[] cells = new CellDefinition[0];
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
        public GridRow[] gridRows = new GridRow[0];

        /// <summary>
        /// Creates a GridData instance from this level definition.
        /// </summary>
        public GridData CreateGridData()
        {
            Debug.Log($"[LevelDefinition] CreateGridData() called for '{name}'");
            Debug.Log($"[LevelDefinition] Grid size: {gridWidth}x{gridHeight}");
            Debug.Log($"[LevelDefinition] GridRows array: {(gridRows != null ? $"{gridRows.Length} rows" : "NULL")}");
            
            GridData gridData = new GridData(gridWidth, gridHeight);

            int cellsWithData = 0;
            int totalLayersAdded = 0;
            
            // Populate the grid with the defined cells
            for (int y = 0; y < gridHeight && y < gridRows.Length; y++)
            {
                if (gridRows[y] != null && gridRows[y].cells != null)
                {
                    for (int x = 0; x < gridWidth && x < gridRows[y].cells.Length; x++)
                    {
                        CellDefinition cellDef = gridRows[y].cells[x];
                        if (cellDef != null && cellDef.layers != null && cellDef.layers.Length > 0)
                        {
                            cellsWithData++;
                            GridCell cell = gridData.GetCell(x, y);
                            if (cell != null)
                            {
                                // Add layers from bottom to top
                                foreach (PixelColor color in cellDef.layers)
                                {
                                    cell.AddLayer(new PixelLayer(color));
                                    totalLayersAdded++;
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log($"[LevelDefinition] Populated {cellsWithData} cells with {totalLayersAdded} total layers");
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

            // Initialize grid rows if empty
            if (gridRows == null || gridRows.Length != gridHeight)
            {
                gridRows = new GridRow[gridHeight];
                for (int y = 0; y < gridHeight; y++)
                {
                    gridRows[y] = new GridRow();
                    gridRows[y].cells = new CellDefinition[gridWidth];
                    for (int x = 0; x < gridWidth; x++)
                    {
                        gridRows[y].cells[x] = new CellDefinition();
                    }
                }
            }
            else
            {
                // Ensure each row has the correct width
                for (int y = 0; y < gridRows.Length; y++)
                {
                    if (gridRows[y] == null)
                    {
                        gridRows[y] = new GridRow();
                        gridRows[y].cells = new CellDefinition[gridWidth];
                        for (int x = 0; x < gridWidth; x++)
                        {
                            gridRows[y].cells[x] = new CellDefinition();
                        }
                    }
                    else if (gridRows[y].cells == null || gridRows[y].cells.Length != gridWidth)
                    {
                        CellDefinition[] newRow = new CellDefinition[gridWidth];
                        int existingWidth = gridRows[y].cells?.Length ?? 0;
                        
                        if (gridRows[y].cells != null)
                        {
                            // Copy existing cells
                            int copyCount = Mathf.Min(existingWidth, gridWidth);
                            Array.Copy(gridRows[y].cells, newRow, copyCount);
                        }
                        
                        // Initialize any new cells
                        for (int x = existingWidth; x < gridWidth; x++)
                        {
                            newRow[x] = new CellDefinition();
                        }
                        gridRows[y].cells = newRow;
                    }
                }
            }
        }
    }
}
