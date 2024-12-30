using UnityEngine;
using System.Collections;

public class EntitySpawner : MonoBehaviour
{
    public GameObject entityPrefab; // The prefab to spawn
    public float spawnInterval = 10f; // Time between spawns in seconds
    private float timer;
    private int maxAmount = 0;
    private int amount;

    [SerializeField] private float spawnDistance = 30;
    [SerializeField] GameObject buildingChecker;

    private bool spawningBlocked = true; 
    
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval; // Set the timer to the spawn interval initially
        Invoke("UnblockSpawning", 10f);
    }

    void UnblockSpawning()
    {
        spawningBlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningBlocked) return; 
        
        // Update the timer
        timer -= Time.deltaTime;

        // Check if it's time to spawn
        if (timer <= 0f)
        {
            UpdateMaxAmount();
            if (amount >= maxAmount || maxAmount == 0) return; 
            StartCoroutine(SpawnNewEntity()); // Call the spawn method
            timer = spawnInterval; // Reset the timer
        }
    }

    // Method to spawn the entity
    IEnumerator SpawnNewEntity()
    {
        spawningBlocked = true; 
        bool foundSpawnPos = false;
        
        while (!foundSpawnPos)
        {
            Vector3 randomPos = transform.position;
            
            randomPos = new Vector3(
                Random.Range(transform.position.x - spawnDistance, transform.position.x + spawnDistance),
                transform.position.y,
                Random.Range(transform.position.z - spawnDistance, transform.position.x + spawnDistance));

            buildingChecker.transform.position = new Vector3(randomPos.x, buildingChecker.transform.position.y, randomPos.z);

            yield return new WaitForSeconds(.2f);
            if (!buildingChecker.GetComponent<CheckIfInBuilding>().IsInBuilding())
            {
                foundSpawnPos = true; 
            }
        }

        if (entityPrefab != null && buildingChecker != null)
        {
            // Instantiate the entityPrefab at the spawner's position with no rotation
            GameObject newEntity = Instantiate(entityPrefab, buildingChecker.transform.position, Quaternion.identity);
            newEntity.GetComponentInChildren<RandomizeCharacter>().GenerateNew();
            newEntity.transform.parent = transform; 
            amount++;
            spawningBlocked = false;
        }
    }

    public IEnumerator SpawnLoadedEntities()
    {
        foreach (NPCValues npcValues in EntityManager.instance.npcValues)
        {
            bool foundSpawnPos = false;

            while (!foundSpawnPos)
            {
                Vector3 randomPos = transform.position;

                randomPos = new Vector3(
                    Random.Range(transform.position.x - spawnDistance, transform.position.x + spawnDistance),
                    transform.position.y,
                    Random.Range(transform.position.z - spawnDistance, transform.position.x + spawnDistance));

                buildingChecker.transform.position =
                    new Vector3(randomPos.x, buildingChecker.transform.position.y, randomPos.z);

                yield return new WaitForSeconds(.2f);
                if (!buildingChecker.GetComponent<CheckIfInBuilding>().IsInBuilding())
                {
                    foundSpawnPos = true;
                }
            }

            if (entityPrefab != null && buildingChecker != null)
            {
                // Instantiate the entityPrefab at the spawner's position with no rotation
                GameObject newEntity =
                    Instantiate(entityPrefab, buildingChecker.transform.position, Quaternion.identity);
                newEntity.GetComponentInChildren<RandomizeCharacter>().LoadExisting(npcValues);
                newEntity.transform.parent = transform;
                amount++;
            }

            yield return new WaitForSeconds(.2f);
        }
        UpdateMaxAmount();
        spawningBlocked = false; 
    }

    void UpdateMaxAmount()
    {
        int machineAmount = WorldInteractables.instance.allAracadeMachines.Count;
        maxAmount = machineAmount; 
    }
}