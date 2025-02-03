
using UnityEngine;

public class FindConversationState : State
{
    private VisitorEntity visitorEntity;
    private float chatFindDuration = 1f;
    private float currentChatFindDuration = 0; 
    #region Constructor

    public FindConversationState(VisitorEntity visitorEntity, Entity entity) : base(entity)
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
        if (WorldInteractables.instance.openToChatEntities.Count > 1 && visitorEntity.conversationPartner == null)
        {
            for (int i = 0; i < WorldInteractables.instance.openToChatEntities.Count; i++)
            {
                if (WorldInteractables.instance.openToChatEntities[i] != entity.gameObject)
                {
                    VisitorEntity otherEntity = WorldInteractables.instance.openToChatEntities[i].GetComponent<VisitorEntity>();
                    
                    if (otherEntity.conversationPartner != null)
                        continue; 
                    
                    visitorEntity.conversationPartner = otherEntity.gameObject;
                    otherEntity.conversationPartner = entity.gameObject;
                    WorldInteractables.instance.openToChatEntities.Remove(entity.gameObject);
                    WorldInteractables.instance.openToChatEntities.Remove(otherEntity.gameObject);
                    otherEntity.gameObjectToWalkTo = entity.gameObject;
                    visitorEntity.gameObjectToWalkTo = otherEntity.gameObject;
                    visitorEntity.talkIndex = 0;
                    otherEntity.talkIndex = 1; 
                    break; 
                }
            }
        }
        
        currentChatFindDuration += Time.deltaTime;
        if (currentChatFindDuration >= chatFindDuration || visitorEntity.conversationPartner != null)
            CheckSwitchState();
    }


    public override void ExitState()
    {
        if(WorldInteractables.instance.openToChatEntities.Contains(entity.gameObject))
            WorldInteractables.instance.openToChatEntities.Remove(entity.gameObject);
        
        currentChatFindDuration = 0; 
    }
    
    #endregion State Methods

    #region Methods
    
    private void Initialize()
    {
       entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
       WorldInteractables.instance.openToChatEntities.Add(entity.gameObject);
    }

    #endregion Methods
}