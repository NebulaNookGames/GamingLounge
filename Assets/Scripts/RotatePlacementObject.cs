using UnityEngine;

public class RotatePlacementObject : MonoBehaviour
{
    [SerializeField] public GameObject objectToRotate;
    public Vector3 rotationDegree;
    public void Rotate()
    {
        Debug.Log("Rotate");
        objectToRotate.transform.Rotate(rotationDegree);
    }
}
