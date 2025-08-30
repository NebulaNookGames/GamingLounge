using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Finds a random position on the NavMesh within a specified radius from this entity and makes the entity walk to that position.
/// The state switches when either the walk duration has elapsed or the destination has been reached.
/// </summary>
public class RandomWalkState : State
{
    #region Variables

    [Header("Walk Settings")]
    private float waitTimeUntilAutoEnd = 10f;  // Time after which the state will end automatically if no other condition is met
    private float currentWaitTimeUntilAutoEnd = 0;

    private const float DistanceWhenDestinationReached = 2;  // The acceptable distance from the destination to consider it reached

    private float walkRadius;  // Maximum radius to choose the walk position from the entity

    private Vector3 walkPosition;  // Position on the NavMesh to walk to

    private bool leaveEffectSpawned = false;  // Flag to track if the leave effect has been spawned

    private VisitorEntity visitorEntity;  // Reference to the associated VisitorEntity

    private float updateInterval = 0.5f;  // How frequently we check for updates
    private float updateTimer = 0;

    private EffectSpawner effectSpawner;  // Reference to the effect spawner component

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the RandomWalkState with the provided entity, visitor entity, and walk radius.
    /// </summary>
    public RandomWalkState(Entity entity, VisitorEntity visitorEntity, float totalRandomWalkRadius) : base(entity)
    {
        this.visitorEntity = visitorEntity;
        walkRadius = totalRandomWalkRadius;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when the state is entered. Initializes the agent and starts the random walk.
    /// </summary>
    public override void EnterState()
    {
        if (entity.Agent.isOnNavMesh)
            entity.Agent.isStopped = false;
        
        effectSpawner = entity.GetComponent<EffectSpawner>();
        updateTimer = updateInterval;

        entity.EntityAnimator.SetFloat("HorizontalSpeed", 1);  // Set animation speed for walking
        
        Initialize();  // Start the walk state initialization
    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// Called every frame while the state is active. Tracks the progress of the walk and handles state transitions.
    /// </summary>
    public override void UpdateState()
    {
        // Check if the agent's path is invalid or partial and switch state if so
        if (entity.Agent.pathStatus == NavMeshPathStatus.PathInvalid || entity.Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            CheckSwitchState();
        }

        // Automatically end the state if the wait time expires
        if (currentWaitTimeUntilAutoEnd > 0)
        {
            currentWaitTimeUntilAutoEnd -= Time.deltaTime;
        }
        else
        {
            CheckSwitchState();
        }

        // Update the timer to check for distance every few seconds
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval;

            // Check if the entity is close enough to the walk position
            if (Vector3.Distance(entity.transform.position, walkPosition) <= DistanceWhenDestinationReached)
            {
                // Check if the visitor has walked enough or not
                if (visitorEntity.currentWalkAmount >= visitorEntity.walkAmount)
                {
                    HandleLeave();  // Handle the logic for leaving the area
                }
                else if (entity.Agent.pathStatus == NavMeshPathStatus.PathPartial || entity.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    CheckSwitchState();
                }
                else
                {
                    CheckSwitchState();
                }
            }
        }
    }

    /// <summary>
    /// Called when the state is exited. Increments the walk amount for the visitor.
    /// </summary>
    public override void ExitState()
    {
        visitorEntity.currentWalkAmount++;
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Initializes the random walk by setting the destination and starting the agent's movement.
    /// </summary>
    private void Initialize()
    {
        // Only walk to a random position if the visitor hasn't walked enough or isn't invited to the lounge
        if (visitorEntity.currentWalkAmount >= visitorEntity.walkAmount)
            walkPosition = RandomNavSphere(entity.transform.position, walkRadius, -1, false);
        else if(!visitorEntity.invitedToLounge)
            walkPosition = RandomNavSphere(entity.transform.position, walkRadius, -1, false);

        else
            walkPosition = RandomNavSphere(entity.transform.position, walkRadius, -1, true);

        // Set the agent's destination if it's on the NavMesh
        if (entity.Agent.isOnNavMesh)
        {
            entity.EntityAnimator.SetFloat("HorizontalSpeed", 1);  // Set animation speed for walking
            entity.Agent.SetDestination(walkPosition);
        }

        currentWaitTimeUntilAutoEnd = waitTimeUntilAutoEnd;  // Set the time before auto-ending the state

        // Check if the path is valid before starting the walk
        if (entity.Agent.pathStatus == NavMeshPathStatus.PathInvalid || entity.Agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            CheckSwitchState();
        }
    }

    /// <summary>
    /// Handles the logic for when the entity leaves the area after completing the walk.
    /// </summary>
    private void HandleLeave()
    {
        // Remove the machine from relevant lists if in use
        if (visitorEntity.machineInUse != null)
        {
            if (WorldInteractables.instance.allAracadeMachines.Contains(visitorEntity.machineInUse))
                WorldInteractables.instance.allAracadeMachines.Remove(visitorEntity.machineInUse);

            if (WorldInteractables.instance.availableArcadeMachines.Contains(visitorEntity.machineInUse))
                WorldInteractables.instance.availableArcadeMachines.Remove(visitorEntity.machineInUse);
        }

        // Stop the agent and destroy the NPC
        entity.GetComponent<NavMeshAgent>().isStopped = true;
        EntityManager.instance.DestroyNPC(entity.gameObject);
        EntitySpawner.instance.amount--;

        // Spawn the leave effect if it hasn't been spawned yet
        if (!leaveEffectSpawned)
        {
            visitorEntity.SpawnObject(visitorEntity.goingHomeEffect);
            leaveEffectSpawned = true;
            effectSpawner.SpawnEffect();
        }
    }

    /// <summary>
    /// Finds a position on the NavMesh within the specified radius and layer mask.
    /// </summary>
    /// <param name="origin">The origin position from which to search.</param>
    /// <param name="dist">The radius within which to search.</param>
    /// <param name="layermask">The layer mask to use for the search.</param>
    /// <param name="onlyInBuilding">Whether to only search inside buildings.</param>
    /// <returns>A valid position on the NavMesh, or the origin if no valid position is found.</returns>
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, bool onlyInBuilding)
    {
        Vector3 lastPosition = visitorEntity.transform.position;

        int maxTries = 50;  // Limit the number of attempts to find a valid position

        for (int i = 0; i < maxTries; i++)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;

            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
            {
                lastPosition = navHit.position;
                visitorEntity.checkIfInBuilding.transform.position = navHit.position;
                bool inBuilding = visitorEntity.checkIfInBuilding.IsInBuilding();

                // If we've found a valid position, check the building condition
                if (i >= maxTries)
                {
                    return navHit.position;
                }

                if (inBuilding && onlyInBuilding)
                {
                    return navHit.position;
                }

                if (!inBuilding && !onlyInBuilding)
                {
                    return navHit.position;
                }
            }
        }
        return lastPosition;  // Return the original position if no valid position was found
    }

    #endregion Methods
}
