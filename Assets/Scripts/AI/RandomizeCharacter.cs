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
        
        int randomIndex = Random.Range(0, characters.Length);
        
        characters[randomIndex].SetActive(true);
        graphAgent.BlackboardReference.SetVariableValue("Anim", characters[randomIndex].GetComponent<Animator>());
        
        int randomColor = Random.Range(0, 9);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[randomIndex]].SetFloat("_Hue", randomColor);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[randomIndex]].SetFloat("_Hue", randomColor);
    }
}