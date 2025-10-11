using UnityEngine;

public class MeshBaker : MonoBehaviour
{
    public GameObject skinnedMeshObject;
    public GameObject staticMeshObject;

    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    private Animator animator;
    private Camera mainCamera;

    private bool usingSkinnedMesh = true;

    private void Awake()
    {
        // skinnedMeshObject.SetActive(true);
        // staticMeshObject.SetActive(false);
        //
        // animator = GetComponent<Animator>();
        // mainCamera = Camera.main;
        //
        // usingSkinnedMesh = true;
    }

    // private void Update()
    // {
    //     if (usingSkinnedMesh && skinnedMeshObject.activeSelf && skinnedMeshRenderer != null)
    //     {
    //         bool isVisible = IsVisibleToCamera(skinnedMeshRenderer);
    //
    //         skinnedMeshRenderer.enabled = isVisible;
    //         animator.enabled = isVisible;
    //     }
    // }

    public void BakeMesh()
    {
        // Mesh bakedMesh = new Mesh();
        // skinnedMeshRenderer.BakeMesh(bakedMesh);
        //
        // meshFilter.mesh = bakedMesh;
        //
        // skinnedMeshObject.SetActive(false);  // Hide skinned mesh
        // staticMeshObject.SetActive(true);    // Show baked mesh
        //
        // usingSkinnedMesh = false;
    }

    public void UnbakeMesh()
    {
        // staticMeshObject.SetActive(false);   // Hide baked mesh
        // skinnedMeshObject.SetActive(true);   // Show skinned mesh
        // animator.enabled = true;
        // skinnedMeshRenderer.enabled = true;
        //
        // usingSkinnedMesh = true;
    }

    private bool IsVisibleToCamera(Renderer rend)
    {
        if (!mainCamera) return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, rend.bounds);
    }
}