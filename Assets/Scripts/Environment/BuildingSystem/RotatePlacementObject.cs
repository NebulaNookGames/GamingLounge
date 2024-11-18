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
        Debug.Log("Rotate"); // Log the rotation action to the console
        objectToRotate.transform.Rotate(rotationDegree); // Rotate the object by the specified degrees
    }
}