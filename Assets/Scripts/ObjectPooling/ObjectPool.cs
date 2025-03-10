using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    #region Singleton

    public static ObjectPool instance;

    #endregion

    #region Prefabs

    // Prefabs for different effect types
    public GameObject footprintEffectPrefab;
    public GameObject moneyEffectPrefab;
    public GameObject multiplierAddEffectPrefab;
    public GameObject multiplierSubtractEffectPrefab;
    public GameObject talkEffectPrefab;
    public GameObject winEffectPrefab;

    #endregion

    #region Queues

    // Queues for storing pooled effects
    public Queue<GameObject> footprintEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> moneyEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> multiplierAddQueue = new Queue<GameObject>();
    public Queue<GameObject> multiplierSubtractQueue = new Queue<GameObject>();
    public Queue<GameObject> talkEffectQueue = new Queue<GameObject>();
    public Queue<GameObject> winEffectQueue = new Queue<GameObject>();

    #endregion

    #region Pool Settings

    // Amount of items to fill each queue with
    public int fillAmountPerQueue = 50;
    // Amount of footprint effects to fill
    public int footprintAmount = 500;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Fill the pool with initial items
        FillPool();
    }

    #endregion

    #region Pooling Methods

    // Fill the pools with the specified number of effects
    public void FillPool()
    {
        // Fill footprint effect pool
        for (int i = 0; i < footprintAmount; i++)
        {
            GameObject footprintEffect = Instantiate(footprintEffectPrefab, transform, true);
            footprintEffectQueue.Enqueue(footprintEffect);
            footprintEffect.SetActive(false);
        }

        // Fill other effect pools
        for (int i = 0; i < fillAmountPerQueue; i++)
        {
            AddEffectToQueue(moneyEffectPrefab, moneyEffectQueue);
            AddEffectToQueue(multiplierAddEffectPrefab, multiplierAddQueue);
            AddEffectToQueue(multiplierSubtractEffectPrefab, multiplierSubtractQueue);
            AddEffectToQueue(talkEffectPrefab, talkEffectQueue);
            AddEffectToQueue(winEffectPrefab, winEffectQueue);
        }
    }

    // Add a single effect to the respective queue
    private void AddEffectToQueue(GameObject effectPrefab, Queue<GameObject> queue)
    {
        GameObject effect = Instantiate(effectPrefab, transform, true);
        queue.Enqueue(effect);
        effect.SetActive(false);
    }

    #endregion

    #region Spawn Methods

    // Spawn effects from their respective queues
    public GameObject SpawnFootprintEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(footprintEffectQueue, pos, rot);
    }

    public GameObject SpawnMoneyEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(moneyEffectQueue, pos, rot);
    }

    public GameObject SpawnMultiplierAddEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(multiplierAddQueue, pos, rot);
    }

    public GameObject SpawnMultiplierSubtractEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(multiplierSubtractQueue, pos, rot);
    }

    public GameObject SpawnTalkEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(talkEffectQueue, pos, rot);
    }

    public GameObject SpawnWinEffect(Vector3 pos, Quaternion rot)
    {
        return SpawnEffectFromQueue(winEffectQueue, pos, rot);
    }

    // Generic method to spawn an effect from any queue
    private GameObject SpawnEffectFromQueue(Queue<GameObject> queue, Vector3 pos, Quaternion rot)
    {
        if (queue.Count > 0)
        {
            GameObject effect = queue.Dequeue();
            if (effect == null) return null;
            effect.SetActive(true);
            effect.transform.position = pos;
            effect.transform.rotation = rot;
            return effect;
        }
        return null;
    }

    #endregion
}
