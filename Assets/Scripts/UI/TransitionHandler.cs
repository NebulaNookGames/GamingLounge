using System;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [Tooltip("The animator that handles the transitiong.")] [SerializeField]
    Animator transitionAnimator;

    [Tooltip("The name of the trigger parameter inside of the transitionAnimator that starts the transition.")]
    [SerializeField]
    string transitionInTrigger;
}
