using UnityEngine;

/// <summary>
/// Manages the preview system for object placement, including preview position, size, and color feedback for placement validity.
/// </summary>
public class PreviewSystem : MonoBehaviour
{
    /// <summary> Vertical offset applied to the preview object to prevent z-fighting with the ground. </summary>
    [SerializeField] float previewYOffset = 0.06f;

    /// <summary> Visual indicator representing the placement cell. </summary>
    [SerializeField] GameObject cellIndicator;

    /// <summary> Instance of the preview object for the currently selected placement object. </summary>
    GameObject previewObject;

    /// <summary> Material prefab used for preview objects, providing a translucent effect. </summary>
    [SerializeField] Material previewMaterialPrefabs;

    /// <summary> Instance of the preview material, adjusted per preview session for visual feedback. </summary>
    Material previewMaterialInstance;

    /// <summary> Renderer for the cell indicator, used for feedback coloring. </summary>
    Renderer cellIndicatorRenderer;

    /// <summary>
    /// Initializes the preview system, including setting up the preview material and disabling the cell indicator.
    /// </summary>
    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefabs);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    /// <summary>
    /// Begins showing a placement preview for a specified object and size.
    /// </summary>
    /// <param name="prefab">The object prefab to preview.</param>
    /// <param name="size">Grid size of the object in x and y dimensions.</param>
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    /// <summary>
    /// Configures the cell indicator's scale and texture based on the object's size.
    /// </summary>
    /// <param name="size">The size of the object to be placed, used to adjust the cell indicator.</param>
    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 && size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size; 
        }
    }

    /// <summary>
    /// Applies the preview material to the preview object's renderers for consistent appearance.
    /// </summary>
    /// <param name="previewObject">The instantiated preview object.</param>
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance; 
            }
            renderer.materials = materials; 
        }
    }

    /// <summary>
    /// Stops showing the current preview by deactivating the cell indicator and destroying the preview object.
    /// </summary>
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);

        if(previewObject != null)
            Destroy(previewObject);
    }

    /// <summary>
    /// Updates the preview object's position and applies feedback based on placement validity.
    /// </summary>
    /// <param name="position">The new position for the preview object and cell indicator.</param>
    /// <param name="validity">Indicates if the placement is valid, adjusting the preview color accordingly.</param>
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if(previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    /// <summary>
    /// Changes the preview object's color based on placement validity.
    /// </summary>
    /// <param name="validity">If true, preview is white; if false, preview is red.</param>
    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c; 
    }

    /// <summary>
    /// Changes the cell indicator's color based on placement validity.
    /// </summary>
    /// <param name="validity">If true, indicator is white; if false, indicator is red.</param>
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    /// <summary>
    /// Moves the cell indicator to the specified position.
    /// </summary>
    /// <param name="position">New position for the cell indicator.</param>
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    /// <summary>
    /// Moves the preview object to the specified position, applying a vertical offset.
    /// </summary>
    /// <param name="position">New position for the preview object.</param>
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    /// <summary>
    /// Activates the cell indicator for removal preview mode and sets feedback color to red.
    /// </summary>
    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}