using System;
using UnityEngine;

public class AddToPoolAfterDuration : MonoBehaviour
{
    public bool isFootprintEffect;
    public bool isMoneyEffect;
    public bool isMultiplierEffect;
    public bool isSubtractMultiplierEffect;
    public bool isTalkEffect;
    public bool isWinEffect; 
    
    public float waitTime = 5.5f;

    private void OnEnable()
    {
        Invoke(nameof(AddToPool), waitTime);
    }

    void AddToPool()
    {
        if (isFootprintEffect)
        {
          ObjectPool.instance.footprintEffectQueue.Enqueue(gameObject);
          gameObject.SetActive(false);
        }

        if (isMoneyEffect)
        {
           ObjectPool.instance.moneyEffectQueue.Enqueue(gameObject);
           gameObject.SetActive(false);
        }

        if (isMultiplierEffect)
        {
            ObjectPool.instance.multiplierAddQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        if (isSubtractMultiplierEffect)
        {
            ObjectPool.instance.multiplierSubtractQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        if (isTalkEffect)
        {
            ObjectPool.instance.talkEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }

        if (isWinEffect)
        {
            ObjectPool.instance.winEffectQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}
