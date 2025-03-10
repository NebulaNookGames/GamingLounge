using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Manages the spawning of entities (NPCs) in the game world at regular intervals.
/// </summary>
public class EntitySpawner : MonoBehaviour
{
    #region Variables

    [Header("Spawner Settings")]
    [Tooltip("Singleton instance of the EntitySpawner.")]
    public static EntitySpawner instance; // Singleton instance of EntitySpawner
    
    [Tooltip("The prefab to spawn for NPCs.")]
    public GameObject entityPrefab; // The prefab to spawn

    [Tooltip("Time between spawns in seconds.")]
    public float spawnInterval = 1f; // Time between spawns in seconds

    private float timer; // Timer to keep track of spawn interval
    public int maxAmount = 0; // Maximum number of entities allowed to spawn
    public int amountPerLand = 10; // Number of entities to spawn per land
    public int initialAmount = 20; // Initial number of entities to spawn
    public int maxAmountFromLand; // Maximum amount based on bought land
    public int amount; // Current number of spawned entities
    public ExpandHandler expandHandler; // Reference to the ExpandHandler to track land expansion
    
    [SerializeField, Tooltip("Distance to check for spawn positions.")]
    private float spawnDistance = 30; // Distance for possible spawn positions

    [SerializeField, Tooltip("The object used to check if a spawn position is blocked by a building.")]
    GameObject buildingChecker; // Used for checking spawn positions

    private float initialTimer = 10f; // Timer to delay the initial spawn
    
    public Action onAmountUpdated; // Action to notify when the amount has been updated

    #endregion Variables

    #region Unity Methods

    /// <summary>
    /// Called when the script is first loaded. Initializes the singleton instance.
    /// </summary>
    void Start()
    {
        instance = this; // Ensure only one instance of EntitySpawner exists
        timer = spawnInterval; // Set the timer to the spawn interval initially
    }

    /// <summary>
    /// Updates every frame, handling the spawning logic and timing.
    /// </summary>
    void Update()
    {
        if (initialTimer > 0)
        {
            initialTimer -= Time.deltaTime; // Countdown the initial timer
            return;
        }

        // Update the spawn timer
        timer -= Time.deltaTime;

        // Check if it's time to spawn a new entity
        if (timer <= 0f)
        {
            int boughtLandAmount = 0;

            // Count the amount of bought land to calculate the spawn limit
            for (int i = 0; i < expandHandler.boughtLand.Length; i++)
            {
                if (expandHandler.boughtLand[i])
                {
                    boughtLandAmount++;
                }
            }

            // Calculate max amount of entities based on bought land and initial amount
            maxAmountFromLand = ((boughtLandAmount * amountPerLand) + initialAmount);
            maxAmount = WorldInteractables.instance.machineCount;

            if (maxAmount >= maxAmountFromLand)
            {
                maxAmount = maxAmountFromLand;
                onAmountUpdated?.Invoke();
            }

            // Don't spawn if max amount is reached or no machines are available
            if (amount >= maxAmount || maxAmount == 0) return;

            // Start spawning a new entity
            StartCoroutine(SpawnNewEntity());
            timer = spawnInterval; // Reset the timer
        }
    }

    #endregion Unity Methods

    #region Methods

    /// <summary>
    /// Coroutine to spawn a new NPC entity at a random position, ensuring it is not inside a building.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    IEnumerator SpawnNewEntity()
    {
        bool foundSpawnPos = false;

        // Keep trying until a valid spawn position is found
        while (!foundSpawnPos)
        {
            // Generate a random position within the spawn distance
            Vector3 randomPos = transform.position;
            randomPos = new Vector3(
                UnityEngine.Random.Range(transform.position.x - spawnDistance, transform.position.x + spawnDistance),
                transform.position.y,
                UnityEngine.Random.Range(transform.position.z - spawnDistance, transform.position.x + spawnDistance));

            // Check if the spawn position is free (not in a building)
            buildingChecker.transform.position = new Vector3(randomPos.x, buildingChecker.transform.position.y, randomPos.z);
            yield return new WaitForSeconds(.2f);

            // If the position is free, break the loop
            if (!buildingChecker.GetComponent<CheckIfInBuilding>().IsInBuilding())
            {
                foundSpawnPos = true;
            }
        }

        // Spawn the NPC if valid prefab and checker exist
        if (entityPrefab != null && buildingChecker != null)
        {
            Quaternion randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
            GameObject newEntity = Instantiate(entityPrefab, buildingChecker.transform.position, randomRotation);
            newEntity.GetComponentInChildren<RandomizeCharacter>().GenerateNew(); // Randomize character appearance
            newEntity.transform.parent = transform; // Set the entity's parent
            amount++; // Increment the amount of spawned entities
            EntityManager.instance.currentNPCs.Add(newEntity); // Add to NPC list
            onAmountUpdated?.Invoke(); // Notify about amount update

            // Check for achievements based on number of entities
            try
            {
                if (amount >= 30)
                {
                    SteamIntegration.instance.UnlockAchievement("THIRTYVISITORS");
                }

                if (amount >= 50)
                {
                    SteamIntegration.instance.UnlockAchievement("FIFTYVISITORS");
                }

                if (amount >= 100)
                {
                    SteamIntegration.instance.UnlockAchievement("ONEHUNDREDVISITORS");
                }
            }
            catch
            {
                Debug.Log("Failed unlocking entity achievement");
            }
        }
    }

    /// <summary>
    /// Coroutine to spawn entities based on previously saved NPC data.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    public IEnumerator SpawnLoadedEntities()
    {
        foreach (NPCValues npcValues in EntityManager.instance.npcValues)
        {
            Vector3 spawnPos = Vector3.zero;

            // If no last known position, generate a random one
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
                // Use the last known position
                spawnPos = npcValues.lastLocation;
            }

            // Spawn the NPC based on the position and saved data
            if (entityPrefab != null && buildingChecker != null)
            {
                GameObject newEntity = Instantiate(entityPrefab, spawnPos, Quaternion.identity);
                newEntity.GetComponentInChildren<RandomizeCharacter>().LoadExisting(npcValues); // Load saved NPC data
                newEntity.transform.parent = transform;
                EntityManager.instance.currentNPCs.Add(newEntity); // Add to NPC list
                amount++; // Increment the amount of spawned entities

                // Check for achievements
                try
                {
                    if (amount >= 30)
                    {
                        SteamIntegration.instance.UnlockAchievement("THIRTYVISITORS");
                    }

                    if (amount >= 50)
                    {
                        SteamIntegration.instance.UnlockAchievement("FIFTYVISITORS");
                    }

                    if (amount >= 100)
                    {
                        SteamIntegration.instance.UnlockAchievement("ONEHUNDREDVISITORS");
                    }
                }
                catch
                {
                    Debug.Log("Failed unlocking entity achievement");
                }
            }
        }
        onAmountUpdated?.Invoke(); // Notify about amount update
        maxAmount = WorldInteractables.instance.machineCount; // Update max amount based on machine count
    }

    #endregion Methods
}
