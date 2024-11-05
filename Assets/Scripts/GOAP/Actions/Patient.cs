using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Patient : Agent
{
    new void Start()
    {
        base.Start();

        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 2);

        SubGoal s2 = new SubGoal("isTreated", 1, true);
        goals.Add(s2, 3);

        SubGoal s3 = new SubGoal("isHome", 1, true);
        goals.Add(s3, 1);

        SubGoal s4 = new SubGoal("relief", 1, false);
        goals.Add(s4, 4);

        Invoke("NeedRelief", Random.Range(10, 20));
    }

    void NeedRelief()
    {
        beliefs.ModifyState("haveToPee", 0);
        Invoke("NeedRelief", Random.Range(10, 20));
    }
}