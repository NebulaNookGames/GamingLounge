using UnityEngine;

public class Nurse : Agent
{
    new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("treatPatient", 1, false);
        goals.Add(s1, 2);

        SubGoal s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);

        SubGoal s3 = new SubGoal("relief", 2, false);
        goals.Add(s3, 3);

        Invoke("GetTired", Random.Range(10,20));
        Invoke("NeedRelief", Random.Range(10, 20));
    }

    void GetTired()
    {
        beliefs.ModifyState("exhausted", 0);
        Invoke("GetTired", Random.Range(30, 50));
    }

    void NeedRelief()
    {
        beliefs.ModifyState("haveToPee", 0);
        Invoke("NeedRelief", Random.Range(10, 20));
    }
}