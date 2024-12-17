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
    public List<PlacementDataEntry> floorPlacementData;
    public List<PlacementDataEntry> wallPlacementData = new();
    public List<PlacementDataEntry> wallDecorPlacementData = new();
    public List<PlacementDataEntry> furniturePlacementData = new();

    public List<int> gameObjectIndexes = new();
    public List<Vector3> gameObjectPositions = new();
    public List<int> placedObjectIndexes = new();

    public int savedMoney;
    public bool[] boughtObjects;
    [FormerlySerializedAs("boughtLands")] public bool[] boughtLand;
}

[Serializable]
public class PlacementDataEntry
{
    public Vector3 key;
    public PlacementData value;

    public PlacementDataEntry(Vector3 key, PlacementData value)
    {
        this.key = key;
        this.value = value;
    }
}