using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "MachineAvailability", story: "[Machine] is available and reachable by [Agent]", category: "Conditions", id: "9917d80bb8f410a003687d3b1504175d")]
public partial class MachineAvailabilityCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Machine;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    
    
    public override bool IsTrue()
    {
        if (WorldInteractables.instance.availableArcadeMachines.Count > 0)
        {
            
            int randomMachineIndex = UnityEngine.Random.Range(0, WorldInteractables.instance.availableArcadeMachines.Count);
            
            NavMeshPath testPath = new NavMeshPath();
            Agent.Value.GetComponent<NavMeshAgent>().CalculatePath(WorldInteractables.instance.availableArcadeMachines[randomMachineIndex].transform.position, testPath);
            if (testPath.status == NavMeshPathStatus.PathPartial) return false; 
            
            Machine.Value = WorldInteractables.instance.availableArcadeMachines[randomMachineIndex];
            WorldInteractables.instance.OccupyAradeMachine(Machine.Value);
            return true;
        }
        return false;
    }
}
