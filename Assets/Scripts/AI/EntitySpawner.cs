using UnityEngine;
using System.Collections;
using System; 
public class EntitySpawner : MonoBehaviour
{
    public static EntitySpawner instance; 
    
    public GameObject entityPrefab; // The prefab to spawn
    public float spawnInterval = 10f; // Time between spawns in seconds
    private float timer;
    public int maxAmount = 0;
    public int amountPerLand = 10; 
    public int maxAmountFromLand;
    public int amount;
    public ExpandHandler expandHandler;
    [SerializeField] private float spawnDistance = 30;
    [SerializeField] GameObject buildingChecker;
    

    public Action onAmountUpdated; 
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this; 
        
        timer = spawnInterval; // Set the timer to the spawn interval initially
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer
        timer -= Time.deltaTime;

        // Check if it's time to spawn
        if (timer <= 0f)
        {
            int boughtLandAmount = 0;
            for (int i = 0; i < expandHandler.boughtLand.Length; i++)
            {
                if (expandHandler.boughtLand[i])
                {
                    boughtLandAmount++;
                }
            }

            maxAmountFromLand = boughtLandAmount * amountPerLand;
            maxAmount = WorldInteractables.instance.machineCount;
            if (maxAmount >= maxAmountFromLand)
            {
                maxAmount = maxAmountFromLand;
                onAmountUpdated?.Invoke();
            }

            if (amount >= maxAmount || maxAmount == 0) return; 
            StartCoroutine(SpawnNewEntity()); // Call the spawn method
            timer = spawnInterval; // Reset the timer
        }
    }

    // Method to spawn the entity
    IEnumerator SpawnNewEntity()
    {
        bool foundSpawnPos = false;
        
        while (!foundSpawnPos)
        {
            Vector3 randomPos = transform.position;
            
            randomPos = new Vector3(
                UnityEngine.Random.Range(transform.position.x - spawnDistance, transform.position.x + spawnDistance),
                transform.position.y,
                UnityEngine.Random.Range(transform.position.z - spawnDistance, transform.position.x + spawnDistance));

            buildingChecker.transform.position = new Vector3(randomPos.x, buildingChecker.transform.position.y, randomPos.z);

            yield return new WaitForSeconds(.2f);
            if (!buildingChecker.GetComponent<CheckIfInBuilding>().IsInBuilding())
            {
                foundSpawnPos = true; 
            }
        }

        if (entityPrefab != null && buildingChecker != null)
        {
            Quaternion randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
            // Instantiate the entityPrefab at the spawner's position with no rotation
            GameObject newEntity = Instantiate(entityPrefab, buildingChecker.transform.position, randomRotation);
            newEntity.GetComponentInChildren<RandomizeCharacter>().GenerateNew();
            newEntity.transform.parent = transform; 
            amount++;
            EntityManager.instance.currentNPCs.Add(newEntity);
            onAmountUpdated?.Invoke();
        }
    }

    public IEnumerator SpawnLoadedEntities()
    {
        foreach (NPCValues npcValues in EntityManager.instance.npcValues)
        {
            Vector3 spawnPos = Vector3.zero; 
            
            if (npcValues.lastLocation == Vector3.zero)
            {
                bool foundSpawnPos = false; 
                
                while (!foundSpawnPos)
                {
                    Vector3 randomPos = transform.position;
            
                    randomPos = new Vector3(
                        UnityEngine.Random.Range(transform.position.x - spawnDistance, transform.position.x + spawnDistance),
                        transform.position.y,
                        UnityEngine.Random.Range(transform.position.z - spawnDistance, transform.position.x + spawnDistance));

                    buildingChecker.transform.position = new Vector3(randomPos.x, buildingChecker.transform.position.y, randomPos.z);

                    yield return new WaitForSeconds(.2f);
                    if (!buildingChecker.GetComponent<CheckIfInBuilding>().IsInBuilding())
                    {
                        foundSpawnPos = true; 
                        spawnPos = buildingChecker.transform.position;
                    }
                }
            }
            else
            {
                spawnPos = npcValues.lastLocation;
            }

            if (entityPrefab != null && buildingChecker != null)
            {
                // Instantiate the entityPrefab at the spawner's position with no rotation
                GameObject newEntity =
                    Instantiate(entityPrefab, spawnPos, Quaternion.identity);
                newEntity.GetComponentInChildren<RandomizeCharacter>().LoadExisting(npcValues);
                newEntity.transform.parent = transform;
                EntityManager.instance.currentNPCs.Add(newEntity);
                amount++;
            }

            yield return new WaitForSeconds(.2f);
        }
        maxAmount = WorldInteractables.instance.machineCount;
        onAmountUpdated?.Invoke();
    }
}