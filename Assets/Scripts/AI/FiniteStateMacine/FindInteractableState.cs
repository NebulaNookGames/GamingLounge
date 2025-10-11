using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Sets the agent's destination to the nearest interactable point of interest and plays an animation.
/// </summary>
public class FindInteractableState : State
{
    #region Variables

    private VisitorEntity visitorEntity;
    
    [Header("Search Settings")]
    [Tooltip("The maximum distance within which the entity searches for an interactable object.")]
    private int interactableSearchDistance = 10;

    #endregion Variables

    #region Constructor

    /// <summary>
    /// Initializes the FindInteractableState with the associated visitor entity and base entity.
    /// </summary>
    /// <param name="visitorEntity">The visitor entity searching for an interaction.</param>
    /// <param name="entity">The base entity reference.</param>
    public FindInteractableState(VisitorEntity visitorEntity, Entity entity) : base(entity)
    {
        this.visitorEntity = visitorEntity;
    }

    #endregion Constructor

    #region State Methods

    /// <summary>
    /// Called upon entering the state. Begins the search for an interactable object.
    /// </summary>
    public override void EnterState()
    {
        Initialize();
    }

    public override void ExitState()
    {
        entity.EntityAnimator.enabled = false;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().BakeMesh();
    }

    /// <summary>
    /// Checks if the state should transition.
    /// </summary>
    public override void UpdateState()
    {
        CheckSwitchState();
    }

    #endregion State Methods

    #region Methods

    /// <summary>
    /// Searches for the nearest valid point of interest within range and sets it as the destination.
    /// </summary>
    private void Initialize()
    {
        entity.EntityAnimator.enabled = true;
        visitorEntity.EntityAnimator.GetComponent<MeshBaker>().UnbakeMesh();
        PointOfInterest chosenPOI = null;
        float lastDistance = float.MaxValue;
        
        foreach (PointOfInterest poi in WorldInteractables.instance.pointOfInterests)
        {
            // Ignore character-based POIs or the entity's current ignored POI.
            if (poi.isCharacter) continue;
            
            float tempDistance = Vector3.Distance(poi.transform.position, visitorEntity.transform.position);
            
            // Select the closest valid POI within the search range.
            if (tempDistance <= interactableSearchDistance && tempDistance < lastDistance)
            {
                lastDistance = tempDistance;
                chosenPOI = poi;
            }
        }

        // Assign the selected point of interest as the entity’s walk-to target.
        if (chosenPOI != null)
        {
            visitorEntity.gameObjectToWalkTo = chosenPOI.transform.parent.gameObject;
        }
    }

    #endregion Methods
}
