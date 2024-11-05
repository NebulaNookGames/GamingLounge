using UnityEngine;

/// <summary>
/// Interface for defining the behavior of different building states.
/// </summary>
public interface IBuildingState
{
    /// <summary>
    /// Ends the current state, performing necessary cleanup.
    /// </summary>
    void EndState();

    /// <summary>
    /// Handles actions performed at a specific grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position where the action is performed.</param>
    void OnAction(Vector3Int gridPosition);

    /// <summary>
    /// Updates the state based on the current grid position.
    /// </summary>
    /// <param name="gridPosition">The current grid position to update the state with.</param>
    void UpdateState(Vector3Int gridPosition);
}