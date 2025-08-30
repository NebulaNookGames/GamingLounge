using UnityEngine;
using UnityEngine.UI; 

public class ActivateOnlyCorrectDeviceUsage : MonoBehaviour
{
    public GameInput.GameDevice gameDevice;
    public Image imageToEnable;
    
    public void OnEnable()
    {
        if (GameInput.Instance.activeGameDevice == gameDevice)
        {
            imageToEnable.enabled = true;
        }
        else
        {
            imageToEnable.enabled = false;
        }
    }
}
