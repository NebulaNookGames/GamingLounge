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

    public List<ObjectData> objectDatas;
    public List<Vector3> positions;
    public List<Quaternion> rotations;
    
    
    public int savedMoney;
    public List<int> moneyInArcadeMachines;
    public List<Vector3> arcadeMachinePositions;
    
    
    public bool[] boughtObjects;
    public bool[] boughtLand;
}