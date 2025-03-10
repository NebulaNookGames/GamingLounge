using UnityEngine;

public class PlayFootStepParticles : MonoBehaviour
{
    #region Serialized Fields

    // References to the left and right foot GameObjects
    public GameObject leftFoot;
    public GameObject rightFoot;

    #endregion

    #region Private Variables

    // Flag to control if particles can be played
    private bool canPlay = true;

    // Flag to toggle between left and right foot
    private bool leftNext = true;

    #endregion

    #region Public Methods

    // Method to play the left foot walking particle
    public void PlayLeftWalkingParticle()
    {
        // If particles can't be played or it's not the left foot's turn, exit the method
        if (!canPlay || !leftNext) return;

        // Prevent playing more particles temporarily
        canPlay = false;
        
        // Set the flag so the right foot will play next
        leftNext = false;
        
        // Spawn the left foot print particle effect at the left foot's position and rotation
        ObjectPool.instance.SpawnFootprintEffect(leftFoot.transform.position, leftFoot.transform.rotation);

        // Allow particles to be played again after a short delay
        Invoke(nameof(Allow), 0.1f);
    }

    // Method to play the right foot walking particle
    public void PlayRightWalkingParticle()
    {
        // If particles can't be played or it's not the right foot's turn, exit the method
        if (!canPlay || leftNext) return;

        // Prevent playing more particles temporarily
        canPlay = false;

        // Set the flag so the left foot will play next
        leftNext = true;

        // Spawn the right foot print particle effect at the right foot's position and rotation
        ObjectPool.instance.SpawnFootprintEffect(rightFoot.transform.position, rightFoot.transform.rotation);

        // Allow particles to be played again after a short delay
        Invoke(nameof(Allow), 0.1f);
    }

    #endregion

    #region Private Methods

    // Method to allow playing of footstep particles again after a delay
    private void Allow()
    {
        canPlay = true;
    }

    #endregion
}
