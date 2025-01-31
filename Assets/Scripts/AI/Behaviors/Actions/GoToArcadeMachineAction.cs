using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.Analytics;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToArcadeMachine", story: "GoToMachine", category: "Action", id: "0366e19b6808b316247d6c8c16ba5912")]
public partial class GoToArcadeMachineAction : Action
{
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    [SerializeReference] public BlackboardVariable<GameObject> HeadTracker;
    
    // Blackboard variable for the arcade machine that the agent is heading to.
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;

    // Blackboard variable that indicates whether the agent has arrived at the machine.
    [SerializeReference] public BlackboardVariable<bool> atMachine;

    
    NavMeshAgent navMeshAgent;
    
    /// <summary>
    /// Starts the action of going to an arcade machine. It checks if there are any available machines,
    /// selects one, and sets the agent's destination to that machine.
    /// </summary>
    /// <returns>Status of the action (Running, Failure, etc.)</returns>
    protected override Status OnStart()
    {
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false; 
        
        if (OccupiedArcadeMachine.Value == null) return Status.Failure;
        
        navMeshAgent.SetDestination(OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position);
        atMachine.Value = true;
        return Status.Running;
    }

    /// <summary>
    /// Updates the action, checking whether the agent has arrived at the arcade machine.
    /// </summary>
    /// <returns>Status of the action (Running, Success, Failure)</returns>
    protected override Status OnUpdate()
    {
        // Check if path is still valid
        if (!navMeshAgent.hasPath || OccupiedArcadeMachine.Value == null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
      
            // Add the arcade machine back to the list of available machines
            if (OccupiedArcadeMachine.Value != null)
            {
                WorldInteractables.instance.EndArcadeMachineOccupation(OccupiedArcadeMachine.Value);
            }

            // Set the status to indicate the agent is no longer at the machine
            atMachine.Value = false;
            // Reset the arcade machine reference
            OccupiedArcadeMachine.Value = null;
            return Status.Failure;
        }

        // Continue going to path
        if (Vector3.Distance(Agent.Value.transform.position,
                OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position) < 1f)
        {
            navMeshAgent.enabled = false;
            Vector3 aimPos = OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position;
            Agent.Value.transform.position = new Vector3(aimPos.x, aimPos.y, aimPos.z);
            Agent.Value.transform.rotation = Quaternion.LookRotation(OccupiedArcadeMachine.Value.GetComponent<RotatePlacementObject>().objectToRotate.transform.forward, Vector3.up);            

            if (HeadTracker.Value != null)
                HeadTracker.Value.GetComponent<HeadTracking>().noTracking = true; 
            
            return Status.Success;
        }

        return Status.Running;
    }
}