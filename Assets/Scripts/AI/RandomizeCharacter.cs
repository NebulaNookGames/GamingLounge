using UnityEngine;
using Unity.Behavior; 

public class RandomizeCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject[] characterMeshes;
    [SerializeField] private int[] headIndexes;
    [SerializeField] private int[] clothIndexes;
    [SerializeField] private BehaviorGraphAgent graphAgent; 
    private void Awake()
    {
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
    }

    public void GenerateNew()
    {
        int randomIndex = Random.Range(0, characters.Length);
        
        characters[randomIndex].SetActive(true);
        graphAgent.BlackboardReference.SetVariableValue("Anim", characters[randomIndex].GetComponent<Animator>());
        
        int randomColor = Random.Range(0, 9);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[randomIndex]].SetFloat("_Hue", randomColor);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[randomIndex]].SetFloat("_Hue", randomColor);
        NPCValues npcValues = new NPCValues(randomIndex, randomColor, false);
        EntityManager.instance.npcValues.Add(npcValues);
    }

    public void LoadExisting(NPCValues npcValues)
    {
        characters[npcValues.randomIndex].SetActive(true);
        graphAgent.BlackboardReference.SetVariableValue("InvitedToLounge", npcValues.invitedToLounge);
        graphAgent.BlackboardReference.SetVariableValue("Anim", characters[npcValues.randomIndex].GetComponent<Animator>());
        characterMeshes[npcValues.randomIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[npcValues.randomIndex]].SetFloat("_Hue", npcValues.colorIndex);
        characterMeshes[npcValues.randomIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[npcValues.randomIndex]].SetFloat("_Hue", npcValues.colorIndex);
    }
}