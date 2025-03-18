using System;
using UnityEngine;
using System.Collections.Generic;
public class PlacementDataHandler : DataHandler
{
    public static PlacementDataHandler instance; 
    public ObjectsDatabaseSO objectsDatabaseSO;
    
    public PlacementSystem placementSystem;
    public ObjectPlacer objectPlacer;

    public List<Vector3Int> gridPositions = new();
    public List<Vector2Int> objectSizes = new();
    public List<int> placedObjectiDs = new();
    public List<int> customizedObjectIDs = new();

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
        placedObjectiDs.Clear();
        customizedObjectIDs.Clear();
        objectDatas.Clear();
        positions.Clear();
        rotations.Clear();
        
        for(int i = 0; i < saveData.gridPositions.Count; i++)
        {
            int index = 0; 
            foreach (ObjectData objData in objectsDatabaseSO.objectsData)
            {
               if (saveData.objectDatas[i].ID == objData.ID)
                    {
                        index = objectPlacer.PlaceObject(
                            objData,
                            saveData.positions[i],
                            saveData.rotations[i], false, false, saveData.customizedObjectiDs[i]);
                    }
            }
       
            switch (saveData.objectDatas[i].objectType)
            {
               case ObjectType.Ground:
                   placementSystem.floorData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       index);
                   break;
               case ObjectType.Wall:
                   placementSystem.wallData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       index);
                   break;
               case ObjectType.WallDecor:
                   placementSystem.wallDecorData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       index);
                   break;
               case ObjectType.Furniture:
                   placementSystem.furnitureData.AddObjectAt(
                       saveData.gridPositions[i],
                       saveData.objectSizes[i],
                       index);
                   break;
            }
        }
    }

    public override void SendData(SaveData saveData)
    {
        saveData.gridPositions = new List<Vector3Int>(gridPositions);
        saveData.objectSizes = new List<Vector2Int>(objectSizes);
        saveData.placedObjectiDs = new List<int>(placedObjectiDs);
        saveData.objectDatas = new List<ObjectData>(objectDatas);
        saveData.positions = new List<Vector3>(positions);
        saveData.rotations = new List<Quaternion>(rotations);
        saveData.customizedObjectiDs = new List<int>(customizedObjectIDs);
    }

    public void AddPlacedObject(Vector3Int gridPosition, Vector2Int objectSize, int placedObjectiD)
    {
        gridPositions.Add(gridPosition);
        objectSizes.Add(objectSize);
        placedObjectiDs.Add(placedObjectiD);
    }

    public void AddData(ObjectData objectData, Vector3 position, Quaternion rotation)
    {
        objectDatas.Add(objectData);
        positions.Add(position);
        rotations.Add(rotation);
        customizedObjectIDs.Add(0);
    }

    public void ChangeCustomizedObjectID(Vector3Int pos, int newID)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i] == pos)
            {
                customizedObjectIDs[i] = newID; 
                Debug.Log("Changed customized id");
                break; 
            }
        }
    }

    public void RemovePlacedObject(GameObject go)
    {
        List<int> indicesToRemove = new List<int>();

        for (int i = 0; i < gridPositions.Count; i++)
        {
            if (positions[i] == go.transform.position)
            {
                indicesToRemove.Add(i);
            }
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
        {
            int index = indicesToRemove[i];
            gridPositions.RemoveAt(index);
            objectSizes.RemoveAt(index);
            placedObjectiDs.RemoveAt(index);
            customizedObjectIDs.RemoveAt(index);
            objectDatas.RemoveAt(index);
            positions.RemoveAt(index);
            rotations.RemoveAt(index);
        }
    }
}