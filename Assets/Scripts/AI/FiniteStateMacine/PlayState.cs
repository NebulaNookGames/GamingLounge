using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the play state of a visitor interacting with various machines (e.g., arcade machine, bike, race).
/// Handles animations, position adjustments, and state transitions after a play duration.
/// </summary>
public class PlayState : State
{
    #region Variables

    /// <summary>
    /// Flags to determine if bike animation or race animation should be played.
    /// </summary>
    private bool doBikeAnim;
    private bool doRaceAnim;

    /// <summary>
    /// Flags to track if the position change for bike or race animation has occurred.
    /// </summary>
    private bool didBikePosChange;
    private bool didRacePosChange;

    /// <summary>
    /// The time remaining before the bike animation position change occurs.
    /// </summary>
    private float currentBikeWaitTime;

    /// <summary>
    /// The time remaining before the race animation position change occurs.
    /// </summary>
    private float currentRaceWaitTime;

    /// <summary>
    /// Offset for positioning the visitor during the bike animation.
    /// </summary>
    private Vector3 bikeOffset = new Vector3(.76f, .05f, 0);

    /// <summary>
    /// The initial avoidance priority for the agent.
    /// </summary>
    private int initialAvoidancePriority;

    /// <summary>
    /// The total play duration for the state, randomly selected at the start.
    /// </summary>
    [SerializeField] private float playDuration;

    /// <summary>
    /// The associated VisitorEntity for managing specific animations and interactions.
    /// </summary>
    public VisitorEntity visitorEntity;

    /// <summary>
    /// The current play duration tracking the time spent in the play state.
    /// </summary>
    private float currentPlayDuration;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the PlayState with an associated entity and visitor entity.
    /// </summary>
    public PlayState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when the state is entered. Stops the agent, disables head tracking, and initializes necessary parameters.
    /// </summary>
    public override void EnterState()
    {
        entity.Agent.isStopped = true;
        if(visitorEntity.headTracking) 
            visitorEntity.headTracking.noTracking = true; // Disable head tracking during play
        
        Initialization(); // Initialize the state
    }

    /// <summary>
    /// Called every frame while the state is active. Tracks the play time, handles animation, and checks if the state should switch.
    /// </summary>
    public override void UpdateState()
    {
        // Handle bike position change if animation is running
        if (doBikeAnim && !didBikePosChange)
        {
            currentBikeWaitTime -= Time.deltaTime;
            if (currentBikeWaitTime <= 0)
            {
                visitorEntity.mesh.transform.localPosition += bikeOffset;
                didBikePosChange = true;
            }
        }
        // Handle race position change if animation is running
        else if (doRaceAnim && !didRacePosChange)
        {
            currentRaceWaitTime -= Time.deltaTime;
            if (currentRaceWaitTime <= 0)
            {
                visitorEntity.mesh.transform.localPosition += visitorEntity.raceOffset;
                didRacePosChange = true;
            }
        }

        // Track the total play time and check if it should switch to the next state
        if (currentPlayDuration < playDuration)
        {
            currentPlayDuration += Time.deltaTime;
        }
        else // Once the play duration is complete, end the occupation and switch state
        {
            WorldInteractables.instance.EndOccupation(visitorEntity.machineInUse);
            CheckSwitchState(); // Call base method to switch state
        }

        // Check if the visitor is trying to interact with an invalid machine/seat
        if (visitorEntity.machineInUse == null || visitorEntity.seatInUse == null && visitorEntity.shouldUseSeat)
        {
            WorldInteractables.instance.EndOccupation(visitorEntity.machineInUse);
            CheckSwitchState();
        }
    }

    /// <summary>
    /// Called when the state is exited. Resets all machine interactions, animations, and visitor settings.
    /// </summary>
    public override void ExitState()
    {
        WorldInteractables.instance.EndOccupation(visitorEntity.machineInUse);

        // Stop any ongoing video or animations
        if (visitorEntity.machineInUse != null)
            visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(false);

        // Reset animations and states
        entity.Agent.isStopped = false;
        ResetAnimations();

        // Reset machine and seat interactions
        visitorEntity.shouldUseSeat = false;
        visitorEntity.seatInUse = null;

        // Clear machine and visitor head tracking
        visitorEntity.machineInUse = null;
        
        if(visitorEntity.headTracking) 
            visitorEntity.headTracking.noTracking = false;
        
        entity.Agent.isStopped = false;
        entity.Agent.avoidancePriority = initialAvoidancePriority;
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Initializes the state, including animations, machine interactions, and positioning.
    /// </summary>
    private void Initialization()
    {
        entity.Agent.isStopped = true;
        entity.Agent.velocity = Vector3.zero;

        // Set position and rotation for the entity based on machine interaction
        Vector3 aimPos = entity.Agent.destination;
        entity.transform.position = new Vector3(aimPos.x, aimPos.y, aimPos.z);
        entity.transform.rotation = Quaternion.LookRotation(visitorEntity.machineInUse.GetComponent<RotatePlacementObject>().objectToRotate.transform.forward, Vector3.up);

        // Handle machine-specific interactions and animations
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
                visitorEntity.machineInUse.GetComponentInChildren<HandleAnimationStarting>().PlayAnimation(visitorEntity.raceWaitTime, "Play");
                entity.EntityAnimator.SetBool("SitRace", true);
                currentRaceWaitTime = visitorEntity.raceWaitTime;
                doRaceAnim = true;
                break;
            default:
                if (visitorEntity.seatInUse && visitorEntity.shouldUseSeat)
                    entity.EntityAnimator.SetBool("SitChair", true);
                else
                    entity.EntityAnimator.SetBool("Interact", true);
                break;
        }

        // Handle the financial transaction (machine usage cost)
        visitorEntity.machineInUse.GetComponent<MoneyHolder>().ChangeMoney(visitorEntity.machineInUse.GetComponent<Cost>().GetCost(), true, true, true);
        visitorEntity.machineInUse.GetComponentInChildren<BeginVideoPlayer>().HandlePlay(true);

        // Set random play duration for the state
        playDuration = Random.Range(10, 30);
        currentPlayDuration = 0;

        // Set initial avoidance priority for the agent
        initialAvoidancePriority = entity.Agent.avoidancePriority;
        entity.Agent.avoidancePriority = 0;
    }

    /// <summary>
    /// Resets all animation flags and positions related to the play state.
    /// </summary>
    private void ResetAnimations()
    {
        entity.EntityAnimator.SetBool("InteractArcadeMachine", false);
        entity.EntityAnimator.SetBool("SitChair", false);
        entity.EntityAnimator.SetBool("SitBike", false);
        entity.EntityAnimator.SetBool("SitRace", false);
        entity.EntityAnimator.SetBool("Interact", false);

        // Reset bike and race animation flags and offsets
        if (doBikeAnim)
        {
            if (visitorEntity.machineInUse)
                visitorEntity.machineInUse.GetComponentInChildren<HandleAnimationStarting>().StopAnimation("Play");

            visitorEntity.mesh.transform.localPosition -= bikeOffset;
            doBikeAnim = false;
            didBikePosChange = false;
        }
        else if (doRaceAnim)
        {
            if (visitorEntity.machineInUse)
                visitorEntity.machineInUse.GetComponentInChildren<HandleAnimationStarting>().StopAnimation("Play");

            visitorEntity.mesh.transform.localPosition -= visitorEntity.raceOffset;
            doRaceAnim = false;
            didRacePosChange = false;
        }
    }

    #endregion Methods
}
