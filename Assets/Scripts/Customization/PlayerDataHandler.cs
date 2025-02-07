using UnityEngine;
using System.Collections.Generic;
using CMF;

public class PlayerDataHandler : DataHandler
{
    public GameObject player; 
    public Material[] possibleMaterials;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    
    public override void ReceiveData(SaveData saveData)
    {
        player.transform.position = saveData.playerPosition;
        
        if (saveData.materialIndexes == null || saveData.materialIndexes.Length == 0)
            return; // No saved data

        Material[] newMaterials = new Material[skinnedMeshRenderer.materials.Length];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            int index = (i < saveData.materialIndexes.Length) ? saveData.materialIndexes[i] : 0; // Default to 0 if out of range

            if (index >= 0 && index < possibleMaterials.Length)
                newMaterials[i] = possibleMaterials[index];
            else
                newMaterials[i] = possibleMaterials[0]; // Fallback material
        }

        skinnedMeshRenderer.materials = newMaterials; // Assign new materials
    }

    public override void SendData(SaveData saveData)
    {
        player.GetComponent<AdvancedWalkerController>().enabled = false;
        saveData.playerPosition = player.transform.position;
        player.GetComponent<AdvancedWalkerController>().enabled = true;
        
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer is null! Cannot send material data.");
            return;
        }

        if (possibleMaterials == null || possibleMaterials.Length == 0)
        {
            Debug.LogError("possibleMaterials array is null or empty! Cannot map materials.");
            return;
        }

        List<int> indexes = new List<int>();

        foreach (Material currentlyUsedMat in skinnedMeshRenderer.materials)
        {
            if (currentlyUsedMat == null) continue; // Skip if null

            string matName = currentlyUsedMat.name.Replace(" (Instance)", ""); // Remove " (Instance)"
            int foundIndex = System.Array.FindIndex(possibleMaterials, mat => mat.name == matName);

            if (foundIndex == -1)
            {
                Debug.LogWarning($"Material '{matName}' not found in possibleMaterials. Using default index 0.");
                foundIndex = 0; // Fallback to index 0
            }

            indexes.Add(foundIndex);
        }

        saveData.materialIndexes = indexes.ToArray(); // Save indexes
    }
}