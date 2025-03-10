using UnityEngine;

/// <summary>
/// Represents the state where a visitor entity searches for a conversation partner.
/// </summary>
public class FindConversationState : State
{
    #region Variables

    private VisitorEntity visitorEntity;
    
    [Header("Chat Settings")]
    [Tooltip("Duration before giving up on finding a conversation partner.")]
    private float chatFindDuration = 3f;
    
    private float currentChatFindDuration = 0; 
    
    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the FindConversationState with the associated visitor entity and base entity.
    /// </summary>
    /// <param name="visitorEntity">The visitor entity that is searching for a conversation.</param>
    /// <param name="entity">The base entity reference.</param>
    public FindConversationState(VisitorEntity visitorEntity, Entity entity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called upon entering the state. Initializes relevant settings.
    /// </summary>
    public override void EnterState()
    {
        Initialize();
    }

    /// <summary>
    /// Continuously searches for the closest available visitor to start a conversation.
    /// </summary>
    public override void UpdateState()
    {
        VisitorEntity closestVisitor = null; 
        float currentDistance = float.MaxValue;
        
        if (WorldInteractables.instance.openToChatEntities.Count > 1 && visitorEntity.conversationPartner == null)
        {
            foreach (GameObject chatEntity in WorldInteractables.instance.openToChatEntities)
            {
                if (chatEntity != entity.gameObject)
                {
                    float tempDistance = Vector3.Distance(visitorEntity.transform.position, chatEntity.transform.position);
                    
                    if (tempDistance < currentDistance)
                    {
                        currentDistance = tempDistance;
                        closestVisitor = chatEntity.GetComponent<VisitorEntity>();
                    }
                }
            }

            if (closestVisitor != null)
            {
                // Assign conversation partners
                visitorEntity.conversationPartner = closestVisitor.gameObject;
                closestVisitor.conversationPartner = entity.gameObject;
                
                // Remove from available chat entities
                WorldInteractables.instance.openToChatEntities.Remove(entity.gameObject);
                WorldInteractables.instance.openToChatEntities.Remove(closestVisitor.gameObject);
                
                // Set movement targets
                closestVisitor.gameObjectToWalkTo = entity.gameObject;
                visitorEntity.gameObjectToWalkTo = closestVisitor.gameObject;
                
                // Assign talk indexes
                visitorEntity.talkIndex = 0;
                closestVisitor.talkIndex = 1;
            }
        }
        
        // Check if the state should switch after a set duration
        currentChatFindDuration += Time.deltaTime;
        if (currentChatFindDuration >= chatFindDuration)
            CheckSwitchState();
    }

    /// <summary>
    /// Called upon exiting the state. Cleans up references and resets variables.
    /// </summary>
    public override void ExitState()
    {
        if (WorldInteractables.instance.openToChatEntities.Contains(entity.gameObject))
            WorldInteractables.instance.openToChatEntities.Remove(entity.gameObject);
        
        currentChatFindDuration = 0; 
    }
    
    #endregion State Methods

    #region Methods
    
    /// <summary>
    /// Initializes the state by stopping movement and adding the entity to the chat queue.
    /// </summary>
    private void Initialize()
    {
       entity.EntityAnimator.SetFloat("HorizontalSpeed", 0);
       WorldInteractables.instance.openToChatEntities.Add(entity.gameObject);
    }

    #endregion Methods
}