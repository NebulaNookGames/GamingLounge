using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "SitPositionValid", story: "[Machine] has SitPosition", category: "Conditions", id: "5a09f3234d075f4fe5007c762526380a")]
public partial class SitPositionValidCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Machine;

    public override bool IsTrue()
    {
        if (Machine.Value == null) return false; 
        if (!Machine.Value.GetComponentInChildren<SitPositionRecognition>()) return false;

        if (Machine.Value.GetComponentInChildren<SitPositionRecognition>().validObjects.Count > 0) return true;

        return false; 
    }
}
