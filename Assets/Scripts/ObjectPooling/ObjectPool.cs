using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance; 
    
    public GameObject footprintEffectPrefab;
    public GameObject moneyEffectPrefab; 
    public Queue<GameObject> footprintEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> moneyEffectQueue = new Queue<GameObject>();
    public int fillAmountPerQueue = 50;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < fillAmountPerQueue; i++)
        {
            GameObject footprintEffect = Instantiate(footprintEffectPrefab, transform, true);
            footprintEffectQueue.Enqueue(footprintEffect);
            footprintEffect.SetActive(false);
            
            GameObject moneyEffect = Instantiate(moneyEffectPrefab, transform, true);
            moneyEffectQueue.Enqueue(moneyEffect);
            moneyEffect.SetActive(false);
        }
    }

    public GameObject SpawnFootprintEffect(Vector3 pos, Quaternion rot)
    {
        if (footprintEffectQueue.Count > 0)
        {
            GameObject tempFootprintEffect = footprintEffectQueue.Dequeue();
            tempFootprintEffect.SetActive(true);
            tempFootprintEffect.transform.position = pos;
            tempFootprintEffect.transform.rotation = rot;
            return tempFootprintEffect;
        }
        return null;
    }
    
    public GameObject SpawnMoneyEffect(Vector3 pos, Quaternion rot)
    {
        if (moneyEffectQueue.Count > 0)
        {
            GameObject tempMoneyEffect = moneyEffectQueue.Dequeue();
            tempMoneyEffect.SetActive(true);
            tempMoneyEffect.transform.position = pos;
            tempMoneyEffect.transform.rotation = rot;
            return tempMoneyEffect;
        }
        return null;
    }
}