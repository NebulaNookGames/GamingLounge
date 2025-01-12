using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    public enum GameDevice { KeyboardMouse, Gamepad,}

    public GameDevice activeGameDevice;

    public Action<GameDevice> OnGameDeviceChanged; 
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InputSystem.onActionChange += InputSystem_OnActionChange;
        DontDestroyOnLoad(gameObject);
    }

    private void InputSystem_OnActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = arg1 as InputAction;
            
            if (inputAction.activeControl.device is Gamepad)
            {
                if (activeGameDevice != GameDevice.Gamepad)
                    ChangeActiveGameDevice(GameDevice.Gamepad);
            }
            else if(inputAction.activeControl.device is Keyboard)
            {
                if(activeGameDevice != GameDevice.KeyboardMouse)
                    ChangeActiveGameDevice(GameDevice.KeyboardMouse);
            }
        }
    }

    private void ChangeActiveGameDevice(GameDevice activeGameDevice)
    {
        this.activeGameDevice = activeGameDevice;
        OnGameDeviceChanged?.Invoke(this.activeGameDevice);
    }
}