using UnityEngine;
using UnityEngine.Serialization;

public class StateMonitor : MonoBehaviour
{
    public string state;
    public float stateStrength;
    public float stateDecayRate;
    public WorldStates beliefs;
    public GameObject resourcePrefab;
    public string queueName;
    public string worldState;
    [FormerlySerializedAs("action")] public Behavior behavior;

    bool stateFound = false;
    float initialStrength;

    private void Awake()
    {
        beliefs = GetComponent<Agent>().beliefs;
        initialStrength = stateStrength;
    }

    private void LateUpdate()
    {
        if(behavior.running)
        {
            stateFound = false;
            stateStrength = initialStrength;
        }

        if(!stateFound && beliefs.HasState(state))
        {
            stateFound = true; 
        }

        if(stateFound)
        {
            stateStrength -= stateDecayRate * Time.deltaTime;

            if(stateStrength <= 0)
            {
                Vector3 location = new Vector3(transform.position.x, resourcePrefab.transform.position.y, transform.position.z);
                GameObject g = Instantiate(resourcePrefab, location, resourcePrefab.transform.rotation);
                stateFound = false;
                stateStrength = initialStrength;
                beliefs.RemoveState(state);
                World.Instance.GetQueue(queueName).AddResource(g);
                World.Instance.GetWorld().ModifyState(worldState, 1);
            }
        }
    }
}
