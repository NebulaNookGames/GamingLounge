using System;
using Unity.Behavior;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "ListNotEmpty", story: "ListNotEmpty", category: "Conditions", id: "5beae1c2d39f002b8e2d821a643a114f")]
public partial class ListNotEmptyCondition : Condition
{
    /// <summary>
    /// Checks if the list of arcade machines is not empty.
    /// </summary>
    /// <returns>True if the arcade machines list has elements, otherwise false.</returns>
    public override bool IsTrue()
    {
        if(WorldInteractables.instance.ArcadeMachines.Count > 0) 
            return true;

        return false; 
    }
}