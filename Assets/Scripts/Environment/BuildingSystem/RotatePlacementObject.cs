using UnityEngine;

/// <summary>
/// Handles the rotation of a specified GameObject in the game world.
/// </summary>
public class RotatePlacementObject : MonoBehaviour
{
    [SerializeField] public GameObject objectToRotate; // The GameObject that will be rotated
    public Vector3 rotationDegree; // The degrees by which the object will be rotated

    /// <summary>
    /// Rotates the specified GameObject by the defined rotation degrees.
    /// </summary>
    public void Rotate()
    {
        if (!objectToRotate) return; 
        
        objectToRotate.transform.Rotate(rotationDegree); // Rotate the object by the specified degrees
    }
}