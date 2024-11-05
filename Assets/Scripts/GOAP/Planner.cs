using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public Behavior Behavior; 

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Behavior _behavior)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        this.Behavior = _behavior; 
    }

    public Node(Node parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, Behavior _behavior)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        foreach(KeyValuePair<string, int> b in beliefStates)
        {
            if(!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }
        }

        this.Behavior = _behavior;
    }
}

public class Planner
{
    public Queue<Behavior> plan(List<Behavior> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<Behavior> usableActions = new List<Behavior>();
        foreach(Behavior a in actions)
        {
            if(a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, World.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null; 
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
                cheapest = leaf;
            else if(leaf.cost < cheapest.cost)
                cheapest = leaf; 
        }

        List<Behavior> result = new List<Behavior>();
        Node n = cheapest;
        while(n != null)
        {
            if(n.Behavior != null)
            {
                result.Insert(0, n.Behavior);
            }
            n = n.parent;
        }

        Queue<Behavior> queue = new Queue<Behavior>();
        foreach(Behavior a in result)
        {
            queue.Enqueue(a);
        }
        // Debug.Log("The Plan is: ");
        foreach(Behavior a in queue)
        {
            // Debug.Log("Q: " + a.actionName);
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<Behavior> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach(Behavior action in usableActions)
        {
            if(action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if(!currentState.ContainsKey(eff.Key))
                        currentState.Add(eff.Key, eff.Value);
                }

                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<Behavior> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundPath = true;                   
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if(!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true; 
    }

    private List<Behavior> ActionSubset(List<Behavior> actions, Behavior removeMe)
    {
        List<Behavior> subset = new List<Behavior>();
        foreach(Behavior a in actions)
        {
            if(!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }


        return subset;
    }
}
