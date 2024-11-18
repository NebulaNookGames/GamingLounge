using System;
using UnityEditor;
using UnityEngine;

public class RandomizeCharacter : MonoBehaviour
{
    [SerializeField] private bool doIt = false; 
    [SerializeField] private GameObject[] bodies;
    [SerializeField] private GameObject[] heads;
    [SerializeField] private GameObject headMesh;
    private int materialIndex; 
    private void Awake()
    {
        if (!doIt) return; 
        
        foreach (GameObject body in bodies)
        {
            body.SetActive(false);
        }

        foreach (GameObject head in heads)
        {
            head.SetActive(false);
        }
        
        int randomIndex = UnityEngine.Random.Range(0, bodies.Length);
        
        bodies[randomIndex].SetActive(true);
        heads[randomIndex].SetActive(true);
        // switch (randomIndex) 
        // {
        //     case 0:
        //         materialIndex = 0;
        //         break;
        //     case 1:
        //         materialIndex = 3;
        //         break;
        //     case 2:
        //         materialIndex = 0;
        //         break;
        // }
        // float randomColorIndex = UnityEngine.Random.Range(0, 1);
        // headMesh.GetComponent<MeshRenderer>().materials[materialIndex].SetFloat("_Hue", randomColorIndex);
    }
}