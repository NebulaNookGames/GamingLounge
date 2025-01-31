using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToSitPosition", story: "Use [Machine] for [SitPosition] for [Agent] movement", category: "Action", id: "2862f95bca7a223f122706bac69779e8")]
public partial class GoToSitPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Machine;
    [SerializeReference] public BlackboardVariable<GameObject> SitPosition;
    [SerializeReference] public BlackboardVariable<GameObject> HeadTracker;
   
    NavMeshAgent navMeshAgent;
    
    protected override Status OnStart()
    {
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        
        navMeshAgent.isStopped = false; 
        
        if (Machine.Value == null) return Status.Failure;
        SitPosition.Value = Machine.Value.GetComponentInChildren<SitPositionRecognition>().GetSitPosition();
        
        if(SitPosition.Value == null) return Status.Failure;
        navMeshAgent.SetDestination(
            SitPosition.Value.transform.position - SitPosition.Value.transform.forward.normalized * 0.7f);
       
        return Status.Running;
    }
    
     protected override Status OnUpdate()
    {
        // Check if path is still valid
        if (!navMeshAgent.hasPath || Machine.Value == null || SitPosition.Value == null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
              // Add the arcade machine back to the list of available machines
              
            if(Machine.Value != null)
                WorldInteractables.instance.EndArcadeMachineOccupation(Machine.Value);
            
            // Reset the arcade machine reference
            Machine.Value = null;
            return Status.Failure;
        }

        // Continue going to path
        if (Vector3.Distance(Agent.Value.transform.position,
                SitPosition.Value.transform.position - SitPosition.Value.transform.forward) < 1f)
        {
            Vector3 aimPos = navMeshAgent.destination;
            Agent.Value.transform.position = new Vector3(aimPos.x, Agent.Value.transform.position.y, aimPos.z);
            navMeshAgent.enabled = false;
            Agent.Value.transform.rotation = Quaternion.LookRotation(-SitPosition.Value.transform.forward, Vector3.up);            
            if (HeadTracker.Value != null)
                HeadTracker.Value.GetComponent<HeadTracking>().noTracking = true; 
            return Status.Success;
        }

        return Status.Running;
    }
}

