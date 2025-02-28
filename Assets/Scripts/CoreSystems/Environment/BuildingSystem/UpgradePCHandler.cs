using UnityEngine;
using UnityEngine.InputSystem; 

public class UpgradePCHandler : MonoBehaviour
{
    private bool isActive = false;
    public GameObject inDistanceProof;
    GameObject UpgradePCCanvas;
    public GameObject activationCheckObject;
    public float activationDistance = .7f;
    public AudioSource audioS;
    public AudioClip onAudioclip;
    public AudioClip offAudioclip;

    public InputActionProperty worldObjectUsageAction;
    
    private void OnEnable()
    {
        UpgradePCCanvas = GameObject.FindWithTag("UpgradeCanvas");
        worldObjectUsageAction.action.performed += UseObject;
    }

    private void OnDisable()
    {
        worldObjectUsageAction.action.performed -= UseObject;
    }

    void UseObject(InputAction.CallbackContext context)
    {
        if(Vector3.Distance(activationCheckObject.transform.position, GameObject.FindWithTag("Player").transform.position) < activationDistance && !GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().IsPaused)
            ChangeActiveState();
    }

    public void ChangeActiveState()
    {
        if (isActive)
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = false;
            Cursor.lockState = CursorLockMode.Locked;
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(false);
            audioS.PlayOneShot(offAudioclip);
            InputReader.instance.enabled = true;
            InputManager.instance.enabled = true;
        }
        else
        {
            GameObject.FindWithTag("MenuUI").GetComponent<PauseHandler>().pausedByOtherSystem = true;
            Cursor.lockState = CursorLockMode.None;
            UpgradePCCanvas.GetComponent<UpgradeCanvasHandler>().SetUpgradePanelState(true);
            audioS.PlayOneShot(onAudioclip);
            InputReader.instance.enabled = false;
            InputManager.instance.enabled = false;
        }
        isActive = !isActive;
    }
}
