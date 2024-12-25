using UnityEngine;

public class ActivateAtDistance : MonoBehaviour
{
    GameObject target;
    public GameObject objectToActivate;
    public GameObject objectToCheckDistanceFrom;

    public float distanceToActivate = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(objectToCheckDistanceFrom.transform.position, target.transform.position);
        if (dist <= distanceToActivate)
        {
            objectToActivate.SetActive(true);
        }
        else
        {
            objectToActivate.SetActive(false);
        }
    }
}
