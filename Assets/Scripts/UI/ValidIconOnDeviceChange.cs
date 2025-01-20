using System;
using UnityEngine;
using UnityEngine.UI; 

public class ValidIconOnDeviceChange : MonoBehaviour
{
    public Sprite keyboardIcon;
    public Sprite gamepadIcon;
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
            imageToChange.sprite = gamepadIcon;
        else
            imageToChange.sprite = keyboardIcon;
    }
}