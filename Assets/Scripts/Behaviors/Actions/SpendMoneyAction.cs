using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpendMoney", story: "SpendMoney", category: "Action", id: "108eb02ce75216e6b3b46a0e95b076e8")]
public partial class SpendMoneyAction : Action
{
    [SerializeReference] public BlackboardVariable<int> spendAmount;
    [SerializeReference] public BlackboardVariable<GameObject> machineInUse;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
        machineInUse.Value.GetComponent<MoneyHolder>().ChangeMoney(spendAmount.Value);
    }
}