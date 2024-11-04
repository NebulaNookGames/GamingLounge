using System;
using UnityEngine;

/// <summary>
/// Manages the placement and removal of objects in the game world. 
/// Utilizes different building states for placement and removal, and interacts with the grid and preview systems.
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    /// <summary> The grid component used for positioning objects. </summary>
    [SerializeField] Grid grid;

    /// <summary> The input manager to handle player interactions. </summary>
    [SerializeField] InputManager inputManager;

    /// <summary>
    /// The reference object used for placement operations in the game.
    /// </summary>
    [SerializeField] public GameObject referenceObject;
    
    /// <summary> Database of all objects that can be placed, containing prefabs and metadata. </summary>
    [SerializeField] ObjectsDatabaseSO database;

    /// <summary> Visualization object that represents the grid during placement mode. </summary>
    [SerializeField] GameObject gridVisualization;

    /// <summary> Data for tracking floor object placements. </summary>
    private GridData floorData;

    /// <summary> Data for tracking wall object placements. </summary>
    private GridData wallData;

    /// <summary> Data for tracking wall decor placements. </summary>
    private GridData wallDecorData;

    /// <summary> Data for tracking furniture placements. </summary>
    private GridData furnitureData;

    /// <summary> System for displaying a preview of the object being placed. </summary>
    [SerializeField] PreviewSystem preview;

    /// <summary> The last detected grid position based on mouse movement, used for updating preview position. </summary>
    Vector3Int lastDetectedPosition = Vector3Int.zero;

    /// <summary> Handles the actual placement of objects in the game world. </summary>
    [SerializeField] ObjectPlacer objectPlacer;

    /// <summary> The current building state, which manages either placement or removal operations. </summary>
    IBuildingState buildingState;
    
    /// <summary>
    /// Initializes grid data and stops any active placement mode at the start.
    /// </summary>
    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        wallData = new GridData();
        wallDecorData = new GridData();
        furnitureData = new GridData();
    }

    /// <summary>
    /// Updates the placement preview based on mouse movement over the grid.
    /// </summary>
    private void Update()
    {
        if (buildingState == null) return;

        Vector3Int gridPosition = GetFrontGridPosition(referenceObject);

        // Check if the last detected position is different before updating the building state.
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    /// <summary>
    /// Starts the placement mode for an object with a specified ID.
    /// </summary>
    /// <param name="_id">The ID of the object to place.</param>
    public void StartPlacement(int _id)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        
        buildingState = new PlacementState(_id, grid, this, preview, database, floorData, wallData, wallDecorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    /// <summary>
    /// Starts the removal mode to delete objects from the grid.
    /// </summary>
    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        
        buildingState = new RemovingState(grid, this, preview, floorData, wallData, wallDecorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    /// <summary>
    /// Places the selected structure at the current grid position, if valid.
    /// </summary>
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;
        
        // Attempt to place or remove the structure at the grid position.
        buildingState.OnAction(GetFrontGridPosition(referenceObject));
    }

    /// <summary>
    /// Stops the current placement or removal mode and resets related settings.
    /// </summary>
    public void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        
        // Unsubscribe event handlers and reset state.
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    /// <summary>
    /// Calculates the grid position directly in front of the specified reference object.
    /// </summary>
    /// <param name="referenceObject">The game object used as reference to determine the front position.</param>
    /// <returns>A Vector3Int representing the grid cell position in front of the reference object.</returns>
    public Vector3Int GetFrontGridPosition(GameObject referenceObject)
    {
        Vector3 frontPosition = referenceObject.transform.position + referenceObject.transform.forward;
        return grid.WorldToCell(frontPosition);
    }
}