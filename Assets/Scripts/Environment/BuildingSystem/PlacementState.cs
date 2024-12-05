using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the placement of objects within a building system.
/// Implements the IBuildingState interface for handling different states of building.
/// </summary>
public class PlacementState : IBuildingState
{
    // Index of the currently selected object
    private int selectedObjectIndex = -1;

    // ID of the object to be placed
    private int ID;

    // Grid system used for placing objects
    private Grid grid;

    // Preview system for showing placement previews
    private PreviewSystem previewSystem;

    // System for handling placement logic
    private PlacementSystem placementSystem;

    // Database containing available objects to place
    private ObjectsDatabaseSO database;

    // Grid data for different object types
    private GridData floorData, wallData, wallDecorData, furnitureData;

    // System for managing placed objects
    private ObjectPlacer objectPlacer;

    /// <summary>
    /// Initializes a new instance of the PlacementState class.
    /// </summary>
    /// <param name="iD">The ID of the object to be placed.</param>
    /// <param name="grid">The grid system used for placement.</param>
    /// <param name="previewSystem">The preview system for showing placement previews.</param>
    /// <param name="placementSystem">The placement system managing object placement.</param>
    /// <param name="database">The database of objects available for placement.</param>
    /// <param name="floorData">Grid data for floor objects.</param>
    /// <param name="wallData">Grid data for wall objects.</param>
    /// <param name="wallDecorData">Grid data for wall decoration objects.</param>
    /// <param name="furnitureData">Grid data for furniture objects.</param>
    /// <param name="objectPlacer">The object placer system for managing placed objects.</param>
    public PlacementState(int iD,
        Grid grid,
        PreviewSystem previewSystem,
        PlacementSystem placementSystem,
        ObjectsDatabaseSO database,
        GridData floorData,
        GridData wallData,
        GridData wallDecorData,
        GridData furnitureData,
        ObjectPlacer objectPlacer)
    {
        ID = iD;
        selectedObjectIndex = iD;
        this.placementSystem = placementSystem;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.wallData = wallData;
        this.wallDecorData = wallDecorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

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
    /// Ends the current placement state, stopping the preview.
    /// </summary>
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Checks if the object can be placed at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position to check.</param>
    /// <param name="selectedObjectIndex">The index of the selected object.</param>
    /// <returns>True if the object can be placed, false otherwise.</returns>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        if (database.objectsData[selectedObjectIndex].cost > MoneyManager.instance.MoneyAmount) return false;

        Vector3 newPosition = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = placementSystem.cam.nearClipPlane;
        Ray ray = placementSystem.cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementSystem.placementMask))
        {
            newPosition = hit.point;
        }

        if (Vector3.Distance(placementSystem.player.transform.position, newPosition) > 3)
        {
            return false;
        }

        if (database.objectsData[selectedObjectIndex].shouldCheckForOverlap)
        {
            // Get the mesh's bounds
            MeshRenderer meshRenderer = previewSystem.previewObject.GetComponentInChildren<MeshRenderer>();
            Bounds meshBounds = meshRenderer.bounds;

            // Calculate the half-extents from the mesh's size
            Vector3 halfExtents = meshBounds.size / 2f;

            Collider[] hitColliders =
                Physics.OverlapBox(
                    previewSystem.previewObject.GetComponentInChildren<MeshRenderer>().transform.position,
                    halfExtents, Quaternion.identity,
                    database.objectsData[selectedObjectIndex].overlapCheckingLayermask);
            if (hitColliders.Length > 0)
            {
                foreach (Collider collider in hitColliders)
                {
                    if (collider.transform.root != previewSystem.previewObject.transform)
                    {
                        return false;
                    }
                }
                
            }
        }

        if (database.objectsData[selectedObjectIndex].shouldCheckForDatabase)
        {
            bool canPlace = true;

            for (int i = 0; i < database.objectsData[selectedObjectIndex].collisionObjectTypes.Length; i++)
            {
                switch (database.objectsData[selectedObjectIndex].collisionObjectTypes[i])
                {
                    case ObjectType.Ground:
                        canPlace = floorData.CanPlaceObjectAt(gridPosition, previewSystem.sizeToUse);
                        break;
                    case ObjectType.Wall:
                        canPlace = wallData.CanPlaceObjectAt(gridPosition, previewSystem.sizeToUse);
                        break;
                    case ObjectType.WallDecor:
                        canPlace = wallDecorData.CanPlaceObjectAt(gridPosition, previewSystem.sizeToUse);
                        break;
                    case ObjectType.Furniture:
                        canPlace = furnitureData.CanPlaceObjectAt(gridPosition, previewSystem.sizeToUse);
                        break;
                }

                if (!canPlace)
                    break;
            }

            return canPlace;
        }
        return true;
    }

/// <summary>
    /// Handles the action of placing an object at a grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position where the object will be placed.</param>
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex],
                                             grid.CellToWorld(gridPosition),
                                             previewSystem.previewObject.GetComponent<RotatePlacementObject>().objectToRotate.transform.rotation);
        
        GridData selectedData = GetUsedGridData();
        selectedData.AddObjectAt(gridPosition, previewSystem.sizeToUse, database.objectsData[selectedObjectIndex].ID, index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    /// <summary>
    /// Gets the grid data corresponding to the currently selected object type.
    /// </summary>
    /// <returns>The grid data for the selected object type.</returns>
    private GridData GetUsedGridData()
    {
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
    /// Updates the state during placement, adjusting the preview position.
    /// </summary>
    /// <param name="gridPosition">The current grid position for the preview.</param>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
