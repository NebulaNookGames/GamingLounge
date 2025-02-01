using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the state for removing objects from the grid in the placement system.
/// </summary>
public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    private Grid grid;
    private PlacementSystem placementSystem;
    private PreviewSystem previewSystem;
    private GridData floorData, wallData, wallDecorData, furnitureData;
    private ObjectPlacer objectPlacer;
    private GameObject gameObjectToColor; 
    
    /// <summary>
    /// Initializes a new instance of the <see cref="RemovingState"/> class.
    /// </summary>
    /// <param name="grid">The grid system for placement.</param>
    /// <param name="previewSystem">The system for showing removal previews.</param>
    /// <param name="placementSystem">The main placement system.</param>
    /// <param name="floorData">Grid data for floor objects.</param>
    /// <param name="wallData">Grid data for wall objects.</param>
    /// <param name="wallDecorData">Grid data for wall decorations.</param>
    /// <param name="furnitureData">Grid data for furniture objects.</param>
    /// <param name="objectPlacer">Manages the placement and removal of objects.</param>
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
        
        if (GameInput.Instance.activeGameDevice == GameInput.GameDevice.Gamepad)
            placementSystem.virtualMouse.gameObject.SetActive(true);
    }

    /// <summary>
    /// Ends the current state, stopping the preview system.
    /// </summary>
    public void EndState()
    {
        if (gameObjectToColor != null)
        {
            previewSystem.ResetFeedbackToRemovalPreview(gameObjectToColor);
            gameObjectToColor = null;
        }
        
        placementSystem.virtualMouse.gameObject.SetActive(false);
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Handles the action of removing an object at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position where the removal action is performed.</param>
    public void OnAction(Vector3Int gridPosition)
    {

        GridData selectedData = GetGridDataForRemoval(gridPosition, GetMousePosition());
        
        if (selectedData == null)
            placementSystem.gameObject.GetComponent<AudioPlayer>().PlayAudioOneShot(placementSystem.invalidPlacementInteractionAudioclip, 1);
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
            {
                placementSystem.gameObject.GetComponent<AudioPlayer>().PlayAudioOneShot(placementSystem.invalidPlacementInteractionAudioclip, 1);
                return;
            }

            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
            placementSystem.gameObject.GetComponent<AudioPlayer>().PlayAudioOneShot(placementSystem.validPlacementInteractionAudioclip, 1);

        }

        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition), false);
    }

    /// <summary>
    /// Checks if a grid position has a valid object for removal.
    /// </summary>
    /// <param name="gridPosition">The grid position to check.</param>
    /// <returns>True if there is an object to remove; otherwise, false.</returns>
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallData.CanPlaceObjectAt(gridPosition, Vector2Int.one)
            && wallDecorData.CanPlaceObjectAt(gridPosition, Vector2Int.one)
            && furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one));
    }

    /// <summary>
    /// Updates the state by checking the validity of the current grid position for removal.
    /// </summary>
    /// <param name="gridPosition">The current grid position.</param>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        GridData selectedData = GetGridDataForRemoval(gridPosition, GetMousePosition());
        if (selectedData != null)
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex > -1)
            {
                GameObject currentGameObjectToColor = objectPlacer.placedGameObjects[gameObjectIndex];
                if (currentGameObjectToColor != null && gameObjectToColor == null)
                {
                    gameObjectToColor = currentGameObjectToColor;
                    previewSystem.ApplyFeedbackToRemovalPreview(gameObjectToColor);
                }
                else if (currentGameObjectToColor != null && gameObjectToColor != null &&
                         currentGameObjectToColor != gameObjectToColor)
                {
                    previewSystem.ResetFeedbackToRemovalPreview(gameObjectToColor);
                    gameObjectToColor = currentGameObjectToColor;
                    previewSystem.ApplyFeedbackToRemovalPreview(gameObjectToColor);
                }
            }
        }
        else if(gameObjectToColor != null)
        {
            previewSystem.ResetFeedbackToRemovalPreview(gameObjectToColor);
            gameObjectToColor = null;
        }

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity, false);
    }

    /// <summary>
    /// Determines which grid data to use for removal based on the grid position and distance.
    /// </summary>
    /// <param name="gridPosition">The grid position to check.</param>
    /// <param name="newPosition">The new position from the raycast hit.</param>
    /// <returns>The appropriate GridData for removal, or null if none is valid.</returns>
    private GridData GetGridDataForRemoval(Vector3Int gridPosition, Vector3 newPosition)
    {
        if (!wallDecorData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            return wallDecorData;
        if (!furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            return furnitureData;
        if (!wallData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            return wallData;
        if (!floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one))
            return floorData;
        
        return null;
    }

    Vector3 GetMousePosition()
    {
        Vector3 newPosition = Vector3.zero;
        Vector3 mousePos = Vector3.zero; 
        
        if(GameInput.Instance.activeGameDevice == GameInput.GameDevice.KeyboardMouse) 
            mousePos = Input.mousePosition;
        else if (GameInput.Instance.activeGameDevice == GameInput.GameDevice.Gamepad)
            mousePos = placementSystem.virtualCursorTransform.transform.position;
        
        mousePos.z = placementSystem.cam.nearClipPlane;
        Ray ray = placementSystem.cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, placementSystem.placementMask))
        {
            newPosition = hit.point;
            return newPosition;
        }
        Debug.DrawLine(placementSystem.player.transform.position, newPosition, Color.green, 5f);
        return newPosition;
    }
}