using UnityEngine;

public class Rest : Behavior
{
    public override bool PrePerform()
    {
        return true; 
    }

    public override bool PostPerform()
    {
        beliefs.RemoveState("exhausted");
        return true;
    }
}