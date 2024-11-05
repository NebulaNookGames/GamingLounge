using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToArcadeMachine", story: "GoToMachine", category: "Action", id: "0366e19b6808b316247d6c8c16ba5912")]
public partial class GoToArcadeMachineAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;

    protected override Status OnStart()
    {
        if (WorldInteractables.instance.ArcadeMachines.Count < 1)
            return Status.Failure;
        
        OccupiedArcadeMachine.Value = WorldInteractables.instance.ArcadeMachines[0];
        WorldInteractables.instance.ArcadeMachines.Remove(OccupiedArcadeMachine.Value);
        Agent.Value.GetComponent<NavMeshAgent>().SetDestination(OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Vector3.Distance(Agent.Value.transform.position, OccupiedArcadeMachine.Value.GetComponent<UsagePositionStorage>().usagePosition.position) < 0.5f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

