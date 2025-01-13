using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public InputAction locomotionInputAction;

    public bool runPressed; 
    
    private void OnEnable()
    {
        locomotionInputAction.Enable();
    }

    private void OnDisable()
    {
        locomotionInputAction.Disable();
    }

    public float GetHorizontalAxis()
    {
        return locomotionInputAction.ReadValue<Vector2>().y;
    }
    
    public float GetVerticalAxis()
    {
        return locomotionInputAction.ReadValue<Vector2>().x;
    }
}
