using UnityEngine;

public class PatientSpawner : MonoBehaviour
{
    public float currentWaitTime;
    public GameObject patient;
    public int numPatients = 5;
    public int minWaitTime = 0;
    public int maxWaitTime = 5;
    int currentNumpatients = 0; 
    private void Start()
    {
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }

    private void Update()
    {
        if (currentNumpatients == numPatients) return; 

        currentWaitTime -= Time.deltaTime;
        if (currentWaitTime <= 0)
        {
            currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
            Instantiate(patient, transform.position, Quaternion.identity);
            currentNumpatients++;
        }
    }
}
