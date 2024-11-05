using UnityEngine;

public class UseRestroom : Behavior
{
    public override bool PrePerform()
    {
        target = World.Instance.GetQueue("toilets").RemoveResource();
        if (target == null)
        {
            Debug.Log("Returned false, no target");
            return false;
        }
        inventory.AddItem(target);
        World.Instance.GetWorld().ModifyState("FreeToilet", -1);
        foreach(GameObject i in inventory.items)
        {
            Debug.Log(i.gameObject.name);
        }
        return true; 
    }

    public override bool PostPerform()
    {
        foreach (GameObject i in inventory.items)
        {
            Debug.Log(i.gameObject.name);
        }
        World.Instance.GetQueue("toilets").AddResource(target);
        inventory.RemoveItem(target);
        World.Instance.GetWorld().ModifyState("FreeToilet", 1);
        beliefs.RemoveState("haveToPee");
        return true;
    }
}