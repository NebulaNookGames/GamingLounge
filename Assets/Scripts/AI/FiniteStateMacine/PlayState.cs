using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class PlayState : State
{
    #region Variables

    private bool doBikeAnim;
    private bool didBikePosChange;
    private float currentBikeWaitTime;
    private Vector3 bikeOffset = new Vector3(.76f, .05f, 0);
    private int initialAvoidancePriority; 
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
        if (doBikeAnim && !didBikePosChange)
        {
            currentBikeWaitTime-= Time.deltaTime;
            if (currentBikeWaitTime <= 0)
            {
                visitorEntity.mesh.transform.localPosition += bikeOffset;
                didBikePosChange = true; 
            }
        }
        
        if (currentPlayDuration < playDuration)
            currentPlayDuration += Time.deltaTime;
        else // Animation has run at least once
            CheckSwitchState(); // Contained in Base class.
        
        if(visitorEntity.machineInUse == null || visitorEntity.seatInUse == null && visitorEntity.shouldUseSeat)
            CheckSwitchState();
    }

    public override void ExitState()
    {
        if (visitorEntity.machineInUse != null)
        {
            WorldInteractables.instance.availableArcadeMachines.Add(visitorEntity.machineInUse);
            visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(false);
        }
        
        entity.Agent.isStopped = false;
        entity.EntityAnimator.SetBool("InteractArcadeMachine", false);
        entity.EntityAnimator.SetBool("SitChair", false);
        entity.EntityAnimator.SetBool("SitBike", false);
        entity.EntityAnimator.SetBool("SitRace", false);
        entity.EntityAnimator.SetBool("Interact", false);
        
        if(visitorEntity.seatInUse != null)
            visitorEntity.seatInUse.GetComponent<SitPositionAvailability>().available = true;      

        if (doBikeAnim)
        {
            visitorEntity.machineInUse.GetComponentInChildren<HandleAnimationStarting>().StopAnimation("Play");
            visitorEntity.mesh.transform.localPosition -= bikeOffset;
            doBikeAnim = false;
            didBikePosChange = false; 
        }

        visitorEntity.shouldUseSeat = false;
        visitorEntity.seatInUse = null;

        visitorEntity.machineInUse = null; 
        visitorEntity.headTracking.noTracking = false;
        entity.Agent.isStopped = false; 
        entity.Agent.avoidancePriority = initialAvoidancePriority; 
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        entity.Agent.isStopped = true; 
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
                visitorEntity.machineInUse.GetComponentInChildren<HandleAnimationStarting>().PlayAnimation(visitorEntity.bikeWaitTime, "Play");
                entity.EntityAnimator.SetBool("SitBike", true);
                currentBikeWaitTime = visitorEntity.bikeWaitTime;
                doBikeAnim = true;
                break;
            case MachineType.Race:
                entity.EntityAnimator.SetBool("SitRace", true);
                break;
            default:
                if (visitorEntity.seatInUse && visitorEntity.shouldUseSeat)
                {
                    entity.EntityAnimator.SetBool("SitChair", true);
                }
                else
                    entity.EntityAnimator.SetBool("Interact", true);
                break;
        }
        
        visitorEntity.machineInUse.GetComponent<MoneyHolder>().ChangeMoney(visitorEntity.machineInUse.GetComponent<Cost>().GetCost(), true, true, true);
        visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(true);
        playDuration = Random.Range(10, 30);
        currentPlayDuration = 0;
        initialAvoidancePriority = entity.Agent.avoidancePriority;
        entity.Agent.avoidancePriority = 0;
    }
    #endregion Methods
}