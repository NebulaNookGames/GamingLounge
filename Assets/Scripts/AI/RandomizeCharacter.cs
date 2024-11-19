using System;
using UnityEditor;
using UnityEngine;

public class RandomizeCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject[] characterMeshes;
    [SerializeField] private int[] headIndexes;
    [SerializeField] private int[] clothIndexes;
    private void Awake()
    {
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
        
        int randomIndex = UnityEngine.Random.Range(0, characters.Length);
        
        characters[randomIndex].SetActive(true);

        int randomColor = UnityEngine.Random.Range(0, 9);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[headIndexes[randomIndex]].SetFloat("_Hue", randomColor);
        characterMeshes[randomIndex].GetComponent<SkinnedMeshRenderer>().materials[clothIndexes[randomIndex]].SetFloat("_Hue", randomColor);

    }
}