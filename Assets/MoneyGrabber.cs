using System;
using UnityEngine;

public class MoneyGrabber : MonoBehaviour
{
    [SerializeField] MoneyManager moneyManager;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Machine"))
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
