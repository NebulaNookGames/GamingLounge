
using UnityEngine;

public class FindConversationState : State
{
    private VisitorEntity visitorEntity;
    private float chatFindDuration = 3f;
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
        VisitorEntity closestVisitor = null; 
        float currentDistance = float.MaxValue;
        
        if (WorldInteractables.instance.openToChatEntities.Count > 1 && visitorEntity.conversationPartner == null)
        {
            for (int i = 0; i < WorldInteractables.instance.openToChatEntities.Count; i++)
            {
                if (WorldInteractables.instance.openToChatEntities[i] != entity.gameObject)
                {
                    float tempDistance = Vector3.Distance(visitorEntity.transform.position,
                        WorldInteractables.instance.openToChatEntities[i].transform.position);
                    if (tempDistance < currentDistance)
                    {
                        currentDistance = tempDistance;
                        closestVisitor = WorldInteractables.instance.openToChatEntities[i]
                            .GetComponent<VisitorEntity>();
                    }
                }
            }

            if (closestVisitor != null)
            {
                visitorEntity.conversationPartner = closestVisitor.gameObject;
                closestVisitor.conversationPartner = entity.gameObject;
                WorldInteractables.instance.openToChatEntities.Remove(entity.gameObject);
                WorldInteractables.instance.openToChatEntities.Remove(closestVisitor.gameObject);
                closestVisitor.gameObjectToWalkTo = entity.gameObject;
                visitorEntity.gameObjectToWalkTo = closestVisitor.gameObject;
                visitorEntity.talkIndex = 0;
                closestVisitor.talkIndex = 1;
            }
        }
        
        currentChatFindDuration += Time.deltaTime;
        if (currentChatFindDuration >= chatFindDuration)
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