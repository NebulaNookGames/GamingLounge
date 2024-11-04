using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PlacementSystem placementSystem;
    PreviewSystem previewSystem;
    GridData floorData, wallData, wallDecorData, furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         PlacementSystem placementSystem,
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

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
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

        
        GridData selectedData = null;

        if (!floorData.canPlaceObjectAt(gridPosition, Vector2Int.one) && Vector3.Distance(placementSystem.player.transform.position, newPosition) < 3)
        {
            selectedData = floorData;
        }
        else if (!wallData.canPlaceObjectAt(gridPosition, Vector2Int.one) && Vector3.Distance(placementSystem.player.transform.position, newPosition) < 3)
        {
            selectedData = wallData;
        }
        else if(!wallDecorData.canPlaceObjectAt(gridPosition, Vector2Int.one) && Vector3.Distance(placementSystem.player.transform.position, newPosition) < 3)
        {
            selectedData = wallDecorData;
        }
        else if(!furnitureData.canPlaceObjectAt(gridPosition, Vector2Int.one) && Vector3.Distance(placementSystem.player.transform.position, newPosition) < 3)
        {
            selectedData = furnitureData;
        }
        if(selectedData == null)
        {
            // sound, nothing to remove
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

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(floorData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallDecorData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && floorData.canPlaceObjectAt(gridPosition, Vector2Int.one)
            && furnitureData.canPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}