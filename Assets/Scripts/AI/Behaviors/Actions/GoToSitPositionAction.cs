using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Random = System.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToSitPosition", story: "Use [Machine] for [SitPosition] for [Agent] movement", category: "Action", id: "2862f95bca7a223f122706bac69779e8")]
public partial class GoToSitPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Machine;
    [SerializeReference] public BlackboardVariable<GameObject> SitPosition;
    
    protected override Status OnStart()
    {
        Agent.Value.GetComponent<NavMeshAgent>().isStopped = false; 
        
        if (Machine.Value == null) return Status.Failure;
        
        SitPosition.Value = Machine.Value.GetComponentInChildren<SitPositionRecognition>().validObjects[UnityEngine.Random.Range(0, Machine.Value.GetComponentInChildren<SitPositionRecognition>().validObjects.Count)];
        SitPosition.Value.GetComponent<SitPositionAvailability>().available = false;
        Machine.Value.GetComponentInChildren<SitPositionRecognition>().validObjects.Remove(SitPosition.Value);
        
        Agent.Value.GetComponent<NavMeshAgent>().SetDestination(
            SitPosition.Value.transform.position - SitPosition.Value.transform.forward.normalized * 0.7f);
       
        return Status.Running;
    }
    
     protected override Status OnUpdate()
    {
        if (Agent.Value.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
            return Status.Failure;
        if (Machine.Value == null)
            return Status.Failure;
        if(SitPosition.Value == null)
            return Status.Failure;
        if (Vector3.Distance(Agent.Value.transform.position,
                SitPosition.Value.transform.position - SitPosition.Value.transform.forward) < .5f)
        {
            Vector3 aimPos = Agent.Value.GetComponent<NavMeshAgent>().destination;
            Agent.Value.transform.position = new Vector3(aimPos.x, Agent.Value.transform.position.y, aimPos.z);
            Agent.Value.GetComponent<NavMeshAgent>().enabled = false;
            Agent.Value.transform.rotation = Quaternion.LookRotation(-SitPosition.Value.transform.forward, Vector3.up);            
            return Status.Success;
        }

        return Status.Running;
    }
}

