using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// Sets the agents destination to the <see cref="destination"/> variable and plays a animation.
/// </summary>
public class WalkToDestinationState : State
{
    #region Variables

    private VisitorEntity visitorEntity; 
    
    #endregion Variables

    #region Constructor

    public WalkToDestinationState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
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
        if (visitorEntity.gameObject != null)
        {
            entity.Agent.SetDestination(visitorEntity.gameObjectToWalkTo.transform.position);
            entity.EntityAnimator.SetFloat("HorizontalSpeed", 1.0f);
        }
    }

   

    #endregion Methods
}