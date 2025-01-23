using System;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public GameObject effect;

    private void OnEnable()
    {
        Instantiate(effect, transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        Instantiate(effect, transform.position, transform.rotation);
    }
}