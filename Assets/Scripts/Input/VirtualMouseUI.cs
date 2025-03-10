using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    #region Serialized Fields

    // RectTransform component to scale and position the virtual mouse UI element
    [SerializeField] private RectTransform rectTransform;

    #endregion

    #region Private Variables

    // The VirtualMouseInput component that controls the virtual mouse
    private VirtualMouseInput virtualMouseInput;

    #endregion

    #region Unity Methods

    // Called when the script is initialized
    private void Awake()
    {
        // Get the VirtualMouseInput component
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    // Called every frame for updates
    private void Update()
    {
        // Adjust the scale of the virtual mouse UI element based on the RectTransform's scale
        transform.localScale = Vector3.one * 1f / rectTransform.localScale.x;
        
        // Bring the virtual mouse UI to the front
        transform.SetAsLastSibling();
    }

    // Called after all Update methods, allowing to apply the virtual mouse's position
    private void LateUpdate()
    {
        // Get the current virtual mouse position
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;

        // Clamp the position to stay within the screen bounds
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);

        // Apply the new virtual mouse position
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }

    #endregion
}