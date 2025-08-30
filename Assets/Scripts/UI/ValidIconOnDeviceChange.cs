    using System;
using UnityEngine;
using UnityEngine.UI; 

public class ValidIconOnDeviceChange : MonoBehaviour
{
    public Sprite keyboardIcon;
    public Sprite gamepadIcon;
    public Sprite switchIcon;
    Image imageToChange; 
    GameInput gameInput;

    private void Start()
    {
        if (GameInput.Instance != null)
        {
            gameInput = GameInput.Instance;
            gameInput.OnGameDeviceChanged += GameInput_DeviceChanged;
            imageToChange = GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        if (GameInput.Instance != null)
        {
            gameInput = GameInput.Instance;
            gameInput.OnGameDeviceChanged += GameInput_DeviceChanged;
            imageToChange = GetComponent<Image>();

            GameInput_DeviceChanged(gameInput.activeGameDevice);
        }
    }
    
    private void OnDisable()
    {
        gameInput.OnGameDeviceChanged -= GameInput_DeviceChanged;
    }

    void GameInput_DeviceChanged(GameInput.GameDevice gameDevice)
    {
        if (gameDevice == GameInput.GameDevice.Gamepad)
        {
            #if !UNITY_SWITCH
            imageToChange.sprite = gamepadIcon;
            #endif
            #if UNITY_SWITCH
            if(switchIcon != null) 
                imageToChange.sprite = switchIcon;
            else 
                imageToChange.sprite = gamepadIcon;
            #endif
        }
        else
            imageToChange.sprite = keyboardIcon;
    }
}