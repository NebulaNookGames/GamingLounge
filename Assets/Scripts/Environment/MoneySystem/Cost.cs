using UnityEngine;
using UnityEngine.Serialization;

public class Cost : MonoBehaviour
{
    [SerializeField] private int minimumAmount = 5; // Minimum cost, when placed outside of buildings
    [SerializeField] private int maximumAmount = 20; // Maximum cost, when placed inside of buildings
    [SerializeField] private LayerMask layerMask; // Layer mask for walls
    [SerializeField] private Transform transformToCheckFrom; // Origin point for raycasts
    private Vector3[] directions =  // 16 Directions to cast rays
    {
        Vector3.left,                             // Left
        Vector3.right,                            // Right
        Vector3.forward,                          // Forward
        Vector3.back,                             // Backward
        (Vector3.forward + Vector3.left).normalized,  // Forward-left diagonal
        (Vector3.forward + Vector3.right).normalized, // Forward-right diagonal
        (Vector3.back + Vector3.left).normalized,     // Backward-left diagonal
        (Vector3.back + Vector3.right).normalized,    // Backward-right diagonal
        (Vector3.left + Vector3.forward * 0.35f).normalized,  // Midpoint: Forward-left and Left
        (Vector3.right + Vector3.forward * 0.35f).normalized, // Midpoint: Forward-right and Right
        (Vector3.left + Vector3.back * 0.35f).normalized,     // Midpoint: Backward-left and Left
        (Vector3.right + Vector3.back * 0.35f).normalized,    // Midpoint: Backward-right and Right
        (Vector3.forward + Vector3.left * 0.35f).normalized,  // Midpoint: Forward-left and Forward
        (Vector3.forward + Vector3.right * 0.35f).normalized, // Midpoint: Forward-right and Forward
        (Vector3.back + Vector3.left * 0.35f).normalized,     // Midpoint: Backward-left and Backward
        (Vector3.back + Vector3.right * 0.35f).normalized     // Midpoint: Backward-right and Backward
    };

    public int GetCost()
    {
        foreach (var direction in directions)
        {
            if (!IsWallInDirection(direction))
            {
                // If any raycast fails to hit a wall, return base cost
                return minimumAmount;
            }
        }

        // If all raycasts hit a wall, double the cost
        return maximumAmount;
    }

    private bool IsWallInDirection(Vector3 direction)
    {
        float rayDistance = 1000; // Adjust as per the scale of your objects/world

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(transformToCheckFrom.position, direction, out hit, rayDistance, layerMask))
        {
            // Check if the hit object is on the "Wall" layer
            return true;
        }

        // No wall detected in this direction
        return false;
    }

    // Optional: Visualize raycasts in the Scene view for debugging
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; 
        
        if (transformToCheckFrom == null) return;

        Gizmos.color = Color.red;
        foreach (var direction in directions)
        {
            Gizmos.DrawRay(transformToCheckFrom.position, direction * 1000); // Adjust ray distance
        }
    }
}
