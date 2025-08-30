using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI; 
public class SetActionsAssetOnEnable : MonoBehaviour
{
    public InputActionAsset uiActions;

    private InputSystemUIInputModule uiInputModule;

    void Awake()
    {
        uiInputModule = GetComponent<InputSystemUIInputModule>();
    }

    void OnEnable()
    {
        if(uiInputModule != null)
        {
            uiInputModule.actionsAsset = uiActions;
        }
    }
}

