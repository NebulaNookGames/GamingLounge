using System;
using UnityEngine;

public class OnEnableChangeLocationToTag : MonoBehaviour
{
    public string tag;
    public Vector3 offset; 
    private void OnEnable()
    {
        Invoke(nameof(ChangeLocation), 5f);
    }

    void ChangeLocation()
    {
        gameObject.transform.position = GameObject.FindGameObjectWithTag(tag).transform.position + offset;
    }
}
