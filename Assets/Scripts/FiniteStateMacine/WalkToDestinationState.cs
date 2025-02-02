using UnityEngine;

/// <summary>
/// Sets the agents destination to the <see cref="destination"/> variable and plays a animation.
/// </summary>
public class WalkToDestinationState : State
{
    #region Variables

    /// <summary>
    /// The Vector3 this agent will walk to while in this state.
    /// </summary>
    private Vector3 destination;

    /// <summary>
    /// The name of the animation that will be played.
    /// </summary>
    private string animationName;

    /// <summary>
    /// How fast the entity rotates itself to look in the moving direction. 
    /// </summary>
    private const float ROTATIONSPEED = .2f;

    /// <summary>
    /// How fast the entity rotates aligns itself to the ground normal.
    /// </summary>
    private const float ALIGNMENTSPEED = .7f;

    #endregion Variables

    #region Constructor

    public WalkToDestinationState(Entity entity, Vector3 destination, string animationName) : base(entity)
    {
        this.destination = destination;
        this.animationName = animationName;
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        Initialize();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Sets the required variables to their initial values.
    /// Plays a animation.
    /// </summary>
    private void Initialize()
    {
        entity.Agent.isStopped = false;

        entity.Agent.SetDestination(destination);

        entity.EntityAnimator.Play(animationName);
    }

   

    #endregion Methods
}