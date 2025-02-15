using UnityEngine;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class PlayState : State
{
    #region Variables

    /// <summary>
    /// The time until the idle state will end.
    /// </summary>
    private float playDuration;

    public VisitorEntity visitorEntity;
  
    /// <summary>
    /// The time the idle state has been active.
    /// </summary>
    private float currentPlayDuration;

    #endregion Variables

    #region Constructor

    public PlayState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity; 
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        entity.Agent.isStopped = true;
        visitorEntity.headTracking.noTracking = true;
        Initialization();
    }

    public override void UpdateState()
    {
        if (currentPlayDuration < playDuration)
            currentPlayDuration += Time.deltaTime;
        else // Animation has run at least once
            CheckSwitchState(); // Contained in Base class.
        
        if(visitorEntity.machineInUse == null || visitorEntity.seatInUse == null && visitorEntity.shouldUseSeat)
            CheckSwitchState();
    }

    public override void ExitState()
    {
        entity.Agent.isStopped = false;
        entity.EntityAnimator.SetBool("InteractArcadeMachine", false);
        entity.EntityAnimator.SetBool("SitChair", false);
        entity.EntityAnimator.SetBool("SitBike", false);
        entity.EntityAnimator.SetBool("SitRace", false);
        entity.EntityAnimator.SetBool("Interact", false);
        
        if(visitorEntity.seatInUse != null)
            visitorEntity.seatInUse.GetComponent<SitPositionAvailability>().available = true;      
        
        visitorEntity.shouldUseSeat = false;
        visitorEntity.seatInUse = null;

        if (visitorEntity.machineInUse != null)
        {
            visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(false);
            WorldInteractables.instance.availableArcadeMachines.Add(visitorEntity.machineInUse);
        }

        visitorEntity.machineInUse = null; 
        visitorEntity.headTracking.noTracking = false;
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        entity.Agent.velocity = Vector3.zero;
        Vector3 aimPos = Vector3.zero;
        aimPos = entity.Agent.destination;
        entity.transform.position = new Vector3(aimPos.x, aimPos.y, aimPos.z);
        entity.transform.rotation = Quaternion.LookRotation(visitorEntity.machineInUse.GetComponent<RotatePlacementObject>().objectToRotate.transform.forward, Vector3.up);            
        
        switch (visitorEntity.machineInUse.GetComponent<UsagePositionStorage>().machineType)
        {
            case MachineType.ArcadeMachine:
                entity.EntityAnimator.SetBool("InteractArcadeMachine", true);
                break;
            case MachineType.Bike:
                entity.EntityAnimator.SetBool("SitBike", true);
                break;
            case MachineType.Race:
                entity.EntityAnimator.SetBool("SitRace", true);
                break;
            default:
                if(visitorEntity.seatInUse && visitorEntity.shouldUseSeat)
                    entity.EntityAnimator.SetBool("SitChair", true);
                else
                    entity.EntityAnimator.SetBool("Interact", true);
                break;
        }
        
        visitorEntity.machineInUse.GetComponent<MoneyHolder>().ChangeMoney(visitorEntity.machineInUse.GetComponent<Cost>().GetCost(), true, true);
        visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(true);
        playDuration = Random.Range(10, 30);
        currentPlayDuration = 0;
    }

    #endregion Methods
}