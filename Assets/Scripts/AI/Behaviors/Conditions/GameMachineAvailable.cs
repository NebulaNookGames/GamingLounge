using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable, GeneratePropertyBag]
[Condition(name: "GameMachineAvailable", story: "[GameObject] is Machine", category: "Conditions", id: "5beae1c2d39f002b8e2d821a643a114f")]
public partial class GameMachineAvailable : Condition
{
    // Blackboard variable representing the GameObject to check for null.
    [SerializeReference] public BlackboardVariable<GameObject> gameObject;
    
    /// <summary>
    /// Checks if the list of arcade machines is not empty.
    /// </summary>
    /// <returns>True if the arcade machines list has elements, otherwise false.</returns>
    public override bool IsTrue()
    {
        if (WorldInteractables.instance.ArcadeMachines.Count > 0)
        {
            gameObject.Value = WorldInteractables.instance.ArcadeMachines[0];
            WorldInteractables.instance.ArcadeMachines.Remove(gameObject.Value);
            return true;
        }

        return false; 
    }
}