using System;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData floorData, wallData, wallDecorData, furnitureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid,
                         PreviewSystem previewSystem,
                         GridData floorData,
                         GridData wallData,
                         GridData wallDecorData,
                         GridData furnitureData,
                         ObjectPlacer objectPlacer)
    {
        this.grid = grid;
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
        GridData selectedData = null;

        if (!floorData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = floorData;
        }
        else if (!wallData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = wallData;
        }
        else if(!wallDecorData.canPlaceObjectAt(gridPosition, Vector2Int.one))
        {
            selectedData = wallDecorData;
        }
        else if(!furnitureData.canPlaceObjectAt(gridPosition, Vector2Int.one))
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