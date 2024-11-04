using Unity.Cinemachine;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    PlacementSystem placementSystem;
    ObjectsDatabaseSO database;
    GridData floorData, wallData, wallDecorData, furnitureData;
    ObjectPlacer objectPlacer;

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

        if ( selectedObjectIndex > -1)
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

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Vector3 newPosition = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = placementSystem.cam.nearClipPlane;
        Ray ray = placementSystem.cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, placementSystem.placementMask))
        {
            newPosition = hit.point;
        }

        Debug.DrawLine(placementSystem.player.transform.position, newPosition, Color.green, 5f);
        if (Vector3.Distance(placementSystem.player.transform.position, newPosition) > 3)
        {
            return false; 
        }
        GridData selectedData = GetUsedGridData();

        return selectedData.canPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;
        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab,
                                       grid.CellToWorld(gridPosition),
                                              previewSystem.previewObject.GetComponent<RotatePlacementObject>().objectToRotate.transform.rotation);

        GridData selectedData = GetUsedGridData();

        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private GridData GetUsedGridData()
    {
        switch (database.objectsData[selectedObjectIndex].objectType)
        {
            case ObjectType.ground:
                return floorData;
            case ObjectType.wall:
                return wallData;
            case ObjectType.wallDecor:
                return wallDecorData;
            case ObjectType.furniture:
                return furnitureData;
            default:
                return floorData;
        }
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}