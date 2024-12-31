using System;
using Unity.Behavior;
using UnityEngine;

public class InviteToLounge : MonoBehaviour
{
    public GameObject objectToCheckActiveState;

    private bool interactedWith = false; 
    
    private void Start()
    {
       Invoke(nameof(InitialCheck), .5f);
    }

    void InitialCheck()
    {
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        if (thisInList == null) return; 
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        if (EntityManager.instance.npcValues[index].invitedToLounge)
        {
            GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("InvitedToLounge", true);
            Invoke(nameof(DestroyObjects), .5f);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (interactedWith) return; 
        
        if (objectToCheckActiveState.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            interactedWith = true;
            GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue("InvitedToLounge", true);
            GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
            int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
            EntityManager.instance.npcValues[index].invitedToLounge = true;
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