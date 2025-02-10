using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        if(Camera.main)
            cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(cam)
            transform.LookAt(cam.transform.position);
    }
}