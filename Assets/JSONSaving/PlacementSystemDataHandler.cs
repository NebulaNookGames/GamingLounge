using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.ShaderGraph;
using UnityEngine.PlayerLoop;

public class PlacementSystemDataHandler : DataHandler
{
    public static PlacementSystemDataHandler Instance;
    
    public List<KeyValuePair<Vector3Int, PlacementData>> floorPlacementData = new();
    public List<KeyValuePair<Vector3Int, PlacementData>> wallPlacementData = new();
    public List<KeyValuePair<Vector3Int, PlacementData>> wallDecorPlacementData = new();
    public List<KeyValuePair<Vector3Int, PlacementData>> furniturePlacementData = new();

    public List<GameObjectData> gameObjectDatas = new List<GameObjectData>();
    public ObjectPlacer objectPlacer;
    public Action OnDataReceived;
    
    public void UpdateData()
    { 
        floorPlacementData.Clear();
        foreach(KeyValuePair<Vector3Int, PlacementData> placementData in PlacementSystem.Instance.floorData.placedObjects)
            floorPlacementData.Add(placementData);
        
        wallPlacementData.Clear();
        foreach(KeyValuePair<Vector3Int, PlacementData> placementData in PlacementSystem.Instance.wallData.placedObjects)
            wallPlacementData.Add(placementData);
        
        wallDecorPlacementData.Clear();
        foreach(KeyValuePair<Vector3Int, PlacementData> placementData in PlacementSystem.Instance.wallDecorData.placedObjects)
            wallDecorPlacementData.Add(placementData);
        
        furniturePlacementData.Clear();
        foreach(KeyValuePair<Vector3Int, PlacementData> placementData in PlacementSystem.Instance.furnitureData.placedObjects)
            furniturePlacementData.Add(placementData);
    }

    public void RemoveGameObjectData(int placedObjectIndex )
    {
        GameObjectData dataToRemove = gameObjectDatas.Find(x => x.placedObjectIndex ==  placedObjectIndex);
        if(dataToRemove != null)
            gameObjectDatas.Remove(dataToRemove);
    }
    
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Destroy(gameObject);
    }
    
    public override void ReceiveData(SaveData saveData)
    {
        floorPlacementData.Clear();
        foreach(PlacementDataEntry goData in saveData.floorPlacementData)
            floorPlacementData.Add(new KeyValuePair<Vector3Int, PlacementData>(new Vector3Int((int)goData.key.x, (int)goData.key.y, (int)goData.key.z), goData.value));
        
        wallPlacementData.Clear();
        foreach(PlacementDataEntry goData in saveData.wallPlacementData)
            wallPlacementData.Add(new KeyValuePair<Vector3Int, PlacementData>(new Vector3Int((int)goData.key.x, (int)goData.key.y, (int)goData.key.z), goData.value));
        
        wallDecorPlacementData.Clear();
        foreach(PlacementDataEntry goData in saveData.wallDecorPlacementData)
            wallDecorPlacementData.Add(new KeyValuePair<Vector3Int, PlacementData>(new Vector3Int((int)goData.key.x, (int)goData.key.y, (int)goData.key.z), goData.value));
        
        furniturePlacementData.Clear();
        foreach(PlacementDataEntry goData in saveData.furniturePlacementData)
            furniturePlacementData.Add(new KeyValuePair<Vector3Int, PlacementData>(new Vector3Int((int)goData.key.x, (int)goData.key.y, (int)goData.key.z), goData.value));

        gameObjectDatas.Clear();
        for(int i = 0; i < saveData.gameObjectPositions.Count; i++)
            gameObjectDatas.Add(new GameObjectData(saveData.gameObjectIndexes[i], 
                                                   saveData.gameObjectPositions[i],
                                                   saveData.placedObjectIndexes[i])); 
        OnDataReceived?.Invoke();
    }

    public override void SendData(SaveData saveData)
    {
            saveData.floorPlacementData = new List<PlacementDataEntry>();
            foreach (KeyValuePair<Vector3Int, PlacementData> goData in floorPlacementData)
                saveData.floorPlacementData.Add(new PlacementDataEntry(goData.Key, goData.Value));
            
            saveData.wallPlacementData = new List<PlacementDataEntry>();
            foreach (KeyValuePair<Vector3Int, PlacementData> goData in wallPlacementData)
                saveData.wallPlacementData.Add(new PlacementDataEntry(goData.Key, goData.Value));
            
            saveData.wallDecorPlacementData = new List<PlacementDataEntry>(); 
            foreach (KeyValuePair<Vector3Int, PlacementData> goData in wallDecorPlacementData) 
                saveData.wallDecorPlacementData.Add(new PlacementDataEntry(goData.Key, goData.Value));
            
            saveData.furniturePlacementData = new List<PlacementDataEntry>(); 
            foreach (KeyValuePair<Vector3Int, PlacementData> goData in furniturePlacementData) 
                saveData.furniturePlacementData.Add(new PlacementDataEntry(goData.Key, goData.Value));
            
            for (int i = 0; i < objectPlacer.placedGameObjects.Count; i++) 
            { 
                bool foundDuplicate = false; 
                foreach (GameObjectData objectData in gameObjectDatas) 
                { 
                    if (objectData.placedObjectIndex == i) 
                    { 
                        foundDuplicate = true; 
                        break; 
                    } 
                } 
                if (foundDuplicate) continue; 
                gameObjectDatas.Add(new GameObjectData(objectPlacer.placedGameObjects[i].Value, 
                    objectPlacer.placedGameObjects[i].Key.transform.position, i)); 
            }
            
            saveData.gameObjectIndexes = new List<int>(); 
            saveData.gameObjectPositions = new List<Vector3>(); 
            saveData.placedObjectIndexes = new List<int>(); 
            
            for (int i = 0; i < gameObjectDatas.Count; i++) 
            { 
                saveData.gameObjectIndexes.Add(gameObjectDatas[i].gameObjectIndex); 
                saveData.gameObjectPositions.Add(gameObjectDatas[i].position); 
                saveData.placedObjectIndexes.Add(gameObjectDatas[i].placedObjectIndex); 
            }
    }
    
    [Serializable]
    public class GameObjectData
    {
        public GameObjectData(int gameObjectIndex, Vector3 position, int placedObjectIndex)
        {
            this.gameObjectIndex = gameObjectIndex;
            this.position = position;
            this.placedObjectIndex = placedObjectIndex;
        }
        public int gameObjectIndex;
        public Vector3 position;
        public int placedObjectIndex; 
    }
}