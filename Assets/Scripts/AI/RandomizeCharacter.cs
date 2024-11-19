using System;
using UnityEditor;
using UnityEngine;

public class RandomizeCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    
    private void Awake()
    {
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
        
        int randomIndex = UnityEngine.Random.Range(0, characters.Length);
        
        characters[randomIndex].SetActive(true);
    }
}