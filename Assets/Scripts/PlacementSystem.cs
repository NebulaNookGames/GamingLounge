using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] Grid grid; 
    [SerializeField] InputManager inputManager;
    [SerializeField] ObjectsDatabaseSO database;  
    [SerializeField] GameObject gridVisualization;
    private GridData floorData, wallData, wallDecorData, furnitureData;
    [SerializeField] PreviewSystem preview;
    Vector3Int lastDetectedPosition = Vector3Int.zero;
    [SerializeField] ObjectPlacer objectPlacer;
    IBuildingState buildingState; 

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
        if (buildingState == null) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if (lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           wallData,
                                           wallDecorData,
                                           furnitureData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid,
                                          preview,
                                          floorData,
                                          wallData,
                                          wallDecorData,
                                          furnitureData,
                                          objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition);
    }

    private void StopPlacement()
    {
        if(buildingState == null) return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}
