using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class triggers animation changes when an object with a specified tag enters or exits the trigger area.
/// It sets a boolean parameter in the Animator to control animation states.
/// </summary>
public class AnimInteractionOnTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("The tags of objects that will trigger the animation change.")]
    [SerializeField] private string[] tagsToCompare;

    [Header("Animator Settings")]
    [Tooltip("The name of the Animator parameter to control (e.g., a bool parameter).")]
    [SerializeField] private string parameterName = "interact";

    [Tooltip("Reference to the Animator that controls the animation.")]
    [SerializeField] private Animator anim;

    // List to keep track of objects currently in the trigger area
    private List<GameObject> objectsInTrigger = new List<GameObject>();

    [Header("Timing Settings")]
    [Tooltip("Time interval for checking objects in the trigger zone.")]
    [SerializeField] private float updateTime = 1f; 
    
    private float timer;

    private void Awake()
    {
        timer = updateTime;
    }

    /// <summary>
    /// Called when another collider enters the trigger area. Adds the object to the list and triggers the animation.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        foreach (string currentTag in tagsToCompare)
        {
            if (other.CompareTag(currentTag))
            {
                if (objectsInTrigger.Contains(other.gameObject)) return;

                objectsInTrigger.Add(other.gameObject);
                anim.SetBool(parameterName, true);
                return;
            }
        }
    }

    /// <summary>
    /// Updates the list of objects inside the trigger zone and resets animation state if necessary.
    /// </summary>
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            objectsInTrigger.RemoveAll(ob => ob == null);
            
            if (objectsInTrigger.Count <= 0)
                anim.SetBool(parameterName, false);
            
            timer = updateTime;
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger area. Removes the object from the list and updates the animation state.
    /// </summary>
    /// <param name="other">The collider of the object exiting the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        foreach (string currentTag in tagsToCompare)
        {
            if (other.CompareTag(currentTag))
            {
                if (!objectsInTrigger.Contains(other.gameObject)) return;

                objectsInTrigger.Remove(other.gameObject);

                if (objectsInTrigger.Count <= 0)
                    anim.SetBool(parameterName, false);
                return;
            }
        }
    }
}