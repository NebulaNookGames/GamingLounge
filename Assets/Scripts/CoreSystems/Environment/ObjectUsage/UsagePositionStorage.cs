using UnityEngine;

/// <summary>
/// Enum representing different types of objects that can be placed.
/// </summary>
public enum MachineType
{
    Console,
    ArcadeMachine,
    Bike,
    Race
};

/// <summary>
/// Stores a usage position as a Transform for use in gameplay or interactions.
/// </summary>
public class UsagePositionStorage : MonoBehaviour
{
    public MachineType machineType; 
    
    /// <summary>
    /// The Transform representing the usage position to be stored.
    /// </summary>
    public Transform usagePosition;
}