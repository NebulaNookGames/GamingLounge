using UnityEngine;

public class Visitor : Agent
{
    new void Start()
    {
        base.Start();
        
        SubGoal s1 = new SubGoal("GoToArcadeMachine", 1, false);
        goals.Add(s1, 1);
        SubGoal s2 = new SubGoal("PlayArcadeMachine", 1, false);
        goals.Add(s2, 2);
    }
}
