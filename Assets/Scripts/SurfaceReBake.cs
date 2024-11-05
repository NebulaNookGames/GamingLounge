using Unity.AI.Navigation;
using UnityEngine;

public class SurfaceReBake : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface; 
    [SerializeField] private PlacementSystem placementSystem;

    public static SurfaceReBake instance; 
    
    private void Awake()
    {
        instance = this; 
        
        if (placementSystem == null) return;
        
        placementSystem.OnPlaced += Rebake;
    }

    public void Rebake()
    {
        navMeshSurface.BuildNavMesh();
    }
}