using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.Serialization;

public class TutorialPlacement : MonoBehaviour
{
    [Header("Tutorial spawning")]
    public ObjectPlacer objectPlacer;
    public List<int> objectIndexes;
    public List<Vector3Int> objectPositions;
    public List<Quaternion> objectRotations;
    public ObjectsDatabaseSO objectsDatabase;
    Grid grid;
    
    public int treeOneIndex;
    public int treeOneAmount;

    public int treeTwoIndex;
    public int treeTwoAmount; 
    
    public int rockIndex;
    public int rockAmount;
    
    public int plantIndex;
    public int plantAmount;
    
    public int fallPlantIndex;
    public int fallPlantAmount;

    public int rockTwoIndex;
    public int rockTwoAmount;

    public int rockThreeIndex;
    public int rockThreeAmount;

    public int bushOneIndex;
    public int bushOneAmount;

    public int bushTwoIndex;
    public int bushTwoAmount;
    
    public int xBounds;
    public int zBounds;

    public int xIgnoreRange;
    public int zIgnoreRange;
    
    public void Start()
    {
        grid = PlacementSystem.Instance.grid;


        for (int i = 0; i < objectIndexes.Count; i++)
        {
            int index = objectPlacer.PlaceObject(objectsDatabase.objectsData[objectIndexes[i]], grid.CellToWorld(objectPositions[i]),
                objectRotations[i], false, false);
            
            GridData selectedData = GetUsedGridData(i);
            if(objectRotations[i].y == 0)
                selectedData.AddObjectAt(objectPositions[i], objectsDatabase.objectsData[objectIndexes[i]].Size, index);
            else
            {
                Vector2Int newSize = new Vector2Int(objectsDatabase.objectsData[objectIndexes[i]].Size.y,
                    objectsDatabase.objectsData[objectIndexes[i]].Size.x);
                
                selectedData.AddObjectAt(objectPositions[i], newSize, index);
            }
        }
        
        // Calling the method for different objects
        PlaceObjects(treeOneAmount, treeOneIndex);
        PlaceObjects(treeTwoAmount, treeTwoIndex);
        PlaceObjects(plantAmount, plantIndex);
        PlaceObjects(rockAmount, rockIndex);
        PlaceObjects(fallPlantAmount, fallPlantIndex);
        PlaceObjects(rockTwoAmount, rockTwoIndex);
        PlaceObjects(rockThreeAmount, rockThreeIndex);
        PlaceObjects(bushOneAmount, bushOneIndex);
        PlaceObjects(bushTwoAmount, bushTwoIndex);


    }
    
    /// <summary>
    /// Gets the grid data corresponding to the currently selected object type.
    /// </summary>
    /// <returns>The grid data for the selected object type.</returns>
    private GridData GetUsedGridData(int selectedObjectIndex)
    {
        switch (objectsDatabase.objectsData[selectedObjectIndex].objectType)
        {
            case ObjectType.Ground:
                return PlacementSystem.Instance.floorData;
            case ObjectType.Wall:
                return PlacementSystem.Instance.wallData;
            case ObjectType.WallDecor:
                return PlacementSystem.Instance.wallDecorData;
            case ObjectType.Furniture:
                return PlacementSystem.Instance.furnitureData;
            default:
                return PlacementSystem.Instance.floorData;
        }
    }
    
    void PlaceObjects(int amount, int objectIndex)
    {
        for (int i = 0; i < amount; i++)
        {
            Quaternion rot = new Quaternion();
            int randomRot = Random.Range(0, 5);
            if (randomRot == 0)
                rot = Quaternion.Euler(0, 0, 0);
            else if(randomRot == 1)
                rot = Quaternion.Euler(0, 90, 0);
            else if (randomRot == 2)
                rot = Quaternion.Euler(0, 180, 0);
            else if (randomRot == 3)
                rot = Quaternion.Euler(0, 270, 0);
            
            int xPos = Random.Range(-xBounds, xBounds);
            int zPos = Random.Range(-zBounds, zBounds);
            Vector3Int pos = new Vector3Int(xPos, zPos, 0);
            for (int j = 0; j < objectPositions.Count; j++)
            {
                if (pos == objectPositions[j])
                {
                    return; 
                }
            }
            
            
            if (pos.x > -xIgnoreRange && pos.x < xIgnoreRange && pos.y > -zIgnoreRange && pos.y < zIgnoreRange)
            {
                i--;
                continue;
            }
            
            if (!PlacementSystem.Instance.furnitureData.CanPlaceObjectAt(pos, objectsDatabase.objectsData[objectIndex].Size))
            {
                i--;
                continue;
            }
            
            int index = objectPlacer.PlaceObject(objectsDatabase.objectsData[objectIndex], grid.CellToWorld(pos),
                rot, false, false);
            PlacementSystem.Instance.furnitureData.AddObjectAt(pos, objectsDatabase.objectsData[objectIndex].Size,
                index);
        }
    }
}
