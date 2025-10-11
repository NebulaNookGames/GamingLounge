using UnityEngine;

/// <summary>
/// Plays an animation on enter and calls the CheckStateSwitch method on the base class
/// once the animation has played at least once.
/// </summary>
public class BehaviorRandomizationState : State
{
    #region Variables

    [Header("Entity References")]
    [Tooltip("Reference to the visitor entity.")]
    private VisitorEntity visitorEntity; 
    
    [Header("Idle State Timing")]
    [Tooltip("The time until the idle state will end.")]
    private float idleDuration;
    
    [Tooltip("The time the idle state has been active.")]
    private float currentIdleTime;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the BehaviorRandomizationState class.
    /// </summary>
    /// <param name="entity">The main entity associated with this state.</param>
    /// <param name="visitorEntity">The visitor entity reference.</param>
    public BehaviorRandomizationState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called when entering the state. Initializes necessary variables.
    /// </summary>
    public override void EnterState()
    {
        Initialization();
    }

    /// <summary>
    /// Called every frame to update the state. Checks for state transitions.
    /// </summary>
    public override void UpdateState()
    {
        CheckSwitchState(); // Contained in Base class.
    }

    public override void ExitState()
    {
        entity.EntityAnimator.enabled = false;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().BakeMesh();
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization, an animation is played, and the time until the idle state ends is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        entity.EntityAnimator.enabled = true;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().UnbakeMesh();
        visitorEntity.randomStateIndex = UnityEngine.Random.Range(0, 5);
    }

    #endregion Methods
}