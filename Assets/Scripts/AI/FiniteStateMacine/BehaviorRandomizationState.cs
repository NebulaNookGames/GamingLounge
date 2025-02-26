using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class BehaviorRandomizationState : State
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

    public BehaviorRandomizationState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    public override void EnterState()
    {
        Initialization();
    }

    public override void UpdateState()
    {
        CheckSwitchState(); // Contained in Base class.
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        visitorEntity.randomStateIndex = UnityEngine.Random.Range(0, 4);
    }

    #endregion Methods
}