using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GameObjectChangerOnInput : MonoBehaviour
{
    private int currentIndex = 0;
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField] public InputActionProperty interactAction;

    private Vector3Int vector3IntPosition;

    [SerializeField] private AudioSource audioS;
    [SerializeField] private AudioClip audioC;
    private void OnEnable()
    {
        interactAction.action.performed += ChangeActiveGameObject; 
        int x = (int)transform.parent.position.x;
        int y = (int)transform.parent.position.y;
        int z = (int)transform.parent.position.z;
        vector3IntPosition = new Vector3Int(x, y, z);
    }

    private void OnDisable()
    {
        interactAction.action.performed -= ChangeActiveGameObject; 
    }

    void ChangeActiveGameObject(InputAction.CallbackContext context)
    {
        if (currentIndex + 1 < gameObjects.Count)
        {
            gameObjects[currentIndex].gameObject.SetActive(false);
            currentIndex++;
            gameObjects[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            gameObjects[currentIndex].gameObject.SetActive(false);
            currentIndex = 0;
            gameObjects[currentIndex].gameObject.SetActive(true);
        }
        if(audioS && audioC)
            audioS.PlayOneShot(audioC);
        
        PlacementDataHandler.instance.ChangeCustomizedObjectID(vector3IntPosition, currentIndex);
    }

    public void ChangeActiveGameObjectByIndex(int index)
    {
        Debug.Log("Change active game object");
        gameObjects[currentIndex].SetActive(false);
        gameObjects[index].SetActive(true);
        currentIndex = index;
    }
}
