using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sets the agents destination to the <see cref="destination"/> variable and plays a animation.
/// </summary>
public class FindMachineState : State
{
    private VisitorEntity visitorEntity;
    
    #region Constructor

    public FindMachineState(VisitorEntity visitorEntity, Entity entity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        if (entity.Agent.isOnNavMesh)
            entity.Agent.isStopped = false;
        
        Initialize();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Sets the required variables to their initial values.
    /// Plays a animation.
    /// </summary>
    private void Initialize()
    {
        Vector3 destination = Vector3.zero; 
        
        if (WorldInteractables.instance.availableArcadeMachines.Count > 0)
        {
            int randomMachineIndex = Random.Range(0, WorldInteractables.instance.availableArcadeMachines.Count);

            if (WorldInteractables.instance.availableArcadeMachines[randomMachineIndex] != null)
            {
                NavMeshPath testPath = new NavMeshPath();
                entity.GetComponent<NavMeshAgent>().CalculatePath(
                    WorldInteractables.instance.availableArcadeMachines[randomMachineIndex].transform.position,
                    testPath);
                if (testPath.status == NavMeshPathStatus.PathPartial)
                    CheckSwitchState();
                else
                {
                    GameObject machineInUse = WorldInteractables.instance.availableArcadeMachines[randomMachineIndex]
                        .gameObject;
                    visitorEntity.machineInUse = machineInUse;
                    WorldInteractables.instance.OccupyAradeMachine(machineInUse);

                    if (machineInUse.GetComponentsInChildren<SitPositionRecognition>().Length > 0)
                    {
                        if (machineInUse.GetComponentsInChildren<SitPositionRecognition>()[0].validObjects.Count > 0)
                        {
                            GameObject seat = machineInUse.GetComponentsInChildren<SitPositionRecognition>()[0].GetSitPosition();
                            destination = seat.transform.position - seat.transform.forward.normalized * 0.7f;
                            visitorEntity.seatInUse = seat;
                            visitorEntity.shouldUseSeat = true;
                        }
                        else
                            destination = machineInUse.GetComponent<UsagePositionStorage>().usagePosition.position;
                    }
                    else
                        destination = machineInUse.GetComponent<UsagePositionStorage>().usagePosition.position;
                    
                    entity.GetComponent<NavMeshAgent>()
                        .SetDestination(destination);
                    
                    entity.EntityAnimator.SetFloat("HorizontalSpeed", 1);
                }
            }
            else CheckSwitchState();
        }
    }

    #endregion Methods
}