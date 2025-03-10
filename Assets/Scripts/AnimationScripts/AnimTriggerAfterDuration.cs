using System;
using UnityEngine;

public class AnimTriggerAfterDuration : MonoBehaviour
{
    // The name of the trigger to activate in the Animator
    public string triggerName;
    // The Animator component to control
    public Animator animator;
    // Duration before the trigger is activated
    public float duration;
    private float timer;

    // Duration to wait before triggering the "SitDown" animation
    public float sitDownWaitDuration;
    private float sitDownTimer;

    /// <summary>
    /// Initializes the timers for animation triggers at the start.
    /// </summary>
    private void Start()
    {
        timer = duration; // Set the initial duration timer
        sitDownTimer = sitDownWaitDuration; // Set the initial sit-down timer
    }

    /// <summary>
    /// Update is called once per frame to manage animation triggers based on timers.
    /// </summary>
    private void Update()
    {
        // Decrease the main animation trigger timer
        timer -= Time.deltaTime;
        
        // If the timer has elapsed, trigger the specified animation and reset the timer
        if (timer <= 0)
        {
            animator.SetTrigger(triggerName);
            timer = duration; // Reset the timer to continue triggering at intervals
        }

        // Handle the sit down animation
        if (sitDownTimer > -1) // Only decrease the timer if it's above -1
        {
            sitDownTimer -= Time.deltaTime;
            
            // If the sit-down wait duration has passed, trigger "SitDown" animation
            if (sitDownTimer <= 0)
            {
                animator.SetTrigger("SitDown");
            }
        }

        // If the character's horizontal speed is greater than 1, reset the sit-down animation
        if (animator.GetFloat("HorizontalSpeed") > 1)
        {
            animator.ResetTrigger("SitDown");
            sitDownTimer = sitDownWaitDuration; // Reset the sit-down wait duration timer
        }
    }
}