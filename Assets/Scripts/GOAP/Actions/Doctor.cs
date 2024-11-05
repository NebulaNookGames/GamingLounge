using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Doctor : Agent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("research", 1, false);
        goals.Add(s1, 1);

        SubGoal s2 = new SubGoal("relief", 1, false);
        goals.Add(s2, 2);

        SubGoal s3 = new SubGoal("rested", 1, false);
        goals.Add(s3, 3);

 

     

        Invoke("GetTired", Random.Range(5,10));
        Invoke("NeedToPee", Random.Range(5, 10));
    }

    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke("GetTired", Random.Range(5, 10));
    }

    void NeedToPee()
    {
        beliefs.ModifyState("haveToPee", 0);
        Invoke("NeedToPee", Random.Range(5, 10));
    }
}