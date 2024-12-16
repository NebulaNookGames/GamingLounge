using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public GameObject entityPrefab; // The prefab to spawn
    public float spawnInterval = 10f; // Time between spawns in seconds
    private float timer;
    private int maxAmount = 0;
    private int amount; 
    
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval; // Set the timer to the spawn interval initially
        WorldInteractables.instance.OnValueChanged += UpdateMaxAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (amount >= maxAmount || maxAmount == 0) return; 
        
        // Update the timer
        timer -= Time.deltaTime;

        // Check if it's time to spawn
        if (timer <= 0f)
        {
            SpawnEntity(); // Call the spawn method
            timer = spawnInterval; // Reset the timer
        }
    }

    // Method to spawn the entity
    void SpawnEntity()
    {
        if (entityPrefab != null)
        {
            // Instantiate the entityPrefab at the spawner's position with no rotation
            GameObject newEntity = Instantiate(entityPrefab, transform.position, Quaternion.identity);
            newEntity.transform.parent = transform; 
            amount++; 
        }
    }

    void UpdateMaxAmount(int newAmount)
    {
        maxAmount = newAmount; 
    }
}