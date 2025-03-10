using UnityEngine;

public class HandleAnimationStarting : MonoBehaviour
{
    public Animator anim; // Reference to the Animator component
    private string boolName; // Name of the bool parameter to control the animation state

    /// <summary>
    /// Stops the animation by setting the given bool parameter to false.
    /// </summary>
    /// <param name="newBoolName">The name of the bool parameter to stop the animation.</param>
    public void StopAnimation(string newBoolName)
    {
        // Set the specified bool parameter to false to stop the animation
        anim.SetBool(newBoolName, false);
    }

    /// <summary>
    /// Starts the animation after a specified time delay by setting the bool parameter to true.
    /// </summary>
    /// <param name="invokeTime">Time delay before the animation starts (in seconds).</param>
    /// <param name="newBoolName">The name of the bool parameter to start the animation.</param>
    public void PlayAnimation(float invokeTime, string newBoolName)
    {
        boolName = newBoolName; // Store the bool parameter name
        // Invoke the Play method after the specified delay
        Invoke(nameof(Play), invokeTime);
    }

    /// <summary>
    /// Helper method to set the stored bool parameter to true, triggering the animation.
    /// </summary>
    void Play()
    {
        // Set the stored bool parameter to true to start the animation
        anim.SetBool(boolName, true);
    }
}