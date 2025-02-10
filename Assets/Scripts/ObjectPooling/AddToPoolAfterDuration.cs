using System;
using UnityEngine;

public class AddToPoolAfterDuration : MonoBehaviour
{
    public bool isFootprintEffect;
    public bool isMoneyEffect;
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
    }
}
