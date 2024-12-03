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
    // Blackboard variable for the agent (the GameObject that will move randomly).
    [SerializeReference] public BlackboardVariable<GameObject> agent;

    // Blackboard variable for the radius within which the agent can move randomly.
    [SerializeReference] public BlackboardVariable<float> radius;

    // The random position the agent will move to.
    private Vector3 randomPosition = Vector3.zero;

    /// <summary>
    /// Starts the action by calculating a random position within the specified radius
    /// and sets the agent's destination to that position.
    /// </summary>
    /// <returns>Status of the action (Running, Failure, etc.)</returns>
    protected override Status OnStart()
    {
        agent.Value.GetComponent<NavMeshAgent>().enabled = true; 
        agent.Value.GetComponent<NavMeshAgent>().isStopped = false;
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
    
    /// <summary>
    /// Updates the action by checking if the agent has reached the random position.
    /// </summary>
    /// <returns>Status of the action (Running, Success, Failure)</returns>
    protected override Status OnUpdate()
    {
        if (agent.Value.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathInvalid)
            return Status.Failure;
        
        if (Vector3.Distance(randomPosition, agent.Value.transform.position) < 1f)
        {
            agent.Value.GetComponent<NavMeshAgent>().isStopped = true;
            return Status.Success;
        }
        
        return Status.Running;
    }

    /// <summary>
    /// Called when the action ends. This implementation is empty but can be overridden for cleanup.
    /// </summary>
    protected override void OnEnd()
    {
    }
}