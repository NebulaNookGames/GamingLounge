using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraControls : MonoBehaviour
{
    // ============================
    // Serialized Fields
    // ============================

    // Reference to the close-up camera for when the player zooms in
    [SerializeField] public CinemachineCamera closeCam;

    // Reference to the standard camera used for regular gameplay
    [SerializeField] public CinemachineCamera standardCam;

    // Reference to the far camera for zooming out
    [SerializeField] public CinemachineCamera farCam;

    // Reference to the map view camera for an overhead view
    [SerializeField] public CinemachineCamera mapViewCam;

    // The input action to detect scroll/zoom input
    [SerializeField] public InputActionProperty scrollAction;

    // ============================
    // Private Variables
    // ============================

    // Audio source for playing camera change sounds
    private AudioSource audioS;

    // Sound clips for camera transitions (high zoom and low zoom)
    [SerializeField] public AudioClip camHigh;
    [SerializeField] public AudioClip camLow;

    // Flag to control whether the camera can change or not
    private bool canChange = true;

    // The duration to wait before allowing the camera to change again
    [SerializeField] public float waitDuration = 1f;

    // The ambient effect that will be toggled when changing projection modes
    [SerializeField] public GameObject ambientEffect;

    // References to the UI panels (build and upgrade) to prevent camera change when active
    [SerializeField] public GameObject buildPanel;
    [SerializeField] public GameObject upgradePanel;

    // ============================
    // Unity Methods
    // ============================

    // Called when the script is initialized
    // Subscribes to the scroll input action to change the camera view
    private void Awake()
    {
        scrollAction.action.performed += ChangeActiveCam; // Subscribe to the scroll action
        audioS = GetComponent<AudioSource>(); // Get the AudioSource component attached to this object
    }

    // ============================
    // Custom Methods
    // ============================

    // Method that changes the active camera based on the scroll input
    // Responds to the input action performed for scrolling (zooming in or out)
    void ChangeActiveCam(InputAction.CallbackContext context)
    {
        // Prevent camera change if conditions aren't met (paused, gamepad input with UI open, etc.)
        if (!canChange || Time.timeScale == 0 || 
            (GameInput.Instance.activeGameDevice == GameInput.GameDevice.Gamepad && (buildPanel.activeSelf || upgradePanel.activeSelf)))
            return;
        
        // Invoke SetCanChange after the specified wait time to re-enable camera switching
        Invoke("SetCanChange", waitDuration);
        canChange = false; // Temporarily disable camera switching while transitioning

        // Get the scroll value from the input (y-axis)
        if (context.ReadValue<Vector2>().y == 0) return; // No change if the scroll is neutral

        if (context.ReadValue<Vector2>().y == 1) // Zooming in (scroll up)
        {
            // Skip if the close-up camera is already active
            if (closeCam.gameObject.activeSelf) return;

            // Transition from standard or far to close-up camera
            if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf && mapViewCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(true); // Enable the far camera
                mapViewCam.gameObject.SetActive(false); // Disable map view
                closeCam.gameObject.SetActive(false); // Disable close camera
                StartCoroutine(ChangeProjectionMode(false, 1f)); // Change camera projection mode
                audioS.PlayOneShot(camLow); // Play the "zoom out" sound effect
            }
            else if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf && !mapViewCam.gameObject.activeSelf)
            {
                // If no camera is active, enable the close camera
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(true);
                audioS.PlayOneShot(camLow);
            }
            else if (farCam.gameObject.activeSelf) // Transition from far camera to standard
            {
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camLow);
            }
            else
            {
                // Transition from standard to far camera
                mapViewCam.gameObject.SetActive(false);
                farCam.gameObject.SetActive(true);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camLow);
            }
        }
        else if (context.ReadValue<Vector2>().y == -1) // Zooming out (scroll down)
        {
            // Skip if the map view camera is already active
            if (mapViewCam.gameObject.activeSelf) return;

            // Transition from close to standard or far camera
            if (closeCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camHigh); // Play the "zoom in" sound effect
            }
            else if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf)
            {
                // Transition from standard to far camera
                closeCam.gameObject.SetActive(false);
                farCam.gameObject.SetActive(true);
                mapViewCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camHigh);
            }
            else if (farCam.gameObject.activeSelf)
            {
                // Transition from far camera to map view
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(true);
                mapViewCam.gameObject.GetComponent<CinemachineCamera>().Lens.OrthographicSize = 12; // Set map view zoom level
                StartCoroutine(ChangeProjectionMode(true, 0)); // Change to orthographic projection
                audioS.PlayOneShot(camHigh);
            }
        }
    }

    // Method to reset the canChange flag after a delay, allowing for another camera change
    void SetCanChange()
    {
        canChange = true;
    }

    // Coroutine to change the camera projection mode between perspective and orthographic
    // The ambient effect will toggle based on the projection mode
    IEnumerator ChangeProjectionMode(bool newValue, float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Wait for a specified time
        ambientEffect.SetActive(!newValue); // Toggle the ambient effect based on the projection mode
        GetComponent<Camera>().orthographic = newValue; // Set the camera's projection mode
    }
}
