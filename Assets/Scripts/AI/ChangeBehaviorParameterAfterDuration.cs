using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Behavior;
public class ChangeBehaviorParameterAfterDuration : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private bool randomDuration;
    [SerializeField] private float minDuration;
    [SerializeField] private float maxDuration;
    [SerializeField] private string parameterName;
    private void Awake()
    {
        if(randomDuration)
            duration = Random.Range(minDuration, maxDuration);
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            GetComponent<BehaviorGraphAgent>().BlackboardReference.SetVariableValue(parameterName, true);
            Destroy(this);
        }
    }
} 
