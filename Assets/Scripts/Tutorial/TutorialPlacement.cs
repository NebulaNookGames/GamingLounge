using UnityEngine;
using System.Collections.Generic;

public class TutorialPlacement : MonoBehaviour
{
    public ObjectPlacer objectPlacer;
    public List<int> objectIndexes;
    public List<Vector3Int> objectPositions;
    public List<Quaternion> objectRotations;
    public ObjectsDatabaseSO objectsDatabase;
    Grid grid; 
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
}
