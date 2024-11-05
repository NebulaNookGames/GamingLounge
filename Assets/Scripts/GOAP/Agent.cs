using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}

public class Agent : MonoBehaviour
{
    public List<Behavior> actions = new List<Behavior>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public Inventory inventory = new Inventory();
    public WorldStates beliefs = new WorldStates();

    Planner planner;
    Queue<Behavior> actionQueue;
    [FormerlySerializedAs("currentAction")] public Behavior currentBehavior;
    SubGoal currentGoal;

    Vector3 destination = Vector3.zero;

    protected virtual void Start()
    {
        Behavior[] acts = this.GetComponents<Behavior>();
        foreach(Behavior act in acts)
        {
            actions.Add(act);
        }
    }

    bool invoked = false; 

    void CompleteAction()
    {
        currentBehavior.running = false;
        currentBehavior.PostPerform();
        invoked = false;
    }

    private void LateUpdate()
    {
        if(currentBehavior != null && currentBehavior.running)
        {
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            if(distanceToTarget < 2f)
            {
                if(!invoked)
                {
                    Invoke("CompleteAction", currentBehavior.duration);
                    invoked = true;
                }
            }
            return;
        }

        if(planner == null || actionQueue == null)
        {
            planner = new Planner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.plan(actions, sg.Key.sgoals, beliefs);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if(currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null; 
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentBehavior = actionQueue.Dequeue();
            if(currentBehavior.PrePerform())
            {
                if (currentBehavior.target == null && currentBehavior.targetTag != "")
                    currentBehavior.target = GameObject.FindWithTag(currentBehavior.targetTag);

                if(currentBehavior.target != null)
                {
                    currentBehavior.running = true;

                    destination = currentBehavior.target.transform.position;
                    Transform dest = currentBehavior.target.transform.Find("NavPoint");
                    
                    if(dest != null)
                    {
                        destination = dest.position;
                    }

                    currentBehavior.agent.SetDestination(destination);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}