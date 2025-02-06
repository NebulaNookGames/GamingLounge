using UnityEngine;
using UnityEngine.Events;

public class DeactivateAtDistance : MonoBehaviour
{
    public GameObject target;
    public GameObject objectToCheckDistanceFrom;
    public GameObject objectToActivateOnDeactivation;
    public UnityEvent onDeactivate;
    public float distanceToActivate = 1;

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(objectToCheckDistanceFrom.transform.position, target.transform.position);
        if (dist <= distanceToActivate)
        {
            onDeactivate?.Invoke();
            objectToActivateOnDeactivation.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}