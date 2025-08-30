using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// Represents the state where the entity walks to a destination and plays a walking animation.
/// </summary>
public class WalkToDestinationState : State
{
    #region Variables

    [Header("Walk to Destination Settings")]
    // Reference to the VisitorEntity for easier access to its properties
    private VisitorEntity visitorEntity;

    [Tooltip("Time the entity will walk before the walk is considered complete.")]
    [SerializeField] private float walkTime = 30f; // Maximum time the entity will walk
    private float currentWalkTime; // Tracks the current walk time

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Constructs a new instance of the WalkToDestinationState.
    /// </summary>
    /// <param name="entity">The entity that will be controlled by this state.</param>
    /// <param name="visitorEntity">The specific visitor entity for behavior customization.</param>
    public WalkToDestinationState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when the entity enters this state. Initializes walk settings and starts walking.
    /// </summary>
    public override void EnterState()
    {
        Initialize(); // Set up initial state conditions
    }

    /// <summary>
    /// Called every frame while the entity is in this state. Updates walking time and checks for state changes.
    /// </summary>
    public override void UpdateState()
    {
        walkTime -= Time.deltaTime; // Decrease walk time as the entity walks
        if (walkTime < 0)
        {
            visitorEntity.walkToDestinationIsOver = true; // Set walk completion flag when time is up
        }
        
        CheckSwitchState(); // Check if it's time to transition to another state
    }

    /// <summary>
    /// Called when the entity exits this state. Resets walk completion flag.
    /// </summary>
    public override void ExitState()
    {
        visitorEntity.walkToDestinationIsOver = false; // Reset walking completion status
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Initializes the walking behavior, sets the destination, and plays walking animation.
    /// </summary>
    private void Initialize()
    {
        currentWalkTime = walkTime; // Initialize the current walk time
        entity.Agent.isStopped = false; // Ensure the agent is not stopped
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 1.0f);

        if (visitorEntity.gameObject != null)
        {
            // If the visitor has a conversation partner, set destination to their position
            if (visitorEntity.conversationPartner != null)
            {
                entity.Agent.SetDestination(visitorEntity.conversationPartner.transform.position);
            }
            // If the visitor has a destination object, set destination to that object
            else if (visitorEntity.gameObjectToWalkTo != null)
            {
                // Play walking animation
                entity.Agent.SetDestination(visitorEntity.gameObjectToWalkTo.transform.position);
            }
            // Default to the entity's current position if no destination is provided
            else
            {
                entity.Agent.SetDestination(entity.transform.position);
            }
        }
    }

    #endregion Methods
}
