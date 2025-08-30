using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveDataBinary
{
    public List<SerializableVector3Int> gridPositions;
    public List<SerializableVector2Int> objectSizes;
    public List<int> placedObjectiDs;
    public List<int> customizedObjectiDs;

    public List<SerializableObjectData> objectDatas;
    public List<SerializableVector3> positions;
    public List<SerializableQuaternion> rotations;

    public int savedMoney;
    public List<int> moneyInArcadeMachines;
    public List<SerializableVector3> arcadeMachinePositions;

    public bool[] boughtObjects;
    public bool[] boughtLand;

    public List<NPCValues> npcValues;

    public SerializableVector3 playerPosition;
    public int[] materialIndexes;

    public static SaveDataBinary FromSaveData(SaveData sd)
    {
        var binary = new SaveDataBinary();
        binary.gridPositions = sd.gridPositions?.ConvertAll(v => new SerializableVector3Int(v));
        binary.objectSizes = sd.objectSizes?.ConvertAll(v => new SerializableVector2Int(v));
        binary.placedObjectiDs = sd.placedObjectiDs;
        binary.customizedObjectiDs = sd.customizedObjectiDs;

        // Convert ObjectData to SerializableObjectData
        binary.objectDatas = sd.objectDatas?.ConvertAll(od => SerializableObjectData.FromObjectData(od));

        binary.positions = sd.positions?.ConvertAll(v => new SerializableVector3(v));
        binary.rotations = sd.rotations?.ConvertAll(q => new SerializableQuaternion(q));
        binary.savedMoney = sd.savedMoney;
        binary.moneyInArcadeMachines = sd.moneyInArcadeMachines;
        binary.arcadeMachinePositions = sd.arcadeMachinePositions?.ConvertAll(v => new SerializableVector3(v));
        binary.boughtObjects = sd.boughtObjects;
        binary.boughtLand = sd.boughtLand;

        // Prepare NPCs for binary save
        binary.npcValues = sd.npcValues;
        foreach (var npc in binary.npcValues)
            npc.PrepareForBinarySave();

        binary.playerPosition = new SerializableVector3(sd.playerPosition);
        binary.materialIndexes = sd.materialIndexes;

        return binary;
    }

    public SaveData ToSaveData()
    {
        var sd = new SaveData();
        sd.gridPositions = gridPositions?.ConvertAll(v => v.ToVector3Int());
        sd.objectSizes = objectSizes?.ConvertAll(v => v.ToVector2Int());
        sd.placedObjectiDs = placedObjectiDs;
        sd.customizedObjectiDs = customizedObjectiDs;

        // Convert back to ObjectData
        sd.objectDatas = objectDatas?.ConvertAll(sod => sod.ToObjectData());

        sd.positions = positions?.ConvertAll(v => v.ToVector3());
        sd.rotations = rotations?.ConvertAll(q => q.ToQuaternion());
        sd.savedMoney = savedMoney;
        sd.moneyInArcadeMachines = moneyInArcadeMachines;
        sd.arcadeMachinePositions = arcadeMachinePositions?.ConvertAll(v => v.ToVector3());
        sd.boughtObjects = boughtObjects;
        sd.boughtLand = boughtLand;

        // Restore NPCs after binary load
        sd.npcValues = npcValues;
        foreach (var npc in sd.npcValues)
            npc.RestoreFromBinaryLoad();

        sd.playerPosition = playerPosition.ToVector3();
        sd.materialIndexes = materialIndexes;

        return sd;
    }

    public void CopyFrom(SaveDataBinary other)
    {
        gridPositions = other.gridPositions != null ? new List<SerializableVector3Int>(other.gridPositions) : null;
        objectSizes = other.objectSizes != null ? new List<SerializableVector2Int>(other.objectSizes) : null;
        placedObjectiDs = other.placedObjectiDs != null ? new List<int>(other.placedObjectiDs) : null;
        customizedObjectiDs = other.customizedObjectiDs != null ? new List<int>(other.customizedObjectiDs) : null;

        objectDatas = other.objectDatas != null ? new List<SerializableObjectData>(other.objectDatas) : null;

        positions = other.positions != null ? new List<SerializableVector3>(other.positions) : null;
        rotations = other.rotations != null ? new List<SerializableQuaternion>(other.rotations) : null;

        savedMoney = other.savedMoney;
        moneyInArcadeMachines = other.moneyInArcadeMachines != null ? new List<int>(other.moneyInArcadeMachines) : null;
        arcadeMachinePositions = other.arcadeMachinePositions != null ? new List<SerializableVector3>(other.arcadeMachinePositions) : null;
        boughtObjects = other.boughtObjects != null ? (bool[])other.boughtObjects.Clone() : null;
        boughtLand = other.boughtLand != null ? (bool[])other.boughtLand.Clone() : null;

        npcValues = other.npcValues != null ? new List<NPCValues>(other.npcValues) : null;
        playerPosition = other.playerPosition;
        materialIndexes = other.materialIndexes != null ? (int[])other.materialIndexes.Clone() : null;
    }
}