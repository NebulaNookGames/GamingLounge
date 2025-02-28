using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sets the agents destination to the <see cref="destination"/> variable and plays a animation.
/// </summary>
public class FindInteractableState : State
{
    private VisitorEntity visitorEntity;
    private int interactableSearchDistance = 10;
    #region Constructor

    public FindInteractableState(VisitorEntity visitorEntity, Entity entity) : base(entity)
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
        PointOfInterest chosenPOI = null;
        float lastDistance = float.MaxValue;
        
        foreach (PointOfInterest poi in WorldInteractables.instance.pointOfInterests)
        {
            if (poi.isCharacter || poi == visitorEntity.headTracking.ignorePointOfInterest) continue; 
            
            float tempDistance = Vector3.Distance(poi.transform.position, visitorEntity.transform.position);
            if (tempDistance <= interactableSearchDistance && tempDistance < lastDistance)
            {
                lastDistance = tempDistance;
                chosenPOI = poi;
            }
        }

        if (chosenPOI != null)
        {
            visitorEntity.gameObjectToWalkTo = chosenPOI.transform.parent.gameObject;
        }
    }

    #endregion Methods
}