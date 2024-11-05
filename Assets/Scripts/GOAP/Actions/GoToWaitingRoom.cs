public class GoToWaitingRoom : Behavior
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        World.Instance.GetWorld().ModifyState("Waiting", 1);
        World.Instance.GetQueue("patients").AddResource(this.gameObject);
        beliefs.ModifyState("atHospital", 1);
        return true;
    }
}
