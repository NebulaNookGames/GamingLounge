using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraControls : MonoBehaviour
{
    public CinemachineCamera closeCam;
    public CinemachineCamera standardCam;
    public CinemachineCamera farCam;
    public CinemachineCamera mapViewCam; 
    
    public InputActionProperty scrollAction;
    private AudioSource audioS;
    public AudioClip camHigh;
    public AudioClip camLow;
    public bool canChange = true;
    public float waitDuration = 1f;
    public GameObject ambientEffect;
    public GameObject upgradePanel;

    private void Awake()
    {
        scrollAction.action.performed += ChangeActiveCam; 
        audioS = GetComponent<AudioSource>();
    }

    void ChangeActiveCam(InputAction.CallbackContext context)
    {
        if (!canChange || Time.timeScale == 0 || upgradePanel.activeSelf) return;

        Invoke("SetCanChange", waitDuration);
        canChange = false; 
        if (context.ReadValue<Vector2>().y == 0) return; 
        
        if (context.ReadValue<Vector2>().y == 1)
        {
            if (closeCam.gameObject.activeSelf) return;
            
            // From Standard to Close
            if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf && mapViewCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(true);
                mapViewCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                StartCoroutine(ChangeProjectionMode(false, 1f));
                audioS.PlayOneShot(camLow);
            }
            else if (!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf && !mapViewCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(true);
                audioS.PlayOneShot(camLow);
            }
            // To Standard
            else if(farCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camLow);
            }
            else
            {
                mapViewCam.gameObject.SetActive(false);
                farCam.gameObject.SetActive(true);
                closeCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camLow);
            }
        }
        else  if (context.ReadValue<Vector2>().y == -1)
        {
            if (mapViewCam.gameObject.activeSelf) return;
            
            // To Standard from Close
            if (closeCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                closeCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camHigh);
            }
            // To far from Standard
            else if(!closeCam.gameObject.activeSelf && !farCam.gameObject.activeSelf)
            {
                closeCam.gameObject.SetActive(false);
                farCam.gameObject.SetActive(true);
                mapViewCam.gameObject.SetActive(false);
                audioS.PlayOneShot(camHigh);
            }
            else if (farCam.gameObject.activeSelf)
            {
                farCam.gameObject.SetActive(false);
                mapViewCam.gameObject.SetActive(true);
                StartCoroutine(ChangeProjectionMode(true, 0));
                audioS.PlayOneShot(camHigh);
            }
            
        }
    }

    void SetCanChange()
    {
        canChange = true;
    }

    IEnumerator ChangeProjectionMode(bool newValue, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ambientEffect.SetActive(!newValue);
        GetComponent<Camera>().orthographic = newValue;
    }
}
