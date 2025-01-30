using System;
using Unity.Behavior;
using Unity.Cinemachine;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI; 

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Despawn", story: "Agent Despawns", category: "Action", id: "e214aba943bc083ffa0e8166024e8c3e")]
public partial class DespawnAction : Action
{
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> agent;
    
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> OccupiedArcadeMachine;
    
    // Blackboard variable for the agent (the GameObject representing the character).
    [SerializeReference] public BlackboardVariable<GameObject> buildingChecker;

    // Blackboard variable for the radius within which the agent can move randomly.
    [SerializeReference] public BlackboardVariable<float> radius;
    
    // The random position the agent will move to.
    private Vector3 randomPosition = Vector3.zero;
    
    protected override Status OnStart()
    {
        agent.Value.GetComponent<NavMeshAgent>().enabled = true; 
        agent.Value.GetComponent<NavMeshAgent>().isStopped = false;

        bool positionFound = false;
        int tries = 0; 
        
        while (!positionFound)
        {
            // Random point within a sphere
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius.Value;
            randomDirection += agent.Value.transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius.Value, NavMesh.AllAreas))
            {
                buildingChecker.Value.transform.position = hit.position;
            }

            if (!buildingChecker.Value.GetComponent<CheckIfInBuilding>().IsInBuilding())
            {
                positionFound = true;
                randomPosition = hit.position;
                agent.Value.GetComponent<NavMeshAgent>().SetDestination(randomPosition);
            }
           
            tries++;
            if (tries >= 50)
            {
                positionFound = true;
                randomPosition = hit.position;
                agent.Value.GetComponent<NavMeshAgent>().SetDestination(randomPosition);
            }
        }
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (!agent.Value.GetComponent<NavMeshAgent>().hasPath)
        {
            agent.Value.GetComponent<NavMeshAgent>().isStopped = true;
            return Status.Success;
        }

        if (Vector3.Distance(randomPosition, agent.Value.transform.position) < 1f)
        {
            agent.Value.GetComponent<NavMeshAgent>().isStopped = true;
            return Status.Success;
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
        EntityManager.instance.DestroyNPC(this.GameObject);
        EntitySpawner.instance.amount--; 
        if (OccupiedArcadeMachine.Value != null)
        {
            if(WorldInteractables.instance.allAracadeMachines.Contains(OccupiedArcadeMachine.Value)) 
                WorldInteractables.instance.allAracadeMachines.Remove(OccupiedArcadeMachine.Value);
            
            if(WorldInteractables.instance.availableArcadeMachines.Contains(OccupiedArcadeMachine.Value))
                WorldInteractables.instance.availableArcadeMachines.Remove(OccupiedArcadeMachine.Value);
        }
    }
}
