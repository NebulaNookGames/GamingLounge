using UnityEngine;

public class GetPatient : Behavior
{
    GameObject resource; 

    public override bool PrePerform()
    {
        target = World.Instance.GetQueue("patients").RemoveResource();
        if(target == null) return false;

        resource = World.Instance.GetQueue("cubicles").RemoveResource();
        if (resource != null)
            inventory.AddItem(resource);
        else
        {
            World.Instance.GetQueue("patients").AddResource(target);
            target = null;
            return false;
        }

        World.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        return true;
    }

    public override bool PostPerform()
    {
        World.Instance.GetWorld().ModifyState("Waiting", -1);
        if(target)
            target.GetComponent<Agent>().inventory.AddItem(resource);

        return true;
    }
}
