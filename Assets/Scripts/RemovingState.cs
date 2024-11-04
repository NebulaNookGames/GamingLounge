using UnityEngine;

/// <summary>
/// Represents the state in which objects can be removed from the grid. Manages preview visuals and removal actions based on the selected grid cell.
/// </summary>
public class RemovingState : IBuildingState
{
    /// <summary> Index of the object currently selected for removal, if applicable. </summary>
    private int gameObjectIndex = -1;

    /// <summary> The grid used for cell-based positioning in the world. </summary>
    private Grid grid;

    /// <summary> Manager for placement and removal operations within the current building state. </summary>
    PlacementSystem placementSystem;

    /// <summary> The system used to show visual feedback during object removal. </summary>
    private PreviewSystem previewSystem;

    /// <summary> Grid data for floor objects. </summary>
    private GridData floorData;

    /// <summary> Grid data for wall objects. </summary>
    private GridData wallData;

    /// <summary> Grid data for wall decoration objects. </summary>
    private GridData wallDecorData;

    /// <summary> Grid data for furniture objects. </summary>
    private GridData furnitureData;

    /// <summary> Manages the actual placement and removal of objects in the scene. </summary>
    private ObjectPlacer objectPlacer;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemovingState"/> class.
    /// Sets up references and initiates the preview system for removal.
    /// </summary>
    /// <param name="grid">The grid used for positioning objects.</param>
    /// <param name="previewSystem">The system for displaying removal previews.</param>
    /// <param name="floorData">Data for managing floor placements.</param>
    /// <param name="wallData">Data for managing wall placements.</param>
    /// <param name="wallDecorData">Data for managing wall decoration placements.</param>
    /// <param name="furnitureData">Data for managing furniture placements.</param>
    /// <param name="objectPlacer">Handles object placement and removal operations.</param>
    public RemovingState(Grid grid,
                         PlacementSystem placementSystem,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData wallData,
                         GridData wallDecorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.placementSystem = placementSystem;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.wallData = wallData;
        this.wallDecorData = wallDecorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    /// <summary>
    /// Ends the removal state, stopping any active previews.
    /// </summary>
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Handles the removal of an object at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position to remove an object from.</param>
    public void OnAction(Vector3Int gridPosition)
    {
        if (gridPosition != placementSystem.GetFrontGridPosition(placementSystem.referenceObject)) return;
        
        GridData selectedData = null;

        if (!floorData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = floorData;
        }
        else if (!wallData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = wallData;
        }
        else if (!wallDecorData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = wallDecorData;
        }
        else if (!furnitureData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = furnitureData;
        }

        if (selectedData == null)
        {
            // No object to remove at this position, potentially trigger a sound or feedback.
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1) return;

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    /// <summary>
    /// Checks if there is an object to remove at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position to check for object presence.</param>
    /// <returns>True if an object is present at the specified position; otherwise, false.</returns>
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(floorData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallDecorData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && furnitureData.canPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    /// <summary>
    /// Updates the preview system to reflect the current position and validity of removal.
    /// </summary>
    /// <param name="gridPosition">The grid position to update the preview for.</param>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}