using System;

public class MoneyDataHandler : DataHandler
{
    MoneyManager moneyManager;
    private int savedMoney;
    
    private void Start()
    {
        moneyManager = MoneyManager.instance;
    }

    public override void ReceiveData(SaveData saveData)
    {
        moneyManager.SetMoney(saveData.savedMoney);
    }

    public override void SendData(SaveData saveData)
    {
        saveData.savedMoney = MoneyManager.instance.MoneyAmount;
    }
}