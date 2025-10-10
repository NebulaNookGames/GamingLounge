using UnityEngine;

public class RandomizeCharacter : MonoBehaviour
{
    #region Serialized Fields

    [Header("Character Prefabs")]
    [Tooltip("Array of different character GameObjects.")]
    [SerializeField] private GameObject[] characters; // List of character prefabs
    
    [Header("Character Meshes")]
    [Tooltip("Array of character meshes used in different LOD levels.")]
    [SerializeField] private GameObject[] characterMeshes; // Meshes for the character
    
    [Header("Level of Detail (LOD) Meshes")]
    [Tooltip("LOD level 1 meshes for characters.")]
    [SerializeField] private GameObject[] lodLevel01CharacterMeshes; // LOD Level 1 meshes
    
    [Tooltip("LOD level 2 meshes for characters.")]
    [SerializeField] private GameObject[] lodLevel02CharacterMeshes; // LOD Level 2 meshes
    
    [Header("Mesh Material Indexes")]
    [Tooltip("Indexes to access head materials for character customization.")]
    [SerializeField] private int[] headIndexes; // Head material index for different characters
    
    [Tooltip("Indexes to access cloth materials for character customization.")]
    [SerializeField] private int[] clothIndexes; // Cloth material index for different characters
    
    [Header("NPC Value Holder")]
    [Tooltip("Reference to the NPC Value Holder.")]
    [SerializeField] private NPCValueHolder npcValueHolder; // Reference to NPC value holder
    
    #endregion Serialized Fields

    #region References

    public VisitorEntity entity; // Reference to the VisitorEntity to interact with the NPC

    #endregion References

    #region Unity Methods

    /// <summary>
    /// Initializes the character meshes by deactivating all characters initially.
    /// </summary>
    private void Awake()
    {
        foreach (GameObject character in characters)
        {
            character.SetActive(false); // Deactivate all characters at the start
        }
    }

    #endregion Unity Methods

    #region Methods

    /// <summary>
    /// Randomly generates a new character by selecting a random character and applying random color to their materials.
    /// </summary>
    public void GenerateNew()
    {
        int randomIndex = Random.Range(0, characters.Length); // Randomly select a character

        for (int i = 0; i < characters.Length; i++)
        {
            if(i != randomIndex)
                Destroy(characters[i]);
        }
        
        characters[randomIndex].SetActive(true); // Activate the selected character
        entity.EntityAnimator = characters[randomIndex].GetComponent<Animator>(); // Set the character's animator
        
        int randomColor = Random.Range(0, 9); // Randomly select a color index
        
        // Apply the selected random color to the character's meshes
        ApplyCharacterColor(randomIndex, randomColor);
        
        // Create a new NPCValue object to store the character's generated data
        NPCValues npcValues = new NPCValues(randomIndex, randomColor, false);
        EntityManager.instance.npcValues.Add(npcValues); // Add the NPC values to the entity manager
        npcValueHolder.Values = npcValues; // Set the NPC values in the NPC Value Holder
    }

    /// <summary>
    /// Loads the existing character values and applies them to the NPC.
    /// </summary>
    /// <param name="npcValues">The NPC values to load into the character.</param>
    public void LoadExisting(NPCValues npcValues)
    {
        characters[npcValues.randomIndex].SetActive(true); // Activate the character based on saved index
        entity.invitedToLounge = npcValues.invitedToLounge; // Set the 'invitedToLounge' state
        entity.EntityAnimator = characters[npcValues.randomIndex].GetComponent<Animator>(); // Set the animator
        
        // Apply the saved color to the character's meshes
        ApplyCharacterColor(npcValues.randomIndex, npcValues.colorIndex);
        
        npcValueHolder.Values = npcValues; // Set the saved NPC values in the value holder
    }

    #endregion Methods

    #region Helper Methods

    /// <summary>
    /// Applies the selected color to the character's head and cloth materials.
    /// </summary>
    /// <param name="characterIndex">The index of the character being modified.</param>
    /// <param name="colorIndex">The index of the color to be applied.</param>
    private void ApplyCharacterColor(int characterIndex, int colorIndex)
    {
        // Apply the color to character meshes
        characterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[characterIndex]].SetFloat("_Hue", colorIndex);
        characterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[characterIndex]].SetFloat("_Hue", colorIndex);

        // // Apply color to LOD Level 1 meshes
        // lodLevel01CharacterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[characterIndex]].SetFloat("_Hue", colorIndex);
        // lodLevel01CharacterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[characterIndex]].SetFloat("_Hue", colorIndex);
        //
        // // Apply color to LOD Level 2 meshes
        // lodLevel02CharacterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[characterIndex]].SetFloat("_Hue", colorIndex);
        // lodLevel02CharacterMeshes[characterIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[characterIndex]].SetFloat("_Hue", colorIndex);
    }

    #endregion Helper Methods
}
