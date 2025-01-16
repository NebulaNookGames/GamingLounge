using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public CinemachineCamera closeCam;
    public CinemachineCamera standardCam;
    public CinemachineCamera farCam;

    public InputActionProperty scrollAction;
    private AudioSource audioS;
    public AudioClip camHigh;
    public AudioClip camLow;
    private bool canChange = true;
    public float waitDuration = 1f; 
    private void Awake()
    {
        scrollAction.action.performed += ChangeActiveCam; 
        audioS = GetComponent<AudioSource>();
    }

    void ChangeActiveCam(InputAction.CallbackContext context)
    {
        if (!canChange) return;

        Invoke("SetCanChange", waitDuration);
        canChange = false; 
        if (context.ReadValue<Vector2>().y == 0) return; 
        
        Debug.Log(context.ReadValue<Vector2>().y);
        
        if (context.ReadValue<Vector2>().y == 1)
        {
            if (closeCam.gameObject.activeSelf) return;
            
            
            if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(true);
                audioS.PlayOneShot(camLow);
            }
            else
            {
                farCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camLow);
            }
        }
        else  if (context.ReadValue<Vector2>().y == -1)
        {
            if (farCam.gameObject.activeSelf) return;
            
            if (closeCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camHigh);
            }
            else
            {
                closeCam.gameObject.SetActive(false);
                farCam.gameObject.SetActive(true);
                audioS.PlayOneShot(camHigh);
            }
        }
    }

    void SetCanChange()
    {
        canChange = true;
    }
}
