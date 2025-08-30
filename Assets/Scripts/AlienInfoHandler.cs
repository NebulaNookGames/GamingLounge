using System;
using Unity.VisualScripting;
using UnityEngine;

public class AlienInfoHandler : MonoBehaviour
{
    public float activationDistance = 2; 
    public GameObject objectToHandle;

    Transform player;

    private void Awake()
    {
        objectToHandle.SetActive(false);
    }

    private void Update()
    {
        if(player == null && GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null) return; 
            
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
