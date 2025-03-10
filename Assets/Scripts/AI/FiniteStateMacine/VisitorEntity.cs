using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

/// <summary>
/// The entity that represents a visitor on the farm, capable of performing various actions
/// such as walking randomly, interacting with machines, engaging in conversation, and more.
/// </summary>
public class VisitorEntity : Entity
{
    #region Variables

    #region States

    [Header("States")]
    // Declaring the states the entity can be in
    private State idleState;
    private State randomWalkState;
    private State findMachineState;
    private State playState;
    private State findConversationState;
    private State talkState;
    private State walkToDestinationState;
    private State findInteractableState; 
    private State lookAtInteractableState;
    private State behaviorRandomizationState; 
    
    #endregion States

    #region State Variables

    [Header("State Variables")]
    // General state variables for the entity's behavior
    [Tooltip("Maximum distance the entity can walk to its destination")]
    [SerializeField] private float maxWalkRadius;

    public bool invitedToLounge;              // Whether the entity has been invited to the lounge
    public GameObject machineInUse;           // Current machine the entity is interacting with
    public GameObject seatInUse;              // Seat that the entity is using (if any)
    public bool shouldUseSeat = false;        // Whether the entity should use a seat
    public HeadTracking headTracking;         // Controls the entity's head tracking behavior
    public int talkIndex;                    // The index of the current conversation
    public int walkAmount = 10;               // The number of times the entity will walk
    public int currentWalkAmount = 0;         // Current progress in walking
    [FormerlySerializedAs("walkToDestination")] public GameObject gameObjectToWalkTo; // Destination object
    public GameObject conversationPartner;    // The partner with whom the entity is conversing
    public CheckIfInBuilding checkIfInBuilding; // To check if the entity is in a building
    public GameObject heartEffect;            // The heart effect object when certain actions occur
    public int randomStateIndex = 0;          // Random state index for behavior randomization
    public GameObject mesh;                   // The entity's mesh (model)
    public Vector3 raceOffset = new Vector3(.76f, .05f, 0);  // Offset for racing-related behavior
    public float bikeWaitTime = 1;            // Time the entity waits while using a bike
    public float raceWaitTime;                // Time the entity waits for a race-related activity

    public bool walkToDestinationIsOver = false;  // Flag indicating whether walking is completed

    #endregion State Variables

    #endregion Variables

    #region Unity Methods

    /// <summary>
    /// Initializes the entity by calling the <see cref="Initialize"/> method and starting up the base class.
    /// </summary>
    private new void Start()
    {
        Initialize();  // Initialize custom properties for this entity
        base.Start();  // Start the base Entity class functionality
    }

    #endregion Unity Methods

    #region Initialization

    /// <summary>
    /// Initializes the states and state machine for the entity, as well as setting up random walking behavior.
    /// </summary>
    private void Initialize()
    {
        CreateStates();   // Create the states for the entity
        CreateTransitions();  // Set up transitions between the states
        initialState = randomWalkState;  // Set the initial state
        walkAmount = Random.Range(20, 40);  // Randomly choose how many times the entity will walk
    }

    /// <summary>
    /// Creates an instance of each possible state the entity can be in.
    /// Each state is constructed with the necessary parameters (e.g., this entity, max walk radius).
    /// </summary>
    private void CreateStates()
    {
        // Initialize states for the visitor entity
        idleState = new IdleState(this);
        randomWalkState = new RandomWalkState(this, this, maxWalkRadius);
        findMachineState = new FindMachineState(this, this);
        playState = new PlayState(this, this);
        findConversationState = new FindConversationState(this, this);
        talkState = new TalkState(this, this);
        walkToDestinationState = new WalkToDestinationState(this, this);
        findInteractableState = new FindInteractableState(this, this);
        lookAtInteractableState = new LookAtInteractableState(this, this);
        behaviorRandomizationState = new BehaviorRandomizationState(this, this);
    }

    /// <summary>
    /// Sets up all transitions between states by defining conditions that determine when the entity 
    /// switches between different states.
    /// </summary>
    private void CreateTransitions()
    {
        // IdleState transition conditions
        List<Transition> idleTransitions = new List<Transition>
        {
            new Transition(() => { return invitedToLounge && WorldInteractables.instance.availableArcadeMachines.Count > 0; }, findMachineState),
            new Transition(() => { return true; }, randomWalkState),
        };
        idleState.Transitions = idleTransitions;

        // Random walk state transition conditions
        List<Transition> randomWalkTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, idleState),  // Random transition back to idle state
        };
        randomWalkState.Transitions = randomWalkTransitions;
        
        // Transitions for finding a machine to interact with
        List<Transition> findMachineTransitions = new List<Transition>
        {
            new Transition(() => 
                { 
                    return machineInUse == null || shouldUseSeat && seatInUse == null 
                                                  || agent.pathStatus == NavMeshPathStatus.PathInvalid 
                                                  || agent.pathStatus == NavMeshPathStatus.PathPartial; 
                }, 
                behaviorRandomizationState),
            new Transition(() => { return Vector3.Distance(agent.destination, transform.position) <= .5f; }, playState),
        };
        findMachineState.Transitions = findMachineTransitions;

        // Transitions for playing state
        List<Transition> playTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, behaviorRandomizationState),
        };
        playState.Transitions = playTransitions;
        
        // Transitions for walking to destination
        List<Transition> walkToDestinationTransitions = new List<Transition>
        {
            new Transition(() =>
                {
                    return conversationPartner != null && gameObjectToWalkTo != null &&
                           Vector3.Distance(gameObjectToWalkTo.transform.position, gameObject.transform.position) < 1f;
                },
                talkState),
            new Transition(() => { return gameObjectToWalkTo != null &&
                                          Vector3.Distance(gameObjectToWalkTo.transform.position, gameObject.transform.position) < 1f; }, lookAtInteractableState),
            new Transition(() => { return gameObjectToWalkTo == null && conversationPartner == null;}, idleState),
            new Transition(() => { return walkToDestinationIsOver;}, idleState),
        };
        walkToDestinationState.Transitions = walkToDestinationTransitions;

        // Transitions for finding a conversation partner
        List<Transition> findConversationTransitions = new List<Transition>
        {
            new Transition(() => { return conversationPartner != null; }, walkToDestinationState),
            new Transition(() => { return conversationPartner == null; }, randomWalkState),
            new Transition(() => { return true; }, idleState),
        };
        findConversationState.Transitions = findConversationTransitions;
        
        // Transitions for talking state
        List<Transition> talkTransitions = new List<Transition>
        {
            new Transition(() => { return conversationPartner == null; }, randomWalkState),
            new Transition(() => { return true; }, idleState),
        };
        talkState.Transitions = talkTransitions;
        
        // Transitions for finding interactable objects
        List<Transition> findInteractableTransitions = new List<Transition>
        {
            new Transition(() => { return gameObjectToWalkTo != null; }, walkToDestinationState),
            new Transition(() => { return gameObjectToWalkTo == null; }, idleState),
            new Transition(() => { return true; }, idleState),
        };
        findInteractableState.Transitions = findInteractableTransitions;
        
        // Transitions for looking at interactables
        List<Transition> lookAtInteractableTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, randomWalkState),
        };
        lookAtInteractableState.Transitions = lookAtInteractableTransitions;
        
        // Transitions for behavior randomization
        List<Transition> behaviorRandomizationTransitions = new List<Transition>
        {
            new Transition(() => { return randomStateIndex == 0; }, findInteractableState),
            new Transition(() => { return randomStateIndex == 1; }, findConversationState),
            new Transition(() => { return randomStateIndex == 2; }, randomWalkState),
            new Transition(() => { return true; }, randomWalkState),
        };
        behaviorRandomizationState.Transitions = behaviorRandomizationTransitions;
    }

    #endregion Initialization

    #region Additional Methods

    /// <summary>
    /// Spawns a specified object at the entity's current position.
    /// </summary>
    /// <param name="objectToSpawn">The object to be spawned</param>
    public void SpawnObject(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

    #endregion Additional Methods
}
