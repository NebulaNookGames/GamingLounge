using System;
using UnityEngine;

/// <summary>
/// This class allows the player to grab money from machines when the player is in range and presses a key.
/// The money is transferred from the machine to the player's money manager.
/// </summary>
public class MoneyGrabber : MonoBehaviour
{
    // Reference to the MoneyManager that handles the player's money.
    [SerializeField] private MoneyManager moneyManager;

    /// <summary>
    /// Checks if the player is in range of a machine and presses the interaction key to grab money.
    /// </summary>
    /// <param name="other">The collider of the object the player is interacting with.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Machine"))
        {
            if (other.GetComponent<MoneyHolder>().moneyBeingHeld > 0)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    moneyManager.ChangeMoney(other.GetComponent<MoneyHolder>().moneyBeingHeld);
                    other.GetComponent<MoneyHolder>().ChangeMoney(-other.GetComponent<MoneyHolder>().moneyBeingHeld);
                }
            }
        }
    }
}