using UnityEngine;
using System.Collections.Generic;

public class PlacementSystemLoadDistributor : MonoBehaviour
{
    PlacementSystemDataHandler dataHandler;
    PlacementSystem placementSystem;
    [SerializeField] ObjectsDatabaseSO objectsDatabaseSO;
    [SerializeField] private ObjectPlacer objectPlacer;

    private void Start()
    {
        placementSystem = PlacementSystem.Instance;
        dataHandler = PlacementSystemDataHandler.Instance;
        dataHandler.OnDataReceived += Distribute;
    }

    private void OnDestroy()
    {
        dataHandler.OnDataReceived -= Distribute;
    }

    void Distribute()
    {
        if (dataHandler.floorPlacementData != null && dataHandler.floorPlacementData.Count > 0)
        {
            GridData newData = new GridData();
            newData.placedObjects = new Dictionary<Vector3Int, PlacementData>();
            foreach (KeyValuePair<Vector3Int, PlacementData> kvp in dataHandler.floorPlacementData)
                newData.AddObjectAt(kvp);

            placementSystem.floorData = newData;
        }

        if (dataHandler.wallPlacementData != null && dataHandler.wallPlacementData.Count > 0)
        {
            GridData newData = new GridData();
            newData.placedObjects = new Dictionary<Vector3Int, PlacementData>();
            foreach (KeyValuePair<Vector3Int, PlacementData> kvp in dataHandler.wallPlacementData)
                newData.AddObjectAt(kvp);

            placementSystem.wallData = newData;
        }

        if (dataHandler.wallDecorPlacementData != null && dataHandler.wallDecorPlacementData.Count > 0)
        {
            GridData newData = new GridData();
            newData.placedObjects = new Dictionary<Vector3Int, PlacementData>();
            foreach (KeyValuePair<Vector3Int, PlacementData> kvp in dataHandler.wallDecorPlacementData)
                newData.AddObjectAt(kvp);

            placementSystem.wallDecorData = newData;
        }

        if (dataHandler.furniturePlacementData != null && dataHandler.furniturePlacementData.Count > 0)
        {
            GridData newData = new GridData();
            newData.placedObjects = new Dictionary<Vector3Int, PlacementData>();
            foreach (KeyValuePair<Vector3Int, PlacementData> kvp in dataHandler.furniturePlacementData)
                newData.AddObjectAt(kvp);

            placementSystem.furnitureData = newData;
        }

        foreach (PlacementSystemDataHandler.GameObjectData gameObjectData in dataHandler.gameObjectDatas)
        {
            foreach (ObjectData objectData in objectsDatabaseSO.objectsData)
            {
                if (objectData.ID == gameObjectData.gameObjectIndex)
                {
                    objectPlacer.PlaceObject(objectData, gameObjectData.position, Quaternion.identity);
                }
            }
        }
    }
}