using System;
using UnityEngine;

public class AddToPoolAfterDuration : MonoBehaviour
{
    #region Serialized Fields

    // Flags to determine which effect type this object represents
    public bool isFootprintEffect;
    public bool isMoneyEffect;
    public bool isMultiplierEffect;
    public bool isSubtractMultiplierEffect;
    public bool isTalkEffect;
    public bool isWinEffect;

    // Time to wait before returning the object to the pool
    public float waitTime = 5.5f;

    #endregion

    #region Unity Methods

    // Called when the object is enabled
    private void OnEnable()
    {
        // Invoke the AddToPool method after the specified wait time
        Invoke(nameof(AddToPool), waitTime);
    }

    #endregion

    #region Pooling Logic

    // Method to add the object back to the correct pool based on its type
    void AddToPool()
    {
        // Check if the object is a footprint effect and add it to the corresponding pool
        if (isFootprintEffect)
        {
            ObjectPool.instance.footprintEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        // Check if the object is a money effect and add it to the corresponding pool
        if (isMoneyEffect)
        {
            ObjectPool.instance.moneyEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        // Check if the object is a multiplier effect and add it to the corresponding pool
        if (isMultiplierEffect)
        {
            ObjectPool.instance.multiplierAddQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        // Check if the object is a subtract multiplier effect and add it to the corresponding pool
        if (isSubtractMultiplierEffect)
        {
            ObjectPool.instance.multiplierSubtractQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        // Check if the object is a talk effect and add it to the corresponding pool
        if (isTalkEffect)
        {
            ObjectPool.instance.talkEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        // Check if the object is a win effect and add it to the corresponding pool
        if (isWinEffect)
        {
            ObjectPool.instance.winEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }

    #endregion
}
