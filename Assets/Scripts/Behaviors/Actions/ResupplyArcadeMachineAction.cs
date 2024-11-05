using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResupplyArcadeMachine", story: "ResupplyArcadeMachine", category: "Action", id: "2ca206c07f46bcb57a2bcee09a2e39a8")]
public partial class ResupplyArcadeMachineAction : Action
{

    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        WorldInteractables.instance.ArcadeMachines.Add(OccupiedArcadeMachine.Value);
        OccupiedArcadeMachine.Value = null;
    }
}

