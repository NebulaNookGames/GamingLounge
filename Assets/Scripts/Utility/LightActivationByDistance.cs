using UnityEngine;

public class LightActivationByDistance : MonoBehaviour
{
    public string tagOfOtherObject;
    GameObject otherObject;
    public float distance;
    public float waitTime = 1f;
    private float timer = 0f;

    public Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     otherObject = GameObject.FindGameObjectWithTag(tagOfOtherObject);   
     timer = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = waitTime;
            if (Vector3.Distance(transform.position, otherObject.transform.position) < distance)
            {
                light.enabled = true; 
            }
            else
            {
                light.enabled = false;
            }
        }
    }
}