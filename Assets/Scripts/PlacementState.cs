using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

/// <summary>
/// Handles the placement logic for building objects in the game. 
/// Manages preview systems, grid data, and placement validity checks.
/// </summary>
public class PlacementState : IBuildingState
{
    /// <summary> Index of the currently selected object to be placed. </summary>
    private int selectedObjectIndex = -1;

    /// <summary> ID of the object being placed. </summary>
    int ID;

    /// <summary> The grid system used for object placement. </summary>
    Grid grid;

    /// <summary> Manager for placement and removal operations within the current building state. </summary>
    PlacementSystem placementSystem;

    /// <summary> Controls the preview display of objects while placing. </summary>
    PreviewSystem previewSystem;

    /// <summary> Database holding data for all placeable objects. </summary>
    ObjectsDatabaseSO database;

    /// <summary> Data for floor placements. </summary>
    GridData floorData;

    /// <summary> Data for wall placements. </summary>
    GridData wallData;

    /// <summary> Data for wall decoration placements. </summary>
    GridData wallDecorData;

    /// <summary> Data for furniture placements. </summary>
    GridData furnitureData;

    /// <summary> Handles the actual placing of objects in the game world. </summary>
    ObjectPlacer objectPlacer;

    /// <summary>
    /// Initializes a new instance of <see cref="PlacementState"/> with dependencies injected.
    /// </summary>
    /// <param name="iD">The ID of the object being placed.</param>
    /// <param name="grid">The grid system used for positioning objects.</param>
    /// <param name="previewSystem">The preview system for showing placement previews.</param>
    /// <param name="database">The database containing object data.</param>
    /// <param name="floorData">Grid data for floor objects.</param>
    /// <param name="wallData">Grid data for wall objects.</param>
    /// <param name="wallDecorData">Grid data for wall decor objects.</param>
    /// <param name="furnitureData">Grid data for furniture objects.</param>
    /// <param name="objectPlacer">The object placer for placing objects in the game world.</param>
    /// <exception cref="System.Exception">Thrown if no object with the given ID exists in the database.</exception>
    public PlacementState(int iD,
                          Grid grid,
                          PlacementSystem placementSystem,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData wallData,
                          GridData wallDecorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        selectedObjectIndex = iD;
        this.grid = grid;
        this.placementSystem = placementSystem;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.wallData = wallData;
        this.wallDecorData = wallDecorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        // Find the selected object index in the database and start showing the preview.
        if (selectedObjectIndex > -1)
        {
            selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab,
                                                       database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }
    }

    /// <summary>
    /// Ends the current placement state by stopping the preview system.
    /// </summary>
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Checks if the selected object can be placed at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The position on the grid to check for placement.</param>
    /// <param name="selectedObjectIndex">Index of the selected object.</param>
    /// <returns>True if placement is valid; otherwise, false.</returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // Get the relevant grid data based on the type of object being placed.
        GridData selectedData = GetUsedGridData();
        return selectedData.canPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    /// <summary>
    /// Places the selected object at the specified grid position, if placement is valid.
    /// </summary>
    /// <param name="gridPosition">The position on the grid where the object should be placed.</param>
    public void OnAction(Vector3Int gridPosition)
    {
        if (gridPosition != placementSystem.GetFrontGridPosition(placementSystem.referenceObject)) return;
        
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;

        // Place the object and get the index for tracking its position in the grid data.
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        // Add object to the appropriate grid data to track its placement.
        GridData selectedData = GetUsedGridData();
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);

        // Update the preview system position.
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// Determines the correct grid data to use based on the object's type.
    /// </summary>
    /// <returns>The grid data corresponding to the object type.</returns>
    private GridData GetUsedGridData()
    {
        // Choose grid data based on object type.
        switch (database.objectsData[selectedObjectIndex].objectType)
        {
            case ObjectType.Ground:
                return floorData;
            case ObjectType.Wall:
                return wallData;
            case ObjectType.WallDecor:
                return wallDecorData;
            case ObjectType.Furniture:
                return furnitureData;
            default:
                return floorData;
        }
    }

    /// <summary>
    /// Updates the preview system's position based on the placement validity at the given position.
    /// </summary>
    /// <param name="gridPosition">The position on the grid for the preview update.</param>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        // Update the preview position and validity display.
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
