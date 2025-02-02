using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem; 

public class InviteToLounge : MonoBehaviour
{
    public GameObject objectToCheckActiveState;

    private bool interactedWith = false;

    public GameObject invitedParticleEffect;
    public GameObject notInvitedIndicator;

    public VisitorEntity entity; 
    public InputActionProperty objectInteractionAction;
    
    private void OnEnable()
    {
        objectInteractionAction.action.performed += Invite;
    }

    private void OnDisable()
    {
        objectInteractionAction.action.performed -= Invite;
    }

    private void Start()
    {
       Invoke(nameof(InitialCheck), .5f);
    }

    void Invite(InputAction.CallbackContext context)
    {
        if (interactedWith || !objectToCheckActiveState.activeSelf) return; 
        
        interactedWith = true;
        entity.invitedToLounge = true;
        Instantiate(invitedParticleEffect, transform.position, Quaternion.identity);
        Destroy(notInvitedIndicator);
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        EntityManager.instance.npcValues[index].invitedToLounge = true;
        Invoke(nameof(DestroyObjects), .5f);
    }
    
    void InitialCheck()
    {
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        if (thisInList == null) return; 
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        if (EntityManager.instance.npcValues[index].invitedToLounge)
        {
            GetComponent<VisitorEntity>().invitedToLounge = true; 
            notInvitedIndicator.active = false;
            Invoke(nameof(DestroyObjects), .5f);
        }
    }

    void DestroyObjects()
    {
        Destroy(GetComponent<ActivateAtDistance>());
        Destroy(objectToCheckActiveState);
        Destroy(this);
    }
}