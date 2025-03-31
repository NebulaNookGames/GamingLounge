using System;
using UnityEngine;

public class DisableAnimatorAfterDuration : MonoBehaviour
{
    public Animator anim;
    public float duration = 2f; 
    private void Awake()
    {
        Invoke(nameof(DisableAnimator), duration);
    }

    void DisableAnimator()
    {
        anim.enabled = false;
    }
}