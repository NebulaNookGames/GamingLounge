using UnityEngine;
using System;

/// <summary>
/// This class tracks the money being held and triggers an event whenever the amount changes.
/// Other components can listen for the money change and react accordingly.
/// </summary>
public class MoneyHolder : MonoBehaviour
{
    // The amount of money currently being held.
    public int moneyBeingHeld { get; private set; }

    // Event triggered when the money amount changes.
    public event Action<int> OnMoneyChanged;

    public GameObject effectSpawnPos; 
    public BeginVideoPlayer beginVideoPlayer;
    
    
    /// <summary>
    /// Changes the money amount by a specified amount and triggers the OnMoneyChanged event.
    /// </summary>
    /// <param name="amount">The amount to change the money by.</param>
    public void ChangeMoney(int amount, bool changePlay, bool play, bool adding)
    {
        moneyBeingHeld += amount; // Update the money amount.
        if (adding)
        {
            ObjectPool.instance.SpawnWinEffect(effectSpawnPos.transform.position, effectSpawnPos.transform.rotation);
        }
        else
        {
            ObjectPool.instance.SpawnMoneyEffect(effectSpawnPos.transform.position, effectSpawnPos.transform.rotation);
        }

        OnMoneyChanged?.Invoke(moneyBeingHeld); // Trigger the OnMoneyChanged event.
    }

    private void OnDestroy()
    {
        if(MoneyManager.instance != null)
            MoneyManager.instance.ChangeMoney(moneyBeingHeld);
    }
}