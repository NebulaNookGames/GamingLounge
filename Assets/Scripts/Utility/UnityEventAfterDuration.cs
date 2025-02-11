using System;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventAfterDuration : MonoBehaviour
{
 public UnityEvent OnAfterDuration;
 public float duration = 2f; 
 
 private void Start()
 { 
  Invoke(nameof(InvokeEvent), duration);
 }

 void InvokeEvent()
 {
  OnAfterDuration?.Invoke();
 }
}
