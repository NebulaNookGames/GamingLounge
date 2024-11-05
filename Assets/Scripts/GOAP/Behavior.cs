using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public abstract class Behavior : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public WorldStates agentBeliefs;

    public bool running = false;

    public Inventory inventory;
    public WorldStates beliefs; 

    public Behavior()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if(preConditions != null )
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }

        inventory = this.GetComponent<Agent>().inventory;
        beliefs = this.GetComponent<Agent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> p in preconditions)
        {
            if(!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true; 
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
