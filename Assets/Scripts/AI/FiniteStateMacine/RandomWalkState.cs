using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Finds a random position on the navmesh in a certain range from this entity and makes the entity walk to that position.
/// Switches state once the walk duration or the destination has been reached.
/// </summary>
public class RandomWalkState : State
{
    #region Variables

    private float waitTimeUntilAutoEnd = 10f;
    private float currentWaitTimeUntilAutoEnd = 0; 
    
    /// <summary>
    /// Distance to destination when reached.
    /// </summary>
    private const float DistanceWhenDestinationReached = 2;
    
    /// <summary>
    /// The maximum radius in which a position to walk to is chosen.
    /// </summary>
    private float walkRadius;

    /// <summary>
    /// The position on the navmesh that the entity will walk to.
    /// </summary>
    private Vector3 walkPosition;

    private bool leaveEffectSpawned = false; 
    
    private VisitorEntity visitorEntity;

    private float updateInterval = .5f;
    private float updateTimer = 0;

    private EffectSpawner effectSpawner; 
    
    #endregion Variables

    #region Constructor

    public RandomWalkState(Entity entity, VisitorEntity visitorEntity, float totalRandomWalkRadius) : base(entity)
    {
        this.visitorEntity = visitorEntity;
        walkRadius = totalRandomWalkRadius;
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        if (entity.Agent.isOnNavMesh)
            entity.Agent.isStopped = false;

        entity.EntityAnimator.SetFloat("HorizontalSpeed", 1);
        effectSpawner = entity.GetComponent<EffectSpawner>();
        updateTimer = updateInterval;
        
        Initialize();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void UpdateState()
    {
        if (currentWaitTimeUntilAutoEnd > 0)
        {
            currentWaitTimeUntilAutoEnd -= Time.deltaTime;
        }
        else
        {
            CheckSwitchState();
        }

        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateInterval; 
            
            if (Vector3.Distance(entity.transform.position, walkPosition) <= DistanceWhenDestinationReached)
            {
                if (visitorEntity.currentWalkAmount >= visitorEntity.walkAmount)
                {
                    if (visitorEntity.machineInUse != null)
                    {
                        if (WorldInteractables.instance.allAracadeMachines.Contains(visitorEntity.machineInUse))
                            WorldInteractables.instance.allAracadeMachines.Remove(visitorEntity.machineInUse);

                        if (WorldInteractables.instance.availableArcadeMachines.Contains(visitorEntity.machineInUse))
                            WorldInteractables.instance.availableArcadeMachines.Remove(visitorEntity.machineInUse);
                    }

                    entity.GetComponent<NavMeshAgent>().isStopped = true;
                    EntityManager.instance.DestroyNPC(entity.gameObject);
                    EntitySpawner.instance.amount--;

                    if (!leaveEffectSpawned)
                    {
                        leaveEffectSpawned = true;
                        effectSpawner.SpawnEffect();
                    }
                }
                else if (entity.Agent.pathStatus == NavMeshPathStatus.PathPartial ||
                         entity.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                {
                    CheckSwitchState();
                }
                else
                    CheckSwitchState();
            }
        }
    }

    public override void ExitState()
    {
        visitorEntity.currentWalkAmount++;
    }
    

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Sets the required variables to their initial values.
    /// Finds a position through a static <see cref="Utility"/> method.
    /// </summary>
    private void Initialize()
    {
        if(visitorEntity.currentWalkAmount >= visitorEntity.walkAmount || !visitorEntity.invitedToLounge)
            walkPosition = RandomNavSphere(entity.transform.position, walkRadius, -1, false);
        else
            walkPosition = RandomNavSphere(entity.transform.position, walkRadius, -1, true);
        
        if (entity.Agent.isOnNavMesh)
            entity.Agent.SetDestination(walkPosition);
        
        currentWaitTimeUntilAutoEnd = waitTimeUntilAutoEnd;
    }
    
    /// <summary>
    /// Finds a position on the navmesh.
    /// </summary>
    /// <param name="origin"></param> The position from which to search a position from.
    /// <param name="dist"></param> The maximum distance to use as search radius.
    /// <param name="layermask"></param> The layermask with which to search for a navmesh position.
    /// <returns></returns> A position on the navmesh, if one is found. If not Vector3.zero is returned.
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask, bool onlyInBuilding)
    {
        Vector3 lastPosition = visitorEntity.transform.position;
        
        int maxTries = 50;

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
        return lastPosition; // Return origin instead of Vector3.zero to indicate failure.
    }

    #endregion Methods
}