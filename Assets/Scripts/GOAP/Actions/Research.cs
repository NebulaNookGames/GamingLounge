using UnityEngine;

public class Research : Behavior
{
    public override bool PrePerform()
    {
        target = World.Instance.GetQueue("offices").RemoveResource();
        if (target == null)
        {
            return false;
        }
        inventory.AddItem(target);
        World.Instance.GetWorld().ModifyState("FreeOffice", -1);
        return true; 
    }

    public override bool PostPerform()
    {
        World.Instance.GetQueue("offices").AddResource(target);
        inventory.RemoveItem(target);
        World.Instance.GetWorld().ModifyState("FreeOffice", 1);
        return true;
    }
}