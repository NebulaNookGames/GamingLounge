using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Finds an available arcade machine and sets the agent's destination accordingly.
/// </summary>
public class FindMachineState : State
{
    #region Variables
    
    private VisitorEntity visitorEntity;
    
    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the FindMachineState with the associated visitor entity and base entity.
    /// </summary>
    /// <param name="visitorEntity">The visitor entity searching for a machine.</param>
    /// <param name="entity">The base entity reference.</param>
    public FindMachineState(VisitorEntity visitorEntity, Entity entity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when entering the state. Ensures the agent is active and begins searching for an arcade machine.
    /// </summary>
    public override void EnterState()
    {
        if (entity.Agent.isOnNavMesh)
            entity.Agent.isStopped = false;
        
        Initialize();
    }

    /// <summary>
    /// Checks if the state should transition.
    /// </summary>
    public override void UpdateState()
    {
        CheckSwitchState();
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Finds an available arcade machine and sets the visitor's destination accordingly.
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
                {
                    CheckSwitchState();
                }
                else
                {
                    GameObject machineInUse = WorldInteractables.instance.availableArcadeMachines[randomMachineIndex]
                        .gameObject;
                    visitorEntity.machineInUse = machineInUse;
                    WorldInteractables.instance.OccupyAradeMachine(machineInUse);

                    // Check if the machine has a valid seat and set the destination accordingly.
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
                        {
                            destination = machineInUse.GetComponent<UsagePositionStorage>().usagePosition.position;
                        }
                    }
                    else
                    {
                        destination = machineInUse.GetComponent<UsagePositionStorage>().usagePosition.position;
                    }
                    
                    entity.GetComponent<NavMeshAgent>().SetDestination(destination);
                    entity.EntityAnimator.SetFloat("HorizontalSpeed", 1);
                }
            }
            else
            {
                CheckSwitchState();
            }
        }
    }

    #endregion Methods
}
