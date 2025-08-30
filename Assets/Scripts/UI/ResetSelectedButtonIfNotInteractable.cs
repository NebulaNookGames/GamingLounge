using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResetSelectedButtonIfNotInteractable : MonoBehaviour
{
    public Button buttonToResetTo;
    public EventSystem eventSystem;

    public void CheckIfShouldReset(Button buttonToCheckIfInteractable)
    {
        if(eventSystem == null && FindObjectOfType<EventSystem>())
            eventSystem = FindObjectOfType<EventSystem>();
        
        
        if (!buttonToCheckIfInteractable.interactable)
        {
            eventSystem.SetSelectedGameObject(buttonToResetTo.gameObject);
        }
    }
}