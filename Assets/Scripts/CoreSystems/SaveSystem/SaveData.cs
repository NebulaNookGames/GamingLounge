using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// This class is used to save all information that needs to be saved and gets passed from and to the save system to save and set values.
/// </summary>
[Serializable]
public class SaveData
{
    public List<Vector3Int> gridPositions;
    public List<Vector2Int> objectSizes;
    public List<int> placedObjectiDs;
    public List<int> customizedObjectiDs;

    public List<ObjectData> objectDatas;
    public List<Vector3> positions;
    public List<Quaternion> rotations;

    public int savedMoney;
    public List<int> moneyInArcadeMachines;
    public List<Vector3> arcadeMachinePositions;

    public bool[] boughtObjects;
    public bool[] boughtLand;

    public List<NPCValues> npcValues;

    public Vector3 playerPosition;
    public int[] materialIndexes;

    public void CopyFrom(SaveData other)
    {
        // Copy primitive and value types
        this.savedMoney = other.savedMoney;
        this.playerPosition = other.playerPosition;

        // Deep copy arrays
        this.boughtObjects = other.boughtObjects != null ? (bool[])other.boughtObjects.Clone() : null;
        this.boughtLand = other.boughtLand != null ? (bool[])other.boughtLand.Clone() : null;
        this.materialIndexes = other.materialIndexes != null ? (int[])other.materialIndexes.Clone() : null;

        // Deep copy lists (value types or Unity structs)
        this.gridPositions = other.gridPositions != null ? new List<Vector3Int>(other.gridPositions) : null;
        this.objectSizes = other.objectSizes != null ? new List<Vector2Int>(other.objectSizes) : null;
        this.placedObjectiDs = other.placedObjectiDs != null ? new List<int>(other.placedObjectiDs) : null;
        this.customizedObjectiDs = other.customizedObjectiDs != null ? new List<int>(other.customizedObjectiDs) : null;

        this.objectDatas = other.objectDatas != null ? new List<ObjectData>(other.objectDatas) : null;
        this.positions = other.positions != null ? new List<Vector3>(other.positions) : null;
        this.rotations = other.rotations != null ? new List<Quaternion>(other.rotations) : null;
        this.moneyInArcadeMachines = other.moneyInArcadeMachines != null ? new List<int>(other.moneyInArcadeMachines) : null;
        this.arcadeMachinePositions = other.arcadeMachinePositions != null ? new List<Vector3>(other.arcadeMachinePositions) : null;

        this.npcValues = other.npcValues != null ? new List<NPCValues>(other.npcValues) : null;
    }
}
