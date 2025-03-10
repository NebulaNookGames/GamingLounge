using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public Vector3 rotation;
    public float rotationSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation, rotationSpeed * Time.deltaTime);
    }
}
