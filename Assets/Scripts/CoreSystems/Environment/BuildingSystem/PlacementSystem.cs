
using System;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; 

/// <summary>
/// Manages the placement and removal of objects in the game world.
/// </summary>
public class PlacementSystem : MonoBehaviour
{
    
    public static PlacementSystem Instance;
    
    [SerializeField]
    public Grid grid; // The grid used for object placement.

    [SerializeField]
    private InputManager inputManager; // Handles user input for placement actions.

    [SerializeField]
    public GameObject player; // The player object in the scene.

    [SerializeField]
    private ObjectsDatabaseSO database; // Database containing the objects available for placement.

    [SerializeField]
    public List<GameObject> unlockedGridParts; // Visual representation of the grids
    
    [SerializeField]
    public List<GameObject> lockedGridVisualizations; // Visual representation of the locked grids
    
    public GridData floorData, wallData, wallDecorData, furnitureData; // Grid data for various object types.

    [SerializeField]
    private PreviewSystem preview; // System for showing placement previews.

    private Vector3Int lastDetectedPosition = Vector3Int.zero; // Last detected grid position.

    [SerializeField]
    private ObjectPlacer objectPlacer; // Handles the actual placement of objects.

    private IBuildingState buildingState; // Current state of the building system.

    public Camera cam; // Camera used for placement raycasting.

    public LayerMask placementMask; // Layer mask used for placement detection.

    public bool placementOn = true; // Indicates whether placement mode is active.

    public GameObject placementCanvas; // UI canvas for placement mode.

    public AudioClip placementMenuOn;
    public AudioClip placementMenuOff;
   
    public Action OnPlaced;

    public GameObject virtualMouse;
    public RectTransform virtualCursorTransform;

    public AudioClip validPlacementInteractionAudioclip;
    public AudioClip invalidPlacementInteractionAudioclip;
    public AudioClip validRemoveInteractionAudioclip; 

    public TextMeshProUGUI moneyText;
    public GameObject moneyTextBackground;

    private bool firstTimeToggle = true;
    
    private void Awake()
    {
        Instance = this; 
        floorData = new GridData();
        wallData = new GridData();
        wallDecorData = new GridData();
        furnitureData = new GridData();
    }

    /// <summary>
    /// Initializes the placement system and sets up the grid data.
    /// </summary>
    private void Start()
    {
        StopPlacement();
      
        inputManager.OnPlacementToggle += TogglePlacement;
        TogglePlacement();
        
        // foreach (GameObject lockedGridVisualization in lockedGridVisualizations) 
        //     lockedGridVisualization.SetActive(true);
        //
        // foreach (GameObject gridPart in unlockedGridParts)
        // {
        //     gridPart.SetActive(true);
        //     gridPart.GetComponent<GameObjectStorage>().storedGameObject.SetActive(false);
        // }
    }

    public void ShowDetails()
    {
        Debug.Log("FloorDatas: " + floorData.placedObjects.Count);
        Debug.Log("WallDatas: " + wallData.placedObjects.Count);
        Debug.Log("WalLDecorDatas: " + wallDecorData.placedObjects.Count);
        Debug.Log("FurnitureDatas: " + furnitureData.placedObjects.Count);
        foreach (Vector3Int temp in wallData.placedObjects.Keys)
        {
            Debug.Log(temp);
        }
    }

    /// <summary>
    /// Updates the placement system each frame, checking for changes in grid position.
    /// </summary>
    private void Update()
    {
        if (buildingState == null)
            return;
        
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.UpdateState(gridPosition);
        lastDetectedPosition = gridPosition;
    }

    /// <summary>
    /// Toggles the placement mode on and off.
    /// </summary>
    private void TogglePlacement()
    {
        if (placementOn)
        {
            placementOn = false;
            if (firstTimeToggle)
                firstTimeToggle = false;
            else 
                GetComponent<AudioPlayer>().PlayAudioOneShot(placementMenuOff, .5f);
            
            moneyText.color = new Color(0, 0, 0, 0);
            if(moneyText != null)
                moneyTextBackground.SetActive(false);
            if(placementCanvas)
                placementCanvas.SetActive(false);
            if(virtualMouse)
                virtualMouse.SetActive(false);
            
            if(GameInput.Instance)
                GameInput.Instance.SetMouseVisibility(false);

            StopPlacement();
        }
        else
        {
            placementOn = true;
            if (firstTimeToggle)
                firstTimeToggle = false;
            else 
                GetComponent<AudioPlayer>().PlayAudioOneShot(placementMenuOn, .5f);
            
            moneyText.color = Color.white;
            moneyTextBackground.SetActive(true);
            
            GameInput.Instance.SetMouseVisibility(true);
            
            placementCanvas.SetActive(true);
        }
    }

    /// <summary>
    /// Starts the placement mode for placing a specific object.
    /// </summary>
    /// <param name="ID">The ID of the object to place.</param>
    public void StartPlacement(int ID)
    {
        StopPlacement();
        
        foreach (GameObject lockedGridVisualization in lockedGridVisualizations) 
            lockedGridVisualization.SetActive(true);
        
        foreach (GameObject gridPart in unlockedGridParts)
        {
            gridPart.SetActive(true);
            gridPart.GetComponent<GameObjectStorage>().storedGameObject.SetActive(false);
        }
        
        buildingState = new PlacementState(ID, grid, preview, this, database, floorData, wallData, wallDecorData,furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
    }
    
    
    /// <summary>
    /// Starts the removal mode for removing objects.
    /// </summary>
    public void StartRemoving()
    {
        StopPlacement();
        
        foreach (GameObject lockedGridVisualization in lockedGridVisualizations)
            lockedGridVisualization.SetActive(true);
        
        foreach (GameObject gridPart in unlockedGridParts)
        {
            gridPart.SetActive(true);
            gridPart.GetComponent<GameObjectStorage>().storedGameObject.SetActive(false);
        }
        
        buildingState = new RemovingState(grid, preview, this, floorData, wallData, wallDecorData, furnitureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
    }

    /// <summary>
    /// Places the structure at the current grid position if conditions are met.
    /// </summary>
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition);
      
    }

    /// <summary>
    /// Stops the current placement or removal mode.
    /// </summary>
    public void StopPlacement()
    {
        foreach (GameObject gridPart in unlockedGridParts)
        {
            if(gridPart)
                gridPart.SetActive(false);
        }

        foreach (GameObject lockedGridVisualization in lockedGridVisualizations)
        {
            if (lockedGridVisualization != null)
                lockedGridVisualization.SetActive(false);
        }

        if (buildingState == null) return;

        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
}