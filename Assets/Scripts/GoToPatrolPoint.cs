using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles the behavior of an agent moving to a patrol point.
/// </summary>
public class GoToPatrolPoint : Behavior
{
    #region Member Variables

    // The radius within which to search for a patrol point.
    [SerializeField] float searchRadius = 50f;

    // The prefab for the patrol point.
    [SerializeField] GameObject patrolPointPrefab;

    #endregion

    #region Methods

    /// <summary>
    /// Executes the pre-performance actions for the patrol behavior.
    /// </summary>
    /// <returns>Returns true when the pre-performance actions are completed.</returns>
    public override bool PrePerform()
    {
        Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
        {
            Vector3 finalPosition = hit.position;
            target = Instantiate(patrolPointPrefab, finalPosition, Quaternion.identity);
        }

        return true;
    }
    
    /// <summary>
    /// Executes the post-performance actions for the patrol behavior.
    /// </summary>
    /// <returns>Returns true when the post-performance actions are completed.</returns>
    public override bool PostPerform()
    {
        Destroy(target);
        return true;
    }

    #endregion
}