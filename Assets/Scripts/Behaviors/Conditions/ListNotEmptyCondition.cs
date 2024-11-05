using System;
using Unity.Behavior;
using UnityEngine;
using System.Collections.Generic;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "ListNotEmpty", story: "ListNotEmpty", category: "Conditions", id: "5beae1c2d39f002b8e2d821a643a114f")]
public partial class ListNotEmptyCondition : Condition
{
    public override bool IsTrue()
    {
        if(WorldInteractables.instance.ArcadeMachines.Count > 0) 
            return true;

        return false; 
    }
}
