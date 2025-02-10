using System;
using UnityEngine;
using UnityEngine.Events;

public class DeactivateAtDistance : MonoBehaviour
{
    public GameObject target;
    public GameObject objectToCheckDistanceFrom;
    public GameObject objectToActivateOnDeactivation;
    public UnityEvent onDeactivate;
    public float distanceToActivate = 1;

    private float checkInterval = .5f;

    private float timer;

    private void Awake()
    {
        timer = checkInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = checkInterval;
            float dist = Vector3.Distance(objectToCheckDistanceFrom.transform.position, target.transform.position);
            if (dist <= distanceToActivate)
            {
                onDeactivate?.Invoke();
                objectToActivateOnDeactivation.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}