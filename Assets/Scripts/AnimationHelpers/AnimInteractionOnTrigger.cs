using System;
using UnityEngine;

public class AnimInteractionOnTrigger : MonoBehaviour
{
    [SerializeField] private string interactionTag;
    [SerializeField] private string parameterName = "interact";
    [SerializeField] private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(interactionTag)) return; 
        anim.SetBool(parameterName, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(interactionTag)) return; 
        anim.SetBool(parameterName, false);
    }
}