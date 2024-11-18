using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "GameObjectIsNull", story: "[GameObject] is Null", category: "Conditions", id: "59d402c2a80e34afaf943a0bce01259d")]
public partial class GameObjectIsNullCondition : Condition
{
    // Blackboard variable representing the GameObject to check for null.
    [SerializeReference] public BlackboardVariable<GameObject> gameObject;

    /// <summary>
    /// Checks if the GameObject is null.
    /// </summary>
    /// <returns>True if the GameObject is null, otherwise false.</returns>
    public override bool IsTrue()
    {
        if(gameObject == null) return true;

        return false;
    }
}