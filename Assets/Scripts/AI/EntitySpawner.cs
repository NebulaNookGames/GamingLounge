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

    private bool spawningBlocked = false; 
    
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval; // Set the timer to the spawn interval initially
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
            StartCoroutine(SpawnEntity()); // Call the spawn method
            timer = spawnInterval; // Reset the timer
        }
    }

    // Method to spawn the entity
    IEnumerator SpawnEntity()
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

            buildingChecker.transform.position = randomPos;

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
            newEntity.transform.parent = transform; 
            amount++;
            spawningBlocked = false;
        }
    }

    void UpdateMaxAmount()
    {
        int machineAmount = WorldInteractables.instance.allAracadeMachines.Count;
        maxAmount = machineAmount; 
    }
}