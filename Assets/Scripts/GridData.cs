using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages grid-based placement of objects, ensuring that placed objects do not overlap.
/// </summary>
public class GridData
{
    /// <summary>
    /// A dictionary mapping grid positions to data about the placed objects.
    /// </summary>
    private Dictionary<Vector3Int, PlacementData> placedObjects = new();

    /// <summary>
    /// Adds an object at the specified grid position and registers its occupied cells.
    /// </summary>
    /// <param name="gridPosition">The position on the grid to start placing the object.</param>
    /// <param name="objectSize">The size of the object in grid units.</param>
    /// <param name="ID">An identifier for the object being placed.</param>
    /// <param name="placedObjectIndex">An index representing the placed object, used for tracking purposes.</param>
    /// <exception cref="Exception">Thrown if an object already occupies a position within the specified area.</exception>
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        // Calculate all positions the object will occupy based on its size and starting grid position.
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        
        // Create new placement data for the object.
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);

        // Check for overlaps, throwing an exception if any positions are already occupied.
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    /// <summary>
    /// Calculates all grid positions that the object will occupy, based on its size.
    /// </summary>
    /// <param name="gridPosition">The starting position on the grid.</param>
    /// <param name="objectSize">The dimensions of the object in grid units.</param>
    /// <returns>A list of all occupied positions.</returns>
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();

        // Iterate through the object's size dimensions to determine all positions it will occupy.
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    /// <summary>
    /// Checks if an object can be placed at the specified position without overlapping other objects.
    /// </summary>
    /// <param name="gridPosition">The position on the grid to check.</param>
    /// <param name="objectSize">The size of the object in grid units.</param>
    /// <returns>True if the object can be placed; otherwise, false.</returns>
    public bool canPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        // Check each position to see if it's already occupied.
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Gets the index representation of the object at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The position on the grid.</param>
    /// <returns>The index of the placed object, or -1 if no object is at the position.</returns>
    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition))
        {
            return -1;
        }
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    /// <summary>
    /// Removes the object located at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The position of the object to remove.</param>
    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        // Remove all occupied positions for the object at the given position.
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

/// <summary>
/// Stores data about a placed object, including its occupied positions and identifiers.
/// </summary>
public class PlacementData
{
    /// <summary>
    /// The grid positions occupied by this object.
    /// </summary>
    public List<Vector3Int> occupiedPositions;

    /// <summary>
    /// Gets the unique identifier of the object.
    /// </summary>
    public int ID { get; private set; }

    /// <summary>
    /// Gets the index used to represent the placed object.
    /// </summary>
    public int PlacedObjectIndex { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlacementData"/> class with specified properties.
    /// </summary>
    /// <param name="occupiedPositions">The list of grid positions this object occupies.</param>
    /// <param name="iD">The unique identifier of the object.</param>
    /// <param name="placedObjectIndex">The index used to represent the placed object.</param>
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}
