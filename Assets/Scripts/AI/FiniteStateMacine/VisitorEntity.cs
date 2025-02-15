using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
    private State findConversationState;
    private State talkState;
    private State walkToDestinationState;
    private State findInteractableState; 
    private State lookAtInteractableState;
    private State behaviorRandomizationState; 
    
    #endregion States

    #region State Variables
    
    [Tooltip("The radius in which the agent should walk.")]
    [SerializeField] private float maxWalkRadius;

    public bool invitedToLounge;
    public GameObject machineInUse;
    public GameObject seatInUse;
    public bool shouldUseSeat = false; 
    public HeadTracking headTracking;
    public int talkIndex; 
    public int walkAmount = 10; 
    public int currentWalkAmount = 0;
    [FormerlySerializedAs("walkToDestination")] public GameObject gameObjectToWalkTo;
    public GameObject conversationPartner; 
    public CheckIfInBuilding checkIfInBuilding;
    public GameObject heartEffect;
    public int randomStateIndex = 0;
    public GameObject mesh;
    public float bikeWaitTime = 1;
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
        initialState = randomWalkState;
        walkAmount = Random.Range(20, 40);
        GetComponent<NavMeshAgent>().avoidancePriority = Random.Range(20, 80);
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
        findConversationState = new FindConversationState(this, this);
        talkState = new TalkState(this, this);
        walkToDestinationState = new WalkToDestinationState(this, this);
        findInteractableState = new FindInteractableState(this, this);
        lookAtInteractableState = new LookAtInteractableState(this, this);
        behaviorRandomizationState = new BehaviorRandomizationState(this, this);
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
            new Transition(() => 
                { 
                    return machineInUse == null || shouldUseSeat && seatInUse == null 
                                                  || agent.pathStatus == NavMeshPathStatus.PathInvalid 
                                                  || agent.pathStatus == NavMeshPathStatus.PathPartial; 
                }, 
                idleState),
            
            new Transition(() => { return Vector3.Distance(agent.destination, transform.position) < 1.5f; }, playState),
        };
        findMachineState.Transitions = findMachineTransitions;

        // GoToMachineState transition
        List<Transition> playTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, behaviorRandomizationState),
        };
        playState.Transitions = playTransitions;
        
        // walkToDestination transition
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
            new Transition(() => { return gameObjectToWalkTo == null;}, idleState),
        };
        walkToDestinationState.Transitions = walkToDestinationTransitions;
        
        // findConversation transition
        List<Transition> findConversationTransitions = new List<Transition>
        {
            new Transition(() => { return conversationPartner != null; }, walkToDestinationState),
            new Transition(() => { return conversationPartner == null; }, randomWalkState),
        };
        
        findConversationState.Transitions = findConversationTransitions;
        
        // talkState transition
        List<Transition> talkTransitions = new List<Transition>
        {
            new Transition(() => { return conversationPartner == null; }, randomWalkState),
        };
        talkState.Transitions = talkTransitions;
        
        // findInteractableState transition
        List<Transition> findInteractableTransitions = new List<Transition>
        {
            new Transition(() => { return gameObjectToWalkTo != null; }, walkToDestinationState),
            new Transition(() => { return gameObjectToWalkTo == null; }, idleState),
        };
        findInteractableState.Transitions = findInteractableTransitions;
        
        // lookAtInteractableState transition
        List<Transition> lookAtInteractableTransitions = new List<Transition>
        {
            new Transition(() => { return true; }, randomWalkState),
        };
        lookAtInteractableState.Transitions = lookAtInteractableTransitions;
        
        // behaviorRandomizationState transition
        List<Transition> behaviorRandomizationTransitions = new List<Transition>
        {
            new Transition(() => { return randomStateIndex == 0; }, findInteractableState),
            new Transition(() => { return randomStateIndex == 1; }, findConversationState),
            new Transition(() => { return randomStateIndex == 2; }, randomWalkState),
            new Transition(() => { return randomStateIndex == 3; }, idleState),
            new Transition(() => { return true; }, randomWalkState),
        };
        behaviorRandomizationState.Transitions = behaviorRandomizationTransitions;
    }

    public void SpawnObject(GameObject objectToSpawn)
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

    #endregion Initialization
}