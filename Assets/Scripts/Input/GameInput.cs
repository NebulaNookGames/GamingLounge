using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    #region Singleton

    // Singleton instance of the GameInput class
    public static GameInput Instance { get; private set; }

    #endregion

    #region Enums

    // Enum to define the types of game devices: Keyboard/Mouse or Gamepad
    public enum GameDevice { KeyboardMouse, Gamepad}

    #endregion

    #region Serialized Fields

    // The currently active game device (either Keyboard/Mouse or Gamepad)
    public GameDevice activeGameDevice;

    #endregion

    #region Events

    // Event to notify when the active game device changes
    public Action<GameDevice> OnGameDeviceChanged;

    #endregion

    #region Unity Methods

    // Called when the script is first initialized
    private void Awake()
    {
        // Ensure only one instance of GameInput exists (Singleton Pattern)
        if (Instance != null)
        {
            Destroy(gameObject); // Destroy the new instance if one already exists
            return;
        }

        Instance = this; // Set this instance as the singleton instance
        transform.parent = null; // Detach from parent object to prevent accidental hierarchy changes
        
        // Subscribe to the action change event from the InputSystem
        InputSystem.onActionChange += InputSystem_OnActionChange;

        // Make this object persist across scenes
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Input Handling

    // Called when the input action changes (used to detect device changes)
    private void InputSystem_OnActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = arg1 as InputAction;

            // Check if the device is a Gamepad
            if (inputAction.activeControl.device is Gamepad)
            {
                if (inputAction.activeControl.magnitude < .2f) return; // Ignore small inputs
                
                // If the active device is not Gamepad, change it
                if (activeGameDevice != GameDevice.Gamepad)
                    ChangeActiveGameDevice(GameDevice.Gamepad);
            }
            // Check if the device is a Keyboard
            else if (inputAction.activeControl.device is Keyboard)
            {
                // If the active device is not Keyboard/Mouse, change it
                if (activeGameDevice != GameDevice.KeyboardMouse)
                    ChangeActiveGameDevice(GameDevice.KeyboardMouse);
            }
        }
    }

    #endregion

    #region Device Switching

    // Changes the active game device and triggers the appropriate event
    private void ChangeActiveGameDevice(GameDevice activeGameDevice)
    {
        this.activeGameDevice = activeGameDevice;
        OnGameDeviceChanged?.Invoke(this.activeGameDevice); // Invoke event for device change

        // Set mouse visibility when switching to Gamepad
        if (activeGameDevice == GameDevice.Gamepad)
            SetMouseVisibility(false);
    }

    #endregion

    #region Mouse Control

    // Controls the visibility and lock state of the mouse cursor based on the active device
    public void SetMouseVisibility(bool value)
    {
        // Show and unlock the cursor if the device is Keyboard/Mouse
        if (value && activeGameDevice != GameDevice.Gamepad)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        // Hide and lock the cursor if the device is Gamepad
        else if (!value && activeGameDevice != GameDevice.Gamepad)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        // Always hide and lock the cursor when the gamepad is active
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    #endregion
}
