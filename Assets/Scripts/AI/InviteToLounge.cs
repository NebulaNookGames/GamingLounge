using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.InputSystem; 

public class InviteToLounge : MonoBehaviour
{
    public GameObject objectToCheckActiveState;

    private bool interactedWith = false;


    public InputActionProperty objectInteractionAction;
    
    public ChangeBehaviorParameterAfterDuration changeBehaviorParameterAfterDuration;

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
        GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("InvitedToLounge", true);
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        EntityManager.instance.npcValues[index].invitedToLounge = true;
        changeBehaviorParameterAfterDuration.enabled = true;
        Invoke(nameof(DestroyObjects), .5f);
    }
    
    void InitialCheck()
    {
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        if (thisInList == null) return; 
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        if (EntityManager.instance.npcValues[index].invitedToLounge)
        {
            GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("InvitedToLounge", true);
            changeBehaviorParameterAfterDuration.enabled = true;
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