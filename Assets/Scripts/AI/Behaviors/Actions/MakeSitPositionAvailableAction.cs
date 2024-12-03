using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MakeSitPositionAvailable", story: "Make [SitPosition] available", category: "Action", id: "2dc53cdbeb0a3425988a61a0c200b56b")]
public partial class MakeSitPositionAvailableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> SitPosition;

    protected override Status OnStart()
    {
        SitPosition.Value.GetComponent<SitPositionAvailability>().available = true;
        SitPosition.Value = null; 
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

