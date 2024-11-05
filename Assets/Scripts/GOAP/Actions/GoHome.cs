using UnityEngine;

public class GoHome : Behavior
{
    public override bool PrePerform()
    {
        beliefs.RemoveState("atHospital");
        return true; 
    }

    public override bool PostPerform()
    {
        Destroy(this.gameObject, 1);
        return true;
    }
}