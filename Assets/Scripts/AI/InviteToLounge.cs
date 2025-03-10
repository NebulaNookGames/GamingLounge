using UnityEngine;
using UnityEngine.InputSystem;

public class InviteToLounge : MonoBehaviour
{
    #region Variables

    [Header("Interaction Settings")]
    [Tooltip("Object to check if it is active or not for interaction.")]
    public GameObject objectToCheckActiveState;

    [Tooltip("AudioSource component to play the invitation sound.")]
    public AudioSource audioS;

    [Tooltip("Audio clip for the invitation sound.")]
    public AudioClip inviteClip;

    private bool interactedWith = false; // Flag to prevent multiple interactions

    [Header("Visual Effects")]
    [Tooltip("Particle effect to show when the visitor is invited.")]
    public GameObject invitedParticleEffect;

    [Tooltip("Indicator to show if the visitor is not invited.")]
    public GameObject notInvitedIndicator;

    [Header("Visitor Data")]
    [Tooltip("The VisitorEntity associated with this interaction.")]
    public VisitorEntity entity;

    [Tooltip("Input action for object interaction.")]
    public InputActionProperty objectInteractionAction;

    #endregion Variables

    #region Unity Methods

    /// <summary>
    /// Subscribes to the interaction action when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        objectInteractionAction.action.performed += Invite;
    }

    /// <summary>
    /// Unsubscribes from the interaction action when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        objectInteractionAction.action.performed -= Invite;
    }

    /// <summary>
    /// Initializes necessary components and checks the visitor's initial state.
    /// </summary>
    private void Start()
    {
        audioS = GetComponent<AudioSource>(); // Get the AudioSource component
        Invoke(nameof(InitialCheck), 0.5f); // Initial check for already invited visitors
    }

    #endregion Unity Methods

    #region Interaction Methods

    /// <summary>
    /// Handles the invitation process when the object is interacted with.
    /// </summary>
    /// <param name="context">Input action context for the interaction.</param>
    void Invite(InputAction.CallbackContext context)
    {
        // Prevent multiple interactions or if the object is not active
        if (interactedWith || !objectToCheckActiveState.activeSelf) return;

        interactedWith = true; // Mark the interaction as done

        // Update the visitor's state and spawn visual effects
        entity.invitedToLounge = true;
        Instantiate(invitedParticleEffect, transform.position, Quaternion.identity);
        Destroy(notInvitedIndicator); // Hide the "not invited" indicator

        // Update the NPC values in the EntityManager
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        EntityManager.instance.npcValues[index].invitedToLounge = true;

        // Play the invitation sound
        audioS.PlayOneShot(inviteClip);

        // Destroy this object and related components after a delay
        Invoke(nameof(DestroyObjects), 0.5f);
    }

    /// <summary>
    /// Checks the initial state of the visitor (whether already invited).
    /// </summary>
    void InitialCheck()
    {
        GameObject thisInList = EntityManager.instance.currentNPCs.Find(obj => obj == gameObject);
        if (thisInList == null) return;

        // Get the index of the current NPC
        int index = EntityManager.instance.currentNPCs.IndexOf(thisInList);
        
        // If the visitor was previously invited, update the state
        if (EntityManager.instance.npcValues[index].invitedToLounge)
        {
            entity.invitedToLounge = true;
            notInvitedIndicator.SetActive(false); // Hide the not invited indicator
            Invoke(nameof(DestroyObjects), 0.5f); // Destroy the objects after a delay
        }
    }

    #endregion Interaction Methods

    #region Helper Methods

    /// <summary>
    /// Destroys unnecessary components and objects after interaction.
    /// </summary>
    void DestroyObjects()
    {
        Destroy(GetComponent<ActivateAtDistance>()); // Destroy distance-based activation component
        Destroy(objectToCheckActiveState); // Destroy the object being checked for activity
        Destroy(this); // Destroy this script
    }

    #endregion Helper Methods
}
