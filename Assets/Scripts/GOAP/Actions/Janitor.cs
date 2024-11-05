using UnityEngine;

public class Janitor : Agent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("clean", 1, false);
        goals.Add(s1, 1);
    }
}