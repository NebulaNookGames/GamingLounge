using UnityEngine;

public class Mop : Behavior
{
    public override bool PrePerform()
    {
        target = World.Instance.GetQueue("puddles").RemoveResource();
        if(target == null)
        {
            return false;
        }
        inventory.AddItem(target);
        World.Instance.GetWorld().ModifyState("FreePuddle", -1);
        return true; 
    }

    public override bool PostPerform()
    {      
        inventory.RemoveItem(target);
        Destroy(target);
        return true;
    }
}