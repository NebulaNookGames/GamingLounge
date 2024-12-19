using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement data on a grid, tracking which objects occupy which cells.
/// </summary>
public class GridData
{
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();

    /// <summary>
    /// Adds an object to the grid at the specified position.
    /// </summary>
    /// <param name="gridPosition">The grid position where the object is placed.</param>
    /// <param name="objectSize">The size of the object in grid cells.</param>
    /// <param name="ID">The ID of the object being placed.</param>
    /// <param name="placedObjectIndex">The index of the placed object in the object placer.</param>
    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach (var pos in positionsToOccupy)
        {
            placedObjects[pos] = data;
        }
        PlacementDataHandler.instance.AddPlacedObject(gridPosition, objectSize, ID, placedObjectIndex);
    }
    
    /// <summary>
    /// Calculates all grid positions occupied by an object based on its size.
    /// </summary>
    /// <param name="gridPosition">The starting grid position.</param>
    /// <param name="objectSize">The size of the object in grid cells.</param>
    /// <returns>A list of all grid positions occupied by the object.</returns>
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positions = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                gridPosition.z = 0; 
                Vector3Int position = gridPosition + new Vector3Int(x, y, 0);
                positions.Add(position);
            }
        }
        return positions;
    }

    /// <summary>
    /// Checks if an object can be placed at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The starting grid position.</param>
    /// <param name="objectSize">The size of the object in grid cells.</param>
    /// <returns>True if the object can be placed; otherwise, false.</returns>
    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets the index of the object placed at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position to check.</param>
    /// <returns>The index of the placed object, or -1 if no object is present.</returns>
    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (!placedObjects.ContainsKey(gridPosition))
        {
            return -1;
        }
        return placedObjects[gridPosition].placedObjectIndex;
    }

    /// <summary>
    /// Removes the object at the specified grid position.
    /// </summary>
    /// <param name="gridPosition">The grid position to remove the object from.</param>
    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

[Serializable]
/// <summary>
/// Stores data about a placed object, including its occupied positions and identifiers.
/// </summary>
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int iD;
    public int placedObjectIndex;

    /// <summary>
    /// Initializes a new instance of the PlacementData class.
    /// </summary>
    /// <param name="occupiedPositions">The grid positions occupied by the object.</param>
    /// <param name="iD">The ID of the object.</param>
    /// <param name="placedObjectIndex">The index of the object in the object placer.</param>
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        this.iD = iD;
        this.placedObjectIndex = placedObjectIndex;
    }
}