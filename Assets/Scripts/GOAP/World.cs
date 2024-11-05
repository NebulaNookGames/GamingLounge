using System.Collections.Generic;
using UnityEngine;


public class ResourceQueue
{
    public Queue<GameObject> que = new Queue<GameObject>();
    public string tag;
    public string modState; 

    public ResourceQueue(string t, string ms, WorldStates w)
    {
        tag = t;
        modState = ms;

        if(t != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject g in resources)
            {
                que.Enqueue(g);
            }
        }

        if(modState != "")
        {
            w.ModifyState(modState, que.Count);
        }       
    }

    public void AddResource(GameObject r)
    {
        que.Enqueue(r);
    }

    public GameObject RemoveResource()
    {
        if (que.Count <= 0) return null;
        else
        {
            return que.Dequeue();
        }
    }
}

public sealed class World
{
    private static readonly World instance = new World();
    private static WorldStates world;
    // private static ResourceQueue patients;
    // private static ResourceQueue cubicles;
    // private static ResourceQueue offices;
    // private static ResourceQueue toilets;
    // private static ResourceQueue puddles;
    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static World()
    {
        world = new WorldStates();

        // patients = new ResourceQueue("", "", world);
        // resources.Add("patients", patients);
        //
        // cubicles = new ResourceQueue("Cubicle", "FreeCubicle", world);
        // resources.Add("cubicles", cubicles);
        //
        // offices = new ResourceQueue("Office", "FreeOffice", world);
        // resources.Add("offices", offices);
        //
        // toilets = new ResourceQueue("Toilet", "FreeToilet", world);
        // resources.Add("toilets", toilets);
        //
        // puddles = new ResourceQueue("Puddle", "FreePuddle", world);
        // resources.Add("puddles", puddles);
    }

    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }

    private World()
    {

    }

    public static World Instance { get { return instance; } }

    public WorldStates GetWorld() { return world; }
}