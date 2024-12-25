using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseMachineAnimation", story: "Choose [AnimatorAnimation] between [Interact] , [Bike] and [Race] from [occupiedMachine]", category: "Action", id: "b8eb4a52b836069beee6525f7c9b9fa4")]
public partial class ChooseMachineAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> AnimatorAnimation;
    [SerializeReference] public BlackboardVariable<string> Interact;
    [SerializeReference] public BlackboardVariable<string> Bike;
    [SerializeReference] public BlackboardVariable<string> Race;
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedMachine;

    protected override Status OnStart()
    {
        if(AnimatorAnimation.Value == null || Interact.Value == null || Bike.Value == null || Race.Value == null || OccupiedMachine.Value == null) return Status.Failure;

        switch (OccupiedMachine.Value.GetComponent<UsagePositionStorage>().machineType)
        {
            case MachineType.ArcadeMachine:
                AnimatorAnimation.Value.SetBool(Interact.Value, true);
                break;
            case MachineType.Bike:
                AnimatorAnimation.Value.SetBool(Bike.Value, true);
                break;
            case MachineType.Race:
                AnimatorAnimation.Value.SetBool(Race.Value, true);
                break;
            default:
                AnimatorAnimation.Value.SetBool(Interact.Value, true);
                break;
        }
        
        return Status.Success;
    }
}

