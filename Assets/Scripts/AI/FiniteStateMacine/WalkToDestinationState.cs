using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/// <summary>
/// Sets the agents destination to the <see cref="destination"/> variable and plays a animation.
/// </summary>
public class WalkToDestinationState : State
{
    #region Variables

    private VisitorEntity visitorEntity;

    private float walkTime = 30f;
    private float currentWalkTime; 
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
        walkTime -= Time.deltaTime;
        if (walkTime < 0)
        {
            visitorEntity.walkToDestinationIsOver = true; 
        }
        
        CheckSwitchState();
    }

    public override void ExitState()
    {
        visitorEntity.walkToDestinationIsOver = false;
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Sets the required variables to their initial values.
    /// Plays a animation.
    /// </summary>
    private void Initialize()
    {
        currentWalkTime = walkTime; 
        entity.Agent.isStopped = false;
        if (visitorEntity.gameObject != null)
        {
            if(visitorEntity.conversationPartner != null)
                entity.Agent.SetDestination(visitorEntity.conversationPartner.transform.position);
            else if(visitorEntity.gameObjectToWalkTo != null)
                entity.Agent.SetDestination(visitorEntity.gameObjectToWalkTo.transform.position);
            else 
                entity.Agent.SetDestination(entity.transform.position);
            
            entity.EntityAnimator.SetFloat("HorizontalSpeed", 1.0f);
        }
    }
    
    #endregion Methods
}