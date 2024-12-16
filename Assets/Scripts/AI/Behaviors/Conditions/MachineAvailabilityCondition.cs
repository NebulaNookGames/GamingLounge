using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "MachineAvailability", story: "[Machine] is available", category: "Conditions", id: "9917d80bb8f410a003687d3b1504175d")]
public partial class MachineAvailabilityCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Machine;

    public override bool IsTrue()
    {
        if (WorldInteractables.instance.ArcadeMachines.Count > 0)
        {
            int randomMachineIndex = UnityEngine.Random.Range(0, WorldInteractables.instance.ArcadeMachines.Count);
            Machine.Value = WorldInteractables.instance.ArcadeMachines[randomMachineIndex];
            WorldInteractables.instance.ArcadeMachines.Remove(Machine.Value);
            return true;
        }
        return false;
    }
}
