using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class allows the player to grab money from machines when the player is in range and presses a key.
/// The money is transferred from the machine to the player's money manager.
/// </summary>
public class MoneyGrabber : MonoBehaviour
{
    // Reference to the MoneyManager that handles the player's money.
    [SerializeField] private MoneyManager moneyManager;

    private bool canGrabMoney = false; 
    MoneyHolder moneyHolder;
    
    public InputActionProperty grabMoneyAction;

    public AudioSource audioS;
    public AudioClip grabClip;
    private void OnEnable()
    {
        grabMoneyAction.action.performed += GrabMoney;
    }

    /// <summary>
    /// Checks if the player is in range of a machine and presses the interaction key to grab money.
    /// </summary>
    /// <param name="other">The collider of the object the player is interacting with.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            if (other.transform.root.GetComponent<MoneyHolder>().moneyBeingHeld > 0)
            {
                canGrabMoney = true;
                moneyHolder = other.transform.root.GetComponent<MoneyHolder>();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            if (other.transform.root.GetComponent<MoneyHolder>() == moneyHolder)
            {
                canGrabMoney = false;
                moneyHolder = null; 
            }
        }
    }

    void GrabMoney(InputAction.CallbackContext context)
    {
        if (!canGrabMoney) return;
        
        moneyManager.ChangeMoney(moneyHolder.moneyBeingHeld);
        moneyHolder.ChangeMoney(-moneyHolder.moneyBeingHeld, false, false, false);
        moneyHolder = null;
        canGrabMoney = false;
        audioS.PlayOneShot(grabClip);
    }
}