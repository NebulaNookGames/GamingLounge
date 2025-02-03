using UnityEngine;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class IdleState : State
{
    #region Variables

    /// <summary>
    /// The time until the idle state will end.
    /// </summary>
    private float idleDuration;

    /// <summary>
    /// The time the idle state has been active.
    /// </summary>
    private float currentIdleTime;

    #endregion Variables

    #region Constructor

    public IdleState(Entity entity) : base(entity)
    {
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        entity.Agent.isStopped = true;

        Initialization();
    }

    public override void UpdateState()
    {
        if (currentIdleTime < idleDuration)
            currentIdleTime += Time.deltaTime;
        else // Animation has run at least once
            CheckSwitchState(); // Contained in Base class.
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
        entity.Agent.velocity = Vector3.zero;

        idleDuration = Random.Range(2, 8);
        currentIdleTime = 0;
    }

    #endregion Methods
}