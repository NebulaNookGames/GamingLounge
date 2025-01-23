using System;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
    public int seconds;

    private void Awake()
    {
        Destroy(gameObject, seconds);
    }
}
