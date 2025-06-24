using System;
using Unity.VisualScripting;
using UnityEngine;

public class AlienInfoHandler : MonoBehaviour
{
    public float activationDistance = 2; 
    public GameObject objectToHandle;

    Transform player; 
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < activationDistance &&
            GetComponentInParent<VisitorEntity>().invitedToLounge)
        {
            objectToHandle.SetActive(true);
        }
        else
        {
            objectToHandle.SetActive(false);
        }
    }
}
