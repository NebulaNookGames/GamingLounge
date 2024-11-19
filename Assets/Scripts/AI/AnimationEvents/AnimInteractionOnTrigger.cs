using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class triggers animation changes when an object with a specified tag enters or exits the trigger area.
/// It sets a boolean parameter in the Animator to control animation states.
/// </summary>
public class AnimInteractionOnTrigger : MonoBehaviour
{
    // The tag of the object that will trigger the animation change.
    [SerializeField] private string tagToCompare = "Entity";

    // The name of the Animator parameter to control (e.g., a bool parameter).
    [SerializeField] private string parameterName = "interact";

    // Reference to the Animator that controls the animation.
    [SerializeField] private Animator anim;

    private List<GameObject> objectsInTrigger = new List<GameObject>();

    /// <summary>
    /// Called when another collider enters the trigger area. Sets the Animator parameter to true.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagToCompare)) return; 
        if (objectsInTrigger.Contains(other.gameObject)) return;

        objectsInTrigger.Add(other.gameObject);
        anim.SetBool(parameterName, true);
    }

    /// <summary>
    /// Called when another collider exits the trigger area. Sets the Animator parameter to false.
    /// </summary>
    /// <param name="other">The collider of the object exiting the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(tagToCompare)) return; 
        if (!objectsInTrigger.Contains(other.gameObject)) return; 
        
        objectsInTrigger.Remove(other.gameObject);

        if(objectsInTrigger.Count <= 0)
            anim.SetBool(parameterName, false);
    }
}