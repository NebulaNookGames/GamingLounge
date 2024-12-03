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
    public void RotateAround()
    {
        if (!objectToRotate) return; 
        
        objectToRotate.transform.Rotate(rotationDegree); // Rotate the object by the specified degrees
    }

    public void RotateOnce()
    {
        if (!objectToRotate) return; 
        
        if(objectToRotate.transform.eulerAngles.y == 0)
            objectToRotate.transform.eulerAngles = new Vector3(0,-90,0);
        else 
            objectToRotate.transform.eulerAngles = new Vector3(0,0,0);
    }
}