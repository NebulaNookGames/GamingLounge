using System;
using UnityEngine;
using UnityEngine.Serialization;

public class OnEnableChangeLocationToTag : MonoBehaviour
{
    [FormerlySerializedAs("tag")] public string tagToFind;
    public Vector3 offset; 
    private void OnEnable()
    {
        Invoke(nameof(ChangeLocation), 5f);
    }

    void ChangeLocation()
    {
        if(GameObject.FindGameObjectWithTag(tagToFind))
            gameObject.transform.position = GameObject.FindGameObjectWithTag(tagToFind).transform.position + offset;
    }
}
