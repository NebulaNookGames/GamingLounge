using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResupplyArcadeMachine", story: "ResupplyArcadeMachine", category: "Action", id: "2ca206c07f46bcb57a2bcee09a2e39a8")]
public partial class ResupplyArcadeMachineAction : Action
{
    // Blackboard variable for the arcade machine to be resupplied.
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;

    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    // Blackboard variable indicating whether the agent is at the machine.
    [SerializeReference] public BlackboardVariable<bool> atMachine;

    [SerializeReference] public BlackboardVariable<GameObject> HeadTracker;
    
    /// <summary>
    /// Starts the resupply action. Currently, no logic is executed in this method, the action remains running.
    /// </summary>
    /// <returns>Status of the action (Running, Failure, etc.)</returns>
    protected override Status OnStart()
    {   if (HeadTracker.Value != null) 
            HeadTracker.Value.GetComponent<HeadTracking>().noTracking = false; 
        return Status.Running;
    }

    /// <summary>
    /// Updates the action by checking if the arcade machine exists.
    /// </summary>
    /// <returns>Status of the action (Success, Failure)</returns>
    protected override Status OnUpdate()
    {
        if (OccupiedArcadeMachine.Value == null) return Status.Failure;
        
        return Status.Success;
    }

    /// <summary>
    /// Ends the action by resupplying the arcade machine and resetting related variables.
    /// </summary>
    protected override void OnEnd()
    {
        // Add the arcade machine back to the list of available machines
        WorldInteractables.instance.EndArcadeMachineOccupation(OccupiedArcadeMachine.Value);
        
        // Set the status to indicate the agent is no longer at the machine
        atMachine.Value = false;

        // Reset the arcade machine reference
        OccupiedArcadeMachine.Value = null;
    }
}