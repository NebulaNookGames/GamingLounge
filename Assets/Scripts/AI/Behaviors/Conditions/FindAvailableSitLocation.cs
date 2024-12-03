using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "FindAvailableSitLocation", story: "FindAvailableSitLocation", category: "Conditions", id: "5beae1c2d39f002b8e2d821a643a114f")]
public partial class FindAvailableSitLocation : Condition
{
    // Blackboard variable representing the GameObject to check for null.
    [SerializeReference] public BlackboardVariable<GameObject> arcadeMachineInUse;
    
    /// <summary>
    /// Checks if the list of arcade machines is not empty.
    /// </summary>
    /// <returns>True if the arcade machines list has elements, otherwise false.</returns>
    public override bool IsTrue()
    {
        if (arcadeMachineInUse.Value.GetComponentInChildren<SitPositionRecognition>() == null) return false;

        if (arcadeMachineInUse.Value.GetComponentInChildren<SitPositionRecognition>().validObjects.Count > 0) return true;

        return false;
    }
}