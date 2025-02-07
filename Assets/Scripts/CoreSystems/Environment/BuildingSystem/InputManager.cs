using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; 

/// <summary>
/// Manages player input for placing objects, toggling placement mode, and rotating objects.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    
    [SerializeField] private Camera sceneCamera; // The camera used to determine the position of objects in the scene

    public event Action OnClicked; // Event triggered when the player clicks
    public event Action OnPlacementToggle; // Event triggered when placement mode is toggled
    public event Action OnRotate; // Event triggered when the player rotates an object

    public RectTransform virtualCursorTransform;

    public InputActionProperty toggleBuildMenuAction;
    public InputActionProperty validateSelectionAction;
    public InputActionProperty rotateAction;

    private bool toggleOn;
    private bool validateOn;
    private bool rotateOn;

    public bool placementInputUnlocked = false; 
    
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        toggleBuildMenuAction.action.performed += PlacementToggle;
        validateSelectionAction.action.performed += Clicked;
        rotateAction.action.performed += Rotate;
    }

    private void OnDisable()
    {
        toggleBuildMenuAction.action.performed -= PlacementToggle;
        validateSelectionAction.action.performed -= Clicked;
        rotateAction.action.performed -= Rotate; 
    }

    private bool toggleHandled;
    private bool validateHandled;
    private bool rotateHandled;

    void PlacementToggle(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || toggleHandled || !placementInputUnlocked)
            return;

        toggleHandled = true;
        OnPlacementToggle?.Invoke();
    }

    void Clicked(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || validateHandled || context.canceled || context.started) 
            return;

        validateHandled = true;
        OnClicked?.Invoke();
    }

    void Rotate(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || rotateHandled)
            return;

        rotateHandled = true;
        OnRotate?.Invoke();
    }

    private void Update()
    {
        // Reset the flags when the actions are released
        if (!toggleBuildMenuAction.action.IsPressed())
            toggleHandled = false;

        if (!validateSelectionAction.action.IsPressed())
        {
            validateHandled = false;
        }

        if (!rotateAction.action.IsPressed())
            rotateHandled = false;
    }

    /// <summary>
    /// Checks if the pointer is currently over a UI element.
    /// </summary>
    /// <returns>True if the pointer is over a UI element; otherwise, false.</returns>
    public bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(); // Return true if the pointer is over a UI object
    }

    private Vector3 lastPosition; // Last known position hit by the raycast

    [SerializeField] private LayerMask placementLayermask; // LayerMask to determine what can be placed on

    /// <summary>
    /// Gets the position in the world where the player has clicked on the map.
    /// </summary>
    /// <returns>The world position of the selected map point.</returns>
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Vector3.zero; 
        if(GameInput.Instance.activeGameDevice == GameInput.GameDevice.KeyboardMouse)
            mousePos = Input.mousePosition; // Get the current mouse position
        else 
            mousePos = virtualCursorTransform.position;
        
        mousePos.z = sceneCamera.nearClipPlane; // Set z to the near clip plane of the camera
        Ray ray = sceneCamera.ScreenPointToRay(mousePos); // Create a ray from the camera to the mouse position
        RaycastHit hit; // Variable to store raycast hit information

        // Perform a raycast to detect objects in the scene
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point; // Update last known position with the hit point
        }
        return lastPosition; // Return the last position hit
    }
}