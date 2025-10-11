using UnityEngine;

/// <summary>
/// Plays an animation on enter and calls the CheckStateSwitch method on the base class once the animation has played at least once.
/// </summary>
public class IdleState : State
{
    #region Variables

    /// <summary>
    /// The time until the idle state will end.
    /// Randomly set between 2 and 8 seconds.
    /// </summary>
    [SerializeField] private float idleDuration; // Exposed to inspector for tuning

    /// <summary>
    /// The time the idle state has been active.
    /// Increases over time until it reaches idleDuration.
    /// </summary>
    private float currentIdleTime;
    private VisitorEntity visitorEntity;
    
    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the IdleState with an associated entity.
    /// </summary>
    /// <param name="entity">The entity that this state is associated with.</param>
    public IdleState(Entity entity, VisitorEntity _visitorEntity) : base(entity)
    {
        visitorEntity = _visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when the state is entered. Stops the agent and initializes the idle state.
    /// </summary>
    public override void EnterState()
    {
        entity.EntityAnimator.enabled = true;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().BakeMesh();
        entity.Agent.isStopped = true;  // Ensure the agent stops moving
        Initialization();              // Initialize idle state (e.g., play animation)
    }

    /// <summary>
    /// Called every frame while the state is active.
    /// Increases the currentIdleTime and checks if it's time to switch state.
    /// </summary>
    public override void UpdateState()
    {
        if (currentIdleTime < idleDuration)
        {
            currentIdleTime += Time.deltaTime; // Accumulate time spent in idle state
        }
        else // Animation has run at least once, time to potentially switch state
        {
            CheckSwitchState(); // Call the CheckSwitchState method from the base class
        }
    }

    /// <summary>
    /// Called when the state is exited. Re-enables the agent for movement.
    /// </summary>
    public override void ExitState()
    {
        entity.EntityAnimator.enabled = false;
        entity.Agent.enabled = true; // Re-enable the agent to allow movement
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Initializes the idle state by setting the animator parameters and choosing a random duration.
    /// </summary>
    private void Initialization()
    {
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0); // Set animation parameter to indicate idle state
        entity.Agent.velocity = Vector3.zero;  // Stop the agent's movement
        entity.Agent.enabled = false;  // Disable the agent while idle to prevent movement
        idleDuration = Random.Range(2, 8); // Random duration for idle state (between 2 and 8 seconds)
        currentIdleTime = 0; // Reset the time counter for idle duration
    }

    #endregion Methods
}
