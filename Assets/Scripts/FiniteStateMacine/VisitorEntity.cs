using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The entity that is part of a farm. This entity switches between being idle and walking randomly
/// and is mainly for decoration.
/// </summary>
public class VisitorEntity : Entity
{
    #region Variables

    #region States

    private State idleState;
    private State randomWalkState;
    private State findMachineState;
    private State playState; 
    
    #endregion States

    #region State Variables
    
    [Tooltip("The radius in which the agent should walk.")]
    [SerializeField] private float maxWalkRadius;

    public bool invitedToLounge;
    public GameObject machineInUse;
    public GameObject seatInUse;
    public bool shouldUseSeat = false; 
    public HeadTracking headTracking;

    public int walkAmount = 10; 
    public int currentWalkAmount = 0; 
    
    public CheckIfInBuilding checkIfInBuilding;
    #endregion State Variables

    #endregion Variables

    #region Unity Method

    /// <summary>
    /// Calls the <see cref="Initialize"/> method and the Start method on the derived <see cref="Entity"/> class.
    /// </summary>
    private new void Start()
    {
        Initialize();
        base.Start();
    }

    #endregion Unity Method

    #region Initialization

    /// <summary>
    /// Gets a reference to the <see cref="PositionGenerator"/> in the parent gameObject.
    /// Begins the state creation process.
    /// </summary>
    private void Initialize()
    {
        CreateStates();
        CreateTransitions();
        initialState = idleState;
        walkAmount = Random.Range(30, 60);
    }

    /// <summary>
    /// Creates an instance of each state this entity could potentially transition to.
    /// Gives their constructors the required variables.
    /// </summary>
    private void CreateStates()
    {
        idleState = new IdleState(this);
        randomWalkState = new RandomWalkState(this, this, maxWalkRadius);
        findMachineState = new FindMachineState(this, this);
        playState = new PlayState(this, this);
    }

    /// <summary>
    /// Creates instances of the transition class and fills its List with possible <see cref="Transition"/>'s for each state.
    /// Then hands the created transitions to the appropriate states.
    /// </summary>
    private void CreateTransitions()
    {
        // IdleState transition
        List<Transition> idleTransitions = new List<Transition>
        {
            new Transition(() => { return invitedToLounge && WorldInteractables.instance.availableArcadeMachines.Count > 0; }, findMachineState),
            new Transition(() => { return true; }, randomWalkState),
        };
        idleState.Transitions = idleTransitions;

        // WalkState transition
        List<Transition> randomWalkTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, idleState),
        };
        randomWalkState.Transitions = randomWalkTransitions;
        
        // GoToMachineState transition
        List<Transition> findMachineTransitions = new List<Transition>
        {
            new Transition(() => { return machineInUse == null || shouldUseSeat && seatInUse == null; }, idleState),
            new Transition(() => { return Vector3.Distance(agent.destination, transform.position) < 1.5f; }, playState),
        };
        findMachineState.Transitions = findMachineTransitions;

        // GoToMachineState transition
        List<Transition> playTransitions = new List<Transition>
        {
            new Transition(() => { return machineInUse == null || seatInUse == null && shouldUseSeat; }, randomWalkState),
            new Transition(() => { return true; }, randomWalkState),
        };
        playState.Transitions = playTransitions;
    }

    #endregion Initialization
}