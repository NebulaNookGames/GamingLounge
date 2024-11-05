using UnityEngine;

public class GoToCubicle : Behavior
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if(target == null)
        {
            return false;
        }
        return true; 
    }

    public override bool PostPerform()
    {
        World.Instance.GetWorld().ModifyState("TreatingPatient", 1);
        World.Instance.GetQueue("cubicles").AddResource(target);
        inventory.RemoveItem(target);
        World.Instance.GetWorld().ModifyState("FreeCubicle", 1);
        return true;
    }
}