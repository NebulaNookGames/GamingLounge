using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToArcadeMachine", story: "GoToMachine", category: "Action", id: "0366e19b6808b316247d6c8c16ba5912")]
public partial class GoToArcadeMachineAction : Action
{
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    // Blackboard variable for the arcade machine that the agent is heading to.
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;

    // Blackboard variable that indicates whether the agent has arrived at the machine.
    [SerializeReference] public BlackboardVariable<bool> atMachine;

    /// <summary>
    /// Starts the action of going to an arcade machine. It checks if there are any available machines,
    /// selects one, and sets the agent's destination to that machine.
    /// </summary>
    /// <returns>Status of the action (Running, Failure, etc.)</returns>
    protected override Status OnStart()
    {
        Agent.Value.GetComponent<NavMeshAgent>().enabled = true;
        Agent.Value.GetComponent<NavMeshAgent>().isStopped = false; 
        
        if (OccupiedArcadeMachine.Value == null) return Status.Failure;
        
        Agent.Value.GetComponent<NavMeshAgent>().SetDestination(OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position);
        atMachine.Value = true;
        return Status.Running;
    }

    /// <summary>
    /// Updates the action, checking whether the agent has arrived at the arcade machine.
    /// </summary>
    /// <returns>Status of the action (Running, Success, Failure)</returns>
    protected override Status OnUpdate()
    {
        if (Agent.Value.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
            return Status.Failure;
        if (OccupiedArcadeMachine.Value == null)
            return Status.Failure;
        if (Vector3.Distance(Agent.Value.transform.position,
                OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position) < 1f)
        {
            Agent.Value.GetComponent<NavMeshAgent>().enabled = false;
            Vector3 aimPos = OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position;
            Agent.Value.transform.position = new Vector3(aimPos.x, Agent.Value.transform.position.y, aimPos.z);
            Agent.Value.transform.rotation = Quaternion.LookRotation(OccupiedArcadeMachine.Value.GetComponent<RotatePlacementObject>().objectToRotate.transform.forward, Vector3.up);            
            return Status.Success;
        }

        return Status.Running;
    }
}