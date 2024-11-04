using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] float previewYOffset = 0.06f;
    [SerializeField] GameObject cellIndicator;
    public GameObject previewObject;
    [SerializeField] private InputManager inputManager;
    [SerializeField] Material previewMaterialPrefabs;
    Material previewMaterialInstance;

    Renderer cellIndicatorRenderer;
    
    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefabs);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
        inputManager.OnRotate += RotatePreview;
    }

    private void RotatePreview()
    {
        if (previewObject == null) return;
        previewObject.GetComponent<RotatePlacementObject>().Rotate();
    }
    
    private void PrepareCursor(Vector2Int size)
    {
        if(size.x > 0 && size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size; 
        }
    }

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

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);

        if(previewObject != null)
            Destroy(previewObject);
    }

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

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c; 
    }

    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}