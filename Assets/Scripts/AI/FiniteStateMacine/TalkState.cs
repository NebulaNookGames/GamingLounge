using UnityEngine;

/// <summary>
/// Plays a animation on enter and calls the CheckStateSwitch method on the base class once the animation has played atleast once.
/// </summary>
public class TalkState : State
{
    #region Variables

    /// <summary>
    /// The time until the idle state will end.
    /// </summary>
    private float idleDuration;

    public VisitorEntity visitorEntity; 
    
    /// <summary>
    /// The time the idle state has been active.
    /// </summary>
    private float currentIdleTime;

    #endregion Variables

    #region Constructor

    public TalkState(Entity entity, VisitorEntity visitorEntity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
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
        {
            visitorEntity.conversationPartner = null;
            visitorEntity.gameObjectToWalkTo = null; 
            visitorEntity.headTracking.specificTarget = null;
            CheckSwitchState(); // Contained in Base class.
        }
    }
    #endregion State Methods

    #region Methods

    /// <summary>
    /// On initialization an animation is played and the time until the idle state is ended is randomly chosen.
    /// </summary>
    private void Initialization()
    {
        
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
        Vector3 directionToPartner = (visitorEntity.conversationPartner.transform.position - entity.transform.position).normalized;

        entity.transform.rotation = Quaternion.LookRotation(directionToPartner, Vector3.up);
        if (visitorEntity.talkIndex == 0)
        {
            entity.EntityAnimator.SetTrigger("Talk01");
        }
        else
        {
            entity.EntityAnimator.SetTrigger("Talk02");
        }

        entity.Agent.velocity = Vector3.zero;

        idleDuration = 16;
        currentIdleTime = 0;
        ObjectPool.instance.SpawnTalkEffect(visitorEntity.transform.position, visitorEntity.transform.rotation);
        visitorEntity.headTracking.specificTarget = visitorEntity.conversationPartner.transform.GetComponentInChildren<PointOfInterest>();
    }

    #endregion Methods
}