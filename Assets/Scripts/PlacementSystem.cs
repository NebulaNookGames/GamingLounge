using System;
using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    [SerializeField] Grid grid; 
    [SerializeField] InputManager inputManager;
    [SerializeField] ObjectsDatabaseSO database;  
    int selectedObjectIndex = -1;
    [SerializeField] GameObject gridVisualization;
    private GridData floorData, wallData, wallDecorData, furnitureData;
    private List<GameObject> placedGameObjects = new();
    [SerializeField] PreviewSystem preview;
    Vector3Int lastDetectedPosition = Vector3Int.zero;


    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        wallData = new GridData();  
        wallDecorData = new GridData();
        furnitureData = new GridData();
    }

    private void Update()
    {
        if ((selectedObjectIndex < 0)) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (!placementValidity) return;

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placedGameObjects.Add(newObject);

        GridData selectedData = GetUsedGridData();
    
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placedGameObjects.Count - 1);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {

        GridData selectedData = GetUsedGridData();

        return selectedData.canPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        mouseIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero; 
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
}
