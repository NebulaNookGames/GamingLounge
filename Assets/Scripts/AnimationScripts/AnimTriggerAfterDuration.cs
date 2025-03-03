using System;
using UnityEngine;

public class AnimTriggerAfterDuration : MonoBehaviour
{
    public string triggerName;
    public Animator animator;
    public float duration;
    private float timer;
    
    public float sitDownWaitDuration;
    private float sitDownTimer; 
    private void Start()
    {
        timer = duration;
        sitDownTimer = sitDownWaitDuration;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            animator.SetTrigger(triggerName);
            timer = duration;
        }

        if (sitDownTimer > -1)
        {
            sitDownTimer -= Time.deltaTime;
            if (sitDownTimer <= 0)
            {
                animator.SetTrigger("SitDown");
            }
        }

        if (animator.GetFloat("HorizontalSpeed") > 1)
        {
            animator.ResetTrigger("SitDown");
            sitDownTimer = sitDownWaitDuration;
        }
    }
}