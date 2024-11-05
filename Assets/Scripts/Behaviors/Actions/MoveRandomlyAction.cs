using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move Randomly", story: "Moves randomly", category: "Action", id: "59441a200545ce120d3f314b617192a5")]
public partial class MoveRandomlyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> agent;
    [SerializeReference] public BlackboardVariable<float> radius;

    private Vector3 randomPosition = Vector3.zero;
    
    protected override Status OnStart()
    {
        // Random point within a sphere
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += agent.Value.transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            randomPosition = hit.position;
        }
        agent.Value.GetComponent<NavMeshAgent>().SetDestination(randomPosition);
        return Status.Running;
    }
    
    protected override Status OnUpdate()
    {
        if (Vector3.Distance(randomPosition, agent.Value.transform.position) < 0.5f)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

