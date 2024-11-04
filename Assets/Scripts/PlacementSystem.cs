using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] Grid grid; 
    [SerializeField] InputManager inputManager;
    [SerializeField] public GameObject player;
    [SerializeField] ObjectsDatabaseSO database;  
    [SerializeField] GameObject gridVisualization;
    private GridData floorData, wallData, wallDecorData, furnitureData;
    [SerializeField] PreviewSystem preview;
    Vector3Int lastDetectedPosition = Vector3Int.zero;
    [SerializeField] ObjectPlacer objectPlacer;
    IBuildingState buildingState;
    public Camera cam;
    public LayerMask placementMask;
    private bool placementOn = true;
    public GameObject placementCanvas;
    private void Start()
    {
        StopPlacement();
        floorData = new GridData();
        wallData = new GridData();  
        wallDecorData = new GridData();
        furnitureData = new GridData();
        inputManager.OnPlacementToggle += TogglePlacement;
        TogglePlacement();
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

    void TogglePlacement()
    {
        if (placementOn)
        {
            placementOn = false;
            placementCanvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StopPlacement();
        }
        else
        {
            placementOn = true;
            placementCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           this,
                                           database,
                                           floorData,
                                           wallData,
                                           wallDecorData,
                                           furnitureData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid,
                                          preview,
                           this,
                                          floorData,
                                          wallData,
                                          wallDecorData,
                                          furnitureData,
                                          objectPlacer);
        inputManager.OnClicked += PlaceStructure;
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
        gridVisualization.SetActive(false);
        
        if(buildingState == null) return;
        
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}
