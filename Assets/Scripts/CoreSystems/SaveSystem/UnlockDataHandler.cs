using UnityEngine;

public class UnlockDataHandler : DataHandler
{
    public UnlockPanelHandler unlockPanelHandler;
    public ExpandHandler expandHandler;
    public override void ReceiveData(SaveData saveData)
    {
       unlockPanelHandler.LoadUnlocks(saveData.boughtObjects);
       expandHandler.UpdateBoughtLand(saveData.boughtLand);
    }

    public override void SendData(SaveData saveData)
    {
       saveData.boughtObjects = unlockPanelHandler.bought;
       saveData.boughtLand = expandHandler.boughtLand;
    }
}
