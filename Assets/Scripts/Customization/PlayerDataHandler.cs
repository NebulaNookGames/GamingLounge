using UnityEngine;
using System.Collections.Generic;
using CMF;

public class PlayerDataHandler : DataHandler
{
    // ============================
    // Serialized Fields
    // ============================

    // Reference to the player GameObject
    [SerializeField] public GameObject player; 

    // Array of possible materials that can be applied to the player's mesh
    [SerializeField] public Material[] possibleMaterials;

    // Reference to the SkinnedMeshRenderer that handles the player's material rendering
    [SerializeField] public SkinnedMeshRenderer skinnedMeshRenderer;
    
    // ============================
    // Override Methods from DataHandler
    // ============================

    // Method that receives saved data and updates the player's position and materials
    // This is called when loading a saved game.
    public override void ReceiveData(SaveData saveData)
    {
        // Set the player's position to the saved position
        player.transform.position = saveData.playerPosition;
        
        // If no material data exists in the saved data, exit early
        if (saveData.materialIndexes == null || saveData.materialIndexes.Length == 0)
            return; // No saved material data to load

        // Create a new array for the player's materials based on the skinned mesh renderer's material count
        Material[] newMaterials = new Material[skinnedMeshRenderer.materials.Length];

        // Loop through the materials and assign them based on the saved indexes
        for (int i = 0; i < newMaterials.Length; i++)
        {
            // Get the saved material index, or default to 0 if out of range
            int index = (i < saveData.materialIndexes.Length) ? saveData.materialIndexes[i] : 0; 

            // Ensure the index is valid, otherwise use the default material (index 0)
            if (index >= 0 && index < possibleMaterials.Length)
                newMaterials[i] = possibleMaterials[index];
            else
                newMaterials[i] = possibleMaterials[0]; // Fallback to default material if invalid index

        }

        // Assign the new materials to the SkinnedMeshRenderer
        skinnedMeshRenderer.materials = newMaterials; 
    }

    // Method that sends the current player data (position and material indexes) to be saved
    // This is called when saving the game.
    public override void SendData(SaveData saveData)
    {
        // Disable the player's movement controller temporarily while saving
        player.GetComponent<AdvancedWalkerController>().enabled = false;
        
        // Save the player's position to the save data
        saveData.playerPosition = player.transform.position;

        // Re-enable the player's movement controller after saving
        player.GetComponent<AdvancedWalkerController>().enabled = true;
        
        // If the SkinnedMeshRenderer is not set, log an error and stop the process
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is null! Cannot send material data.");
            return;
        }

        // If there are no possible materials to save, log an error and stop the process
        if (possibleMaterials == null || possibleMaterials.Length == 0)
        {
            Debug.LogError("possibleMaterials array is null or empty! Cannot map materials.");
            return;
        }

        // List to store the indexes of materials that are currently being used
        List<int> indexes = new List<int>();

        // Loop through all the materials applied to the player's SkinnedMeshRenderer
        foreach (Material currentlyUsedMat in skinnedMeshRenderer.materials)
        {
            // Skip if the material is null
            if (currentlyUsedMat == null) continue; 

            // Remove the "(Instance)" suffix (if present) to match the material name
            string matName = currentlyUsedMat.name.Replace(" (Instance)", ""); 

            // Find the index of the material in the possibleMaterials array by name
            int foundIndex = System.Array.FindIndex(possibleMaterials, mat => mat.name == matName);

            // If the material is not found in the list, log a warning and default to index 0
            if (foundIndex == -1)
            {
                Debug.LogWarning($"Material '{matName}' not found in possibleMaterials. Using default index 0.");
                foundIndex = 0; // Fallback to index 0 if material is not found
            }

            // Add the found index to the list
            indexes.Add(foundIndex);
        }

        // Save the list of material indexes into the save data
        saveData.materialIndexes = indexes.ToArray(); 
    }
}
