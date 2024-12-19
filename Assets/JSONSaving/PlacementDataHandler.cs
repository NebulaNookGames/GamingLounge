using System;
using UnityEngine;
using System.Collections.Generic;
public class PlacementDataHandler : DataHandler
{
    public static PlacementDataHandler instance; 
    
    public PlacementSystem placementSystem;
    public ObjectPlacer objectPlacer;

    public List<Vector3Int> gridPositions = new();
    public List<Vector2Int> objectSizes = new();
    public List<int> iDs = new();
    public List<int> placedObjectiDs = new();

    public List<ObjectData> objectDatas = new();
    public List<Vector3> positions = new();
    public List<Quaternion> rotations = new();

    private void Awake()
    {
        instance = this;
    }

    public override void ReceiveData(SaveData saveData)
    {
        gridPositions.Clear();
        objectSizes.Clear();
        iDs.Clear();
        placedObjectiDs.Clear();
        objectDatas.Clear();
        positions.Clear();
        rotations.Clear();
        
        for(int i = 0; i < saveData.gridPositions.Count; i++)
        {
            objectPlacer.PlaceObject(
                saveData.objectDatas[i],
                saveData.positions[i],
                saveData.rotations[i]);
       
            switch (saveData.objectDatas[i].objectType)
            {
               case ObjectType.Ground:
                   placementSystem.floorData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       saveData.iDs[i],
                       saveData.placedObjectiDs[i]);
                   break;
               case ObjectType.Wall:
                   placementSystem.wallData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       saveData.iDs[i],
                       saveData.placedObjectiDs[i]);
                   break;
               case ObjectType.WallDecor:
                   placementSystem.wallDecorData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       saveData.iDs[i],
                       saveData.placedObjectiDs[i]);
                   break;
               case ObjectType.Furniture:
                   placementSystem.furnitureData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       saveData.iDs[i],
                       saveData.placedObjectiDs[i]);
                   break;
            }
        }
    }

    public override void SendData(SaveData saveData)
    {
        saveData.gridPositions = new List<Vector3Int>(gridPositions);
        saveData.objectSizes = new List<Vector2Int>(objectSizes);
        saveData.iDs = new List<int>(iDs);
        saveData.placedObjectiDs = new List<int>(placedObjectiDs);
        saveData.objectDatas = new List<ObjectData>(objectDatas);
        saveData.positions = new List<Vector3>(positions);
        saveData.rotations = new List<Quaternion>(rotations);
    }

    public void AddPlacedObject(Vector3Int gridPosition, Vector2Int objectSize, int iD, int placedObjectiD)
    {
        gridPositions.Add(gridPosition);
        objectSizes.Add(objectSize);
        iDs.Add(iD);
        placedObjectiDs.Add(placedObjectiD);
    }

    public void AddData(ObjectData objectData, Vector3 position, Quaternion rotation)
    {
        objectDatas.Add(objectData);
        positions.Add(position);
        rotations.Add(rotation);
    }

    public void RemovePlacedObject(GameObject go)
    {
        for (int i = 0; i < gridPositions.Count; i++)
        {
            if (positions[i] == go.transform.position)
            {
                gridPositions.RemoveAt(i);
                objectSizes.RemoveAt(i);
                iDs.RemoveAt(i);
                placedObjectiDs.RemoveAt(i);
                objectDatas.RemoveAt(i);
                positions.RemoveAt(i);
                rotations.RemoveAt(i);
            }
        }
    }
}