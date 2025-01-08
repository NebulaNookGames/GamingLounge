using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Animations.Rigging;


[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseMachineAnimation", story: "Choose [AnimatorAnimation] between [Interact] , [Bike] and [Race] from [occupiedMachine]", category: "Action", id: "b8eb4a52b836069beee6525f7c9b9fa4")]
public partial class ChooseMachineAnimationAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> AnimatorAnimation;
    [SerializeReference] public BlackboardVariable<string> ArcadeMachine;
    [SerializeReference] public BlackboardVariable<string> InteractDefault;
    [SerializeReference] public BlackboardVariable<string> Bike;
    [SerializeReference] public BlackboardVariable<string> Race;
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedMachine;
    protected override Status OnStart()
    {
        if(AnimatorAnimation.Value == null || ArcadeMachine.Value == null || InteractDefault.Value == null || Bike.Value == null || Race.Value == null || OccupiedMachine.Value == null) return Status.Failure;

        switch (OccupiedMachine.Value.GetComponent<UsagePositionStorage>().machineType)
        {
            case MachineType.ArcadeMachine:
                AnimatorAnimation.Value.SetBool(ArcadeMachine.Value, true);
                break;
            case MachineType.Bike:
                AnimatorAnimation.Value.SetBool(Bike.Value, true);
                break;
            case MachineType.Race:
                AnimatorAnimation.Value.SetBool(Race.Value, true);
                break;
            default:
                AnimatorAnimation.Value.SetBool(InteractDefault.Value, true);
                break;
        }
        
        return Status.Success;
    }
}

