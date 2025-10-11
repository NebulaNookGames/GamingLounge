using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// This state handles the look-at behavior for interactable objects. It plays an animation on enter,
/// and calls the CheckStateSwitch method in the base class once the animation has played for a sufficient amount of time.
/// </summary>
public class LookAtInteractableState : State
{
    #region Variables

    /// <summary>
    /// The VisitorEntity associated with this state. Used for handling the look-at behavior.
    /// </summary>
    private VisitorEntity visitorEntity; 

    /// <summary>
    /// The time until the state will end and switch.
    /// </summary>
    [SerializeField] private float idleDuration; // Time the entity will spend looking at the interactable object, exposed to inspector for easy tuning.

    /// <summary>
    /// The time the idle state has been active.
    /// </summary>
    private float currentIdleTime;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Constructor for the LookAtInteractableState, initializing the state with the given entity and visitor entity.
    /// </summary>
    /// <param name="entity">The entity associated with the state.</param>
    /// <param name="visitorEntity">The visitor entity used for specific behaviors.</param>
    public LookAtInteractableState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when the state is entered. Sets up the agent and prepares the look-at behavior.
    /// </summary>
    public override void EnterState()
    {
        entity.EntityAnimator.enabled = true;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().UnbakeMesh();
        entity.Agent.autoRepath = false;  // Disable automatic pathfinding repathing to prevent interruption
        entity.Agent.isStopped = true;    // Stop the agent's movement
        Initialization();                 // Initialize the look-at interaction and idle timer
    }

    /// <summary>
    /// Called every frame while the state is active. Updates the idle time and checks if it's time to switch states.
    /// </summary>
    public override void UpdateState()
    {
        if (currentIdleTime < idleDuration)
        {
            currentIdleTime += Time.deltaTime; // Increment idle time
        }
        else // The animation has run for the required duration, time to switch state
        {
            CheckSwitchState(); // Switch to the next state if required (base class method)
        }
    }

    /// <summary>
    /// Called when exiting the state. Resets the agent and visitor entity.
    /// </summary>
    public override void ExitState()
    {
        entity.EntityAnimator.enabled = false;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().BakeMesh();
        visitorEntity.gameObjectToWalkTo = null;  // Clear the target object to walk to
        entity.Agent.enabled = true;               // Re-enable the agent for movement
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Initializes the look-at behavior by playing an animation and choosing a random duration for idle state.
    /// </summary>
    private void Initialization()
    { 
        visitorEntity.SpawnObject(visitorEntity.heartEffect);  // Spawn the heart effect (or any effect tied to the look-at)
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);    // Set animator parameter to indicate idle state
        entity.Agent.velocity = Vector3.zero;  // Stop the agent's velocity
        entity.Agent.enabled = false;         // Disable the agent while it's in the look-at state (no movement)
        idleDuration = 2;                     // Set idle duration to 2 seconds (adjustable via inspector)
        currentIdleTime = 0;                  // Reset idle time to zero
    }

    #endregion Methods
}
