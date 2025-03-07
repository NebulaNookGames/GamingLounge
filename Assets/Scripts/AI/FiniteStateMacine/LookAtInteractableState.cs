using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class LookAtInteractableState : State
{
    #region Variables

    private VisitorEntity visitorEntity; 
    
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

    public LookAtInteractableState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        entity.Agent.autoRepath = false; 
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

    public override void ExitState()
    {
        visitorEntity.gameObjectToWalkTo = null;
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    { 
        visitorEntity.SpawnObject(visitorEntity.heartEffect);
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
        entity.Agent.velocity = Vector3.zero;

        idleDuration = 2;
        currentIdleTime = 0;
    }

    #endregion Methods
}