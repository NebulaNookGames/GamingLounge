using UnityEngine;

public class TalkState : State
{
    #region Variables

    private float idleDuration;
    public VisitorEntity visitorEntity;
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
        {
            currentIdleTime += Time.deltaTime;
        }
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

    private void Initialization()
    {
        // Set the entity's animator to idle and face the conversation partner
        entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
        Vector3 directionToPartner = (visitorEntity.conversationPartner.transform.position - entity.transform.position).normalized;
        entity.transform.rotation = Quaternion.LookRotation(directionToPartner, Vector3.up);

        // Play appropriate animation based on talk index
        if (visitorEntity.talkIndex == 0)
        {
            entity.EntityAnimator.SetTrigger("Talk01");
        }
        else
        {
            entity.EntityAnimator.SetTrigger("Talk02");
        }

        // Stop the agent's movement
        entity.Agent.velocity = Vector3.zero;

        // Randomize idle duration for a more natural feel
        idleDuration = Random.Range(10f, 20f); // Random time between 10 and 20 seconds
        currentIdleTime = 0;

        // Spawn the talk effect from the object pool
        ObjectPool.instance.SpawnTalkEffect(visitorEntity.transform.position, visitorEntity.transform.rotation);

        // Set head tracking target
        if (visitorEntity.conversationPartner != null)
        {
            visitorEntity.headTracking.specificTarget = visitorEntity.conversationPartner.transform.GetComponentInChildren<PointOfInterest>();
        }
    }

    #endregion Methods
}
