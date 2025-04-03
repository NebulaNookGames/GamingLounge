using System; 
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class manages the rebaking of the NavMesh when a placement action occurs in the game.
/// It listens for placement events and triggers the NavMesh rebaking process to update the navigation mesh.
/// </summary>
public class SurfaceReBake : MonoBehaviour
{
    // The NavMeshSurface component used to rebake the navigation mesh.
    [SerializeField] private NavMeshSurface navMeshSurface;

    // The PlacementSystem component that handles placement actions in the game.
    [SerializeField] private PlacementSystem placementSystem;

    // A static instance of the SurfaceReBake class, useful for accessing the class methods globally.
    public static SurfaceReBake instance;

    public event Action OnRebake; 
    
    /// <summary>
    /// Called when the script is initialized. Sets up the instance and subscribes to the placement system's event.
    /// </summary>
    private void Awake()
    {
        // Set the static instance to the current object.
        instance = this;

        // If the placement system is not assigned, exit the method.
        if (placementSystem == null) return;
        // Subscribe to the OnPlaced event to trigger the Rebake method when placement happens.
    }

    /// <summary>
    /// Rebuilds the NavMesh using the NavMeshSurface component.
    /// This is triggered when a placement occurs.
    /// </summary>
    public void Rebake()
    {
        Debug.Log("Rebake");
        // Rebuild the NavMesh to account for any changes in the environment.
        navMeshSurface.BuildNavMesh();
        OnRebake?.Invoke();
    }
}