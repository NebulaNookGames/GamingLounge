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
            if (gamepadIcon == null)
                imageToChange.enabled = false;
            else
            {
                imageToChange.enabled = true;
                imageToChange.sprite = gamepadIcon;
            }
#endif
            #if UNITY_SWITCH
            if (gamepadIcon == null && switchIcon == null)
            {
                imageToChange.enabled = false;
            }
            else if (switchIcon != null)
            {
                imageToChange.enabled = true;
                imageToChange.sprite = switchIcon;
            }
            else if (gamepadIcon != null)
            {
                imageToChange.enabled = true;
                imageToChange.sprite = gamepadIcon;
            }
            else
                imageToChange.enabled = false;
#endif
        }
        else
            imageToChange.sprite = keyboardIcon;
    }
}