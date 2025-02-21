using UnityEngine;

public class ConstantUIUpMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    public Vector3 originalPosition;
    public int resetTime = 10;
    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        Invoke(nameof(Reset), resetTime);
    }

    public void Update()
    {
        transform.position += transform.up * (Time.deltaTime * movementSpeed);
    }

    private void OnDisable()
    {
        if(IsInvoking(nameof(Reset))) 
            CancelInvoke(nameof(Reset));
        Reset();
    }

    void Reset()
    {
        transform.localPosition = originalPosition;
        Invoke(nameof(Reset), resetTime);
    }
}