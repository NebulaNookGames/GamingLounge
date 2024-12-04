using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI; 

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpendMoney", story: "SpendMoney", category: "Action", id: "108eb02ce75216e6b3b46a0e95b076e8")]
public partial class SpendMoneyAction : Action
{
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    // Blackboard variable for the arcade machine currently in use.
    [SerializeReference] public BlackboardVariable<GameObject> machineInUse;

    /// <summary>
    /// Starts the action. No specific logic is executed in this method, the action remains running.
    /// </summary>
    /// <returns>Status of the action (Running, Failure, etc.)</returns>
    protected override Status OnStart()
    {
        return Status.Running;
    }

    /// <summary>
    /// Updates the action by checking if a machine is in use.
    /// </summary>
    /// <returns>Status of the action (Success, Failure)</returns>
    protected override Status OnUpdate()
    {
        if (machineInUse.Value == null) return Status.Failure;
        
        return Status.Success;
    }

    /// <summary>
    /// Ends the action by spending money from the machine.
    /// </summary>
    protected override void OnEnd()
    {
        if(machineInUse.Value != null)
            machineInUse.Value.GetComponent<MoneyHolder>().ChangeMoney(machineInUse.Value.GetComponent<Cost>().amount);
    }
}