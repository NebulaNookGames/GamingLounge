using UnityEngine;

/// <summary>
/// Manages the visual representation of object placement and removal in the game world.
/// Provides feedback on whether an object can be placed at a given position.
/// </summary>
public class PreviewSystem : MonoBehaviour
{
    [SerializeField] float previewYOffset = 0.06f; // Vertical offset for the preview object
    [SerializeField] GameObject cellIndicator; // Indicator showing the current grid cell
    public GameObject previewObject; // The object currently being previewed for placement
    [SerializeField] private InputManager inputManager; // Handles player input for actions like rotation
    [SerializeField] Material previewMaterialPrefabs; // Material used for the preview object
    private Material previewMaterialInstance; // Instance of the preview material for the preview object

    private SpriteRenderer cellIndicatorRenderer; // Renderer for the cell indicator
    public Vector2Int sizeToUse;
    private GameObject currentObject; 
    private void Start()
    {
        // Initialize the preview material and set the cell indicator to inactive
        previewMaterialInstance = new Material(previewMaterialPrefabs);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Starts showing the placement preview for a specified prefab and size.
    /// </summary>
    /// <param name="prefab">The GameObject to be previewed.</param>
    /// <param name="size">The size of the object being placed.</param>
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        sizeToUse = size;
        currentObject = prefab;
        previewObject = Instantiate(prefab); // Instantiate the preview object
        PreparePreview(previewObject); // Prepare the preview with the appropriate material
        PrepareCursor(size); // Adjust the cell indicator to match the size of the object
        cellIndicator.SetActive(true); // Show the cell indicator
        inputManager.OnRotate += RotatePreview; // Subscribe to rotation events
    }

    /// <summary>
    /// Rotates the currently previewed object.
    /// </summary>
    private void RotatePreview()
    {
        if (previewObject == null) return; // Check if the preview object exists
        
        if(sizeToUse.x == sizeToUse.y)
            previewObject.GetComponent<RotatePlacementObject>().RotateAround(); // Rotate the object
        else
        {
            int tempX = sizeToUse.x;
            int tempY = sizeToUse.y;
            sizeToUse.x = tempY;
            sizeToUse.y = tempX;
            PrepareCursor(sizeToUse); // Adjust the cell indicator to match the size of the object
            previewObject.GetComponent<RotatePlacementObject>().RotateOnce();
        }
    }

    /// <summary>
    /// Prepares the cursor (cell indicator) based on the size of the object being placed.
    /// </summary>
    /// <param name="size">The size of the object being placed.</param>
    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 && size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // Set scale
            cellIndicatorRenderer.material.mainTextureScale = size; // Set texture scale
        }
    }

    /// <summary>
    /// Prepares the preview object with the appropriate material.
    /// </summary>
    /// <param name="previewObject">The preview object to prepare.</param>
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>(); // Get all renderers in the preview object
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials; // Get the current materials
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance; // Replace with the preview material instance
            }
            renderer.materials = materials; // Assign updated materials
        }
    }

    /// <summary>
    /// Stops showing the placement preview and cleans up.
    /// </summary>
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false); // Hide the cell indicator
        inputManager.OnRotate -= RotatePreview; // Subscribe to rotation events

        if (previewObject != null)
            Destroy(previewObject); // Destroy the preview object if it exists
    }

    /// <summary>
    /// Updates the position of the preview object and the cursor based on validity.
    /// </summary>
    /// <param name="position">The new position for the preview.</param>
    /// <param name="validity">Indicates whether the placement is valid.</param>
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position); // Move the preview object
            ApplyFeedbackToPreview(validity); // Update feedback for the preview object
        }
        MoveCursor(position); // Move the cursor indicator
        ApplyFeedbackToCursor(validity); // Update feedback for the cursor
    }

    /// <summary>
    /// Applies visual feedback to the preview object based on placement validity.
    /// </summary>
    /// <param name="validity">Indicates whether the placement is valid.</param>
    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.green : Color.red; // Choose color based on validity
        c.a = 0.5f; // Set transparency
        previewMaterialInstance.color = c; // Apply color to the preview material
    }

    /// <summary>
    /// Applies visual feedback to the cursor indicator based on placement validity.
    /// </summary>
    /// <param name="validity">Indicates whether the placement is valid.</param>
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.green : Color.red; // Choose color based on validity
        c.a = 0.5f; // Set transparency
        cellIndicatorRenderer.color = c; 
    }

    /// <summary>
    /// Moves the cursor indicator to the specified position.
    /// </summary>
    /// <param name="position">The position to move the cursor to.</param>
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position; // Set the position of the cursor
    }

    /// <summary>
    /// Moves the preview object to the specified position with an offset.
    /// </summary>
    /// <param name="position">The position to move the preview to.</param>
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z); // Move with offset
    }

    /// <summary>
    /// Starts showing the removal preview for the selected object.
    /// </summary>
    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true); // Show the cell indicator
        PrepareCursor(Vector2Int.one); // Prepare the cursor for single cell removal
        ApplyFeedbackToCursor(false); // Set feedback to invalid
    }
    
    // void OnDrawGizmos()
    // {
    //     Debug.Log("Drawing Gizmos");
    //     
    //     // Ensure previewObject is not null
    //     if (previewObject == null)
    //         return;
    //
    //     // Get the MeshRenderer to access the bounds
    //     MeshRenderer meshRenderer = previewObject.GetComponentInChildren<MeshRenderer>();
    //     if (meshRenderer == null)
    //         return;
    //
    //     // Get the bounds of the mesh
    //     Bounds meshBounds = meshRenderer.bounds;
    //
    //     // Calculate the half-extents (size / 2)
    //     Vector3 halfExtents = meshBounds.size / 2f;
    //
    //     // Set the Gizmo color to make it visible
    //     Gizmos.color = Color.green;
    //
    //     // Draw the Gizmo box at the position of the mesh's center with the calculated half-extents
    //     Gizmos.DrawWireCube(meshBounds.center, meshBounds.size);
    //
    //     // Optionally, you can add a sphere at the center of the box to indicate the overlap position
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(meshBounds.center, 0.1f);
    // }
}