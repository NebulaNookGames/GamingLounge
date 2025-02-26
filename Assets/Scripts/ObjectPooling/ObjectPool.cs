using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance; 
    
    public GameObject footprintEffectPrefab;
    public GameObject moneyEffectPrefab;
    public GameObject multiplierAddEffectPrefab;
    public GameObject multiplierSubtractEffectPrefab;
    public GameObject talkEffectPrefab;
    public GameObject winEffectPrefab; 
    
    public Queue<GameObject> footprintEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> moneyEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> multiplierAddQueue = new Queue<GameObject>();
    public Queue<GameObject> multiplierSubtractQueue = new Queue<GameObject>();
    public Queue<GameObject> talkEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> winEffectQueue = new Queue<GameObject>();
    
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
            
            GameObject multiplierEffect = Instantiate(multiplierAddEffectPrefab, transform, true);
            multiplierAddQueue.Enqueue(multiplierEffect);
            multiplierEffect.SetActive(false);
            
            GameObject multiplierSubtractEffect = Instantiate(multiplierSubtractEffectPrefab, transform, true);
            multiplierSubtractQueue.Enqueue(multiplierSubtractEffect);
            multiplierSubtractEffect.SetActive(false);
            
            GameObject talkEffect = Instantiate(talkEffectPrefab, transform, true);
            talkEffectQueue.Enqueue(talkEffect);
            talkEffect.SetActive(false);
            
            GameObject winEffect = Instantiate(winEffectPrefab, transform, true);
            winEffectQueue.Enqueue(winEffect);
            winEffect.SetActive(false);
        }
    }

    public GameObject SpawnFootprintEffect(Vector3 pos, Quaternion rot)
    {
        if (footprintEffectQueue.Count > 0)
        {
            GameObject tempFootprintEffect = footprintEffectQueue.Dequeue();
            if(tempFootprintEffect == null) return null;
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
            if(tempMoneyEffect == null) return null;
            tempMoneyEffect.SetActive(true);
            tempMoneyEffect.transform.position = pos;
            tempMoneyEffect.transform.rotation = rot;
            return tempMoneyEffect;
        }
        return null;
    }

    public GameObject SpawnMultiplierAddEffect(Vector3 pos, Quaternion rot)
    {
        if (multiplierAddQueue.Count > 0)
        {
            GameObject tempMultiplierEffect = multiplierAddQueue.Dequeue();
            if(tempMultiplierEffect == null) return null;
            tempMultiplierEffect.SetActive(true);
            tempMultiplierEffect.transform.position = pos;
            tempMultiplierEffect.transform.rotation = rot;
            return tempMultiplierEffect;
        }
        return null;
    }

    public GameObject SpawnMultiplierSubtractEffect(Vector3 pos, Quaternion rot)
    {
        if (multiplierSubtractQueue.Count > 0)
        {
            GameObject tempMultiplierEffect = multiplierSubtractQueue.Dequeue();
            if(tempMultiplierEffect == null) return null;
            
            tempMultiplierEffect.SetActive(true);
            tempMultiplierEffect.transform.position = pos;
            tempMultiplierEffect.transform.rotation = rot;
            return tempMultiplierEffect;
        }
        return null;
    }

    public GameObject SpawnTalkEffect(Vector3 pos, Quaternion rot)
    {
        if (talkEffectQueue.Count > 0)
        {
            GameObject tempTalkEffect = talkEffectQueue.Dequeue();
            if(tempTalkEffect == null) return null;
            
            tempTalkEffect.SetActive(true);
            tempTalkEffect.transform.position = pos;
            tempTalkEffect.transform.rotation = rot;
            return tempTalkEffect;
        }

        return null;
    }

    public GameObject SpawnWinEffect(Vector3 pos, Quaternion rot)
    {
        if (winEffectQueue.Count > 0)
        {
            GameObject tempWinEffect = winEffectQueue.Dequeue();
            if (tempWinEffect == null) return null;

            tempWinEffect.SetActive(true);
            tempWinEffect.transform.position = pos;
            tempWinEffect.transform.rotation = rot;
            return tempWinEffect;
        }
        return null;
    }
}