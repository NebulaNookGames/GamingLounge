using UnityEngine;

public class ActivateAtDistance : MonoBehaviour
{
    GameObject target;
    public GameObject objectToActivate;
    public GameObject objectToCheckDistanceFrom;
    public float distanceToActivate = 1;
    
    // Update is called once per frame
    void Update()
    {
        if(target == null && GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player");
        else if (target != null)
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
}
